function dumpTest(%amt, %reset)
{
	if (%reset)
	{
		deleteVariables("$EOTW::RandomTest*");
	
		for (%i = 0; %i < %amt; %i++)
			spawnGatherable();
	}
		
	for (%i = 0; %i < getFieldCount($EOTW::MatSpawnList); %i++)
	{
		%matter = GetMatterType(getField($EOTW::MatSpawnList, %i));
		echo(%matter.name SPC ($EOTW::RandomTest[%matter.name] + 0));
	}
	
}

function getGatherableDensity()
{
	deleteVariables("$EOTW::MatTest*");
	
	for (%i = 0; %i < Gatherables.getCount(); %i++)
		$EOTW::MatTest[Gatherables.getObject(%i).material]++;
		
	for (%i = 0; %i < getFieldCount($EOTW::MatSpawnList); %i++)
	{
		%matter = GetMatterType(getField($EOTW::MatSpawnList, %i));
		echo(%matter.name SPC ($EOTW::MatTest[%matter.name] + 0));
		%sum += $EOTW::MatTest[%matter.name];
	}
	echo("Total: " @ %sum);
}

function getFieldFromValue(%list, %value)
{
	for (%i = 0; %i < getFieldCount(%list); %i++)
		if (getField(%list, %i) $= %value)
			return %i;
			
	return -1;
}

function decimalFromHex(%hex)
{
	%seq = "0123456789ABCDEF";
	
	%val = 0;
	for (%i = 0; %i < strLen(%hex); %i++)
	{
		%char = getSubStr(%hex, %i, 1);
		%d = striPos(%seq, %char);
		%val = 16*%val + %d;
	}
	
	return %val;
}
//
function hexFromFloat(%float)
{
	%hex = "0123456789abcdef";
	
	%val = %float * 255;
	
	%first = getSubStr(%hex, mFloor(%val / 16), 1);
	%second = getSubStr(%hex, mFloor(%val % 16), 1);
	
	return %first @ %second;
}

function getColorFromHex(%hex)
{
	for (%i = 0; %i < strLen(%hex); %i += 2)
		%color = %color SPC (decimalFromHex(getSubStr(%hex, %i, 2)) / 255);
		
	if (strLen(%hex) == 6)
		%color = %color SPC "1.0";
	
	return getClosestColor(trim(%color));
}

function ServerCmdHexFromPaintColor(%client)
{
	if (!%client.isSuperAdmin || !isObject(%player = %client.player))
		return;
	
	%color = getColorIDTable(%player.currSprayCan);
	
	%hex = hexFromFloat(getWord(%color, 0)) @ hexFromFloat(getWord(%color, 1)) @ hexFromFloat(getWord(%color, 2)) @ hexFromFloat(getWord(%color, 3));
	SetClipboard(%hex);
	talk(%hex);
}

function SimObject::doCall(%this, %method, %arg0, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9)
{
	for (%i = 0; %arg[%i] !$= ""; %i++)
		%args = %args @ %arg[%i] @ ",";
		
	%args = getSubStr(%args, 0, getMax(strLen(%args) - 1, 0));
	
	eval(%this @ "." @ %method @ "(" @ %args @ ");");
}

function hasWord(%str, %word)
{
	for (%i = 0; %i < getWordCount(%str); %i++)
		if (getWord(%str, %i) $= %word)
			return true;
			
	return false;
}

function SimSet::Shuffle(%set)
{
	%count = %set.getCount();
	
	while (%count > 0)
	{
		%count--;
		%obj = %set.getObject(getRandom(0, %count));
		%set.pushToBack(%obj);
	}
}

function ServerCmdGetAllMats(%client)
{
	if (true || %client.isSuperAdmin) //Yeah I know but it is just there for players to play around sandbox style as the game is being developed
	{
		for (%i = 0; %i < MatterData.getCount(); %i++)
			$EOTW::Material[%client.bl_id, MatterData.getObject(%i).name] += 500000;
			
		%client.chatMessage("Inventory updated.");
	}
}

function ServerCmdReloadCode(%client)
{
	if (%client.isSuperAdmin)
	{
		exec("add-ons/gamemode_solar_apoc_expanded2/server.cs");
		createuinametable();
		transmitdatablocks();
		commandtoall('missionstartphase3');
		
		%client.chatMessage("Code reloaded.");
	}
}

function setNewSkyBox(%dml)
{
	for (%i = 0; %i < $EnvGUIServer::SkyCount; %i++)
	{
		if ($EnvGUIServer::Sky[%i] $= %dml)
		{
			servercmdEnvGui_SetVar(EnvMaster, "SkyIdx", %i);
			break;
		}
	}
}

function setNewWater(%water)
{
	for (%i = 0; %i < $EnvGUIServer::WaterCount; %i++)
	{
		if ($EnvGUIServer::Water[%i] $= %water)
		{
			servercmdEnvGui_SetVar(EnvMaster, "WaterIdx", %i);
			break;
		}
	}
}

function mMin(%a, %b)
{
	if (%a > %b)
		return %b;
	else
		return %a;
}

function mMax(%a, %b)
{
	if (%a < %b)
		return %b;
	else
		return %a;
}