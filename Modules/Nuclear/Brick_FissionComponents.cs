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

	fissionLoopFunc = "Fission_HeatSinkTick";
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

	fissionLoopFunc = "Fission_HeatSinkTick";
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

	fissionLoopFunc = "Fission_HeatSinkTick";
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

	fissionLoopFunc = "Fission_HeatSinkTick";
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

	fissionLoopFunc = "Fission_HeatSinkTick";
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
		talk("Parts: " @ %parts);
		for (%i = 0; %i < getWordCount(%parts); %i++)
		{
			%part = getWord(%parts, %i);
			%partData = %part.getDatablock();
			if (%partData.maxHeatCapacity > 0)
			{
				%hull.queuedHeat += %part.changeHeat(mFloor(%data.adjacentHeatPushRate / getWordCount(%parts)) * -1) * -1;
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

	fissionLoopFunc = "Fission_HeatExchangerTick";
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

	fissionLoopFunc = "Fission_HeatExchangerTick";
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
	
	fissionLoopFunc = "Fission_HeatExchangerTick";
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

	fissionLoopFunc = "Fission_HeatExchangerTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 2500;
	adjcaentTransferRate = 0;
	reactorTransferRate = 720;
};

function fxDtsBrick::Fission_HeatExchangerTick(%obj)
{
	%data = %obj.getDatablock();

	%fission = %obj.fissionParent;
	%hull = %fission.hullBrick;
	%hullData = %hull.getDatablock();

	if (%data.adjcaentTransferRate > 0)
	{
		%parts = %fission.GetAdjacentParts(%obj);

		for (%i = 0; %i < getWordCount(%parts); %i++)
		{
			%part = getWord(%parts, %i);
			%partData = %part.getDatablock();
			if (%partData.maxHeatCapacity > 0)
			{
				%percentDifference = (%part.fissionHeat / %partData.maxHeatCapacity) - (%obj.fissionHeat / %data.maxHeatCapacity);
				%average = (%partData.maxHeatCapacity + %data.maxHeatCapacity) / 2;
				%toalChange = mRound(mClamp(%percentDifference * %average * 0.5, %data.adjcaentTransferRate * -1, %data.adjcaentTransferRate));
				%obj.changeHeat(%part.changeHeat(%toalChange * -1) * -1);
			}
		}
	}

	if (%data.reactorTransferRate > 0)
	{
		%percentDifference = (%hull.fissionHeat / %hullData.maxHeatCapacity) - (%obj.fissionHeat / %data.maxHeatCapacity);
		%average = (%hullData.maxHeatCapacity + %data.maxHeatCapacity) / 2;
		%toalChange = mRound(mClamp(%percentDifference * %average * 0.5, %data.adjcaentTransferRate * -1, %data.adjcaentTransferRate));
		%obj.changeHeat(%hull.changeHeat(%toalChange * -1) * -1);
	}
}

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