exec("./Bot_Unfleshed.cs");
exec("./Bot_Husk.cs");
exec("./Bot_Swarmer.cs");

function SetupFaunaSpawnData()
{
	if (isObject(FaunaSpawnData))
	{
		FaunaSpawnData.deleteAll();
		FaunaSpawnData.delete();
	}

	new SimSet(FaunaSpawnData)
	{
		new ScriptObject(FaunaSpawnType) { data="UnfleshedHoleBot";		spawnWeight=1.0;	spawnCost=10;	maxSpawnGroup=5;	timeRange=(12 TAB 24);	}; //Basic Grunt
		new ScriptObject(FaunaSpawnType) { data="HuskHoleBot";			spawnWeight=0.8;	spawnCost=20;	maxSpawnGroup=5;	timeRange=(00 TAB 12);	}; //Offensive Grunt
		new ScriptObject(FaunaSpawnType) { data="SwarmerHoleBot";		spawnWeight=0.8;	spawnCost=5;	maxSpawnGroup=15;	timeRange=(12 TAB 24);	}; //Horde Grunt
		//new ScriptObject(FaunaSpawnType) { data="IntoxicatedHoleBot";	spawnWeight=0.6;	spawnCost=40;	maxSpawnGroup=2; 	timeRange=(12 TAB 24);	}; //Tank Grunt
		//new ScriptObject(FaunaSpawnType) { data="RevenantHoleBot";	spawnWeight=0.6;	spawnCost=20;	maxSpawnGroup=3; 	timeRange=(18 TAB 24);	}; //Ranger Grunt

		//new ScriptObject(FaunaSpawnType) { data="FireWispHoleBot";	spawnWeight=0.4;	spawnCost=45;	maxSpawnGroup=4; 	timeRange=(16 TAB 24);	}; //Basic Elemental
		//new ScriptObject(FaunaSpawnType) { data="ElementalHoleBot";	spawnWeight=0.4;	spawnCost=100;	maxSpawnGroup=1; 	timeRange=(18 TAB 24);	}; //Upgraded Elemental

		//new ScriptObject(FaunaSpawnType) { data="BlobHoleBot";		spawnWeight=0.2;	spawnCost=60;	maxSpawnGroup=2; 	timeRange=(12 TAB 24);	}; //Blob Infernal
		//new ScriptObject(FaunaSpawnType) { data="HunterHoleBot";		spawnWeight=0.2;	spawnCost=150;	maxSpawnGroup=1; 	timeRange=(20 TAB 24);	}; //Hunter Infernal
		//new ScriptObject(FaunaSpawnType) { data="GolemHoleBot";		spawnWeight=0.2;	spawnCost=200;	maxSpawnGroup=1; 	timeRange=(00 TAB 12);	}; //Golem Infernal

		//new ScriptObject(FaunaSpawnType) { data="DeathSquadHoleBot";	spawnWeight=1.0;	spawnCost=500;	maxSpawnGroup=3; 	timeRange=(00 TAB 24);	}; //Death Squad, we got too many points.
	};

	$EOTW::FaunaSpawnWeight = 0;
	$EOTW::FaunaSpawnList = "";
	for (%i = 0; %i < FaunaSpawnData.getCount(); %i++)
	{
		if (FaunaSpawnData.getObject(%i).spawnWeight > 0)
		{
			$EOTW::FaunaSpawnList = $EOTW::FaunaSpawnList TAB FaunaSpawnData.getObject(%i).data;
			$EOTW::FaunaSpawnWeight += FaunaSpawnData.getObject(%i).spawnWeight;
		}
	}
	$EOTW::FaunaSpawnList = trim($EOTW::FaunaSpawnList);

	if (!isObject(EOTWEnemies)) new SimGroup(EOTWEnemies);
}
SetupFaunaSpawnData();

//The spawning mechanic is a bit more indepth, so I will try my best to show what is going on.
//TLDR: The function gains "points" overtime, which will then be spent on a random target after a period of time.
function spawnFaunaLoop()
{
	cancel($EOTW::spawnFaunaLoop);

	if (ClientGroup.getCount() > 0 && EOTWEnemies.getCount() < 30)
	{
		//Give the spawner a credit, and decrement time left before spawn.
		$EOTW::MonsterSpawnCredits++;
		$EOTW::MonsterSpawnDelay--;

		if ($EOTW::MonsterSpawnDelay <= 0)
		{
			//Figure out what monster we should spawn
			%rand = getRandom() * $EOTW::FaunaSpawnWeight;
			for (%i = 0; %i < FaunaSpawnData.getCount() && %rand > 0; %i++)
			{
				%spawnData = FaunaSpawnData.getObject(%i);
				%spawnWeight = %spawnData.spawnWeight;
				//%spawnWeight = ($EOTW::MonsterSpawnCredits > (%spawnData.spawnWeight * %spawnData.maxSpawnGroup * 3) ? %spawnData.spawnWeight / 2 : %spawnData.spawnWeight); //Prioritize higher cost fauna if we got lots of points
				if (%rand < %spawnWeight && $EOTW::MonsterSpawnCredits >= %spawnData.spawnCost && $EOTW::Time >= getField(%spawnData.timeRange, 0) && $EOTW::Time <= getField(%spawnData.timeRange, 1))
					break;

				%spawnData = "";
				%rand -= %spawnData.spawnWeight;
			}

			if (isObject(%spawnData))
			{
				%totalSpawn = getRandom(1, getMin(mFloor($EOTW::MonsterSpawnCredits / %spawnData.spawnCost), %spawnData.maxSpawnGroup));
				$EOTW::MonsterSpawnCredits -= %totalSpawn * %spawnData.spawnCost;

				for (%fail = 0; !isObject(%target) && %fail < 100; %fail++)
					%target = ClientGroup.getObject(getRandom(0, ClientGroup.getCount() - 1)).player;

				if (isObject(%target))
				{
					for (%i = 0; %i < %totalSpawn; %i++)
					{
						spawnNewFauna(GetRandomSpawnLocation(%target.getPosition()), %spawnData.data);
					}
				}	
				$EOTW::MonsterSpawnDelay = getRandom(15, 25);
			}
			else
				$EOTW::MonsterSpawnDelay = getRandom(5, 15);
		}
	}

	if (isObject(EOTWEnemies))
	{
		for (%j = 0; %j < EOTWEnemies.getCount(); %j++)
		{
			%bot = EOTWEnemies.getObject(%j);
			
			//Just loop through each player instead of doing a radius raycast since that is significantly more expensive computation wise
			for (%i = 0; %i < ClientGroup.getCount(); %i++)
			{
				%client = ClientGroup.getObject(%i);
				
				if (isObject(%player = %client.player) && vectorDist(%player.getPosition(), %bot.getPosition()) < 64 && %bot.DespawnLife < 100)
				{
					%bot.DespawnLife = 100;
					break;
				}
			}
			
			%bot.DespawnLife--;
				
			if (%bot.DespawnLife <= 0)
					%bot.delete();
		}
	}
	

	$EOTW::spawnFaunaLoop = schedule(1000, 0, "spawnFaunaLoop");
}
schedule(100, 0, "spawnFaunaLoop");

//spawnNewFauna(vectorAdd(%pl.getPosition(), "5 5 5"), UnfleshedHoleBot);
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

		hPlayerscale = %hBotType.hPlayerscale;
	};

	%player.despawnLife = getRandom(150, 250);

	missionCleanup.add(%player);
		
	EOTWEnemies.add(%player);

	ApplyBotSkin(%player);

	if (%hBotType.hShoot)
		%player.mountImage(%hBotType.hWep,0);

	if (%hBotType.hPlayerscale !$= "")
		%player.setScale(%hBotType.hPlayerscale);
	
	%player.hGridPosition = getWords(%trans, 0, 2);
	%player.scheduleNoQuota(10,spawnProjectile,"audio2d","spawnProjectile","0 0 0", 1);

	return %player;
}

function ApplyBotSkin(%obj)
{
	%data = %obj.getDataBlock();
	%dataName = %data.getName();
	if ($EOTW::FaunaSkin[%dataName, "Exists"])
	{
		$pref::Avatar::Accent = $EOTW::FaunaSkin[%dataName, "Accent"];
		$pref::Avatar::AccentColor = $EOTW::FaunaSkin[%dataName, "AccentColor"];
		$pref::Avatar::Authentic = $EOTW::FaunaSkin[%dataName, "Authentic"];
		$Pref::Avatar::Chest = $EOTW::FaunaSkin[%dataName, "Chest"];
		$pref::Avatar::ChestColor = $EOTW::FaunaSkin[%dataName, "ChestColor"];
		$pref::Avatar::DecalColor = $EOTW::FaunaSkin[%dataName, "DecalColor"];
		$Pref::Avatar::DecalName = $EOTW::FaunaSkin[%dataName, "DecalName"];
		$pref::Avatar::FaceColor = $EOTW::FaunaSkin[%dataName, "FaceColor"];
		$Pref::Avatar::FaceName = $EOTW::FaunaSkin[%dataName, "FaceName"];
		$pref::Avatar::Hat = $EOTW::FaunaSkin[%dataName, "Hat"];
		$pref::Avatar::HatColor = $EOTW::FaunaSkin[%dataName, "HatColor"];
		$Pref::Avatar::HatList = $EOTW::FaunaSkin[%dataName, "HatList"];
		$pref::Avatar::HeadColor = $EOTW::FaunaSkin[%dataName, "HeadColor"];
		$Pref::Avatar::Hip = $EOTW::FaunaSkin[%dataName, "Hip"];
		$pref::Avatar::HipColor = $EOTW::FaunaSkin[%dataName, "HipColor"];
		$Pref::Avatar::LArm = $EOTW::FaunaSkin[%dataName, "LArm"];
		$pref::Avatar::LArmColor = $EOTW::FaunaSkin[%dataName, "LArmColor"];
		$Pref::Avatar::LHand = $EOTW::FaunaSkin[%dataName, "LHand"];
		$pref::Avatar::LHandColor = $EOTW::FaunaSkin[%dataName, "LHandColor"];
		$Pref::Avatar::LLeg = $EOTW::FaunaSkin[%dataName, "LLeg"];
		$pref::Avatar::LLegColor = $EOTW::FaunaSkin[%dataName, "LLegColor"];
		$pref::Avatar::Pack = $EOTW::FaunaSkin[%dataName, "Pack"];
		$pref::Avatar::PackColor = $EOTW::FaunaSkin[%dataName, "PackColor"];
		$Pref::Avatar::RArm = $EOTW::FaunaSkin[%dataName, "RArm"];
		$pref::Avatar::RArmColor = $EOTW::FaunaSkin[%dataName, "RArmColor"];
		$Pref::Avatar::RHand = $EOTW::FaunaSkin[%dataName, "RHand"];
		$pref::Avatar::RHandColor = $EOTW::FaunaSkin[%dataName, "RHandColor"];
		$Pref::Avatar::RLeg = $EOTW::FaunaSkin[%dataName, "RLeg"];
		$pref::Avatar::RLegColor = $EOTW::FaunaSkin[%dataName, "RLegColor"];
		$Pref::Avatar::SecondPack = $EOTW::FaunaSkin[%dataName, "SecondPack"];
		$pref::Avatar::SecondPackColor = $EOTW::FaunaSkin[%dataName, "SecondPackColor"];
		$pref::Avatar::Symmetry = $EOTW::FaunaSkin[%dataName, "Symmetry"];
		$pref::Avatar::TorsoColor = $EOTW::FaunaSkin[%dataName, "TorsoColor"];
	}
	else
	{
		exec("./Skin_" @ %obj.getDataBlock().hName @ ".cs");

		$EOTW::FaunaSkin[%dataName, "Accent"] = $pref::Avatar::Accent;
		$EOTW::FaunaSkin[%dataName, "AccentColor"] = $pref::Avatar::AccentColor;
		$EOTW::FaunaSkin[%dataName, "Authentic"] = $pref::Avatar::Authentic;
		$EOTW::FaunaSkin[%dataName, "Chest"] = $Pref::Avatar::Chest;
		$EOTW::FaunaSkin[%dataName, "ChestColor"] = $pref::Avatar::ChestColor;
		$EOTW::FaunaSkin[%dataName, "DecalColor"] = $pref::Avatar::DecalColor;
		$EOTW::FaunaSkin[%dataName, "DecalName"] = $Pref::Avatar::DecalName;
		$EOTW::FaunaSkin[%dataName, "FaceColor"] = $pref::Avatar::FaceColor;
		$EOTW::FaunaSkin[%dataName, "FaceName"] = $Pref::Avatar::FaceName;
		$EOTW::FaunaSkin[%dataName, "Hat"] = $pref::Avatar::Hat;
		$EOTW::FaunaSkin[%dataName, "HatColor"] = $pref::Avatar::HatColor;
		$EOTW::FaunaSkin[%dataName, "HatList"] = $Pref::Avatar::HatList;
		$EOTW::FaunaSkin[%dataName, "HeadColor"] = $pref::Avatar::HeadColor;
		$EOTW::FaunaSkin[%dataName, "Hip"] = $Pref::Avatar::Hip;
		$EOTW::FaunaSkin[%dataName, "HipColor"] = $pref::Avatar::HipColor;
		$EOTW::FaunaSkin[%dataName, "LArm"] = $Pref::Avatar::LArm;
		$EOTW::FaunaSkin[%dataName, "LArmColor"] = $pref::Avatar::LArmColor;
		$EOTW::FaunaSkin[%dataName, "LHand"] = $Pref::Avatar::LHand;
		$EOTW::FaunaSkin[%dataName, "LHandColor"] = $pref::Avatar::LHandColor;
		$EOTW::FaunaSkin[%dataName, "LLeg"] = $Pref::Avatar::LLeg;
		$EOTW::FaunaSkin[%dataName, "LLegColor"] = $pref::Avatar::LLegColor;
		$EOTW::FaunaSkin[%dataName, "Pack"] = $pref::Avatar::Pack;
		$EOTW::FaunaSkin[%dataName, "PackColor"] = $pref::Avatar::PackColor;
		$EOTW::FaunaSkin[%dataName, "RArm"] = $Pref::Avatar::RArm;
		$EOTW::FaunaSkin[%dataName, "RArmColor"] = $pref::Avatar::RArmColor;
		$EOTW::FaunaSkin[%dataName, "RHand"] = $Pref::Avatar::RHand;
		$EOTW::FaunaSkin[%dataName, "RHandColor"] = $pref::Avatar::RHandColor;
		$EOTW::FaunaSkin[%dataName, "RLeg"] = $Pref::Avatar::RLeg;
		$EOTW::FaunaSkin[%dataName, "RLegColor"] = $pref::Avatar::RLegColor;
		$EOTW::FaunaSkin[%dataName, "SecondPack"] = $Pref::Avatar::SecondPack;
		$EOTW::FaunaSkin[%dataName, "SecondPackColor"] = $pref::Avatar::SecondPackColor;
		$EOTW::FaunaSkin[%dataName, "Symmetry"] = $pref::Avatar::Symmetry;
		$EOTW::FaunaSkin[%dataName, "TorsoColor"] = $pref::Avatar::TorsoColor;
		$EOTW::FaunaSkin[%dataName, "Exists"] = true;
	}
	
	%i = 0;
	while (%i < $numDecal)
	{
		if (fileBase ($decal[%i]) $= fileBase ($Pref::Avatar::DecalName))
		{
			$pref::Avatar::DecalColor = %i;
			break;
		}
		%i += 1;
	}
	%i = 0;
	while (%i < $numFace)
	{
		if (fileBase ($face[%i]) $= fileBase ($Pref::Avatar::FaceName))
		{
			$pref::Avatar::FaceColor = %i;
			break;
		}
		%i += 1;
	}

	$Pref::Avatar::DecalName = fileBase($Pref::Avatar::DecalName);
	$Pref::Avatar::FaceName = fileBase($Pref::Avatar::FaceName);

	servercmdupdatebodyparts(%obj, $pref::Avatar::Hat, $pref::Avatar::Accent, $pref::Avatar::Pack, $Pref::Avatar::SecondPack, $Pref::Avatar::Chest, $Pref::Avatar::Hip, $Pref::Avatar::LLeg, $Pref::Avatar::RLeg, $Pref::Avatar::LArm, $Pref::Avatar::RArm, %LHand, $Pref::Avatar::RHand);
	servercmdupdatebodycolors(%obj, $pref::Avatar::HeadColor, $pref::Avatar::HatColor, $pref::Avatar::AccentColor, $pref::Avatar::PackColor, $pref::Avatar::SecondPackColor, $pref::Avatar::TorsoColor, $pref::Avatar::HipColor, $pref::Avatar::LLegColor, $pref::Avatar::RLegColor, $pref::Avatar::LArmColor, $pref::Avatar::RArmColor, $pref::Avatar::LHandColor, $pref::Avatar::RHandColor, $Pref::Avatar::DecalName, $Pref::Avatar::FaceName);
	
	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

package EOTW_Fauna
{
	function Player::RemoveBody(%player, %forceRemove)
	{
		%data = %player.getDataBlock();
		if(%player.getClassName() $= "AIPlayer" && getRandom() < getField(%data.EOTWLootTableData, 1) && !%forceRemove)
		{
			%player.isGibbable = true;
			%player.RemoveBodySchedule = %player.schedule(1000 * 60, "RemoveBody", true);
		}
		else
		{
			%rand = getRandom() * getField(%data.EOTWLootTableData, 0);
	
			for (%i = 0; %data.EOTWLootTable[%i] !$= ""; %i++)
			{
				if (%rand >= getField(%data.EOTWLootTable[%i], 0))
				{
					%rand -= getField(%data.EOTWLootTable[%i], 0);
					continue;
				}

				EOTW_SpawnOreDrop(getRandom(getField(%data.EOTWLootTable[%i], 1), getField(%data.EOTWLootTable[%i], 2)) * 3, getField(%data.EOTWLootTable[%i], 3), %player.getPosition());
				
				break;
			}

			return parent::RemoveBody(%player);
		}
	}
	function AIPlayer::lavaDamage(%obj, %amt)
	{
		Player::lavaDamage(%obj, %amt);
	}
	function Player::lavaDamage(%obj, %amt)
	{
		if (%obj.getDataBlock().lavaImmune)
			return;

		%obj.Damage (0, %obj.getPosition(), %amt, $DamageType::Lava);
		if (isEventPending(%obj.lavaSchedule))
		{
			cancel(%obj.lavaSchedule);
			%obj.lavaSchedule = 0;
		}
		%obj.lavaSchedule = %obj.schedule (300, lavaDamage, %amt);
	}
	function Vehicle::lavaDamage (%obj, %amt)
	{
		if (%obj.getDataBlock().lavaImmune)
			return;
			
		%obj.Damage (0, %obj.getPosition (), %amt, $DamageType::Lava);
		if (isEventPending (%obj.lavaSchedule))
		{
			cancel (%obj.lavaSchedule);
			%obj.lavaSchedule = 0;
		}
		%obj.lavaSchedule = %obj.schedule (300, lavaDamage, %amt);
	}

};
activatePackage("EOTW_Fauna");