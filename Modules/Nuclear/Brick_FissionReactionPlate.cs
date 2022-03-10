datablock fxDTSBrickData(brickMFRReactionPlateData)
{
	brickFile = "./Bricks/MFRReactionPlate.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Reaction Plate";

	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};