//People don't seem to like this interface so I need to figure out something better.

//////////////////////
//sap menu specifics//
//////////////////////
$BCM::DefaultMenu = "SAP";

//initial menu
$BCM::Menu["SAP", 1] = "static" SPC "Inventory\nMaterial Recipes\nHelp";
$BCM::Transitions["SAP", 1] = "2 3 4";
$BCM::PrevState["SAP", 1] = "0";

//helper functions
function GameConnection::getItemList(%client,%type)
{
	if (%type $= "Material")
	{
		%list = "Glass";
		%list = %list NL "Grass";
		%list = %list NL "Metal";
		if ($EOTW::Day >= 12) %list = %list NL "Copper";
		if ($EOTW::Day >= 17) %list = %list NL "Lead";
		%list = %list NL "Stone";
		if ($EOTW::Day >= 40) %list = %list NL getSturdiumName();
		%list = %list NL "Vine";
		%list = %list NL "Wood";
		%list = %list NL "Wolfram";
		return %list;
	}
	if (%type $= "Item")
	{
		%inv = ("InventoryHandler_" @ %client.bl_id).getID();
		
		for (%i = 0; %i < %inv.getCount(); %i++)
		{
			%list = %list NL %inv.getObject(%i).getName();
		}
		return removeRecord(%list, 0);
	}
	if (%type $= "Ammo")
	{
		return "Small Round\nMedium Rounds\nShotgun Shells\n.357 Rounds\n.308 Rounds\nHeavy Rounds\n40mm Grenades";
	}
	if (%type $= "Runes")
	{
		return "Air Rune\nWater Rune\nEarth Rune\nFire Rune\nLight Rune\nDark Rune\nArcane Rune\nCosmic Rune\nShatter Rune";
	}
	if (%type $= "Spellbook")
	{
		return "Work In Progress";
	}
	
}

function Menu_SetQuantity(%client, %selection, %type)
{
	if (%type $= "Material")
	{
		%client.BCM_MenuQuantity = $EOTW::Material[%client.bl_id, %selection] + 0;
	}
	if (%type $= "Item")
	{
		%inv = ("InventoryHandler_" @ %client.bl_id).getID();
		%client.BCM_MenuQuantity = %inv.EOTWInv_GetItem(%selection);
	}
	if (%type $= "Ammo")
	{
		if (!isObject(%client.player))
		{
			%client.BCM_MenuQuantity = 0;
			return;
		}
		
		switch$(%selection)
		{
		case "Small Rounds":
				%client.BCM_MenuQuantity = %client.player.sReserve["9mm"];
		case "Medium Rounds":
				%client.BCM_MenuQuantity = %client.player.sReserve["556"];
		case "Shotgun Shells":
				%client.BCM_MenuQuantity = %client.player.sReserve["shotgun_shells"];
		case ".357 Rounds":
				%client.BCM_MenuQuantity = %client.player.sReserve["357"];
		case ".308 Rounds":
				%client.BCM_MenuQuantity = %client.player.sReserve["308"];
		case "Heavy Rounds":
				%client.BCM_MenuQuantity = %client.player.sReserve["heavy"];
		case "40mm Grenades":
				%client.BCM_MenuQuantity = %client.player.sReserve["40mmGrenades"];
		default:
				%client.BCM_MenuQuantity = "1337";
		}
	}
	if (%type $= "Runes")
	{
		%client.BCM_MenuQuantity = %client.runes[getWord(%selection,0)] + 0;
	}
}

function Menu_VerifyQuantity(%client, %quantity)
{
	%selection = getRecord(%client.BCM_MenuCache, %client.BCM_MenuPosition);
	%amt = ("InventoryHandler_" @ %client.bl_id).EOTWInv_GetItem(%selection);
	if(%amt < %quantity)
	{
		messageClient(%client, 'MsgError');
		%client.BCM_MenuQuantity = %amt;
	}
}

function Menu_DisplayCrafting(%client, %menu, %position)
{
	%quantity = %client.BCM_MenuQuantity;
	%init = mClamp(%position - 2, 0, getRecordCount(%menu) - 5);
	%position-= %init;
	%rec = getRecord(%menu, %init);
	%color = %client.hasRequiredItems(%rec) ? "\c2" : "\c0";
	%print = %color @ %rec;
	for(%i = %init + 1; %i < %init + 5; %i++)
	{
		%rec = getRecord(%menu, %i);
		%color = %client.hasRequiredItems(%rec) ? "\c2" : "\c0";
		if(%client.canCraftAnything)
			%color = "\c2";
		
		%print = %print NL %color @ %rec;
	}
	%print = setRecord(%print, %position, "\c3>>" @ getRecord(%print, %position) @ " \c3x" @ %quantity);
	return %print;
}

////////////////////////
//BrickControlsMenu.cs//
////////////////////////
//by Amad� (ID 10716)

$BCM::Version = 1;

$BCM::Up1 = "0 0 1"; //shift up 1 plate
$BCM::Up3 = "0 0 3"; //shift up 1 block
$BCM::Down1 = "0 0 -1"; //shift down 1 plate
$BCM::Down3 = "0 0 -3"; //shift down 1 block
$BCM::Right = "-1 0 0";
$BCM::Left = "1 0 0";
$BCM::Forward = "0 1 0";
$BCM::Back = "0 -1 0";

//helper functions
function hasColor(%str)
{
	if(%str $= "")
	{
		return false;
	}
	%firstChar = getSubStr(%str, 0, 1);
	if(%firstChar $= "<")
	{
		return true;
	}
	return %firstChar $= "\c0" || %firstChar $= "\c1" || %firstChar $= "\c2" || %firstChar $= "\c3";
}

function BCM_GetMenuText(%str, %client)
{
	switch$(firstWord(%str))
	{
		case "static":
			return restWords(%str);
		//case "variable":
			//eval("%str = " @ restWords(%str));
			//return %str;
		case "function":
			eval("%str = " @ restWords(%str));
			return %str;
		default:
			return %str;
	}
}

//the main attraction
package BrickControlsMenu
{
	function serverCmdPlantBrick(%client)
	{
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuEnter();
		}
		else return parent::serverCmdPlantBrick(%client);
	}

	function serverCmdCancelBrick(%client)
	{
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuCancel();
		}
		else return parent::serverCmdCancelBrick(%client);
	}

	function serverCmdShiftBrick(%client, %y, %x, %z)
	{
		//y+: forward
		//x+: left
		//z+: up
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuShift(%x, %y, %z);
		}
		else return parent::serverCmdShiftBrick(%client, %y, %x, %z);
	}

	function serverCmdSuperShiftBrick(%client, %y, %x, %z)
	{
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuShift(%x, %y, %z);
		}
		else return parent::serverCmdSuperShiftBrick(%client, %y, %x, %z);
	}
};
activatePackage(BrickControlsMenu);

function GameConnection::BCM_MenuEnter(%client)
{
	if(!strLen(%profile = %client.BCM_MenuProfile))
	{
		%client.BCM_MenuProfile = %profile = $BCM::DefaultMenu;
	}
	%state = %client.BCM_MenuState;
	if(%state == 0)
	{
		return %client.BCM_MenuChangeState(1);
	}
	%position = %client.BCM_MenuPosition;
	if((%fn = $BCM::EnterCallback[%profile, %state]) !$= "")
	{
		if($BCM::EnableQuantity[%profile, %state])
		{
			%quantity = %client.BCM_MenuQuantity;
		}
		%menu = %client.BCM_MenuCache;
		%selection = stripMLControlChars(getRecord(%menu, %position));
		eval(%fn);
	}
	if((%transitions = $BCM::Transitions[%profile, %state]) !$= "")
	{
		%client.BCM_MenuChangeState(getWord(%transitions, %position));
	}
}

function GameConnection::BCM_MenuCancel(%client)
{
	if(isEventPending(%event = %client.BCM_MenuCancelSched))
	{
		cancel(%event);
		%client.centerPrint();
	}
	%client.BCM_MenuState = 0;
}

function GameConnection::BCM_MenuShift(%client, %x, %y, %z)
{
	if(!strLen(%profile = %client.BCM_MenuProfile))
	{
		%client.BCM_MenuProfile = %profile = $BCM::DefaultMenu;
	}
	%state = %client.BCM_MenuState;
	if(!%state)
	{
		return %client.BCM_MenuEnter;
	}
	switch$(%x SPC %y SPC %z)
	{
		case $BCM::Forward:
			return %client.BCM_MenuScroll(-1);
		case $BCM::Back:
			return %client.BCM_MenuScroll(1);
		case $BCM::Right:
			return %client.BCM_MenuEnter();
		case $BCM::Left:
			return %client.BCM_MenuBack();
		case $BCM::Up1:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(1);
			}
			else
			{
				return %client.BCM_MenuScroll(-1);
			}
		case $BCM::Up3:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(10);
			}
			else
			{
				return %client.BCM_MenuScroll(-1);
			}
		case $BCM::Down1:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(-1);
			}
			else
			{
				return %client.BCM_MenuScroll(1);
			}
		case $BCM::Down3:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(-10);
			}
			else
			{
				return %client.BCM_MenuScroll(1);
			}
	}
}

function GameConnection::BCM_MenuBack(%client)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%client.BCM_MenuChangeState($BCM::PrevState[%profile, %state]);
}

function GameConnection::BCM_MenuScroll(%client, %dir)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%menu = %client.BCM_MenuCache;
	%client.BCM_MenuPosition+= %dir;
	%position = %client.BCM_MenuPosition;
	%recordCount = getRecordCount(%menu);
	if(%position > %recordCount - 1 || %recordCount == 0)
	{
		%client.BCM_MenuPosition = %position = 0;
	}
	else if(%position < 0)
	{
		%client.BCM_MenuPosition = %position = %recordCount - 1;
	}
	if((%fn = $BCM::ScrollCallback[%profile, %state]) !$= "")
	{
		%selection = getRecord(%menu, %position);
		eval(%fn);
	}
	%client.BCM_MenuDisplay();
}

function GameConnection::BCM_MenuAddQuantity(%client, %amt)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%quantity = %client.BCM_MenuQuantity + %amt;
	if(%quantity < 1 && !$BCM::NegativeQuantity[%profile, %state])
	{
		%quantity = 1;
		messageClient(%client, 'MsgError');
	}
	%client.BCM_MenuQuantity = %quantity;
	if((%fn = $BCM::QuantityCallback[%profile, %state]) !$= "")
	{
		eval(%fn);
	}
	%client.BCM_MenuDisplay();
}

function GameConnection::BCM_MenuChangeState(%client, %newstate)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	if(%state != %newstate)
	{
		%client.BCM_MenuPosition = 0;
	}
	%position = %client.BCM_MenuPosition;
	%client.BCM_MenuState = %state = %newstate;
	if((%quantity = $BCM::DefaultQuantity[%profile, %state]) !$= "")
	{
		%client.BCM_MenuQuantity = %quantity;
	}
	else
	{
		%client.BCM_MenuQuantity = 0;
	}
	%client.BCM_MenuCache = %menu = BCM_GetMenuText($BCM::Menu[%profile, %state], %client);
	if((%fn = $BCM::ScrollCallback[%profile, %state]) !$= "")
	{
		%selection = getRecord(%menu, %position);
		eval(%fn);
	}
	%client.BCM_MenuDisplay();
}

function GameConnection::BCM_MenuDisplay(%client)
{
	if(%client.BCM_MenuState == 0)
	{
		return %client.centerPrint();
	}
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%menu = %client.BCM_MenuCache;
	%position = %client.BCM_MenuPosition;
	%recordCount = getRecordCount(%menu);
	%selection = getRecord(%menu, %position);
	if((%fn = $BCM::MenuFunction[%profile, %state]) !$= "")
	{
		return eval(%fn);
	}
	else if((%fn = $BCM::DisplayFunction[%profile, %state]) !$= "")
	{
		%print = eval(%fn);
	}
	else
	{
		%selection = hasColor(%selection) ? %selection : "\c2" @ %selection;
		if($BCM::EnableQuantity[%profile, %state])
		{
			%menu = setRecord(%menu, %position, "<div:1>\c3>>" @ %selection @ " \c3x" @ %client.BCM_MenuQuantity);
		}
		else
		{
			%menu = setRecord(%menu, %position, "<color:FFFF00><div:1>>>" @ %selection @ "<<");
		}
		%init = mClamp(%position - 2, 0, %recordCount - 5);
		%print = "\c2" @ getRecord(%menu, %init);
		for(%i = %init + 1; %i < %init + 5; %i++)
		{
			%rec = getRecord(%menu, %i);
			%rec = hasColor(%rec) ? %rec : "\c2" @ %rec;
			%print = %print NL %rec;
		}
	}
	%client.centerPrint(%print, 5);
	%client.scheduleBCM_MenuCancel();
}

function GameConnection::scheduleBCM_MenuCancel(%client)
{
	cancel(%client.BCM_MenuCancelSched);
	%client.BCM_MenuCancelSched = %client.schedule(5000, BCM_MenuCancel);
}