//nukeLauncher.cs
forceRequiredAddon("Weapon_Rocket_Launcher");

//audio
datablock AudioProfile(nukeFireSound)
{
   filename    = "./sounds/nuke_Fire.wav";
   description = AudioClosest3d;
   preload = true;
};

datablock AudioProfile(nukeExplodeSound)
{
   filename    = "./sounds/nuke_blast.wav";
   description = AudioDefault3d;
   preload = true;
};

//muzzle flash effects
datablock ParticleData(nukeLauncherFlashParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = 1.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 1500;
	lifetimeVarianceMS   = 150;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.9 0.4 0.0 0.9";
	colors[1]     = "0.9 0.5 0.0 0.0";
	sizes[0]      = 0.25;
	sizes[1]      = 0.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(nukeLauncherFlashEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 10.0;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "nukeLauncherFlashParticle";


};

datablock ParticleData(nukeLauncherSmokeParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 300;
	lifetimeVarianceMS   = 250;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;

   colors[0]     = "0.8 0.7 0.5 0.0";
	colors[1]     = "0.5 0.5 0.5 0.9";
	colors[2]     = "0.5 0.5 0.5 0.0";

	sizes[0]      = 1.25;
   sizes[1]      = 2.0;
	sizes[2]      = 1.75;

   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(nukeLauncherSmokeEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 10.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 25;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "nukeLauncherSmokeParticle";
};


//bullet trail effects
datablock ParticleData(nukeTrailParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -0.0;
	inheritedVelFactor   = 0.15;
	constantAcceleration = 0.0;
	lifetimeMS           = 3000;
	lifetimeVarianceMS   = 805;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -150.0;
	spinRandomMax		= 150.0;
	colors[0]     = "1.0 1.0 0.0 0.4";
	colors[1]     = "1.0 0.2 0.0 0.5";
   colors[2]     = "0.20 0.20 0.20 0.3";
   colors[3]     = "0.0 0.0 0.0 0.0";

	sizes[0]      = 0.75;
	sizes[1]      = 1.12;
   sizes[2]      = 5.35;
 	sizes[3]      = 0.05;

   times[0] = 0.0;
   times[1] = 0.05;
   times[2] = 0.3;
   times[3] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(nukeTrailEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 1;
   ejectionVelocity = 0.25;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "nukeTrailParticle";
};

datablock ParticleData(nukeExplosionParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -1.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 9800;
	lifetimeVarianceMS   = 100;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "0.1 0.1 0.1 1";
	colors[1]     = "0.4 0.5 0.5 0.9";
	colors[1]     = "0.2 0.1 0.1 0.0";
	sizes[0]      = 12.0;
	sizes[1]      = 182.0;
	sizes[2]      = 1.0;

	useInvAlpha = true;
};
datablock ParticleEmitterData(nukeExplosionEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 50;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 0;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "nukeExplosionParticle";


   emitterNode = TenthEmitterNode;
};

datablock ParticleData(nukeOverlayExplosionParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = 5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 1800;
	lifetimeVarianceMS   = 100;
	textureName          = "base/data/particles/star";
	spinSpeed		= 900.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "0.9 0.1 0.1 0.1";
	colors[0]     = "0.9 0.5 0.2 0.06";
	colors[0]     = "0.7 0.4 0.1 0.0";
	sizes[0]      = 90.0;
	sizes[1]      = 2.0;
	sizes[2]      = 1.0;
   times[0] = 0.0;
   times[1] = 0.05;
   times[2] = 1.0;


	useInvAlpha = true;
};
datablock ParticleEmitterData(nukeOverlayExplosionEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 90;
   velocityVariance = 0.0;
   ejectionOffset   = 9.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "nukeOverlayExplosionParticle";


   emitterNode = TenthEmitterNode;
};

datablock ParticleData(nukeExplosionRingParticle)
{
	dragCoefficient      = 4;
	gravityCoefficient   = -2.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 9350;
	lifetimeVarianceMS   = 135;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 25.0;
	spinRandomMin		= -25.0;
	spinRandomMax		= 25.0;
	colors[0]     = "1 0.3 0.0 0.2";
	colors[1]     = "0.2 0.2 0.2 0.0";
	sizes[0]      = 16;
	sizes[1]      = 19;

	useInvAlpha = false;
};
datablock ParticleEmitterData(nukeExplosionRingEmitter)
{
	lifeTimeMS = 90;

   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 120;
   velocityVariance = 1.0;
   ejectionOffset   = 20.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "nukeExplosionRingParticle";
};

datablock DebrisData(nukeBitsDebris)
{
   emitters = "nukeTrailEmitter";

	lifetime = 0.1;
	minSpinSpeed = -300.0;
	maxSpinSpeed = 300.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 0;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = false;

	gravModifier = 1;
};

datablock ExplosionData(nukeExplosion)
{
   explosionShape = "";
   explosionShape = "./shapes/weapons/nukeExplosion.dts";
	soundProfile = nukeExplodeSound;

   debris = nukeBitsDebris;
   debrisNum = 90;
   debrisNumVariance = 0;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisThetaMin = 0;
   debrisThetaMax = 180;
   debrisVelocity = 680;
   debrisVelocityVariance = 60;
   lifeTimeMS = 250;

   particleEmitter = nukeExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   emitter[0] = nukeExplosionRingEmitter;
   emitter[1] = nukeOverlayExplosionEmitter;

   faceViewer     = true;
   explosionScale = "25 25 25";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "3.0 10.0 3.0";
   camShakeDuration = 2.5;
   camShakeRadius = 20.0;

   // Dynamic light
   lightStartRadius = 60;
   lightEndRadius = 80;
   lightStartColor = "0.9 0.8 0.2 1";
   lightEndColor = "0.3 0.1 0.0 1";

   damageRadius = 25;
   radiusDamage = 9000;

   impulseRadius = 49;
   impulseForce = 94000;
};

datablock ProjectileData(nukeProjectile)
{
   projectileShapeName = "./shapes/weapons/missile.dts";
   directDamage        = 160;
   directDamageType = $DamageType::RocketDirect;
   radiusDamageType = $DamageType::RocketRadius;
   impactImpulse	   = 1000;
   verticalImpulse	   = 1000;
   explosion           = nukeExplosion;
   particleEmitter     = nukeTrailEmitter;

   brickExplosionRadius = 29;
   brickExplosionImpact = false;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 70;             
   brickExplosionMaxVolume = 290;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 390;  //max volume of bricks that we can destroy if they aren't connected to the ground (should always be >= brickExplosionMaxVolume)

   muzzleVelocity      = 65;
   velInheritFactor    = 1.0;

   armingDelay         = 00;
   lifetime            = 40000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = true;
   gravityMod = 1;

   hasLight    = true;
   lightRadius = 5.0;
   lightColor  = "1 0.5 0.0";

   uiName = "Mini-Nuke";
};

//////////
// item //
//////////
datablock ItemData(nukeLauncherItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/weapons/MISSILELAUNCHER.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "AP6 - Mini-Nuke";
	iconName = "./shapes/weapons/CI_WTFNUKE";
	doColorShift = false;
	colorShiftColor = "0.100 0.500 0.250 1.000";

   maxmag = 1;
   ammotype = "Mini-Nuke";
   reload = true;

   nochamber = 1;

	 // Dynamic properties defined by the scripts
	image = nukeLauncherImage;
	canDrop = true;

   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponSoftImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(nukeLauncherImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/MISSILELAUNCHER.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   //eyeOffset = "0.0 1.0 -1.05";
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
   item = nukeLauncherItem;
   ammo = " ";
   projectile = nukeProjectile;
   projectileType = Projectile;

	//casing = nukeLauncherShellDebris;
	shellExitDir        = "1.0 -1.3 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;	
	shellVelocity       = 7.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   LarmReady = false;
   minShotTime = 10;   //minimum time allowed between shots (needed to prevent equip/dequip exploit)

   doColorShift = true;
   colorShiftColor = "0.100 0.500 0.250 1.000";

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
   stateSequence[0]                 = "ready";
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
   stateEmitter[2]                  = nukeLauncherFlashEmitter;
   stateEmitterTime[2]              = 0.05;
   stateEmitterNode[2]              = "muzzleNode";
   stateEjectShell[2]               = false;
   stateSound[2]                    = nukeFireSound;

   stateName[3]                     = "Smoke";
   stateEmitter[3]                  = nukeLauncherSmokeEmitter;
   stateEmitterTime[3]              = 0.05;
   stateEmitterNode[3]              = "muzzleNode";
   stateTimeoutValue[3]             = 0.07;
   stateTransitionOnTimeout[3]      = "AmmoCheck";
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
   stateTransitionOnTimeout[10]     = "Empty";
   stateAllowImageChange[10]         = false;
   stateTimeoutValue[10]             = 0.05;
   stateWaitForTimeout[10]           = false;
   stateSequence[10]                 = "untrig";
   
   stateName[11]                     = "ReloadA"; 
   stateTransitionOnTimeout[11]  = "shell";
   stateAllowImageChange[11]         = true;
   stateScript[11]                   = "OnClipRemoved";
   stateSequence[11]                 = "open";
   stateTimeoutValue[11]             = 0.25;
     
   stateName[17]                     = "ReloadInter";
   stateTransitionOnTimeout[17]      = "ReloadC";
   stateTimeoutValue[17]             = 0.3;
   stateAllowImageChange[17]         = true;
     
   stateName[13]                     = "ReloadC";
   stateTransitionOnTimeout[13]  = "ReloadD";
   stateAllowImageChange[13]         = true;
   stateScript[13]                   = "OnReloadC";
   stateTimeoutValue[13]             = 0.3;
   stateSequence[13]                 = "shell";
   
   stateName[16]                     = "ReloadD";
   stateTransitionOnTimeout[16]  = "ReloadReady";
   stateSequence[16]                 = "close";
   stateAllowImageChange[16]         = true;
   stateTimeoutValue[16]             = 0.1;

   stateName[14]                     = "untrig";
   stateTimeoutValue[14]             = 0.05;
   stateSequence[14]                 = "untrig";
   stateTransitionOnTimeout[14]      = "Ready";
   stateSound[14]                    = "";
   
   stateName[15]                     = "AmmoCheckReady";
   stateTransitionOnNoAmmo[15]       = "Empty";
   stateScript[15]                   = "onAmmoCheck";
   stateTransitionOnTimeout[15]      = "Ready";
   stateAllowImageChange[15]         = true;
  
   stateName[18]                     = "shell";
   stateTransitionOnTimeout[18]      = "ReloadInter";
   stateEjectShell[18]               = true;

};

function nukeLauncherImage::onMount(%this,%obj,%slot)
{
	Parent::onMount(%this,%obj,%slot);
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function nukeLauncherImage::onUnMount(%this,%obj,%slot)
{
	Parent::onUnMount(%this,%obj,%slot);	
   %obj.playThread(0, root);
   hl2DisplayAmmo(%this,%obj,%slot,-1);
}

function nukeLauncherImage::onAmmoCheck( %this, %obj, %slot )
{
   hl2AmmoCheck(%this,%obj,%slot);
   hl2DisplayAmmo(%this,%obj,%slot);
}

/////// reload functions
function nukeLauncherImage::onReloadStart( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function nukeLauncherImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function nukeLauncherImage::onEmptyFire( %this, %obj, %slot )
{
   %obj.playThread(2, plant);
}

function nukeLauncherImage::onEmpty(%this,%obj,%slot)
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

function nukeLauncherImage::onFire(%this,%obj,%slot)
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

   Parent::onFire(%this,%obj,%slot);
}

function nukeLauncherImage::OnClipRemoved(%this, %obj, %slot)
{
        schedule(getRandom(700,800),0,serverPlay3D,BAADShellShotty @ getRandom(1,7) @ Sound,%obj.getPosition());
      	%obj.playThread(2,shiftTo);
        schedule(0, 0, serverPlay3D, baadReload2Sound, %obj.getPosition());
        %obj.schedule(100, "playThread", "3", "plant");
        %obj.schedule(390, "playThread", "2", "shiftRight");
        %obj.schedule(390, "playThread", "3", "leftRecoil");
        schedule(getRandom(350,390),0,serverPlay3D,BAADHeavyInsert @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function nukeLauncherImage::OnReloadC(%this, %obj, %slot)
{
   %obj.schedule(275, "playThread", "3", "plant");
   %obj.schedule(300, "playThread", "2", "shiftAway");
   schedule(275, 0, serverPlay3D, baadCock6Sound, %obj.getPosition());
}
