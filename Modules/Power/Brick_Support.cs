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

function fxDtsBrick::EOTW_ChargePadLoop(%obj)
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
$EOTW::CustomBrickCost["brickEOTWEnergyRecoveryPadData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity"; //1.00 TAB "7a7a7aff" TAB 2048 TAB "Wood" TAB 96 TAB "Steel" TAB 16 TAB "Silver";
$EOTW::BrickDescription["brickEOTWEnergyRecoveryPadData"] = "Drain's the player's battery into its own energy storage.";

datablock fxDTSBrickData(brickEOTWThumperData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Mining Thumper";
	energyGroup = "Machine";
	energyMaxBuffer = 640;
	loopFunc = "EOTW_ThumperLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWThumperData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity"; //1.00 TAB "7a7a7aff" TAB 480 TAB "Lead" TAB 288 TAB "Steel" TAB 128 TAB "Dielectics";
$EOTW::BrickDescription["brickEOTWThumperData"] = "When active gives a speed boost to gathering resources.";

datablock fxDTSBrickData(brickEOTWChemDiffuserData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Chem Diffuser";
	energyGroup = "Machine";
	energyMaxBuffer = 640;
	loopFunc = "EOTW_ChemDiffuserLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWChemDiffuserData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity"; //1.00 TAB "7a7a7aff" TAB 256 TAB "Plastic" TAB 128 TAB "Silver" TAB 128 TAB "Lithium";
$EOTW::BrickDescription["brickEOTWChemDiffuserData"] = "Efficiently applies potion mixes to nearby players. Does not accept Overload Mix.";

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
	energyMaxBuffer = 640;
	loopFunc = "EOTW_SolarShieldProjectorLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};
$EOTW::CustomBrickCost["brickEOTWSolarShieldProjectorData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWSolarShieldProjectorData"] = "When powered for long enough produces a large bubble shield that grants all living entities immunity to the sun.";