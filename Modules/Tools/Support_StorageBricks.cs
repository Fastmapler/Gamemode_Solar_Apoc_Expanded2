//Storage Bricks
datablock fxDTSBrickData(brickToolStorageData)
{
	brickFile = "./Bricks/Box.blb";
	category = "Solar Apoc";
	subCategory = "Support";
	uiName = "Tool Storage";
	//iconName = "./Bricks/Icon_Generator";

    maxStoredTools = 5;
};


function brickToolStorageData::onPlant(%this,%brick)
{
	Parent::onPlant(%this,%brick);
	if(isObject(%client = %brick.client))
	{
		if(!%client.hasSeenStorageHelp)
		{
			%client.chatmessage("\c3StorageHelp\c6: This brick can store items.");
			%client.chatmessage("\c3StorageHelp\c6: Select an item from your inventory by pulling it out, then closing your inventory.");
			%client.chatmessage("\c3StorageHelp\c6: Left-Click the storage brick with empty hands to put the last selected item into storage.");
			%client.chatmessage("\c3StorageHelp\c6: Right-Click with empty hands to pull an item out of storage.");
			%client.chatmessage("\c3StorageHelp\c6: Crouch with empty hands to view the brick's inventory.");
			%client.hasSeenStorageHelp = 1;
		}
		%client.centerPrint("\c6"@ %this.uiName @ "\n\c6Max Slots: \c3" @ %this.maxStoredTools, 4);
	}
}

function fxDtsData::toolStorageAdd(%brick, %player, %slot)
{
    for(%i = 0; %i < %brick.numEvents; %i++)
    {
        if(%brick.eventInput[%i] $= "setStoredItem")
        {
            %numItems++;
            %itemsList = %itemsList@"|\c6"@%brick.eventOutputParameter[%i,1]@"\c3|";
        }
    }
    if (%numItems >= %data.maxStoredTools || !isObject(%tool = %player.tool[%slot]))
        return;
    
    %numEvents = mAbs(%brick.numEvents);
    %brick.eventEnabled[%numEvents] = 1;
    %brick.eventDelay[%numEvents] = 0;
    %brick.eventInput[%numEvents] = "setStoredItem";
    %brick.eventInputIdx[%numEvents] = inputEvent_GetInputEventIdx("setStoredItem");
    %brick.eventTarget[%numEvents] = "And";
    %brick.eventTargetIdx[%numEvents] = 0;
    %brick.eventOutput[%numEvents] = "storeItem";
    %brick.eventOutputIdx[%numEvents] = outputEvent_GetOutputEventIdx("StorageBrick","storeItem");
    %brick.eventOutputParameter[%numEvents, 1] = %tool.getName();
	%brick.eventOutputParameter[%numEvents, 2] = %player.toolMag[%slot];
}

//Dummy events
function fxDTSBrick::setStoredItem(%brick){}
function StorageBrick::storeItem(%this,%client){}

registerInputEvent(fxDTSBrick, setStoredItem, "And StorageBrick");
registerOutputEvent(StorageBrick, storeItem, "string 200 50" TAB "string 200 50" TAB "string 200 50" TAB "string 200 50");

//Anti-Dup Function
function fxDTSBrick::dupedItems(%brick)
{
	for(%i=0;%i<%brick.numEvents;%i++)
	{
		if(%brick.eventInput[%i]$="setStoredItem")
			%brick.eventOutputParameter[%i,1] = "";
	}
}

if($Pref::Server::StorageAllowItemDuping$="")
	$Pref::Server::StorageAllowItemDuping = 0;

deactivatepackage(BrickStoragePackage);
package BrickStoragePackage
{
	function serverCmdClearEvents(%client)
	{
		if(!isObject(%brick = %client.wrenchBrick))
			return Parent::serverCmdClearEvents(%client);
		for(%i=0;%i<%brick.numEvents;%i++)
		{
			if(%brick.eventInput[%i]$="setStoredItem"&&%brick.eventOutputParameter[%i,1]!$="")
			{
				if(!%client.isAdmin)
				{
					%client.chatmessage("\c6Oops! You can't edit that brick due to the item stored on line "@%i@".");
					%client.chatmessage("\c6An admin could clear the item for you.");
					%client.wrenchBrick = "";
					return;
				}
				break;
			}
		}
		Parent::serverCmdClearEvents(%client);
	}
	function serverCmdAddEvent(%client, %enabled, %eventID, %delay, %targetID, %NameID, %outputID, %par1, %par2, %par3, %par4)
	{
		if(!isObject(%brick = %client.wrenchBrick))
			return Parent::serverCmdAddEvent(%client, %enabled, %eventID, %delay, %targetID, %NameID, %outputID, %par1, %par2, %par3, %par4);
		if(%eventID==inputEvent_GetInputEventIdx("setStoredItem"))
		{
			if(!%client.isAdmin)
				%par1 = "";
		}
		Parent::serverCmdAddEvent(%client, %enabled, %eventID, %delay, %targetID, %NameID, %outputID, %par1, %par2, %par3, %par4);
	}
	function WrenchImage::onHitObject(%this, %player, %slot, %object, %pos, %normal)
	{
		if(%object.getClassName()!$="fxDTSBrick")
			return Parent::onHitObject(%this, %player, %slot, %object, %pos, %normal);
		
		%data = %object.getDatablock();
		if(%data.maxStoredTools<=0)
			return Parent::onHitObject(%this, %player, %slot, %object, %pos, %normal);

		%adminonly = $Server::WrenchEventsAdminOnly;
		$Server::WrenchEventsAdminOnly = 1;
		Parent::onHitObject(%this, %player, %slot, %object, %pos, %normal);
		$Server::WrenchEventsAdminOnly = %adminonly;
	}
};
activatepackage(BrickStoragePackage);