exec("./MachineCrafting.cs");
exec("./Brick_Generators.cs");
exec("./Brick_Storage.cs");
exec("./Brick_Logistics.cs");
exec("./Brick_MatterReactor.cs");
exec("./Brick_WaterWorks.cs");
exec("./Brick_Military.cs");
exec("./Brick_Support.cs");

$EOTW::PowerTickRate = 20;
$EOTW::ObjectsPerLoop = 200;

function PowerMasterLoop()
{
	cancel($EOTW::PowerMasterLoop);
	
	//Cycle power generators
	if (isObject(PowerGroupSource))
		for (%i = 0; %i < PowerGroupSource.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupSource.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);

	//Randomize cable list so we get a kind of even spread of power transfer (when multiple ropes are connected)
	if (getRandom() < 0.02 && isObject(PowerGroupCablePower))
		PowerGroupCablePower.Shuffle();

	if (isObject(PowerGroupCablePower))
		for (%i = 0; %i < PowerGroupCablePower.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupCablePower.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);
	
	//Randomize cable list so we get a kind of even spread of power transfer (when multiple ropes are connected)
	if (getRandom() < 0.02 && isObject(PowerGroupPipeMatter))
		PowerGroupPipeMatter.Shuffle();
	
	//Move matter using matter pipes
	if (isObject(PowerGroupPipeMatter))
		for (%i = 0; %i < PowerGroupPipeMatter.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupPipeMatter.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);
	
	//Randomize pipe list so we get a kind of even spread of matter transfer (when multiple ropes are connected)
	if (getRandom() < 0.02 && isObject(PowerGroupCablePower))
		PowerGroupCablePower.Shuffle();
	
	//Run storage/sorting devices
	if (isObject(PowerGroupStorage))
		for (%i = 0; %i < PowerGroupStorage.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupStorage.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);

	if (getRandom() < 0.02 && isObject(PowerGroupStorage))
		PowerGroupStorage.Shuffle();

	//Run machines
	if (isObject(PowerGroupMachine))
		for (%i = 0; %i < PowerGroupMachine.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupMachine.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);

	if (getRandom() < 0.02 && isObject(PowerGroupMachine))
		PowerGroupMachine.Shuffle();
	
	$EOTW::PowerMasterLoop = schedule(1000 / $EOTW::PowerTickRate, 0, "PowerMasterLoop");
}
schedule(10, 0, "PowerMasterLoop");

function SimSet::IterateLoopCalled(%obj, %start, %length)
{
	for (%i = %start; (%i < %start + %length) && %i < %obj.getCount(); %i++)
	{
		%brick = %obj.getObject(%i);
		if (getSimTime() - %brick.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
		{
			%brick.lastEnergyUpdate = getSimTime();
			if (%brick.getClassName() $= "SimObject")
			{
				switch$ (%brick.transferType)
				{
					case "Power": %brick.doPowerTransferFull();
					case "Matter":%brick.doMatterTransferFull();
				}
			}
			else if(%brick.getDatablock().loopFunc !$= "" && !%brick.machineDisabled)
				%brick.doCall(%brick.getDatablock().loopFunc);
		}
	}
}

function SimObject::doPowerTransferFull(%obj)
{
	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		if (isObject(%obj.parent))
			%obj.parent.RemoveCableData();
		else
			%obj.delete();
			
		return;
	}
	%obj.powerTransfer = mCeil(%obj.powerTransfer);
	
	if (%obj.buffer > 0)
	{
		%transferAmount = getMin(%obj.buffer, %obj.powerTarget.getDatablock().energyMaxBuffer - %obj.powerTarget.energy);
		%obj.powerTarget.changePower(%transferAmount);
		%obj.buffer += (%transferAmount * -1);
	}
	
	if (%obj.buffer <= 0)
	{
		%transferAmount = getMin(%obj.powerSource.energy, %obj.powerTransfer);
		%obj.powerSource.changePower(%transferAmount * -1);
		%obj.buffer += (%transferAmount);
	}
}

function SimObject::doMatterTransferFull(%obj)
{
	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		%obj.parent.RemoveCableData();
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
		if (%change * -1 > %obj.energy)
			%totalChange = %obj.energy * -1;
		else
			%totalChange = %change;

		%obj.energy += %totalChange;
		return %totalChange;
	}

	return;
}

function fxDtsBrick::ChangePower_uInt(%obj, %change)
{
	%data = %obj.getDatablock();

	if ((%maxEnergy = %data.energyMaxBuffer) <= 0)
		return 0;

	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;

	if (%change > 0)
	{
		%totalChange = getMin(%change, uint_sub(%maxEnergy, %obj.energy));
		%obj.energy = uint_add(%obj.energy, %totalChange);
		return %totalChange;
	}
	else if (%change < 0)
	{
		if (uint_mul(%change, -1) > %obj.energy)
			%totalChange = uint_mul(%obj.energy, -1);
		else
			%totalChange = %change;

		%obj.energy = uint_add(%obj.energy, %totalChange);
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
	
	function CreateTransferRope(%source, %sourcePortPos, %target, %targetPortPos, %rate, %material, %amt, %type, %simTimeOffset)
	{
		if (!isObject(PowerGroupCablePower))
			new SimSet(PowerGroupCablePower);
		if (!isObject(PowerGroupPipeMatter))
			new SimSet(PowerGroupPipeMatter);

		%simTimeOffset = %simTimeOffset + 0;

		%cable = new SimObject();
		
		%cable.powerSource = %source;
		%cable.powerTarget = %target;
		%cable.powerTransfer = %rate;

		%cable.powerSourcePort = %sourcePortPos;
		%cable.powerTargetPort = %targetPortPos;
		%cable.transferType = %type;

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

		%group = _getRopeGroup(getSimTime() + %simTimeOffset, %source.getGroup().bl_id, %creationData);
		createRope(%sourcePortPos, %targetPortPos, %color, %diameter, %slack, %group);

		%group.material = %material TAB %amt;
		%group.cable = %cable;
		%cable.parent = %group;

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

function fxDTSbrick::SetMachinePowered(%brick,%mode)
{
	switch (%mode)
	{
		case 0: %brick.machineDisabled = true;
		case 1: %brick.machineDisabled = false;
		case 2: %brick.machineDisabled = !%brick.machineDisabled;
	}

	if (%brick.machineDisabled && isObject(%brick.getDatablock().loopNoise))
		%brick.playSoundLooping();
}
registerOutputEvent(fxDTSbrick, "SetMachinePowered", "list Off 0 On 1 Toggle 2", 0);


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
			
			if ((getField(%matter, 1) + %change) <= 0)
				%obj.matter[%type, %i] = "";
			
			if(%obj.getDatablock().matterUpdateFunc !$= "" && !%ignoreUpdate)
				%obj.doCall(%obj.getDatablock().matterUpdateFunc);
			
			return %change;
		}
	}
	
	//So we don't remove from an empty slot, somehow
	if (%amount <= 0)
		return 0;
	
	//Double pass to add to an empty slot.
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
	%position = vectorSub(%brick.getPosition(), "0 0 " @ %datablock.brickSizeZ / 10);
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
	%newPosition = vectorAdd(%position,"0 0 " @ %datablock.portHeight[%type]);
	return %newPosition;
}

function fxDTSBrick::playSoundLooping(%obj, %data)
{
	if (isObject(%obj.AudioEmitter))
		%obj.AudioEmitter.delete();

	%obj.AudioEmitter = 0;
	if (!isObject(%data) || %data.getClassName () !$= "AudioProfile" || !%data.getDescription().isLooping || %data.fileName $= "")
		return;

	%audioEmitter = new AudioEmitter("")
	{
		profile = %data;
		useProfileDescription = 1;
	};
	MissionCleanup.add(%audioEmitter);
	%obj.AudioEmitter = %audioEmitter;
	%audioEmitter.setTransform(%obj.getTransform());
}