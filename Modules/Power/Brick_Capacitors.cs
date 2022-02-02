//Brick_Capacitors

datablock fxDTSBrickData(brickEOTWMicroCapacitorData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Micro Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 16000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMicroCapacitorData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 16 TAB "Silver" TAB 32 TAB "Copper";

datablock fxDTSBrickData(brickEOTWCapacitor1Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 64000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor1";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor1Data"] = 1.00 TAB "d36b04ff" TAB 256 TAB "Iron" TAB 144 TAB "Lead" TAB 64 TAB "Copper";

datablock fxDTSBrickData(brickEOTWCapacitor2Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Quad Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 256000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor2";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor2Data"] = 1.00 TAB "dfc47cff" TAB 256 TAB "Iron" TAB 144 TAB "Lead" TAB 64 TAB "Electrum";

datablock fxDTSBrickData(brickEOTWCapacitor3Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Quad-Quad Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 999999;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor3";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor3Data"] = 1.00 TAB "d69c6bff" TAB 256 TAB "Iron" TAB 128 TAB "Plastic" TAB 64 TAB "Energium";

function Player::EOTW_DefaultInspectLoop(%player, %brick)
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

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(2000 / $EOTW::PowerTickRate, "EOTW_DefaultInspectLoop", %brick);
}