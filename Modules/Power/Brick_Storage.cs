datablock fxDTSBrickData(brickEOTWMicroCapacitorData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Micro Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 320;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMicroCapacitorData"] = 1.00 TAB "7a7a7aff" TAB 32 TAB "Iron" TAB 4 TAB "Silver" TAB 8 TAB "Copper";
$EOTW::BrickDescription["brickEOTWMicroCapacitorData"] = "A compact, cheap, low capacity capacitor useful for connecting long distance wires.";

datablock fxDTSBrickData(brickEOTWMicroCapacitor2xData)
{
	brickFile = "./Bricks/MicroCapacitor2x.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "2x2 Micro Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 1280;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMicroCapacitor2xData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 16 TAB "Silver" TAB 32 TAB "Copper";
$EOTW::BrickDescription["brickEOTWMicroCapacitor2xData"] = "A bigger micro capacitor to help fit symmetry.";

datablock fxDTSBrickData(brickEOTWCapacitor1Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 62500;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor1";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor1Data"] = 1.00 TAB "d36b04ff" TAB 256 TAB "Iron" TAB 96 TAB "Lead" TAB 64 TAB "Copper";
$EOTW::BrickDescription["brickEOTWCapacitor1Data"] = "Buffers large amounts of power. This one is set at 62,500 EU.";

datablock fxDTSBrickData(brickEOTWCapacitor2Data)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Quad Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 250000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Capacitor2";
};
$EOTW::CustomBrickCost["brickEOTWCapacitor2Data"] = 1.00 TAB "dfc47cff" TAB 256 TAB "Iron" TAB 144 TAB "Lead" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWCapacitor2Data"] = "Buffers huge amounts of power. This one is set at 250,000 EU.";

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
$EOTW::CustomBrickCost["brickEOTWCapacitor3Data"] = 1.00 TAB "d69c6bff" TAB 256 TAB "Steel" TAB 128 TAB "Plastic" TAB 64 TAB "Energium";
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

	for (%i = 0; %i < %data.matterSlots["Input"]; %i++)
	{
		%matter = %brick.Matter["Input", %i];

		if (%matter !$= "")
			%printText = %printText @ "Input " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Input " @ (%i + 1) @ ": --" @ "\n";
	}

	for (%i = 0; %i < %data.matterSlots["Buffer"]; %i++)
	{
		%matter = %brick.Matter["Buffer", %i];

		if (%matter !$= "")
			%printText = %printText @ "Buffer " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Buffer " @ (%i + 1) @ ": --" @ "\n";
	}

	for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_DefaultInspectLoop", %brick);
}

datablock fxDTSBrickData(brickEOTWMicroMatterTankData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Material Storage";
	uiName = "Micro Matter Tank";
    matterMaxBuffer = 80;
	matterSlots["Buffer"] = 1;
    inspectFunc = "EOTW_MatterTankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMicroMatterTankData"] = 1.00 TAB "7a7a7aff" TAB 32 TAB "Iron" TAB 16 TAB "Glass" TAB 12 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMicroMatterTankData"] = "A tiny and compact matter tank.";

datablock fxDTSBrickData(brickEOTWMicroMatterTank2xData)
{
	brickFile = "./Bricks/MicroCapacitor2x.blb";
	category = "Solar Apoc";
	subCategory = "Material Storage";
	uiName = "2x2 Micro Matter Tank";
    matterMaxBuffer = 320;
	matterSlots["Buffer"] = 1;
    inspectFunc = "EOTW_MatterTankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMicroMatterTank2xData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 64 TAB "Glass" TAB 48 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMicroMatterTank2xData"] = "A bigger micro matter tank for better symmetry.";

datablock fxDTSBrickData(brickEOTWMatterTank1Data)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Material Storage";
	uiName = "Matter Tank";
    matterMaxBuffer = 50000;
	matterSlots["Buffer"] = 1;
    inspectFunc = "EOTW_MatterTankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMatterTank1Data"] = 1.00 TAB "7a7a7aff" TAB 192 TAB "Steel" TAB 128 TAB "Glass" TAB 128 TAB "Rosium";
$EOTW::BrickDescription["brickEOTWMatterTank1Data"] = "Buffers up to 50,000u of one type of material";

datablock fxDTSBrickData(brickEOTWMatterTank12Data)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Material Storage";
	uiName = "Quad Matter Tank";
    matterMaxBuffer = 200000;
	matterSlots["Buffer"] = 1;
    inspectFunc = "EOTW_MatterTankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMatterTank12Data"] = 1.00 TAB "7a7a7aff" TAB 768 TAB "Steel" TAB 512 TAB "Glass" TAB 512 TAB "Rosium";
$EOTW::BrickDescription["brickEOTWMatterTank12Data"] = "Buffers up to 200,000u of one type of material";

datablock fxDTSBrickData(brickEOTWMatterTank2Data)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Material Storage";
	uiName = "Multi-Celled Matter Tank";
    matterMaxBuffer = 50000;
	matterSlots["Buffer"] = 4;
    inspectFunc = "EOTW_MatterTankInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMatterTank2Data"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Teflon" TAB 128 TAB "Naturum";
$EOTW::BrickDescription["brickEOTWMatterTank2Data"] = "Buffers up to 50,000u of four unique materials. Note that pipes only buffer one unique material at a time!";

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