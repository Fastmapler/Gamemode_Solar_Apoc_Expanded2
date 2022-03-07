//exec("Add-Ons/Support_ShapelinesV2/server.cs");

$EOTW::CustomBrickCost["brickEOTWTurretRepeaterData"] = 1.00 TAB "7a7a7aff" TAB 288 TAB "Steel" TAB 128 TAB "Energium" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickEOTWTurretRepeaterData"] = "Quickly zaps beams at nearby enemies within sight.";
datablock fxDTSBrickData(brickEOTWTurretRepeaterData)
{
	brickFile = "./Bricks/TurretRepeater.blb";
	category = "Solar Apoc";
	subCategory = "Military";
	uiName = "Turret - Repeater";
	energyGroup = "Machine";
	energyMaxBuffer = 100;
	loopFunc = "EOTW_TurretLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";
};

function fxDtsBrick::EOTW_TurretLoop(%obj)
{
	if (getSimTime() - %obj.LastChargeLoop < 200)
		return;

	%obj.LastChargeLoop = getSimTime();

    if (isObject(%obj.turretTarget))
    {
        if (%obj.turretTarget.getState() $= "DEAD")
            %obj.turretTarget = "";
        else
        {
            %ray = firstWord(containerRaycast(%obj.getPosition(), %obj.turretTarget.getPosition(), $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType));
            if (isObject(%ray))
                %obj.turretTarget = "";
        }
    }
    
    if (!isObject(%obj.turretTarget))
    {
        initContainerRadiusSearch(%obj.getPosition(), 8, $TypeMasks::PlayerObjectType);
        while(isObject(%hit = containerSearchNext()))
        {
            if(%hit.getClassName() $= "AIPlayer" && %hit.getState() !$= "DEAD")
            {
                %ray = firstWord(containerRaycast(%obj.getPosition(), %hit.getPosition(), $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType));
                if (!isObject(%ray))
                {
                    %obj.turretTarget = %hit;
                    break;
                }
            }
        }
    }

    %costPerShot = 10;
    if (isObject(%obj.turretTarget) && %obj.getPower() >= %costPerShot)
    {
        %obj.changePower(%costPerShot * -1);
        %obj.turretTarget.addHealth(mCeil((%costPerShot * -1) / 2));
        spawnBeam(%obj.getPosition(), %obj.turretTarget.getPosition(), 1);
    }
}

function Player::EOTW_TurretInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

    //Code
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_TurretInspectLoop", %brick);
}