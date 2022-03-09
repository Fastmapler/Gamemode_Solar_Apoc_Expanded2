exec("./Brick_FissionHull.cs");
exec("./Brick_FissionPorts.cs");
exec("./Brick_FissionHeatPlating.cs");
exec("./Brick_FissionReactionPlate.cs");
exec("./Brick_FissionComponents.cs");
exec("./Brick_FissionDisplayPanel.cs");

package EOTW_Fission
{
    function fxDtsBrick::onPlant(%brick)
    {
        Parent::onPlant(%brick);

        if (!isObject(%brick))
            return;

        %data = %brick.getDatablock();
        if (%data.reqFissionPart !$= "" && isObject(%downBrick = %brick.getDownBrick(0)))
        {
            if (%downBrick.getDatablock().getName() $= %data.reqFissionPart)
            {
                if (isObject(%fission = %downBrick.fissionParent))
                    %fission.AddFissionPart(%brick);
            }
            else
            {
                if (isObject(%client = %brick.getGroup().client))
                    %client.chatMessage("This brick must be planted on a " @ %data.reqFissionPart.uiName @ "!");

                %brick.killBrick();
            }
            
        }
    }
};
activatePackage("EOTW_Fission");