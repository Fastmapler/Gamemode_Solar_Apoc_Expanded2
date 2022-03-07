datablock AudioProfile(AlloyForgeLoopSound)
{
    filename    = "./Sounds/AlloyForgeLoop.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(MatterReactorLoopSound)
{
    filename    = "./Sounds/MatterReactorLoop.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(RefineryLoopSound)
{
    filename    = "./Sounds/RefineryLoop.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(SeperatorLoopSound)
{
    filename    = "./Sounds/SeperatorLoop.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(BreweryLoopSound)
{
    filename    = "./Sounds/BreweryLoop.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock fxDTSBrickData(brickEOTWAlloyForgeData)
{
	brickFile = "./Bricks/AlloyForge.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Alloy Forge";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/AlloyForge";
	
	energyGroup = "Machine";
	energyMaxBuffer = 200;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 20;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	loopNoise = AlloyForgeLoopSound;

	//port info
	portGoToEdge["PowerIn"] = true;
	portGoToEdge["MatterIn"] = true;
	portHeight["PowerIn"] = "0.5";
	portHeight["MatterIn"] = "0.2";
};
$EOTW::CustomBrickCost["brickEOTWAlloyForgeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 128 TAB "Glass" TAB 64 TAB "Copper";
$EOTW::BrickDescription["brickEOTWAlloyForgeData"] = "Uses different metals and materials to create alloys.";

datablock fxDTSBrickData(brickEOTWMatterReactorData)
{
	brickFile = "./Bricks/MatterReactor.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Matter Reactor";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MatterReactor";
	
	energyGroup = "Machine";
	energyMaxBuffer = 200;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 20;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 3;
	matterSlots["Output"] = 1;

	loopNoise = MatterReactorLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWMatterReactorData"] = 1.00 TAB "7a7a7aff" TAB 384 TAB "Steel" TAB 144 TAB "Lead" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWMatterReactorData"] = "Takes in various materials to produce chemicals.";

datablock fxDTSBrickData(brickEOTWRefineryData)
{
	brickFile = "./Bricks/Refinery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Refinery";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Refinery";
	
	energyGroup = "Machine";
	energyMaxBuffer = 200;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 20;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	loopNoise = RefineryLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWRefineryData"] = 1.00 TAB "7a7a7aff" TAB 384 TAB "Steel" TAB 144 TAB "Lead" TAB 128 TAB "Rosium";
$EOTW::BrickDescription["brickEOTWRefineryData"] = "Refines inputted materials into a potentially more useful material.";

datablock fxDTSBrickData(brickEOTWSeperatorData)
{
	brickFile = "./Bricks/Seperator.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Seperator";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/Seperator";
	
	energyGroup = "Machine";
	energyMaxBuffer = 200;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 20;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 2;

	loopNoise = SeperatorLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWSeperatorData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Adamantine" TAB 256 TAB "Electrum" TAB 256 TAB "Rosium";
$EOTW::BrickDescription["brickEOTWSeperatorData"] = "Electrically seperates specific materials into core elements.";

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
		%printText = %printText @ (%brick.getPower() + 0) @ " EU (" @ mFloor((%brick.craftingPower / %craft.energyCost * 100)) @ "\%)\n";
	else
		%printText = %printText @ (%brick.getPower() + 0) @ " EU\n";

	for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_MatterReactorInspectLoop", %brick);
}

function fxDtsBrick::EOTW_MatterReactorLoop(%obj)
{
	if (!isObject(%craft = %obj.craftingProcess))
		return;

	%data = %obj.getDatablock();

	if (isObject(%data.loopNoise) && getSimTime() - %obj.lastLoopNoise >= 500)
	{
		%obj.lastLoopNoise = getSimTime();
		ServerPlay3D(%data.loopNoise, %obj.getPosition());
	}
	
	%change = mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.getPower());
	%obj.craftingPower += %change;
	%obj.changePower(%change * -1);
	
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
	if (isObject(%craftData = %obj.craftingProcess))
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
				if ((getField(%output, 0) !$= "" && getField(%output, 0) !$= getField(%craftData.output[%k], 0) && %craftData.output[%k + 1] $= "") || (getField(%craftData.output[%k], 1) + getField(%output, 1) > %data.matterMaxBuffer))
				{
					%craftFail = true;
					break;
				}
				else if (getField(%output, 0) $= getField(%craftData.output[%k], 0))
					break;
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
					if ((getField(%output, 0) !$= "" && getField(%output, 0) !$= getField(%craftData.output[%k], 0) && %craftData.output[%k + 1] $= "") || (getField(%craftData.output[%k], 1) + getField(%output, 1) > %data.matterMaxBuffer))
					{
						%craftFail = true;
						break;
					}
					else if (getField(%output, 0) $= getField(%craftData.output[%k], 0))
						break;
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

datablock fxDTSBrickData(brickEOTWBreweryData)
{
	brickFile = "./Bricks/Brewery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Brewery";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MatterReactor";
	
	energyGroup = "Machine";
	energyMaxBuffer = 200;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 10;
	inspectFunc = "EOTW_MatterReactorInspectLoop";
	
	matterMaxBuffer = 2048;
	matterSlots["Input"] = 4;
	matterSlots["Output"] = 1;

	loopNoise = BreweryLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWBreweryData"] = 1.00 TAB "7a7a7aff" TAB 288 TAB "Steel" TAB 128 TAB "Rosium" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWBreweryData"] = "Brews potion fluid from the combination of various materials.";