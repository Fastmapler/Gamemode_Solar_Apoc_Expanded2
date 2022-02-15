//sounds
datablock AudioProfile(huntingRifleFireSound)
{
   filename    = "./Sounds/Fire/huntingRifle.wav";
   description = AudioClose3d;
   preload = true;
};

datablock DebrisData(huntingRifleClipDebris)
{
	shapeFile = "./shapes/weapons/huntingRifleMag.dts";
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
datablock ExplosionData(huntingRifleClipExplosion)
{
	debris 					= huntingRifleClipDebris;
	debrisNum 				= 1;
	debrisNumVariance 		= 0;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 1;
	debrisVelocityVariance 	= 0;
};
datablock ProjectileData(huntingRifleClipProjectile)
{
	explosion = huntingRifleClipExplosion;
};

//muzzle flash effects
datablock ProjectileData(huntingRifleProjectile1 : gunProjectile)
{
   directDamage        = 50;//8;
   explosion           = QuietGunExplosion;
   impactImpulse       = 100;
   verticalImpulse     = 50;

   muzzleVelocity      = 200;
   velInheritFactor    = 0;

   armingDelay         = 00;
   lifetime            = 4000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 0.0;

   particleEmitter     = advBigBulletTrailEmitter; //bulletTrailEmitter;
   headshotMultiplier = 4.5;
   sound = BAADWhiz1Sound;
   uiName = "";	
};

datablock ProjectileData(huntingRifleProjectile2 : huntingRifleProjectile1)
{
   sound = BAADWhiz2Sound;
};

datablock ProjectileData(huntingRifleProjectile3 : huntingRifleProjectile1)
{
   sound = BAADWhiz3Sound;
};

//////////
// item //
//////////

datablock ItemData(huntingRifleItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/huntingRifle.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "AP - Hunting Rifle";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 4;
   ammotype = "Rifle";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = huntingRifleImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponHardImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(huntingRifleImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/huntingRifle.dts";
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
   item = huntingRifleItem;
   ammo = " ";
   projectile = huntingRifleProjectile;
   projectileType = Projectile;

   casing = BAADRifleDebris;
   shellExitDir        = "0.25 -0.05 0.5";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 8.0;   
   shellVelocity       = 10.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = huntingRifleItem.colorShiftColor;//"0.400 0.196 0 1.000";

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
   stateTimeoutValue[2]             = 0.2;
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
   stateTimeoutValue[3]             = 0.001;
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
   stateTimeoutValue[11]             = 0.25;
   
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
function huntingRifleImage::onMount( %this, %obj, %slot )
{
   parent::onMount(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}
function huntingRifleImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function huntingRifleImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions

function huntingRifleImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function huntingRifleImage::onEmpty(%this,%obj,%slot)
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

function huntingRifleImage::onEmptyFire( %this, %obj, %slot )
{
   %obj.playThread(2, plant);
}

function huntingRifleImage::onSmoke( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
   %obj.playThread(2,plant);
}

function huntingRifleImage::onFire( %this, %obj, %slot )
{
   if(%obj.getDamagePercent() > 1.0)
   {
      return;
   }

   %obj.toolMag[%obj.currTool] -= 1;

   if(%obj.toolMag[%obj.currTool] < 1)
   {
      %obj.toolMag[%obj.currTool] = 0;
      %obj.setImageAmmo(0,0);
   }
   hl2DisplayAmmo(%this,%obj,%slot);

    %projectiles = "huntingRifleProjectile1" TAB "huntingRifleProjectile2" TAB "huntingRifleProjectile3";
    %projectile = getField(%projectiles, getRandom(0, getFieldCount(%projectiles)-1));
	%spread = 0.00001;
	%shellcount = 1;

  	%fvec = %obj.getForwardVector();
  	%fX = getWord(%fvec,0);
  	%fY = getWord(%fvec,1);
  
  	%evec = %obj.getEyeVector();
  	%eX = getWord(%evec,0);
  	%eY = getWord(%evec,1);
  	%eZ = getWord(%evec,2);
  
  	%eXY = mSqrt(%eX*%eX+%eY*%eY);
  
  	%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
	//%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%aimVec,"-1")));
	%obj.spawnExplosion(advLittleRecoilProjectile,"1.1 1.1 1.1");
            		
	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}

   %obj.playThread(2, shiftRight);
   %obj.playThread(3, shiftLeft);
   serverPlay3d( huntingRifleFireSound, %obj.getPosition());
}

function huntingRifleProjectile1::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function huntingRifleProjectile2::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function huntingRifleProjectile3::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}

function huntingRifleImage::OnReloadA(%this, %obj, %slot) 
{
   schedule(getRandom(500,600),0,serverPlay3D,BAADShellRifle @ getRandom(1,6) @ Sound,%obj.getPosition());
   %obj.playThread(2,plant);
   %obj.playThread(3,shiftLeft);
   schedule(0, 0, serverPlay3D, baadBoltOpenSound, %obj.getPosition());
}

function huntingRifleImage::OnReloadB(%this, %obj, %slot) 
{
   %obj.schedule(0, "playThread", "2", "wrench");
   %obj.schedule(0, "playThread", "3", "shiftRight");
          schedule(0, 0, serverPlay3D, baadReload2Sound, %obj.getPosition());
          schedule(getRandom(250,350),0,serverPlay3D,BAADMagDrop @ getRandom(1,3) @ Sound,%obj.getPosition());
          %up = %obj.getUpVectorHack();
          %forward = %obj.getEyeVectorHack();
          %p = new projectile()
          {
              datablock = "huntingRifle" @ (%obj.toolAmmo[%obj.currTool] <= 0 ? "Clip" : "") @ "Projectile";
              initialPosition = vectorAdd(%obj.getSlotTransform(0),vectorRelativeShift(%forward,%up,"0.4 0 -0.5"));
          };
          %p.explode();
}

function huntingRifleImage::OnReloadC(%this, %obj, %slot)
{
   %obj.schedule(0, "playThread", "2", "shiftUp");
   schedule(0, 0, serverPlay3D, baadReload11Sound, %obj.getPosition());
}

function huntingRifleImage::OnReloadD(%this, %obj, %slot)
{
   %obj.playThread(2,plant);
   %obj.playThread(3,shiftRight);
   schedule(0, 0, serverPlay3D, baadBoltCloseSound, %obj.getPosition());
}