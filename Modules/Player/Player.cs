function PlayerLoop()
{
	cancel($EOTW:PlayerLoop);
	
	for (%i = 0; %i < ClientGroup.GetCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		%client.PrintEOTWInfo();

		if (isObject(%player = %client.player))
		{
			%energy = %player.GetMatterCount("Energy");
			if (%energy > 0)
			{
				%change = getMin(%player.GetMaxBatteryEnergy() - %player.GetBatteryEnergy(), 10);
				%change = getMin(%change, %energy);
				if (%change > 0)
				{
					%player.ChangeMatterCount("Energy", %change * -1);
					%player.ChangeBatteryEnergy(%change);
					%player.lastBatteryRequest = getSimTime();
				}
				else
				{
					%player.ChangeMatterCount("Energy", -2);
				}
			}

			if (%player.getDamageLevel() > 0)
				%player.setDamageLevel(%player.getDamageLevel() - 0.01);
		}
	}
	
	$EOTW:PlayerLoop = schedule(100,ClientGroup,"PlayerLoop");
}
schedule(100, 0, "PlayerLoop");

function GetTimeStamp()
{
	%hours = mFloor($EOTW::Time);
    %minutes = 60*($EOTW::Time - %hours);
    %minutes = mFloor(%minutes * 0.1) * 10;

    %hours = (%hours + 6) % 24; //corrects for dawn being considered hour 0
	
	if (%minutes < 10)
		%minutes = "0" @ %minutes;
		
    return %hours @ ":" @ %minutes;
}

function GameConnection::PrintEOTWInfo(%client)
{
	if (!isObject(%player = %client.player))
	{
		%client.bottomPrint("<just:center>\c7You died!");
		return;
	}

	%blacklist = "CardsOutImage ChipImage DeckOutImage";
	if (isObject(%image = %player.getMountedImage(0)) && hasWord(%blacklist, %image.getName()))
		return;
			
	%health = mCeil(%player.getDatablock().maxDamage - %player.getDamageLevel());
	
	if (%player.getDamagePercent() > 0.75)
		%health = "<color:ff0000>" @ %health;
	else if (%player.getDamagePercent() > 0.25)
		%health = "<color:ffff00>" @ %health;
	else
		%health = "<color:00ff00>" @ %health;
	
	%health = %health @ "<color:ffffff>/" @ %player.getDatablock().maxDamage;
	
	if (isObject(%image = %player.getMountedImage(0)))
	{
		%db = %client.inventory[%client.currInvSlot];

		if (!isObject(%db) && isObject(%player.tempBrick))
			%db = %player.tempBrick.getDatablock();
			
		if (%image.getName() $= "BrickImage" && isObject(%db))
		{
			if (%client.buildMaterial $= "")
				%client.buildMaterial = MatterData.getObject(0).name;

			if ($EOTW::BrickDescription[%db.getName()] !$= "")
			{
				%centerText = "<br><br><br><br>\c6" @ $EOTW::BrickDescription[%db.getName()];
			}
			
			if ($EOTW::CustomBrickCost[%db.getName()] !$= "")
			{
				%cost = $EOTW::CustomBrickCost[%db.getName()];
				
				%brickText = "<br>";
				
				if (getField(%cost, 0) < 1.0)
					%brickText = %brickText @ "(" @ ((1.0 - getField(%cost, 0)) * 100) @ "% Refund Fee!)";
					
				for (%i = 2; %i < getFieldCount(%cost); %i += 2)
				{
					%volume = getField(%cost, %i);
					%matter = getMatterType(getField(%cost, %i + 1));
					%name = %matter.name;
					%color = "<color:" @ %matter.color @ ">";
					%inv = $EOTW::Material[%client.bl_id, %name] + 0;
					if (%inv < %volume) %inv = "\c0" @ %inv;
				
					%brickText = %brickText SPC %inv @ "\c6/" @ %volume SPC %color @ %name @ "\c6,";
				}
				
				%brickText = getSubStr(%brickText, 0, strLen(%brickText) - 1);
			}
			else
			{
				%matter = getMatterType(%client.buildMaterial);
				%name = %matter.name;
				%color = "<color:" @ %matter.color @ ">";
				%inv = $EOTW::Material[%client.bl_id, %name] + 0;
			
				%volume = %db.brickSizeX * %db.brickSizeY * %db.brickSizeZ;
				if (%inv < %volume) %inv = "\c0" @ %inv;
				
				%brickText = "<br>" @ %color @ %name @ "\c6: " @ %inv @ "\c6/" @ %volume SPC "[" @ %db.getName() @ "]";
			}
		}
		else if (%image.printPlayerBattery)
			%brickText = "<br>" @ %player.GetBatteryText();
	}
	else if (getSimTime() - %player.lastBatteryRequest < 1000)
		%brickText = "<br>" @ %player.GetBatteryText();
	
	if (%centerText !$= "")
		%client.centerPrint(%centerText, 1);

	%dayText = $EOTW::Time >= 12 ? "Night\c6:" SPC $EOTW::Day : "Day\c6:" SPC $EOTW::Day;
	%client.bottomPrint("<just:center>\c3" @ %dayText @ " | \c3Time\c6:" SPC GetTimeStamp() SPC "| \c3Health\c6:" SPC %health @ %brickText,3);
}

function GetRandomSpawnLocation(%initPos, %failCount)
{
	
	if (%initPos !$= "")
	{
		%xOffset = (getRandom() < 0.5 ? getRandom(32, 64) : getRandom(-64, -32));
		%yOffset = (getRandom() < 0.5 ? getRandom(32, 64) : getRandom(-64, -32));
		%eye = (getField(%initPos,0) + %xOffset) SPC (getField(%initPos,1) + %yOffset) SPC 495; //getRandom(0, 1664)
	}
	else
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2)) / 1) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3)) / 1) SPC 495;
	%dir = "0 0 -1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
	
	if (getWord(%ray,3) < $EOTW::LavaHeight && %failCount < 500)
		return GetRandomSpawnLocation(%initPos, %failCount + 1); //Try again lol
		
	%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
	if(isObject(%hit = firstWord(%ray)))
	{
		%pos = vectorAdd(%pos,"0 0 5");
		return %pos;
	}
}

function Player::LookingAtBrick(%obj, %brick)
{
	%eye = %obj.getEyePoint();
	%dir = %obj.getEyeVector();
	%for = %obj.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getID() $= %brick.getID())
		return true;

	return false;
}

function Player::GetMatterCount(%obj, %matter)
{
	if (!isObject(%client = %obj.client))
		return;

	return $EOTW::Material[%client.bl_id, %matter] + 0;
}

function Player::ChangeMatterCount(%obj, %matter, %count)
{
	if (!isObject(%client = %obj.client))
		return;

	$EOTW::Material[%client.bl_id, %matter] += %count;

	if ($EOTW::Material[%client.bl_id, %matter] < 0)
		$EOTW::Material[%client.bl_id, %matter] = 0;

	return $EOTW::Material[%client.bl_id, %matter];
}

function clearIllegalEvents()
{
	unregisterOutputEvent("fxDtsBrick", "setItem");				//Bypasses blacklist
	unregisterOutputEvent("fxDtsBrick", "spawnExplosion");		//Assholes try to lag the server up.
	unregisterOutputEvent("fxDtsBrick", "spawnItem");			//Allows players to bypass the crafting process + Bypasses Blacklist
	unregisterOutputEvent("fxDtsBrick", "spawnProjectile");		//People shoot projectiles at others.
	
	unregisterOutputEvent("Player", "addHealth");				//This is a survival-based gamemode. No healing.
	unregisterOutputEvent("Player", "changeDatablock");			//Incredibly abusable; people give themselves jets.
	unregisterOutputEvent("Player", "clearTools");				//People *will* use this to clear others' tools.
	unregisterOutputEvent("Player", "instantRespawn");			//Death traps and the like. People are assholes.
	unregisterOutputEvent("Player", "kill");					//Death traps and the like. People are assholes.
	unregisterOutputEvent("Player", "setHealth");				//This is a survival-based gamemode. No healing.
	unregisterOutputEvent("Player", "spawnExplosion");			//Assholes try to lag the server up + Landmines/Turrets
	unregisterOutputEvent("Player", "spawnProjectile");			//People shoot projectiles at others + Turrets
	unregisterOutputEvent("Player", "setPlayerScale");			//Trap players by increasing their size.
	unregisterOutputEvent("Player", "setMaxHealth");			//Allows to give players inf. health
	unregisterOutputEvent("Player", "addMaxHealth");			//Allows to give players inf. health
	unregisterOutputEvent("Player", "setInvulnerbilityTime");	//No godmode allowed
	unregisterOutputEvent("Player", "setFInvulnerbilityTime");	//No godmode allowed
	unregisterOutputEvent("Player", "setInvulnerbility");		//No godmode allowed
	unregisterOutputEvent("Player", "saveHealth");				//We have our own health system
	unregisterOutputEvent("Player", "loadHealth");				//We have our own health system
	unregisterOutputEvent("Player", "addVelocity");				//So players don't nuke other players into orbit
	unregisterOutputEvent("Player", "setVelocity");				//So players don't nuke other players into orbit
	
	unregisterOutputEvent("MiniGame", "CenterPrintAll");		//Spammable.
	unregisterOutputEvent("MiniGame", "BottomPrintAll");		//Spammable.
	unregisterOutputEvent("MiniGame", "ChatMsgAll");			//Spammable.
	unregisterOutputEvent("MiniGame", "Reset");					//NOOO
	unregisterOutputEvent("MiniGame", "RespawnAll");			//NOOO
	
	unregisterOutputEvent("bot", "setMaxHealth");
	unregisterOutputEvent("bot", "addMaxHealth");
	unregisterOutputEvent("bot", "setInvulnerbilityTime");
	unregisterOutputEvent("bot", "setFInvulnerbilityTime");
	unregisterOutputEvent("bot", "setInvulnerbility");
	
	unregisterOutputEvent("vehicle", "setMaxHealth");
	unregisterOutputEvent("vehicle", "addMaxHealth");
	unregisterOutputEvent("vehicle", "setInvulnerbilityTime");
	unregisterOutputEvent("vehicle", "setFInvulnerbilityTime");
	unregisterOutputEvent("vehicle", "setInvulnerbility");
}
schedule(10, 0, "clearIllegalEvents");

package EOTW_Player
{
	function Armor::onTrigger(%data, %obj, %trig, %tog)
	{
		if(isObject(%client = %obj.client))
		{
			if(%trig == 0 && %tog && !isObject(%obj.getMountedImage(0)))
			{
				%eye = %obj.getEyePoint();
				%dir = %obj.getEyeVector();
				%for = %obj.getForwardVector();
				%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
				%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
				%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
				if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
				{
					%data = %hit.getDatablock();
					if (%data.inspectFunc !$= "")
					{
						cancel(%obj.PoweredBlockInspectLoop);
						%obj.PoweredBlockInspectLoop = %obj.schedule(2000 / $EOTW::PowerTickRate, %data.inspectFunc, %hit);
					}
				}
			}
		}
		Parent::onTrigger(%data, %obj, %trig, %tog);
	}
	function serverCmdClearCheckpoint(%client)
	{
		if(isObject(%client.checkPointBrick))
		{
			%client.checkPointBrick = "";
			%client.checkPointBrickPos = "";

			messageClient(%client, '', '\c3Checkpoint reset');
		}
	}
};
activatePackage("EOTW_Player");

exec("./Player_SolarApoc.cs");
exec("./Support_MultipleSlots.cs");
exec("./Support_PlayerBattery.cs");
exec("./Item_Armors.cs");
//exec("./BrickControlsMenu.cs");