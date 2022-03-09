datablock fxDTSBrickData(brickMFRFuelPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Fuel/Waste I/O";

	matterMaxBuffer = 50000;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	reqFissionPart = brickMFRHullData;
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

	reqFissionPart = brickMFRHullData;
};

datablock fxDTSBrickData(brickMFRBreederPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Isotope/Breeded I/O";

	matterMaxBuffer = 50000;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	reqFissionPart = brickMFRHullData;
};
