$EOTW::PowerTickRate = 20;

exec("./MachineCrafting.cs");
exec("./Brick_Generators.cs");
exec("./Brick_Capacitors.cs");
exec("./Brick_MatterReactor.cs");
exec("./Brick_Debug.cs");

function PowerMasterLoop()
{
	cancel($EOTW::PowerMasterLoop);
	
	//Cycle power generators
	if (isObject(PowerGroupSource))
	for (%i = 0; %i < PowerGroupSource.getCount(); %i++)
	{
		%brick = PowerGroupSource.getObject(%i);
		if (getSimTime() - %brick.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
		{
			%brick.lastEnergyUpdate = getSimTime();
			if(%brick.getDatablock().loopFunc !$= "")
				%brick.doCall(%brick.getDatablock().loopFunc);
		}
	}
	
	//Move power using power cables
	if (isObject(PowerGroupCablePower))
	for (%i = 0; %i < PowerGroupCablePower.getCount(); %i++)
	{
		%cable = PowerGroupCablePower.getObject(%i);
		if (getSimTime() - %cable.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
		{
			%cable.lastEnergyUpdate = getSimTime();
			%cable.doPowerTransferFull();
		}
	}

	//Randomize cable list so we get a kind of even spread of power transfer (when multiple ropes are connected)
	if (isObject(PowerGroupCablePower))
		PowerGroupCablePower.Shuffle();

	if (isObject(PowerGroupPipeMatter))
	for (%i = 0; %i < PowerGroupPipeMatter.getCount(); %i++)
	{
		%cable = PowerGroupPipeMatter.getObject(%i);
		if (getSimTime() - %cable.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
		{
			%cable.lastEnergyUpdate = getSimTime();
			%cable.doMatterTransferFull();
		}
	}
	
	//Randomize cable list so we get a kind of even spread of power transfer (when multiple ropes are connected)
	if (isObject(PowerGroupPipeMatter))
		PowerGroupPipeMatter.Shuffle();
	
	//Move matter using matter pipes
	if (isObject(PowerGroupPipeMatter))
	for (%i = 0; %i < PowerGroupPipeMatter.getCount(); %i++)
	{
		%pipe = PowerGroupPipeMatter.getObject(%i);
		if (getSimTime() - %pipe.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
		{
			%pipe.lastEnergyUpdate = getSimTime();
			%pipe.doMatterTransferFull();
		}
	}
	
	//Randomize pipe list so we get a kind of even spread of matter transfer (when multiple ropes are connected)
	if (isObject(PowerGroupCablePower))
		PowerGroupCablePower.Shuffle();
	
	//Run machines
	if (isObject(PowerGroupMachine))
	for (%i = 0; %i < PowerGroupMachine.getCount(); %i++)
	{
		%brick = PowerGroupMachine.getObject(%i);
		if (getSimTime() - %brick.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
		{
			%brick.lastEnergyUpdate = getSimTime();
			if(%brick.getDatablock().loopFunc !$= "")
				%brick.doCall(%brick.getDatablock().loopFunc);
		}
	}
	
	$EOTW::PowerMasterLoop = schedule(1000 / $EOTW::PowerTickRate, 0, "PowerMasterLoop");
}
schedule(10, 0, "PowerMasterLoop");

function SimObject::doPowerTransferFull(%obj)
{
	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		%obj.delete();
		return;
	}
	%obj.powerTransfer = mCeil(%obj.powerTransfer);
	
	if (%obj.energy > 0)
	{
		%transferAmount = getMin(%obj.energy, %obj.powerTarget.getDatablock().energyMaxBuffer - %obj.powerTarget.energy);
		%obj.powerTarget.energy += %transferAmount;
		%obj.energy -= %transferAmount;
	}
	
	if (%obj.energy <= 0)
	{
		%transferAmount = getMin(%obj.powerSource.energy, %obj.powerTransfer);
		%obj.powerSource.energy -= %transferAmount;
		%obj.energy += %transferAmount;
	}
}

function SimObject::doMatterTransferFull(%obj)
{
	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		%obj.delete();
		return;
	}
	%obj.powerTransfer = mCeil(%obj.powerTransfer);
	
	if (%obj.buffer !$= "")
	{
		%typelist = "Input" TAB "Buffer";
		for (%j = 0; %j < getFieldCount(%typelist); %j++)
		{
			%type = getField(%typelist, %j);
			%change = %obj.powerTarget.ChangeMatter(getField(%obj.buffer, 0), getField(%obj.buffer, 1), %type);
			%obj.buffer = getField(%obj.buffer, 0) TAB (getField(%obj.buffer, 1) - %change);

			if (getField(%obj.buffer, 1) <= 0)
			{
				%obj.buffer = "";
				break;
			}
		}
	}

	%data = %obj.powerSource.getDatablock();

	%typelist = "Buffer" TAB "Output";
	for (%j = 0; %j < getFieldCount(%typelist); %j++)
	{
		%type = getField(%typelist, %j);
		for (%i = 0; %i < %data.matterSlots[%type]; %i++)
		{
			%matterData = %obj.powerSource.matter[%type, %i];
			if (getField(%matterData, 0) !$= "" && (getField(%matterData, 0) $= getField(%obj.buffer, 0) || getField(%obj.buffer, 0) $= ""))
			{
				%transferAmount = getMin(%obj.powerTransfer - getField(%obj.buffer, 1), getField(%matterData, 1));
				%totalChange = %obj.powerSource.ChangeMatter(getField(%matterData, 0), %transferAmount * -1, %type);
				%obj.buffer = getField(%matterData, 0) TAB (getField(%obj.buffer, 1) - %totalChange);
				break;
			}
		}
	}
}

function fxDtsBrick::ChangePower(%obj, %change)
{
	%data = %obj.getDatablock();

	if ((%maxEnergy = %data.energyMaxBuffer) <= 0)
		return 0;

	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;

	if (%change > 0)
	{
		%totalChange = getMin(%change, %maxEnergy - %obj.energy);
		%obj.energy += %totalChange;
		return %totalChange;
	}
	else if (%change < 0)
	{
		if ((%change * -1) > %obj.energy)
			%totalChange = %obj.energy * -1;
		else
			%totalChange = %change;

		%obj.energy += %totalChange;
		return %totalChange;
	}

	return;
}

function fxDtsBrick::SetPower(%obj, %value)
{
	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;

	%obj.energy = mClamp(%value, 0, %obj.getDatablock().energyMaxBuffer);

	return %obj.energy;
}

function fxDtsBrick::GetPower(%obj)
{
	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;

	return %obj.energy;
}


package EOTWPower
{
	function fxDtsBrick::onPlant(%obj,%b)
	{
		parent::onPlant(%obj,%b);
		
		LoadPowerData(%obj);
	}

	function fxDtsBrick::onLoadPlant(%obj,%b)
	{
		parent::onLoadPlant(%obj,%b);
		
		LoadPowerData(%obj);
	}
	
	function LoadPowerData(%obj)
	{
		%db = %obj.getDatablock();
		
		if (%db.energyGroup !$= "")
		{
			if (!isObject("PowerGroup" @ %db.energyGroup))
				new SimSet("PowerGroup" @ %db.energyGroup);
				
			("PowerGroup" @ %db.energyGroup).add(%obj);
		}
	}
	
	function CreateTransferRope(%source, %sourcePortPos, %target, %targetPortPos, %rate, %material, %amt, %type)
	{
		if (!isObject(PowerGroupCablePower))
			new SimSet(PowerGroupCablePower);
		if (!isObject(PowerGroupPipeMatter))
			new SimSet(PowerGroupPipeMatter);

		%cable = new SimObject();
		
		%cable.powerSource = %source;
		%cable.powerTarget = %target;
		%cable.powerTransfer = %rate;

		%color = getColorFromHex(getMatterType(%material).color);

		switch$(%type)
		{
				case "Power":
					%diameter = 0.1;
					%slack = 0.25;
					PowerGroupCablePower.add(%cable);
				case "Matter":
					%diameter = 0.2;
					%slack = 0;
					PowerGroupPipeMatter.add(%cable);
				default:
					return;
		}
	
		%creationData = %source SPC %target SPC %color SPC %diameter SPC %slack;

		_removeRopeGroup(%creationData);

		%group = _getRopeGroup(getSimTime(), %source.getGroup().bl_id, %creationData);
		createRope(%sourcePortPos, %targetPortPos, %color, %diameter, %slack, %group);

		%group.material = %material TAB %amt;
		%group.cable = %cable;

		%source.ropeGroups = trim(%source.ropeGroups SPC %group);
		%target.ropeGroups = trim(%target.ropeGroups SPC %group);

		return %cable;
	}
};
activatePackage("EOTWPower");

function fxDtsBrick::HasMatter(%obj, %matter, %amount, %type)
{
	%data = %obj.getDatablock();
	
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matterData = %obj.matter[%type, %i];
		
		if (getField(%matterData, 0) $= %matter && getField(%matterData, 1) >= %amount)
			return true;
	}
	
	return false;
}

function fxDtsBrick::GetMatter(%obj, %matter, %type)
{
	%data = %obj.getDatablock();
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matterData = %obj.matter[%type, %i];
		
		if (getField(%matterData, 0) $= %matter)
			return getField(%matterData, 1);
	}
	
	return 0;
}

function fxDtsBrick::ChangeMatter(%obj, %matterName, %amount, %type, %ignoreUpdate)
{
	%data = %obj.getDatablock();
	
	//Check to verify if the inputted matter even exists.
	for (%i = 0; %i < MatterData.getCount(); %i++)
	{
		
		%matterType = MatterData.getObject(%i);
		if (%matterType.name $= %matterName)
			break;
		else
			%matterType = "";
	}
	
	if (%matterType $= "")
		return 0;
	
	//Try to add to existing matter.
	//We will only allow a unique matter to occupy 1 slot in its slot type.
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matter = %obj.matter[%type, %i];
		
		if (getField(%matter, 0) $= %matterName)
		{
			%testAmount = getField(%matter, 1) + %amount;
			%change = %amount;
			
			if (%testAmount > %data.matterMaxBuffer)
				%change = %data.matterMaxBuffer - getField(%matter, 1);
			else if (%testAmount < 0)
				%change = getField(%matter, 1) * -1;
			
			%obj.matter[%type, %i] = getField(%matter, 0) TAB (getField(%matter, 1) + %change);
			
			if (getField(%obj.matter[%type, %i], 1) < 1)
				%obj.matter[%type, %i] = "";
			
			if(%obj.getDatablock().matterUpdateFunc !$= "" && !%ignoreUpdate)
				%obj.doCall(%obj.getDatablock().matterUpdateFunc);
			
			return %change;
		}
	}
	
	//So we don't remove from an empty slot, somehow
	if (%amount < 0)
		return 0;
	
	//Double pass to add to an empty slot. It is only 3-4 loops at most... right?
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matter = %obj.matter[%type, %i];
		
		if (getField(%matter, 0) $= "")
		{
			%change = %amount > %data.matterMaxBuffer ? %data.matterMaxBuffer : %amount;
			%obj.matter[%type, %i] = %matterName TAB %change;
			
			if(%obj.getDatablock().matterUpdateFunc !$= "" && !%ignoreUpdate)
				%obj.doCall(%obj.getDatablock().matterUpdateFunc);
			
			return %change;
		}
	}
	
	return 0;
}

function fxDtsBrick::getPortPosition(%brick,%type,%pos)
{
	%datablock = %brick.getDatablock();
	%position = setWord(%brick.getPosition(),2,0);
	if(%datablock.portGoToEdge[%type])
	{
		//move position to the nearest edge of the brick
		%relPos = vectorNormalize(vectorSub(%pos,%position));
		%relY = getWord(%relPos,0);
		%relX = getWord(%relPos,1);
		%angle = mAcos((%relX) / (mSqrt(mPow(%relX,2) + mPow(%relY,2))));

		%fullAngle = %angle;
		if(%relY > 0)
		{
			%fullAngle = -(%angle - (2 * $pi));
		}

		%side = (%fullAngle / ($pi / 2) + 0.5) % 4;
		if(%side % 2 == 0)
		{
			%x = %datablock.brickSizeX / 4;
			if(%side == 2)
			{
				%x = -%x;
			}

			%quadAngle = %angle;
			if(%fullAngle < $PI)
			{
				%quadAngle = -%quadAngle;
				
			}
			talk(%quadAngle);
			
			
			%y = %x * mTan(%quadAngle);
		}
		else
		{
			%y = %datablock.brickSizeY / 4;
			if(%side == 1)
			{
				%y = -%y;
			}
			
			%quadAngle = %angle;
			
			%x = %y / mTan(%quadAngle);
		}
		%position = VectorAdd(%position,%y SPC %x SPC "0");
	}
	%newPosition = vectorSub(vectorAdd(%position,"0 0 " @ %datablock.portHeight[%type]),"0 0 " @ (%datalock.brickSizeZ / 10));
	return %newPosition;
}