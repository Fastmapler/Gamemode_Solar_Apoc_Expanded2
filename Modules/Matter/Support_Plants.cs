function PlantLife_TickLoop()
{
    if (!isObject(EOTWPlants) || EOTWPlants.getCount() < 1)
        return;

    %totalTicks = 0;
    for (%i = EOTWPlants.getCount() / 100; %i > 0; %i--)
        if (getRandom() < %i)
            %totalTicks++;

    for (%i = 0; %i < %totalTicks; %i++)
        EOTWPlants.getObject(getRandom(0, EOTWPlants.getCount() - 1)).EOTW_PlantLifeTick();
}

function fxDtsBrick::EOTW_PlantLifeTick(%obj)
{
    //Vines will attempt to grow along walls
    //Moss will attempt to grow along floors and ceilings
    //Cacti (if implemented) will grow only grow upwards, up to a stack limit. Will not grow if horizontally adjacent to another cane block
    //Both will only grow on blocks owned by the same bl_id
    //bl_id 888888 and 1337 are blacklisted completely
}