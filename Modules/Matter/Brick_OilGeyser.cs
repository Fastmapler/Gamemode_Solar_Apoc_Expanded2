datablock fxDTSBrickData(brickEOTWOilGeyserData)
{
	brickFile = "./Shapes/OilGeyser.blb";
	category = "Special";
	subCategory = "Custom";
	uiName = "Oil Geyser";
	//iconName = "";
};

function brickEOTWOilGeyserData::onPlant(%this, %obj)
{
	if (!isObject(OilGeyserSet))
		new SimSet(OilGeyserSet);

	%obj.OilCapacity = getRandom(16, 32) * 8;
	Gatherables.add(%obj);
	OilGeyserSet.add(%obj);
	%obj.despawnLife = getRandom(300, 500);

	Parent::onPlant(%this, %obj);
}

function SpawnOilGeyser(%eye)
{
	if (!isObject(OilGeyserSet))
		new SimSet(OilGeyserSet);

	if (OilGeyserSet.getCount() > mCeil(getMapArea() * $EOTW::MatterDensity / 100))
		return;

	if (%eye $= "")
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2))) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3))) SPC 1000;
		
	%dir = "0 0 -1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
	%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
	if(isObject(%hit = firstWord(%ray)) && (getWord(%pos, 2) > $EOTW::LavaHeight + 1))
	{
		if (%hit.getClassName() !$= "FxPlane" && strPos(%hit.getDatablock().uiName,"Ramp") > -1)
			%pos = vectorAdd(%pos,"0 0 0.4");
		
		%pos = vectorAdd(%pos,"0 0 0.2");

        %output = CreateBrick(EnvMaster, brickEOTWOilGeyserData, %pos, getColorFromHex(getMatterType("Crude Oil").color), getRandom(0, 3));

        if (getField(%output, 1) == 0)
        {
            %brick = getField(%output, 0);
        }
	}
	else if (getWord(%eye, 2) == 1000)
		SpawnOilGeyser(getWord(%eye, 0) SPC getWord(%eye, 1) SPC 500);
}
