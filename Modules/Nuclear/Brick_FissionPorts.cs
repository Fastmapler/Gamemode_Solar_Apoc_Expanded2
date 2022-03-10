datablock fxDTSBrickData(brickMFRFuelPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Fuel/Waste I/O";

	matterMaxBuffer = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};

datablock fxDTSBrickData(brickMFRCoolantPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Coolant/Hot Coolant I/O";

	matterMaxBuffer = 50000;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};

datablock fxDTSBrickData(brickMFRBreederPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Isotope/Breeded I/O";

	matterMaxBuffer = 4;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};
