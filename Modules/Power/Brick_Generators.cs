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
	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;
	
    %wattage = 160;

	if ($EOTW::Time < 12)
	{
		%val = ($EOTW::Time / 12) * $pi;
	    %ang = ($EnvGuiServer::SunAzimuth / 180) * $pi;
	    %dir = vectorScale(mSin(%ang) * mCos(%val) SPC mCos(%ang) * mCos(%val) SPC mSin(%val), 512);
		%ray = containerRaycast(vectorAdd(%pos = %obj.getPosition(), %dir), %pos, $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType);
		if(!isObject(%hit = firstWord(%ray)) || %hit == %obj)
            %obj.ChangePower(%wattage / $EOTW::PowerTickRate);
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
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWManualCrankData"] = 0.85 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 32 TAB "Copper" TAB 48 TAB "Lead";

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

	%wattage = 320;
	%brick.ChangePower(%wattage / $EOTW::PowerTickRate);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_HandCrankInspectLoop", %brick);
}