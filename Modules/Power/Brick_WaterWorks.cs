$EOTW::SteamToWaterRatio = 1; //A higher ratio would be more realistic but it will result in the funny decimal

datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Bricks/WaterPump.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Water Pump";
	energyGroup = "Machine";
	energyMaxBuffer = 100;
	//energyWattage = 10; //moved to method
	loopFunc = "EOTW_WaterPumpLoop";
	inspectFunc = "EOTW_WaterPumpInspectLoop";
	//iconName = "./Bricks/Icon_Generator";

    matterMaxBuffer = 128;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Rubber" TAB 96 TAB "Steel" TAB 96 TAB "Lead";
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


function EOTW_WaterPumpLoop(%obj)
{
	//Adjusted:
	//1 water per 2 seconds

	//Produce some water - then bar for 2 seconds

	%costPerUnit = 20;
	if(%obj.energy >= %costPerUnit)
	{
		%change = %obj.changeMatter("Water", 1, "Output");
		%obj.changePower(-%costPerUnit);

		//sleep 1 second
		%obj.power_simtime_block = getSimTime() + 2000;
		return;
	}

	//Sleep 6 seconds if there is no energy left
	%obj.power_simtime_block = getSimTime() + 6000 + mRound(getRandom() * 1000);
}

datablock fxDTSBrickData(brickEOTWSteamEngineData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Water Boiler";
	energyGroup = "Source";
	energyMaxBuffer = 0;
	matterMaxBuffer = 250;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;
	loopFunc = "EOTW_SteamEngineLoop";
    inspectFunc = "EOTW_SteamEngineInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWSteamEngineData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Electrum" TAB 128 TAB "Rosium";
$EOTW::BrickDescription["brickEOTWSteamEngineData"] = "A more advanced and efficent stirling engine that takes inputted water and fuel and creates steam. Use the steam turbine with this.";

function EOTW_SteamEngineLoop(%obj)
{
	%wattage = 100;
	if (%obj.storedFuel > 0)
	{
		%fuelConsumption = getMin(%obj.storedFuel, %wattage / $EOTW::PowerTickRate);
		%waterChange -= %obj.changeMatter("Water", (%fuelConsumption * -1) / $EOTW::SteamToWaterRatio, "Input");
		%steamCreated = %obj.changeMatter("Steam", %waterChange * $EOTW::SteamToWaterRatio, "Output");
		%obj.storedFuel -= %steamCreated; //The steam engine has a +15% efficency to power. Get on steam power.
		//Also compesates for the small loss of water when using the steam turbine.
	}	

	if (%obj.storedFuel < 1)
	{
		for (%i = 0; %i < %obj.getDatablock().matterSlots["Input"]; %i++)
		{
			%matterType = getMatterType(getField(%obj.matter["Input", %i], 0));
			if (isObject(%matterType) && %matterType.fuelCapacity > 0)
			{
				%obj.storedFuel += mFloor(%obj.changeMatter(%matterType.name, -32, "Input") * %matterType.fuelCapacity * -1.5);
				break;
			}
		}
	}
}

function Player::EOTW_SteamEngineInspectLoop(%player, %brick)
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
    for (%i = 0; %i < %data.matterSlots["Input"]; %i++)
	{
		%matter = %brick.Matter["Input", %i];

		if (%matter !$= "")
			%printText = %printText @ "Input " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Input " @ (%i + 1) @ ": --" @ "\n";
	}
	%printText = %printText @ (%brick.storedFuel + 0) @ "u of Unburned Fuel<br>";

	 for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_SteamEngineInspectLoop", %brick);
}

datablock fxDTSBrickData(brickEOTWThermoelectricBoilerData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Fluid Heat Exchanger";
	energyGroup = "Source";
	loopFunc = "EOTW_ThermoelectricBoilerLoop";
	inspectFunc = "EOTW_DefaultInspectLoop";
	matterMaxBuffer = 256;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 2;
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWThermoelectricBoilerData"] = 1.00 TAB "7a7a7aff" TAB 160 TAB "Copper" TAB 96 TAB "Steel" TAB 64 TAB "Dielectrics";
$EOTW::BrickDescription["brickEOTWThermoelectricBoilerData"] = "Uses hot coolant or hot cryostablizer to heat water into steam.";

function EOTW_ThermoelectricBoilerLoop(%obj)
{
	%data = %obj.getDatablock();

	%matter1 = %obj.matter["Input", 0];
	%matter2 = %obj.matter["Input", 1];

	if (isObject(%matter = getMatterType(getField(%matter1, 0))))
	{
		if (%matter.boilMatter !$= "")
			%cooling = %matter;
		if (%matter.cooledMatter !$= "")
			%heating = %matter;
	}
	if (isObject(%matter = getMatterType(getField(%matter2, 0))))
	{
		if (%matter.boilMatter !$= "")
			%cooling = %matter;
		if (%matter.cooledMatter !$= "")
			%heating = %matter;
	}
	if (isObject(%cooling) && isObject(%heating) && %cooling != %heating)
	{
		%coolingAmount = %obj.GetMatter(%cooling.name, "Input");
		%heatingAmount = %obj.GetMatter(%heating.name, "Input");
		%coolingChange = %cooling.boilCapacity * %coolingAmount;
		%heatingChange = %heating.boilCapacity * %heatingAmount;

		%boilRatio	   = %heating.boilCapacity / %cooling.boilCapacity;
		%totalExchange = %coolingChange / %heatingChange;

		//echo("Test" SPC %coolingAmount SPC "|" SPC %heatingAmount SPC "|" SPC  %coolingChange SPC "|" SPC  %heatingChange SPC "|" SPC  %boilRatio SPC "|" SPC  %totalExchange);
		if (%totalExchange >= 1)
		{
			%change1 = %obj.changeMatter(%cooling.boilMatter, mFloor(%totalExchange) * mFloor(%boilRatio), "Output");
			%obj.changeMatter(%cooling.name, %change1 * -1, "Input");

			%change2 = %obj.changeMatter(%heating.cooledMatter, mFloor((%totalExchange * %change1) / mFloor(%boilRatio)), "Output");
			%obj.changeMatter(%heating.name, %change2 * -1, "Input");
			echo("Test2" SPC %change1 SPC %change2);
		}
	}
}
