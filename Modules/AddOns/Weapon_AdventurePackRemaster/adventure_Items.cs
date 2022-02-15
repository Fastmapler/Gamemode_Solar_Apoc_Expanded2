//
// ammo items for the adventurer weapon pack
//

datablock ItemData(advAmmoItem : hammerItem)
{
	shapeFile = "./shapes/items/AMMO_GROUP.dts";
	uiName = "Ammo [ALL]";
	iconName = "";
	image = "";
	doColorShift = true;
	colorShiftColor = "0.471 0.471 0.471 1.000";
	canPickUp = true;
	ammoBox = true;
	ammoType = "ALL";
};

datablock ItemData(advAmmoPistolItem : advAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_9MM.dts";
	uiName = "Ammo [Pistol]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Pistol";
};

datablock ItemData(advAmmoMachinePistolItem : advAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_9MM.dts";
	uiName = "Ammo [Machine Pistol]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Machine Pistol";
};

datablock ItemData(advAmmoRevolverItem : advAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_MAGNUM.dts";
	uiName = "Ammo [Revolver]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Revolver";
};
datablock ItemData(advAmmoRifleItem : advAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_9MM.dts";
	uiName = "Ammo [Rifle]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Rifle";
};

datablock ItemData(advAmmoRifleNadeItem : HammerItem)
{
	shapeFile = "./shapes/items/AMMO_708.dts";
	uiName = "Ammo [R. Grenade]";
	iconName = "";
	image = "";
	canPickUp = true;
	ammoBox = true;
	altAmmoType = "riflenade";
	altAmmoAdd = 1;
};

datablock ItemData(advAmmoSniperRifleItem : advAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_BOLT.dts";
	uiName = "Ammo [Sniper Rifle]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Sniper Rifle";
};

datablock ItemData(advAmmoMachineRifleItem : advAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_556.dts";
	uiName = "Ammo [M. Rifle]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Machine Rifle";
};

datablock ItemData(advAmmoShotgunItem : advAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_SHOTGUN.dts";
	uiName = "Ammo [Shotgun]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Shotgun";
};

datablock ItemData(advAmmoHMGItem : warAmmoItem)
{
	shapeFile = "./shapes/items/AMMO_708.dts";
	uiName = "Ammo [Heavy Machine Gun]";
	iconName = "";
	image = "";
	ammoBox = true;
	ammoType = "Heavy Machine Gun";
};