//Oh boy this is going to be.. Interesting do to. Gotta start somewhere.

$EOTW::SaveLocation = "config/server/SAEX2/";
function EOTW_SaveData()
{
    //Save Player Data
    for (%i = 0; %i < ClientGroup.getCount(); %i++)
        EOTW_SaveData_PlayerData(ClientGroup.getObject(%i));
}

function EOTW_SaveData_PlayerData(%client)
{
    %player = %client.player;
    %saveDir = $EOTW::SaveLocation @ "/PlayerData/" @ %client.bl_id @ "/";
    
    //Save Materials
    export("$EOTW::Material" @ %client.bl_id @ "*", %saveDir @ "Materials.cs");

    //Save Player Battery
    %file = new FileObject();
    if(%file.openForWrite(%saveDir @ "Battery.cs"))
    {
        %file.writeLine("BATTERY" TAB %client.GetBatteryEnergy());
        %file.writeLine("MAXBATTERY" TAB %client.GetMaxBatteryEnergy());
    }
    %file.close();
    %file.delete();

    //Save Position & Checkpoint Position
    %file = new FileObject();
    if(%file.openForWrite(%saveDir @ "Position.cs"))
    {
        if (isObject(%player))
            %file.writeLine("POSITION" TAB %player.getTransform());
        if (isObject(%client.checkpointBrick))
            %file.writeLine("CHECKPOINT" TAB %client.checkpointBrick.getPosition());
    }
    %file.close();
    %file.delete();

    //Save Tool Data
    %file = new FileObject();
    if(%file.openForWrite(%saveDir @ "ToolData.cs") && isObject(%player))
    {
        %file.writeLine("DAMAGELEVEL" TAB %player.getDamageLevel());
        %file.writeLine("ENERGYLEVEL" TAB %player.getEnergyLevel());
        %file.writeLine("VELOCITY" TAB %player.getVelocity());
        %file.writeLine("PLAYERTYPE" TAB %player.getDataBlock().getName());
        for (%i = 0; %i < %player.getDataBlock().maxTools; %i++)
        {
            if (isObject(%tool = %player.tool[%i]))
            {
                %file.writeLine("TOOL" TAB %i TAB %tool.getName());

                if (%player.toolMag[%i] !$= "")
                    %file.writeLine("TOOLMAG" TAB %i TAB %player.toolMag[%i]);
            }
        }

        %ammoTypes = "Pistol" TAB "Machine Pistol" TAB "Revolver" TAB "Rifle" TAB "Sniper Rifle" TAB "Machine Rifle" TAB "Shotgun" TAB "Heavy Machine Gun";
        for (%i = 0; %i < getFieldCount(%ammoTypes); %i++)
        {
            %type = getField(%ammoTypes, %i);
            if (%player.toolAmmo[%type] > 0)
                %file.writeLine("TOOLAMMO" TAB %type TAB %player.toolAmmo[%type]);
        }

        //Save misc. data
    }
    %file.close();
    %file.delete();
}

function EOTW_SaveData_BrickData()
{
    %blacklist = "888888 999999 1337";
    %saveList = "Material\tEnergy\tProcessTime\tCondensatorBuffer\tstoredFuel";
    %saveList = %saveList @ "\tMatterBuffer_0\tMatterBuffer_1\tMatterBuffer_2\tMatterBuffer_3\tMatterBuffer_4";
    %saveList = %saveList @ "\tMatterInput_0\tMatterInput_1\tMatterInput_2\tMatterInput_3\tMatterInput_4";
    %saveList = %saveList @ "\tMatterOutput_0\tMatterOutput_1\tMatterOutput_2\tMatterOutput_3\tMatterOutput_4";

    deleteVariables("$EOTW::BrickData*");
    for (%j = 0; %j < MainBrickGroup.getCount(); %j++)
    {
        %group = MainBrickGroup.getObject(%j);
        %blid = %group.bl_id;

        if (hasWord(%blacklist, %blid))
            continue;

        for (%i = 0; %i < %group.getCount(); %i++)
        {
            %brick = %group.getObject(%i);
            %pos = %brick.getPosition();

            if (!%brick.isPlanted)
                continue;

            $EOTW::BrickData[%pos, 0] = %brick.getDatablock().getName();
            
            %dataCount = 0;
            for (%k = 0; %k < getFieldCount(%saveList); %k++)
            {
                %varName = getField(%saveList, %k);
                %varData = get_var_obj(%brick, %varName);
                if (%varData !$= "")
                {
                    $EOTW::BrickData[%pos, %dataCount++] = %varName TAB %varData;
                }
            }
        }
    }
    export("$EOTW::BrickData*", $EOTW::SaveLocation @ "BrickData.cs");
}

function EOTW_LoadData_BrickData()
{
    %file = new FileObject();
    %file.openForRead($EOTW::SaveLocation @ "BrickData.cs");
    while(!%file.isEOF())
    {
        %line = %file.readLine();
        %subLine1 = getSubStr(%line, 16, strPos(%line, "_") - 16);
        %subLine2 = getSubStr(%line, strPos(%line, "_") + 1, strPos(%line, "=") - strPos(%line, "_") - 2);
        %subLine3 = getSubStr(%line, strPos(%line, "="), strLen(%line));
        %eval = "$EOTW::BrickData[\"" @ %subLine1 @ "\", " @ %subLine2 @ "] " @ %subLine3;
        eval(%eval);
    }
    %file.close();
    %file.delete();
}


function EOTW_LoadData_PlayerData(%client)
{
    %player = %client.player;
    %saveDir = $EOTW::SaveLocation @ "/PlayerData/" @ %client.bl_id @ "/";

    //Load Materials
    %file = new FileObject();
    %file.openForRead(%saveDir @ "Materials.cs");
    while(!%file.isEOF())
    {
        %line = %file.readLine();
        
        //HERESY, HERESY, I DIDN'T HAVE TO TAKE THIS PATH BUT YET I DID. I COULD OF JUST USED SOME FUNCTION TO SHORTEN THIS.
        %subLen = strLen(getSubStr(%line, 0, strPos(%line, "=") - 1)) - strLen(getSubStr(%line, 0, strPos(%line, "_") + 1));
        %eval = getSubStr(%line, 0, strPos(%line, "_") + 1) @ "[\"" @ getSubStr(%line, strPos(%line, "_") + 1, %subLen) @ "\"] " @ getSubStr(%line, strPos(%line, "="), strLen(%line));
        eval(%eval);
    }
    %file.close();
    %file.delete();

    //Load Battery
    %file = new FileObject();
    %file.openForRead(%saveDir @ "Battery.cs");
    while(!%file.isEOF())
    {
        %line = %file.readLine();
        switch$ (getField(%line, 0))
        {
            case "BATTERY":
                %client.SetBatteryEnergy(getField(%line, 1));
            case "MAXBATTERY":
                %client.SetMaxBatteryEnergy(getField(%line, 1));
        }
    }
    %file.close();
    %file.delete();

    //Load Position
    %file = new FileObject();
    %file.openForRead(%saveDir @ "Position.cs");
    while(!%file.isEOF())
    {
        %line = %file.readLine();
        switch$ (getField(%line, 0))
        {
            case "POSITION":
                %client.savedSpawnTransform = getField(%line, 1);
            case "CHECKPOINT":
                initContainerRadiusSearch(getField(%line, 1), 0.1, $TypeMasks::fxBrickAlwaysObjectType);
                while(isObject(%hit = containerSearchNext()))
                {
                    if(%hit.getDataBlock().getName() $= "brickCheckpointData")
                    {
                        %client.checkpointBrick = %hit;
                        break;
                    }
                }
        }
    }
    %file.close();
    %file.delete();

    //Load Tool Data
    %file = new FileObject();
    %file.openForRead(%saveDir @ "ToolData.cs");

    %i = 0;
    while(!%file.isEOF())
    {
        %line = %file.readLine();
        set_var_obj(%client, "saved" @ %i, trim(getField(%line, 0) TAB getField(%line, 1) TAB getField(%line, 2) TAB getField(%line, 3)));
        %i++;
    }
    %file.close();
    %file.delete();
}

package EOTW_SavingLoading
{
    function GameConnection::onClientLeaveGame(%client)
	{
		EOTW_SaveData_PlayerData(%client);
		parent::onClientLeaveGame(%client);
	}
    function GameConnection::createPlayer(%client, %trans)
	{
        if (!%client.hasSpawnedOnce)
            EOTW_LoadData_PlayerData(%client);

        if (%client.savedSpawnTransform !$= "")
        {
            %trans = %client.savedSpawnTransform;
            %client.savedSpawnTransform = "";
        }
		else if (!isObject(%client.checkpointBrick))
			%trans = GetRandomSpawnLocation();
			
		Parent::createPlayer(%client, %trans);

        if (isObject(%player = %client.player))
        {
            %clearedTools = false;
            for (%i = 0; %client.saved[%i] !$= ""; %i++)
            {
                %saveData = %client.saved[%i];
                %type = getField(%saveData, 0);
                switch$ (%type)
                {
                    case "DAMAGELEVEL":
                        %player.setDamagelevel(getField(%saveData, 1));
                    case "ENERGYLEVEL":
                        %player.setEnergylevel(getField(%saveData, 1));
                    case "VELOCITY":
                        %player.setEnergylevel(getField(%saveData, 1));
                    case "PLAYERTYPE":
                        %player.changeDatablock(getField(%saveData, 1));
                    case "TOOL":
                        if (!%clearedTools)
                        {
                            %player.clearTools();
                            %clearedTools = true;
                        }
                        if (!isObject(%player.tool[getField(%saveData, 1)]))
                            %player.weaponCount++;
                        %player.tool[getField(%saveData, 1)] = getField(%saveData, 2).getID();
                        messageClient(%client, 'MsgItemPickup', '', getField(%saveData, 1), getField(%saveData, 2));
                    case "TOOLMAG":
                        %player.toolMag[getField(%saveData, 1)] = getField(%saveData, 2);
                    case "TOOLAMMO":
                        %player.toolAmmo[getField(%saveData, 1)] = getField(%saveData, 2);
                }

                %client.saved[%i] = "";
            }
        }
    }
    function fxDtsBrick::onLoadPlant(%obj, %b)
    {
        parent::onLoadPlant(%obj, %b);

        if (isObject(%obj) && $EOTW::BrickData[%obj.getPosition(), 0] !$= "" && $EOTW::BrickData[%obj.getPosition(), 0] == %obj.getDatablock().getName())
        {
            for (%i = 1; $EOTW::BrickData[%obj.getPosition(), %i] !$= ""; %i++)
            {
                %data = $EOTW::BrickData[%obj.getPosition(), %i];
                %varData = "";
                for (%j = 1; getField(%data, %j) !$= ""; %j++)
                    %varData = %varData TAB getField(%data, %j);

                %varData = trim(%varData);

                talk(%data SPC %varData);
                set_var_obj(%obj, getField(%data, 0), %varData);
            }
                
        }
    }
};
activatePackage("EOTW_SavingLoading");