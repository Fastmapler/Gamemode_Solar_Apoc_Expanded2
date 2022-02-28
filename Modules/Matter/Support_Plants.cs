function PlantLife_TickLoop()
{
    cancel($EOTW::PlantLifeLoop);
    if (isObject(EOTWPlants) && EOTWPlants.getCount() >= 1)
    {
        %totalTicks = 0;
        for (%i = EOTWPlants.getCount() / 100; %i > 0; %i--)
            if (getRandom() < %i)
                %totalTicks++;

        for (%i = 0; %i < %totalTicks; %i++)
            EOTWPlants.getObject(getRandom(0, EOTWPlants.getCount() - 1)).EOTW_PlantLifeTick();
    }
    $EOTW::PlantLifeLoop = schedule(1000, 0, "PlantLife_TickLoop");
}
$EOTW::PlantLifeLoop = schedule(1000, 0, "PlantLife_TickLoop");

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
        %angleID = getRandom(0,5);

        switch (%angleID)
        {
                case 0: %dir = "0.5 0 0";
                case 1: %dir = "-0.5 0 0";
                case 2: %dir = "0 0.5 0";
                case 3: %dir = "0 -0.5 0";
                case 4: %dir = "0 0 0.2";
                case 5: %dir = "0 0 -0.2";
            default: %dir = "0 0 0.5";
        }

        %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), %dir), %obj.getColorID(), %angleID);
        %brick = getField(%output, 0);
        %err = getField(%output, 1);
        if (isObject(%brick))
        {
            %brick.Material = "Custom";

            //check to see if there is a non vine brick of the same brick group around
            %supportingBrick = false;
            %next = containerFindFirst($TypeMasks::fxBrickObjectType, %brick.getPosition(), 0.5, 0.5, 0.2);
            while(%next !$= "")
            {
                
                %relPos = vectorSub(%brick.getPosition(),vectorSub(%next.getPosition(),"0 0" SPC %next.getDataBlock().brickSizeZ / 10));
                if(%next.getGroup() == %brick.getGroup() && !%next.getDatablock().isPlantBrick && !(getWord(%relPos,2) < 0))
                {
                    %supportingBrick = true;
                    break;
                }
                %next = containerFindNext();
            }
            if (!%supportingBrick || (%err > 0 && %err != 2))
            {
                %brick.dontRefund = true;
                %brick.delete();
            }
        }
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

        %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), %dir), %obj.getColorID(), %angleID);
        %brick = getField(%output, 0);
        %err = getField(%output, 1);
        if (isObject(%brick))
        {
            %downBrick = %brick.getDownBrick(0);
            %upBrick = %brick.getUpBrick(0);
            %brick.Material = "Custom";

            if (%err > 0 || (!isObject(%downBrick) && !isObject(%upBrick)) || (isObject(%downBrick) && %downBrick.getGroup() != %brick.getGroup()) || isObject(%upBrick) && %upBrick.getGroup() != %brick.getGroup())
            {
                %brick.dontRefund = true;
                %brick.delete();
            }
        }
        
    }
    else if (%data.getName() $= "brickEOTWCactiData")
    {
        if (isObject(%obj.getUpBrick(0)) && !%obj.isplanted)
            return;

        if(%obj.length > 16 || isObject(%obj.split1) || isObject(%obj.split2))
        {
            return;
        }

        if(getRandom() < 0.1 && !%obj.straight)
        {
            %angleID = getRandom(0, 1);

            switch (%angleID)
            {
                case 0: %dir = "0.5 0 0";
                        %dir2 = "-0.5 0 0";
                case 1: %dir = "0 0.5 0";
                        %dir2 = "0 -0.5 0";
            }
            %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), %dir), %obj.getColorID(), %angleID);
            %brick = getField(%output, 0);
            %err = getField(%output, 1);
            if (isObject(%brick))
            {
                if (%err > 0 && %err != 2)
                {
                    %brick.dontRefund = true;
                    %brick.delete();
                }
                else
                {
                    %brick.Material = "Custom";
                    %brick.length = %obj.length + 1;
                    %obj.split1 = %brick;
                }
            }
            %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), %dir2), %obj.getColorID(), %angleID);
            %brick = getField(%output, 0);
            %err = getField(%output, 1);
            if (isObject(%brick))
            {
                if (%err > 0 && %err != 2)
                {
                    %brick.dontRefund = true;
                    %brick.delete();
                }
                else
                {
                    %brick.Material = "Custom";
                    %brick.length = %obj.length + 1;
                    %obj.split2 = %brick;
                }
            }
        }
        else
        {
            %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), "0 0 0.2"), %obj.getColorID(), %angleID);
            %brick = getField(%output, 0);
            %err = getField(%output, 1);
            if (isObject(%brick))
            {
                if (%err > 0)
                {
                    %brick.dontRefund = true;
                    %brick.delete();
                }
                else
                {
                    %brick.Material = "Custom";
                    %brick.length = %obj.length + 1;
                    %obj.straight = true;
                }
            }
        }
        
    }
    else if (%data.getName() $= "brickEOTWDeadPlantData")
    {
        %obj.energy--;
        if (%obj.energy < -10)
        {
            %obj.dontRefund = true;
            %obj.killBrick();
        }
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
$EOTW::CustomBrickCost["brickEOTWDeadPlantData"] = (1/16) TAB "75502eff" TAB 16 TAB "Wood";

package EOTW_Plants
{
    function fxDtsBrick::onPlant(%obj,%b)
	{
		parent::onPlant(%obj,%b);

        if (%obj.getDatablock().isPlantBrick)
        {
            if (!isObject(EOTWPlants))
            {
                new SimSet(EOTWPlants);
                PlantLife_TickLoop();
            }
            EOTWPlants.add(%obj);
        }
	}

	function fxDtsBrick::onLoadPlant(%obj,%b)
	{
		parent::onLoadPlant(%obj,%b);

        if (%obj.getDatablock().isPlantBrick)
        {
            if (!isObject(EOTWPlants))
            {
                new SimSet(EOTWPlants);
                PlantLife_TickLoop();
            }
            EOTWPlants.add(%obj);
        }
	}
};
activatePackage("EOTW_Plants");