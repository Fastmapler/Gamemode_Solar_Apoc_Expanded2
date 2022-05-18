exec("./MachineCrafting.cs");
exec("./Brick_Generators.cs");
exec("./Brick_Storage.cs");
exec("./Brick_Logistics.cs");
exec("./Brick_MatterReactor.cs");
exec("./Brick_WaterWorks.cs");
exec("./Brick_Military.cs");
exec("./Brick_Support.cs");

if(!$EOTW::bricksDirty)
	$EOTW::bricksDirty = 0;

$EOTW::PowerTickRate = 10;
$EOTW::ObjectsPerLoop = 1; //temp 1 obj (not used, only in the cut code)

//look to next 100 normal power nodes
$EOTW::PowerNodePathSkip = 100;

//look to next 500 transmission nodes
$EOTW::PowerNodeTransSkip = 500;

//TODO: power link search-ahead, once it sees a >60k storage object,
//needs to continue until it encounters a <60k storage node
//(AKA allow power to traverse entire capacitor banks but stop at the end)
//capacitors over 60,000 EU are unskippable in path skips
$EOTW::PowerNodePathCapLimit = 60000;

if(!$EOTW::powerDebug)
	$EOTW::powerDebug = 0;

if(!$EOTW::journalPowerNonce)
	$EOTW::journalPowerNonce = 1;

//if blank, set it to zero
if($EOTW::PT_debug $= "" || !isFunction("ptimer_start"))
	$EOTW::PT_debug = 0;

%g = PowerMasterLoopGroup; if(!isObject(%g)) new SimSet(%g);
%g = PowerGroupTransmission; if(!isObject(%g)) new SimSet(%g);
%g = PowerGroupSource; if(!isObject(%g)) new SimSet(%g);
%g = PowerGroupPipeMatter; if(!isObject(%g)) new SimSet(%g);

%g = PowerGroupStorage; if(!isObject(%g)) new SimSet(%g);
%g = PowerGroupDirtyExitStorage; if(!isObject(%g)) new SimSet(%g);
%g = PowerGroupDirtyEnterStorage; if(!isObject(%g)) new SimSet(%g);
%g = PowerGroupDirtyStorage; if(!isObject(%g)) new SimSet(%g);

%g = PowerGroupMachine; if(!isObject(%g)) new SimSet(%g);

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
	$EOTW::POWER_SIMTIME_BLOCKED = 0;
	$EOTW::ITERATE_DIFF = getSimTime() - $EOTW::LAST_POWER_ITERATE;

	if($EOTW::PT_debug)
	{
		$EOTW::POWERLOOP_ALL_TIMED_FINAL = $EOTW::POWERLOOP_ALL_TIMED / 1000;
		$EOTW::POWERLOOP_ALL_TIMED = 0;
		$EOTW::POWERLOOP_TIMED = 0;
		ptimer_start("PowerMaster");
		ptimer_start("PowerMaster_p1");
	}

	PowerMasterLoopGroup.clear();

	%shuffle_groups = getRandom() < 0.02;

	//Cycle power generators
	%g=PowerGroupSource;
	%g.POWER_SIMTIME_BLOCKED = 0;
	for(%i = 0; %i < %g.getCount(); %i++)
	{
		if($BLOCKALLSource || $BLOCKALL) continue; //temp
		%obj = %g.getObject(%i);
		%obj.POWER_LAST++;
		if(%obj.power_simtime_block > getSimTime())
		{
			$EOTW::POWER_SIMTIME_BLOCKED++;
			%g.POWER_SIMTIME_BLOCKED++;
			%obj.POWER_SIMTIME_BLOCKED++;
			continue;
		}

		PowerMasterLoopGroup.add(%obj);
	}

	//Randomize power group sources
	if(%shuffle_groups)
		%g.Shuffle();

	//Move matter using matter pipes
	%g=PowerGroupPipeMatter;
	%g.POWER_SIMTIME_BLOCKED = 0;
	for(%i = 0; %i < %g.getCount(); %i++)
	{
		if($BLOCKALLPipeMatter || $BLOCKALL) continue; //temp
		%obj = %g.getObject(%i);
		%obj.POWER_LAST++;
		if(%obj.power_simtime_block > getSimTime())
		{
			$EOTW::POWER_SIMTIME_BLOCKED++;
			%g.POWER_SIMTIME_BLOCKED++;
			%obj.POWER_SIMTIME_BLOCKED++;
			continue;
		}

		PowerMasterLoopGroup.add(%obj);
	}

	//Randomize pipe list so we get a kind of even spread of matter transfer (when multiple ropes are connected)
	if(%shuffle_groups)
		%g.Shuffle();


	//TODO: improve step1-2-3 storage code (can't omit #3 loop??)

	//1. Remove outbound power storage objects
	%g = PowerGroupDirtyExitStorage;
	for(%i = 0; %i < %g.getCount(); %i++)
	{
		if(PowerGroupDirtyStorage.isMember(%o = %g.getObject(%i)))
		{
			if($EOTW::powerDebug && strpos(%o.getName(), "_journal") == 0)
				journalPower(%o, "REALEXIT");

			PowerGroupDirtyStorage.remove(%o);

			if(PowerMasterLoopGroup.isMember(%o))
				PowerMasterLoopGroup.remove(%o);
		}
	}
	%g.clear();

	//2. Add inbound
	%g = PowerGroupDirtyEnterStorage;
	for(%i = 0; %i < %g.getCount(); %i++)
		if(!PowerGroupDirtyStorage.isMember(%o = %g.getObject(%i)))
		{
			if($EOTW::powerDebug && strpos(%o.getName(), "_journal") == 0)
				journalPower(%o, "REALENTER");

			PowerGroupDirtyStorage.add(%o);
			PowerMasterLoopGroup.add(%o);
		}
	%g.clear();

	//3. Run dirty storage/sorting devices
	%g=PowerGroupDirtyStorage;
	%g.POWER_SIMTIME_BLOCKED = 0;
	for(%i = 0; %i < %g.getCount(); %i++)
	{
		if($BLOCKALLStorage || $BLOCKALL) continue; //temp
		%obj = %g.getObject(%i);
		%obj.POWER_LAST++;
		if(%obj.power_simtime_block > getSimTime())
		{
			$EOTW::POWER_SIMTIME_BLOCKED++;
			%g.POWER_SIMTIME_BLOCKED++;
			%obj.POWER_SIMTIME_BLOCKED++;
			continue;
		}

		PowerMasterLoopGroup.add(%obj);
	}

	if(%shuffle_groups)
		%g.Shuffle();

	//Run machines
	//TODO: only hungry machines shall be allowed
	%g=PowerGroupMachine;
	%g.POWER_SIMTIME_BLOCKED = 0;
	for(%i = 0; %i < %g.getCount(); %i++)
	{
		if($BLOCKALLMachine || $BLOCKALL) continue; //temp
		%obj = %g.getObject(%i);
		%obj.POWER_LAST++;
		if(%obj.power_simtime_block > getSimTime())
		{
			$EOTW::POWER_SIMTIME_BLOCKED++;
			%g.POWER_SIMTIME_BLOCKED++;
			%obj.POWER_SIMTIME_BLOCKED++;
			continue;
		}

		PowerMasterLoopGroup.add(%obj);
	}

	if(%shuffle_groups)
		%g.Shuffle();

	//Run master set

	if($EOTW::PT_debug)
	{
		ptimer_end("PowerMaster_p1");
		$EOTW::POWERLOOP_P1_TIMED = ptimer_duration_usec("PowerMaster_p1");
		ptimer_start("PowerMaster_p2");
	}

	//START CUT - Power-cut-code.cs
	//  END CUT - Power-cut-code.cs

	//new loop - no aggregation
	for(%i = 0; %i < PowerMasterLoopGroup.getCount(); %i++)
	{
		schedule(0, 0, IterateLoopCalled, PowerMasterLoopGroup.getObject(%i));
		//IterateLoopCalled(PowerMasterLoopGroup.getObject(%i));
	}


	if($EOTW::PT_debug)
	{
		ptimer_end("PowerMaster_p2");
		$EOTW::POWERLOOP_P2_TIMED = ptimer_duration_usec("PowerMaster_p2");
	}

	$EOTW::PowerMasterLoop = schedule(1000 / $EOTW::PowerTickRate, 0, "PowerMasterLoop");

	if($EOTW::PT_debug)
	{
		ptimer_end("PowerMaster");
		$EOTW::POWERLOOP_ALL_TIMED += ptimer_duration_usec("PowerMaster");
		$EOTW::POWERLOOP_TIMED = ptimer_duration_usec("PowerMaster");
	}
}

//TODO: can this wait until after build is finished loading?
$EOTW::PowerMasterLoop = schedule(10, 0, "PowerMasterLoop");

//No longer used:
function SuperIterateLoopCalled(%set)
{
	if($EOTW::PT_debug)
		ptimer_start("SuperIterateLoopCalled");

	for(%setI = 0; %setI < %set.getCount(); %setI++)
	{
		ptimer_start("object-IterateLoopCalled");
		IterateLoopCalled(%set.getObject(%setI));
		ptimer_end("object-IterateLoopCalled");

		if($EOTW::PT_debug && $TEMP_POWER_MONITOR)
		{
			echo("ehehe");
			$EOTW::LAST_SELECTED_TIMER = ptimer_duration_usec("object-IterateLoopCalled");
			$TEMP_POWER_MONITOR = 0;
		}
	}

	if($EOTW::PT_debug)
	{
		ptimer_end("SuperIterateLoopCalled");
		$EOTW::POWERLOOP_ALL_TIMED += ptimer_duration_usec("SuperIterateLoopCalled");
	}

	$TEMP_POWER_MONITOR = 0;

	return %setI;
}

function IterateLoopCalled(%item)
{
	if($EOTW::PT_debug)
	{
		ptimer_start("IterateLoopCalled");
		//NOTE: no returns in this function...
	}

	//TODO: remove
	if(%item.getID() == 54023)
	{
		//echo("e");
		$TEMP_POWER_MONITOR = 1;
	}

	$EOTW::LAST_POWER_ITERATE = getSimTime();

	//if(!isObject(%item))
	//	return;

	if(%item.energy $= "" || %item.energy < 0)
		%item.energy = 0;

	%stime = getSimTime();
	if(%stime - %item.lastEnergyUpdate >= (1000 / $EOTW::PowerTickRate))
	{
		%item.lastEnergyUpdate = %stime;
		if(%item.getClassName() $= "SimObject" && %item.transferType $= "Matter")
		{
			%item.doMatterTransferFull();
		}

		else if(%item.getDatablock().loopFunc !$= "" && !%item.machineDisabled)
			call(%item.getDatablock().loopFunc, %item);

		//Superceded by using power_simtime_block in Brick_Generators.cs

		//if there is energy stored here - try to move it
		//special rules for solar panel - dont release under 30 (except after sunset)
		//or if [TODO] last energy release time was > 3 seconds ago
		//if(%item.getClassName() $= "FxDTSBrick" && %item.getDatablock().uiName $= "Solar Panel")
		//{
		//	//TODO: allow release of < 30 energy at 100% decay (whenever it's implemented...)
		//	if(%item.energy >= 51 || $EOTW::Time >= 12)
		//		%item.tryPowerTransfer();
		//}

		//else if(%item.energy > 0)
		if(%item.energy > 0)
		{
			fxDTSBrick__tryPowerTransfer(%item);
			//%item.tryPowerTransfer();
		}

		//Is it in Dirty Storage, where it might need to be removed?
		if(PowerGroupDirtyStorage.isMember(%item))
		{
			if($EOTW::powerDebug && strpos(%item.getName(), "_journal") == 0)
				journalPower(%item, "IterateLoopCalled detected as member of DirtyStorage. need removal?");

			//nowhere to move energy, or I'm empty and can't forward anthing more
			if(%item.cableOutputs.getCount() == 0 || %item.energy <= 0)
			{
				if($EOTW::powerDebug && strpos(%item.getName(), "_journal") == 0)
				{
					if(%item.cableOutputs.getCount() == 0)
						journalPower(%item, "EXIT: IterateLoopCalled no outputs");
					else
						journalPower(%item, "EXIT: IterateLoopCalled energy <= 0");
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
					journalPower(%item, "IterateLoopCalled detected full. what about outputs?");

				//Non-Storage objects are out of scope for determining sleep state
				// (all nodes feeding machines/whatever stay awake)
				%nonfull_outputs = 0;
				%nonstorage_outputs = 0;
				if(isObject(%g=%item.cableOutputs))
				{
					for(%i = 0; %i < %g.getCount(); %i++)
					{
						%dst = %g.getObject(%i).powerTarget;
						%dst_free = %dst.getDatablock().energyMaxBuffer - %dst.energy;

						if(%dst_free > 0)
						{
							%nonfull_outputs = 1;
							break;
						}

						if(%dst.getDatablock().energyGroup !$= "Storage")
							%nonstorage_outputs = 1;
						
					}
				}

				if(!%nonfull_outputs)
				{
					if(%nonstorage_outputs)
					{
						//TODO: to be replaced by more efficient system of machines waking up storage inputs
						//Temp fix is just sleep this node for 10 sec in case machines wake up
						%item.power_simtime_block = %stime + 10000 + mRound(getRandom() * 1000);
					}
					else
					{
						if($EOTW::powerDebug)
						{
							if(strpos(%item.getName(), "_journal") == 0)
								journalPower(%item, "EXIT: IterateLoopCalled [set brown] no non-full outputs - energy on elem0 is: "@ %g.getObject(0).powerTarget.energy);

							cancel(%item.colorSched);
							%item.setColor(8);
						}

						if(PowerGroupDirtyEnterStorage.isMember(%item))
							PowerGroupDirtyEnterStorage.remove(%item);

						PowerGroupDirtyExitStorage.add(%item);
					}
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
							journalPower(%item, "IterateLoopCalled Waking up input due to loss of energy: "@ %src.getName());

						if($EOTW::powerDebug && strpos(%src.getName(), "_journal") == 0)
							journalPower(%src, "ENTER: IterateLoopCalled output "@ %item.getName() @" now has room and i'm not empty");

						PowerGroupDirtyEnterStorage.add(%src);
					}
				}
		}
	}

	if($EOTW::PT_debug)
	{
		ptimer_end("IterateLoopCalled");
		//NOTE: no returns in this function...
		$EOTW::POWERLOOP_ALL_TIMED += ptimer_duration_usec("IterateLoopCalled");

		if($TEMP_POWER_MONITOR)
		{
			$EOTW::LAST_SELECTED_TIMER = ptimer_duration_usec("IterateLoopCalled");
			$TEMP_POWER_MONITOR = 0;
		}
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

	%transfered_from_a_source = 0;

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
				%transfered_from_a_source = 1;
				break;
			}
		}
	}

	//if source is empty and i have no buffer, sleep 5 seconds
	if(!%transfered_from_a_source && %obj.buffer $= "")
	{
		//%obj.detected_idle = getSimTime();
		%obj.power_simtime_block = getSimTime() + 5000 + mRound(getRandom() * 250);
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

function fxDTSBrick__tryPowerTransfer(%obj)
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
		%dest_free = %dest.getDatablock().energyMaxBuffer - %dest.energy;
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
	%reached_nontransmission = 0;
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
		//make sure transmission line doesn't think this is a failure (%reached_nontransmission)
		else if(%nextObj.energy >= %nextObj.getDatablock().energyMaxBuffer)
		{
			%reached_nontransmission = 1;
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

		//If just ended a transmission line, but current is not a transmission: this is a break point
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

	//TODO: cannot disambiguate between [full objects at the end]
	//if last object is a transmission node - big oops!
	if((%c=%chain.getCount()) > 0 && (%o=%chain.getObject(%c-1)).getDatablock().energyGroup $= "Transmission")
	{
		if(%reached_nontransmission)
			%first_transmission.transmission_error="";
		else
			%first_transmission.transmission_error="Transmission line unfinished or too long";
	}

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
		%dest_free = %dest.getDatablock().energyMaxBuffer - %dest.energy;

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

	function fxDTSBrick::onAdd(%brick)
	{
		$EOTW::bricksDirty = 1;

		Parent::onAdd(%brick);
	}

	function fxDTSBrick::onRemove(%brick)
	{
		$EOTW::bricksDirty = 1;

		//already be properly covered by ropeClearAll() in RopePackage
		//Fixed schedules - this caused lag issue by deleting ropes before ropeClearAll() could see it
		if(isObject(%o = %brick.cableInputs))
		{
			//Delete all cable objects
			%o.schedule(0, deleteAll);
			%o.schedule(0, delete);
		}

		if(isObject(%o = %brick.cableOutputs))
		{
			%o.schedule(0, deleteAll);
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
	%obj.power_simtime_block = 0;
	
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
			{
				%obj.TESTchangeMatter1 = getSimTime();
				call(%obj.getDatablock().matterUpdateFunc, %obj);
			}
			
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
			{
				%obj.TESTchangeMatter2 = getSimTime();
				call(%obj.getDatablock().matterUpdateFunc, %obj);
			}
			
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

function serverCmdPowerData(%client)
{
	if (!isObject(%player = %client.player))
		return;

	%eye = %player.getEyePoint();
	%dir = %player.getEyeVector();
	%for = %player.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)))
	{
		if(%hit.getType() & $Typemasks::fxBrickAlwaysObjectType)
		{
			%client.chatMessage("["@ %hit.getDatablock().uiName @"]");

			if(!PowerGroupTransmission.isMember(%hit))
				%client.chatMessage("  \c6Energy: \c4"@ %hit.energy);

			if(%hit.power_simtime_block > 0 && %hit.power_simtime_block > getSimTime())
				%client.chatMessage("  \c6Sleeping for: \c3"@ (%hit.power_simtime_block - getSimTime()) @" ms");

			if(PowerGroupStorage.isMember(%hit))
			{
				%msg = "  \c6Storage: \c2YES\c6;";

				%isDirty = PowerGroupDirtyStorage.isMember(%hit);

				if(%isDirty)
					%msg = %msg @" \c6Storage active: \c2YES";
				else
					%msg = %msg @" \c6Storage active: \c0NO";

				%client.chatMessage(%msg);
			}

			else if(PowerGroupTransmission.isMember(%hit))
			{
				%msg = "  \c6Transmission: \c2YES\c6;";

				%error = %hit.transmission_error;

				if(strlen(%error) > 0)
					%msg = %msg @" \c6Error: \c2"@ %error;
				else
					%msg = %msg @" \c6Error: \c7(none)";

				%client.chatMessage(%msg);
			}

			%g[0] = "Inputs";
			%g[1] = "Outputs";
			for(%i = 0; %i < 2; %i++)
			{
				%g = %hit.cable[%g[%i]];

				if(!isObject(%g) || %g.getCount() == 0)
				{
					%client.chatMessage("\c6    ["@ %g[%i] @"] \c7(none)");
					continue;
				}

				%client.chatMessage("\c6    ["@ %g[%i] @"] \c2"@ %g.getCount());
				for(%c = 0; %c < %g.getCount(); %c++)
				{
					%cable = %g.getObject(%c);

					%mat = getField(%cable.parent.material, 0);
					%mat_hex = getMatterType(%mat).color;

					%len = getField(%cable.parent.material, 1);

					%prep = "to";
					%other_obj_name = %cable.powerTarget.getDatablock().uiName;
					if(%g[%i] $= "Inputs")
					{
						%other_obj_name = %cable.powerSource.getDatablock().uiName;
						%prep = "from";
					}

					%client.chatMessage("\c6        - "@ %len @" <color:"@ %mat_hex @">"@ %mat @" \c6"@ %prep @" \c5"@ %other_obj_name);
				}
			}
		}

		else if(%hit.getType() & $TypeMasks::StaticShapeObjectType && %hit.isRope)
		{
			if(!isObject(%hit.getGroup()) || !isObject(%hit.getGroup().cable))
				return;

			%cable = %hit.getGroup().cable;

			%mat = getField(%cable.parent.material, 0);
			%mat_hex = getMatterType(%mat).color;

			%len = getField(%cable.parent.material, 1);

			%client.chatMessage("[Cable: <color:"@ %mat_hex @">"@ %mat @"\c0]");
			%client.chatMessage("\c6  Length: "@ %len);
			%client.chatMessage("\c6  Source: \c3"@ %cable.powerSource.getDatablock().uiName @"");
			%client.chatMessage("\c6  Target: \c3"@ %cable.powerTarget.getDatablock().uiName @"");
			if(%cable.periodLimit > 0)
			{
				%client.chatMessage("\c6  Wattage: \c3"@ %cable.periodLimit @" EU / sec");
				%cable_expiry = getSimTime() - %cable.periodStart;
				%expiry_text = "in last \c3"@ %cable_expiry @"ms";
				if(%cable_expiry >= 1000)
					%expiry_text = "(expired \c3"@ mRound((%cable_expiry / 1000) - 1) @" seconds\c6 ago)";

				%client.chatMessage("\c6  Power transfered: \c3"@ %cable.periodTransfered @"\c6 EU "@ %expiry_text);
			}
			else
				%client.chatMessage("\c6    - No power transfered during lifetime");
		}
	}
}

function serverCmdPD(%client)
{
	serverCmdPowerData(%client);
}

function serverCmdGetPD(%client)
{
	serverCmdPowerData(%client);
}

function serverCmdGetPowerData(%client)
{
	serverCmdPowerData(%client);
}


