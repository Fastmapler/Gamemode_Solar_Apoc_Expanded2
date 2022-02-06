//Solar Panel
datablock fxDTSBrickData(brickEOTWSolarPanelData)
{
	brickFile = "./Bricks/SolarPanel.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Solar Panel";
	energyGroup = "Source";
	energyMaxBuffer = 1600;
	loopFunc = "EOTW_SolarPanelLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	//port info
	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.2";

};
$EOTW::CustomBrickCost["brickEOTWSolarPanelData"] = 0.85 TAB "7a7a7aff" TAB 64 TAB "Silver" TAB 64 TAB "Rosium" TAB 64 TAB "Teflon";
$EOTW::BrickDescription["brickEOTWSolarPanelData"] = "Produces power when exposed to direct sunlight. Topside must be completely untouched for functionality.";

function fxDtsBrick::EOTW_SolarPanelLoop(%obj)
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
			%wattage = 160;
			%obj.ChangePower(%wattage / $EOTW::PowerTickRate);
		}
	}
}

//Manual Crank
datablock fxDTSBrickData(brickEOTWHandCrankData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Hand Crank";
	energyGroup = "Source";
	energyMaxBuffer = 6400;
	loopFunc = "";
    inspectFunc = "EOTW_HandCrankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWHandCrankData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 32 TAB "Copper" TAB 48 TAB "Lead";
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

    %printText = %printText @ "Cranking power...<br>"@ (%brick.getPower() + 0) @ "/" @ %data.energyMaxBuffer @ " EU\n";

	%client.centerPrint(%printText, 1);

	%wattage = 160;
	%brick.ChangePower(%wattage / $EOTW::PowerTickRate);
	
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
	energyMaxBuffer = 12800;
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 1;
	loopFunc = "EOTW_StirlingEngineUpdate";
    inspectFunc = "EOTW_StirlingEngineInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWStirlingEngineData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 192 TAB "Lead" TAB 96 TAB "Gold";
$EOTW::BrickDescription["brickEOTWStirlingEngineData"] = "Burns various materials to produce decent amounts of power.";

function fxDtsBrick::EOTW_StirlingEngineUpdate(%obj)
{
	%wattage = 640;
	if (%obj.storedFuel > 0)
		%obj.storedFuel -= %obj.changePower(mMin(%obj.storedFuel, %wattage / $EOTW::PowerTickRate));

	if (%obj.storedFuel <= 0)
	{
		%matterType = getMatterType(getField(%obj.matter["Input", 0], 0));
		if (isObject(%matterType) && %matterType.fuelCapacity > 0)
			%obj.storedFuel += %obj.changeMatter(%matterType.name, -16, "Input") * %matterType.fuelCapacity * -1;
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

//Steam Engine
datablock fxDTSBrickData(brickEOTWSteamEngineData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Steam Engine";
	energyGroup = "Source";
	energyMaxBuffer = 12800;
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 2;
	loopFunc = "EOTW_SteamEngineLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWSteamEngineData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWSteamEngineData"] = "[[(WIP)]] A more advanced stirling engine that takes inputted water and fuel and creates steam.";

function fxDtsBrick::EOTW_SteamEngineLoop(%obj)
{

}

//Steam Turbine
datablock fxDTSBrickData(brickEOTWSteamTurbineData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Steam Turbine";
	energyGroup = "Source";
	energyMaxBuffer = 4096;
	loopFunc = "EOTW_SteamTurbineLoop";
	inspectFunc = "EOTW_DefaultInspectLoop";
	matterMaxBuffer = 100000;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWSteamTurbineData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWSteamTurbineData"] = "[[(WIP)]] Uses steam to create large amounts of power. Returns steam as water.";

function fxDtsBrick::EOTW_SteamTurbineLoop(%obj)
{

}

//Soul Reactor
datablock fxDTSBrickData(brickEOTWSoulReactorData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Plutonium RTG";
	energyGroup = "Source";
	energyMaxBuffer = 4096;
	loopFunc = "EOTW_SoulReactorLoop";
	inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["EOTW_SoulReactorLoop"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["EOTW_SoulReactorLoop"] = "[[(WIP)]] Dusts nearby monster corpses for power.";

function fxDtsBrick::EOTW_SteamTurbineLoop(%obj)
{

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
	inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWRadioIsotopeGeneratorData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Adamantine" TAB 128 TAB "Plutonium" TAB 480 TAB "Lead";
$EOTW::BrickDescription["brickEOTWRadioIsotopeGeneratorData"] = "Passively produces power.";

function fxDtsBrick::EOTW_RadioIsotopeGeneratorLoop(%obj)
{
		%obj.changePower(40 / $EOTW::PowerTickRate);
}

exec("./Brick_MFR.cs");