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
	%obj.OilCapacity = getRandom(64, 128) * 2;
	
	Parent::onPlant(%this, %obj);
}

function SpawnOilGeyser(%eye)
{
	if (%eye $= "")
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2))) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3))) SPC 1000;
		
	%dir = "0 0 -1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
	%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
	if(isObject(%hit = firstWord(%ray)))
	{
		if (%hit.getClassName() !$= "FxPlane" && strPos(%hit.getDatablock().uiName,"Ramp") > -1)
			%pos = vectorAdd(%pos,"0 0 0.4");
		
        %output = CreateBrick(EnvMaster, brickEOTWOilGeyserData, %pos, getColorFromHex(getMatterType("Crude Oil").color), getRandom(0, 3));

        if (getField(%output, 1) == 0)
        {
            %brick = getField(%output, 0);
        }
	}
	else if (getWord(%eye, 2) == 1000)
		SpawnOilGeyser(getWord(%eye, 0) SPC getWord(%eye, 1) SPC 500);
}
