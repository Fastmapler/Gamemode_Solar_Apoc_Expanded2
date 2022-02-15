
//sounds
datablock AudioProfile(doubleShotgunFireSound)
{
   filename    = "./Sounds/Fire/New/SHT_DB_VER2.R.wav";
   description = AudioDefault3d;
   preload = true;
};

//muzzle flash effects
datablock ProjectileData(doubleShotgunProjectile : gunProjectile)
{
   directDamage        = 12;//8;
   explosion           = QuieterGunExplosion;
   impactImpulse       = 200;
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
   headshotMultiplier = 1.5;
};

datablock ProjectileData(doubleShotgunBlastProjectile : gunProjectile)
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
   headshotMultiplier = 3;
   
};

//////////
// item //
//////////

datablock ItemData(doubleShotgunItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/doubleShotgun.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "AP - Double Shotgun";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 2;
   ammotype = "Shotgun";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = doubleShotgunImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponHardImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
//////////////// 
datablock ShapeBaseImageData(doubleShotgunImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/doubleShotgun.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 -0.055 -0.0075";
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
   item = doubleShotgunItem;
   ammo = " ";
   projectile = doubleShotgunProjectile;
   projectileType = Projectile;

   casing = BAADShotgunDebris;
   shellExitDir        = "0 -0.25 0.25";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 20.0;   
   shellVelocity       = 10.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = doubleShotgunItem.colorShiftColor;//"0.400 0.196 0 1.000";

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

   stateName[3]                     = "Smoke";
   stateEmitter[3]                  = advSmallBulletSmokeEmitter;
   stateEmitterTime[3]              = 0.05;
   stateEmitterNode[3]              = "muzzleNode";
   stateTimeoutValue[3]             = 0.07;
   stateTransitionOnTimeout[3]    = "AmmoCheck";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTimeoutValue[4]             = 0.15;
   stateTransitionOnTimeout[4]      = "Ready";
   stateSound[4]                    = "";

   stateName[5]                     = "AmmoCheck";
   stateTransitionOnTriggerUp[5]      = "untrig";
   stateAllowImageChange[5]         = true;
   stateScript[5]                   = "onAmmoCheck";
   
  stateName[6]                     = "Reload";
   stateTransitionOnTimeout[6]      = "ReloadA";
   stateTimeoutValue[6]             = 0.01;
   stateAllowImageChange[6]         = true;
   stateScript[6]                   = "onReloadStart";
   stateSound[6]                    = advSound;

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
   
   stateName[11]                     = "ReloadA"; 
   stateTransitionOnTimeout[11]  = "shell1";
   stateAllowImageChange[11]         = true;
   stateScript[11]                   = "OnClipRemoved";
   stateSequence[11]                 = "open";
   stateTimeoutValue[11]             = 0.25;
     
   stateName[12]                     = "ReloadInter";
   stateTransitionOnTimeout[12]      = "ReloadB";
   stateTimeoutValue[12]             = 0.3;
   stateAllowImageChange[12]         = true;
	 
   stateName[13]                     = "ReloadB";
   stateTransitionOnTimeout[13]  = "ReloadC";
   stateAllowImageChange[13]         = true;
   stateTimeoutValue[13]             = 0.3;
   stateScript[13]                   = "OnReloadB";
   stateSequence[13]                 = "shella";
     
   stateName[14]                     = "ReloadC";
   stateTransitionOnTimeout[14]  = "ReloadD";
   stateAllowImageChange[14]         = true;
   stateScript[14]                   = "OnReloadC";
   stateTimeoutValue[14]             = 0.3;
   stateSequence[14]                 = "shellb";
   
   stateName[15]                     = "ReloadD";
   stateTransitionOnTimeout[15]  = "ReloadReady";
   stateSequence[15]                 = "close";
   stateAllowImageChange[15]         = true;
   stateTimeoutValue[15]             = 0.1;

   stateName[16]                     = "untrig";
   stateTimeoutValue[16]             = 0.05;
   stateSequence[16]                 = "untrig";
   stateTransitionOnTimeout[16]      = "Ready";
   stateSound[16]                    = "";
   
   stateName[17]                     = "AmmoCheckReady";
   stateTransitionOnNoAmmo[17]       = "Empty";
   stateScript[17]                   = "onAmmoCheck";
   stateTransitionOnTimeout[17]      = "Ready";
   stateAllowImageChange[17]         = true;
  
   stateName[18]                     = "shell1";
   stateTimeoutValue[18]             = 0.001;
   stateTransitionOnTimeout[18]      = "shell2";
   stateEjectShell[18]               = true;
   
   stateName[19]                     = "shell2";
   stateTimeoutValue[19]             = 0.001;
   stateTransitionOnTimeout[19]      = "ReloadInter";
   stateEjectShell[19]               = true;
};

  ////// ammo display functions
function doubleShotgunImage::onMount( %this, %obj, %slot )
{
   parent::onMount(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}
function doubleShotgunImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function doubleShotgunImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function doubleShotgunImage::onReloadStart( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function doubleShotgunImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function doubleShotgunImage::onEmptyFire( %this, %obj, %slot )
{
   %obj.playThread(2, plant);
}

function doubleShotgunImage::onEmpty(%this,%obj,%slot)
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

function doubleShotgunImage::onFire( %this, %obj, %slot )
{
   if(%obj.getDamagePercent() >= 1)
   return;

   %obj.toolMag[%obj.currTool] -= 2;

   if(%obj.toolMag[%obj.currTool] < 2)
   {
      %obj.toolMag[%obj.currTool] = 0;
      %obj.setImageAmmo(0,0);
   }
   hl2DisplayAmmo(%this,%obj,%slot);

	%projectile = %this.projectile;
	%spread = 0.004;
	%shellcount = 15;

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

	%projectile = doubleShotgunBlastProjectile;
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

   %obj.playThread(2, activate);
   %obj.playThread(3, plant);
   serverPlay3d( doubleShotgunFireSound, %obj.getPosition());
}

function doubleShotgunProjectile1::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function doubleShotgunProjectile2::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function doubleShotgunProjectile3::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}


function doubleShotgunImage::OnClipRemoved(%this, %obj, %slot)
{
        schedule(getRandom(600,700),0,serverPlay3D,BAADShellShotty @ getRandom(1,7) @ Sound,%obj.getPosition());
        schedule(getRandom(600,700),0,serverPlay3D,BAADShellShotty @ getRandom(1,7) @ Sound,%obj.getPosition());
       	%obj.playThread(2,shiftTo);
        schedule(0, 0, serverPlay3D, baadReload2Sound, %obj.getPosition());
        %obj.schedule(100, "playThread", "3", "plant");
        %obj.schedule(390, "playThread", "2", "shiftRight");
        %obj.schedule(390, "playThread", "3", "leftRecoil");
        schedule(getRandom(350,390),0,serverPlay3D,BAADHeavyInsert @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function doubleShotgunImage::OnReloadB(%this, %obj, %slot) 
{
   %obj.schedule(245, "playThread", "2", "shiftLeft");
   %obj.schedule(245, "playThread", "3", "leftRecoil");
   schedule(getRandom(210,245),0,serverPlay3D,BAADHeavyInsert @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function doubleShotgunImage::OnReloadC(%this, %obj, %slot)
{
   %obj.schedule(275, "playThread", "3", "plant");
   %obj.schedule(300, "playThread", "2", "shiftAway");
   schedule(275, 0, serverPlay3D, baadCock6Sound, %obj.getPosition());
}
