datablock fxDTSBrickData(brickEOTWMatterReactorData)
{
	brickFile = "./Bricks/MatterReactor.blb";
	category = "Solar Apoc";
	subCategory = "Material Processing";
	uiName = "Matter Reactor";
	//iconName = "./Bricks/Icon_Generator";
	
	energyGroup = "Machine";
	energyMaxBuffer = 12800;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 640;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 3;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWMatterReactorData"] = 1.00 TAB "7a7a7aff" TAB 384 TAB "Steel" TAB 240 TAB "Lead" TAB 64 TAB "Silver";

datablock fxDTSBrickData(brickEOTWAlloyForgeData)
{
	brickFile = "./Bricks/AlloyForge.blb";
	category = "Solar Apoc";
	subCategory = "Material Processing";
	uiName = "Alloy Forge";
	//iconName = "./Bricks/Icon_Generator";
	
	energyGroup = "Machine";
	energyMaxBuffer = 12800;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 640;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWAlloyForgeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 128 TAB "Copper" TAB 96 TAB "Glass";

datablock fxDTSBrickData(brickEOTWRefineryData)
{
	brickFile = "./Bricks/Refinery.blb";
	category = "Solar Apoc";
	subCategory = "Material Processing";
	uiName = "Refinery";
	//iconName = "./Bricks/Icon_Generator";
	
	energyGroup = "Machine";
	energyMaxBuffer = 12800;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 640;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWRefineryData"] = 1.00 TAB "7a7a7aff" TAB 384 TAB "Steel" TAB 128 TAB "Copper" TAB 64 TAB "Silver";

datablock fxDTSBrickData(brickEOTWSeperatorData)
{
	brickFile = "./Bricks/Seperator.blb";
	category = "Solar Apoc";
	subCategory = "Material Processing";
	uiName = "Seperator";
	//iconName = "./Bricks/Icon_Generator";
	
	energyGroup = "Machine";
	energyMaxBuffer = 12800;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 640;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 2;
};
$EOTW::CustomBrickCost["brickEOTWSeperatorData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Adamantine" TAB 256 TAB "Electrum" TAB 256 TAB "Rosium";

function Player::EOTW_MatterReactorInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	%data = %brick.getDatablock();
	%printText = "<color:ffffff>";
	for (%i = 0; %i < %data.matterSlots["Input"]; %i++)
	{
		%matter = %brick.Matter["Input", %i];

		if (%matter !$= "")
			%printText = %printText @ "Input " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Input " @ (%i + 1) @ ": --" @ "\n";
	}

	if (isObject(%craft = %brick.craftingProcess))
		%printText = %printText @ (%brick.energy + 0) @ " EU (" @ mFloor((%brick.craftingPower / %craft.energyCost * 100)) @ "\%)\n";
	else
		%printText = %printText @ (%brick.energy + 0) @ " EU\n";

	for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(2000 / $EOTW::PowerTickRate, "EOTW_MatterReactorInspectLoop", %brick);
}

function fxDtsBrick::EOTW_MatterReactorLoop(%obj)
{
	if (!isObject(%craft = %obj.craftingProcess))
		return;
	
	%data = %obj.getDatablock();
	
	%obj.craftingPower += mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.energy);
	
	if (%obj.craftingPower >= %obj.craftingProcess.energyCost)
	{
		%obj.craftingProcess = "";
		%obj.craftingPower = 0;
		
		for (%i = 0; %craft.input[%i] !$= ""; %i++)
			%obj.changeMatter(getField(%craft.input[%i], 0), getField(%craft.input[%i], 1) * -1, "Input", true);
		
		for (%i = 0; %craft.output[%i] !$= ""; %i++)
			%obj.changeMatter(getField(%craft.output[%i], 0), getField(%craft.output[%i], 1), "Output");
	}
}

function fxDtsBrick::EOTW_MatterReactorMatterUpdate(%obj)
{
	%data = %obj.getDatablock();
	//Check to see if we can still process a running process, otherwise check to see if we can craft something.
	if (isObject(%obj.craftingProcess))
	{
		for (%i = 0; %craftData.input[%i] !$= ""; %i++)
		{
			if (!%obj.hasMatter(getField(%craftData.input[%i], 0),getField(%craftData.input[%i], 1), "Input"))
			{
				%craftFail = true;
				break;
			}
		}
		
		for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
		{
			%output = %obj.matter["Output", %i];
			for (%k = 0; %craftData.output[%k] !$= ""; %k++)
			{
				//This long check cheks to see if there is another material occupying the output(s), or if the output is too full to output into when done
				if ((getField(%output, 0) !$= "" && getField(%output, 0) !$= getField(%craftData.output[%k], 0)) || (getField(%craftData.output[%k], 1) + getField(%output, 1) > %data.matterMaxBuffer))
				{
					%craftFail = true;
					break;
				}
			}
		}
		
		if (%craftFail)
		{
			%obj.craftingProcess = "";
			%obj.craftingPower = 0;
		}
	}
	else
	{
		for (%i = 0; %i < MatterCraftingData.getCount(); %i++)
		{
			%craftData = MatterCraftingData.getObject(%i);
			
			if (%craftData.type !$= %data.uiName)
				continue;
			
			%craftFail = false;
			
			for (%j = 0; %craftData.input[%j] !$= ""; %j++)
			{
				if (!%obj.hasMatter(getField(%craftData.input[%j], 0),getField(%craftData.input[%j], 1), "Input"))
				{
					%craftFail = true;
					break;
				}
			}
			
			if (%craftFail)
				continue;
			
			for (%j = 0; %j < %data.matterSlots["Output"]; %j++)
			{
				%output = %obj.matter["Output", %j];
				for (%k = 0; %craftData.output[%k] !$= ""; %k++)
				{
					//This long check cheks to see if there is another material occupying the output(s), or if the output is too full to output into when done
					if ((getField(%output, 0) !$= "" && getField(%output, 0) !$= getField(%craftData.output[%k], 0)) || (getField(%craftData.output[%k], 1) + getField(%output, 1) > %data.matterMaxBuffer))
					{
						%craftFail = true;
						break;
					}
				}
				
				if (%craftFail)
					break;
				
			}
			
			if (%craftFail)
				continue;
			
			%obj.craftingProcess = %craftData;
			%obj.craftingPower = 0;
		}
	}
}