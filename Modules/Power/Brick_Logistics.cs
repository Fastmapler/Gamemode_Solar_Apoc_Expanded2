datablock fxDTSBrickData(brickEOTWTrashBinData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Logistics";
	uiName = "Trash Bin";
	energyGroup = "Storage";
	energyMaxBuffer = 999999;
    matterMaxBuffer = 999999;
	matterSlots["Input"] = 1;
	loopFunc = "EOTW_TrashBinLoop";
    inspectFunc = "";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWTrashBinData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 256 TAB "Granite";
$EOTW::BrickDescription["brickEOTWTrashBinData"] = "_Permanently_ destroys any inputted energy or materials.";

function fxDtsBrick::EOTW_TrashBinLoop(%obj)
{
    %obj.energy = 0;
    %obj.matter["Input", 0] = "";
}

datablock fxDTSBrickData(brickEOTWSplitterData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Logistics";
	uiName = "Normal Splitter";
	energyGroup = "Storage";
	energyMaxBuffer = 640;
    matterMaxBuffer = 1024;
	matterSlots["Buffer"] = 1;
	loopFunc = "EOTW_SplitterUpdate";
    inspectFunc = "EOTW_SplitterInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";

    isSplitter = true;
};
$EOTW::CustomBrickCost["brickEOTWSplitterData"] = 1.00 TAB "7a7a7aff" TAB 32 TAB "Electrum" TAB 32 TAB "Rosium" TAB 32 TAB "Copper";
$EOTW::BrickDescription["brickEOTWSplitterData"] = "Equally splits its energy and buffered material into splitters above and below.";

datablock fxDTSBrickData(brickEOTWInertSplitterData : brickEOTWSplitterData)
{
	uiName = "Inert Splitter";
	loopFunc = "";
};
$EOTW::CustomBrickCost["brickEOTWInertSplitterData"] = 1.00 TAB "7a7a7aff" TAB 32 TAB "Electrum" TAB 32 TAB "Rosium" TAB 256 TAB "Granite";
$EOTW::BrickDescription["brickEOTWInertSplitterData"] = "Can take in energy and materials from splitters, but won't split them again.";

function Player::EOTW_SplitterInspectLoop(%player, %brick)
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
    for (%i = 0; %i < %data.matterSlots["Buffer"]; %i++)
	{
		%matter = %brick.Matter["Buffer", %i];

		if (%matter !$= "")
			%printText = %printText @ "Buffer " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Buffer " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_SplitterInspectLoop", %brick);
}

function fxDtsBrick::EOTW_SplitterUpdate(%obj)
{
    if (isObject(%downBrick = %obj.getDownBrick(0)))
        if(!%downBrick.getDatablock().isSplitter)
            %downBrick = "";
    if (isObject(%upBrick = %obj.getUpBrick(0)))
        if(!%upBrick.getDatablock().isSplitter)
            %upBrick = "";

    %buffer = %obj.matter["Buffer", 0];
    if (isObject(%downBrick) && isObject(%upBrick))
    {
        %energyChange = mCeil(%obj.getPower() / 2);
        %matterChange = getField(%buffer, 0) TAB mCeil(getField(%buffer, 1) / 2);

        //%dir == 1 means upBrick
        %dir = getRandom(0, 1);

        if (%dir == 1)
        {
            if (%obj.splitterFilter[%dir] $= "" || getFieldIndex(%obj.splitterFilter[%dir], getField(%matterChange, 0)) > -1)
                %totalMatterChange += %upBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
            %totalEnergyChange += %upBrick.ChangePower(%energyChange);
        }
        else
        {
            if (%obj.splitterFilter[%dir] $= "" || getFieldIndex(%obj.splitterFilter[%dir], getField(%matterChange, 0)) > -1)
                %totalMatterChange += %downBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
            %totalEnergyChange += %downBrick.ChangePower(%energyChange);
        }
       
        %obj.ChangePower(%totalEnergyChange * -1);
        %obj.ChangeMatter(getField(%matterChange, 0), %totalMatterChange * -1, "Buffer");
        %totalEnergyChange = 0;
        %totalMatterChange = 0;

        %buffer = %obj.matter["Buffer", 0];
        %energyChange = mCeil(%obj.getPower() / 1);
        %matterChange = getField(%buffer, 0) TAB mCeil(getField(%buffer, 1) / 1);

        if (%dir == 1)
        {
            if (%obj.splitterFilter[%dir] $= "" || getFieldIndex(%obj.splitterFilter[%dir], getField(%matterChange, 0)) > -1)
                %totalMatterChange += %downBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
            %totalEnergyChange += %downBrick.ChangePower(%energyChange);
        }
        else
        {
            if (%obj.splitterFilter[%dir] $= "" || getFieldIndex(%obj.splitterFilter[%dir], getField(%matterChange, 0)) > -1)
                %totalMatterChange += %upBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
            %totalEnergyChange += %upBrick.ChangePower(%energyChange);
        }
    }
    else if (isObject(%downBrick))
    {
        %energyChange = mCeil(%obj.getPower() / 1);
        %matterChange = getField(%buffer, 0) TAB mCeil(getField(%buffer, 1) / 1);
        %totalEnergyChange += %downBrick.ChangePower(%energyChange);
        %totalMatterChange += %downBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
    }
    else if (isObject(%upBrick))
    {
        %energyChange = mCeil(%obj.getPower() / 1);
        %matterChange = getField(%buffer, 0) TAB mCeil(getField(%buffer, 1) / 1);
        %totalEnergyChange += %upBrick.ChangePower(%energyChange);
        %totalMatterChange += %upBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
    }
    %obj.ChangePower(%totalEnergyChange * -1);
    %obj.ChangeMatter(getField(%matterChange, 0), %totalMatterChange * -1, "Buffer");
}