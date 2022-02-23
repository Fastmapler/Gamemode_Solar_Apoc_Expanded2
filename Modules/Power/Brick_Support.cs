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
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType | $Typemasks::PlayerObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 2)), %mask, %obj);
	
	if (isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "Player")
	{
		%obj.changePower(%hit.ChangeBatteryEnergy(%obj.getPower()) * -1);
		if (isObject(%client = %hit.client))
			%client.centerPrint(%hit.GetBatteryText(), 1);
	}
}
