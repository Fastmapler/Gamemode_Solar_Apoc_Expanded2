exec("./Bot_Unfleshed.cs");
exec("./Bot_Husk.cs");
exec("./Bot_Swarmer.cs");
exec("./Bot_Intoxicated.cs");
exec("./Bot_Revenant.cs");

exec("./Bot_Blob.cs");
exec("./Bot_Hunter.cs");

exec("./Bot_BossHeirophant.cs");

function SetupFaunaSpawnData()
{
	if (isObject(FaunaSpawnData))
	{
		FaunaSpawnData.deleteAll();
		FaunaSpawnData.delete();
	}

	new SimSet(FaunaSpawnData)
	{
		new ScriptObject(FaunaSpawnType) { data="UnfleshedHoleBot";		spawnWeight=1.0;	spawnCost=20;	maxSpawnGroup=5;	timeRange=(00 TAB 24);	}; //Basic Grunt
		new ScriptObject(FaunaSpawnType) { data="HuskHoleBot";			spawnWeight=0.8;	spawnCost=40;	maxSpawnGroup=4;	timeRange=(00 TAB 12);	}; //Offensive Grunt
		new ScriptObject(FaunaSpawnType) { data="SwarmerHoleBot";		spawnWeight=0.8;	spawnCost=10;	maxSpawnGroup=10;	timeRange=(12 TAB 24);	}; //Horde Grunt
		new ScriptObject(FaunaSpawnType) { data="IntoxicatedHoleBot";	spawnWeight=0.6;	spawnCost=40;	maxSpawnGroup=2; 	timeRange=(12 TAB 18);	}; //Tank Grunt
		new ScriptObject(FaunaSpawnType) { data="RevenantHoleBot";		spawnWeight=0.6;	spawnCost=20;	maxSpawnGroup=3; 	timeRange=(18 TAB 24);	}; //Ranger Grunt

		//new ScriptObject(FaunaSpawnType) { data="FireWispHoleBot";	spawnWeight=0.5;	spawnCost=45;	maxSpawnGroup=4; 	timeRange=(16 TAB 24);	}; //Basic Elemental
		//new ScriptObject(FaunaSpawnType) { data="ElementalHoleBot";	spawnWeight=0.3;	spawnCost=100;	maxSpawnGroup=1; 	timeRange=(18 TAB 24);	}; //Upgraded Elemental

		new ScriptObject(FaunaSpawnType) { data="BlobHoleBot";			spawnWeight=0.4;	spawnCost=75;	maxSpawnGroup=2; 	timeRange=(12 TAB 21);	}; //Splitting Blob Infernal
		new ScriptObject(FaunaSpawnType) { data="HunterHoleBot";		spawnWeight=0.4;	spawnCost=150;	maxSpawnGroup=1; 	timeRange=(15 TAB 24);	}; //Sleath Hunter Infernal
		//new ScriptObject(FaunaSpawnType) { data="GolemHoleBot";		spawnWeight=0.4;	spawnCost=200;	maxSpawnGroup=1; 	timeRange=(00 TAB 12);	}; //Rock Golem Infernal
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
			for (%i = 0; %i < FaunaSpawnData.getCount() && %rand >= 0; %i++)
			{
				%spawnData = FaunaSpawnData.getObject(%i);
				%spawnWeight = %spawnData.spawnWeight;
				//echo(%spawnData.data SPC %rand SPC (%rand <= %spawnWeight) SPC ($EOTW::MonsterSpawnCredits >= %spawnData.spawnCost) SPC ($EOTW::Time >= getField(%spawnData.timeRange, 0)) SPC ($EOTW::Time <= getField(%spawnData.timeRange, 1)));
				//%spawnWeight = ($EOTW::MonsterSpawnCredits > (%spawnData.spawnWeight * %spawnData.maxSpawnGroup * 3) ? %spawnData.spawnWeight / 2 : %spawnData.spawnWeight); //Prioritize higher cost fauna if we got lots of points
				if (%rand < %spawnWeight && $EOTW::MonsterSpawnCredits >= %spawnData.spawnCost && $EOTW::Time >= getField(%spawnData.timeRange, 0) && $EOTW::Time <= getField(%spawnData.timeRange, 1))
					break;

				%rand -= %spawnData.spawnWeight;
				%spawnData = "";
			}

			if (isObject(%spawnData))
			{
				%totalSpawn = getRandom(1, getMin(mFloor($EOTW::MonsterSpawnCredits / %spawnData.spawnCost), %spawnData.maxSpawnGroup));
				$EOTW::MonsterSpawnCredits -= %totalSpawn * %spawnData.spawnCost;

				for (%fail = 0; !isObject(%target) && %fail < 100; %fail++)
				{
					%player = ClientGroup.getObject(getRandom(0, ClientGroup.getCount() - 1)).player;
					if (isObject(%player) && vectorLen(%player.getPosition()) < 9000)
					{
						%target = %player;
						break;
					}
				}
					

				if (isObject(%target))
				{
					for (%i = 0; %i < %totalSpawn; %i++)
					{
						%mob = spawnNewFauna(GetRandomSpawnLocation(%target.getPosition()), %spawnData.data);
					}
				}	
				$EOTW::MonsterSpawnDelay = getRandom(15, 25) * 2;
			}
			else
				$EOTW::MonsterSpawnDelay = getRandom(5, 15) * 2;
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

//spawnNewFauna(vectorAdd(%player.getPosition(), "5 5 5"), BlobHoleBot);
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
		hDamageType = (strLen(%hBotType.hDamageType) > 0 ? %hBotType.hDamageType : $DamageType::HoleMelee);
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

	if (%hBotType.hideBody)
		%player.hideNode("ALL");

	if (%hBotType.hPlayerscale !$= "")
		%player.setScale(%hBotType.hPlayerscale);
	
	%player.hGridPosition = getWords(%trans, 0, 2);
	%player.scheduleNoQuota(10,spawnProjectile,"audio2d","spawnProjectile","0 0 0", 1);
	%player.playThread(1, armReadyBoth);
	return %player;
}

function ApplyBotSkin(%obj)
{
	%data = %obj.getDataBlock();
	%dataName = %data.getName();
	if ($EOTW::FaunaSkin[%dataName, "Exists"])
	{
		$EOTW::TempAvatar::Accent = $EOTW::FaunaSkin[%dataName, "Accent"];
		$EOTW::TempAvatar::AccentColor = $EOTW::FaunaSkin[%dataName, "AccentColor"];
		$EOTW::TempAvatar::Authentic = $EOTW::FaunaSkin[%dataName, "Authentic"];
		$EOTW::TempAvatar::Chest = $EOTW::FaunaSkin[%dataName, "Chest"];
		$EOTW::TempAvatar::ChestColor = $EOTW::FaunaSkin[%dataName, "ChestColor"];
		$EOTW::TempAvatar::DecalColor = $EOTW::FaunaSkin[%dataName, "DecalColor"];
		$EOTW::TempAvatar::DecalName = $EOTW::FaunaSkin[%dataName, "DecalName"];
		$EOTW::TempAvatar::FaceColor = $EOTW::FaunaSkin[%dataName, "FaceColor"];
		$EOTW::TempAvatar::FaceName = $EOTW::FaunaSkin[%dataName, "FaceName"];
		$EOTW::TempAvatar::Hat = $EOTW::FaunaSkin[%dataName, "Hat"];
		$EOTW::TempAvatar::HatColor = $EOTW::FaunaSkin[%dataName, "HatColor"];
		$EOTW::TempAvatar::HatList = $EOTW::FaunaSkin[%dataName, "HatList"];
		$EOTW::TempAvatar::HeadColor = $EOTW::FaunaSkin[%dataName, "HeadColor"];
		$EOTW::TempAvatar::Hip = $EOTW::FaunaSkin[%dataName, "Hip"];
		$EOTW::TempAvatar::HipColor = $EOTW::FaunaSkin[%dataName, "HipColor"];
		$EOTW::TempAvatar::LArm = $EOTW::FaunaSkin[%dataName, "LArm"];
		$EOTW::TempAvatar::LArmColor = $EOTW::FaunaSkin[%dataName, "LArmColor"];
		$EOTW::TempAvatar::LHand = $EOTW::FaunaSkin[%dataName, "LHand"];
		$EOTW::TempAvatar::LHandColor = $EOTW::FaunaSkin[%dataName, "LHandColor"];
		$EOTW::TempAvatar::LLeg = $EOTW::FaunaSkin[%dataName, "LLeg"];
		$EOTW::TempAvatar::LLegColor = $EOTW::FaunaSkin[%dataName, "LLegColor"];
		$EOTW::TempAvatar::Pack = $EOTW::FaunaSkin[%dataName, "Pack"];
		$EOTW::TempAvatar::PackColor = $EOTW::FaunaSkin[%dataName, "PackColor"];
		$EOTW::TempAvatar::RArm = $EOTW::FaunaSkin[%dataName, "RArm"];
		$EOTW::TempAvatar::RArmColor = $EOTW::FaunaSkin[%dataName, "RArmColor"];
		$EOTW::TempAvatar::RHand = $EOTW::FaunaSkin[%dataName, "RHand"];
		$EOTW::TempAvatar::RHandColor = $EOTW::FaunaSkin[%dataName, "RHandColor"];
		$EOTW::TempAvatar::RLeg = $EOTW::FaunaSkin[%dataName, "RLeg"];
		$EOTW::TempAvatar::RLegColor = $EOTW::FaunaSkin[%dataName, "RLegColor"];
		$EOTW::TempAvatar::SecondPack = $EOTW::FaunaSkin[%dataName, "SecondPack"];
		$EOTW::TempAvatar::SecondPackColor = $EOTW::FaunaSkin[%dataName, "SecondPackColor"];
		$EOTW::TempAvatar::Symmetry = $EOTW::FaunaSkin[%dataName, "Symmetry"];
		$EOTW::TempAvatar::TorsoColor = $EOTW::FaunaSkin[%dataName, "TorsoColor"];
	}
	else
	{
		exec("./Skin_" @ %obj.getDataBlock().hName @ ".cs");

		$EOTW::FaunaSkin[%dataName, "Accent"] = $EOTW::TempAvatar::Accent;
		$EOTW::FaunaSkin[%dataName, "AccentColor"] = $EOTW::TempAvatar::AccentColor;
		$EOTW::FaunaSkin[%dataName, "Authentic"] = $EOTW::TempAvatar::Authentic;
		$EOTW::FaunaSkin[%dataName, "Chest"] = $EOTW::TempAvatar::Chest;
		$EOTW::FaunaSkin[%dataName, "ChestColor"] = $EOTW::TempAvatar::ChestColor;
		$EOTW::FaunaSkin[%dataName, "DecalColor"] = $EOTW::TempAvatar::DecalColor;
		$EOTW::FaunaSkin[%dataName, "DecalName"] = $EOTW::TempAvatar::DecalName;
		$EOTW::FaunaSkin[%dataName, "FaceColor"] = $EOTW::TempAvatar::FaceColor;
		$EOTW::FaunaSkin[%dataName, "FaceName"] = $EOTW::TempAvatar::FaceName;
		$EOTW::FaunaSkin[%dataName, "Hat"] = $EOTW::TempAvatar::Hat;
		$EOTW::FaunaSkin[%dataName, "HatColor"] = $EOTW::TempAvatar::HatColor;
		$EOTW::FaunaSkin[%dataName, "HatList"] = $EOTW::TempAvatar::HatList;
		$EOTW::FaunaSkin[%dataName, "HeadColor"] = $EOTW::TempAvatar::HeadColor;
		$EOTW::FaunaSkin[%dataName, "Hip"] = $EOTW::TempAvatar::Hip;
		$EOTW::FaunaSkin[%dataName, "HipColor"] = $EOTW::TempAvatar::HipColor;
		$EOTW::FaunaSkin[%dataName, "LArm"] = $EOTW::TempAvatar::LArm;
		$EOTW::FaunaSkin[%dataName, "LArmColor"] = $EOTW::TempAvatar::LArmColor;
		$EOTW::FaunaSkin[%dataName, "LHand"] = $EOTW::TempAvatar::LHand;
		$EOTW::FaunaSkin[%dataName, "LHandColor"] = $EOTW::TempAvatar::LHandColor;
		$EOTW::FaunaSkin[%dataName, "LLeg"] = $EOTW::TempAvatar::LLeg;
		$EOTW::FaunaSkin[%dataName, "LLegColor"] = $EOTW::TempAvatar::LLegColor;
		$EOTW::FaunaSkin[%dataName, "Pack"] = $EOTW::TempAvatar::Pack;
		$EOTW::FaunaSkin[%dataName, "PackColor"] = $EOTW::TempAvatar::PackColor;
		$EOTW::FaunaSkin[%dataName, "RArm"] = $EOTW::TempAvatar::RArm;
		$EOTW::FaunaSkin[%dataName, "RArmColor"] = $EOTW::TempAvatar::RArmColor;
		$EOTW::FaunaSkin[%dataName, "RHand"] = $EOTW::TempAvatar::RHand;
		$EOTW::FaunaSkin[%dataName, "RHandColor"] = $EOTW::TempAvatar::RHandColor;
		$EOTW::FaunaSkin[%dataName, "RLeg"] = $EOTW::TempAvatar::RLeg;
		$EOTW::FaunaSkin[%dataName, "RLegColor"] = $EOTW::TempAvatar::RLegColor;
		$EOTW::FaunaSkin[%dataName, "SecondPack"] = $EOTW::TempAvatar::SecondPack;
		$EOTW::FaunaSkin[%dataName, "SecondPackColor"] = $EOTW::TempAvatar::SecondPackColor;
		$EOTW::FaunaSkin[%dataName, "Symmetry"] = $EOTW::TempAvatar::Symmetry;
		$EOTW::FaunaSkin[%dataName, "TorsoColor"] = $EOTW::TempAvatar::TorsoColor;
		$EOTW::FaunaSkin[%dataName, "Exists"] = true;
	}
	
	%i = 0;
	while (%i < $numDecal)
	{
		if (fileBase ($decal[%i]) $= fileBase ($EOTW::TempAvatar::DecalName))
		{
			$EOTW::TempAvatar::DecalColor = %i;
			break;
		}
		%i += 1;
	}
	%i = 0;
	while (%i < $numFace)
	{
		if (fileBase ($face[%i]) $= fileBase ($EOTW::TempAvatar::FaceName))
		{
			$EOTW::TempAvatar::FaceColor = %i;
			break;
		}
		%i += 1;
	}

	$EOTW::TempAvatar::DecalName = fileBase($EOTW::TempAvatar::DecalName);
	$EOTW::TempAvatar::FaceName = fileBase($EOTW::TempAvatar::FaceName);

	servercmdupdatebodyparts(%obj, $EOTW::TempAvatar::Hat, $EOTW::TempAvatar::Accent, $EOTW::TempAvatar::Pack, $EOTW::TempAvatar::SecondPack, $EOTW::TempAvatar::Chest, $EOTW::TempAvatar::Hip, $EOTW::TempAvatar::LLeg, $EOTW::TempAvatar::RLeg, $EOTW::TempAvatar::LArm, $EOTW::TempAvatar::RArm, %LHand, $EOTW::TempAvatar::RHand);
	servercmdupdatebodycolors(%obj, $EOTW::TempAvatar::HeadColor, $EOTW::TempAvatar::HatColor, $EOTW::TempAvatar::AccentColor, $EOTW::TempAvatar::PackColor, $EOTW::TempAvatar::SecondPackColor, $EOTW::TempAvatar::TorsoColor, $EOTW::TempAvatar::HipColor, $EOTW::TempAvatar::LLegColor, $EOTW::TempAvatar::RLegColor, $EOTW::TempAvatar::LArmColor, $EOTW::TempAvatar::RArmColor, $EOTW::TempAvatar::LHandColor, $EOTW::TempAvatar::RHandColor, $EOTW::TempAvatar::DecalName, $EOTW::TempAvatar::FaceName);
	
	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);

	//Special cases

	if (striPos(%obj.getDataBlock().getName(),"Blob") != -1)
	{
		%obj.hideNode("ALL");
		%obj.unHideNode("skirtHip");
		%obj.unHideNode("lShoe");
		%obj.unHideNode("rshoe");
			
		%obj.setNodeColor("lShoe", $EOTW::TempAvatar::LLegColor);
		%obj.setNodeColor("rShoe", $EOTW::TempAvatar::RLegColor);
	}
}
AddDamageType("EOTWLava", '%1 went for a swim.', '%1 went for a swim.', 1, 1);
package EOTW_Fauna
{
	function Player::RemoveBody(%player, %forceRemove)
	{
		%data = %player.getDataBlock();
		if(%player.getClassName() $= "AIPlayer" && getRandom() < getField(%data.EOTWLootTableData, 1) && !%forceRemove)
		{
			%player.setShapeName("(Gibbable)", 8564862);
			%player.isGibbable = true;
			%player.RemoveBodySchedule = %player.schedule(1000 * 60, "RemoveBody", true);
		}
		else
		{
			%rand = getRandom() * getField(%data.EOTWLootTableData, 0);
	
			for (%i = 0; %data.EOTWLootTable[%i] !$= ""; %i++)
			{
				%loot = %data.EOTWLootTable[%i];
				if (%rand >= getField(%loot, 0))
				{
					%rand -= getField(%loot, 0);
					continue;
				}

				if (getField(%loot, 1) !$= "ITEM")
				{
					EOTW_SpawnOreDrop(getRandom(getField(%loot, 1), getField(%loot, 2)) * 3, getField(%loot, 3), %player.getPosition());
				}
				else
				{
					%item = new Item()
					{
						datablock	= getField(%loot, 2);
						static		= "0";
						position	= %player.getPosition();
						rotation	= EulerToAxis(getRandom(0,359) SPC getRandom(0,359) SPC getRandom(0,359)); //Todo: Get this to work.
						craftedItem = true;
					};
					%item.setVelocity(getRandom(-7,7) SPC getRandom(-7,7) SPC 7);
					%item.schedulePop();
				}
				
				
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

		%obj.Damage (0, %obj.getPosition(), %amt, $DamageType::EOTWLava);
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
			
		%obj.Damage (0, %obj.getPosition (), %amt, $DamageType::EOTWLava);
		if (isEventPending (%obj.lavaSchedule))
		{
			cancel (%obj.lavaSchedule);
			%obj.lavaSchedule = 0;
		}
		%obj.lavaSchedule = %obj.schedule (300, lavaDamage, %amt);
	}
	function Armor::damage(%this, %obj, %sourceObj, %position, %damage, %damageType)
	{
		Parent::damage(%this, %obj, %sourceObj, %position, %damage, %damageType);

		if (%obj.getClassName() $= "AIPlayer")
		{
			if (%obj.getDatablock().isBoss)
			{
				%obj.setShapeNameDistance(128);
				%obj.setShapeNameColor("1 0 0");
			}
			else
			{
				%obj.setShapeNameDistance(16);
				%obj.setShapeNameColor("1 0.5 0");
			}
			
			%obj.setShapeName(mCeil((1 - %obj.getDamagePercent()) * 100) @ "\% HP", 8564862);
		}
	}
};
activatePackage("EOTW_Fauna");