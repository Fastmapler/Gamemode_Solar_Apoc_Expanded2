//Control Cells
$EOTW::CustomBrickCost["brickMFRCellReflectorData"] = 1.00 TAB "e8e4e2ff" TAB 384 TAB "Coal" TAB 32 TAB "Silver" TAB 64 TAB "Rosium";
$EOTW::BrickDescription["brickMFRCellReflectorData"] = "Takes in neutrons from adjacent active fuel rods to power MFR Neutron Activators.";
datablock fxDTSBrickData(brickMFRCellReflectorData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Reflector";
	notRecolorable = true;

	reqFissionPart = brickMFRReactionPlateData;
	allowReflection = true;
	powerBreeders = true;
};

$EOTW::CustomBrickCost["brickMFRCellControlRodData"] = 1.00 TAB "99958cff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickMFRCellControlRodData"] = "Allows neutrons from adjacent fuel rods to travel further. Can be disabled.";
datablock fxDTSBrickData(brickMFRCellControlRodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Control Rod";
	notRecolorable = true;

	reqFissionPart = brickMFRReactionPlateData;
};

$EOTW::CustomBrickCost["brickMFRCellFuelRodData"] = 1.00 TAB "503623ff" TAB 192 TAB "Lead" TAB 192 TAB "Steel" TAB 128 TAB "Fissile Uranium";
$EOTW::BrickDescription["brickMFRCellFuelRodData"] = "Consumes fuel from fuel ports to generate fission heat (Heat Units, HU) in adjacent parts.";
datablock fxDTSBrickData(brickMFRCellFuelRodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Fuel Rod";
	notRecolorable = true;

	fissionLoopFunc = "Fission_FuelCellLoop";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Fuel Rod";
	fuelBurn = 1;
	allowReflection = true;
};

$EOTW::CustomBrickCost["brickMFRCellFuel2RodData"] = 1.00 TAB "9e7250ff" TAB 192 TAB "Lead" TAB 192 TAB "Steel" TAB 64 TAB "Fissile Thorium";
$EOTW::BrickDescription["brickMFRCellFuel2RodData"] = "Compact fuel rod with twice the fuel burn (and thus heat).";
datablock fxDTSBrickData(brickMFRCellFuel2RodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Dual Fuel Rod";
	notRecolorable = true;

	fissionLoopFunc = "Fission_FuelCellLoop";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Fuel Rod";
	fuelBurn = 2;
	allowReflection = true;
};

$EOTW::CustomBrickCost["brickMFRCellFuel4RodData"] = 1.00 TAB "ddb389ff" TAB 192 TAB "Lead" TAB 192 TAB "Steel" TAB 32 TAB "Fissile Americium";
$EOTW::BrickDescription["brickMFRCellFuel4RodData"] = "Extremely compact fuel rod with quadruple the fuel burn (and thus heat).";
datablock fxDTSBrickData(brickMFRCellFuel4RodData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Control Cells";
	uiName = "Quad Fuel Rod";
	notRecolorable = true;

	fissionLoopFunc = "Fission_FuelCellLoop";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Fuel Rod";
	fuelBurn = 4;
	allowReflection = true;
};

function fxDtsBrick::Fission_FuelCellLoop(%obj)
{
	%data = %obj.getDatablock();

	%fission = %obj.fissionParent;
	%hull = %fission.hullBrick;

	if (%data.fuelBurn > 0)
	{
		%parts = %fission.GetAdjacentParts(%obj);

		for (%i = 0; %i < %fission.getCount(); %i++)
		{
			%port = %fission.getObject(%i);
			%portData = %port.getDatablock();
			if (%portData.getName() !$= "brickMFRFuelPortBrick" || !isObject(%matter = getMatterType(getField(%port.matter["Input", 0], 0))) || %matter.fissionPower <= 0)
				continue;

			%change = %port.ChangeMatter(%matter.name, %data.fuelBurn * -1, "Input");
			%totalHeat = %change * %matter.fissionPower * -1;
			if (%totalHeat > 0)
			{
				%port.ChangeMatter("Nuclear Waste", mRound(%change * -1 * %matter.fissionWasteRate), "Output");
				break;
			}
		}

		if (%totalHeat <= 0)
			return;
		
		//Check for reflectance and possible heat targets
		for (%i = 0; %i < getWordCount(%parts); %i++)
		{
			%part = getWord(%parts, %i);
			%partData = %part.getDatablock();

			if (%partData.allowReflection)
				%totalHeat += %data.fuelBurn;

			if (%partData.maxHeatCapacity > 0)
				%heatTargets = trim(%heatTargets SPC %part);

			if (%partData.powerBreeders)
			{
				//Not very well optimized.. (Looping over the same list in a loop looping over said list)
				for (%j = 0; %j < %fission.getCount(); %j++)
				{
					%brick = %fission.getObject(%j);
					if (%brick.getDataBlock().getName() $= "brickMFRBreederPortBrick" && isObject(%craft = %brick.craftingProcess))
					{
						%brick.changePower(1);
						break;
					}
				}
					
			}
		}

		if (getWordCount(%heatTargets) > 0)
		{
			for (%i = 0; %i < getWordCount(%heatTargets); %i++)
				getWord(%heatTargets, %i).changeHeat(%totalHeat / getWordCount(%heatTargets));
		}
		else
			%hull.changeHeat(%totalHeat);
	}
}

//Heat Sinks
$EOTW::CustomBrickCost["brickMFRCellHeatSinkBasicData"] = 1.00 TAB "b8b3aaff" TAB 96 TAB "Steel" TAB 128 TAB "Plastic";
$EOTW::BrickDescription["brickMFRCellHeatSinkBasicData"] = "Attempts to push 60 HU/tick to coolant.";
datablock fxDTSBrickData(brickMFRCellHeatSinkBasicData)
{
	brickFile = "./Bricks/MFRHeatSink.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Basic Heat Sink";
	notRecolorable = true;

	fissionLoopFunc = "Fission_HeatSinkTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 0;
	selfHeatPushRate = 60;
	adjacentheatPushRate = 0;
};

$EOTW::CustomBrickCost["brickMFRCellHeatSinkSuperData"] = 1.00 TAB "1f568cff" TAB 192 TAB "Steel" TAB 256 TAB "Plastic" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickMFRCellHeatSinkSuperData"] = "Attempts to push 120 HU/tick to coolant.";
datablock fxDTSBrickData(brickMFRCellHeatSinkSuperData)
{
	brickFile = "./Bricks/MFRHeatSink.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Super Heat Sink";
	notRecolorable = true;

	fissionLoopFunc = "Fission_HeatSinkTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 0;
	selfHeatPushRate = 120;
	adjacentheatPushRate = 0;
};

$EOTW::CustomBrickCost["brickMFRCellHeatSinkComponentData"] = 1.00 TAB "007c3fff" TAB 96 TAB "Steel" TAB 128 TAB "Plastic" TAB 64 TAB "Rosium";
$EOTW::BrickDescription["brickMFRCellHeatSinkComponentData"] = "Attempts to push 40 HU/tick in adjacent parts to coolant.";
datablock fxDTSBrickData(brickMFRCellHeatSinkComponentData)
{
	brickFile = "./Bricks/MFRHeatSink.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Component Heat Sink";
	notRecolorable = true;

	fissionLoopFunc = "Fission_HeatSinkTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 0;
	selfHeatPushRate = 0;
	adjacentheatPushRate = 40;
};

$EOTW::CustomBrickCost["brickMFRCellHeatSinkReactorData"] = 1.00 TAB "931f23ff" TAB 96 TAB "Steel" TAB 128 TAB "Plastic" TAB 64 TAB "Electrum";
$EOTW::BrickDescription["brickMFRCellHeatSinkReactorData"] = "Attempts to pull 50 HU/tick from reactor, then pushes 50 HU/tick to coolant.";
datablock fxDTSBrickData(brickMFRCellHeatSinkReactorData)
{
	brickFile = "./Bricks/MFRHeatSink.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Reactor Heat Sink";
	notRecolorable = true;

	fissionLoopFunc = "Fission_HeatSinkTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Heat Sink";
	maxHeatCapacity = 10000;
	reactorHeatPullRate = 50;
	selfHeatPushRate = 50;
	adjacentheatPushRate = 0;
};

$EOTW::CustomBrickCost["brickMFRCellHeatSinkOverclockedData"] = 1.00 TAB "e2af13ff" TAB 192 TAB "Steel" TAB 128 TAB "Energium" TAB 128 TAB "Naturum";
$EOTW::BrickDescription["brickMFRCellHeatSinkOverclockedData"] = "Attempts to pull 360 HU/tick from reactor, then pushes 200 HU/tick to coolant.";
datablock fxDTSBrickData(brickMFRCellHeatSinkOverclockedData)
{
	brickFile = "./Bricks/MFRHeatSink.blb";
	category = "Nuclear";
	subCategory = "Heat Sinks";
	uiName = "Overclocked Heat Sink";
	notRecolorable = true;

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

$EOTW::CustomBrickCost["brickMFRCellHeatExchangerBasicData"] = 1.00 TAB "b8b3aaff" TAB 96 TAB "Steel" TAB 128 TAB "Teflon";
$EOTW::BrickDescription["brickMFRCellHeatExchangerBasicData"] = "Tries to equalize heat up to 120 HU in each adjacent part to itself, and then 40 HU with the reactor.";
datablock fxDTSBrickData(brickMFRCellHeatExchangerBasicData)
{
	brickFile = "./Bricks/MFRHeatExchanger.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Basic Heat Exchanger";
	notRecolorable = true;

	fissionLoopFunc = "Fission_HeatExchangerTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 25000;
	adjcaentTransferRate = 120;
	reactorTransferRate = 40;
};

$EOTW::CustomBrickCost["brickMFRCellHeatExchangerSuperData"] = 1.00 TAB "1f568cff" TAB 192 TAB "Steel" TAB 256 TAB "Teflon" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickMFRCellHeatExchangerSuperData"] = "Tries to equalize heat up to 240 HU in each adjacent part to itself, and then 80 HU with the reactor.";
datablock fxDTSBrickData(brickMFRCellHeatExchangerSuperData)
{
	brickFile = "./Bricks/MFRHeatExchanger.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Super Heat Exchanger";
	notRecolorable = true;

	fissionLoopFunc = "Fission_HeatExchangerTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 25000;
	adjcaentTransferRate = 240;
	reactorTransferRate = 80;
};

$EOTW::CustomBrickCost["brickMFRCellHeatExchangerComponentData"] = 1.00 TAB "007c3fff" TAB 96 TAB "Steel" TAB 128 TAB "Teflon" TAB 64 TAB "Rosium";
$EOTW::BrickDescription["brickMFRCellHeatExchangerComponentData"] = "Tries to equalize heat up to 360 HU in each adjacent part to itself.";
datablock fxDTSBrickData(brickMFRCellHeatExchangerComponentData)
{
	brickFile = "./Bricks/MFRHeatExchanger.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Component Heat Exchanger";
	notRecolorable = true;
	
	fissionLoopFunc = "Fission_HeatExchangerTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 25000;
	adjcaentTransferRate = 360;
	reactorTransferRate = 0;
};

$EOTW::CustomBrickCost["brickMFRCellHeatExchangerReactorData"] = 1.00 TAB "931f23ff" TAB 96 TAB "Steel" TAB 128 TAB "Teflon" TAB 64 TAB "Electrum";
$EOTW::BrickDescription["brickMFRCellHeatExchangerReactorData"] = "Tries to equalize heat up to 720 HU with the reactor.";
datablock fxDTSBrickData(brickMFRCellHeatExchangerReactorData)
{
	brickFile = "./Bricks/MFRHeatExchanger.blb";
	category = "Nuclear";
	subCategory = "Heat Exchangers";
	uiName = "Reactor Heat Exchanger";
	notRecolorable = true;

	fissionLoopFunc = "Fission_HeatExchangerTick";
	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Exchanger";
	maxHeatCapacity = 25000;
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

$EOTW::CustomBrickCost["brickMFRCellCoolantBasicData"] = 1.00 TAB "1c496bff" TAB 128 TAB "Water" TAB 64 TAB "Copper";
$EOTW::BrickDescription["brickMFRCellCoolantBasicData"] = "Heat capacitor ranked up to 100,000 HU.";
datablock fxDTSBrickData(brickMFRCellCoolantBasicData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Coolant Cells";
	uiName = "Basic Coolant Cell";
	notRecolorable = true;

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Coolant Cell";
	maxHeatCapacity = 100000;
};

$EOTW::CustomBrickCost["brickMFRCellCoolantSuperData"] = 1.00 TAB "507582ff" TAB 128 TAB "Coolant" TAB 64 TAB "Copper";
$EOTW::BrickDescription["brickMFRCellCoolantSuperData"] = "Heat capacitor ranked up to 300,000 HU.";
datablock fxDTSBrickData(brickMFRCellCoolantSuperData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Coolant Cells";
	uiName = "Super Coolant Cell";
	notRecolorable = true;

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Coolant Cell";
	maxHeatCapacity = 300000;
};

$EOTW::CustomBrickCost["brickMFRCellCoolantUltraData"] = 1.00 TAB "9ab6b5ff" TAB 128 TAB "Cryostablizer" TAB 64 TAB "Copper";
$EOTW::BrickDescription["brickMFRCellCoolantUltraData"] = "Heat capacitor ranked up to 600,000 HU.";
datablock fxDTSBrickData(brickMFRCellCoolantUltraData)
{
	brickFile = "./Bricks/MFRCell.blb";
	category = "Nuclear";
	subCategory = "Coolant Cells";
	uiName = "Ultra Coolant Cell";
	notRecolorable = true;

	reqFissionPart = brickMFRReactionPlateData;
	ComponentType = "Coolant Cell";
	maxHeatCapacity = 600000;
};

//$EOTW::CustomBrickCost["brickMFRCellCoolantOmegaData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
//$EOTW::BrickDescription["brickMFRCellCoolantOmegaData"] = "Heat capacitor ranked up to 999,999 HU.";
//datablock fxDTSBrickData(brickMFRCellCoolantOmegaData)
//{
//	brickFile = "./Bricks/MFRCell.blb";
//	category = "Nuclear";
//	subCategory = "Coolant Cells";
//	uiName = "Omega Coolant Cell";
//	notRecolorable = true;
//
//	reqFissionPart = brickMFRReactionPlateData;
//	ComponentType = "Coolant Cell";
//	maxHeatCapacity = 999999;
//};