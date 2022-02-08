datablock fxDTSBrickData(brickEOTWMicroCapacitorData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Storage Device";
	uiName = "Micro Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 1280;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMicroCapacitorData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 16 TAB "Silver" TAB 32 TAB "Copper";
$EOTW::BrickDescription["brickEOTWMicroCapacitorData"] = "A compact low capacity capacitor that is only 1x1 big.";

datablock fxDTSBrickData(brickEOTWCapacitor1Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Storage Device";
	uiName = "Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 62500;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor1";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor1Data"] = 1.00 TAB "d36b04ff" TAB 256 TAB "Iron" TAB 144 TAB "Lead" TAB 64 TAB "Copper";
$EOTW::BrickDescription["brickEOTWCapacitor1Data"] = "Buffers large amounts of power. This one is set at 62,500 EU.";

datablock fxDTSBrickData(brickEOTWCapacitor2Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Storage Device";
	uiName = "Quad Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 250000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor2";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor2Data"] = 1.00 TAB "dfc47cff" TAB 256 TAB "Iron" TAB 144 TAB "Lead" TAB 64 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWCapacitor2Data"] = "Buffers huge amounts of power. This one is set at 250,000 EU.";

datablock fxDTSBrickData(brickEOTWCapacitor3Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Storage Device";
	uiName = "Quad-Quad Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 999999;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor3";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor3Data"] = 1.00 TAB "d69c6bff" TAB 256 TAB "Iron" TAB 128 TAB "Plastic" TAB 64 TAB "Energium";
$EOTW::BrickDescription["brickEOTWCapacitor3Data"] = "Buffers insane amounts of power. This one is set at 999,999 EU.";

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

    %printText = %printText @ (%brick.getPower()) @ "/" @ %data.energyMaxBuffer @ " EU\n";

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_DefaultInspectLoop", %brick);
}

datablock fxDTSBrickData(brickEOTWMatterTank1Data)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Storage Device";
	uiName = "Matter Tank";
    matterMaxBuffer = 50000;
	matterSlots["Buffer"] = 1;
    inspectFunc = "EOTW_MatterTankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMatterTank1Data"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWMatterTank1Data"] = "Buffers up to 50,000u of one type of material";

datablock fxDTSBrickData(brickEOTWMatterTank2Data)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Storage Device";
	uiName = "Celled Matter Tank";
    matterMaxBuffer = 50000;
	matterSlots["Buffer"] = 4;
    inspectFunc = "EOTW_MatterTankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMatterTank2Data"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWMatterTank2Data"] = "A matter tank with four slots for four unique materials.";

function Player::EOTW_MatterTankInspectLoop(%player, %brick)
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

    for (%i = 0; %i < %data.matterSlots["Buffer"]; %i++)
	{
		%matter = %brick.Matter["Buffer", %i];

		if (%matter !$= "")
			%printText = %printText @ "Buffer " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Buffer " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_MatterTankInspectLoop", %brick);
}