exec("./Bot_Unfleshed.cs");

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
	};
	
    if (!isObject(EOTWEnemies)) new SimGroup(EOTWEnemies);

	missionCleanup.add(%player);
		
	EOTWEnemies.add(%player);

	ApplyBotSkin(%player);

	if (%hBotType.hShoot)
		%player.mountImage(%hBotType.hWep,0);
	
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
			%rand = getRandom(0, getField(%data.EOTWLootTableData, 0));
	
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
};
activatePackage("EOTW_Fauna");