//RecurveBow.cs


datablock AudioProfile(RecurveBowExplosionSound)
{
   filename    = "./sounds/arrowHit.wav";
   description = AudioClose3d;
   preload = false;
};

datablock AudioProfile(RecurveBowFireSound)
{
   filename    = "./sounds/Fire.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(RecurveBowChargeSound)
{
   filename    = "./sounds/charge.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(RecurveBowFireFireSound)
{
   filename    = "./sounds/FireFire.wav";
   description = AudioClose3d;
	preload = true;
};
datablock ParticleData(FireTrailParticle)
{
	textureName          = "base/data/particles/cloud";
	dragCoefficient      = 0.0;
	gravityCoefficient   = 0.0;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 0;
	constantAcceleration = 3.0;
	lifetimeMS           = 800;
	lifetimeVarianceMS   = 100;
	spinSpeed     = 0;
	spinRandomMin = -90.0;
	spinRandomMax =  90.0;
	useInvAlpha   = false;

	colors[0]	= "1   1   0.3 0.0";
	colors[1]	= "1   1   0.3 1.0";
	colors[2]	= "0.6 0.0 0.0 0.0";

	sizes[0]	= 0.0;
	sizes[1]	= 1.0;
	sizes[2]	= 0.6;

	times[0]	= 0.0;
	times[1]	= 0.2;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(FireTrailEmitter)
{
   ejectionPeriodMS = 14;
   periodVarianceMS = 4;
   ejectionVelocity = 1;
   ejectionOffset   = 0.00;
   velocityVariance = 0.0;
   thetaMin         = 0;
   thetaMax         = 5;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = true;

   //lifetimeMS = 5000;
   particles = FireTrailParticle;   


};

datablock ParticleData(RBowFireParticle)
{
	textureName          = "base/data/particles/cloud";
	dragCoefficient      = 0.10;
	gravityCoefficient   = -15.10;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 0;
	constantAcceleration = 5.0;
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 50;
	spinSpeed     = 0;
	spinRandomMin = -90.0;
	spinRandomMax =  90.0;
	useInvAlpha   = false;

	colors[0]	= "1   1   0.3 0.0";
	colors[1]	= "1   1   0.3 1.0";
	colors[2]	= "0.6 0.0 0.0 0.0";

	sizes[0]	= 0.0;
	sizes[1]	= 0.5;
	sizes[2]	= 0.2;

	times[0]	= 0.0;
	times[1]	= 0.2;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(RBowFireEmitter)
{
   ejectionPeriodMS = 14;
   periodVarianceMS = 4;
   ejectionVelocity = 1;
   ejectionOffset   = 0.00;
   velocityVariance = 0.0;
   thetaMin         = 0;
   thetaMax         = 5;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   //lifetimeMS = 5000;
   particles = RBowFireParticle;   


};
datablock ParticleData(RecurveArrowTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1000;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/dot";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]	= "0.5 0.5 0.5 0.2";
	colors[1]	= "0.5 0.5 0.5 0.0";
	sizes[0]	= 0.1;
	sizes[1]	= 0.1;
	times[0]	= 0.0;
	times[1]	= 1.0;
};

datablock ParticleEmitterData(RecurveArrowTrailEmitter)
{
   ejectionPeriodMS = 2;
   periodVarianceMS = 0;

   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;

   ejectionOffset = 0;

   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = RecurveArrowTrailParticle;

   useEmitterColors = true;


};

datablock ExplosionData(RecurvearrowStickExplosion)
{
   //explosionShape = "";
	soundProfile = RecurveBowExplosionSound;

   lifeTimeMS = 150;

   particleEmitter = "";
   particleDensity = 10;
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
datablock ParticleData(RecurveBowFireExplosionParticle)
{
   textureName          = "./particles/smoke";
   dragCoefficient      = 2;
   gravityCoefficient   = 0.2;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 150;

   colors[0]     = "0.56 0.36 0.26 1.0";
   colors[1]     = "0.56 0.36 0.26 0.0";

   sizes[0]      = 0.5;
   sizes[1]      = 1.0;
};

datablock ParticleEmitterData(RecurveBowFireExplosionEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 2;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   particles = "RecurveBowFireExplosionParticle";
};

datablock ParticleData(RecurveBowFireExplosionSmoke)
{
   textureName          = "./particles/smoke";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = -0.80;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 300;
   useInvAlpha =  true;
   spinRandomMin = -80.0;
   spinRandomMax =  80.0;

   colors[0]     = "0.56 0.36 0.26 1.0";
   colors[1]     = "0.2 0.2 0.2 1.0";
   colors[2]     = "0.0 0.0 0.0 0.0";

   sizes[0]      = 1.0;
   sizes[1]      = 1.5;
   sizes[2]      = 2.0;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;

};

datablock ParticleEmitterData(RecurveBowFireExplosionSmokeEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 4;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "RecurveBowFireExplosionSmoke";
};

datablock ParticleData(RecurveBowFireExplosionSparks)
{
   textureName          = "./particles/spark";
   dragCoefficient      = 1;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 350;

   colors[0]     = "0.60 0.40 0.30 1.0";
   colors[1]     = "0.60 0.40 0.30 1.0";
   colors[2]     = "1.0 0.40 0.30 0.0";

   sizes[0]      = 0.5;
   sizes[1]      = 0.25;
   sizes[2]      = 0.25;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(RecurveBowFireExplosionSparkEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 13;
   velocityVariance = 6.75;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   orientParticles  = true;
   lifetimeMS       = 100;
   particles = "RecurveBowFireExplosionSparks";
};
datablock ExplosionData(RecurvearrowExplosion)
{
   //explosionShape = "";
	soundProfile = "";

   lifeTimeMS = 50;

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
datablock ExplosionData(RecurveBowFireSubExplosion1)
{
   offset = 1.0;
   emitter[0] = RecurveBowFireExplosionSmokeEmitter;
   emitter[1] = RecurveBowFireExplosionSparkEmitter;
};

datablock ExplosionData(RecurveBowFireSubExplosion2)
{
   offset = 1.0;
   emitter[0] = RecurveBowFireExplosionSmokeEmitter;
   emitter[1] = RecurveBowFireExplosionSparkEmitter;
};

datablock ExplosionData(RecurveBowFireExplosion)
{
   soundProfile   = RecurveBowFireFireSound;
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = RecurveBowFireExplosionEmitter;
   particleDensity = 80;
   particleRadius = 1;

   // Point emission
   emitter[0] = RecurveBowFireExplosionSmokeEmitter;
   emitter[1] = RecurveBowFireExplosionSparkEmitter;

   // Sub explosion objects
   subExplosion[0] = RecurveBowFireSubExplosion1;
   subExplosion[1] = RecurveBowFireSubExplosion2;
   
   // Camera Shaking
   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Dynamic light
   lightStartRadius = 2;
   lightEndRadius = 1;
   lightStartColor = "0.5 0.5 0";
   lightEndColor = "0 0 0";
   radiusDamage        = 30;
   damageRadius        = 10;
};

datablock ProjectileData(RecurveBowProjectile)
{
   projectileShapeName = "add-ons/weapon_bow/arrow.dts";
   directDamage        = 40;
   directDamageType    = $DamageType::ArrowDirect;

   radiusDamage        = 0;
   damageRadius        = 0;
   radiusDamageType    = $DamageType::ArrowDirect;



   particleEmitter       = RecurveArrowTrailEmitter;
   explodeOnPlayerImpact = true;
   explodeOnDeath        = true;  

   armingDelay         = 4000;
   lifetime            = 4000;
   fadeDelay           = 4000;

   isBallistic         = true;
   bounceAngle         = 170; //stick almost all the time
   minStickVelocity    = 10;
   bounceElasticity    = 0.2;
   bounceFriction      = 0.01;   
   gravityMod = 0.8;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   muzzleVelocity      = 150;
   velInheritFactor    = 1;
};
datablock ProjectileData(RecurveBowunchargedProjectile)
{
   projectileShapeName = "add-ons/weapon_bow/arrow.dts";
   directDamage        = 15;
   directDamageType    = $DamageType::ArrowDirect;

   radiusDamage        = 0;
   damageRadius        = 0;
   radiusDamageType    = $DamageType::ArrowDirect;



   explodeOnPlayerImpact = true;
   explodeOnDeath        = true;  

   armingDelay         = 4000;
   lifetime            = 4000;
   fadeDelay           = 4000;

   isBallistic         = true;
   bounceAngle         = 170; //stick almost all the time
   minStickVelocity    = 10;
   bounceElasticity    = 0.2;
   bounceFriction      = 0.01;   
   gravityMod = 0.8;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   muzzleVelocity      = 30;
   velInheritFactor    = 1;
};
datablock ProjectileData(RecurveBowFireProjectile)
{
   projectileShapeName = "add-ons/weapon_bow/arrow.dts";
   directDamage        = 75;
   directDamageType    = $DamageType::ArrowDirect;

   radiusDamage        = 30;
   damageRadius        = 10;
   radiusDamageType    = $DamageType::ArrowDirect;

   explosion  = RecurveBowFireExplosion;
   particleEmitter       = FireTrailEmitter;
   explodeOnPlayerImpact = true;
   explodeOnDeath        = true;  

   armingDelay         = 0;
   lifetime            = 4000;
   fadeDelay           = 4000;

   isBallistic         = true;
   bounceAngle         = 170; //stick almost all the time
   minStickVelocity    = 10;
   bounceElasticity    = 0.2;
   bounceFriction      = 0.01;   
   gravityMod = 0.8;

   hasLight    = true;
   lightStartRadius = 2;
   lightEndRadius = 1;
   lightStartColor = "0.5 0.5 0";
   lightEndColor = "0 0 0";

   muzzleVelocity      = 180;
   velInheritFactor    = 1;
};

//////////
// item //
//////////
datablock ItemData(RecurveBowItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/RBowItem.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Recurved Bow";
	iconName = "./icons/RBow";
	doColorShift = true;
	colorShiftColor = "0.882353 0.443137 0 1";

	 // Dynamic properties defined by the scripts
	image = RecurveBowImage;
	canDrop = true;
};

//function RecurveBow::onUse(%this,%user)
//{
//	//mount the image in the right hand slot
//	%user.mountimage(%this.image, $RightHandSlot);
//}

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(RecurveBowImage)
{
   // Basic Item properties
   shapeFile = "./shapes/RBow3.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0.3 0";
   //eyeOffset = "0.1 0.2 -0.55";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";
//exec("add-ons/weapon_recurvebow/server.cs");
   // Projectile && Ammo.
   item = RecurveBowItem;
   ammo = " ";
   projectile = RecurveBowProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise youwr arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.882353 0.443137 0 1";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.1;
	stateTransitionOnTimeout[0]	= "Ready";
	stateSequence[0]		= "ready";

	stateSound[0]					= weaponSwitchSound;

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Charge";
	stateAllowImageChange[1]	= true;
	stateScript[1]		= "onready";
	
	stateName[2]                    = "Charge";
	stateTransitionOnTimeout[2]	= "Armed";
	stateSound[2]				= RecurveBowChargeSound;
	stateTimeoutValue[2]            = 1;
	stateSequence[2]		= "Charge";
	stateWaitForTimeout[2]		= false;
	stateTransitionOnTriggerUp[2]	= "AbortCharge";

	stateAllowImageChange[2]        = false;
	
	stateName[3]			= "AbortCharge";
	stateTimeoutValue[3]		= 0.4;
	stateFire[3]			= true;
	stateSequence[3]		= "Fire";
	stateScript[3]			= "onFiretwo";
	stateWaitForTimeout[3]		= true;
	stateSound[3]				= RecurveBowFireSound;
	stateAllowImageChange[3]	= false;
	stateTransitionOnTimeout[3]	= "Reload";

	stateName[4]			= "Armed";
	stateTransitionOnTimeout[4]	= "ArmedwithFire";
	stateTimeoutValue[4]            = 5;
	stateWaitForTimeout[4]		= false;
	stateTransitionOnTriggerUp[4]	= "Fire";
	stateAllowImageChange[4]	= false;

	stateName[5]			= "Fire";
	stateTimeoutValue[5]		= 0.2;
	stateFire[5]			= true;
	stateSequence[5]		= "Fire";
	stateScript[5]			= "onFire";
	stateWaitForTimeout[5]		= true;
	stateAllowImageChange[5]	= false;
	stateSound[5]				= RecurveBowFireSound;
	stateTransitionOnTimeout[5]	= "Reload";
	//--------------------------------------------------------------------------------------Skip over
		stateName[6]			= "Reload";
	stateTimeoutValue[6]		= 1.0;
	stateTransitionOnTimeout[6]	= "Ready";
	stateSequence[6]		= "reload";
		stateAllowImageChange[6]	= false;
		

	
	stateName[7]			= "ArmedwithFire";
	stateTimeoutValue[7]		= 0.3;
	stateTransitionOnTriggerUp[7]	= "FirewithFire";
	stateEmitter[7]					= RBowFireEmitter;//exec("add-ons/weapon_recurvebow/server.cs");
	stateEmitterTime[7]				= 0.4;
	stateEmitterNode[7]				= "muzzleNode";
	stateAllowImageChange[7]	= false;
		stateWaitForTimeout[7]		= False;
	stateTransitionOnTimeout[7]	= "ArmedwithFire";
	
	stateName[8]			= "FirewithFire";
	stateTimeoutValue[8]		= 0.2;
	stateFire[8]			= true;
	stateSequence[8]		= "Fire";
	stateScript[8]			= "onFireFire";
	stateWaitForTimeout[8]		= true;
	stateAllowImageChange[8]	= false;
	stateSound[8]				= RecurveBowFireSound;
	stateTransitionOnTimeout[8]	= "Reload";
};
function RecurveBowImage::onFiretwo(%this,%obj,%slot)
{

	

	%projectile = RecurveBowunchargedprojectile;
	%spread = 0.1;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
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
	return %p;
}
function RecurveBowImage::onFireFire(%this,%obj,%slot)
{

	

	%projectile = RecurveBowFireprojectile;
	%spread = 0.00001;
	%shellcount = 1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 2 * 3.1415926 * %spread;
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
	return %p;
}
function RecurveBowImage::onMount(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}

function RecurveBowImage::onUnMount(%this, %obj, %slot)
{
	  	%obj.unhideNode(lhand);
		%obj.unhideNode(rhand);
}

function RecurveBowImage::onReady(%this, %obj, %slot)
{
	  	 %obj.hideNode(lhand);
	  	 %obj.hideNode(lhook);
	     %obj.hideNode(rhand);
	  	 %obj.hideNode(rhook);
}













