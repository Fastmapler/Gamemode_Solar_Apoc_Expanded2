datablock fxDTSBrickData(brickEOTWDebugGeneratorData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Debug Generator";
	energyGroup = "Source";
	energyMaxBuffer = 6400;
	loopFunc = "EOTW_DebugGeneratorLoop";
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWDebugGeneratorData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";

function fxDtsBrick::EOTW_DebugGeneratorLoop(%obj)
{
	%db = %obj.getDatablock();
	
	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;
		
	if (%obj.energy < %db.energyMaxBuffer)
		%obj.energy += 20;
}

datablock fxDTSBrickData(brickEOTWDebugStationData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Debug Machine";
	energyGroup = "Machine";
	energyMaxBuffer = 6400;
	loopFunc = "EOTW_DebugStationLoop";
	//iconName = "./Bricks/Icon_HealStation";
};
$EOTW::CustomBrickCost["brickEOTWDebugStationData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";

function fxDtsBrick::EOTW_DebugStationLoop(%obj)
{
	%db = %obj.getDatablock();
	
	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;
		
	if (%obj.energy >= 400)
	{
		%obj.energy -= 400;
		talk("Cycle" SPC %obj);
	}
}
