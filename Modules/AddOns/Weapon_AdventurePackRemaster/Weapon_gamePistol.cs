//sounds
datablock AudioProfile(gamePistolFireSound)
{
   filename    = "./Sounds/Fire/gamePistol.wav";
   description = AudioClose3d;
   preload = true;
};

//muzzle flash effects
datablock ProjectileData(gamePistolProjectile1 : gunProjectile)
{
   directDamage        = 35;//8;

   impactImpulse       = 500;
   verticalImpulse     = 450;

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
   headshotMultiplier = 3.5;
   sound = BAADWhiz1Sound;
   uiName = "";	
};

datablock ProjectileData(gamePistolProjectile2 : gamePistolProjectile1)
{
   sound = BAADWhiz2Sound;
};

datablock ProjectileData(gamePistolProjectile3 : gamePistolProjectile1)
{
   sound = BAADWhiz3Sound;
};

//////////
// item //
//////////

datablock ItemData(gamePistolItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/gamePistol.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "Brush Pistol";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 6;
   ammotype = "Revolver";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = gamePistolImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponSoftImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(gamePistolImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/gamePistol.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0.1 0";
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
   item = gamePistolItem;
   ammo = " ";
   projectile = gamePistolProjectile;
   projectileType = Projectile;

   casing = gunShellDebris;
   shellExitDir        = "1.0 -1.3 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 15.0;   
   shellVelocity       = 7.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = gamePistolItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
   stateName[0]                     = "Activate";
   stateTimeoutValue[0]             = 0.15;
   stateTransitionOnTimeout[0]      = "AmmoCheck";
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
   stateEmitter[2]                  = advBigBulletFireEmitter;
   stateEmitterTime[2]              = 0.05;
   stateEmitterNode[2]              = "muzzleNode";

   stateName[3]                     = "Smoke";
   stateTimeoutValue[3]             = 0.4;
   stateTransitionOnTimeout[3]    = "Cycle";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTimeoutValue[4]             = 0.1;
   stateTransitionOnTimeout[4]      = "SecondCycle";
   stateScript[4]                   = "onClickdown";
   stateSequence[4]                 = "Clickdown";
   stateSound[4]                    = revolverCycleSound;

   stateName[5]                     = "AmmoCheck";
   stateTransitionOnTimeout[5]      = "Ready";
   stateAllowImageChange[5]         = true;
   stateScript[5]                   = "onAmmoCheck";

   stateName[6]                     = "Reload";
   stateTransitionOnTimeout[6]      = "ReloadReady";
   stateTimeoutValue[6]             = 2.75;
   stateAllowImageChange[6]         = true;
   stateScript[6]                   = "onReloadStart";
   stateSound[6]                    = advSound;

   stateName[7]                     = "ReloadReady";
   stateTransitionOnTimeout[7]      = "Ready";
   stateTimeoutValue[7]             = 0.1;
   stateAllowImageChange[7]         = true;
   stateScript[7]                   = "onReload";

   stateName[8]                     = "Empty";
   stateTransitionOnTriggerDown[8]  = "EmptyFire";
   stateAllowImageChange[8]         = true;
   stateScript[8]                   = "onEmpty";
   stateTransitionOnAmmo[8]         = "Reload";
   //stateSequence[8]                 = "noammo";

   stateName[9]                     = "EmptyFire";
   stateTransitionOnTriggerUp[9]    = "Empty";
   stateTimeoutValue[9]             = 0.13;
   stateAllowImageChange[9]         = false;
   stateWaitForTimeout[9]           = true;
   stateSound[9]                    = baadEmptySound;
   stateSequence[9]                 = "noammo_fire";

   stateName[10]                     = "SecondCycle";
   stateTimeoutValue[10]             = 0.45;
   stateTransitionOnTimeout[10]      = "AmmoCheck";
   stateSound[10]                    = "";
};

  ////// ammo display functions
function gamePistolImage::onMount( %this, %obj, %slot )
{
   parent::onMount(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}
function gamePistolImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function gamePistolImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function gamePistolImage::onReloadStart( %this, %obj, %slot )
{
   %obj.playThread(2,shiftTo);
   serverPlay3d( advReloadOut0Sound, %obj.getPosition());

   %obj.schedule(550, "playThread", "2", "shiftRight");
   %obj.schedule(950, "playThread", "2", "shiftDown");
   %obj.schedule(1450, "playThread", "2", "shiftUp");
   %obj.schedule(2550, "playThread", "2", "shiftLeft");
   %obj.schedule(2650, "playThread", "2", "plant");

   schedule(550, 0, serverPlay3D, advReloadOut1Sound, %obj.getPosition());
   schedule(950, 0, serverPlay3D, advReloadOut3Sound, %obj.getPosition());
   schedule(1450, 0, serverPlay3D, advReload4Sound, %obj.getPosition());
   schedule(2550, 0, serverPlay3D, advReloadTap2Sound, %obj.getPosition());
   schedule(2650, 0, serverPlay3D, advReloadTap0Sound, %obj.getPosition());

   hl2DisplayAmmo(%this,%obj,%slot);
}

function gamePistolImage::onClickdown( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
   %obj.playThread(2,plant);
}

function gamePistolImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function gamePistolImage::onEmpty(%this,%obj,%slot)
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

function gamePistolImage::onFire( %this, %obj, %slot )
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

    %projectiles = "gamePistolProjectile1" TAB "gamePistolProjectile2" TAB "gamePistolProjectile3";
    %projectile = getField(%projectiles, getRandom(0, getFieldCount(%projectiles)-1));
	%spread = 0.0001;
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
	%obj.spawnExplosion(advLittleRecoilProjectile,"1 1 1");
            		
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
   serverPlay3d( gamePistolFireSound, %obj.getPosition());
}

function gamePistolProjectile1::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function gamePistolProjectile2::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function gamePistolProjectile3::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
