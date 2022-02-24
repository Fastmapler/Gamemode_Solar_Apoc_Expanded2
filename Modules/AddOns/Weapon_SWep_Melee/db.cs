$SwolMelee_TmpDatablockCnt = datablockGroup.getCount();
//Blunt Swings
datablock AudioProfile(bluntSwing1Sound){						preload	= true;	filename = "./sound/swing_blunt1.wav";		description = AudioDefault3d;};
datablock AudioProfile(bluntSwing2Sound				: bluntSwing1Sound){		filename = "./sound/swing_blunt2.wav";};
datablock AudioProfile(bluntSwing3Sound				: bluntSwing1Sound){		filename = "./sound/swing_blunt3.wav";};
datablock AudioProfile(bluntSwing4Sound				: bluntSwing1Sound){		filename = "./sound/swing_blunt4.wav";};
//Large Blunt Swings
datablock AudioProfile(largeBluntSwing1Sound		: bluntSwing1Sound){		filename = "./sound/swing_largeBlunt1.wav";};
datablock AudioProfile(largeBluntSwing2Sound		: bluntSwing1Sound){		filename = "./sound/swing_largeBlunt2.wav";};
//Blade Swings
datablock AudioProfile(bladeSwing1Sound				: bluntSwing1Sound){		filename = "./sound/swing_blade1.wav";};
datablock AudioProfile(bladeSwing2Sound				: bluntSwing1Sound){		filename = "./sound/swing_blade2.wav";};
//Large Blade Swings
datablock AudioProfile(largeBladeSwing1Sound		: bluntSwing1Sound){		filename = "./sound/swing_largeBlade1.wav";};
datablock AudioProfile(largeBladeSwing2Sound		: bluntSwing1Sound){		filename = "./sound/swing_largeBlade2.wav";};
//Fist Swings
datablock AudioProfile(fistSwing1Sound				: bluntSwing1Sound){		filename = "./sound/swing_fist1.wav";};
datablock AudioProfile(fistSwing2Sound				: bluntSwing1Sound){		filename = "./sound/swing_fist2.wav";};
datablock AudioProfile(footSwingSound				: bluntSwing1Sound){		filename = "./sound/swing_foot.wav";};
//Parry Sounds
datablock AudioProfile(metal_metalParry1Sound		: bluntSwing1Sound){		filename = "./sound/metal_parryMetal1.wav";	description = AudioClose3d;};
datablock AudioProfile(metal_metalParry2Sound		: bluntSwing1Sound){		filename = "./sound/metal_parryMetal2.wav";	description = AudioClose3d;};
datablock AudioProfile(metal_woodParry1Sound		: bluntSwing1Sound){		filename = "./sound/metal_parryWood1.wav";	description = AudioClose3d;};
datablock AudioProfile(metal_woodParry2Sound		: bluntSwing1Sound){		filename = "./sound/metal_parryWood2.wav";	description = AudioClose3d;};
datablock AudioProfile(wood_woodParry1Sound			: bluntSwing1Sound){		filename = "./sound/wood_parryWood1.wav";	description = AudioClose3d;};
datablock AudioProfile(wood_woodParry2Sound			: bluntSwing1Sound){		filename = "./sound/wood_parryWood2.wav";	description = AudioClose3d;};

//Machete Sounds
datablock AudioProfile(macheteHitEnv1Sound			: bluntSwing1Sound){		filename = "./sound/machete_hitEnv1.wav";	description = AudioClose3d;};
datablock AudioProfile(macheteHitEnv2Sound			: bluntSwing1Sound){		filename = "./sound/machete_hitEnv2.wav";	description = AudioClose3d;};
datablock AudioProfile(macheteHitPl1Sound			: bluntSwing1Sound){		filename = "./sound/machete_hitPl1.wav";	description = AudioClose3d;};
datablock AudioProfile(macheteHitPl2Sound			: bluntSwing1Sound){		filename = "./sound/machete_hitPl2.wav";	description = AudioClose3d;};
//BaseballBat Sounds
datablock AudioProfile(baseballBatHitEnv1Sound		: bluntSwing1Sound){		filename = "./sound/baseballbat_hitEnv1.wav";};
datablock AudioProfile(baseballBatHitEnv2Sound		: bluntSwing1Sound){		filename = "./sound/baseballbat_hitEnv2.wav";};
datablock AudioProfile(baseballBatHitPl1Sound		: bluntSwing1Sound){		filename = "./sound/baseballbat_hitPl1.wav";};
datablock AudioProfile(baseballBatHitPl2Sound		: bluntSwing1Sound){		filename = "./sound/baseballbat_hitPl2.wav";};
//SpikeBat Sounds
datablock AudioProfile(spikeBatHitPl1Sound			: bluntSwing1Sound){		filename = "./sound/spikeBat_hitPl1.wav";};
datablock AudioProfile(spikeBatHitPl2Sound			: bluntSwing1Sound){		filename = "./sound/spikeBat_hitPl2.wav";};
//Pipewrench Sounds
datablock AudioProfile(pipeWrenchHitEnv1Sound		: bluntSwing1Sound){		filename = "./sound/pipeWrench_hitEnv1.wav";};
datablock AudioProfile(pipeWrenchHitEnv2Sound		: bluntSwing1Sound){		filename = "./sound/pipeWrench_hitEnv2.wav";};
datablock AudioProfile(pipeWrenchHitPl1Sound		: bluntSwing1Sound){		filename = "./sound/pipeWrench_hitPl1.wav";};
datablock AudioProfile(pipeWrenchHitPl2Sound		: bluntSwing1Sound){		filename = "./sound/pipeWrench_hitPl2.wav";};
//Sledgehammer Sounds
datablock AudioProfile(sledgeHammerHitEnv1Sound		: bluntSwing1Sound){		filename = "./sound/sledgeHammer_hitEnv1.wav";};
datablock AudioProfile(sledgeHammerHitEnv2Sound		: bluntSwing1Sound){		filename = "./sound/sledgeHammer_hitEnv2.wav";};
datablock AudioProfile(sledgeHammerHitPl1Sound		: bluntSwing1Sound){		filename = "./sound/sledgeHammer_hitPl1.wav";};
datablock AudioProfile(sledgeHammerHitPl2Sound		: bluntSwing1Sound){		filename = "./sound/sledgeHammer_hitPl2.wav";};
//Fryin Pan Sounds
datablock AudioProfile(fryingPanHitEnv1Sound		: bluntSwing1Sound){		filename = "./sound/fryingPan_hitEnv1.wav";};
datablock AudioProfile(fryingPanHitEnv2Sound		: bluntSwing1Sound){		filename = "./sound/fryingPan_hitEnv2.wav";};
datablock AudioProfile(fryingPanHitPl1Sound			: bluntSwing1Sound){		filename = "./sound/fryingPan_hitPl1.wav";};
datablock AudioProfile(fryingPanHitPl2Sound			: bluntSwing1Sound){		filename = "./sound/fryingPan_hitPl2.wav";};
//Meat Cleaver Sounds
datablock AudioProfile(meatCleaverHitPl1Sound		: bluntSwing1Sound){		filename = "./sound/meatCleaver_hitPl1.wav";};
datablock AudioProfile(meatCleaverHitPl2Sound		: bluntSwing1Sound){		filename = "./sound/meatCleaver_hitPl2.wav";};
//Fist Sounds
datablock AudioProfile(fistHitEnv1Sound				: bluntSwing1Sound){		filename = "./sound/fist_hitEnv1.wav";};
datablock AudioProfile(fistHitEnv2Sound				: bluntSwing1Sound){		filename = "./sound/fist_hitEnv2.wav";};
datablock AudioProfile(fistHitPl1Sound				: bluntSwing1Sound){		filename = "./sound/fist_hitPl1.wav";};
datablock AudioProfile(fistHitPl2Sound				: bluntSwing1Sound){		filename = "./sound/fist_hitPl2.wav";};
datablock AudioProfile(footHitPlSound				: bluntSwing1Sound){		filename = "./sound/foot_hitPl.wav";};

//Kurgan Sounds
datablock AudioProfile(thereCanOnlyBeOneSound		: bluntSwing1Sound){		filename = "./sound/therecanonlybeone.wav";};

function swolMelee_addSFX(%type,%db)
{
	$SwolMelee_SFX[%type,($SwolMelee_SFXCnt[%type]|0)] = %db;
	$SwolMelee_SFXCnt[%type]++;
}
function swolMelee_getSFX(%type)
{
	if((%cnt = $SwolMelee_SFXCnt[%type]) $= "")
		return;
	return $SwolMelee_SFX[%type,getRandom(0,%cnt-1)];
}

function swolMelee_AUTO_ASSEMBLE_SOUNDS()
{
	%cnt = (%g=datablockGroup).getCount();
	%pCnt = getWordCount(%pivot = "Swing Parry HitEnv HitPl");
	for(%i=$SwolMelee_TmpDatablockCnt;%i<%cnt;%i++)
	{
		if(!(%db = %g.getObject(%i)).preload)
			continue;
		%name = %db.getName();
		for(%a=0;%a<%pCnt;%a++)
		{
			if((%tmpA = strPos(%name,%tmpB = getWord(%pivot,%a))) != -1)
			{
				if(%tmpB $= "Parry")
				{
					%oA = getSubStr(%name,0,%tmpPiv = strPos(%name,"_"));
					%oB = getSubStr(%name,%tmpPiv+1,%tmpA-(%tmpPiv+1));
					swolMelee_addSFX(%oA @ "_" @ %oB @ "_" @ %tmpB,%db);
					if(%oA !$= %oB)
						swolMelee_addSFX(%oB @ "_" @ %oA @ "_" @ %tmpB,%db);
				}
				else
					swolMelee_addSFX(getSubStr(%name,0,%tmpA) @ "_" @ %tmpB,%db);
				break;
			}
		}
	}
	$SwolMelee_TmpDatablockCnt = "";
}
swolMelee_AUTO_ASSEMBLE_SOUNDS();
function swolMelee_AUTO_ASSEMBLE_SOUNDS(){}

function swolMelee_createMeleeItem(%name,%ui,%a0,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16)
{
	%csh = getField(%ui,1);
	%ui = getField(%ui,0);
	%basePath = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Addons/Weapon_Swep_Melee/";
	if($DamageType["::" @ %name] $= "")
		AddDamageType(%name,addTaggedString("<bitmap:" @ %basePath @ "model/icon/ci_" @ %name @ "> %1"),addTaggedString("%2 <bitmap:" @ %basePath @ "model/icon/ci_" @ %name @ "> %1"),0.2,1);
	%str = "datablock ItemData(" @ %name @ "Item){category=\"Weapon\";className=\"Weapon\";weaponClass=\"melee\";rotate=false;mass=1;density=0.2;elasticity=0.2;friction=0.6;emap=1;canDrop=1;isSMelee=1;";
	%str = %str @ "image=" @ %name @ "Image;shapeFile=\"" @ (%name $= "fist" ? "base/data/shapes/empty.dts" : %basePath @ "model/" @ %name @ ".dts") @ "\";iconName=\"" @ %basePath @ "model/icon/icon_" @ %name @ "\";uiName=\"" @ %ui @ "\";smDamageType=$DamageType::" @ %name @ ";";
	if(%csh !$= "")
		%str = %str @ "doColorShift=1;colorShiftColor=\"" @ %csh @ "\";";
	for(%i=0;%a[%i]!$="";%i++)
		%str = %str @ "sm" @ getWord(%a[%i],0) @ "=\"" @ getWords(%a[%i],1,getWordCount(%a[%i])) @ "\";";
	%str = %str @ "};datablock ShapeBaseImageData(" @ %name @ "Image" @ "){className=\"WeaponImage\";emap=1;mountPoint=0;" @ (%csh !$= "" ? "doColorShift=1;colorShiftColor=" @ %name @ "Item.colorShiftColor;":"") @ "shapeFile=" @ %name @ "Item.shapeFile;item=" @ %name @ "Item;";
	if(%a[%i+1]!$="")
		for(%i=%i+1;%a[%i]!$="";%i++)
			%str = %str @ getWord(%a[%i],0) @ "=\"" @ getWords(%a[%i],1,getWordCount(%a[%i])) @ "\";";
	eval(%str @ "};");
}
function swolMelee_addSwingAnim(%str,%animName,%windup)
{
	$SwolMelee_Anim[%str] = %animName;
	$SwolMelee_AnimWindup[%str] = %windup;
	//exec("./data/" @ %str @ ".cs");
}
swolMelee_addSwingAnim("A","swingA",140);
swolMelee_addSwingAnim("B","swingB",90);
swolMelee_addSwingAnim("SB","stabB",90);
swolMelee_addSwingAnim("F","punchR",90);

swolMelee_createMeleeItem("meatcleaver",
	"SM2 - Meat Cleaver" TAB	"0.47 0.35 0.2 1",
	SwingSound SPC		"blade_Swing",
	HitPlSound SPC		"meatCleaver_HitPl",
	HitEnvSound SPC		"machete_HitEnv",
	Material SPC		"metal",
	SwingAnim SPC		"A",
	SwingDelay SPC		450,
	Length SPC			2,
	Damage SPC			50,
	Force SPC			2,
	Stun SPC			0
);

swolMelee_createMeleeItem("pipeWrench",
	"SM3 - Pipe Wrench" TAB	"0.32 0.32 0.32 1",
	SwingSound SPC		"blunt_Swing",
	HitPlSound SPC		"pipeWrench_HitPl",
	HitEnvSound SPC		"pipeWrench_HitEnv",
	Material SPC		"metal",
	SwingAnim SPC		"A",
	SwingDelay SPC		450,
	Length SPC			2,
	Damage SPC			35,
	Force SPC			6,
	Stun SPC			600,
	"",
	offset SPC			"0 0 -0.12"
);

swolMelee_createMeleeItem("fryingpan",
	"SM2 - Frying Pan" TAB	"0.2 0.2 0.2 1",
	SwingSound SPC		"blunt_Swing",
	HitPlSound SPC		"fryingpan_HitPl",
	HitEnvSound SPC		"fryingpan_HitEnv",
	Material SPC		"metal",
	SwingAnim SPC		"A",
	SwingDelay SPC		550,
	Length SPC			3,
	Damage SPC			40,
	Force SPC			6,
	Stun SPC			600,
	"",
	offset SPC			"0 -0.06 -0.15",
	rotation SPC		"0 0 1 36"
);

swolMelee_createMeleeItem("baseballBat",
	"SM1 - Baseball Bat" TAB	"0.47 0.35 0.2 1",
	SwingSound SPC		"blunt_Swing",
	HitPlSound SPC		"baseballBat_HitPl",
	HitEnvSound SPC		"baseballBat_HitEnv",
	Material SPC		"wood",
	SwingAnim SPC		"A",
	SwingDelay SPC		750,
	Length SPC			4,
	Damage SPC			45,
	Force SPC			13,
	TwoHanded SPC		0,
	Stun SPC			1200
);

swolMelee_createMeleeItem("machete",
	"SM3 - Machete" TAB		"0.47 0.35 0.2 1",
	SwingSound SPC		"largeBlade_Swing",
	HitPlSound SPC		"machete_HitPl",
	HitEnvSound SPC		"machete_HitEnv",
	Material SPC		"metal",
	SwingAnim SPC		"A",
	SwingDelay SPC		950,
	Length SPC			4,
	Damage SPC			90,
	Force SPC			4,
	Decapitate SPC		1
);

swolMelee_createMeleeItem("sledgeHammer",
	"SM5 - Sledgehammer" TAB	"0.47 0.35 0.2 1",
	SwingSound SPC		"largeBlunt_Swing",
	HitPlSound SPC		"sledgeHammer_HitPl",
	HitEnvSound SPC		"sledgeHammer_HitEnv",
	Material SPC		"wood",
	SwingAnim SPC		"B",
	SwingDelay SPC		1500,
	Length SPC			5,
	Damage SPC			150,
	Force SPC			20,
	Stun SPC			900,
	TwoHanded SPC		1,
	Decapitate SPC		1
);
//Blacklisted
//swolMelee_createMeleeItem("hockeystick",
//	"Hockey Stick" TAB	"0.65 0.55 0.45 1",
//	SwingSound SPC		"largeBlunt_Swing",
//	HitPlSound SPC		"baseballBat_HitPl",
//	HitEnvSound SPC		"baseballBat_HitEnv",
//	Material SPC		"wood",
//	SwingAnim SPC		"B",
//	SwingDelay SPC		600,
//	Length SPC			5,
//	Damage SPC			90,
//	Force SPC			15,
//	Stun SPC			1300,
//	TwoHanded SPC		1,
//	"",
//	rotation SPC		"0 0 1 180"
//);
swolMelee_createMeleeItem("shovel",
	"SM4 - Shovel" TAB		"0.47 0.35 0.2 1",
	SwingSound SPC		"blunt_Swing",
	HitPlSound SPC		"sledgeHammer_HitPl",
	HitEnvSound SPC		"sledgeHammer_HitEnv",
	Material SPC		"wood",
	SwingAnim SPC		"SB",
	SwingDelay SPC		500,
	Length SPC			5,
	Damage SPC			90,
	Force SPC			15,
	Stun SPC			1300,
	TwoHanded SPC		1,
	Decapitate SPC		1,
	"",
	offset SPC			"0 0 -0.1"
);

swolMelee_createMeleeItem("pitchfork",
	"SM4 - Pitchfork" TAB 	"0.47 0.35 0.2 1",
	SwingSound SPC		"blunt_swing",
	HitPlSound SPC		"machete_HitPl",
	HitEnvSound SPC		"machete_HitEnv",
	Material SPC		"metal",
	SwingAnim SPC		"SB",
	SwingDelay SPC		1300,
	Length SPC			5,
	Damage SPC			150,
	Force SPC			15,
	Stun SPC			0,
	TwoHanded SPC		1
);
swolMelee_createMeleeItem("spikeBat",
	"SM1 - Spike Bat" TAB		"0.47 0.35 0.2 1",
	SwingSound SPC		"blunt_Swing",
	HitPlSound SPC		"spikeBat_HitPl",
	HitEnvSound SPC		"baseballBat_HitEnv",
	Material SPC		"wood",
	SwingAnim SPC		"A",
	SwingDelay SPC		850,
	Length SPC			4,
	Damage SPC			75,
	Force SPC			15,
	Stun SPC			0
);

swolMelee_createMeleeItem("kurgan",
	"SM5 - Kurgan Great Sword" TAB "0.85 0.85 0.85 1",
	SwingSound SPC		"largeBlade_Swing",
	HitPlSound SPC		"machete_HitPl",
	HitEnvSound SPC		"machete_HitEnv",
	Material SPC		"metal",
	SwingAnim SPC		"B",
	SwingDelay SPC		1600,
	Length SPC			6,
	Damage SPC			150,
	Force SPC			15,
	Stun SPC			0,
	TwoHanded SPC		1,
	Decapitate SPC		1,
	"",
	offset SPC			"0 0 0.22"
);
//Blacklisted
//swolMelee_createMeleeItem("fist",
//	"Fists" TAB	"",
//	SwingSound SPC		"fist_swing",
//	HitPlSound SPC		"fist_HitPl",
//	HitEnvSound SPC		"fist_HitEnv",
//	Material SPC		"wood",
//	SwingAnim SPC		"F",
//	SwingDelay SPC		250,
//	Length SPC			1,
//	Damage SPC			20,
//	Force SPC			2,
//	Stun SPC			600,
//	TwoHanded SPC		1
//);

datablock PlayerData(playerFrozen : PlayerStandardArmor)
{
	runForce = 1800;
	runEnergyDrain = 0;
	minRunEnergy = 0;
	maxForwardSpeed = 0;
	maxBackwardSpeed = 0;
	maxSideSpeed = 0;
	maxForwardCrouchSpeed = 0;
	maxBackwardCrouchSpeed = 0;
	maxSideCrouchSpeed = 0;
	jumpForce = 0;
	jumpDelay = 0;
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;
	runSurfaceAngle  = 150;
	jumpSurfaceAngle = 150;
	uiName = "";
};

datablock PlayerData(meleeAnimPlayer : playerStandardArmor)
{
	shapeFile = "./model/hand.dts";
	boundingBox = "2 2 2";
	crouchboundingBox = "0.01 0.01 0.01";
	isEmptyPlayer = true;
	useCustomPainEffects = true;
	deleteOnDrop = true;
	deathSound = "";
	painSound = "";
	uiName = "";
};
datablock PlayerData(meleeFistsPlayer : playerStandardArmor)
{
	shapeFile = "./model/fists.dts";
	boundingBox = "2 2 2";
	crouchboundingBox = "0.01 0.01 0.01";
	isEmptyPlayer = true;
	useCustomPainEffects = true;
	deleteOnDrop = true;
	deathSound = "";
	painSound = "";
	uiName = "";
};

function swolMelee_createMeleeItem(){}

datablock ParticleData(meleeDustExplosionParticle)
{
	dragCoefficient		= 1;
	gravityCoefficient	= 0.4;
	inheritedVelFactor	= 0.2;
	constantAcceleration= 0.0;
	lifetimeMS			= 500;
	lifetimeVarianceMS	= 100;
	textureName			= "base/data/particles/cloud";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]			= "0.8 0.8 0.6 0.3";
	colors[1]			= "0.8 0.8 0.6 0.0";
	sizes[0]			= 0.75;
	sizes[1]			= 1.5;
	useInvAlpha 		= true;
};

datablock ParticleEmitterData(meleeDustExplosionEmitter)
{
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 2;
	velocityVariance	= 1.0;
	ejectionOffset  	= 0.0;
	thetaMin			= 89;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= meleeDustExplosionParticle;
};
datablock DebrisData(meleeFragDebris)
{
	shapeFile 			= "./model/meleeFrag.dts";
	lifetime 			= 2.8;
	spinSpeed			= 1200.0;
	minSpinSpeed 		= -3600.0;
	maxSpinSpeed 		= 3600.0;
	elasticity 			= 0.5;
	friction 			= 0.2;
	numBounces 			= 3;
	staticOnMaxBounce 	= true;
	snapOnMaxBounce 	= false;
	fade 				= true;
	gravModifier 		= 4;
};
datablock ExplosionData(meleeGenericHitExplosion)
{
	debris					= meleeFragDebris;
	debrisNum				= 6;
	debrisNumVariance		= 2;
	debrisPhiMin			= 0;
	debrisPhiMax			= 360;
	debrisThetaMin			= 0;
	debrisThetaMax			= 180;
	debrisVelocity			= 12;
	debrisVelocityVariance	= 6;
	explosionShape			= "";
	particleEmitter			= meleeDustExplosionEmitter;
	particleDensity			= 20;
	particleRadius			= 0.2;
	lifeTimeMS				= 150;
	faceViewer				= true;
	explosionScale			= "1 1 1";
	shakeCamera				= true;
	camShakeFreq			= "10.0 11.0 10.0";
	camShakeAmp				= "2.0 3.0 2.0";
	camShakeDuration		= 0.3;
	camShakeRadius			= 10.0;
};
datablock ProjectileData(meleeGenericHitProjectile)
{
	explosion = meleeGenericHitExplosion;
};
datablock ParticleData(meleeBloodExplosionParticle)
{
	dragCoefficient		= 2;
	gravityCoefficient	= 0.1;
	inheritedVelFactor	= 0.2;
	constantAcceleration= 0.0;
	lifetimeMS			= 500;
	lifetimeVarianceMS	= 400;
	textureName			= "base/data/particles/cloud";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]			= "0.5 0.1 0.1 0.3";
	colors[1]			= "0.7 0.1 0.1 0.0";
	sizes[0]			= 0.55;
	sizes[1]			= 1.2;
	useInvAlpha 		= true;
};

datablock ParticleEmitterData(meleeBloodExplosionEmitter)
{
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 1;
	velocityVariance	= 1.0;
	ejectionOffset		= 0.0;
	thetaMin			= 89;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= meleeBloodExplosionParticle;
};
datablock ParticleData(meleeBloodBitParticle)
{
	dragCoefficient		= 0;
	gravityCoefficient	= 1;
	inheritedVelFactor	= 0.2;
	constantAcceleration= 0.0;
	lifetimeMS			= 400;
	lifetimeVarianceMS	= 100;
	textureName			= "base/data/particles/dot";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]			= "0.5 0.1 0.1 0.5";
	colors[1]			= "0.7 0.1 0.1 0.0";
	sizes[0]			= 0.12;
	sizes[1]			= 0;
	useInvAlpha 		= true;
};

datablock ParticleEmitterData(meleeBloodBitEmitter)
{
	ejectionPeriodMS	= 5;
	periodVarianceMS	= 0;
	ejectionVelocity	= 3;
	velocityVariance	= 1.0;
	ejectionOffset		= 0.0;
	thetaMin			= 0;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= meleeBloodBitParticle;
};
datablock ExplosionData(meleeBloodHitExplosion)
{
	explosionShape			= "";
	emitter[0]				= meleeBloodBitEmitter;
	particleEmitter			= meleeBloodExplosionEmitter;
	particleDensity			= 20;
	particleRadius			= 0.2;
	lifeTimeMS				= 150;
	faceViewer				= true;
	explosionScale			= "1 1 1";
	shakeCamera				= true;
	camShakeFreq			= "10.0 11.0 10.0";
	camShakeAmp				= "1.0 1.5 1.0";
	camShakeDuration		= 0.3;
	camShakeRadius			= 5.0;
};
datablock ProjectileData(meleeBloodHitProjectile)
{
	explosion = meleeBloodHitExplosion;
};



datablock ParticleData(swolMelee_stunAParticle)
{
	dragCoefficient		= 13;
	gravityCoefficient	= 0.2;
	inheritedVelFactor	= 1.0;
	constantAcceleration= 0.0;
	lifetimeMS			= 400;
	lifetimeVarianceMS	= 0;
	textureName			= "base/data/particles/star1";
	spinSpeed			= 0.0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 0.0;
	colors[0]			= "1 1 0.2 0.9";
	colors[1]			= "1 1 0.4 0.5";
	colors[2]			= "1 1 0.5 0";

	sizes[0]			= 0.5;
	sizes[1]			= 0.2;
	sizes[2]			= 0.1;

	times[0]			= 0.0;
	times[1]			= 0.5;
	times[2]			= 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(swolMelee_stunAEmitter)
{
	ejectionPeriodMS	= 26;
	periodVarianceMS	= 1;
	ejectionVelocity	= 5.25;
	velocityVariance	= 0.0;
	ejectionOffset		= 0.25;
	thetaMin			= 0;
	thetaMax			= 180;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= swolMelee_stunAParticle;
};
datablock ShapeBaseImageData(swolMelee_stunAImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;
	offset = "0 0 0.4";
	eyeOffset = "0 0 999";

	stateName[0]				= "Ready";
	stateTimeoutValue[0]		= 0.01;
	stateTransitionOnTimeout[0]	= "FireA";

	stateName[1]				= "FireA";
	stateEmitter[1]				= swolMelee_stunAEmitter;
	stateEmitterTime[1]			= 1.2;
	stateTimeoutValue[1]		= 1.2;
	stateTransitionOnTimeout[1]	= "Done";
	stateWaitForTimeout[1]		= true;

	stateName[2]				= "Done";
	stateTimeoutValue[2]		= 0.01;
	stateTransitionOnTimeout[2]	= "FireA";
};

datablock ParticleData(swolMelee_stunBParticle)
{
	dragCoefficient		= 13;
	gravityCoefficient	= 0.2;
	inheritedVelFactor	= 1.0;
	constantAcceleration= 0.0;
	lifetimeMS			= 400;
	lifetimeVarianceMS	= 0;
	textureName			= "base/data/particles/star1";
	spinSpeed			= 0.0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 0.0;
	colors[0]			= "1 0.4 0.2 0.9";
	colors[1]			= "1 0.4 0.4 0.5";
	colors[2]			= "1 0.4 0.5 0";

	sizes[0]			= 0.5;
	sizes[1]			= 0.2;
	sizes[2]			= 0.1;

	times[0]			= 0.0;
	times[1]			= 0.5;
	times[2]			= 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(swolMelee_stunBEmitter)
{
	ejectionPeriodMS	= 12;
	periodVarianceMS	= 1;
	ejectionVelocity	= 5.25;
	velocityVariance	= 0.0;
	ejectionOffset		= 0.25;
	thetaMin			= 0;
	thetaMax			= 180;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= swolMelee_stunBParticle;
};
datablock ShapeBaseImageData(swolMelee_stunBImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;
	offset = "0 0 0.4";
	eyeOffset = "0 0 999";

	stateName[0]				= "Ready";
	stateTimeoutValue[0]		= 0.01;
	stateTransitionOnTimeout[0]	= "FireA";

	stateName[1]				= "FireA";
	stateEmitter[1]				= swolMelee_stunBEmitter;
	stateEmitterTime[1]			= 1.2;
	stateTimeoutValue[1]		= 1.2;
	stateTransitionOnTimeout[1]	= "Done";
	stateWaitForTimeout[1]		= true;

	stateName[2]				= "Done";
	stateTimeoutValue[2]		= 0.01;
	stateTransitionOnTimeout[2]	= "FireA";
};

if(!isObject(headBloodSprayParticle))
{
	datablock ParticleData(headBloodSprayParticle)
	{
		dragCoefficient      = 0;
		gravityCoefficient   = 2;
		inheritedVelFactor   = 0.2;
		constantAcceleration = 0.0;
		lifetimeMS           = 600;
		lifetimeVarianceMS   = 300;
		textureName          = "base/data/particles/dot";
		spinSpeed			= 10.0;
		spinRandomMin		= -50.0;
		spinRandomMax		= 50.0;
		colors[0]			= "0.5 0.1 0.1 0.8";
		colors[1]			= "0.7 0.1 0.1 0.0";
		sizes[0]			= 0.3;
		sizes[1]			= 0;
		useInvAlpha 		= true;
	};
}
if(!isObject(headBloodSprayEmitter))
{
	datablock ParticleEmitterData(headBloodSprayEmitter)
	{
		ejectionPeriodMS	= 4;
		periodVarianceMS	= 0;
		ejectionVelocity	= 8;
		velocityVariance	= 1.0;
		ejectionOffset  	= 0.1;
		thetaMin			= 0;
		thetaMax			= 40;
		phiReferenceVel		= 0;
		phiVariance			= 360;
		overrideAdvance		= false;
		particles			= headBloodSprayParticle;
	};
}
if(!isObject(headBloodSprayImage))
{
	datablock shapeBaseImageData(headBloodSprayImage)
	{
		shapeFile = "base/data/shapes/empty.dts";
		offset = "0 0 -0.25";
		mountPoint = $headSlot;
		rotation = "-1 0 0 90";
		correctMuzzleVector = 0;
		
		stateName[0]					= "Ready";
		stateTimeoutValue[0]			= 0.01;
		stateTransitionOnTimeout[0] 	= "EmitA";
		
		stateName[1]					= "EmitA";
		stateTimeoutValue[1]			= 5;
		stateEmitterTime[1]				= 5;
		stateEmitter[1]					= headBloodSprayEmitter;
		stateTransitionOnTimeout[1] 	= "EmitB";
		
		stateName[2]					= "EmitB";
		stateTimeoutValue[2]			= 5;
		stateEmitterTime[2]				= 5;
		stateEmitter[2]					= headBloodSprayEmitter;
		stateTransitionOnTimeout[2] 	= "EmitA";
	};
}
if(!isObject(headBloodTrickleParticle))
{
	datablock ParticleData(headBloodTrickleParticle)
	{
		dragCoefficient      = 0;
		gravityCoefficient   = 1.5;
		inheritedVelFactor   = 0.2;
		constantAcceleration = 0.0;
		lifetimeMS           = 300;
		lifetimeVarianceMS   = 150;
		textureName          = "base/data/particles/dot";
		spinSpeed			= 10.0;
		spinRandomMin		= -50.0;
		spinRandomMax		= 50.0;
		colors[0]			= "0.5 0.1 0.1 0.7";
		colors[1]			= "0.7 0.1 0.1 0.0";
		sizes[0]			= 0.16;
		sizes[1]			= 0;
		useInvAlpha 		= true;
	};
}
if(!isObject(headBloodTrickleEmitter))
{
	datablock ParticleEmitterData(headBloodTrickleEmitter)
	{
		ejectionPeriodMS	= 4;
		periodVarianceMS	= 0;
		ejectionVelocity	= 2;
		velocityVariance	= 1.0;
		ejectionOffset  	= 0.1;
		thetaMin			= 0;
		thetaMax			= 180;
		phiReferenceVel		= 0;
		phiVariance			= 360;
		overrideAdvance		= false;
		particles			= headBloodTrickleParticle;
	};
}
if(!isObject(headBloodTrickleImage))
{
	datablock shapeBaseImageData(headBloodTrickleImage)
	{
		shapeFile = "base/data/shapes/empty.dts";
		offset = "0 0 -0.25";
		mountPoint = $headSlot;
		rotation = "-1 0 0 90";
		correctMuzzleVector = 0;
		
		stateName[0]					= "Ready";
		stateTimeoutValue[0]			= 0.01;
		stateTransitionOnTimeout[0] 	= "EmitA";
		
		stateName[1]					= "EmitA";
		stateTimeoutValue[1]			= 5;
		stateEmitterTime[1]				= 5;
		stateEmitter[1]					= headBloodTrickleEmitter;
		stateTransitionOnTimeout[1] 	= "EmitB";
		
		stateName[2]					= "EmitB";
		stateTimeoutValue[2]			= 5;
		stateEmitterTime[2]				= 5;
		stateEmitter[2]					= headBloodTrickleEmitter;
		stateTransitionOnTimeout[2] 	= "EmitA";
	};
}