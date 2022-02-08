datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Water Pump";
	energyGroup = "Machine";
	energyMaxBuffer = 100;
    energyWattage = 10;
	loopFunc = "EOTW_WaterPumpLoop";
    inspectFunc = "EOTW_WaterPumpInspectLoop";
	//iconName = "./Bricks/Icon_Generator";

    matterMaxBuffer = 128;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Steel" TAB 32 TAB "Silver" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "Uses energy to pump water from deep underground.";

function Player::EOTW_WaterPumpInspectLoop(%player, %brick)
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
    for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_WaterPumpInspectLoop", %brick);
}


function fxDtsBrick::EOTW_WaterPumpLoop(%obj)
{
    %data = %obj.getDatablock();
    %costPerUnit = 20;
	if (%obj.craftingPower >= %costPerUnit)
	{
		%change = %obj.changeMatter("Water", 1, "Output");
        %obj.craftingPower -= %change * %costPerUnit;
	}
    else
    {
        %change = mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.getPower());
        %obj.craftingPower += %change;
        %obj.changePower(%change * -1);
    }
}

datablock fxDTSBrickData(brickEOTWSteamEngineData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Steam Engine";
	energyGroup = "Source";
	energyMaxBuffer = 400;
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 2;
	loopFunc = "EOTW_SteamEngineLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWSteamEngineData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWSteamEngineData"] = "[[(WIP)]] A more advanced stirling engine that takes inputted water and fuel and creates steam. Use power for slight burn boost.";

function fxDtsBrick::EOTW_SteamEngineLoop(%obj)
{

}

datablock fxDTSBrickData(brickEOTWThermoelectricBoilerData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Thermoelectric Boiler";
	energyGroup = "Source";
	energyMaxBuffer = 400;
	loopFunc = "EOTW_ThermoelectricBoilerLoop";
	inspectFunc = "EOTW_DefaultInspectLoop";
	matterMaxBuffer = 100000;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 2;
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWThermoelectricBoilerData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWThermoelectricBoilerData"] = "[[(WIP)]] Uses hot coolant or hot cryostablizer to heat water into steam. Use power for slight burn boost.";

function fxDtsBrick::EOTW_ThermoelectricBoilerLoop(%obj)
{

}