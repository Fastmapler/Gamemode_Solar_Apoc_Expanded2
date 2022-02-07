exec("./Bot_Unfleshed.cs");

function spawnNewFauna(%trans,%hBotType)
{
	if(!isObject(FakeBotSpawnBrick))
	{
		new FxDtsBrick(FakeBotSpawnBrick)
		{
			datablock = brick1x1Data;
			isPlanted = false;
			itemPosition = 1;
			position = "0 0 -2000";
		};
	}
	%spawnBrick = FakeBotSpawnBrick;
	
	if(%hBotType $= "")
		%hBotType = ZombieHoleBot;
	
	%player = new AIPlayer()
	{
		dataBlock = %hBotType;
		path = "";
		spawnBrick = %spawnBrick;
		mini = $defaultMinigame;
		
		position = getWords(%trans, 0, 2);
		hGridPosition = getWords(%trans, 0, 2);
		rotation = getWords(%trans, 3, 6);
		
		//Apply attributes to Bot
		client = 0;
		isHoleBot = 1;
			
		//Apply attributes to Bot
		Name = %hBotType.hName;
		hType = %hBotType.hType;
		hDamageType = (strLen(%hBotType.hDamageType) !$= "" ? $DamageType["::" @ %damageType] : $DamageType::HoleMelee);
		hSearchRadius = %hBotType.hSearchRadius;
		hSearch = %hBotType.hSearch;
		hSight = %hBotType.hSight;
		hWander = %hBotType.hWander;
		hGridWander = %hBotType.hGridWander;
		hReturnToSpawn = %hBotType.hReturnToSpawn;
		hSpawnDist = %hBotType.hSpawnDist;
		hHerding = %hBotType.hHerding;
		hMelee = %hBotType.hMelee;
		hAttackDamage = %hBotType.hAttackDamage;
		hSpazJump = %hBotType.hSpazJump;
		hSearchFOV = %hBotType.hSearchFOV;
		hFOVRadius = %hBotType.hFOVRadius;
		hTooCloseRange = %hBotType.hTooCloseRange;
		hAvoidCloseRange = %hBotType.hAvoidCloseRange;
		hShoot = %hBotType.hShoot;
		hMaxShootRange = %hBotType.hMaxShootRange;
		hStrafe = %hBotType.hStrafe;
		hAlertOtherBots = %hBotType.hAlertOtherBots;
		hIdleAnimation = %hBotType.hIdleAnimation;
		hSpasticLook = %hBotType.hSpasticLook;
		hAvoidObstacles = %hBotType.hAvoidObstacles;
		hIdleLookAtOthers = %hBotType.hIdleLookAtOthers;
		hIdleSpam = %hBotType.hIdleSpam;
		hAFKOmeter = %hBotType.hAFKOmeter + getRandom( 0, 2 );
		hHearing = %hBotType.hHearing;
		hIdle = %hBotType.hIdle;
		hSmoothWander = %hBotType.hSmoothWander;
		hEmote = %hBotType.hEmote;
		hSuperStacker = %hBotType.hSuperStacker;
		hNeutralAttackChance = %hBotType.hNeutralAttackChance;
		hFOVRange = %hBotType.hFOVRange;
		hMoveSlowdown = %hBotType.hMoveSlowdown;
		hMaxMoveSpeed = 1.0;
		hActivateDirection = %hBotType.hActivateDirection;
	};
	
    if (!isObject(EOTWEnemies)) new SimGroup(EOTWEnemies);

	missionCleanup.add(%player);
		
	EOTWEnemies.add(%player);

	if (%hBotType.hShoot)
		%player.mountImage(%hBotType.hWep,0);
	
	%player.hGridPosition = getWords(%trans, 0, 2);
	%player.scheduleNoQuota(10,spawnProjectile,"audio2d","spawnProjectile","0 0 0", 1);

	return %player;
}