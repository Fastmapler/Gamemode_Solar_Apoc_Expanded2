//
// common particle effects for the adventurer pack
//

 // small bullet trails and fire particles
 /////////////////////////////////////////////////////

datablock ParticleData(BAADPistolTrailParticle)
{
	dragCoefficient		= 5000.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
    colors[0]     = "0.9 0.45 0 0.2";
    colors[1]     = "0.9 0.5 0 0.1";
    colors[2]     = "1 0.55 0.2 0.0";
	sizes[0]	= 0.1;
	sizes[1]	= 0.04;
	sizes[2]	= 0.0;
	times[0]	= 0.0;
	times[1]	= 0.25;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(BAADPistolTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = BAADPistolTrailParticle;

   useEmitterColors = true;
};

datablock ParticleData(BAADRifleTrailParticle)
{
	dragCoefficient		= 5000.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
    colors[0]     = "0.9 0.45 0 0.2";
    colors[1]     = "0.9 0.5 0 0.1";
    colors[2]     = "1 0.55 0.2 0.0";
	sizes[0]	= 0.2;
	sizes[1]	= 0.1;
	sizes[2]	= 0.0;
	times[0]	= 0.0;
	times[1]	= 0.25;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(BAADRifleTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = BAADRifleTrailParticle;

   useEmitterColors = true;
};

datablock ParticleData(BAADSilencedTrailParticle)
{
	dragCoefficient		= 5000.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
    colors[0]     = "1.0 0.5 0 0.0";
    colors[1]     = "0.9 0.4 0 0.0";
    colors[2]     = "1 0.5 0.2 0.0";
	sizes[0]	= 0.15;
	sizes[1]	= 0.08;
	sizes[2]	= 0.0;
	times[0]	= 0.0;
	times[1]	= 0.25;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(BAADSilencedTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = BAADSilencedTrailParticle;

   useEmitterColors = true;
};

datablock ParticleData(AutogunExplosionParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 450;
	lifetimeVarianceMS   = 200;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "0.9 0.9 0.9 0.3";
	colors[1]     = "0.9 0.5 0.6 0.0";
	sizes[0]      = 0.5;
	sizes[1]      = 0.75;

	useInvAlpha = true;
};
datablock ParticleEmitterData(AutogunExplosionEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 2;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "AutogunExplosionParticle";

};

datablock ParticleData(AutogunExplosionRingParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 50;
	lifetimeVarianceMS   = 25;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 500.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.5 0.25 0.15 1";
	colors[1]     = "0.4 0.15 0.05 0.0";
	sizes[0]      = 1;
	sizes[1]      = 0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(AutogunExplosionRingEmitter)
{
	lifeTimeMS = 50;

   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   useEmitterColors = true;
   particles = "AutogunExplosionRingParticle";
};

datablock ParticleData(advSmallBulletFireParticle)
{
    dragCoefficient      = 0;
    gravityCoefficient   = 0;
    inheritedVelFactor   = 0;
    constantAcceleration = 0.0;
    lifetimeMS           = 50;
    lifetimeVarianceMS   = 0;
    textureName          = "base/data/particles/star1";
    spinSpeed        = 9000.0;
    spinRandomMin        = -5000.0;
    spinRandomMax        = 5000.0;

    colors[0]     = "1.0 0.5 0 0.9";
    colors[1]     = "0.9 0.4 0 0.8";
    colors[2]     = "1 0.5 0.2 0.6";
    colors[3]     = "1 0.5 0.2 0.4";

    sizes[0]      = 1.15;
   sizes[1]      = 0.4;
    sizes[2]      = 0.10;
    sizes[3]      = 0.0;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.5;
   times[3] = 1.0;

    useInvAlpha = false;
};
datablock ParticleEmitterData(advSmallBulletFireEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 54.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   particles = "advSmallBulletFireParticle";
};

datablock ParticleData(advSmallBulletSmokeParticle)
{
    dragCoefficient      = 3;
    gravityCoefficient   = 0;
    inheritedVelFactor   = 0;
    constantAcceleration = 0.0;
    lifetimeMS           = 100;
    lifetimeVarianceMS   = 0;
    textureName          = "base/data/particles/cloud";
    spinSpeed        = 9000.0;
    spinRandomMin        = -5000.0;
    spinRandomMax        = 5000.0;

    colors[0]     = "0.6 0.6 0.6 0.0";
    colors[1]     = "0.7 0.7 0.7 0.3";
    colors[2]     = "1 1 1 0.2";
    colors[3]     = "1 1 1 0";

    sizes[0]      = 0.0;
   sizes[1]      = 0.4;
    sizes[2]      = 0.10;
    sizes[3]      = 0.05;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.5;
   times[3] = 1.0;

    useInvAlpha = false;
};
datablock ParticleEmitterData(advSmallBulletSmokeEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 32.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   particles = "advSmallBulletSmokeParticle";
};

datablock ParticleData(advSmallBulletTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 80;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]	= "1 1 0.5 0.1";
	colors[1]	= "1 1 0.7 0.08";
	colors[2]	= "0.9 0.9 0.9 0";
	sizes[0]	= 0.2;
	sizes[1]	= 0.15;
	sizes[2]	= 0.0;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};
datablock ParticleEmitterData(advSmallBulletTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = advSmallBulletTrailParticle;
};

 // big bullet trails and fire particles
 /////////////////////////////////////////////////////

datablock ParticleData(advBigBulletFireParticle)
{
    dragCoefficient      = 0;
    gravityCoefficient   = 0;
    inheritedVelFactor   = 0;
    constantAcceleration = 0.0;
    lifetimeMS           = 50;
    lifetimeVarianceMS   = 0;
    textureName          = "base/data/particles/star1";
    spinSpeed        = 9000.0;
    spinRandomMin        = -5000.0;
    spinRandomMax        = 5000.0;

    colors[0]     = "1.0 0.5 0 0.9";
    colors[1]     = "0.9 0.4 0 0.8";
    colors[2]     = "1 0.5 0.2 0.6";
    colors[3]     = "1 0.5 0.2 0.4";

    sizes[0]      = 1.75;
   sizes[1]      = 0.4;
    sizes[2]      = 0.10;
    sizes[3]      = 0.0;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.5;
   times[3] = 1.0;

    useInvAlpha = false;
};
datablock ParticleEmitterData(advBigBulletFireEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 64.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   particles = "advBigBulletFireParticle";
};

datablock ParticleData(advBigBulletSmokeParticle)
{
    dragCoefficient      = 3;
    gravityCoefficient   = 0;
    inheritedVelFactor   = 0;
    constantAcceleration = 0.0;
    lifetimeMS           = 100;
    lifetimeVarianceMS   = 0;
    textureName          = "base/data/particles/cloud";
    spinSpeed        = 9000.0;
    spinRandomMin        = -5000.0;
    spinRandomMax        = 5000.0;

    colors[0]     = "0.6 0.6 0.6 0.0";
    colors[1]     = "0.7 0.7 0.7 0.3";
    colors[2]     = "1 1 1 0.2";
    colors[3]     = "1 1 1 0";

    sizes[0]      = 0.0;
   sizes[1]      = 0.4;
    sizes[2]      = 0.10;
    sizes[3]      = 0.05;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.5;
   times[3] = 1.0;

    useInvAlpha = false;
};
datablock ParticleEmitterData(advBigBulletSmokeEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 32.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   particles = "advBigBulletSmokeParticle";
};

datablock ParticleData(advBigBulletTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 190;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]	= "1 1 0.5 0.1";
	colors[1]	= "1 1 0.7 0.08";
	colors[2]	= "0.9 0.9 0.9 0";
	sizes[0]	= 0.3;
	sizes[1]	= 0.15;
	sizes[2]	= 0.0;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};
datablock ParticleEmitterData(advBigBulletTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = advBigBulletTrailParticle;
};

 // huge bullet trails and fire particles
 /////////////////////////////////////////////////////

datablock ParticleData(advHugeBulletFireParticle)
{
    dragCoefficient      = 0;
    gravityCoefficient   = 0;
    inheritedVelFactor   = 0;
    constantAcceleration = 0.0;
    lifetimeMS           = 50;
    lifetimeVarianceMS   = 0;
    textureName          = "base/data/particles/star1";
    spinSpeed        = 9000.0;
    spinRandomMin        = -5000.0;
    spinRandomMax        = 5000.0;

    colors[0]     = "1.0 0.5 0 0.9";
    colors[1]     = "0.9 0.4 0 0.8";
    colors[2]     = "1 0.5 0.2 0.6";
    colors[3]     = "1 0.5 0.2 0.4";

    sizes[0]      = 2.75;
   sizes[1]      = 1.3;
    sizes[2]      = 0.10;
    sizes[3]      = 0.0;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.5;
   times[3] = 1.0;

    useInvAlpha = false;
};
datablock ParticleEmitterData(advHugeBulletFireEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 96.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   particles = "advHugeulletFireParticle";
};

datablock ParticleData(advHugeBulletSmokeParticle)
{
    dragCoefficient      = 3;
    gravityCoefficient   = 0;
    inheritedVelFactor   = 0;
    constantAcceleration = 0.0;
    lifetimeMS           = 100;
    lifetimeVarianceMS   = 0;
    textureName          = "base/data/particles/cloud";
    spinSpeed        = 9000.0;
    spinRandomMin        = -5000.0;
    spinRandomMax        = 5000.0;

    colors[0]     = "0.6 0.6 0.6 0.0";
    colors[1]     = "0.7 0.7 0.7 0.1";
    colors[2]     = "1 1 1 0.1";
    colors[3]     = "1 1 1 0";

    sizes[0]      = 0.0;
   sizes[1]      = 0.7;
    sizes[2]      = 0.10;
    sizes[3]      = 0.05;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.5;
   times[3] = 1.0;

    useInvAlpha = false;
};
datablock ParticleEmitterData(advHugeBulletSmokeEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 32.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   particles = "advHugeBulletSmokeParticle";
};

datablock ParticleData(advHugeBulletTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 190;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]	= "1 1 0.5 0.1";
	colors[1]	= "1 1 0.7 0.08";
	colors[2]	= "0.9 0.9 0.9 0";
	sizes[0]	= 0.9;
	sizes[1]	= 0.55;
	sizes[2]	= 0.0;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};
datablock ParticleEmitterData(advHugeBulletTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = advHugeBulletTrailParticle;
};

// new shit

datablock DebrisData(BAADPistolDebris) 
{
	shapeFile = "./shapes/weapons/BAADSMGShell.dts";
	lifetime = 2.0;
	minSpinSpeed = 700.0;
	maxSpinSpeed = 800.0;
	elasticity = 0.5;
	friction = 0.1;
	numBounces = 3;
	staticOnMaxBounce = false;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};

datablock DebrisData(BAADBigPistolDebris) 
{
	shapeFile = "./shapes/weapons/BAADBigSMGShell.dts";
	lifetime = 2.0;
	minSpinSpeed = 700.0;
	maxSpinSpeed = 800.0;
	elasticity = 0.5;
	friction = 0.1;
	numBounces = 3;
	staticOnMaxBounce = false;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};

datablock DebrisData(BAADRifleDebris) 
{
	shapeFile = "./shapes/weapons/BAADRifleShell.dts";
	lifetime = 2.0;
	minSpinSpeed = 700.0;
	maxSpinSpeed = 800.0;
	elasticity = 0.5;
	friction = 0.1;
	numBounces = 3;
	staticOnMaxBounce = false;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};

datablock DebrisData(BAADBigRifleDebris) 
{
	shapeFile = "./shapes/weapons/BAADBigRifleShell.dts";
	lifetime = 2.0;
	minSpinSpeed = 700.0;
	maxSpinSpeed = 800.0;
	elasticity = 0.5;
	friction = 0.1;
	numBounces = 3;
	staticOnMaxBounce = false;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};

datablock DebrisData(BAADShotgunDebris) 
{
	shapeFile = "./shapes/weapons/BAADShotgunShell.dts";
	lifetime = 2.0;
	minSpinSpeed = 700.0;
	maxSpinSpeed = 800.0;
	elasticity = 0.5;
	friction = 0.1;
	numBounces = 3;
	staticOnMaxBounce = false;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};

datablock ExplosionData(QuietGunExplosion : gunExplosion)
{
   lifeTimeMS = 100;
   
   soundProfile = QuietImpactSound;
 
   particleEmitter = AutogunExplosionEmitter;
   particleDensity = 5;
   particleRadius = 0.2;

   emitter[0] = GunExplosionRingEmitter;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "1.0 1.0 1.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 1.5;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 1;
   lightStartColor = "1.0 0.75 0.25";
   lightEndColor = "0 0 0";
};

datablock AudioProfile(QuietImpactSound)
{
   filename    = "./Sounds/Physics/QuietBulletHit.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock ExplosionData(QuieterGunExplosion : gunExplosion)
{
   lifeTimeMS = 100;
   
   soundProfile = QuieterImpactSound;
 
   particleEmitter = AutogunExplosionEmitter;
   particleDensity = 5;
   particleRadius = 0.2;

   emitter[0] = GunExplosionRingEmitter;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "1.0 1.0 1.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 1.5;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 1;
   lightStartColor = "1.0 0.75 0.25";
   lightEndColor = "0 0 0";
};

datablock AudioProfile(QuieterImpactSound)
{
   filename    = "./Sounds/Physics/QuieterBulletHit.wav";
   description = AudioDefault3d;
   preload = true;
};