function SetupMatterData()
{
	if (isObject(MatterData))
	{
		MatterData.deleteAll();
		MatterData.delete();
	}

	new SimSet(MatterData)
	{
		//Buildable Material
		new ScriptObject(MatterType) { name="Wood";			color="75502eff";	tier=1;	spawnWeight=300;	spawnVeinSize=6;	spawnValue=512;	collectTime=2000;	placable=true;	health=1.0;	heatCapacity=25;	meteorImmune=false;	gatherableDB="brickEOTWGatherableBasicData";	fuelCapacity=200/512; };
		new ScriptObject(MatterType) { name="Granite";		color="c1a872ff";	tier=1;	spawnWeight=400;	spawnVeinSize=4;	spawnValue=256;	collectTime=4000;	placable=true;	health=2.0;	heatCapacity=50;	meteorImmune=false;	gatherableDB="brickEOTWGatherableBasicData"; };
		new ScriptObject(MatterType) { name="Glass";		color="181d26a8";	tier=2;	spawnWeight=150;	spawnVeinSize=4;	spawnValue=64;	collectTime=8000;	placable=true;	health=3.0;	heatCapacity=50;	meteorImmune=true;	gatherableDB="brickEOTWGatherableCrystalData"; };
		new ScriptObject(MatterType) { name="Iron";			color="7a7a7aff";	tier=2;	spawnWeight=200;	spawnVeinSize=5;	spawnValue=128;	collectTime=12000;	placable=true;	health=4.0;	heatCapacity=60;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";	};
		new ScriptObject(MatterType) { name="Sturdium";		color="646defff";	tier=4;	spawnWeight=005;	spawnVeinSize=2;	spawnValue=40;	collectTime=24000;	placable=true;	health=999;	heatCapacity=90;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";	};
		//Growable Organics
		new ScriptObject(MatterType) { name="Moss";			color="75ba6dff";	tier=1;	spawnWeight=025;	spawnVeinSize=2;	spawnValue=4;	collectTime=1000;	gatherableDB="brickEOTWGatherableBasicData";	 };
		new ScriptObject(MatterType) { name="Vines";		color="226027ff";	tier=2;	spawnWeight=025;	spawnVeinSize=2;	spawnValue=16;	collectTime=1500;	gatherableDB="brickEOTWGatherableBasicData";	 };
		new ScriptObject(MatterType) { name="Cacti";		color="226027ff";	tier=2;	spawnWeight=025;	spawnVeinSize=2;	spawnValue=8;	collectTime=2000;	gatherableDB="brickEOTWGatherableBasicData";	 };
		//Basic Gatherable Materials
		new ScriptObject(MatterType) { name="Copper";		color="d36b04ff";	tier=3;	spawnWeight=100;	spawnVeinSize=4;	spawnValue=32;	collectTime=13000;	placable=true;	health=3.0;	heatCapacity=65;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";	cableTransfer=400;	 };
		new ScriptObject(MatterType) { name="Silver";		color="e0e0e0ff";	tier=3;	spawnWeight=075;	spawnVeinSize=4;	spawnValue=16;	collectTime=14000;	placable=true;	health=5.0;	heatCapacity=70;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";	 };
		new ScriptObject(MatterType) { name="Lead";			color="533d60ff";	tier=3;	spawnWeight=050;	spawnVeinSize=4;	spawnValue=48;	collectTime=15000;	placable=true;	health=7.0;	heatCapacity=55;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";	pipeTransfer=64; };
		new ScriptObject(MatterType) { name="Gold";			color="e2af14ff";	tier=4;	spawnWeight=030;	spawnVeinSize=3;	spawnValue=56;	collectTime=20000;	placable=true;	health=9.0;	heatCapacity=55;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";	 };
		new ScriptObject(MatterType) { name="Diamond";		color="00d0ffa8";	tier=4;	spawnWeight=010;	spawnVeinSize=2;	spawnValue=8;	collectTime=22000;	gatherableDB="brickEOTWGatherableCrystalData";	 };
		//Alloys
		new ScriptObject(MatterType) { name="Electrum";		color="dfc47cff";	tier=5;	cableTransfer=1600;	};
		new ScriptObject(MatterType) { name="Energium";		color="d69c6bff";	tier=6;	cableTransfer=6400;	};
		new ScriptObject(MatterType) { name="Rosium";		color="ca959eff";	tier=5;	pipeTransfer=256;	};
		new ScriptObject(MatterType) { name="Naturum";		color="83bc8cff";	tier=6;	pipeTransfer=1024;	};
		new ScriptObject(MatterType) { name="Steel";		color="2f2d2fff";	tier=4;	};
		new ScriptObject(MatterType) { name="Addy Base";	color="561f1cff";	tier=5;	};
		new ScriptObject(MatterType) { name="Adamantine";	color="bf1f21ff";	tier=6;	};
		//Other Organics
		new ScriptObject(MatterType) { name="Bio Fuel";		color="93690eff";	tier=3;	fuelCapacity=800/20;	};
		new ScriptObject(MatterType) { name="Gibs";			color="82281fff";	tier=2;	};
		new ScriptObject(MatterType) { name="Rubber";		color="18161aff";	tier=3;	};
		new ScriptObject(MatterType) { name="Leather";		color="503623ff";	tier=3;	};
		//Complex Gatherable Materials
		new ScriptObject(MatterType) { name="Coal";			color="000000ff";	tier=3;	spawnWeight=050;	spawnVeinSize=4;	spawnValue=96;	collectTime=10000;	fuelCapacity=800/96;	gatherableDB="brickEOTWGatherableBasicData";	};
		new ScriptObject(MatterType) { name="Crude Oil";	color="1c1108ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Fluorine";		color="1f568cff";	tier=4;	spawnWeight=015;	spawnVeinSize=4;	spawnValue=32;	collectTime=10000;	requiredCollectFuel=("Sulfur" TAB 16);	gatherableDB="brickEOTWGatherableCrystalData";	};
		//Chemicals
		new ScriptObject(MatterType) { name="Petroleum";	color="4f494bff";	tier=3;	fuelCapacity=1600/96;	};
		new ScriptObject(MatterType) { name="Sulfur";		color="93690eff";	tier=4;	};
		new ScriptObject(MatterType) { name="Ethanol";		color="953800ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Ethylene";		color="a5a189ff";	tier=4;	};
		new ScriptObject(MatterType) { name="Plastic";		color="797260ff";	tier=4;	};
		new ScriptObject(MatterType) { name="Teflon";		color="504b3fff";	tier=5;	};
		new ScriptObject(MatterType) { name="Rocket Fuel";	color="f8cfaaff";	tier=5;	fuelCapacity=3200/64;	};
		new ScriptObject(MatterType) { name="Dielectrics";	color="264b38ff";	tier=5;	};
		new ScriptObject(MatterType) { name="Plasteel";		color="ddb389ff";	tier=5;	placable=true;	health=4.0;	heatCapacity=50;	meteorImmune=true;	};
		//Water Based
		new ScriptObject(MatterType) { name="Water";		color="bcc1c88e";	tier=1;	boilCapacity=1;	};
		new ScriptObject(MatterType) { name="Oxygen";		color="bcc1c88e";	tier=2;	};
		new ScriptObject(MatterType) { name="Hydrogen";		color="bcc1c88e";	tier=2;	};
		new ScriptObject(MatterType) { name="Brine";		color="bcc1c88e";	tier=3;	};
		new ScriptObject(MatterType) { name="Lithium";		color="706e6eff";	tier=4;	};
		new ScriptObject(MatterType) { name="Sodium";		color="ffffffff";	tier=5;	};
		new ScriptObject(MatterType) { name="Tritium";		color="ffffffff";	tier=5;	};
		new ScriptObject(MatterType) { name="Deuterium";	color="ffffffff";	tier=5;	};
		new ScriptObject(MatterType) { name="Coolant";		color="9ab6b5ff";	tier=3;	boilCapacity=10;		};
		new ScriptObject(MatterType) { name="Cryostablizer";color="89a3b8ff";	tier=5;	boilCapacity=50;		};
		//Heated Coolants
		new ScriptObject(MatterType) { name="Steam";		color="bcc1c88e";	tier=1;	};
		new ScriptObject(MatterType) { name="Hot Coolant";	color="9ab6b5ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Hot Cryostablizer";	color="89a3b8ff";	tier=5;	};
		//Nuclear //Fission Power: Amount of heat units created per unboosted unit. //fissionWasteRate: Amount of nuclear waste produced per unit of fuel consumed
		new ScriptObject(MatterType) { name="Raw Uranium";		color="007c3fff";	tier=4;	spawnWeight=15;	spawnVeinSize=2;	spawnValue=64;	collectTime=18000;	requiredCollectFuel=("Sulfur" TAB 32);	gatherableDB="brickEOTWGatherableCrystalData";	};
		new ScriptObject(MatterType) { name="Raw Thorium";		color="007c3fff";	tier=4;	};
		new ScriptObject(MatterType) { name="Fissile Uranium";		color="56643bff";	tier=6;	fuelCapacity=3800/128;	fissionPower=40;  fissionWasteRate=1;	};
		new ScriptObject(MatterType) { name="Fissile Thorium";		color="56643bff";	tier=6;	fuelCapacity=4800/128;	fissionPower=80;  fissionWasteRate=0.5;	};
		new ScriptObject(MatterType) { name="Fissile Americium";	color="56643bff";	tier=7;	fuelCapacity=5800/128;	fissionPower=320; fissionWasteRate=4;	};
		new ScriptObject(MatterType) { name="Fissile Curium";		color="56643bff";	tier=7;	fuelCapacity=6800/128;	fissionPower=1600;fissionWasteRate=8;	};
		new ScriptObject(MatterType) { name="Nuclear Waste";color="605042ff";	tier=3;	};
		new ScriptObject(MatterType) { name="Plutonium";	color="d8d1ccff";	tier=6;	};
		//Potions
		new ScriptObject(MatterType) { name="Healing Mix";	color="bcc1c88e";	tier=4;	isMix=true;	};
		new ScriptObject(MatterType) { name="Steroid Mix";	color="bcc1c88e";	tier=4;	isMix=true;	};
		new ScriptObject(MatterType) { name="Adrenline Mix";color="bcc1c88e";	tier=4;	isMix=true;	};
		new ScriptObject(MatterType) { name="Gatherer Mix";	color="bcc1c88e";	tier=5;	isMix=true;	};
		new ScriptObject(MatterType) { name="Overload Mix";	color="bcc1c88e";	tier=5;	isMix=true;	};
		new ScriptObject(MatterType) { name="Leatherskin Mix";		color="bcc1c88e";	tier=5;	isMix=true;	};
		//Exotic
		new ScriptObject(MatterType) { name="Infinity";		color="3d5472ff";	tier=7;	cableTransfer=999999;	pipeTransfer=999999;	};
		new ScriptObject(MatterType) { name="Singularity";	color="ffffff00";	tier=7;	};
		new ScriptObject(MatterType) { name="Scrip";		color="507582ff";	tier=7;	};
		new ScriptObject(MatterType) { name="Energy";		color="00d0ffa8";	tier=1;	};
	};
	
	$EOTW::PlacableList = "";
	for (%i = 0; %i < MatterData.getCount(); %i++)
		if (MatterData.getObject(%i).placable)
			$EOTW::PlacableList = $EOTW::PlacableList TAB MatterData.getObject(%i).name;
	$EOTW::PlacableList = trim($EOTW::PlacableList);
	
	$EOTW::MatSpawnWeight = 0;
	$EOTW::MatSpawnList = "";
	for (%i = 0; %i < MatterData.getCount(); %i++)
	{
		if (MatterData.getObject(%i).spawnWeight > 0)
		{
			$EOTW::MatSpawnList = $EOTW::MatSpawnList TAB MatterData.getObject(%i).name;
			$EOTW::MatSpawnWeight += MatterData.getObject(%i).spawnWeight;
		}
	}
	$EOTW::MatSpawnList = trim($EOTW::MatSpawnList);

	schedule(10, 0, "EOTWbsm_PopulateRecipesMenu");
}
SetupMatterData();