exec("./Lava.cs");

$EOTW::LoadMap = false;

function EnvMasterSetup()
{
	if ($EOTW::Initilized)
		return;
		
	if(!isObject(EnvMaster))
			new ScriptObject(EnvMaster) { isAdmin = 1; isSuperAdmin = 1; environMaster = 1; };
	
	if ($EOTW::LoadMap)
		serverDirectSaveFileLoad("Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Environment/MainMap2.bls", 3, "", 2);

	setNewSkyBox("Add-Ons/Sky_ROBLOX/Alien Red/AlienRed.dml");
	setNewWater("Add-Ons/Water_BrickLava/bricklava.water");
	
	$EOTW::TimeScale = 1;
	$EOTW::TimeDialation = 0.003;
	$EOTW::IsDay = false;
	$EOTW::Day = 0;
	$EOTW::Time = 23;
	SetWorldColor($EOTW::Day);
	servercmdEnvGui_SetVar(EnvMaster, "SimpleMode",0);
	servercmdEnvGui_SetVar(EnvMaster, "SunFlareColor", "0 0 0");
	servercmdEnvGui_SetVar(EnvMaster, "SunAzimuth", 75);
	setLavaHeight(35);
	servercmdEnvGui_SetVar(EnvMaster, "WaterColor", "1 0.5 0 0.05");
	servercmdEnvGui_SetVar(EnvMaster, "GroundColor", "0.4 0.4 0.4 1.0");
	servercmdEnvGui_SetVar(EnvMaster, "UnderWaterColor", "1 0.5 0 1");
	
	EnvMasterLoop();
	
	$EOTW::Initilized = true;
}
schedule(200,0,"EnvMasterSetup");

function SetWorldColor(%day)
{
	%size = 0.1 * %day;
	if (%size > 5)
		%size = 5;
		
	%color = "1 1 1";
	%colInc = "0.00 -0.02 -0.02";
	%colTarg = "1 0 0";
	for (%i = 0; %i < %day; %i++)
	{
		%color = vectorAdd(%color, %colInc);
		if(vectorDist(%color, %colTarg) == 0)
		{
			switch$(%colTarg)
			{
				case "1 0 0":
					if(%colInc $= "0.1 0 0")
					{
						%colInc = "0 0.1 0";
						%colTarg = "1 1 0";
					}
					else
					{
						%colInc = "0 0 0";
						%colTarg = "0 0 0";
					}
				case "1 1 0":
					%colInc = "-0.1 0 0";
					%colTarg = "0 1 0";
				case "0 1 0":
					%colInc = "0 0 0.1";
					%colTarg = "0 1 1";
				case "0 1 1":
					%colInc = "0 -0.1 0";
					%colTarg = "0 0 1";
				case "0 0 1":
					%colInc = "0.1 0.1 0";
					%colTarg = "1 1 1";
				case "1 1 1":
					%colInc = "0 0 0";
					%colTarg = "0 0 0";
			}
		}
	}
	
	$EOTW::WorldColor = %color;
	$EOTW::SunSize = %size;
	
	if ($EOTW::Time < 12)
		servercmdEnvGui_SetVar(EnvMaster, "SunFlareColor", $EOTW::WorldColor);
	else
		servercmdEnvGui_SetVar(EnvMaster, "SunFlareColor", "0 0 0");
}

function EnvMasterLoop()
{
	cancel($EOTW::EnvMasterLoop);
	
	//Time events
	if ($EOTW::Time >= 24)
	{
		if (!$EOTW::IsDay)
		{
			$EOTW::IsDay = true;
			$EOTW::Time -= 24;
			$EOTW::Day++;
			
			%stats = EnvMasterRollWeather($EOTW::Day);
			setWorldColor(getField(%stats, 0));
			
			servercmdEnvGui_SetVar(EnvMaster, "SunFlareColor", $EOTW::WorldColor);
			
			talk("The sun rises on day " @ $EOTW::Day @ ".");
			talk("Today's Weather: " @ "[HEAT: " @ (getField(%stats, 0) * 0.1) @ "] [MOBS: Peaceful] [STORM: None]");
		}
	}
	else if ($EOTW::Time >= 12)
	{
		if ($EOTW::IsDay)
		{
			servercmdEnvGui_SetVar(EnvMaster, "SunFlareColor", "0 0 0");
			
			talk("The sun sets on day " @ $EOTW::Day @ ".");
			
			%stats = EnvMasterRollWeather($EOTW::Day + 1);
			talk("Tommorow's Weather: " @ "[HEAT: " @ ((getField(%stats, 1) * 0.1) - 0.2) @ "-" @ ((getField(%stats, 1) * 0.1) + 0.2) @ "] [MOBS: Peaceful] [STORM: None]");
			
			$EOTW::IsDay = false;
		}
	}
	else
	{
		EnvMasterSunDamageEntity();
		EnvMasterSunDamageBrick();
	}
	
	//Time Calculations
	if($EOTW::Time > 12)
	{
		%flare = 0;
		%realA = 1 - (mAbs(%time - 6) / 8);
		%realB = 1 - (mAbs(%time - 30) / 8);
		%realFlare = getMax(%realA, %realB);
		if(%realFlare < 0) 
			%realFlare = 0;
	}
	else
	{
		%flare = 1 - (mAbs($EOTW::Time - 6) / 8);
		%realFlare = %flare;
	}
		
	if($EOTW::Time >= 22 || $EOTW::Time <= 14)
	{
		if($EOTW::Time >= 22)
			%colTime = $EOTW::Time - 24;
		else
			%colTime = $EOTW::Time;
		%colVal = 1 - mAbs((%colTime - 6) / 8);
	}
	else
		%colVal = 0;
		
	if($EOTW::Time >= 23 || $EOTW::Time <= 1)
	{
		%fogTime = -(mAbs($EOTW::Time - 12) - 12);
		%fogVal = 1 - %fogTime;
	}
	else if($EOTW::Time >= 11 && $EOTW::Time <= 13)
		%fogVal = 1 - mAbs($EOTW::Time - 12);
	else
		%fogVal = 0;
	if(%fogVal >= 0)
		servercmdEnvGui_SetVar(EnvMaster, "FogColor", vectorScale(vectorAdd(vectorScale($EOTW::WorldColor, %fogVal), vectorScale($EOTW::WorldColor, %colVal)), 0.8));
		
	%sizeVal = %colVal * mSqrt(%flare);
	%realSizeVal = %colVal * mSqrt(%realFlare);
	%sunCol = vectorScale($EOTW::WorldColor, (%realSizeVal + %sizeVal) * 0.5);
	%ambCol = vectorScale($EOTW::WorldColor, %realSizeVal * 0.7);
	%shadCol = vectorScale($EOTW::WorldColor, %realSizeVal * 0.4);
	servercmdEnvGui_SetVar(EnvMaster, "VignetteColor", vectorScale($EOTW::WorldColor, %colVal) SPC (($EOTW::SunSize * %colVal) / 5));
	servercmdEnvGui_SetVar(EnvMaster, "SkyColor", vectorScale($EOTW::WorldColor, %colVal));
	servercmdEnvGui_SetVar(EnvMaster, "DirectLightColor", %sunCol);
	servercmdEnvGui_SetVar(EnvMaster, "AmbientLightColor", %ambCol);
	servercmdEnvGui_SetVar(EnvMaster, "ShadowColor", %shadCol);
	servercmdEnvGui_SetVar(EnvMaster, "SunFlareSize", (%val = (mSqrt(%flare) * $EOTW::SunSize)) < 0.1 ? 0.1 : %val);
	servercmdEnvGui_SetVar(EnvMaster, "SunElevation", ($EOTW::Time / 24) * 360);
	
	$EOTW::Time += ($EOTW::TimeScale * $EOTW::TimeDialation);
	$EOTW::EnvMasterLoop = schedule(100, 0, "EnvMasterLoop");
}

function EnvMasterRollWeather(%day)
{
	%dayLoop = %day % 60;
	if (%dayLoop >= 40)
		%sunSize = 40 - (2 * (%dayLoop - 40));
	else
		%sunSize = %dayLoop;
	
	%sunSize += 3;
	%sunSizeRoll += %sunSize + getRandom(-2, 2);
	return %sunSizeRoll TAB %sunSize;
}

function EnvMasterSunDamageEntity()
{
	%val = ($EOTW::Time / 12) * $pi;
	%ang = ($EnvGuiServer::SunAzimuth / 180) * $pi;
	%dir = vectorScale(mSin(%ang) * mCos(%val) SPC mCos(%ang) * mCos(%val) SPC mSin(%val), 512);
	%pos[-1+%posCount++] = "0 0 0";
	
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
		if(isObject(%pl = ClientGroup.getObject(%i).player))
			%pos[-1+%posCount++] = %pl.getPosition();
			
	for(%i = 0; %i < %posCount; %i++)
	{
		initContainerRadiusSearch(%pos[%i], 240, $Typemasks::PlayerObjectType | $TypeMasks::VehicleObjectType);
		while(isObject(%obj = containerSearchNext()))
		{
			%isVehicle = striPos(%obj.getClassName(),"Vehicle") > -1;
			if(!%hasHarmed[%obj] && (%isVehicle || %obj.getState() !$= "DEAD"))
			{
				%hasHarmed[%obj] = 1;
				
				if (%isVehicle) %hit = containerRaycast(vectorAdd(%pos = %obj.getPosition(), %dir), %pos, $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType);
				else %hit = containerRaycast(vectorAdd(%pos = %obj.getHackPosition(), %dir), %pos, $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType);
				
				//drawArrow(%pos, %dir, "1 0 0", 2, 1, 0).schedule(1000,"delete");
				
				if(!isObject(%hit) && $EOTW::SunSize >= 1 && $EOTW::Time < 12 && $EOTW::Timescale > 0)
				{
					%obj.damage(0, %obj.getPosition(), $EOTW::SunSize, '');
					
					if (!%isVehicle)
						%obj.setDamageFlash(0.25);
				}
			}
		}
	}
}

$EOTW::SunDamageBlacklist = "888888 1337";
function EnvMasterSunDamageBrick()
{
	%brickcount = 0;
	for (%i = 0; %i < MainBrickgroup.getCount(); %i++)
	{
		%group = MainBrickgroup.getObject(%i);
		if (!hasWord($EOTW::SunDamageBlacklist, %group.bl_id))
		{
			%brickcount += %group.getCount();
			%brickgroups = %brickgroups SPC %group;
		}
	}
	%brickgroups = trim(%brickgroups);
	
	%val = ($EOTW::Time / 12) * $pi;
	%ang = ($EnvGuiServer::SunAzimuth / 180) * $pi;
	%dir = vectorScale(mSin(%ang) * mCos(%val) SPC mCos(%ang) * mCos(%val) SPC mSin(%val), 512);
			
	for(%i = 0; %i < (%brickcount / 2000); %i++)
	{
		%rand = getRandom(0, %brickcount);
	
		for (%j = 0; %j < getWordCount(%brickgroups); %j++)
		{
			%group = getWord(%brickgroups, %j);
			%count = %group.getCount();
			
			if (%rand < %count)
				break;
			
			%rand -= %count;
		}
		
		if (isObject(%group) && %group.getCount() > 0)
		{
			%brick = %group.getObject(getRandom(0, %count - 1));
			%matter = getMatterType(%brick.material);
			if (isObject(%matter) && %matter.heatCapacity < ($EOTW::SunSize * 10))
			{
				%ray = containerRaycast(vectorAdd(%pos = %brick.getPosition(), %dir), %pos, $Typemasks::fxBrickAlwaysObjectType | $Typemasks::StaticShapeObjectType);
				if(!isObject(%hit = firstWord(%ray)) || %hit == %brick)
				{
					%damage = $EOTW::SunSize - (%matter.heatCapacity / 10);
					%brick.sunDamage += %damage;
					
					%data = %brick.getDatablock();
					%volume = %data.brickSizeX * %data.brickSizeY * %data.brickSizeZ;
					
					if (%brick.sunDamage >= (%volume * %matter.health))
					{
						%brick.dontRefund = true;
						%brick.killBrick();
					}
				}
			}
			
		}
	}
}