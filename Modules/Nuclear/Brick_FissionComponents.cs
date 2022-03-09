//Control Cells
datablock fxDTSBrickData(brickMFRCellReflectorData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Reflector";

	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellControlRodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Control Rod";

	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellFuelRodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Fuel Rod";

	reqFissionPart = brickMFRReactionPlateData;
	fuelBurn = 1;
};

datablock fxDTSBrickData(brickMFRCellFuel2RodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Dual Fuel Rod";

	reqFissionPart = brickMFRReactionPlateData;
	fuelBurn = 2;
};

datablock fxDTSBrickData(brickMFRCellFuel4RodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Quad Fuel Rod";

	reqFissionPart = brickMFRReactionPlateData;
	fuelBurn = 4;
};

//Heat Sinks
datablock fxDTSBrickData(brickMFRCellHeatSinkBasicData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Basic Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellHeatSinkSuperData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Super Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellHeatSinkComponentData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Component Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellHeatSinkReactorData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Reactor Heat Sink";
	reqFissionPart = brickMFRReactionPlateData;

};

datablock fxDTSBrickData(brickMFRCellHeatSinkOverclockedData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Overclocked Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
};

//Heat Exchangers
datablock fxDTSBrickData(brickMFRCellHeatExchangerBasicData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Basic Heat Exchanger";

	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellHeatExchangerSuperData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Super Heat Exchanger";

	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellHeatExchangerComponentData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Component Heat Exchanger";
	
	reqFissionPart = brickMFRReactionPlateData;
};

datablock fxDTSBrickData(brickMFRCellHeatExchangerReactorData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Reactor Heat Exchanger";

	reqFissionPart = brickMFRReactionPlateData;
};