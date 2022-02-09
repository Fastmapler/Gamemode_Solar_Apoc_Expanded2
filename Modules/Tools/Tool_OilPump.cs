datablock itemData(OilPumpItem)
{
	uiName = "Oil Pump";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.70 0.70 0.25 1.00";
	
	shapeFile = "base/data/shapes/printGun.dts";
	image = OilPumpImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(OilPumpImage)
{
	shapeFile = "base/data/shapes/printGun.dts";
	item = OilPumpItem;
	
	mountPoint = 0;
	offset = "0 0.25 0.15";
	rotation = eulerToMatrix("0 5 70");
	
	eyeOffset = "0.75 1.15 -0.24";
	eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = OilPumpItem.doColorShift;
	colorShiftColor = OilPumpItem.colorShiftColor;
	
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

function OilPumpImage::onFire(%this, %obj, %slot)
{
    if (!isObject(%client = %obj.client))
        return;

    %eye = %obj.getEyePoint();
    %dir = %obj.getEyeVector();
    %for = %obj.getForwardVector();
    %face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
    %mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
    %ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
    if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
    {
        if (%hit.getDataBlock().getName() $= "brickEOTWOilGeyserData" && %hit.OilCapacity > 0)
        {
            if(%hit.beingCollected > 0 && %hit.beingCollected != %client.bl_id)
                %cl.centerPrint("<color:FFFFFF>Someone is already collecting that material brick!", 3);
            else
            {
                %hit.lastGatherTick = getSimTime();
                %hit.beingCollected = %client.bl_id;
                %hit.cancelCollecting = %hit.schedule(10000, "cancelCollecting");
                %obj.collectOilLoop(%hit);
            }
        }
    }
}

function Player::collectOilLoop(%obj, %target)
{
	cancel(%obj.collectOilSchedule);
	
    if (!isObject(%client = %obj.client))
        return;

    %eye = %obj.getEyePoint();
    %dir = %obj.getEyeVector();
    %for = %obj.getForwardVector();
    %face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
    %mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
    %ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
    if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick" && %hit == %target)
    {
        %obj.collectOilSchedule = %obj.schedule(16, "collectOilLoop", %target);
    }
}