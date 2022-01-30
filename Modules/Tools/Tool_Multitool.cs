datablock itemData(MultitoolItem)
{
	uiName = "Multitool";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.25 0.70 0.25 1.00";
	
	shapeFile = "base/data/shapes/printGun.dts";
	image = MultitoolImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(MultitoolImage)
{
	shapeFile = "base/data/shapes/printGun.dts";
	item = MultitoolItem;
	
	mountPoint = 0;
	offset = "0 0.25 0.15";
	rotation = eulerToMatrix("0 5 70");
	
	eyeOffset = "0.75 1.15 -0.24";
	eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = MultitoolItem.doColorShift;
	colorShiftColor = MultitoolItem.colorShiftColor;
	
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

function MultitoolImage::onFire(%this, %obj, %slot)
{
	if(!isObject(%client = %obj.client))
		return;
	
	%pos = %obj.getEyePoint();
	%vector = vectorAdd(%pos, vectorScale(%obj.getEyeVector(), 1000));
	%targets = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = ContainerRayCast(%pos, %vector, %targets, %obj);
	%col = getWord(%ray, 0);
	%hitpos = getWords(%ray, 1, 3);
	
	if (isObject(%col) && (%col.getType() & $TypeMasks::FxBrickObjectType))
	{
		
	}
}

function MultitoolImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
   
   if (!isObject(%client = %obj.client))
		return;
	
	%obj.MultitoolMessage();
}

function MultitoolImage::onUnMount(%this, %obj, %slot)
{
	Parent::onUnMount(%this, %obj, %slot);
	
	cancel(%obj.MultitoolMessageLoop);
   
}

function Player::MultitoolMessage(%obj)
{
	cancel(%obj.MultitoolMessageLoop);
	
	if (!isObject(%client = %obj.client))
		return;
		
	%pos = %obj.getEyePoint();
	%vector = vectorAdd(%pos, vectorScale(%obj.getEyeVector(), 1000));
	%targets = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = ContainerRayCast(%pos, %vector, %targets, %obj);
	%col = getWord(%ray, 0);
	%hitpos = getWords(%ray, 1, 3);
	
	%target = "--";
	%power = "--";
	
	if (isObject(%col) && (%col.getType() & $TypeMasks::FxBrickObjectType))
	{
		if (%col.getDatablock().energyGroup !$= "")
		{
			%db = %col.getDatablock();
			%target = %db.uiName;
			%power = (%col.energy + 0) @ "/" @ %db.energyMaxBuffer;
		}
	}
	
	%client.centerPrint("<just:left>\c6Machine: " @ %target @ "<br>\c6Energy: " @ %power, 1);
		
	%obj.MultitoolMessageLoop = %obj.schedule(100, "MultitoolMessage");
}
