datablock fxDTSBrickData(brickMFRDisplayPanelData)
{
	brickFile = "./Bricks/MFRDisplay.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "Display Panel";

	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};