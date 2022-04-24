datablock fxDTSBrickData(brickEOTWChargePadData)
{
	brickFile = "./Bricks/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Charge Pad";
	energyGroup = "Machine";
	energyMaxBuffer = 640;
	loopFunc = "EOTW_ChargePadLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWChargePadData"] = 1.00 TAB "7a7a7aff" TAB 1024 TAB "Wood" TAB 128 TAB "Iron" TAB 32 TAB "Copper";
$EOTW::BrickDescription["brickEOTWChargePadData"] = "Charges the player's internal battery.";

function EOTW_ChargePadLoop(%obj)
{
	if (getSimTime() - %obj.LastChargeLoop < 100)
		return;

	%obj.LastChargeLoop = getSimTime();

	%eye = %obj.GetPosition();
	%dir = "0 0 1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::PlayerObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 2)), %mask, %obj);
	
	if (isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "Player")
	{
		%obj.changePower(%hit.ChangeBatteryEnergy(%obj.getPower()) * -1);
		if (isObject(%client = %hit.client))
			%client.centerPrint(%hit.GetBatteryText(), 1);
	}
}

datablock fxDTSBrickData(brickEOTWEnergyRecoveryPadData)
{
	brickFile = "./Bricks/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Energy Recovery Pad";
	energyGroup = "Source";
	energyMaxBuffer = 1280;
	loopFunc = "EOTW_EnergyRecoveryPadLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWEnergyRecoveryPadData"] = 1.00 TAB "ff0000ff" TAB 2048 TAB "Wood" TAB 96 TAB "Steel" TAB 16 TAB "Silver";
$EOTW::BrickDescription["brickEOTWEnergyRecoveryPadData"] = "Drain's the player's battery into its own energy storage.";

function EOTW_EnergyRecoveryPadLoop(%obj)
{
	if (getSimTime() - %obj.LastChargeLoop < 100)
		return;

	%obj.LastChargeLoop = getSimTime();

	%eye = %obj.GetPosition();
	%dir = "0 0 1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::PlayerObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 2)), %mask, %obj);
	
	if (isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "Player")
	{
		%change = %obj.changePower(%hit.getBatteryEnergy());
		%hit.ChangeBatteryEnergy(%change * -1);
		if (isObject(%client = %hit.client))
			%client.centerPrint(%hit.GetBatteryText(), 1);
	}
}

datablock fxDTSBrickData(brickEOTWThumperData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Mining Thumper";
	energyGroup = "Machine";
	energyMaxBuffer = 640;
	energyWattage = 40;
	loopFunc = "EOTW_ThumperLoop";
    inspectFunc = "EOTW_ThumperInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWThumperData"] = 1.00 TAB "7a7a7aff" TAB 240 TAB "Lead" TAB 288 TAB "Steel" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickEOTWThumperData"] = "When active gives a 100% speed boost to gathering nearby resources.";

function Player::EOTW_ThumperInspectLoop(%player, %brick)
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
	%printText = %printText @ (%brick.getPower() + 0) @ " EU (" @ %data.energyWattage @ " EU/s when active)\n";

	if (getSimTime() - %brick.lastThump < 1000)
		%printText = %printText @ "Now boosting by +100% in a 128 stud radius.";

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_ThumperInspectLoop", %brick);
}

function EOTW_ThumperLoop(%obj)
{
	%data = %obj.getDatablock();
	%change = mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.getPower());
	%obj.changePower(%change * -1);
    
	if (%obj.getPower() / %data.energyMaxBuffer > 0.5)
		%obj.lastThump = getSimTime();
}

datablock fxDTSBrickData(brickEOTWChemDiffuserData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Chem Diffuser";
	energyGroup = "Machine";
	energyMaxBuffer = 640;
	matterMaxBuffer = 128;
	energyWattage = 20;
	matterSlots["Input"] = 1;
	loopFunc = "EOTW_ChemDiffuserLoop";
    inspectFunc = "EOTW_ChemDiffuserInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWChemDiffuserData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Plastic" TAB 128 TAB "Silver" TAB 128 TAB "Lithium";
$EOTW::BrickDescription["brickEOTWChemDiffuserData"] = "Efficently applies mixes to all players alive. Fuel burn does not increase with more players.";

function EOTW_ChemDiffuserLoop(%obj)
{
	%data = %obj.getDatablock();
	%change = mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.getPower());
	%obj.changePower(%change * -1);

	if (getSimTime() - %obj.lastChemTick < 1000)
		return;

	%obj.lastChemTick = getSimTime();

	if (%obj.getPower() / %data.energyMaxBuffer < 0.5)
		return;

	if (%obj.storedFuel > 0)
	{
		for (%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			%client = ClientGroup.getObject(%i);
			if (!isObject(%player = %client.player))
				continue;

			switch$ (%obj.fuelType)
			{
				case "Healing Mix":
					%player.addHealth(1);
				case "Steroid Mix":
					%player.steroidlevel += 0.5;
					%player.schedule(1000, "ChemDiffuserEndEffect", %obj.fuelType);
				case "Adrenline Mix":
					%player.ChangeSpeedMulti(0.5);
					%player.schedule(1000, "ChemDiffuserEndEffect", %obj.fuelType);
				case "Gatherer Mix":
					%player.Gathererlevel += 0.25;
					%player.schedule(1000, "ChemDiffuserEndEffect", %obj.fuelType);
				case "Overload Mix":
					%player.ammoReturnLevel += 0.5;
					%player.schedule(1000, "ChemDiffuserEndEffect", %obj.fuelType);
				case "Leatherskin Mix":
					%player.sunResistance += 1.0;
					%player.schedule(1000, "ChemDiffuserEndEffect", %obj.fuelType);
			}
		}

		%obj.storedFuel--;
	}
	else
	{
		if (isObject(%matter = getMatterType(getField(%obj.matter["Input", 0], 0))) && %matter.isMix && %obj.GetMatter(%matter.name, "Input") > 0)
		{
			%obj.storedFuel += 16;
			%obj.fuelType = %matter.name;
			%obj.changeMatter(%matter.name, -1, "Input");
		}
	}
}

function Player::EOTW_ChemDiffuserInspectLoop(%player, %brick)
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
	%printText = %printText @ (%brick.storedFuel + 0) @ " seconds left of " @ (%brick.fuelType $= "" ? "---" : %brick.fuelType) @ ".";

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_ChemDiffuserInspectLoop", %brick);
}

function Player::ChemDiffuserEndEffect(%obj, %effect)
{
	switch$ (%effect)
	{
		case "Steroid Mix":
			%obj.steroidlevel -= 0.5;
		case "Adrenline Mix":
			%obj.ChangeSpeedMulti(-0.5);
		case "Gatherer Mix":
			%obj.Gathererlevel -= 0.25;
		case "Overload Mix":
			%obj.ammoReturnLevel -= 0.5;
		case "Leatherskin Mix":
			%obj.sunResistance -= 1.0;
	}
}

datablock fxDTSBrickData(brickEOTWDroneServerData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Droner Server";
	energyGroup = "Machine";
	energyMaxBuffer = 640;
	loopFunc = "EOTW_DroneServerLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWDroneServerData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity"; //1.00 TAB "7a7a7aff" TAB 512 TAB "Naturum" TAB 256 TAB "Energium" TAB 256 TAB "Coolant";
$EOTW::BrickDescription["brickEOTWDroneServerData"] = "While active grants the owner Server Points (SP). SP is required for drones to run.";

datablock fxDTSBrickData(brickEOTWSolarShieldProjectorData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Solar Shield Projector";
	energyGroup = "Machine";
	energyMaxBuffer = 1280;
	energyWattage = 120;
	loopFunc = "EOTW_SolarShieldProjectorLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWSolarShieldProjectorData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Energium" TAB 256 TAB "Teflon" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickEOTWSolarShieldProjectorData"] = "When powered for long enough produces a large bubble shield that grants all living entities immunity to the sun.";

function EOTW_SolarShieldProjectorLoop(%obj)
{
	%data = %obj.getDatablock();
	%change = mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.getPower());
	%obj.changePower(%change * -1);
    
	if (%obj.getPower() / %data.energyMaxBuffer > 0.5)
	{
		if (!isObject(%obj.shieldShape))
		{
			%obj.shieldShape = new StaticShape()
			{
				datablock = SolarShieldProjectorShieldShape;
				position = %obj.getPosition();
				scale = "1 1 1";
			};
			%obj.shieldShape.setTransform(%obj.getPosition());
			%obj.shieldShape.EOTW_SetShieldLevel(18);
			
			//serverplay3D(shieldPowerUpSound, %obj.getPosition());
		}

		//Using schedules to make sure we stop the projector if we get shutoff via events
		//Or if the bricks get hammered.
		cancel(%obj.shieldShape.shieldSchedule);
		%obj.shieldShape.shieldSchedule = %obj.shieldShape.schedule(1000, "EOTW_SolarShieldProjectorEnd");
	}
}

function brickEOTWSolarShieldProjectorData::onPlant(%this,%brick)
{
	Parent::onPlant(%this,%brick);

	if (!isObject(SolarShieldGroup))
		new SimSet(SolarShieldGroup);
		
	SolarShieldGroup.add(%brick);
}

function brickEOTWSolarShieldProjectorData::onLoadPlant(%this,%brick)
{
	Parent::onLoadPlant(%this,%brick);
	
	if (!isObject(SolarShieldGroup))
		new SimSet(SolarShieldGroup);

	SolarShieldGroup.add(%brick);
}

datablock StaticShapeData(SolarShieldProjectorShieldShape)
{
	category = "Shapes";
	shapeFile = "./Shapes/shield.dts";
};

function StaticShape::EOTW_SolarShieldProjectorEnd(%obj)
{
	%obj.delete();
}

function StaticShape::EOTW_SetShieldLevel(%obj, %Level)
{
	%obj.level = %level;
	%obj.setScale(vectorScale("1 1 1", 1.5 + %obj.level / 5));
}

function StaticShape::EOTW_GetShieldRadius(%obj)
{
	%obj.level = %obj.level + 0;

	return (4 * (2.4 + %obj.level / 4));
}