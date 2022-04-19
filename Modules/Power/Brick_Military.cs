//exec("Add-Ons/Support_ShapelinesV2/server.cs");
datablock AudioProfile(TurretRepeaterFireSound)
{
    filename    = "./Sounds/TurretRepeaterFire.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(TurretHeavyFireSound)
{
    filename    = "./Sounds/TurretHeavyFire.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(TeslaCoilFireSound)
{
    filename    = "./Sounds/TeslaCoilFire.wav";
    description = AudioClosest3d;
    preload = true;
};

$EOTW::CustomBrickCost["brickEOTWTurretRepeaterData"] = 1.00 TAB "ff7700ff" TAB 288 TAB "Steel" TAB 128 TAB "Energium" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickEOTWTurretRepeaterData"] = "Quickly zaps beams at nearby enemies within sight.";
datablock fxDTSBrickData(brickEOTWTurretRepeaterData)
{
	brickFile = "./Bricks/TurretRepeater.blb";
	category = "Solar Apoc";
	subCategory = "Military";
	uiName = "Turret - Repeater";
	energyGroup = "Machine";
	energyMaxBuffer = 180;
	loopFunc = "EOTW_TurretLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Icons/TurretRepeater";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";

    projectile = "";
    attackCost = 18;
    attackCooldown = 100;
};

$EOTW::CustomBrickCost["brickEOTWTurretHeavyData"] = 1.00 TAB "ff0000ff" TAB 288 TAB "Steel" TAB 128 TAB "Naturum" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickEOTWTurretHeavyData"] = "Fires explosive rounds at nearby enemies within sight.";
datablock fxDTSBrickData(brickEOTWTurretHeavyData)
{
	brickFile = "./Bricks/TurretHeavy.blb";
	category = "Solar Apoc";
	subCategory = "Military";
	uiName = "Turret - Heavy";
	energyGroup = "Machine";
	energyMaxBuffer = 250;
	loopFunc = "EOTW_TurretLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Icons/TurretHeavy";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";

    projectile = rocketLauncherProjectile;
    attackCost = 200;
    attackCooldown = 1000;
};

function fxDtsBrick::EOTW_TurretLoop(%obj)
{
    %range = 8;
    %data = %obj.getDataBlock();

	if (getSimTime() - %obj.LastChargeLoop < %data.attackCooldown)
		return;

	%obj.LastChargeLoop = getSimTime();

    if (isObject(%obj.turretTarget))
    {
        if (%obj.turretTarget.getState() $= "DEAD" || vectorDist(%obj.getPosition(), %obj.turretTarget.getPosition()) > %range)
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
        initContainerRadiusSearch(%obj.getPosition(), %range, $TypeMasks::PlayerObjectType);
        while(isObject(%hit = containerSearchNext()))
        {
            if(%hit.getClassName() $= "AIPlayer" && %hit.getState() !$= "DEAD" && %hit.getDatablock().hType $= "enemy")
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

    %costPerShot = getMax(%data.attackCost + 0, 2);
    if (isObject(%obj.turretTarget) && %obj.getPower() >= %costPerShot)
    {
        if (isObject(%data.Projectile))
        {
            %projectile = %data.Projectile;
            %spread = 0.001;

            %vector = VectorNormalize(vectorSub(%obj.turretTarget.getPosition(), %obj.getPosition()));
            %velocity = VectorScale(%vector, %projectile.muzzleVelocity);
            %velocity = vectorAdd(%velocity, %obj.turretTarget.getVelocity());
            %x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
            %y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
            %z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
            %mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
            %velocity = MatrixMulVector(%mat, %velocity);

            %p = new (Projectile)()
            {
                dataBlock = %projectile;
                initialVelocity = %velocity;
                initialPosition = %obj.getPosition();
                sourceObject = %obj;
                sourceSlot = %slot;
                client = %obj.client;
            };
            MissionCleanup.add(%p);

            ServerPlay3D(TurretHeavyFireSound,%obj.getPosition());
        }
        else
        {
            ServerPlay3D(TurretRepeaterFireSound,%obj.getPosition());
            %obj.turretTarget.addHealth(mCeil((%costPerShot * -1) / 2));
            spawnBeam(%obj.getPosition(), %obj.turretTarget.getPosition(), 1);

            //Chain Hit
            %pos = %obj.turretTarget.getPosition();
            initContainerRadiusSearch(%pos, %range / 2, $TypeMasks::PlayerObjectType);
            while(isObject(%newHit = containerSearchNext()))
            {
                if (%newHit.getID() == %obj.turretTarget.getID())
                    continue;
                    
                if(%newHit.getClassName() $= "AIPlayer" && %newHit.getState() !$= "DEAD" && %newHit.getDatablock().hType $= "enemy")
                {
                    %ray = firstWord(containerRaycast(%pos, %newHit.getPosition(), $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType, %obj.turretTarget));
                    if (!isObject(%ray))
                    {
                        %newHit.addHealth(mCeil((%costPerShot * -1) / 2));
                        spawnBeam(%obj.turretTarget.getPosition(), %pos, 1);
                        break;
                    }
                }
            }
        }
        %obj.changePower(%costPerShot * -1);
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

$EOTW::CustomBrickCost["brickEOTWTeslaCoilData"] = 1.00 TAB "00ffffff" TAB 256 TAB "Electrum" TAB 256 TAB "Rosium" TAB 128 TAB "Dielectrics";
$EOTW::BrickDescription["brickEOTWTeslaCoilData"] = "Stuns nearby enemies and deals minor damage.";
datablock fxDTSBrickData(brickEOTWTeslaCoilData)
{
	brickFile = "./Bricks/TurretRepeater.blb";
	category = "Solar Apoc";
	subCategory = "Military";
	uiName = "Tesla Coil";
	energyGroup = "Machine";
	energyMaxBuffer = 200;
	loopFunc = "EOTW_TeslaCoilLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

	portGoToEdge["PowerOut"] = true;
	portHeight["PowerOut"] = "0.0";

    attackCost = 100;
    attackCooldown = 2000;
};

function fxDtsBrick::EOTW_TeslaCoilLoop(%obj)
{
    %range = 7;
    %data = %obj.getDataBlock();

	if (getSimTime() - %obj.LastChargeLoop < %data.attackCooldown || %obj.getPower() < %data.attackCost)
		return;

	%obj.LastChargeLoop = getSimTime();

    initContainerRadiusSearch(%obj.getPosition(), %range, $TypeMasks::PlayerObjectType);
    while(isObject(%hit = containerSearchNext()))
    {
        if(%hit.getClassName() $= "AIPlayer" && %hit.getState() !$= "DEAD" && %hit.getDatablock().hType $= "enemy")
        {
            swolMelee_stunPlayer(%hit,1,1900,1);
            %hit.addHealth(-15);
            spawnBeam(%obj.getPosition(), %hit.getPosition(), 1);

            if (%hitCount < 1)
            {
                ServerPlay3D(TeslaCoilFireSound,%obj.getPosition());
                %obj.changePower(%data.attackCost * -1);
            }

            %hitCount++;
        }
    }
}

$EOTW::CustomBrickCost["brickEOTWLandmineData"] = 1.00 TAB "7a7a7aff" TAB 64 TAB "Plastic" TAB 48 TAB "Steel" TAB 8 TAB "Plutonium";
$EOTW::BrickDescription["brickEOTWLandmineData"] = "A reusable very-mini-mini-nuke landmine. Requires 8 Rocket Fuel per use.";
datablock fxDTSBrickData(brickEOTWLandmineData)
{
	brickFile = "base/data/bricks/rounds/2x2disc.blb";
	category = "Solar Apoc";
	subCategory = "Military";
	uiName = "Landmine";
	energyGroup = "Machine";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";

    matterMaxBuffer = 256;
	matterSlots["Input"] = 1;
};

function fxDTSBrick::forceSpawnExplosion(%obj, %projectileData, %scale, %client)
{
	%WB = %obj.getWorldBox ();
	%wbX = getWord (%WB, 0);
	%wbY = getWord (%WB, 1);
	%wbZ = getWord (%WB, 2);
	%wbXSize = getWord (%WB, 3) - %wbX;
	%wbYSize = getWord (%WB, 4) - %wbY;
	%wbZSize = getWord (%WB, 5) - %wbZ;
	if (%wbXSize < 1.05)
	{
		%wbX += %wbXSize / 2;
		%wbXSize = 0;
	}
	if (%wbYSize < 1.05)
	{
		%wbY += %wbYSize / 2;
		%wbYSize = 0;
	}
	if (%wbZSize < 0.65)
	{
		%wbZ += %wbZSize / 2;
		%wbZSize = 0;
	}
	%pos = %wbX + (getRandom () * %wbXSize) SPC %wbY + (getRandom () * %wbYSize) SPC %wbZ + (getRandom () * %wbZSize);
	%p = new Projectile ("")
	{
		dataBlock = %projectileData;
		initialVelocity = "0 0 1";
		initialPosition = %pos;
		sourceClient = %client;
		sourceObject = %obj;
		client = %client;
	};
	if (!isObject (%p))
		return;

	MissionCleanup.add (%p);
	%p.setScale (%scale SPC %scale SPC %scale);
	%p.spawnBrick = %obj;
	%p.explode();
    return %p;
}

package EOTW_Landmines
{
    function Armor::onCollision(%this, %obj, %col, %vec, %speed)
    {
        Parent::onCollision(%this, %obj, %col, %vec, %speed);

        %cost = 8;
        %cooldown = 1000;
        if (%obj.getClassName() $= "AIPlayer" && %obj.getState() !$= "DEAD" && %col.getDatablock().getName() $= "brickEOTWLandmineData" && getSimTime() - %col.lastExplosion > %cooldown && %col.GetMatter("Rocket Fuel", "Input") >= %cost)
        {
            %proj = %col.forceSpawnExplosion(nukeProjectile, 0.1, %col.getGroup().client);
            if (isObject(%proj))
            {
                %col.disappear(%cooldown / 1000);
                %col.lastExplosion = getSimTime();
                %col.ChangeMatter("Rocket Fuel", %cost * -1, "Input");
            }
        }
    }
};
activatePackage("EOTW_Landmines");