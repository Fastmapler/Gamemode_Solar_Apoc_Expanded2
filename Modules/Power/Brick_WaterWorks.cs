datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Water Pump";
	energyGroup = "Machine";
	energyMaxBuffer = 640;
    energyWattage = 160;
	loopFunc = "EOTW_WaterPumpLoop";
    inspectFunc = "EOTW_WaterPumpInspectLoop";
	//iconName = "./Bricks/Icon_Generator";

    matterMaxBuffer = 128;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "Uses energy to pump water from deep underground.";

function Player::EOTW_WaterPumpInspectLoop(%player, %brick)
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

    %printText = %printText @ (%brick.getPower() + 0) @ "/" @ %data.energyMaxBuffer @ " EU\n";
    for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_WaterPumpInspectLoop", %brick);
}


function fxDtsBrick::EOTW_WaterPumpLoop(%obj)
{
    %data = %obj.getDatablock();
    %costPerUnit = 320;
	if (%obj.craftingPower >= %costPerUnit)
	{
		%change = %obj.changeMatter("Water", 1, "Output");
        %obj.craftingPower -= %change * %costPerUnit;
	}
    else
    {
        %change = mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.getPower());
        %obj.craftingPower += %change;
        %obj.changePower(%change * -1);
    }
}