//sounds
datablock AudioProfile(taserHitSound)
{
   filename    = "./Sounds/Reload/taser.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(taserFireSound)
{
   filename    = "./Sounds/Fire/taserLaunch.wav";
   description = AudioClose3d;
   preload = true;
};

datablock ParticleData(electrocuteParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 50;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 1000.0;
	spinRandomMin		= -5000.0;
	spinRandomMax		= 5000.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/star1";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]     = "1 1 1 1";
	colors[1]     = "0 0 0 0.7";
	colors[2]	= "0 0 0 0.7";
	sizes[0]	= 2;
	sizes[1]	= 3;
	sizes[2]	= 2;
	times[0]	= 0.0;
	times[1]	= 0.4;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(electrocuteEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;

   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;

   ejectionOffset = 0;

   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = electrocuteParticle;
};

datablock ParticleData(electrocuteSmogParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = -0.2;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "0.3 0.3 0.3 0.1";
	colors[1]     = "0.3 0.3 0.3 0.0";
	sizes[0]      = 2.13;
	sizes[1]      = 2.0;
};

datablock ParticleEmitterData(electrocuteSmogEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 2;
   velocityVariance = 0.0;
   ejectionOffset   = 0.5;
   thetaMin         = 80;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "electrocuteSmogParticle";

   useEmitterColors = true;
};

datablock ExplosionData(electrocuteSmogExplosion)
{
   //explosionShape = "";
	soundProfile = taserHitSound;

   lifeTimeMS = 150;

   particleEmitter = electrocuteSmogEmitter;
   particleDensity = 17;
   particleRadius = 0.2;

   emitter[0] = "";

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = false;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 1;
   lightStartColor = "0.3 0.6 0.7";
   lightEndColor = "0 0 0";
};

//muzzle flash effects
datablock ProjectileData(taserProjectile : gunProjectile)
{
   directDamage        = 100;//8;

   impactImpulse       = 100;
   verticalImpulse     = 50;

   muzzleVelocity      = 200;
   velInheritFactor    = 0;
   explosion           = electrocuteSmogExplosion;

   armingDelay         = 00;
   lifetime            = 80;
   fadeDelay           = 80;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 0.0;

   particleEmitter     = advSmallBulletTrailEmitter; //bulletTrailEmitter;
   headshotMultiplier = 1.5;
};

//////////
// item //
//////////

$hl2Weapons::MaxAmmo["Taser"] = 12;
$hl2Weapons::AddAmmo["Taser"] = 3;

datablock ItemData(taserItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/taser.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "Taser";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 1;
   ammotype = "Taser";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = taserImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponSoftImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(taserImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/taser.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
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
   item = taserItem;
   ammo = " ";
   projectile = taserProjectile;
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
   colorShiftColor = taserItem.colorShiftColor;//"0.400 0.196 0 1.000";

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
   stateTimeoutValue[2]             = 0.12;
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
   stateTimeoutValue[3]             = 0.1;
   stateTransitionOnTriggerUp[3]    = "AmmoCheck";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTimeoutValue[4]             = 0.15;
   stateTransitionOnTimeout[4]      = "AmmoCheck";
   stateSound[4]                    = "";

   stateName[5]                     = "AmmoCheck";
   stateTransitionOnTimeout[5]      = "Ready";
   stateAllowImageChange[5]         = true;
   stateScript[5]                   = "onAmmoCheck";

   stateName[6]                     = "Reload";
   stateTransitionOnTimeout[6]      = "ReloadReady";
   stateTimeoutValue[6]             = 0.85;
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

};

  ////// ammo display functions

function taserImage::onMount( %this, %obj, %slot )
{
   parent::onMount(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function taserImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function taserImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function taserImage::onReloadStart( %this, %obj, %slot )
{
   %obj.playThread(2,shiftUp);
   serverPlay3d( advReloadOut0Sound, %obj.getPosition());

   %obj.schedule(650, "playThread", "2", "shiftLeft");
   %obj.schedule(750, "playThread", "2", "plant");

   schedule(650, 0, serverPlay3D, advReloadTap1Sound, %obj.getPosition());
   schedule(750, 0, serverPlay3D, advReloadTap0Sound, %obj.getPosition());

   hl2DisplayAmmo(%this,%obj,%slot);
}

function taserImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function taserImage::onEmpty(%this,%obj,%slot)
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

function taserImage::onFire( %this, %obj, %slot )
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

	%projectile = %this.projectile;
	%spread = 0.0003;
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
	%obj.spawnExplosion(advLittleRecoilProjectile,"0.75 0.75 0.75");
            		
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
   serverPlay3d( taserFireSound, %obj.getPosition());
}

function taserProjectile::damage( %this, %obj, %col, %fade, %pos, %normal )
{
   if(%col.isCorpse)
   {
      return;
   }
   %corpse = %col.client.createCorpse();
   %col.client.corpse.isUnconscious = true;
   %col.client.corpse.corpseDamageLevel = 0;

   %col.client.corpse.reviveCorpseTimer(15);
   %col.client.camera.setMode("Corpse", %corpse);
   %col.client.setControlObject(%col.client.camera);
   %col.delete();
}