datablock AudioProfile(baadsniperRifleFireSound)
{
	filename    = "./Sounds/Fire/sniperRifle.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock DebrisData(baadsniperRifleClipDebris)
{
	shapeFile = "./shapes/weapons/sniperRifleMag.dts";
	lifetime = 2.0;
	minSpinSpeed = 700.0;
	maxSpinSpeed = 800.0;
	elasticity = 0.5;
	friction = 0.1;
	numBounces = 3;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
  doColorShift = false;
  colorShiftColor = "0.4 0.4 0.4 1.000";
};
datablock ExplosionData(baadsniperRifleClipExplosion)
{
	debris 					= baadsniperRifleClipDebris;
	debrisNum 				= 1;
	debrisNumVariance 		= 0;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 1;
	debrisVelocityVariance 	= 0;
};
datablock ProjectileData(baadsniperRifleClipProjectile)
{
	explosion = baadsniperRifleClipExplosion;
};

datablock ProjectileData(baadsniperRifleProjectile : gunProjectile)
{
    projectileShapeName = "base/data/shapes/empty.dts";
    directDamage        = 0;//8;

    impactImpulse       = 500;
    verticalImpulse     = 450;
    explosion           = QuietGunExplosion;
    particleEmitter     = ""; //bulletTrailEmitter;

    muzzleVelocity      = 200;
    velInheritFactor    = 1;

    armingDelay         = 00;
    lifetime            = 4000;
    fadeDelay           = 3500;
    bounceElasticity    = 0.5;
    bounceFriction      = 0.20;
    isBallistic         = false;
    gravityMod = 0.0;
};

//////////
// item //
//////////

datablock ItemData(baadsniperRifleItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/sniperRifle.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "AP - Sniper Rifle";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 3;
   ammotype = "Sniper Rifle";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = baadsniperRifleImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponHardImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
//////////////// 
datablock ShapeBaseImageData(baadsniperRifleImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/sniperRifle.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 -0.13 -0.085";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = baadsniperRifleItem;
   ammo = " ";
   projectile = baadsniperRifleProjectile;
   projectileType = Projectile;
   
   raycastWeaponRange = 400;
   raycastWeaponTargets =
						 $TypeMasks::PlayerObjectType |    //AI/Players
						 $TypeMasks::StaticObjectType |    //Static Shapes
						 $TypeMasks::TerrainObjectType |    //Terrain
						 $TypeMasks::VehicleObjectType |    //Terrain
						 $TypeMasks::FXBrickObjectType;    //Bricks
   raycastExplosionProjectile = baadsniperRifleProjectile;
   raycastExplosionBrickSound = "";
   raycastExplosionPlayerSound = "";
   raycastDirectDamage = 50; //10
   raycastDirectDamageType = $DamageType::BAADPistol;
   raycastSpreadAmt = 0.0001;
   raycastSpreadCount = 1;
 //  raycastTracerProjectile = BAADPistolTracerProjectile;
 //  raycastTracerShape = hl2TracerShapeData;
 //  raycastTracerSize = 1;
//   raycastTracerColor = "0.8 0.5 0.0 1";
   raycastFromMuzzle = false;

   casing = BAADBigRifleDebris;
   shellExitDir        = "0.25 -0.05 0.5";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 8.0;   
   shellVelocity       = 10.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = baadsniperRifleItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
   stateName[0]                     = "Activate";
   stateTimeoutValue[0]             = 0.75;
   stateScript[0]                   = "onAmmoCheck";
   stateSequence[0]                 = "yup";
   stateTransitionOnTimeout[0]      = "AmmoCheckReady";
   stateSound[0]                    = "";

   stateName[1]                     = "Ready";
   stateTransitionOnTriggerDown[1]  = "Fire";
   stateTransitionOnNoAmmo[1]       = "Empty";
   stateAllowImageChange[1]         = true;
   stateSequence[1]                 = "root";

   stateName[2]                     = "Fire";
   stateTimeoutValue[2]             = 0.1;
   stateTransitionOnTimeout[2]      = "Smoke";
   stateFire[2]                     = true;
   stateAllowImageChange[2]         = false;
   stateSequence[2]                 = "Fire";
   stateScript[2]                   = "onFire";
   stateWaitForTimeout[2]           = true;
   stateEmitter[2]                  = advSmallBulletFireEmitter;
   stateEmitterTime[2]              = 0.05;
   stateEmitterNode[2]              = "muzzleNode";
   stateEjectShell[2]               = false;

   stateName[3]                     = "Smoke";
   stateEmitter[3]                  = advSmallBulletSmokeEmitter;
   stateEmitterTime[3]              = 0.05;
   stateEmitterNode[3]              = "muzzleNode";
   stateTimeoutValue[3]             = 0.32;
   stateTransitionOnTimeout[3]    = "AmmoCheck";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTimeoutValue[4]             = 0.05;
   stateSequence[4]                 = "untrig";
   stateTransitionOnTimeout[4]      = "Empty";

   stateName[5]                     = "AmmoCheck";
   stateTransitionOnTriggerUp[5]      = "untrig";
   stateAllowImageChange[5]         = true;
   stateScript[5]                   = "onAmmoCheck";

   stateName[6]                     = "Reload";
   stateTransitionOnTimeout[6]      = "ReloadA";
   stateTimeoutValue[6]             = 0.01;
   stateAllowImageChange[6]         = true;
   stateScript[6]                   = "onReloadStart";

   stateName[7]                     = "ReloadReady";
   stateTransitionOnTimeout[7]      = "Ready";
   stateTimeoutValue[7]             = 0.1;
   stateAllowImageChange[7]         = true;
   stateScript[7]                   = "onReload";

   stateName[8]                     = "Empty";
   stateTransitionOnTriggerDown[8]  = "EmptyFireA";
   stateAllowImageChange[8]         = true;
   stateScript[8]                   = "onEmpty";
   stateTransitionOnAmmo[8]         = "Reload";
   //stateSequence[8]                 = "noammo";

   stateName[9]                     = "EmptyFireA";
   stateTransitionOnTriggerUp[9]    = "EmptyFireB";
   stateScript[9]                   = "onEmptyFire";
   stateTimeoutValue[9]             = 0.05;
   stateAllowImageChange[9]         = false;
   stateWaitForTimeout[9]           = false;
   stateSound[9]                    = baadEmptySound;
   stateSequence[9]                 = "fire";
   
   stateName[10]                     = "EmptyFireB";
   stateTransitionOnTimeout[10]  = "Empty";
   stateAllowImageChange[10]         = false;
   stateTimeoutValue[10]             = 0.05;
   stateWaitForTimeout[10]           = false;
   stateSequence[10]                 = "untrig";
   
   stateName[11]                    = "ReloadA";
   stateTransitionOnTimeout[11]  = "ReloadB";
   stateAllowImageChange[11]         = true;
   stateEjectShell[11]               = true;
   stateSequence[11]                 = "boltopen";
   stateScript[11]                   = "OnReloadA";
   stateTimeoutValue[11]             = 0.225;
   
   stateName[12]                     = "ReloadB";
   stateTransitionOnTimeout[12]  = "Wait";
   stateAllowImageChange[12]         = true;
   stateSequence[12]                 = "magout";
   stateScript[12]                   = "OnReloadB";
   stateTimeoutValue[12]             = 0.6;
     
   stateName[13]                     = "ReloadC";
   stateTransitionOnTimeout[13]  = "ReloadD";
   stateAllowImageChange[13]         = true;
   stateSequence[13]                 = "magin";
   stateScript[13]                   = "OnReloadC";
   stateTimeoutValue[13]             = 0.4;
   
   stateName[14]                     = "ReloadD";
   stateTransitionOnTimeout[14]  = "ReloadReady";
   stateAllowImageChange[14]         = true;
   stateSequence[14]                 = "boltclose";
   stateScript[14]                   = "OnReloadD";
   stateTimeoutValue[14]             = 0.3;

   stateName[15]                     = "untrig";
   stateTimeoutValue[15]             = 0.05;
   stateSequence[15]                 = "untrig";
   stateTransitionOnTimeout[15]      = "BoltOpen";
   stateTransitionOnNoAmmo[15]       = "Cycle";
   stateSound[15]                    = "";
   
   stateName[16]                     = "AmmoCheckReady";
   stateTransitionOnNoAmmo[16]       = "Empty";
   stateScript[16]                   = "onAmmoCheck";
   stateTransitionOnTimeout[16]      = "Ready";
   stateAllowImageChange[16]         = true;
   
   stateName[17]                     = "BoltOpen";
   stateTimeoutValue[17]             = 0.25;
   stateEjectShell[17]               = true;
   stateTransitionOnTimeout[17]      = "BoltClose";
   stateScript[17]                   = "OnReloadA";
   stateSequence[17]                 = "BoltOpen";
   
   stateName[18]                     = "BoltClose";
   stateTimeoutValue[18]             = 0.3;
   stateTransitionOnTimeout[18]      = "Ready";
   stateScript[18]                   = "OnReloadD";
   stateSequence[18]                 = "BoltClose";

   stateName[19]                     = "CockEmpty";
   stateTimeoutValue[19]             = 0.15;
   stateTransitionOnTimeout[19]      = "CycleEmpty";
   stateScript[19]                   = "onCock";
   stateSequence[19]                 = "cock";

   stateName[20]                     = "CycleEmpty";
   stateTimeoutValue[20]             = 0.35;
   stateTransitionOnTriggerUp[20]      = "Empty";
   
    stateName[21]                     = "Wait";
   stateTimeoutValue[21]             = 0.35;
   stateTransitionOnTriggerUp[21]      = "ReloadC";
   
   stateName[22]                     = "WaitA";
   stateTimeoutValue[22]             = 0.35;
   stateTransitionOnTriggerUp[22]      = "BoltClose";
};


  ////// ammo display functions
function baadsniperRifleImage::onMount( %this, %obj, %slot )
{
parent::onMount(%this,%obj,%slot); 
hl2DisplayAmmo(%this,%obj,%slot,0);
schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function baadsniperRifleImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function baadsniperRifleImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function baadsniperRifleImage::onReloadStart( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function baadsniperRifleImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function baadsniperRifleImage::onEmptyFire( %this, %obj, %slot )
{
   %obj.playThread(2, plant);
}

function baadsniperRifleImage::onEmpty(%this,%obj,%slot)
{
   if( $hl2Weapons::Ammo && %obj.toolAmmo[%this.item.ammotype] < 1 )
   {
      return;
   }

   if(%obj.toolMag[%obj.currTool] < 1)
   {
      serverCmdLight(%obj.client);
   }
}

function baadsniperRifleImage::onFire( %this, %obj, %slot )
{
   if(%obj.getDamagePercent() >= 1)
   return;
   parent::onFire( %this, %obj, %slot );
   %obj.toolMag[%obj.currTool] -= 1;
   if(%obj.toolMag[%obj.currTool] < 1)
   {
      %obj.toolMag[%obj.currTool] = 0;
      %obj.setImageAmmo(0,0);
   }
   hl2DisplayAmmo(%this,%obj,%slot);
   %obj.spawnExplosion(advLittleRecoilProjectile,"1.5 1.5 1.5");
	%obj.playThread(2, shiftRight);
	%obj.playThread(3, shiftLeft);
   serverPlay3D( baadsniperRifleFireSound,%obj.getPosition());
}

function baadsniperRifleImage::OnReloadA(%this, %obj, %slot) 
{
   schedule(getRandom(500,600),0,serverPlay3D,BAADShellRifle @ getRandom(1,6) @ Sound,%obj.getPosition());
   %obj.playThread(2,plant);
   %obj.playThread(3,shiftLeft);
   schedule(0, 0, serverPlay3D, baadBoltOpenSound, %obj.getPosition());
}

function baadsniperRifleImage::OnReloadB(%this, %obj, %slot) 
{
   %obj.schedule(0, "playThread", "2", "wrench");
   %obj.schedule(0, "playThread", "3", "shiftRight");
          schedule(0, 0, serverPlay3D, baadReload2Sound, %obj.getPosition());
          schedule(getRandom(250,350),0,serverPlay3D,BAADMagDrop @ getRandom(1,3) @ Sound,%obj.getPosition());
          %up = %obj.getUpVectorHack();
          %forward = %obj.getEyeVectorHack();
          %p = new projectile()
          {
              datablock = "baadsniperRifle" @ (%obj.toolAmmo[%obj.currTool] <= 0 ? "Clip" : "") @ "Projectile";
              initialPosition = vectorAdd(%obj.getSlotTransform(0),vectorRelativeShift(%forward,%up,"0.4 0 -0.5"));
          };
          %p.explode();
}

function baadsniperRifleImage::OnReloadC(%this, %obj, %slot)
{
   %obj.schedule(0, "playThread", "2", "shiftUp");
   schedule(0, 0, serverPlay3D, baadReload11Sound, %obj.getPosition());
}

function baadsniperRifleImage::OnReloadD(%this, %obj, %slot)
{
   %obj.playThread(2,plant);
   %obj.playThread(3,shiftRight);
   schedule(0, 0, serverPlay3D, baadBoltCloseSound, %obj.getPosition());
}

function baadsniperRifleImage::onRaycastDamage(%this,%obj,%slot,%col,%pos,%normal,%shotVec,%crit)
{
	%damageType = $DamageType::Direct;
	if(%this.raycastDirectDamageType)
		%damageType = %this.raycastDirectDamageType; 
	
	%scale = getWord(%obj.getScale(), 2);
	%damage = mClampF(%this.raycastDirectDamage, -100, 100) * %scale;

	%headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );

		%colscale = getWord(%col.getScale(),2);
		if(getword(%pos, 2) > getword(%col.getWorldBoxCenter(), 2) - 3.3*%colscale)
		{
			%damage = 150;
		}
		
	if(%this.raycastImpactImpulse > 0)
		%col.applyImpulse(%pos,vectorScale(%shotVec,%this.raycastImpactImpulse));
	
	if(%this.raycastVerticalImpulse > 0)
		%col.applyImpulse(%pos,vectorScale("0 0 1",%this.raycastVerticalImpulse));
	
	%col.damage(%obj, %pos, %damage, %damageType);
}