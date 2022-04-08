$EOTW::CustomBrickCost["brickMFRHeatPlatingData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickMFRHeatPlatingData"] = "Increases the reactor's max heat by 4000 HU.";
datablock fxDTSBrickData(brickMFRHeatPlatingData)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Heat Plating";

	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};