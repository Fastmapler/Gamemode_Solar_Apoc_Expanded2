exec("./Tool_SurvivalKnife.cs");
exec("./Tool_CableLayer.cs");
exec("./Tool_PipeLayer.cs");
exec("./Tool_Scanner.cs");
exec("./Tool_OilPump.cs");
exec("./Tool_Pickaxes.cs");
exec("./Tool_Flasks.cs");
exec("./Tool_Syringes.cs");
exec("./Tool_Sickle.cs");
exec("./Support_DropInventoryOnDeath.cs");
//exec("./Tool_Multitool.cs");

//Blacklist specific items
function updateItemNames()
{
	$EOTW::BacklistedItems = 0;
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "BowItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "GunItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "AkimboGunItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "horseRayItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "pushBroomItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "rocketLauncherItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "spearItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "swordItem";
    
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "acidItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "BioRifleItem";

	for(%i = 0; $EOTW::BacklistedItem[%i] !$= ""; %i++)
	{
		%data = ($EOTW::BacklistedItem[%i]).getID();

        if (isObject(%data))
            %data.uiName = "";
	}
	
	createuinametable();
	transmitdatablocks();
	commandtoall('missionstartphase3');
}
schedule(0, 0, "updateItemNames");

$Game::Item::PopTime = 1000 * 60;

function ServerCmdGrantItem(%client, %data, %target)
{
    if (!%client.isSuperAdmin || !isObject(%targetPlayer = findClientByName(%target).player) || !isObject(%data))
        return;

    %item = new Item()
    {
        dataBlock = %data;
        position = %targetPlayer.getPosition();
    };

    %client.chatMessage("Granted");
}