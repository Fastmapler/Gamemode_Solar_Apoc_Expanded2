//Brick_Capacitors

datablock fxDTSBrickData(brickEOTWMicroCapacitorData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Micro Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 20000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWMicroCapacitorData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 16 TAB "Silver" TAB 32 TAB "Copper";

datablock fxDTSBrickData(brickEOTWBasicCapacitorData)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 250000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_Capacitor";
};
$EOTW::CustomBrickCost["brickEOTWBasicCapacitorData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 144 TAB "Lead" TAB 64 TAB "Copper";

datablock fxDTSBrickData(brickEOTWDoubleCapacitorData)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Dual Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 500000;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_Capacitor";
};
$EOTW::CustomBrickCost["brickEOTWDoubleCapacitorData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 144 TAB "Lead" TAB 64 TAB "Electrum";

datablock fxDTSBrickData(brickEOTWQuadCapacitorData)
{
	brickFile = "./Bricks/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Storage";
	uiName = "Quad Capacitor";
	energyGroup = "Storage";
	energyMaxBuffer = 999999;
	loopFunc = "";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_Capacitor";
};
$EOTW::CustomBrickCost["brickEOTWQuadCapacitorData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 128 TAB "Plastic" TAB 64 TAB "Energium";

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

    %printText = %printText @ (%brick.energy + 0) @ "/" @ %data.energyMaxBuffer @ " EU\n";

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(2000 / $EOTW::PowerTickRate, "EOTW_DefaultInspectLoop", %brick);
}