//sounds
datablock AudioProfile(pairedShotgunFireSound)
{
   filename    = "./Sounds/Fire/pairedShotgun.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(pairedShotgunReloadSound)
{
   filename    = "./Sounds/Reload/battleShotgunReload.wav";
   description = AudioClose3d;
   preload = true;
};

//muzzle flash effects
datablock ProjectileData(pairedShotgunProjectile : gunProjectile)
{
   directDamage        = 14;//8;
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

datablock ProjectileData(pairedShotgunBlastProjectile : gunProjectile)
{
   directDamage        = 60;//8;
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

datablock ItemData(pairedShotgunItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/pairedShotgun.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "AP - Paired Shotgun";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 8;
   ammotype = "Shotgun";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = pairedShotgunImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponHardImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
//////////////// 
datablock ShapeBaseImageData(pairedShotgunImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/pairedShotgun.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 -0.085 -0.012";
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
   item = pairedShotgunItem;
   ammo = " ";
   projectile = pairedShotgunProjectile;
   projectileType = Projectile;

   casing = BAADShotgunDebris;
   shellExitDir        = "0 0.01 0.1";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 25.0;   
   shellVelocity       = 8.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = pairedShotgunItem.colorShiftColor;//"0.400 0.196 0 1.000";

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
   stateTimeoutValue[2]             = 0.27;
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
   stateTimeoutValue[3]             = 0.2;
   stateTransitionOnTimeout[3]    = "Cycle";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTransitionOnTriggerUp[4]      = "untrigcockammo";

   stateName[5]                     = "AmmoCheck";
   stateTransitionOnTimeout[5]      = "Ready";
   stateAllowImageChange[5]         = true;
   stateScript[5]                   = "onAmmoCheck";
   
   stateName[6]                    = "CockOpen";
   stateTimeoutValue[6]            = 0.15;
   stateScript[6]                   = "onCockAmmo";
   stateSequence[6]                = "Pump_open";
   stateTransitionOnTimeout[6]     = "shell1";
   stateWaitForTimeout[6]          = true;
   stateEjectShell[6]              = false;
   
   stateName[7]                    = "CockClose";
   stateTimeoutValue[7]            = 0.2;
   stateScript[7]                   = "onCockAmmoB";
   stateSequence[7]                = "Pump_close";
   stateTransitionOnTimeout[7]     = "Ready";
   stateWaitForTimeout[7]          = true;
   stateEjectShell[7]              = false;
   
   stateName[8]                    = "Cock";
   stateTimeoutValue[8]            = 0.4;
   stateScript[8]                   = "onCock";
   stateSequence[8]                = "Pump";
   stateTransitionOnTimeout[8]     = "Ready";
   stateWaitForTimeout[8]          = true;
   stateEjectShell[8]              = false;

	stateName[9]                    = "Reload";
   stateScript[9]                  = "onReloadSingle";
	stateTransitionOnTimeout[9]	  = "CheckChamber";

   stateName[10]                     = "CheckChamber";
   stateTransitionOnTriggerDown[10]  = "AmmoCheck";
   stateTimeoutValue[10]             = 0.65;
   stateTransitionOnTimeout[10]      = "Reload";
   stateTransitionOnAmmo[10]         = "Wait";
   stateScript[10]                   = "onCheckChamber";
   stateAllowImageChange[10]         = false;
   stateWaitForTimeout[10]           = true;

   stateName[11]                     = "Empty";
   stateTransitionOnTriggerDown[11]  = "EmptyFireA";
   stateAllowImageChange[11]         = true;
   stateScript[11]                   = "onEmpty";
   stateTransitionOnAmmo[11]         = "Reload";

   stateName[12]                     = "EmptyFireA";
   stateTransitionOnTriggerUp[12]    = "EmptyFireB";
   stateScript[12]                   = "onEmptyFire";
   stateTimeoutValue[12]             = 0.05;
   stateAllowImageChange[12]         = false;
   stateWaitForTimeout[12]           = false;
   stateSound[12]                    = baadEmptySound;
   stateSequence[12]                 = "fire";
   
   stateName[13]                     = "EmptyFireB";
   stateTransitionOnTimeout[13]  = "Empty";
   stateAllowImageChange[13]         = false;
   stateTimeoutValue[13]             = 0.05;
   stateWaitForTimeout[13]           = false;
   stateSequence[13]                 = "untrig";
   
   stateName[14]                    = "Wait";
   stateTimeoutValue[14]            = 0.3;
   stateScript[14]                   = "onWait";
   stateTransitionOnTimeout[14]     = "Cock";
   stateWaitForTimeout[14]         = true;
   
   stateName[15]                     = "AmmoCheckReady";
   stateTransitionOnNoAmmo[15]       = "Empty";
   stateScript[15]                   = "onAmmoCheck";
   stateTransitionOnTimeout[15]      = "Ready";
   stateAllowImageChange[15]         = true;
   
   stateName[16]                    = "untrigcockammo";
   stateTimeoutValue[16]            = 0.001;
   stateTransitionOnTimeout[16]     = "CockOpen";
   stateSequence[16]                = "untrig";
   stateWaitForTimeout[16]         = false;
   
   stateName[17]                     = "shell1";
   stateTimeoutValue[17]             = 0.001;
   stateTransitionOnTimeout[17]      = "shell2";
   stateEjectShell[17]               = true;
   
   stateName[18]                     = "shell2";
   stateTimeoutValue[18]             = 0.001;
   stateTransitionOnTimeout[18]      = "CockClose";
   stateEjectShell[18]               = true;
};

  ////// ammo display functions

function pairedShotgunImage::onMount( %this, %obj, %slot )
{
   parent::onMount(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function pairedShotgunImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function pairedShotgunImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function pairedShotgunImage::onCheckChamber( %this, %obj, %slot )
{
   if(%obj.toolMag[%obj.currTool] > %this.item.maxmag)
   {
      %obj.toolMag[%obj.currTool] = %this.item.maxmag;
      %obj.setImageAmmo(0,1);
   }
   hl2DisplayAmmo(%this,%obj,%slot);
}

function pairedShotgunImage::onReloadSingle( %this, %obj, %slot )
{
   %obj.playThread(2, "shiftUp");
   %obj.playThread(3, "plant");
   %obj.schedule(200, "playThread", "2", "shiftUp");
   %obj.schedule(200, "playThread", "3", "plant");
   schedule(getRandom(0,50),0,serverPlay3D,BAADHeavyInsert @ getRandom(1,3) @ Sound,%obj.getPosition());
   schedule(getRandom(200,250),0,serverPlay3D,BAADHeavyInsert @ getRandom(1,3) @ Sound,%obj.getPosition());
   hl2DisplayAmmo(%this,%obj,%slot);
   hl2AmmoOnReloadSingle(%this,%obj,%slot);
   hl2AmmoOnReloadSingle(%this,%obj,%slot);
}

function pairedShotgunImage::onCockAmmo( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
   serverPlay3d( pairedShotgunReloadSound, %obj.getPosition() );
   %obj.playThread(3, "plant");
   schedule(getRandom(500,600),0,serverPlay3D,BAADShellShotty @ getRandom(1,7) @ Sound,%obj.getPosition());
   schedule(getRandom(500,600),0,serverPlay3D,BAADShellShotty @ getRandom(1,7) @ Sound,%obj.getPosition());
}

function pairedShotgunImage::onCock( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
   serverPlay3d( pairedShotgunReloadSound, %obj.getPosition() );
   %obj.playThread(3, "plant");
}

function pairedShotgunImage::onEmptyFire( %this, %obj, %slot )
{
   %obj.playThread(2, plant);
}

function pairedShotgunImage::onWait( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function pairedShotgunImage::onEmpty(%this,%obj,%slot)
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

function pairedShotgunImage::onFire( %this, %obj, %slot )
{
   if(%obj.getDamagePercent() >= 1)
   return;

   if(%obj.toolMag[%obj.currTool] <= 2)
   {
      %obj.toolMag[%obj.currTool] = 0;
      %obj.setImageAmmo(0,0);

	%projectile = %this.projectile;
	%spread = 0.002;
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

	%projectile = pairedShotgunBlastProjectile;
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
   }



   if(%obj.toolMag[%obj.currTool] > 1)
   {
   	%obj.toolMag[%obj.currTool] -= 2;

	%projectile = %this.projectile;
	%spread = 0.0043;
	%shellcount = 14;

  	%fvec = %obj.getForwardVector();
  	%fX = getWord(%fvec,0);
  	%fY = getWord(%fvec,1);
  
  	%evec = %obj.getEyeVector();
  	%eX = getWord(%evec,0);
  	%eY = getWord(%evec,1);
  	%eZ = getWord(%evec,2);
  
  	%eXY = mSqrt(%eX*%eX+%eY*%eY);
  
  	%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
	//%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%aimVec,"-5")));
	%obj.spawnExplosion(advRecoilProjectile,"2 2 2");
            		
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

	%projectile = pairedShotgunBlastProjectile;
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
   }

   hl2DisplayAmmo(%this,%obj,%slot);

   if(%obj.toolMag[%obj.currTool] == 0)
   {
      %obj.setImageAmmo(0,0);
   }

   %obj.playThread(2, activate);
   %obj.playThread(3, plant);
   serverPlay3d( pairedShotgunFireSound, %obj.getPosition());
}

function pariedShotgunProjectile1::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function pariedShotgunProjectile2::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function pariedShotgunProjectile3::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
