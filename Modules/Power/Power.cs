$EOTW::PowerTickRate = 20;

exec("./MachineCrafting.cs");
exec("./Brick_Debug.cs");
exec("./Brick_Capacitors.cs");
exec("./Brick_MatterReactor.cs");

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

function SimObject::doPowerTransferBuffer(%obj)
{
	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		%obj.delete();
		return;
	}
		
		
	%obj.transferAmount = getMin(%obj.powerSource.energy, %obj.powerTransfer);
	%obj.transferAmount = getMin(%obj.transferAmount, %obj.powerTarget.getDatablock().energyMaxBuffer);
}

function SimObject::doPowerTransfer(%obj)
{
	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		%obj.delete();
		return;
	}
	
	%obj.transferAmount = getMin(%obj.transferAmount, %obj.powerTarget.getDatablock().energyMaxBuffer - %obj.powerTarget.energy);
	%obj.powerSource.energy -= %obj.transferAmount;
	%obj.powerTarget.energy += %obj.transferAmount;
	%obj.transferAmount = 0;
}

function SimObject::doPowerTransferFull(%obj)
{
	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		%obj.delete();
		return;
	}
	
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
	
	function CreateTransferRope(%source, %target, %rate, %material, %amt, %type)
	{
		%cable = new SimObject();
		
		%cable.powerSource = %source;
		%cable.powerTarget = %target;
		%cable.powerTransfer = %rate;
		
		%color = getColorFromHex(getMatterType(%material).color);
		%diameter = 0.1;
		%slack = getRandom(25, 50) * 0.01;
		
		%creationData = %source SPC %target SPC %color SPC %diameter SPC %slack;

		_removeRopeGroup(%creationData);

		%group = _getRopeGroup(getSimTime(), %source.getGroup().bl_id, %creationData);

		createRope(%source.getPosition(), %target.getPosition(), %color, %diameter, %slack, %group);
		
		%group.material = %material TAB %amt;
		%group.cable = %cable;

		%source.ropeGroups = trim(%source.ropeGroups SPC %group);
		%target.ropeGroups = trim(%target.ropeGroups SPC %group);
		
		if (!isObject(PowerGroupCablePower))
			new SimSet(PowerGroupCablePower);
				
		PowerGroupCablePower.add(%cable);
	}
};
activatePackage("EOTWPower");

function fxDtsBrick::HasMatter(%obj, %matter, %amount, %type)
{
	%data = %obj.getDatablock();
	
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matterData = %obj.matter[%type, %i];
		
		if (getWord(%matterData, 0) $= %matter && getWord(%matterData, 1) >= %amount)
			return true;
	}
	
	return false;
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
		
		if (getWord(%matter, 0) $= %matterName)
		{
			%testAmount = getWord(%matter, 1) + %amount;
			%change = %amount;
			
			if (%testAmount > %data.matterMaxBuffer)
				%change = %data.matterMaxBuffer - getWord(%matter, 1);
			else if (%testAmount < 0)
				%change = getWord(%matter, 1) * -1;
			
			%obj.matter[%type, %i] = getWord(%matter, 0) TAB (getWord(%matter, 1) + %change);
			
			if (getWord(%obj.matter[%type, %i], 1) <= 0)
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
		
		if (getWord(%matter, 0) $= "")
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