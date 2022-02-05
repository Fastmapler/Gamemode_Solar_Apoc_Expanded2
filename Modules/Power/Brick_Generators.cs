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

datablock fxDTSBrickData(brickEOTWManualCrankData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Manual Crank";
	energyGroup = "Source";
	energyMaxBuffer = 6400;
	loopFunc = "";
    inspectFunc = "EOTW_HandCrankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWManualCrankData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 32 TAB "Copper" TAB 48 TAB "Lead";

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