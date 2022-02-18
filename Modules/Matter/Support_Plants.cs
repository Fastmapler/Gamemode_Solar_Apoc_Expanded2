function PlantLife_TickLoop()
{
    cancel($EOTW::PlantLifeLoop);

    if (!isObject(EOTWPlants) || EOTWPlants.getCount() < 1)
        return;

    %totalTicks = 0;
    for (%i = EOTWPlants.getCount() / 100; %i > 0; %i--)
        if (getRandom() < %i)
            %totalTicks++;

    for (%i = 0; %i < %totalTicks; %i++)
        EOTWPlants.getObject(getRandom(0, EOTWPlants.getCount() - 1)).EOTW_PlantLifeTick();

    $EOTW::PlantLifeLoop = schedule(1000, EOTWPlants, "PlantLife_TickLoop");
}
$EOTW::PlantLifeLoop = schedule(1000, EOTWPlants, "PlantLife_TickLoop");

function fxDtsBrick::EOTW_PlantLifeTick(%obj)
{
    //Vines will attempt to grow along walls
    //Moss will attempt to grow along floors and ceilings. Will not stack vertically.
    //Cacti will grow only grow upwards, up to a stack limit. Will not grow if horizontally adjacent to another cane block
    //Both will only grow on blocks owned by the same bl_id
    //bl_id 888888 and 1337 are blacklisted completely

    if (!isObject(%client = %obj.getGroup().client))
        return;

    %data = %obj.getDatablock();

    if (%data.getName() $= "brickEOTWVinesData")
    {

    }
    else if (%data.getName() $= "brickEOTWMossData")
    {
        %angleID = getRandom(0, 3);

        switch (%angleID)
        {
             case 0: %dir = "0.5 0 0";
             case 1: %dir = "-0.5 0 0";
             case 2: %dir = "0 0.5 0";
             case 3: %dir = "0 -0.5 0";
            default: %dir = "0 0 0.5";
        }

        %brick = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), %dir), %obj.getColorID(), %angleID);
        if (isObject(%brick) && !isObject(%brick.getDownBrick(0)) && !isObject(%brick.getUpBrick(0)))
            %brick.setDataBlock(brickEOTWDeadPlantData);
    }
    else if (%data.getName() $= "brickEOTWCactiData")
    {
        %angleID = getRandom(0, 3);

        %brick = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), "0 0 0.2"), %obj.getColorID(), %angleID);
        if (isObject(%brick) && isObject(%brick.getDownBrick(15)) && %brick.getDownBrick(15).getDataBlock() == %data)
            %brick.setDataBlock(brickEOTWDeadPlantData);
    }
    else if (%data.getName() $= "brickEOTWDeadPlantData")
    {
        %obj.energy--;
        if (%obj.energy < -10)
            %obj.killBrick();
    }
}

datablock fxDTSBrickData (brickEOTWVinesData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Vines";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWVinesData"] = 0.25 TAB "226027ff" TAB 16 TAB "Vines";

datablock fxDTSBrickData (brickEOTWMossData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Moss";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWMossData"] = (1/4) TAB "75ba6dff" TAB 4 TAB "Moss";

datablock fxDTSBrickData (brickEOTWCactiData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Cacti";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWCactiData"] = (1/8) TAB "226027ff" TAB 8 TAB "Cacti";

datablock fxDTSBrickData (brickEOTWDeadPlantData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Dead Plant";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWCactiData"] = (1/16) TAB "75502eff" TAB 16 TAB "Wood";