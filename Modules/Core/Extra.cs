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

function doTipLoop(%num)
{
	cancel($EOTW::TipLoop);
	%num++;
	switch (%num)
	{
		case 1: %text = "\c5Tip\c6: Crouch while scrolling through your materials to scroll backwards.";
		case 2: %text = "\c5Tip\c6: If you have build trust with another player, your machines can interact with eachother.";
		case 3: %text = "\c5Tip\c6: The hilly terrain can be used as a ramp boost.";
		case 4: %text = "\c5Tip\c6: \cp<color:ff00ff>I LOVE LEAN!!!\co";
		case 5: %text = "\c5Tip\c6: This world can be exploited for maximal capitalistic gain.";
		case 6: %text = "\c5Tip\c6: Having multiple of the same machine can allow further automation.";
		case 7: %text = "\c5Tip\c6: With an empty hand and no ghost brick, press Plant Brick to view machine crafting recipes.";
		case 8: %text = "\c5Tip\c6: Energy pickups will fill up your player battery. However, if your battery is full it will instead decay!";
		case 9: %text = "\c5Tip\c6: A man without a checkpoint is better off homeless.";
		default: %text = "\c5Tip\c6: Dying is bad, don't do it."; %num = 0;
	}
	
	messageAll('',%text);
	
	$EOTW::TipLoop = schedule(60000 * 3, 0, "doTipLoop",%num);
}
schedule(60000 * 3, 0, "doTipLoop",%num);

function getGatherableDensity()
{
	deleteVariables("$EOTW::MatTest*");
	
	for (%i = 0; %i < Brickgroup_1337.getCount(); %i++)
		$EOTW::MatTest[Brickgroup_1337.getObject(%i).material]++;
		
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

function getFieldIndex(%text, %field)
{
	for (%i = 0; %i < getFieldCount(%text); %i++)
		if (%field $= getField(%text, %i))
			return %i;
	return -1;
}

function HexToRGB(%hex) {
	if(strLen(%hex) != 6) {
		return;
	}

	%chars = "0123456789abcdef";

	for(%i=0;%i<3;%i++) {
		%value = getSubStr(%hex, 2*%i, 2);

		%first = getSubStr(%value, 0, 1);
		%last = getSubStr(%value, 1, 1);
		
		%first = stripos(%chars, %first)*16;
		%last = stripos(%chars, %last);

		%sum = %first + %last;
		%str = trim(%str SPC %sum);
	}

	return %str;
}

function RGBToHex(%rgb) {
	%rgb = getWords(%rgb,0,2);
	for(%i=0;%i<getWordCount(%rgb);%i++) {
		%dec = mFloor(getWord(%rgb,%i)*255);
		%str = "0123456789ABCDEF";
		%hex = "";

		while(%dec != 0) {
			%hexn = %dec % 16;
			%dec = mFloor(%dec / 16);
			%hex = getSubStr(%str,%hexn,1) @ %hex;    
		}

		if(strLen(%hex) == 1)
			%hex = "0" @ %hex;
		if(!strLen(%hex))
			%hex = "00";

		%hexstr = %hexstr @ %hex;
	}

	if(%hexstr $= "") {
		%hexstr = "FF00FF";
	}
	return %hexstr;
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
	if (%client.isSuperAdmin)
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

function ServerCmdDumpMatSpawnRates(%client)
{
	if (%client.isSuperAdmin)
	{
		for (%i = 0; %i < MatterData.getCount(); %i++)
		{
			%matter = MatterData.getObject(%i);
			if (%matter.spawnWeight > 0)
			{
				talk(%matter.name @ ": " @ ((%matter.spawnWeight / $EOTW::MatSpawnWeight) * 100) @ "%");
			}
		}
	}
}

function serverCmdDonate(%client,%receiver,%amt,%mat1,%mat2,%mat3,%mat4)
{
	%mat = trim(%mat1 SPC %mat2 SPC %mat3 SPC %mat4);
	%amt = mFloor(%amt);
	if (%receiver $= "" || %amt $= "" || %mat $= "")
	{
		messageClient(%client, '', "Usage: /donate <receiver> <amount> <material>");
		return;
	}
	%rc = findClientByName(%receiver);
	if (%rc == 0)
	{
		messageClient(%client, '', "Could not find user:" SPC %receiver @ ".");
		return;
	}
	
	if (%rc == %client)
	{
		messageClient(%client, '', "You can't donate to yourself.");
		return;
	}
	
	if (%amt < 1)
	{
		messageClient(%client, '', "You need to donate atleast one material.");
		return;
	}
	
	if (!isObject(%matter = getMatterType(%mat)))
	{
		messageClient(%client, '', %mat SPC "is not a material.");
		return;
	}
	
	if ($EOTW::Material[%client.bl_id, %mat] < %amt)
	{
		messageClient(%client, '', "You can't donate than more of what you have. (You have " @ $EOTW::Material[%client.bl_id, %mat] @ " of the material you selected.)");
		return;
	}
	
	$EOTW::Material[%client.bl_id, %matter.name] -= %amt;
	$EOTW::Material[%rc.bl_id, %matter.name] += %amt;

	messageClient(%client, '', "\c6You donated\c4 " @ %amt @ " \c2" @ %matter.name @ " \c6to\c5 " @ %rc.netName @ "\c6.");
	messageClient(%rc, '', "\c5" @ %client.netName @ "\c6 gave you \c4" @ %amt @ " \c2" @ %matter.name @ "\c6.");
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

function purgeAllGatherables()
{
	%dataNames = "brickEOTWOilGeyserData";
	for (%i = 0; %i < MainBrickGroup.getCount(); %i++)
	{
		%group = MainBrickGroup.getObject(%i);
		for (%j = 0; %j < %group.getCount(); %j++)
		{
			%brick = %group.getObject(%j);
			if (hasWord(%dataNames, %brick.getDatablock().getName()))
				%brick.delete();
		}
	}
}

function periodicConsoleLogReset()
{
	cancel($EOTW::LogReset);
	setLogMode(0);
	setLogMode(2);

	$EOTW::LogReset = schedule(1000 * 120, 0, "periodicConsoleLogReset");
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




function serverCmdGullible(%cl)
{
	if ($EOTW::ColorScroll[%cl.bl_id] $= "")
	{
		%cl.chatMessage("\c5Super secret rainbow player name activated!");
		%cl.player.ScrollNameColor();
	}
	else
	{
		%cl.chatMessage("\c5Super secret rainbow player name deactivated!");
		cancel($EOTW::ColorScroll[%cl.bl_id]);
		$EOTW::ColorScroll[%cl.bl_id] = "";
		%cl.player.setShapeNameColor("1 1 1");
	}
}

function Player::ScrollNameColor(%obj,%scroll)
{
	%scroll++;
	switch (%scroll)
	{
		case  1: %obj.setShapeNameColor("1.00 0.00 0.00");
		case  2: %obj.setShapeNameColor("1.00 0.42 0.00");
		case  3: %obj.setShapeNameColor("1.00 0.72 0.00");
		case  4: %obj.setShapeNameColor("0.71 1.00 0.00");
		case  5: %obj.setShapeNameColor("0.30 1.00 0.00");
		case  6: %obj.setShapeNameColor("0.00 1.00 0.12");
		case  7: %obj.setShapeNameColor("0.00 1.00 0.35");
		case  8: %obj.setShapeNameColor("0.00 1.00 1.00");
		case  9: %obj.setShapeNameColor("0.00 0.58 1.00");
		case 10: %obj.setShapeNameColor("0.00 0.15 1.00");
		case 11: %obj.setShapeNameColor("0.28 0.00 1.00");
		case 12: %obj.setShapeNameColor("0.70 0.00 1.00");
		case 13: %obj.setShapeNameColor("1.00 0.00 0.86");
		case 14: %obj.setShapeNameColor("1.00 0.00 0.43"); %scroll = 0;
	}
	
	$EOTW::ColorScroll[%obj.client.bl_id] = %obj.schedule(1000 / 14,"ScrollNameColor",%scroll);
}