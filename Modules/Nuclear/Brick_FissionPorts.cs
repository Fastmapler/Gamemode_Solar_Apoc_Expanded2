$EOTW::CustomBrickCost["brickMFRFuelPortBrick"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickMFRFuelPortBrick"] = "Takes in nuclear fissile fuel, outputs nuclear waste.";
datablock fxDTSBrickData(brickMFRFuelPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Fuel/Waste I/O";

	matterMaxBuffer = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
	inspectFunc = "EOTW_DefaultInspectLoop";

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};

$EOTW::CustomBrickCost["brickMFRCoolantPortBrick"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickMFRCoolantPortBrick"] = "Takes in coolants, outputs heated coolants.";
datablock fxDTSBrickData(brickMFRCoolantPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Coolant/Hot Coolant I/O";

	matterMaxBuffer = 50000;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
	inspectFunc = "EOTW_DefaultInspectLoop";

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};

$EOTW::CustomBrickCost["brickMFRBreederPortBrick"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickMFRBreederPortBrick"] = "Takes in specific materials, outputs neutron activated materials. Powered by placing fuel rods adjacent to reflectors.";
datablock fxDTSBrickData(brickMFRBreederPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Neutron Activator";

	matterMaxBuffer = 4;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	energyGroup = "Source";
	energyMaxBuffer = 80;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 40;
	inspectFunc = "EOTW_MatterReactorInspectLoop";

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};
