
//sounds
datablock AudioProfile(automaticShotgunFireSound)
{
   filename    = "./Sounds/Fire/pairedShotgun.wav";
   description = AudioDefault3d;
   preload = true;
};

//muzzle flash effects
datablock ProjectileData(automaticShotgunProjectile : gunProjectile)
{
   directDamage        = 8;//8;
   explosion           = QuieterGunExplosion;
   impactImpulse       = 400;
   verticalImpulse     = 150;

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
   headshotMultiplier = 1.25;
};

datablock ProjectileData(automaticShotgunBlastProjectile : gunProjectile)
{
   directDamage        = 40;//8;
   explosion           = QuieterGunExplosion;
   impactImpulse       = 700;
   verticalImpulse     = 250;

   muzzleVelocity      = 200;
   velInheritFactor    = 0;

   armingDelay         = 00;
   lifetime            = 100;
   fadeDelay           = 100;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 0.0;

   particleEmitter     = advSmallBulletTrailEmitter; //bulletTrailEmitter;
   headshotMultiplier = 4;
};

//////////
// item //
//////////

datablock ItemData(automaticShotgunItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/automaticShotgun.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "AP - Automatic Shotgun";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 4;
   ammotype = "Shotgun";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = automaticShotgunImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponHardImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
//////////////// 
datablock ShapeBaseImageData(automaticShotgunImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/automaticShotgun.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0.125";
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
   item = automaticShotgunItem;
   ammo = " ";
   projectile = automaticShotgunProjectile;
   projectileType = Projectile;

   casing = BAADShotgunDebris;
   shellExitDir        = "0.1 -0.05 0.5";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;   
   shellVelocity       = 10.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = automaticShotgunItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
   stateName[0]                     = "Activate";
   stateTimeoutValue[0]             = 0.5;
   stateScript[0]                   = "onAmmoCheck";
   stateTransitionOnTimeout[0]      = "AmmoCheckReady";
   stateSound[0]                    = "";

   stateName[1]                     = "Ready";
   stateTransitionOnTriggerDown[1]  = "Fire";
   stateTransitionOnNoAmmo[1]       = "Empty";
   stateAllowImageChange[1]         = true;

   stateName[2]                     = "Fire";
   stateTimeoutValue[2]             = 0.225;
   stateTransitionOnTimeout[2]      = "Smoke";
   stateFire[2]                     = true;
   stateAllowImageChange[2]         = false;
   stateSequence[2]                 = "Fire";
   stateScript[2]                   = "onFire";
   stateWaitForTimeout[2]           = true;
   stateEmitter[2]                  = advSmallBulletFireEmitter;
   stateEmitterTime[2]              = 0.05;
   stateEmitterNode[2]              = "muzzleNode";
   stateEjectShell[2]               = true;

   stateName[3]                     = "Smoke";
   stateEmitter[3]                  = advSmallBulletSmokeEmitter;
   stateEmitterTime[3]              = 0.05;
   stateEmitterNode[3]              = "muzzleNode";
   stateTimeoutValue[3]             = 0.001;
   stateTransitionOnTimeout[3]    = "Cycle";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTransitionOnTriggerUp[4]      = "untrigcockammo";

   stateName[5]                     = "AmmoCheck";
   stateTransitionOnTimeout[5]      = "Ready";
   stateAllowImageChange[5]         = true;
   stateScript[5]                   = "onAmmoCheck";

   stateName[7]                    = "Reload";
   stateScript[7]                  = "onReloadSingle";
   stateTransitionOnTimeout[7]	  = "CheckChamber";

   stateName[8]                     = "CheckChamber";
   stateTransitionOnTriggerDown[8]  = "AmmoCheck";
   stateTimeoutValue[8]             = 0.4;
   stateTransitionOnTimeout[8]      = "Reload";
   stateTransitionOnAmmo[8]         = "Wait";
   stateScript[8]                   = "onCheckChamber";
   stateAllowImageChange[8]         = false;
   stateWaitForTimeout[8]           = true;

   stateName[9]                     = "Empty";
   stateTransitionOnTriggerDown[9]  = "EmptyFireA";
   stateAllowImageChange[9]         = true;
   stateScript[9]                   = "onEmpty";
   stateTransitionOnAmmo[9]         = "Reload";

   stateName[16]                     = "EmptyFireA";
   stateTransitionOnTriggerUp[16]    = "EmptyFireB";
   stateScript[16]                   = "onEmptyFire";
   stateTimeoutValue[16]             = 0.05;
   stateAllowImageChange[16]         = false;
   stateWaitForTimeout[16]           = false;
   stateSound[16]                    = baadEmptySound;
   stateSequence[16]                 = "trig";
   
   stateName[17]                     = "EmptyFireB";
   stateTransitionOnTimeout[17]  = "Empty";
   stateAllowImageChange[17]         = false;
   stateTimeoutValue[17]             = 0.05;
   stateWaitForTimeout[17]           = false;
   stateSequence[17]                 = "untrig";
   
   stateName[13]                    = "Wait";
   stateTimeoutValue[13]            = 0.3;
   stateScript[13]                   = "onWait";
   stateTransitionOnTimeout[13]     = "Ready";
   stateWaitForTimeout[13]         = true;
   
   stateName[15]                     = "AmmoCheckReady";
   stateTransitionOnNoAmmo[15]       = "Empty";
   stateScript[15]                   = "onAmmoCheck";
   stateTransitionOnTimeout[15]      = "Ready";
   stateAllowImageChange[15]         = true;
   
   stateName[18]                    = "untrigcockammo";
   stateTimeoutValue[18]            = 0.001;
   stateScript[18]                   = "onCockAmmo";
   stateTransitionOnTimeout[18]     = "Ready";
   stateSequence[18]                = "untrig";
   stateWaitForTimeout[18]         = false;
};

  ////// ammo display functions
function automaticShotgunImage::onMount( %this, %obj, %slot )
{
   parent::onMount(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}
function automaticShotgunImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function automaticShotgunImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function automaticShotgunImage::onCheckChamber( %this, %obj, %slot )
{
   if(%obj.toolMag[%obj.currTool] > %this.item.maxmag)
   {
      %obj.toolMag[%obj.currTool] = %this.item.maxmag;
      %obj.setImageAmmo(0,1);
   }
   hl2DisplayAmmo(%this,%obj,%slot);
}

function automaticShotgunImage::onReloadSingle( %this, %obj, %slot )
{
   %obj.playThread(2, "shiftUp");
   %obj.playThread(3, "plant");
   schedule(getRandom(0,50),0,serverPlay3D,BAADHeavyInsert @ getRandom(1,3) @ Sound,%obj.getPosition());
   hl2DisplayAmmo(%this,%obj,%slot);
   hl2AmmoOnReloadSingle(%this,%obj,%slot);
}

function automaticShotgunImage::onCockAmmo( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function automaticShotgunImage::onEmptyFire( %this, %obj, %slot )
{
   %obj.playThread(2, plant);
}

function automaticShotgunImage::onWait( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function automaticShotgunImage::onEmpty(%this,%obj,%slot)
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

function automaticShotgunImage::onFire( %this, %obj, %slot )
{
   if(%obj.getDamagePercent() >= 1)
   return;

   %obj.toolMag[%obj.currTool] -= 1 - mRound(getRandom() * %obj.ammoReturnLevel);

   if(%obj.toolMag[%obj.currTool] < 1)
   {
      %obj.toolMag[%obj.currTool] = 0;
      %obj.setImageAmmo(0,0);
   }
   hl2DisplayAmmo(%this,%obj,%slot);

	%projectile = %this.projectile;
	%spread = 0.006;
	%shellcount = 7;

  	%fvec = %obj.getForwardVector();
  	%fX = getWord(%fvec,0);
  	%fY = getWord(%fvec,1);
  
  	%evec = %obj.getEyeVector();
  	%eX = getWord(%evec,0);
  	%eY = getWord(%evec,1);
  	%eZ = getWord(%evec,2);
  
  	%eXY = mSqrt(%eX*%eX+%eY*%eY);
  
  	%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
//	//%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%aimVec,"-2")));
	%obj.spawnExplosion(advRecoilProjectile,"1 1 1");
            		
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

	%projectile = automaticShotgunBlastProjectile;
	%spread = 0.0005;
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

   %obj.playThread(2, shiftAway);
   %obj.playThread(3, shiftRight);
   serverPlay3d( automaticShotgunFireSound, %obj.getPosition());
   if(getSimTime()-%obj.lastShellSound > 200 || getRandom(0,1) == 0)
   {
   %obj.lastShellSound = getSimTime();
   schedule(getRandom(500,600),0,serverPlay3D,BAADShellShotty @ getRandom(1,7) @ Sound,%obj.getPosition());
   }
}

function automaticShotgunProjectile1::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function automaticShotgunProjectile2::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function automaticShotgunProjectile3::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
