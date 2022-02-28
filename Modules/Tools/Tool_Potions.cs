//Healing
$EOTW::ItemCrafting["mixFlaskHealingItem"] = (32 TAB "Healing Mix") TAB (16 TAB "Glass");
datablock ItemData(mixFlaskHealingItem)
{
    category = "Weapon";
    className = "Weapon";

    shapeFile = "./Shapes/Potion.dts";
    rotate = false;
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    uiName = "MIX - Flask Healing";
    //iconName = "./potion";
    doColorShift = true;
    colorShiftColor = "0.85 0.40 0.40 1.00";

    image = mixFlaskHealingImage;
    canDrop = true;
};

datablock ShapeBaseImageData(mixFlaskHealingImage)
{
    shapeFile = "./Shapes/Potion.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    eyeOffset = 0;
    rotation = eulerToMatrix( "0 0 0" );

    className = "WeaponImage";
    item = mixFlaskHealingItem;

    armReady = true;

    doColorShift = mixFlaskHealingItem.doColorShift;
    colorShiftColor = mixFlaskHealingItem.colorShiftColor;

    stateName[0]                     = "Activate";
    stateTimeoutValue[0]             = 0.15;
    stateTransitionOnTimeout[0]      = "Ready";

    stateName[1]                     = "Ready";
    stateTransitionOnTriggerDown[1]  = "Fire";
    stateAllowImageChange[1]         = true;

    stateName[2]                     = "Fire";
    stateTransitionOnTimeout[2]      = "Ready";
    stateAllowImageChange[2]         = true;
    stateScript[2]                   = "onFire";
    stateTimeoutValue[2]		     = 1.0;
};

function mixFlaskHealingImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskHealing();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskHealing(%obj, %tick)
{
    if (%tick >= 120)
        return;

    %obj.addHealth(2);
    %obj.PotionSchedule["PotionTick_FlaskHealing"] = %obj.schedule(1000, "PotionTick_FlaskHealing", %tick + 1);
}

//Steroids
$EOTW::ItemCrafting["mixFlaskSteroidItem"] = (32 TAB "Steroid Mix") TAB (16 TAB "Glass");
datablock ItemData(mixFlaskSteroidItem : mixFlaskHealingItem)
{
    uiName = "MIX - Flask Steroids";
    colorShiftColor = "0.25 0.00 0.00 1.00";
    image = mixFlaskSteroidImage;
};

datablock ShapeBaseImageData(mixFlaskSteroidImage : mixFlaskHealingImage)
{
    item = mixFlaskSteroidItem;
    doColorShift = mixFlaskSteroidItem.doColorShift;
    colorShiftColor = mixFlaskSteroidItem.colorShiftColor;
};

function mixFlaskSteroidImage::onFire(%this,%obj,%slot)
{
    return;
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskSteroid();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskSteroid(%obj, %tick)
{
    if (%tick >= 60)
    {
        %obj.steroidlevel--;
        return;
    }
    else if (%tick == 0)
    {
        %obj.steroidlevel++;
    }

    %obj.PotionSchedule["PotionTick_FlaskSteroid"] = %obj.schedule(1000, "PotionTick_FlaskSteroid", %tick + 1);
}