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
$EOTW::BrickDescription["brickEOTWDebugGeneratorData"] = "Produces free power.";

function fxDtsBrick::EOTW_DebugGeneratorLoop(%obj)
{
	if (%obj.customOutput > 0)
		%obj.changePower(%obj.customOutput);
	else
		%obj.changePower(20);
}