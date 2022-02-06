exec("./ItemCrafting.cs");

$EOTW::MatterDensity = 5000 / (2048 * 2048);

function SetupMatterData()
{
	if (isObject(MatterData))
	{
		MatterData.deleteAll();
		MatterData.delete();
	}

	new SimSet(MatterData)
	{
		//Buildable Material
		new ScriptObject(MatterType) { name="Wood";			color="75502eff";	tier=1;	spawnWeight=300;	spawnVeinSize=6;	spawnValue=512;	collectTime=2000;	placable=true;	health=1.0;	heatCapacity=25;	gatherableDB="brickEOTWGatherableBasicData";	fuelCapacity=2; };
		new ScriptObject(MatterType) { name="Granite";		color="c1a872ff";	tier=1;	spawnWeight=400;	spawnVeinSize=4;	spawnValue=256;	collectTime=4000;	placable=true;	health=2.0;	heatCapacity=40;	gatherableDB="brickEOTWGatherableBasicData"; };
		new ScriptObject(MatterType) { name="Glass";		color="181d26a8";	tier=2;	spawnWeight=150;	spawnVeinSize=4;	spawnValue=64;	collectTime=8000;	placable=true;	health=3.0;	heatCapacity=45;	gatherableDB="brickEOTWGatherableCrystalData"; };
		new ScriptObject(MatterType) { name="Iron";			color="7a7a7aff";	tier=2;	spawnWeight=200;	spawnVeinSize=5;	spawnValue=128;	collectTime=12000;	placable=true;	health=4.0;	heatCapacity=50;	gatherableDB="brickEOTWGatherableMetalData";	};
		new ScriptObject(MatterType) { name="Sturdium";		color="646defff";	tier=4;	spawnWeight=005;	spawnVeinSize=2;	spawnValue=40;	collectTime=24000;	placable=true;	health=999;	heatCapacity=75;	gatherableDB="brickEOTWGatherableMetalData";	};
		//Growable Organics
		new ScriptObject(MatterType) { name="Moss";			color="75ba6dff";	tier=1;	spawnWeight=050;	spawnVeinSize=2;	spawnValue=4;	collectTime=1000;	gatherableDB="brickEOTWGatherableBasicData";	 };
		new ScriptObject(MatterType) { name="Vines";		color="226027ff";	tier=2;	spawnWeight=050;	spawnVeinSize=2;	spawnValue=16;	collectTime=1500;	gatherableDB="brickEOTWGatherableBasicData";	 };
		//Basic Gatherable Materials
		new ScriptObject(MatterType) { name="Copper";		color="d36b04ff";	tier=3;	spawnWeight=100;	spawnVeinSize=4;	spawnValue=32;	collectTime=13000;	gatherableDB="brickEOTWGatherableMetalData";	cableTransfer=1024;	 };
		new ScriptObject(MatterType) { name="Silver";		color="e0e0e0ff";	tier=3;	spawnWeight=075;	spawnVeinSize=4;	spawnValue=16;	collectTime=14000;	gatherableDB="brickEOTWGatherableMetalData";	 };
		new ScriptObject(MatterType) { name="Lead";			color="533d60ff";	tier=3;	spawnWeight=050;	spawnVeinSize=4;	spawnValue=48;	collectTime=15000;	gatherableDB="brickEOTWGatherableMetalData";	pipeTransfer=16; };
		new ScriptObject(MatterType) { name="Gold";			color="e2af14ff";	tier=4;	spawnWeight=030;	spawnVeinSize=3;	spawnValue=56;	collectTime=20000;	gatherableDB="brickEOTWGatherableMetalData";	 };
		new ScriptObject(MatterType) { name="Diamond";		color="00d0ffa8";	tier=4;	spawnWeight=010;	spawnVeinSize=2;	spawnValue=8;	collectTime=22000;	gatherableDB="brickEOTWGatherableCrystalData";	 };
		//Alloys
		new ScriptObject(MatterType) { name="Electrum";		color="dfc47cff";	tier=5;	cableTransfer=4096;	};
		new ScriptObject(MatterType) { name="Energium";		color="d69c6bff";	tier=6;	cableTransfer=16384;};
		new ScriptObject(MatterType) { name="Rosium";		color="ca959eff";	tier=5;	pipeTransfer=64;	};
		new ScriptObject(MatterType) { name="Naturum";		color="83bc8cff";	tier=6;	pipeTransfer=256;	};
		new ScriptObject(MatterType) { name="Steel";		color="2f2d2fff";	tier=4;	};
		new ScriptObject(MatterType) { name="Addy Base";	color="561f1cff";	tier=5;	};
		new ScriptObject(MatterType) { name="Adamantine";	color="bf1f21ff";	tier=6;	};
		//Other Organics
		new ScriptObject(MatterType) { name="Bio Fuel";		color="93690eff";	tier=3;	fuelCapacity=128;	};
		new ScriptObject(MatterType) { name="Gibs";			color="82281fff";	tier=2;	};
		new ScriptObject(MatterType) { name="Rubber";		color="18161aff";	tier=3;	};
		new ScriptObject(MatterType) { name="Leather";		color="503623ff";	tier=3;	};
		//Complex Gatherable Materials
		new ScriptObject(MatterType) { name="Coal";			color="000000ff";	tier=3;	spawnWeight=50;	spawnVeinSize=4;	spawnValue=96;	collectTime=10000;	fuelCapacity=67;	gatherableDB="brickEOTWGatherableBasicData";	};
		new ScriptObject(MatterType) { name="Crude Oil";	color="1c1108ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Fluorine";		color="1f568cff";	tier=4;	spawnWeight=15;	spawnVeinSize=4;	spawnValue=32;	collectTime=10000;	requiredCollectFuel=("Sulfur" TAB 16);	gatherableDB="brickEOTWGatherableCrystalData";	};
		new ScriptObject(MatterType) { name="Uranium";		color="007c3fff";	tier=4;	spawnWeight=15;	spawnVeinSize=2;	spawnValue=64;	collectTime=18000;	requiredCollectFuel=("Sulfur" TAB 32);	gatherableDB="brickEOTWGatherableCrystalData";	};
		//Chemicals
		new ScriptObject(MatterType) { name="Petroleum";	color="4f494bff";	tier=3;	fuelCapacity=150;	};
		new ScriptObject(MatterType) { name="Sulfur";		color="93690eff";	tier=4;	};
		new ScriptObject(MatterType) { name="Ethanol";		color="953800ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Ethylene";		color="a5a189ff";	tier=4;	};
		new ScriptObject(MatterType) { name="Plastic";		color="797260ff";	tier=4;	};
		new ScriptObject(MatterType) { name="Teflon";		color="504b3fff";	tier=5;	};
		new ScriptObject(MatterType) { name="Rocket Fuel";	color="f8cfaaff";	tier=5;	fuelCapacity=250;	};
		new ScriptObject(MatterType) { name="Dielectrics";	color="264b38ff";	tier=5;	};
		//Water Based
		new ScriptObject(MatterType) { name="Water";		color="bcc1c88e";	tier=1;	};
		new ScriptObject(MatterType) { name="Oxygen";		color="bcc1c88e";	tier=2;	};
		new ScriptObject(MatterType) { name="Hydrogen";		color="bcc1c88e";	tier=2;	};
		new ScriptObject(MatterType) { name="Brine";		color="bcc1c88e";	tier=3;	};
		new ScriptObject(MatterType) { name="Lithium";		color="706e6eff";	tier=4;	};
		new ScriptObject(MatterType) { name="Tritium";		color="ffffffff";	tier=5;	};
		new ScriptObject(MatterType) { name="Deuterium";	color="ffffffff";	tier=5;	};
		new ScriptObject(MatterType) { name="Coolant";		color="9ab6b5ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Cryostablizer";color="89a3b8ff";	tier=5;	};
		//Nuclear
		new ScriptObject(MatterType) { name="Fissile Fuel";	color="56643bff";	tier=6;	fuelCapacity=300;	};
		new ScriptObject(MatterType) { name="Nuclear Waste";color="605042ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Polonium";		color="d8d1ccff";	tier=6;	};
		//Exotic
		new ScriptObject(MatterType) { name="Infinity";		color="3d5472ff";	tier=7;	};
		new ScriptObject(MatterType) { name="Singularity";	color="ffffff00";	tier=7;	};
		new ScriptObject(MatterType) { name="Scrip";		color="507582ff";	tier=7;	};
	};
	
	$EOTW::PlacableList = "";
	for (%i = 0; %i < MatterData.getCount(); %i++)
		if (MatterData.getObject(%i).placable)
			$EOTW::PlacableList = $EOTW::PlacableList TAB MatterData.getObject(%i).name;
	$EOTW::PlacableList = trim($EOTW::PlacableList);
	
	$EOTW::MatSpawnWeight = 0;
	$EOTW::MatSpawnList = "";
	for (%i = 0; %i < MatterData.getCount(); %i++)
	{
		if (MatterData.getObject(%i).spawnWeight > 0)
		{
			$EOTW::MatSpawnList = $EOTW::MatSpawnList TAB MatterData.getObject(%i).name;
			$EOTW::MatSpawnWeight += MatterData.getObject(%i).spawnWeight;
		}
	}
	$EOTW::MatSpawnList = trim($EOTW::MatSpawnList);
}
SetupMatterData();

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
	
	if(!isObject(Gatherables))
		MainBrickgroup.add(new SimGroup(Gatherables) { bl_id = 1337; name = "God"; });
		
	if (Gatherables.getCount() < (getMapArea() * $EOTW::MatterDensity))
		SpawnGatherableVein();
	
	for (%j = 0; %j < 30 && %despawnValue < Gatherables.getCount(); %j++)
	{
		%brick = Gatherables.getObject(%despawnValue);
		if (isObject(%brick))
		{
			//Just loop through each player instead of doing a radius raycast since that is significantly more expensive comp. wise
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
				%brick.delete();
				
		}
		
		%despawnValue++;
	}
	
	if (%despawnValue >= Gatherables.getCount())
		%despawnValue = 0;
	
	$EOTW::GatherableLoop = schedule(50, 0, "GatherableSpawnLoop", %despawnValue);
}
schedule(10, 0, "GatherableSpawnLoop");

function spawnGatherableRandom(%eye)
{
	if (%eye $= "")
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2)) / 2) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3)) / 2) SPC 1000;
		
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
	%origin = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2)) / 2) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3)) / 2) SPC 495;
	%matter = GetRandomSpawnMaterial();
	
	%veinSize = getRandom(1, %matter.spawnVeinSize);
	%veinRange = 8;
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
		if(isObject(%hit = firstWord(%ray)))
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
	
	if(!isObject(Gatherables))
		MainBrickgroup.add(new SimGroup(Gatherables) { bl_id = 1337; name = "God"; });
		
	if (%despawnLife > 0)
		%brick.DespawnLife = %despawnLife;
	else
		%brick.DespawnLife = getRandom(100, 300);
	
	Gatherables.add(%brick);
	
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
							%cl.centerPrint("\c0Whoops!<br>\c6You don't have enough " @ %name @ " to place that brick!<br>\c6You need" SPC (%volume - $EOTW::Material[%cl.bl_id, %name]) SPC "more.", 3);
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
						%cl.centerPrint("\c0Whoops!<br>\c6You don't have enough " @ %cl.buildMaterial @ " to place that brick!<br>\c6You need" SPC (%volume - $EOTW::Material[%cl.bl_id, %mat]) SPC "more.", 3);
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
		if(!%brick.dontRefund)
		{
			%data = %brick.getDatablock();
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
				$EOTW::Material[%brick.getGroup().bl_id, %brick.material] += %data.brickSizeX * %data.brickSizeY * %data.brickSizeZ;
			
		}
		
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
							%cl.centerPrint("<color:FFFFFF>Someone is already collecting that material brick!", 3);
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