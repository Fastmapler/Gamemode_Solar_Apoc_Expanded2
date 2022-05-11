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

//look to next 100 normal 100 nodes
$EOTW::PowerNodePathSkip = 100;

//look to next 500 transmission nodes
$EOTW::PowerNodeTransSkip = 500;

//capacitors over 60,000 EU are unskippable in path skips
$EOTW::PowerNodePathCapLimit = 60000;

if(!$EOTW::powerDebug)
	$EOTW::powerDebug = 0;

if(!$EOTW::journalPowerNonce)
	$EOTW::journalPowerNonce = 1;


function journalPower(%brick, %entry)
{
	if(!%brick.journalCounter || %brick.journalNonce != $EOTW::journalPowerNonce)
	{
		%brick.journalCounter = 0;
		%brick.journalNonce = $EOTW::journalPowerNonce;
		%brick.journalEntries.delete();
		%brick.journalEntries = new ScriptObject();
	}

	%idx = %brick.journalCounter;

	while(strlen(%idx) < 6)
		%idx = "0"@ %idx;

	%brick.journalEntries._[%idx] = getSimTime() @"   "@ %entry;
	%brick.journalCounter++;
}

function PowerMasterLoop()
{
	cancel($EOTW::PowerMasterLoop);
	
	//Cycle power generators
	if (isObject(PowerGroupSource))
		for (%i = 0; %i < PowerGroupSource.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupSource.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);


	//TODO: remove cable processing: no more energy on cables
	//TODO: currently just does isObject checks on source and target, then deletes self if there's a problem
	//Randomize cable list so we get a kind of even spread of power transfer (when multiple ropes are connected)
	//if (getRandom() < 0.02 && isObject(PowerGroupCablePower))
	//	PowerGroupCablePower.Shuffle();

	//if (isObject(PowerGroupCablePower))
	//	for (%i = 0; %i < PowerGroupCablePower.getCount(); %i += $EOTW::ObjectsPerLoop)
	//		PowerGroupCablePower.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);
	
	//Randomize cable list so we get a kind of even spread of power transfer (when multiple ropes are connected)
	//if(getRandom() < 0.02 && isObject(PowerGroupPipeMatter))
	//	PowerGroupCablePower.Shuffle();
	
	//Move matter using matter pipes
	if(isObject(PowerGroupPipeMatter))
		for (%i = 0; %i < PowerGroupPipeMatter.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupPipeMatter.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);
	
	//Randomize pipe list so we get a kind of even spread of matter transfer (when multiple ropes are connected)
	if(getRandom() < 0.02 && isObject(PowerGroupPipeMatter))
		PowerGroupPipeMatter.Shuffle();

	//Run storage/sorting devices
	//if(isObject(PowerGroupStorage))
	//	for (%i = 0; %i < PowerGroupStorage.getCount(); %i += $EOTW::ObjectsPerLoop)
	//		PowerGroupStorage.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);

	//if(getRandom() < 0.02 && isObject(PowerGroupStorage))
	//	PowerGroupStorage.Shuffle();


	//1. Remove outbound power storage objects
	if(isObject(%g = PowerGroupDirtyExitStorage))
	{
		for(%i = 0; %i < %g.getCount(); %i++)
			if(PowerGroupDirtyStorage.isMember(%o = %g.getObject(%i)))
			{
				if($EOTW::powerDebug && strpos(%o.getName(), "_journal") == 0)
					journalPower(%o, "REALEXIT");
				PowerGroupDirtyStorage.remove(%o);
			}
		%g.clear();
	}

	//2. Add inbound
	if(isObject(%g = PowerGroupDirtyEnterStorage))
	{
		for(%i = 0; %i < %g.getCount(); %i++)
			if(!PowerGroupDirtyStorage.isMember(%o = %g.getObject(%i)))
			{
				if($EOTW::powerDebug && strpos(%o.getName(), "_journal") == 0)
					journalPower(%o, "REALENTER");
				PowerGroupDirtyStorage.add(%o);
			}
		%g.clear();
	}

	//3. Run dirty storage/sorting devices
	if(isObject(PowerGroupDirtyStorage))
		for (%i = 0; %i < PowerGroupDirtyStorage.getCount(); %i += $EOTW::ObjectsPerLoop)
		{
			//talk("Processed "@ $EOTW::ObjectsPerLoop @" objects for PowerGroupDirtyStorage");
			PowerGroupDirtyStorage.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);
		}

	if(getRandom() < 0.02 && isObject(PowerGroupDirtyStorage))
		PowerGroupDirtyStorage.Shuffle();

	//Run machines
	//TODO: only hungry machines shall be allowed
	if (isObject(PowerGroupMachine))
		for (%i = 0; %i < PowerGroupMachine.getCount(); %i += $EOTW::ObjectsPerLoop)
			PowerGroupMachine.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);

	if (getRandom() < 0.02 && isObject(PowerGroupMachine))
		PowerGroupMachine.Shuffle();

	$EOTW::PowerMasterLoop = schedule(1000 / $EOTW::PowerTickRate, 0, "PowerMasterLoop");
}

$EOTW::PowerMasterLoop = schedule(10, 0, "PowerMasterLoop");

function SimSet::IterateLoopCalled(%obj, %start, %length)
{
	for (%i = %start; (%i < %start + %length) && %i < %obj.getCount(); %i++)
	{
		%item = %obj.getObject(%i);

		if(%item.energy $= "" || %item.energy < 0)
			%item.energy = 0;

		if(getSimTime() - %item.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
		{
			%item.lastEnergyUpdate = getSimTime();
			if (%item.getClassName() $= "SimObject")
			{
				switch$ (%item.transferType)
				{
					case "Power":  %item.doPowerTransferFull();
					case "Matter": %item.doMatterTransferFull();
				}
			}

			else if(%item.getDatablock().loopFunc !$= "" && !%item.machineDisabled)
				%item.doCall(%item.getDatablock().loopFunc);


			//if there is energy stored here - try to move it
			//special rules for solar panel - dont release under 30 (except after sunset)
			//or if last energy release time was > 3 seconds ago
			if(%item.getDatablock().uiName $= "Solar Panel")
			{
				//TODO: allow release of < 30 energy at 100% decay (whenever it's implemented...)
				if(%item.energy >= 30 || $EOTW::Time >= 12)
					%item.tryPowerTransfer();
			}

			else if(%item.energy > 0)
			{
				%item.tryPowerTransfer();
			}

			//Is it in Dirty Storage, where it might need to be removed?
			if(PowerGroupDirtyStorage.isMember(%item))
			{
				if($EOTW::powerDebug && strpos(%item.getName(), "_journal") == 0)
					journalPower(%item, "SimSet::IterateLoopCalled detected as member of DirtyStorage. need removal?");

				//nowhere to move energy, or I'm empty and can't forward anthing more
				if(%item.cableOutputs.getCount() == 0 || %item.energy <= 0)
				{
					if($EOTW::powerDebug && strpos(%item.getName(), "_journal") == 0)
					{
						if(%item.cableOutputs.getCount() == 0)
							journalPower(%item, "EXIT: SimSet::IterateLoopCalled no outputs");
						else
							journalPower(%item, "EXIT: SimSet::IterateLoopCalled energy <= 0");
					}

					if(PowerGroupDirtyEnterStorage.isMember(%item))
						PowerGroupDirtyEnterStorage.remove(%item);

					PowerGroupDirtyExitStorage.add(%item);
				}

				//but I may need to wake up inputs, only if i have room

				//I'm full; i can go to sleep if all of my outputs are full
				if(%item.getDatablock().energyMaxBuffer - %item.energy <= 0)
				{
					if($EOTW::powerDebug && strpos(%item.getName(), "_journal") == 0)
						journalPower(%item, "SimSet::IterateLoopCalled detected full. what about outputs?");

					%cant_forward = 1;
					if(isObject(%g=%item.cableOutputs))
						for(%i = 0; %i < %g.getCount(); %i++)
						{
							%dst = %g.getObject(%i).powerTarget;
							%dst_free = %dst.getDatablock().energyMaxBuffer - %dst.energy;
							if(%dst_free > 0)
							{
								%cant_forward = 0;
								break;
							}
						}

					if(%cant_forward)
					{
						if($EOTW::powerDebug)
						{
							if(strpos(%item.getName(), "_journal") == 0)
								journalPower(%item, "EXIT: SimSet::IterateLoopCalled [set brown] no non-full outputs - energy on elem0 is: "@ %g.getObject(0).powerTarget.energy);

							cancel(%item.colorSched);
							%item.setColor(8);
						}

						if(PowerGroupDirtyEnterStorage.isMember(%item))
							PowerGroupDirtyEnterStorage.remove(%item);

						PowerGroupDirtyExitStorage.add(%item);
					}
				}

				//i have some room, so wake up non-empty input objects
				else if(isObject(%g=%item.cableInputs))
					for(%i = 0; %i < %g.getCount(); %i++)
					{
						%src = %g.getObject(%i).powerSource;
						if(%src.energy > 0 && %src.getDatablock().energyGroup $= "Storage")
						{
							if($EOTW::powerDebug && strpos(%item.getName(), "_journal") == 0)
								journalPower(%item, "SimSet::IterateLoopCalled Waking up input due to loss of energy: "@ %src.getName());

							if($EOTW::powerDebug && strpos(%src.getName(), "_journal") == 0)
								journalPower(%src, "ENTER: SimSet::IterateLoopCalled output "@ %item.getName() @" now has room and i'm not empty");

							PowerGroupDirtyEnterStorage.add(%src);
						}
					}
			}
		}
	}
}

function SimObject::doPowerTransferFull(%obj)
{
	//Should be no more on this code

	if (!isObject(%obj.powerSource) || !isObject(%obj.powerTarget) || %obj.powerTransfer <= 0)
	{
		if (isObject(%obj.parent))
			%obj.parent.RemoveCableData();
		else
			%obj.delete();
			
		return;
	}

	return;
	//TODO: OMIT BELOW...

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

function fxDtsBrick::ChangePower(%obj, %change, %force_dirty)
{
	%data = %obj.getDatablock();

	if(%force_dirty && %obj.getDatablock().energyGroup $= "Storage")
	{
		if($EOTW::powerDebug && strpos(%obj.getName(), "_journal") == 0)
			journalPower(%obj, "ENTER: forced dirty");
		PowerGroupDirtyEnterStorage.add(%obj);
	}

	//safeguard for 0-capacity storage bricks
	if((%maxEnergy = %data.energyMaxBuffer) <= 0)
	{
		if(%obj.getDatablock().energyGroup $= "Storage" && PowerGroupDirtyStorage.isMember(%obj))
		{
			if($EOTW::powerDebug && strpos(%obj.getName(), "_journal") == 0)
				journalPower(%obj, "EXIT: safeguard for 0-energy nodes");

			if(PowerGroupDirtyEnterStorage.isMember(%obj))
				PowerGroupDirtyEnterStorage.remove(%obj);

			PowerGroupDirtyExitStorage.add(%obj);
		}

		return 0;
	}


	if(%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;

	if(%change > 0)
	{
		%totalChange = getMin(%change, %maxEnergy - %obj.energy);
		%obj.energy += %totalChange;
	}

	else if(%change < 0)
	{
		if(-%change > %obj.energy)
			%totalChange = -%obj.energy;
		else
			%totalChange = %change;

		%obj.energy += %totalChange;
	}

	if(%totalChange != 0)
	{
		if($EOTW::powerDebug)
		{
			if(strpos(%obj.getName(), "_journal") == 0)
				journalPower(%obj, "fxDtsBrick::ChangePower [set red, 1 sec] total change: "@ %totalChange @" now: "@ %obj.energy);

			cancel(%obj.colorSched);
			%obj.setColor(0);
			%obj.colorSched = %obj.schedule(1000, setColor, 6);
		}
	}

	return %totalChange;
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

	return %obj.energy = mClamp(%value, 0, %obj.getDatablock().energyMaxBuffer);
}

function fxDtsBrick::GetPower(%obj)
{
	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;

	return %obj.energy;
}

function fxDTSBrick::tryPowerTransfer(%obj)
{
	//TODO: create a transmission tag when iterating through objects
	//(if all in a row are transmission)

	//subsequent transmission noeds have higher limits (500?)

	//can't transfer power as there are no outputs
	if(!isObject(%obj.cableOutputs) || %obj.cableOutputs.getCount() == 0)
	{
		if($EOTW::powerDebug && strpos(%obj.getName(), "_journal") == 0)
			journalPower(%obj, "fxDTSBrick::tryPowerTransfer power transfer failed: no outputs");
		return 0;
	}

	//Shuffle outputs
	%obj.cableOutputs.shuffle();

	//First condition: select a random output that isn't full
	//TODO: don't choose a wattage-limited cable!
	for(%i = 0; %i < %obj.cableOutputs.getCount(); %i++)
	{
		%destCable = %obj.cableOutputs.getObject(%i);
		%dest = %destCable.powerTarget;
		%dest_free = %dest.getDatablock().energyMaxBuffer - %dest.getPower();
		if(%dest_free > 0)
			break;
	}

	if(!isObject(%dest))
		return 0;

	%nextObj = %dest;
	%nextCable = %destCable;

	//Search for next brick

	%chain = new SimSet();
	%chain.schedule(0, delete);
	%cableChain = new SimSet();
	%cableChain.schedule(0, delete);

	%i2 = 0; //limit normal nodes
	%last_was_transmission = 0;
	for(%i = 0; %i < $EOTW::PowerNodeTransSkip; %i++)
	{
		%detected_transmission = 0;

		//check if cable hit the limit in last 1 sec
		%cable_limit_expired = getSimTime() - %nextCable.periodStart >= 1000;

		//if the period is still valid and the transfered energy exceeds limit
		if(!%cable_limit_expired && %nextCable.periodTransfered >= %nextCable.periodLimit)
				return;

		//reset the wattage timer if period expired
		if(%cable_limit_expired)
		{
			%nextCable.periodLimit = %nextCable.powerTransfer * $EOTW::PowerTickRate;
			
			//if within last 2 sec, simply move period ahead 1 sec
			%cable_limit_next_expired = getSimTime() - %nextCable.periodStart >= 2000;

			if(%cable_limit_next_expired)
				%nextCable.periodStart = getSimTime();

			else
				%nextCable.periodStart += 1000;

			%nextCable.periodTransfered = 0;
		}

		//if object is in set: set it to yellow and quit
		if(%chain.isMember(%nextObj))
		{
			if($EOTW::powerDebug)
			{
				cancel(%nextObj.colorSched);
				%nextObj.setColor(1);
			}
			break;
		}

		if(%nextObj.getDatablock().energyGroup $= "Transmission")
		{
			%nextObj.transmission_error = "";
			%detected_transmission = 1;
			%last_was_transmission = 1;

			if(!isObject(%first_transmission))
				%first_transmission = %nextObj;

			if(strpos(%nextObj.getName(), "_journal") == 0)
				journalPower(%nextObj, "fxDTSBrick::tryPowerTransfer located transmission");
		}

		//object is full, set to brown and kill loop (but not transmission nodes)
		else if(%nextObj.getPower() >= %nextObj.getDatablock().energyMaxBuffer)
		{
			if($EOTW::powerDebug)
			{
				if(strpos(%nextObj.getName(), "_journal") == 0)
					journalPower(%nextObj, "fxDTSBrick::tryPowerTransfer [set brown] previous brick "@ %obj.getName() @" pathed and found me full");

				cancel(%nextObj.colorSched);
				%nextObj.setColor(8);
			}
			break;
		}

		%chain.add(%nextObj);
		%cableChain.add(%nextCable);

		//Large capacitors are automatic break points
		if(%nextObj.getDatablock().energyMaxBuffer > $EOTW::PowerNodePathCapLimit)
			break;

		//If just ended a transmission line, this is a break point
		if(%last_was_transmission && !%detected_transmission)
				break;

		if(!isObject(%nextObj.cableOutputs) || %nextObj.cableOutputs.getCount() != 1)
			break;


		if(!%detected_transmission)
			%i2++;



		if(%i2 > $EOTW::PowerNodePathSkip)
			break;

		%nextCable = %nextObj.cableOutputs.getObject(0);
		%nextObj = %nextCable.powerTarget;
	}

	//TODO: cannot disambiguate between
	//if last object is a transmission node - big oops!
	if((%c=%chain.getCount()) > 0 && (%o=%chain.getObject(%c-1)).getDatablock().energyGroup $= "Transmission")
		%first_transmission.transmission_error="\c4-- Transmission line unfinished or too long --";

	//%rope.setNodeColor("ALL", getColorIDTable(%color));

	//%obj.testList = "";
	//for(%i = 0; %i < %chain.getCount(); %i++)
	//	%obj.testList = %obj.testList SPC %chain.getObject(%i).getName();

	%energy_to_move = %obj.energy;
	%energy_moved = 0;
	for(%i = %chain.getCount() - 1; %i >= 0; %i--)
	{
		if(%energy_to_move < 1)
			break;

		%dest = %chain.getObject(%i);

		//TODO: do i need the dest_free check with the changePower below?
		%dest_free = %dest.getDatablock().energyMaxBuffer - %dest.getPower();

		//is %dest full?
		if(%dest_free <= 0)
			continue;

		//Cable wattage - determine limits
		%cable_limited = 0;
		%max_transmissible = 999999;
		for(%j = 0; %j < %cableChain.getCount(); %j++)
		{
			%cable = %cableChain.getObject(%j);

			//period should already be in scope

			if((%test_limit = %cable.periodLimit - %cable.periodTransfered) < %max_transmissible)
				%max_transmissible = %test_limit;
		}

		if(%max_transmissible < %energy_to_move)
			%energy_to_move = %max_transmissible;

		if($EOTW::powerDebug && strpos(%obj.getName(), "_journal") == 0)
			journalPower(%obj, "Trying to change power: "@ %energy_to_move @" to "@ %dest.getName());

		//Try to change power and ignore transfer attempt
		%energy_transfered = %dest.ChangePower(%energy_to_move, 1);
		%obj.changePower(-%energy_transfered, 0);

		if($EOTW::powerDebug && strpos(%obj.getName(), "_journal") == 0)
			journalPower(%obj, "Actual change power: "@ %energy_transfered @" to "@ %dest.getName());

		//Cable wattage - mark power transfered
		for(%j = 0; %j < %cableChain.getCount(); %j++)
		{
			%cable = %cableChain.getObject(%j);
			%cable.periodTransfered += %energy_transfered;
		}

		if(%dest.getDatablock().energyGroup $= "Storage" && %energy_transfered > 0)
		{
			if($EOTW::powerDebug && strpos(%obj.getName(), "_journal") == 0)
				journalPower(%obj, "Setting to dirty due to non-zero transfer: "@ %dest.getName());

			PowerGroupDirtyEnterStorage.add(%dest);
		}

		%energy_to_move -= %energy_transfered;
		%energy_moved += %energy_transfered;
	}

	%chain.delete();
	%cableChain.delete();

	//set all non-empty inputs to dirty only if energy has moved on this current object
	if(isObject(%obj.cableInputs) && %energy_moved > 0)
		for(%i = 0; %i < %obj.cableInputs.getCount(); %i++)
		{
			%src = %obj.cableInputs.getObject(%i).powerSource;
			if(%src.energy > 0 && %src.getDatablock().energyGroup $= "Storage")
			{
				if($EOTW::powerDebug && strpos(%obj.getName(), "_journal") == 0)
					journalPower(%obj, "fxDTSBrick::tryPowerTransfer Waking up input due to loss of energy: "@ %src.getName());

				if($EOTW::powerDebug && strpos(%src.getName(), "_journal") == 0)
					journalPower(%src, "ENTER: fxDTSBrick::tryPowerTransfer from "@ %obj.getName() @" now has room and i'm not empty");

				PowerGroupDirtyEnterStorage.add(%src);
			}
		}
}

function LoadPowerData(%obj)
{
	%db = %obj.getDatablock();
	
	if (%db.energyGroup !$= "")
	{
		if(!isObject("PowerGroup" @ %db.energyGroup))
			new SimSet("PowerGroup" @ %db.energyGroup);

		if(%db.energyGroup $= "Storage")
		{
			if(!isObject(%g=PowerGroupDirtyStorage)) new SimSet(%g);
			if(!isObject(%g=PowerGroupDirtyEnterStorage)) new SimSet(%g);
			if(!isObject(%g=PowerGroupDirtyExitStorage)) new SimSet(%g);
			PowerGroupDirtyEnterStorage.add(%obj);
		}

		//TODO: generic PowerGroupDirty

		("PowerGroup"@ %db.energyGroup).add(%obj);

		if(!isObject(%obj.cableInputs))  %obj.cableInputs = new SimSet();
		if(!isObject(%obj.cableOutputs)) %obj.cableOutputs = new SimSet();
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

	function fxDtsBrick::onRemove(%brick)
	{
		//might already be properly covered by ropeClearAll() in RopePackage
		if(isObject(%o = %brick.cableInputs))
		{
			//Delete all cable objects
			%o.deleteAll();
			%o.schedule(0, delete);
		}

		if(isObject(%o = %brick.cableOutputs))
		{
			%o.deleteAll();
			%o.schedule(0, delete);
		}

		Parent::onRemove(%brick);
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

		%target.addCableInput(%cable);
		%source.addCableOutput(%cable);

		//transferType determines Power or Matter

		//TODO: REMOVE ropeGroups (simsets cableInputs and cableOutputs prefered)
		%source.ropeGroups = trim(%source.ropeGroups SPC %group);
		%target.ropeGroups = trim(%target.ropeGroups SPC %group);


		if(%source.getDatablock().energyGroup $= "Storage")
		{
			PowerGroupDirtyEnterStorage.add(%source);
		}

		if(%target.getDatablock().energyGroup $= "Storage")
		{
			PowerGroupDirtyEnterStorage.add(%target);
		}

		return %cable;
	}
};
activatePackage("EOTWPower");

function fxDTSBrick::addCableInput(%this, %cable)
{
	if(!isObject(%group = %this.cableInputs))
		%group = %this.cableInputs = new SimSet();

	%group.add(%cable);
}

function fxDTSBrick::addCableOutput(%this, %cable)
{
	if(!isObject(%group = %this.cableOutputs))
		%group = %this.cableOutputs = new SimSet();

	%group.add(%cable);
}

function fxDTSBrick::removeCableInput(%this, %cable)
{
	if(!isObject(%group = %this.cableInputs))
		return;

	%group.remove(%cable);
}

function fxDTSBrick::removeCableOutput(%this, %cable)
{
	if(!isObject(%group = %this.cableOutputs))
		return;

	%group.remove(%cable);
}

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
