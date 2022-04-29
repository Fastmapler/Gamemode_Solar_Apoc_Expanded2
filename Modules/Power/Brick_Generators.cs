//Manual Crank
datablock fxDTSBrickData(brickEOTWHandCrankData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Hand Crank";
	energyGroup = "Source";
	energyMaxBuffer = 100;
	loopFunc = "";
    inspectFunc = "EOTW_HandCrankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWHandCrankData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWHandCrankData"] = "A simple device that produces power when activated.";

function Player::EOTW_HandCrankInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	%data = %brick.getDatablock();
	%printText = "<color:ffffff>";

	%wattage = 20;
	%brick.ProcessTime += %wattage / $EOTW::PowerTickRate;

    %printText = %printText @ "Cranking power ACTIVE...<br>"@ (%brick.getPower() + 0) @ "/" @ %data.energyMaxBuffer @ " EU\nYou are producing " @ %wattage @ " EU/s";

	%client.centerPrint(%printText, 1);

	

	if (%brick.ProcessTime >= 1)
	{
		%ProcessTimeChange = mFloor(%brick.ProcessTime);
		%brick.ChangePower(%ProcessTimeChange);
		%brick.ProcessTime -= %ProcessTimeChange;
	}
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_HandCrankInspectLoop", %brick);
}

//Stirling Engine
datablock fxDTSBrickData(brickEOTWStirlingEngineData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Stirling Engine";
	energyGroup = "Source";
	energyMaxBuffer = 200;
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 1;
	loopFunc = "EOTW_StirlingEngineUpdate";
    inspectFunc = "EOTW_StirlingEngineInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWStirlingEngineData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 128 TAB "Glass" TAB 112 TAB "Gold";
$EOTW::BrickDescription["brickEOTWStirlingEngineData"] = "Burns various materials to produce decent amounts of power.";

function EOTW_StirlingEngineUpdate(%obj)
{
	%wattage = 40;
	if (%obj.storedFuel > 0)
		%obj.storedFuel -= %obj.changePower(mMin(%obj.storedFuel, %wattage / $EOTW::PowerTickRate));

	if (%obj.storedFuel < 1)
	{
		%matterType = getMatterType(getField(%obj.matter["Input", 0], 0));
		if (isObject(%matterType) && %matterType.fuelCapacity > 0)
			%obj.storedFuel += mFloor(%obj.changeMatter(%matterType.name, -16, "Input") * %matterType.fuelCapacity * -1);
	}
}

function Player::EOTW_StirlingEngineInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	%data = %brick.getDatablock();
	%printText = "<color:ffffff>";

    %printText = %printText @ (%brick.getPower() + 0) @ "/" @ %data.energyMaxBuffer @ " EU\n";
    for (%i = 0; %i < %data.matterSlots["Input"]; %i++)
	{
		%matter = %brick.Matter["Input", %i];

		if (%matter !$= "")
			%printText = %printText @ "Input " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Input " @ (%i + 1) @ ": --" @ "\n";
	}
	%printText = %printText @ (%brick.storedFuel + 0) @ "u of Unburned Fuel";

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_StirlingEngineInspectLoop", %brick);
}

//Soul Reactor
datablock fxDTSBrickData(brickEOTWSoulReactorData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Soul Reactor";
	energyGroup = "Source";
	energyMaxBuffer = 250;
	loopFunc = "EOTW_SoulReactorLoop";
	inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWSoulReactorData"] = 1.00 TAB "7a7a7aff" TAB 288 TAB "Steel" TAB 64 TAB "Leather" TAB 64 TAB "Rubber";
$EOTW::BrickDescription["brickEOTWSoulReactorData"] = "Dusts nearby monster corpses for power.";

function EOTW_SoulReactorLoop(%obj)
{
	initContainerRadiusSearch(%obj.getPosition(), 16, $TypeMasks::CorpseObjectType);

	if (getSimTime() - %obj.lastTickTime > 2000)
	{
		%obj.lastTickTime = getSimTime();
		while(isObject(%hit = containerSearchNext()))
		{
			spawnBeam(%hit.getPosition(), %obj.getPosition(), 1);
			%obj.changePower(mCeil(%hit.getDatablock().maxDamage / 1));
			%hit.delete();
		}
	}
}

datablock StaticShapeData(EOTWBeamStatic) { shapeFile = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Shapes/bullettrail.dts"; };
function spawnBeam(%startpos,%endpos,%size)
{
	%p = new StaticShape() { dataBlock = EOTWBeamStatic; };
	MissionCleanup.add(%p);
	
	%vel = vectorNormalize(vectorSub(%startpos,%endpos));
	%x = getWord(%vel,0)/2;
	%y = (getWord(%vel,1) + 1)/2;
	%z = getWord(%vel,2)/2;
	%p.setTransform(%endpos SPC VectorNormalize(%x SPC %y SPC %z) SPC mDegToRad(180));
	%p.setScale(%size SPC vectorDist(%startpos,%endpos) SPC %size);
}

function EOTWBeamStatic::onAdd(%this,%obj)
{
	%obj.playThread(0,root);
	%obj.schedule(100,delete);
}

//Steam Turbine
datablock fxDTSBrickData(brickEOTWSteamTurbineData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Steam Turbine";
	energyGroup = "Source";
	energyMaxBuffer = 250;
	loopFunc = "EOTW_SteamTurbineLoop";
	inspectFunc = "EOTW_DefaultInspectLoop";
	matterMaxBuffer = 256;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWSteamTurbineData"] = 1.00 TAB "7a7a7aff" TAB 288 TAB "Steel" TAB 256 TAB "Glass" TAB 128 TAB "Rosium";
$EOTW::BrickDescription["brickEOTWSteamTurbineData"] = "Uses steam to create large amounts of power. Returns most steam as water.";

function EOTW_SteamTurbineLoop(%obj)
{
	if (%obj.CondensatorBuffer > 0)
	{
		%obj.CondensatorBuffer -= %obj.changeMatter("Water", %obj.CondensatorBuffer, "Output");
	}
	else
	{
		%steamAmount = %obj.GetMatter("Steam", "Input");
		if (%steamAmount > 0)
		{
			%change = %obj.changePower(%steamAmount);
			%obj.changeMatter("Steam", %change * -1, "Input");

			%obj.CondensatorBuffer += mFloor(%change / $EOTW::SteamToWaterRatio);
			%obj.CondensatorBuffer -= %obj.changeMatter("Water", %obj.CondensatorBuffer, "Output");
		}
	}
}

//Solar Panel
datablock fxDTSBrickData(brickEOTWSolarPanelData)
{
	brickFile = "./Bricks/SolarPanel.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Solar Panel";
	energyGroup = "Source";
	energyMaxBuffer = 100;
	loopFunc = "EOTW_SolarPanelLoop";
    inspectFunc = "EOTW_SolarPanelInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	//port info
	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.2";

};
$EOTW::CustomBrickCost["brickEOTWSolarPanelData"] = 1.00 TAB "7a7a7aff" TAB 64 TAB "Adamantine" TAB 64 TAB "Teflon" TAB 16 TAB "Silver";
$EOTW::BrickDescription["brickEOTWSolarPanelData"] = "Produces power when exposed to direct sunlight. Topside must be completely untouched for functionality."; // Will eventually burn out and need to be replaced.

function EOTW_SolarPanelLoop(%obj)
{
	if ($EOTW::Time < 12)
	{
		%val = ($EOTW::Time / 12) * $pi;
	    %ang = ($EnvGuiServer::SunAzimuth / 180) * $pi;
	    %dir = vectorScale(mSin(%ang) * mCos(%val) SPC mCos(%ang) * mCos(%val) SPC mSin(%val), 500);
		%ray = containerRaycast(vectorAdd(%pos = %obj.getPosition(), %dir), %pos, $Typemasks::fxBrickObjectType);
		%hit = firstWord(%ray);
		if((!isObject(%hit) || (%hit == %obj)) && !%obj.getUpBrick(0))
		{
			%wattage = 10 * $EOTW::TimeBoost;
			%obj.ProcessTime += %wattage / $EOTW::PowerTickRate;

			if (%obj.ProcessTime >= 1 && %obj.decayAmount < 15000)
			{
				%ProcessTimeChange = mFloor(%obj.ProcessTime);
				%obj.ChangePower(%ProcessTimeChange);
				%obj.ProcessTime -= %ProcessTimeChange;

				//if (getRandom() < 0.05)
					//%obj.decayAmount += %ProcessTimeChange;

				if (%obj.decayAmount >= 15000)
					%obj.setShapeFX(2);
			}
		}
	}
}

function Player::EOTW_SolarPanelInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	%data = %brick.getDatablock();
	%printText = "<color:ffffff>";


    %printText = %printText @ (%brick.getPower()) @ "/" @ %data.energyMaxBuffer @ " EU\n";

	%wattage = 10 * $EOTW::TimeBoost;

	if (%obj.decayAmount < 15000)
		%printText = %printText @ "Producing " @ %brick.RTGWattageValue() @ " EU/s.";
	else
		%printText = %printText @ "\c0The voltaic cells are burnt out! Replace brick or fix!";

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_SolarPanelInspectLoop", %brick);
}

//Radioisotope
datablock fxDTSBrickData(brickEOTWRadioIsotopeGeneratorData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Plutonium RTG";
	energyGroup = "Source";
	energyMaxBuffer = 64;
	loopFunc = "EOTW_RadioIsotopeGeneratorLoop";
	inspectFunc = "EOTW_RTGInspectLoop";
	//iconName = "./Bricks/Icon_Generator";
};

$EOTW::CustomBrickCost["brickEOTWRadioIsotopeGeneratorData"] = 0.85 TAB "7a7a7aff" TAB 128 TAB "Adamantine" TAB 64 TAB "Plutonium" TAB 480 TAB "Lead";
$EOTW::BrickDescription["brickEOTWRadioIsotopeGeneratorData"] = "Passively produces power. Decays slowly overtime.";

function fxDtsBrick::RTGWattageValue(%obj)
{
	//Wattage is based off of half-life formula.
	%baseWattage = 25;
	%halflife3isrealyoucandownloaditby = 86400;
	%wattage = %baseWattage * mPow(0.5, (%obj.decayAmount + 0) / %halflife3isrealyoucandownloaditby);
	return %wattage;
}

function EOTW_RadioIsotopeGeneratorLoop(%obj)
{
	%obj.ProcessTime += %obj.RTGWattageValue();

	if (%obj.ProcessTime >= 1)
	{
		%ProcessTimeChange = mFloor(%obj.ProcessTime);
		%obj.ChangePower(%ProcessTimeChange);
		%obj.ProcessTime -= %ProcessTimeChange;
	}

	if (getSimTime() - %obj.lastDecay >= 1000 && %obj.decayAmount < 999999)
	{
		%obj.lastDecay = getSimTime();
		%obj.decayAmount++;
	}
}

function Player::EOTW_RTGInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	%data = %brick.getDatablock();
	%printText = "<color:ffffff>";


    %printText = %printText @ (%brick.getPower()) @ "/" @ %data.energyMaxBuffer @ " EU\n";
	%printText = %printText @ "Producing " @ %brick.RTGWattageValue() @ " EU/s.";

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_RTGInspectLoop", %brick);
}


exec("./Brick_MFR.cs");
