$EOTW::PipeSizeLimit = 16;
$EOTW::PipeCostMulti = 1;

datablock itemData(PipeLayerItem)
{
	uiName = "Pipe Layer";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.70 0.25 0.25 1.00";
	
	shapeFile = "base/data/shapes/printGun.dts";
	image = PipeLayerImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(PipeLayerImage)
{
	shapeFile = "base/data/shapes/printGun.dts";
	item = PipeLayerItem;
	
	mountPoint = 0;
	offset = "0 0.25 0.15";
	rotation = eulerToMatrix("0 5 70");
	
	eyeOffset = "0.75 1.15 -0.24";
	eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = PipeLayerItem.doColorShift;
	colorShiftColor = PipeLayerItem.colorShiftColor;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.1;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "checkFire";
	
	stateName[3]					= "checkFire";
	stateTransitionOnTriggerUp[3] 	= "Ready";
};

function PipeLayerImage::onFire(%this, %obj, %slot)
{
	if(!isObject(%client = %obj.client))
		return;
	
	%pos = %obj.getEyePoint();
	%vector = vectorAdd(%pos, vectorScale(%obj.getEyeVector(), 5));
	%targets = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = ContainerRayCast(%pos, %vector, %targets, %obj);
	%col = getWord(%ray, 0);
	%hitpos = getWords(%ray, 1, 3);
	
	if (isObject(%col))
	{
		if (getTrustLevel(%obj, %col) < $TrustLevel::Hammer)
		{
			if (%col.stackBL_ID $= "" || %col.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}

		if (%col.getType() & $TypeMasks::FxBrickObjectType)
		{
			if (%obj.PipeLayerBuffer !$= "")
			{
				if (%obj.PipeLayerBuffer == %col)
				{
					%obj.PipeLayerBuffer = "";
					%client.chatMessage("\c6Cancelled Pipe.", 3);
				}
				else if (%col.getDatablock().matterSlots["Buffer"] <= 0 && %col.getDatablock().matterSlots["Input"] <= 0)
				{
					%client.chatMessage("\c6Target has no material input or buffer!", 3);
				}
				else if (getWordCount(%col.ropeGroups) >= $EOTW::MaxRopeCount)
				{
					%client.chatMessage("\c6Too many connected ropes! Max ropes: " @ $EOTW::MaxRopeCount, 3);
				}
				else
				{
					%cost = mCeil(vectorDist(%obj.PipeLayerBuffer.getPosition(), %col.getPosition()) * $EOTW::PipeCostMulti);
					%PipeType = getMatterType(%client.PipeLayerMat);
					%matInv = ($EOTW::Material[%client.bl_id, %PipeType.name] + 0);
					
					for (%i = 0; %i < getWordCount(%obj.PipeLayerBuffer.ropeGroups); %i++)
					{
						%group = getWord(%obj.PipeLayerBuffer.ropeGroups, %i);
						if (getWord(%group.creationData, 0) == %obj.PipeLayerBuffer.getID() && getWord(%group.creationData, 1) == %col.getID())
						{
							%client.chatMessage("\c6A Pipe already exists in this direction!", 3);
							return;
						}
						else if (getWord(%group.creationData, 1) == %obj.PipeLayerBuffer.getID() && getWord(%group.creationData, 0) == %col.getID())
						{
							%client.chatMessage("\c6A Pipe already exists in this direction!", 3);
							return;
						}
					}
					
					if (%cost > $EOTW::PipeSizeLimit)
					{
						%client.chatMessage("\c6Pipe is too long! Max Cost: " @ $EOTW::PipeSizeLimit, 3);
					}
					else if (%matInv >= %cost)
					{
						$EOTW::Material[%client.bl_id, %PipeType.name] -= %cost;
						
						if (%PipeType.PipeTransfer > 0)
							%transferRate = %PipeType.PipeTransfer / $EOTW::PowerTickRate;
						else
							%transferRate = 2;
							
						%client.chatMessage("\c6Pipe target set to " @ %col.getDatablock().uiName @ ". Pipe created!", 3);
						CreateTransferRope(%obj.PipeLayerBuffer, %obj.cableLayerBufferPort, %col.getPortPosition("MatterIn",%hitpos), %col, %transferRate, %PipeType.name, %cost, "Matter");
						%obj.PipeLayerBuffer = "";
					}
					else
						%client.chatMessage("\c6Not enough material to build Pipe!", 3);
					
				}
			}
			else
			{
				if (%col.getDatablock().matterSlots["Buffer"] <= 0 && %col.getDatablock().matterSlots["Output"] <= 0)
				{
					%client.chatMessage("\c6Target has no material output or buffer!", 3);
				}
				else if (getWordCount(%col.ropeGroups) >= $EOTW::MaxRopeCount)
				{
					%client.chatMessage("\c6Too many connected ropes! Max ropes: " @ $EOTW::MaxRopeCount, 3);
				}
				else
				{
					%obj.PipeLayerBuffer = %col;
					%obj.PipeLayerBufferPort = %col.getPortPosition("MatterOut",%hitpos);
					%client.chatMessage("\c6Pipe source set to " @ %col.getDatablock().uiName, 3);
				}
				
			}
		}
		else if (%col.getType() & $TypeMasks::StaticShapeObjectType)
		{
			if (isObject(%col.getGroup()) && %col.getGroup().material !$= "")
			{
				%col.getGroup().RemoveCableData();
				%client.chatMessage("\c6Pipe sucessfully removed.", 1);
			}
		}
	}
}

function PipeLayerImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
   
   if (!isObject(%client = %obj.client))
		return;
		
	if (%client.PipeLayerMat $= "")
		%client.PipeLayerMat = "Lead";
	
	%obj.PipeLayerMessage();
}
function PipeLayerImage::onUnMount(%this, %obj, %slot)
{
	Parent::onUnMount(%this, %obj, %slot);
   
	%obj.PipeLayerBuffer = "";
	cancel(%obj.PipeLayerMessageLoop);
}

function Player::PipeLayerMessage(%obj)
{
	cancel(%obj.PipeLayerMessageLoop);
	
	if (!isObject(%client = %obj.client))
		return;
		
	%pos = %obj.getEyePoint();
	%vector = vectorAdd(%pos, vectorScale(%obj.getEyeVector(), 5));
	%targets = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = ContainerRayCast(%pos, %vector, %targets, %obj);
	%col = getWord(%ray, 0);
	%hitpos = getWords(%ray, 1, 3);
	
	%PipeType = getMatterType(%client.PipeLayerMat);
	%matInv = ($EOTW::Material[%client.bl_id, %PipeType.name] + 0);
	
	%source = "--";
	%target = "--";
	%cost = "--";
		
	if (isObject(%obj.PipeLayerBuffer))
	{
		%source = %obj.PipeLayerBuffer.getDatablock().uiName;
		%cost = "--";
		if (isObject(%col) && (%col.getType() & $TypeMasks::FxBrickObjectType))
		{
			if (%col.getDatablock().energyGroup !$= "" && getTrustLevel(%obj, %col) >= $TrustLevel::Hammer)
			{
				if (%obj.PipeLayerBuffer == %col)
				{
					%target = "\c7" @ %col.getDatablock().uiName SPC "(Cancel)";
				}
				else
				{
					%cost = mCeil(vectorDist(%obj.PipeLayerBuffer.getPosition(), %col.getPosition()) * $EOTW::PipeCostMulti);
					
					if (%cost > $EOTW::Material[%client.bl_id, %PipeType.name])
						%matInv = "\c0" @ %matInv;
					else
						%matInv = "\c6" @ %matInv;
					
					%target = "\c6" @ %col.getDatablock().uiName;
				}
			}
			else
				%target = "\c0" @ %col.getDatablock().uiName;
		}
			
	}
	else if (isObject(%col))
	{
		if (%col.getType() & $TypeMasks::FxBrickObjectType)
		{
			if (%col.getDatablock().energyGroup !$= "" && getTrustLevel(%obj, %col) >= $TrustLevel::Hammer)
			{
				%source = "\c6" @ %col.getDatablock().uiName;
			}
			else
				%source = "\c0" @ %col.getDatablock().uiName;
		}
		else if (%col.getType() & $TypeMasks::StaticShapeObjectType)
		{
			%group = %col.getGroup();
			
			if (isObject(%group) && %group.material !$= "")
			{
				%matter = getMatterType(getField(%group.material, 0));
				%source = "\c6Matter Pipe (" @ getField(%group.material, 1) SPC "<color:" @ %matter.color @ ">" @ %matter.name @ "\c6)";
				%target = "\c7(Remove)";
			}
		}
	}
			
	%client.centerPrint("<just:left>\c6Source: " @ %source @ "<br>\c6Target: " @ %target @ "<br>\c6Material: " @ %matInv @ "\c6/" @ %cost @ " <color:" @ %PipeType.color @ ">" @ %PipeType.name @ "\c6 (" @ %PipeType.PipeTransfer @ "u/S)", 1);
		
	%obj.PipeLayerMessageLoop = %obj.schedule(100, "PipeLayerMessage");
}

package Tool_PipeLayer
{
	function Armor::onTrigger(%data, %obj, %trig, %tog)
	{
		if(isObject(%client = %obj.client))
		{
			if(%trig == 4 && %tog && isObject(%image = %obj.getMountedImage(0)) && %image.getName() $= "PipeLayerImage")
			{
				switch$(%client.PipeLayerMat)
				{
					case "Lead": %client.PipeLayerMat = "Rosium";
					case "Rosium": %client.PipeLayerMat = "Naturum";
					case "Naturum": %client.PipeLayerMat = "Lead";
					default: %client.PipeLayerMat = "Lead";
				}
			}
		}
		Parent::onTrigger(%data, %obj, %trig, %tog);
	}
};
activatePackage("Tool_PipeLayer");