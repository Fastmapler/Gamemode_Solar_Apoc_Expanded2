exec("./ItemCrafting.cs");
exec("./Item_MaterialPickup.cs");
exec("./Brick_OilGeyser.cs");
exec("./Support_Plants.cs");
exec("./MatterData.cs");

$EOTW::MatterDensity = 5000 / (2048 * 2048);

datablock fxDTSBrickData(brickEOTWGatherableBasicData)
{
	brickFile = "./Bricks/GatherableBasic.blb";
	category = "Special";
	subCategory = " Custom";
	uiName = "Gatherable Basic";
};
datablock fxDTSBrickData(brickEOTWGatherableMetalData)
{
	brickFile = "./Bricks/GatherableMetal.blb";
	category = "Special";
	subCategory = " Custom";
	uiName = "Gatherable Metal";
};
datablock fxDTSBrickData(brickEOTWGatherableCrystalData)
{
	brickFile = "./Bricks/GatherableCrystal.blb";
	category = "Special";
	subCategory = " Custom";
	uiName = "Gatherable Crystal";
};


function GetMatterType(%type)
{
	for (%i = 0; %i < MatterData.getCount(); %i++)
		if (MatterData.getObject(%i).name $= %type)
			return MatterData.getObject(%i);
			
	return;
}

function GatherableSpawnLoop(%despawnValue)
{
	cancel($EOTW::GatherableLoop);
	
	if(!isObject(BrickGroup_1337))
		MainBrickgroup.add(new SimGroup(BrickGroup_1337) { bl_id = 1337; name = "God"; });
		
	if (BrickGroup_1337.getCount() < (getMapArea() * $EOTW::MatterDensity))
	{
		if (getRandom() < 0.01)
			SpawnOilGeyser();
		
		SpawnGatherableVein();
	}
	
	for (%j = 0; %j < 30 && %despawnValue < BrickGroup_1337.getCount(); %j++)
	{
		%brick = BrickGroup_1337.getObject(%despawnValue);
		if (isObject(%brick))
		{
			//Just loop through each player instead of doing a radius raycast since that is significantly more expensive computation wise
			for (%i = 0; %i < ClientGroup.getCount(); %i++)
			{
				%client = ClientGroup.getObject(%i);
				
				if (isObject(%player = %client.player) && vectorDist(%player.getPosition(), %brick.getPosition()) < 64 && %brick.DespawnLife < 100)
				{
					%brick.DespawnLife = 100;
					break;
				}
			}
			
			%brick.DespawnLife--;
				
			if (%brick.DespawnLife <= 0)
			{
				if (%brick.OilCapacity > 0)
					%brick.OilCapacity -= 8;
				else
					%brick.delete();
			}
				
		}
		
		%despawnValue++;
	}
	
	if (%despawnValue >= BrickGroup_1337.getCount())
		%despawnValue = 0;
	
	$EOTW::GatherableLoop = schedule(50, 0, "GatherableSpawnLoop", %despawnValue);
}
schedule(10, 0, "GatherableSpawnLoop");

function spawnGatherableRandom(%eye)
{
	if (%eye $= "")
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2))) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3))) SPC 1000;
		
	%dir = "0 0 -1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
	%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
	if(isObject(%hit = firstWord(%ray)))
	{
		if (%hit.getClassName() !$= "FxPlane" && strPos(%hit.getDatablock().uiName,"Ramp") > -1)
			%pos = vectorAdd(%pos,"0 0 0.4");
		SpawnGatherable(%pos, GetRandomSpawnMaterial());
	}
	else if (getWord(%eye, 2) == 1000)
		spawnGatherableRandom(getWord(%eye, 0) SPC getWord(%eye, 1) SPC 500);
}

function SpawnGatherableVein()
{
	%origin = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2))) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3))) SPC 495;
	%matter = GetRandomSpawnMaterial();
	
	//Chance for super concentrated spawn
	if (getRandom() < 0.02)
	{
		%veinSize = %matter.spawnVeinSize * 2;
		%veinRange = 4;
	}
	else
	{
		%veinSize = getRandom(1, %matter.spawnVeinSize);
		%veinRange = 8;
	}
	
	%despawnLife = getRandom(100, 300);
	for (%i = 0; %i < %veinSize; %i++)
	{
		%offset = (getRandom(-1 * %veinRange, %veinRange) / 2) SPC (getRandom(-1 * %veinRange, %veinRange) / 2) SPC "0";
		%eye = vectorAdd(%origin, %offset);
		%dir = "0 0 -1";
		%for = "0 1 0";
		%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
		%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
		%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
		%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
		if(isObject(%hit = firstWord(%ray)) && (getWord(%pos, 2) > $EOTW::LavaHeight + 2))
		{
			if (%hit.getClassName() !$= "FxPlane" && strPos(%hit.getDatablock().uiName,"Ramp") > -1)
				%pos = vectorAdd(%pos,"0 0 0.4");
				
			if (!%hit.isCollectable)
				SpawnGatherable(%pos, %matter, %despawnLife);
		}
	}
}

function SpawnGatherable(%pos, %matter, %despawnLife)
{
	if (%pos $= "")
		return;
		
	if (!isObject(%matter))
		%matter = GetRandomSpawnMaterial();
	
	if (%matter.gatherableDB !$= "")
		%brickDB = %matter.gatherableDB.getID();
	else
		%brickDB = brick1x1fData;
	
	%data = CreateBrick(EnvMaster, %brickDB, %pos, getColorFromHex(%matter.color), getRandom(0, 3)); //brickEOTWGatherableMetalData //brick1x1FData
	%brick = getField(%data, 0);
	if(getField(%data, 1)) { %brick.delete(); return; }
	
	if(!isObject(BrickGroup_1337))
		MainBrickgroup.add(new SimGroup(BrickGroup_1337) { bl_id = 1337; name = "God"; });
		
	if (%despawnLife > 0)
		%brick.DespawnLife = %despawnLife;
	else
		%brick.DespawnLife = getRandom(100, 300);
	
	BrickGroup_1337.add(%brick);
	
	%brick.material = %matter.name;
	%brick.isCollectable = true;
	
	$EOTW::RandomTest[%matter.name]++;
}

function GetRandomSpawnMaterial()
{
	%rand = getRandom(0, $EOTW::MatSpawnWeight);
	
	for (%i = 0; %i < getFieldCount($EOTW::MatSpawnList); %i++)
	{
		%matter = GetMatterType(getField($EOTW::MatSpawnList, %i));
		
		if (%rand < %matter.spawnWeight)
			break;
		
		%rand -= %matter.spawnWeight;
	}
	
	return %matter;
}

function ServerCmdInv(%client, %cata)
{
	for (%i = 0; %i < MatterData.getCount(); %i++)
	{
		%matter = MatterData.getObject(%i);
		%client.chatMessage("<color:" @ getSubStr(%matter.color, 0, 6) @ ">" @ %matter.name @ "<color:ffffff>: " @ ($EOTW::Material[%client.bl_id, %matter.name] + 0));
	}
}

function Player::CollectLoop(%player, %brick)
{
	cancel(%player.collectLoop);
	if(!isObject(%client = %player.client) || %player.getState() $= "DEAD") return;
	if(!isObject(%brick) || %brick.isDead()) return;
	%eye = %player.getEyePoint();
	%dir = %player.getEyeVector();
	%for = %player.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %this);
	if(isObject(%hit = firstWord(%ray)) && %hit == %brick && %brick.material !$= "")
	{
		if (!isObject(%brick.matterType))
			%brick.matterType = getMatterType(%brick.material);
			
		cancel(%brick.cancelCollecting);
		
		%reqFuel = %brick.matterType.requiredCollectFuel;
		if (%reqFuel !$= "" && %player.GetMatterCount(getField(%reqFuel, 0)) < getField(%reqFuel, 1))
		{
			%client.centerPrint("<br><color:FFFFFF>You need atleast " @ getField(%reqFuel, 1) SPC getField(%reqFuel, 0) @ " to gather this " @ %brick.matterType.name @ "!", 3);
		}
		else if (%brick.gatherProcess >= %brick.matterType.collectTime)
		{
			if (%reqFuel !$= "")
				%player.ChangeMatterCount(getField(%reqFuel, 0), getField(%reqFuel, 1) * -1);

			$EOTW::Material[%client.bl_id, %brick.matterType.name] += %brick.matterType.spawnValue;
			%client.centerPrint("<br><color:FFFFFF>Collected a gatherable " @ %brick.material @ " brick.<br>100% complete.<br>You now have " @ $EOTW::Material[%client.bl_id, %brick.matterType.name] SPC %brick.matterType.name @ ".", 3);
			%brick.killBrick();
		}
		else
		{
			%brick.cancelCollecting = %brick.schedule(10000, "cancelCollecting");
			%player.collectLoop = %player.schedule(16, "collectLoop", %brick);
			%client.centerPrint("<br><color:FFFFFF>Collecting a gatherable " @ %brick.material @ " brick.<br>" @ mFloor((%brick.gatherProcess / %brick.matterType.collectTime) * 100) @ "% complete.", 3);
			
			%brick.gatherProcess += getSimTime() - %brick.lastGatherTick;
			%brick.lastGatherTick = getSimTime();
		}
		
	}
}

function fxDtsBrick::cancelCollecting(%brick) { %brick.beingCollected = 0; }

package EOTW_Matter
{
	function fxDTSBrick::KillBrick(%brick)
	{
		if (%brick.getGroup().bl_id == 888888)
			return;
		
		parent::KillBrick(%brick);
	}
	function servercmdPlantBrick(%cl)
	{
		if(!%cl.builderMode && !$EOTW::Freebuild)
		{
			if(isObject(%pl = %cl.player) && isObject(%temp = %pl.tempbrick))
			{
				%data = %temp.getDatablock();
					
				if ($EOTW::CustomBrickCost[%data.getName()] !$= "")
				{
					%cost = $EOTW::CustomBrickCost[%data.getName()];
					
					for (%i = 2; %i < getFieldCount(%cost); %i += 2)
					{
						%volume = getField(%cost, %i);
						%matter = getMatterType(getField(%cost, %i + 1));
						%name = %matter.name;
						%color = "<color:" @ %matter.color @ ">";
						%inv = $EOTW::Material[%cl.bl_id, %name] + 0;
				
						if(%inv < %volume)
						{
							%cl.chatMessage("\c0Whoops!<br>\c6You don't have enough " @ %name @ " to place that brick! \c6You need" SPC (%volume - $EOTW::Material[%cl.bl_id, %name]) SPC "more.");
							return;
						}
					}
					%brick = Parent::servercmdPlantBrick(%cl); if(!isObject(%brick)) return %brick;
					
					for (%i = 2; %i < getFieldCount(%cost); %i += 2)
					{
						%volume = getField(%cost, %i);
						%matter = getMatterType(getField(%cost, %i + 1));
						%name = %matter.name;
						$EOTW::Material[%cl.bl_id, %name] -= %volume;
					}
					%brick.setColor(getColorFromHex(getField(%cost, 1)));
					%brick.material = "Custom";
				}
				else
				{
					%volume = %data.brickSizeX * %data.brickSizeY * %data.brickSizeZ;
					%mat = %cl.buildMaterial;

					if($EOTW::Material[%cl.bl_id, %mat] < %volume)
						%cl.chatMessage("\c0Whoops!<br>\c6You don't have enough " @ %cl.buildMaterial @ " to place that brick! \c6You need" SPC (%volume - $EOTW::Material[%cl.bl_id, %mat]) SPC "more.");
					else
					{
						%brick = Parent::servercmdPlantBrick(%cl); if(!isObject(%brick)) return %brick;
						$EOTW::Material[%cl.bl_id, %mat] -= %volume;
						%brick.material = %mat;
						%brick.setColor(getColorFromHex(getMatterType(%mat).color));
					}
				}
			}
		}
		else return Parent::servercmdPlantBrick(%cl);
	}
	function fxDtsBrick::onRemove(%brick)
	{
		%data = %brick.getDatablock();

		if(!%brick.dontRefund)
		{
			if ($EOTW::CustomBrickCost[%data.getName()] !$= "" && %brick.material $= "Custom")
			{
				%cost = $EOTW::CustomBrickCost[%data.getName()];
				
				for (%i = 2; %i < getFieldCount(%cost); %i += 2)
				{
					%volume = getField(%cost, %i);
					%matter = getMatterType(getField(%cost, %i + 1));
					%name = %matter.name;
					
					if (getField(%cost, 0) < 1.0)
						%volume = mCeil(%volume * getField(%cost, 0));
						
					$EOTW::Material[%brick.getGroup().bl_id, %name] += %volume;
				}
			}
			else if (%brick.material !$= "Custom" && %brick.material !$= "")
			{
				//Support for returning proper refund for open doors.
				if (%data.closedCW !$= "")
				{
					%newData = %data.closedCW;
					%volume = %newData.brickSizeX * %newData.brickSizeY * %newData.brickSizeZ;
				}
				else if (%data.closedCCW !$= "")
				{
					%newData = %data.closedCCW;
					%volume = %newData.brickSizeX * %newData.brickSizeY * %newData.brickSizeZ;
				}
				else
				{
					%volume = %data.brickSizeX * %data.brickSizeY * %data.brickSizeZ;
				}

				$EOTW::Material[%brick.getGroup().bl_id, %brick.material] += %volume;
			}
		}

		for (%i = 0; %i < %data.matterSlots["Input"]; %i++)
			if (%brick.matter["Input", %i] !$= "")
				EOTW_SpawnOreDrop(getField(%brick.matter["Input", %i], 1), getField(%brick.matter["Input", %i], 0), %brick.getPosition());
				
		for (%i = 0; %i < %data.matterSlots["Buffer"]; %i++)
			if (%brick.matter["Buffer", %i] !$= "")
				EOTW_SpawnOreDrop(getField(%brick.matter["Buffer", %i], 1), getField(%brick.matter["Buffer", %i], 0), %brick.getPosition());

		for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
			if (%brick.matter["Output", %i] !$= "")
				EOTW_SpawnOreDrop(getField(%brick.matter["Output", %i], 1), getField(%brick.matter["Output", %i], 0), %brick.getPosition());

		if (%brick.GetPower() > 0)
			EOTW_SpawnOreDrop(%brick.GetPower(), "Energy", %brick.getPosition());
		
		Parent::onRemove(%brick);
	}
	function fxDtsBrick::setColor(%brick, %color)
	{
		%matter = getMatterType(%brick.material);
		if(getWord(getColorIDTable(%color), 3) > 0.95 || !isObject(%matter) || decimalFromHex(getSubStr(%matter.color, 6, 2)) < 250)
			Parent::setColor(%brick, %color);
	}
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
					if (%hit.isCollectable)
					{
						if(%hit.beingCollected > 0 && %hit.beingCollected != %client.bl_id)
							%client.centerPrint("<color:FFFFFF>Someone is already collecting that material brick!", 3);
						else
						{
							%hit.lastGatherTick = getSimTime();
							%hit.beingCollected = %client.bl_id;
							%hit.cancelCollecting = %hit.schedule(10000, "cancelCollecting");
							%obj.collectLoop(%hit);
						}
					}
				}
			}
			if(%trig == 4 && %tog && isObject(%image = %obj.getMountedImage(0)) && %image.getName() $= "BrickImage")
			{
				%db = %client.inventory[%client.currInvSlot];

				if (!isObject(%db) && isObject(%obj.tempBrick))
					%db = %obj.tempBrick.getDatablock();

				if (isObject(%db) && $EOTW::CustomBrickCost[%db.getName()] $= "")
				{
					%pos = getFieldFromValue($EOTW::PlacableList, %client.buildMaterial);
					
					if (!%obj.isCrouched())
					{
						%pos++;
						if (%pos < getFieldCount($EOTW::PlacableList))
							%client.buildMaterial = getField($EOTW::PlacableList, %pos);
						else
							%client.buildMaterial = getField($EOTW::PlacableList, 0);
					}
					else
					{
						%pos--;
						if (%pos >= 0)
							%client.buildMaterial = getField($EOTW::PlacableList, %pos);
						else
							%client.buildMaterial = getField($EOTW::PlacableList, getFieldCount($EOTW::PlacableList) - 1);
					}
				}
			}
		}
		Parent::onTrigger(%data, %obj, %trig, %tog);
	}
};
activatePackage("EOTW_Matter");