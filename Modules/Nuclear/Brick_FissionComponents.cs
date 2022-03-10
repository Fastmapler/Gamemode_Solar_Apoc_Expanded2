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
	ComponentType = "Fuel Rod";
	fuelBurn = 1;
};

datablock fxDTSBrickData(brickMFRCellFuel2RodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Dual Fuel Rod";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Fuel Rod";
	fuelBurn = 2;
};

datablock fxDTSBrickData(brickMFRCellFuel4RodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Quad Fuel Rod";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Fuel Rod";
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
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 0;
	selfHeatPushRate = 60;
	adjacentheatPushRate = 0;
};

datablock fxDTSBrickData(brickMFRCellHeatSinkSuperData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Super Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 0;
	selfHeatPushRate = 120;
	adjacentheatPushRate = 0;
};

datablock fxDTSBrickData(brickMFRCellHeatSinkComponentData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Component Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 0;
	selfHeatPushRate = 0;
	adjacentheatPushRate = 40;
};

datablock fxDTSBrickData(brickMFRCellHeatSinkReactorData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Reactor Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 50;
	selfHeatPushRate = 50;
	adjacentheatPushRate = 0;
};

datablock fxDTSBrickData(brickMFRCellHeatSinkOverclockedData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Overclocked Heat Sink";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 360;
	selfHeatPushRate = 200;
	adjacentHeatPushRate = 0;
};

function fxDtsBrick::Fission_HeatSinkTick(%obj)
{
	%data = %obj.getDatablock();

	%fission = %obj.fissionParent;
	%hull = %fission.hullBrick;

	if (%data.reactorHeatPullRate > 0)
		%obj.changeHeat(%hull.changeHeat(%data.reactorHeatPullRate * -1) * -1);

	if (%data.selfHeatPushRate > 0)
		%hull.queuedHeat += %obj.changeHeat(%data.selfHeatPushRate * -1) * -1;

	if (%data.adjacentHeatPushRate > 0)
	{
		%parts = %fission.GetAdjacentParts(%obj);

		for (%i = 0; %i < getWordCount(%parts); %i++)
		{
			%part = getWord(%parts, %i);
			%partData = %part.getDatablock();
			if (%partData.maxHeatCapacity > 0)
			{
				%hull.queuedHeat += %obj.changeHeat(mFloor(%data.adjacentHeatPushRate / getWordCount(%parts)) * -1) * -1;
			}
		}
	}
	
}

//Heat Exchangers
datablock fxDTSBrickData(brickMFRCellHeatExchangerBasicData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Basic Heat Exchanger";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 2500;
	adjcaentTransferRate = 120;
	reactorTransferRate = 40;
};

datablock fxDTSBrickData(brickMFRCellHeatExchangerSuperData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Super Heat Exchanger";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 2500;
	adjcaentTransferRate = 240;
	reactorTransferRate = 80;
};

datablock fxDTSBrickData(brickMFRCellHeatExchangerComponentData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Component Heat Exchanger";
	
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 2500;
	adjcaentTransferRate = 360;
	reactorTransferRate = 0;
};

datablock fxDTSBrickData(brickMFRCellHeatExchangerReactorData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Reactor Heat Exchanger";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 2500;
	adjcaentTransferRate = 0;
	reactorTransferRate = 720;
};

//Coolant Cells
datablock fxDTSBrickData(brickMFRCellCoolantBasicData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Coolant Cells";
	uiName = "Basic Coolant Cell";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Coolant Cell";
	maxHeatCapacity = 100000;
};

datablock fxDTSBrickData(brickMFRCellCoolantSuperData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Coolant Cells";
	uiName = "Super Coolant Cell";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Coolant Cell";
	maxHeatCapacity = 300000;
};

datablock fxDTSBrickData(brickMFRCellCoolantUltraData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Coolant Cells";
	uiName = "Ultra Coolant Cell";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Coolant Cell";
	maxHeatCapacity = 600000;
};

datablock fxDTSBrickData(brickMFRCellCoolantOmegaData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Coolant Cells";
	uiName = "Omega Coolant Cell";

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Coolant Cell";
	maxHeatCapacity = 999999;
};