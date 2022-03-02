$EOTW::ItemCrafting["MiningScannerItem"] = (56 TAB "Gold") TAB (16 TAB "Silver");
$EOTW::ItemDescription["MiningScannerItem"] = "Gives nearby materials a glow effect for a short time. Requires 100 EU per use. Charge at a charge pad.";
datablock itemData(MiningScannerItem)
{
	uiName = "TLS - Mining Scanner";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.25 0.70 0.70 1.00";
	
	shapeFile = "base/data/shapes/printGun.dts";
	image = MiningScannerImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(MiningScannerImage)
{
	shapeFile = "base/data/shapes/printGun.dts";
	item = MiningScannerItem;
	
	mountPoint = 0;
	offset = "0 0.25 0.15";
	rotation = eulerToMatrix("0 5 70");
	
	eyeOffset = "0.75 1.15 -0.24";
	eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = MiningScannerItem.doColorShift;
	colorShiftColor = MiningScannerItem.colorShiftColor;

	printPlayerBattery = true;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 1.0;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 1.0;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "checkFire";
	
	stateName[3]					= "checkFire";
	stateTransitionOnTriggerUp[3] 	= "Ready";
};

function MiningScannerImage::onFire(%this, %obj, %slot)
{
	if (!isObject(%client = %obj.client))
        return;

	%energyCost = 100;
	if (%obj.GetBatteryEnergy() >= %energyCost)
	{
		%obj.ChangeBatteryEnergy(%energyCost * -1);
		%obj.MiningScannerPing(64, 5000);
	}
	else
	{
		%client.chatMessage("\c6You need atleast " @ %energyCost @ " EU per use! Charge at a charge pad brick.");
	}
}

function Player::MiningScannerPing(%obj, %range, %time)
{
    initContainerRadiusSearch(%obj.getPosition(), %range, $TypeMasks::FxBrickAlwaysObjectType);
    while(isObject(%hit = containerSearchNext()))
        if (%hit.isCollectable)
            %hit.schedule(vectorDist(%obj.getPosition(), %hit.getPosition()) * 25, "TempColorFX", 3, %time);
}

function fxDtsBrick::TempColorFX(%obj, %fx, %time)
{
    if (%obj.TempColorFxSchedule !$= "")
        return;

    %obj.TempColorFxSchedule = %obj.schedule(%time, "TempColorFxEnd", %obj.getColorFXID());
    %obj.setColorFx(%fx);
}

function fxDtsBrick::TempColorFxEnd(%obj, %fx)
{
        %obj.TempColorFxSchedule = "";
        %obj.setColorFx(%fx);
}