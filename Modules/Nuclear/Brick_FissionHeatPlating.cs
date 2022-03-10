datablock fxDTSBrickData(brickMFRHeatPlatingData)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Heat Plating";

	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};