function Player::GetBatteryText(%obj)
{
    if (!isObject(%client = %obj.client))
            return;

    %text = "\c6[\c4";

    %i = 0.05;

    while (%i <= (%obj.GetBatteryEnergy() / %obj.GetMaxBatteryEnergy()))
    {
        %text = %text @ "|";
        %i += 0.05;
    }

    %text = %text @ "\c0";

    while (%i <= 1.0)
    {
        %text = %text @ "|";
        %i += 0.05;
    }

    %text = %text @ "\c6] (" @ %obj.GetBatteryEnergy() @ "/" @ %obj.GetMaxBatteryEnergy() @ " EU)";

    return %text;
}

function Player::GetBatteryEnergy(%obj)
{
    if (%obj.BatteryEnergy $= "" || %obj.BatteryEnergy < 0)
        %obj.BatteryEnergy = 0;

    return %obj.BatteryEnergy;
}

function Player::SetBatteryEnergy(%obj, %set)
{
     %obj.BatteryEnergy = %set;

    return %obj.GetBatteryEnergy();
}

function Player::GetMaxBatteryEnergy(%obj)
{
    //Will be customizable later(tm)
    if (%obj.MaxBatteryEnergy $= "" || %obj.MaxBatteryEnergy < 0)
        %obj.MaxBatteryEnergy = 5000;

    return %obj.MaxBatteryEnergy;
}

function Player::SetMaxBatteryEnergy(%obj, %set)
{
     %obj.MaxBatteryEnergy = %set;

    return %obj.GetMaxBatteryEnergy();
}

function Player::ChangeBatteryEnergy(%obj, %change)
{
    %oldEnergy = %obj.GetBatteryEnergy();
    %obj.BatteryEnergy += %change;
    %obj.BatteryEnergy = mClamp(%obj.BatteryEnergy, 0, %obj.GetMaxBatteryEnergy());

    return %obj.BatteryEnergy - %oldEnergy;
}