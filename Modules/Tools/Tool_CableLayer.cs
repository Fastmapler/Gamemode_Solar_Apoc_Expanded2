$EOTW::CableSizeLimit = 16;
$EOTW::MaxRopeCount = 4;
$EOTW::CableCostMulti = 1;

datablock itemData(CableLayerItem)
{
	uiName = "Cable Layer";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.25 0.25 0.70 1.00";
	
	shapeFile = "base/data/shapes/printGun.dts";
	image = CableLayerImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(CableLayerImage)
{
	shapeFile = "base/data/shapes/printGun.dts";
	item = CableLayerItem;
	
	mountPoint = 0;
	offset = "0 0.25 0.15";
	rotation = eulerToMatrix("0 5 70");
	
	eyeOffset = "0.75 1.15 -0.24";
	eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = CableLayerItem.doColorShift;
	colorShiftColor = CableLayerItem.colorShiftColor;
	
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

function CableLayerImage::onFire(%this, %obj, %slot)
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
		if (%col.getType() & $TypeMasks::FxBrickObjectType)
		{
			if (%col.getDatablock().energyGroup !$= "")
			{
				if (%obj.cableLayerBuffer !$= "")
				{
					if (%obj.cableLayerBuffer == %col)
					{
						%obj.cableLayerBuffer = "";
						%client.chatMessage("\c6Cancelled cable.", 3);
					}
					else if (%col.getDatablock().energyGroup $= "Source")
					{
						%client.chatMessage("\c6Power sources can't import power!", 3);
					}
					else if (getWordCount(%col.ropeGroups) >= $EOTW::MaxRopeCount)
					{
						%client.chatMessage("\c6Too many connected ropes! Max ropes: " @ $EOTW::MaxRopeCount, 3);
					}
					else
					{
						%cost = mCeil(vectorDist(%obj.cableLayerBuffer.getPosition(), %col.getPosition()) * $EOTW::CableCostMulti);
						%cableType = getMatterType(%client.CableLayerMat);
						%matInv = ($EOTW::Material[%client.bl_id, %cableType.name] + 0);
						
						for (%i = 0; %i < getWordCount(%obj.cableLayerBuffer.ropeGroups); %i++)
						{
							%group = getWord(%obj.cableLayerBuffer.ropeGroups, %i);
							if (getWord(%group.creationData, 0) == %obj.cableLayerBuffer.getID() && getWord(%group.creationData, 1) == %col.getID())
							{
								%client.chatMessage("\c6A cable already exists in this direction!", 3);
								return;
							}
							else if (getWord(%group.creationData, 1) == %obj.cableLayerBuffer.getID() && getWord(%group.creationData, 0) == %col.getID())
							{
								%client.chatMessage("\c6A cable already exists in this direction!", 3);
								return;
							}
						}
						
						if (%cost > $EOTW::CableSizeLimit)
						{
							%client.chatMessage("\c6Cable is too long! Max Cost: " @ $EOTW::CableSizeLimit, 3);
						}
						else if (%matInv >= %cost)
						{
							$EOTW::Material[%client.bl_id, %cableType.name] -= %cost;
							
							if (%cableType.cableTransfer > 0)
								%transferRate = %cableType.cableTransfer / $EOTW::PowerTickRate;
							else
								%transferRate = 2;
								
							%client.chatMessage("\c6Cable target set to " @ %col.getDatablock().uiName @ ". Cable created!", 3);
							CreateTransferRope(%obj.cableLayerBuffer, %obj.cableLayerBufferPort, %col,%col.getPortPosition("PowerIn",%hitpos), %transferRate, %cableType.name, %cost, "Power");
							%obj.cableLayerBuffer = "";
						}
						else
							%client.chatMessage("\c6Not enough material to build cable!", 3);
						
					}
				}
				else
				{
					if (%col.getDatablock().energyGroup $= "Machine")
					{
						%client.chatMessage("\c6Working machines can't export power!", 3);
					}
					else if (getWordCount(%col.ropeGroups) >= $EOTW::MaxRopeCount)
					{
						%client.chatMessage("\c6Too many connected ropes! Max ropes: " @ $EOTW::MaxRopeCount, 3);
					}
					else
					{
						%obj.cableLayerBuffer = %col;
						%obj.cableLayerBufferPort = %col.getPortPosition("PowerOut",%hitpos);
						%client.chatMessage("\c6Cable source set to " @ %col.getDatablock().uiName, 3);
					}
					
				}
			}
			else
				%client.chatMessage("\c6No power found.", 1);
		}
		else if (%col.getType() & $TypeMasks::StaticShapeObjectType)
		{
			if (isObject(%col.getGroup()) && %col.getGroup().material !$= "")
			{
				%col.getGroup().RemoveCableData();
				%client.chatMessage("\c6Cable sucessfully removed.", 1);
			}
		}
	}
}

function CableLayerImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
   
   if (!isObject(%client = %obj.client))
		return;
		
	if (%client.CableLayerMat $= "")
		%client.CableLayerMat = "Copper";
	
	%obj.CableLayerMessage();
}
function CableLayerImage::onUnMount(%this, %obj, %slot)
{
	Parent::onUnMount(%this, %obj, %slot);
   
	%obj.cableLayerBuffer = "";
	cancel(%obj.CableLayerMessageLoop);
}

function Player::CableLayerMessage(%obj)
{
	cancel(%obj.CableLayerMessageLoop);
	
	if (!isObject(%client = %obj.client))
		return;
		
	%pos = %obj.getEyePoint();
	%vector = vectorAdd(%pos, vectorScale(%obj.getEyeVector(), 5));
	%targets = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = ContainerRayCast(%pos, %vector, %targets, %obj);
	%col = getWord(%ray, 0);
	%hitpos = getWords(%ray, 1, 3);
	
	%cableType = getMatterType(%client.CableLayerMat);
	%matInv = ($EOTW::Material[%client.bl_id, %cableType.name] + 0);
	
	%source = "--";
	%target = "--";
	%cost = "--";
		
	if (isObject(%obj.cableLayerBuffer))
	{
		%source = %obj.cableLayerBuffer.getDatablock().uiName SPC "(" @ %obj.cableLayerBuffer.energy @ "/" @ %obj.cableLayerBuffer.getDatablock().energyMaxBuffer @ ")";
		%cost = "--";
		if (isObject(%col) && (%col.getType() & $TypeMasks::FxBrickObjectType))
		{
			if (%col.getDatablock().energyGroup !$= "")
			{
				if (%obj.cableLayerBuffer == %col)
				{
					%target = "\c7" @ %col.getDatablock().uiName SPC "(Cancel)";
				}
				else
				{
					%cost = mCeil(vectorDist(%obj.cableLayerBuffer.getPosition(), %col.getPosition()) * $EOTW::CableCostMulti);
					
					if (%cost > $EOTW::Material[%client.bl_id, %cableType.name])
						%matInv = "\c0" @ %matInv;
					else
						%matInv = "\c6" @ %matInv;
					
					%target = "\c6" @ %col.getDatablock().uiName SPC "(" @ %col.energy @ "/" @ %col.getDatablock().energyMaxBuffer @ ")";
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
			if (%col.getDatablock().energyGroup !$= "")
			{
				%source = "\c6" @ %col.getDatablock().uiName SPC "(" @ %col.energy @ "/" @ %col.getDatablock().energyMaxBuffer @ ")";
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
				%source = "\c6Power Cable (" @ getField(%group.material, 1) SPC "<color:" @ %matter.color @ ">" @ %matter.name @ "\c6)";
				%target = "\c7(Remove)";
			}
		}	
	}
			
	%client.centerPrint("<just:left>\c6Source: " @ %source @ "<br>\c6Target: " @ %target @ "<br>\c6Material: " @ %matInv @ "\c6/" @ %cost @ " <color:" @ %cableType.color @ ">" @ %cableType.name @ "\c6 (" @ %cableType.cableTransfer @ "W)", 1);
		
	%obj.CableLayerMessageLoop = %obj.schedule(100, "CableLayerMessage");
}

package Tool_CableLayer
{
	function Armor::onTrigger(%data, %obj, %trig, %tog)
	{
		if(isObject(%client = %obj.client))
		{
			if(%trig == 4 && %tog && isObject(%image = %obj.getMountedImage(0)) && %image.getName() $= "CableLayerImage")
			{
				switch$(%client.CableLayerMat)
				{
					case "Copper": %client.CableLayerMat = "Electrum";
					case "Electrum": %client.CableLayerMat = "Energium";
					case "Energium": %client.CableLayerMat = "Copper";
					default: %client.CableLayerMat = "Copper";
				}
			}
		}
		Parent::onTrigger(%data, %obj, %trig, %tog);
	}
};
activatePackage("Tool_CableLayer");