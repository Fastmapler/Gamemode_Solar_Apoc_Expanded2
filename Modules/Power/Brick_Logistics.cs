datablock fxDTSBrickData(brickEOTWTransmissionNodeData)
{
	brickFile = "./Bricks/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Logistics";
	uiName = "Transmission Node Down";
	energyGroup = "Transmission";
	energyMaxBuffer = 0;
	loopFunc = "";
	inspectFunc = "EOTW_DefaultInspectLoop";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/MicroCapacitor";
};
$EOTW::CustomBrickCost["brickEOTWTransmissionNodeData"] = 1.00 TAB "7a7a7aff" TAB 32 TAB "Iron" TAB 4 TAB "Silver" TAB 8 TAB "Copper";
$EOTW::BrickDescription["brickEOTWTransmissionNodeData"] = "A transmission node (down)";

//transmission node up
//need model or figure out how to relocate wire to top of node


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

function EOTW_TrashBinLoop(%obj)
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

    %printText = %printText @ "\c6Upward Push Whitelist: " @ strReplace(%brick.splitterFilter["Up"] @ "\n", "\t", ", ");
    %printText = %printText @ "\c6Downward Push Whitelist: " @ strReplace(%brick.splitterFilter["Down"] @ "\n", "\t", ", ");

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_SplitterInspectLoop", %brick);
}

function EOTW_SplitterUpdate(%obj)
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

        %dir = getRandom(0, 1);

        if (%dir == 1)
        {
            if (%obj.splitterFilter["Up"] $= "" || getFieldIndex(%obj.splitterFilter["Up"], getField(%matterChange, 0)) > -1)
                if (getFieldIndex(%obj.splitterFilter["Down"], getField(%matterChange, 0)) == -1 || %downBrick.GetMatter(getField(%matterChange, 0), "Buffer") >= %downBrick.getDataBlock().matterMaxBuffer)
                    %totalMatterChange += %upBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
            %totalEnergyChange += %upBrick.ChangePower(%energyChange);
        }
        else
        {
            if (%obj.splitterFilter["Down"] $= "" || getFieldIndex(%obj.splitterFilter["Down"], getField(%matterChange, 0)) > -1)
                if (getFieldIndex(%obj.splitterFilter["Up"], getField(%matterChange, 0)) == -1 || %upBrick.GetMatter(getField(%matterChange, 0), "Buffer") >= %upBrick.getDataBlock().matterMaxBuffer)
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
            if (%obj.splitterFilter["Down"] $= "" || getFieldIndex(%obj.splitterFilter["Down"], getField(%matterChange, 0)) > -1)
                if (getFieldIndex(%obj.splitterFilter["Up"], getField(%matterChange, 0)) == -1 || %upBrick.GetMatter(getField(%matterChange, 0), "Buffer") >= %upBrick.getDataBlock().matterMaxBuffer)
                    %totalMatterChange += %downBrick.ChangeMatter(getField(%matterChange, 0), getField(%matterChange, 1), "Buffer");
            %totalEnergyChange += %downBrick.ChangePower(%energyChange);
        }
        else
        {
            if (%obj.splitterFilter["Up"] $= "" || getFieldIndex(%obj.splitterFilter["Up"], getField(%matterChange, 0)) > -1)
                if (getFieldIndex(%obj.splitterFilter["Down"], getField(%matterChange, 0)) == -1 || %downBrick.GetMatter(getField(%matterChange, 0), "Buffer") >= %downBrick.getDataBlock().matterMaxBuffer)
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

function ServerCmdAF(%client, %filterDir, %mat1, %mat2, %mat3, %mat4) { ServerCmdAddFilter(%client, %filterDir, %mat1, %mat2, %mat3, %mat4); }
function ServerCmdAddFilter(%client, %filterDir, %mat1, %mat2, %mat3, %mat4)
{
    if (!isObject(%player = %client.player))
        return;

    %material = trim(%mat1 SPC %mat2 SPC %mat3 SPC %mat4);

    if (%material $= "" || %filterDir $= "")
	{
		%client.chatMessage("Usage: /AddFilter <up (u)/down (d)> <material>");
		return;
	}

    %eye = %player.getEyePoint();
	%dir = %player.getEyeVector();
	%for = %player.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
	{
        if (!%hit.getDatablock().isSplitter)
        {
            %client.chatMessage("This command can only be applied to splitter bricks.");
			return;
        }
		if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
		{
			if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}
        if (!isObject(%matter = getMatterType(%material)))
        {
            %client.chatMessage("No material type by the name of \"" @ %material @ "\" exists.");
			return;
        }

        switch$ (%filterDir)
        {
            case "up" or "u":
                %hit.splitterFilter["Up"] = trim(%hit.splitterFilter["Up"] TAB %matter.name);
                %client.chatMessage("\c6Added " @ %matter.name @ " to Push Up direction whitelist.");
            case "down" or "d":
                %hit.splitterFilter["Down"] = trim(%hit.splitterFilter["Down"] TAB %matter.name);
                %client.chatMessage("\c6Added " @ %matter.name @ " to Push Down direction whitelist.");
            default:
                %client.chatMessage("Direction \"" @ %filterDir @ "\" not found.");
        }
    }
}

function ServerCmdRF(%client, %filterDir, %mat1, %mat2, %mat3, %mat4) { ServerCmdRemoveFilter(%client, %filterDir, %mat1, %mat2, %mat3, %mat4); }
function ServerCmdRemoveFilter(%client, %filterDir, %mat1, %mat2, %mat3, %mat4)
{
    if (!isObject(%player = %client.player))
        return;

    %material = trim(%mat1 SPC %mat2 SPC %mat3 SPC %mat4);

    if (%material $= "" || %filterDir $= "")
	{
		%client.chatMessage("Usage: /RemoveFilter <up (u)/down (d)> <material>");
		return;
	}

    %eye = %player.getEyePoint();
	%dir = %player.getEyeVector();
	%for = %player.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
	{
        if (!%hit.getDatablock().isSplitter)
        {
            %client.chatMessage("This command can only be applied to splitter bricks.");
			return;
        }
		if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
		{
			if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}
        if (!isObject(%matter = getMatterType(%material)))
        {
            %client.chatMessage("No material type by the name of \"" @ %material @ "\" exists.");
			return;
        }

        switch$ (%filterDir)
        {
            case "up" or "u":
                %idx = getFieldIndex(%hit.splitterFilter["Up"], %matter.name);
                if (%idx > -1)
                {
                    %hit.splitterFilter["Up"] = removeField(%hit.splitterFilter["Up"], %idx);
                    %client.chatMessage("\c6Removed " @ %matter.name @ " from Push Up direction whitelist.");
                }
                else
                {
                    %client.chatMessage("Did not find " @ %matter.name @ " in Push Up direction whitelist.");
                }
            case "down" or "d":
                %idx = getFieldIndex(%hit.splitterFilter["Down"], %matter.name);
                if (%idx > -1)
                {
                    %hit.splitterFilter["Down"] = removeField(%hit.splitterFilter["Down"], %idx);
                    %client.chatMessage("\c6Removed " @ %matter.name @ " from Push Down direction whitelist.");
                }
                else
                {
                    %client.chatMessage("Did not find " @ %matter.name @ " in Push Down direction whitelist.");
                }
            default:
                %client.chatMessage("Direction \"" @ %filterDir @ "\" not found.");
        }
    }
}

function ServerCmdGF(%client) { ServerCmdGetFilter(%client); }
function ServerCmdGetFilter(%client)
{
    if (!isObject(%player = %client.player))
        return;

    %eye = %player.getEyePoint();
	%dir = %player.getEyeVector();
	%for = %player.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
	{
        if (!%hit.getDatablock().isSplitter)
        {
            %client.chatMessage("This command can only be applied to splitter bricks.");
			return;
        }

        %client.chatMessage("\c6Filter List for Splitter (Blank means all allowed):");
        %client.chatMessage("\c6Upward Push: " @ strReplace(%hit.splitterFilter["Up"], "\t", ", "));
        %client.chatMessage("\c6Downward Push: " @ strReplace(%hit.splitterFilter["Downward"], "\t", ", "));
    }
}
