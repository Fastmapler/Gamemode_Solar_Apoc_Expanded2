datablock fxDTSBrickData(brickEOTWSplitterData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Logistics";
	uiName = "Normal Splitter";
	energyGroup = "Storage";
	energyMaxBuffer = 16000;
    matterMaxBuffer = 1024;
	matterSlots["Buffer"] = 1;
	loopFunc = "EOTW_SplitterUpdate";
    inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";

    isSplitter = true;
};
$EOTW::CustomBrickCost["brickEOTWSplitterData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";

datablock fxDTSBrickData(brickEOTWInertSplitterData : brickEOTWSplitterData)
{
	uiName = "Inert Splitter";
	loopFunc = "";
};
$EOTW::CustomBrickCost["brickEOTWInertSplitterData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";

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
        %energyChange = mCeil(%obj.getPower() / 3);
        %matterChange = getField(%buffer, 0) TAB mCeil(getField(%buffer, 1) / 3);
        %totalEnergyChange += %downBrick.ChangePower(%energyChange);
        %totalEnergyChange += %upBrick.ChangePower(%energyChange);
        %totalMatterChange += %downBrick.ChangeMatter(getField(%buffer, 0), %matterChange, "Buffer");
        %totalMatterChange += %upBrick.ChangeMatter(getField(%buffer, 0), %matterChange, "Buffer");
    }
    else if (isObject(%downBrick))
    {
        %energyChange = mCeil(%obj.getPower() / 2);
        %matterChange = getField(%buffer, 0) TAB mCeil(getField(%buffer, 1) / 2);
        %totalEnergyChange += %downBrick.ChangePower(%energyChange);
        %totalMatterChange += %downBrick.ChangeMatter(getField(%buffer, 0), %matterChange, "Buffer");
    }
    else if (isObject(%upBrick))
    {
        %energyChange = mCeil(%obj.getPower() / 2);
        %matterChange = getField(%buffer, 0) TAB mCeil(getField(%buffer, 1) / 2);
        %totalEnergyChange += %upBrick.ChangePower(%energyChange);
        %totalMatterChange += %upBrick.ChangeMatter(getField(%buffer, 0), %matterChange, "Buffer");
    }
    %obj.ChangePower(%totalEnergyChange * -1);
    %obj.ChangeMatter(getField(%buffer, 0), %totalEnergyChange * -1, "Buffer");
}