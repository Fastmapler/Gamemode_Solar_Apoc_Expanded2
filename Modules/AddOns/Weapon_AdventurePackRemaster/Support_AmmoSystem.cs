//########## Ammo System
if($JackMods::Server::hl2AmmoSystemVersion > 1)
{
  return;
}

$JackMods::Server::hl2AmmoSystemVersion = 1;
function hl2AmmoCheck(%this,%obj,%slot)
{
   if(%obj.toolMag[%obj.currTool] $= "")
   {
      %obj.toolMag[%obj.currTool] = %this.item.maxmag;
   }

   if(%obj.toolAltMag[%this.item.altammotype] $= "" && %this.item.altmaxmag !$= "")
   {
      %obj.toolAltMag[%this.item.altammotype] = 0;//%this.item.altmaxmag;
   }

   if(%obj.toolAmmo[%this.item.ammotype] $= "")
   {
      %obj.toolAmmo[%this.item.ammotype] = $hl2Weapons::AddAmmo[%this.item.ammotype];
   }

   if(%obj.toolMag[%obj.currTool] < 1)
   {
      %obj.toolMag[%obj.currTool] = 0;
      %obj.setImageAmmo(0,0);
      if(%obj.toolAmmo[%this.item.ammotype] < 1)
      {
         %obj.toolMag[%obj.currTool] = %obj.toolAmmo[%this.item.ammotype] = 0;
      }
   }

   if(%obj.toolMag[%obj.currTool] >= 1)
   {
      %obj.setImageAmmo(0,1);
   }

   if(%obj.toolMag[%obj.currTool] > %this.item.maxmag)
   {
      %obj.toolMag[%obj.currTool] = %this.item.maxmag;
   }
}

function hl2AmmoOnReload(%this,%obj,%slot)
{
  if($hl2Weapons::Ammo)
  {
    if(%this.item.nochamber)
    {
      %a = %this.item.maxmag - %obj.toolMag[%obj.currTool];
      if(%a > %obj.toolAmmo[%this.item.ammotype])
        %a = %obj.toolAmmo[%this.item.ammotype];
      %obj.toolMag[%obj.currTool] += %a;
      %obj.toolAmmo[%this.item.ammotype] -= %a;
      %obj.setImageAmmo(0,1);
    }
    else
    {
      if(%obj.toolMag[%obj.currTool] > 0)
      {
        %a = (%this.item.maxmag) - %obj.toolMag[%obj.currTool];
        if(%a > %obj.toolAmmo[%this.item.ammotype])
          %a = %obj.toolAmmo[%this.item.ammotype];
        %obj.toolMag[%obj.currTool] += %a;
        %obj.toolAmmo[%this.item.ammotype] -= %a;
        %obj.setImageAmmo(0,1);
      }
      else
      {
        %a = %this.item.maxmag - %obj.toolMag[%obj.currTool];
        if(%a > %obj.toolAmmo[%this.item.ammotype])
          %a = %obj.toolAmmo[%this.item.ammotype];
        %obj.toolMag[%obj.currTool] += %a;
        %obj.toolAmmo[%this.item.ammotype] -= %a;
        %obj.setImageAmmo(0,0);
      }
    }
  }
  else
  {
    if(%this.item.nochamber)
    {
      %obj.toolMag[%obj.currTool] = %this.item.maxmag;
      %obj.setImageAmmo(0,1);
    }
    else
    {
      if(%obj.toolMag[%obj.currTool] > 0)
      {
        %obj.toolMag[%obj.currTool] = %this.item.maxmag + 1;
        %obj.setImageAmmo(0,1);
      }
      else
      {
        %obj.toolMag[%obj.currTool] = %this.item.maxmag;
        %obj.setImageAmmo(0,0);
      }
    }
  }
}

function hl2AmmoOnReloadSingle(%this,%obj,%slot)
{
  if($hl2Weapons::Ammo)
  {
    %obj.toolMag[%obj.currTool]++;
    %obj.toolAmmo[%this.item.ammotype]--;
    if(%obj.toolAmmo[%this.item.ammotype] < 1) {
      %obj.setImageAmmo(0,1);
      return; }
    else
      %obj.setImageAmmo(0,0);
    if(%obj.toolMag[%obj.currTool] < %this.item.maxmag)
      %obj.setImageAmmo(0,0);
    else
      %obj.setImageAmmo(0,1);
  }
  else
  {
    %obj.toolMag[%obj.currTool]++;
    if(%obj.toolMag[%obj.currTool] < %this.item.maxmag)
      %obj.setImageAmmo(0,0);
    else
      %obj.setImageAmmo(0,1);
  }
}

function hl2DisplayAmmo(%this, %obj, %slot, %delay) {
  if (!$HL2Weapons::ShowAmmo) {
    return;
  }

  if (%delay == -1) {
    clearBottomPrint(%obj.client);
    return;
  }

  %altMag = %obj.toolAltMag[%this.item.altAmmoType];

  if (%altMag !$= "") {
    %altAmmoText = "    " SPC %altmag;
  }

  if ($HL2Weapons::Ammo == 0) {
    %str = %obj.toolMag[%obj.currTool] @ "/" @ %this.item.maxMag;
  }
  else {
    %str = %obj.toolMag[%obj.currTool] @ "/" @ %obj.toolAmmo[%this.item.ammoType];
  }

  %ammocol = "<color:ffffff>";
  %str = %str SPC "<font:tahoma:20>AMMO<font:impact:24>" @ ( %altAmmoText !$= "" ? %altAmmoText SPC "<font:tahoma:20>ALT<font:impact:24>" : "" );
  %obj.client.bottomPrint(%ammocol @ "<font:impact:24><just:right>" @ %str SPC "\n", %delay, 1);
}

package hl2AmmoSystem
{
  function gameConnection::onDeath( %client, %source, %killer, %type, %location )
  {
    hl2DisplayAmmo(0, %client.player, %slot, -1);
    parent::onDeath( %client, %source, %killer, %type, %location );
  }

  function Player::pickUp(%this,%item)
  {
    %data = %item.dataBlock;

    if(%item.mag !$= "")
    {
      %mag = %item.mag;
    }
    %parent = parent::pickUp(%this,%item);
    if(!%data.reload)
    {
      return %parent;
    }

    %slot = -1;
    for(%i=0;%i<%this.dataBlock.maxTools;%i++)
    {
      if(isObject(%this.tool[%i]) && %this.tool[%i].getID() == %data.getID())
      {
        %slot = %i;
        break;
      }
    }
    if(%slot == -1)
    {
      return;
    }
    if(%mag !$= "")
    {
      %this.toolMag[%slot] = %mag;
    }
    else
    {
      %this.toolmag[%slot] = %data.maxmag;
    }
  }

  function serverCmdDropTool(%client,%slot)
  {
    if(isObject(%client.player))
    {
      %item = %client.player.tool[%client.player.currTool];
      $hl2weaponMag = %client.player.toolMag[%client.player.currTool];
      %client.player.toolMag[%client.player.currTool] = "";
    }
    parent::serverCmdDropTool(%client,%slot);
    %client.player.unMountImage(0);
  }

  function ItemData::onAdd(%this,%obj)
  {
    parent::onAdd(%this,%obj);
    if($hl2weaponMag !$= "") { %obj.mag = $hl2weaponMag; $hl2weaponMag = ""; }
  }

  function serverCmdLight(%client)
  {
    if(isObject(%client.player)) {
      %player = %client.player;
      %image = %player.getMountedImage(0);
      if(%image.item.reload)
      {
        if(%player.getImageState(0) $= "Ready" || %player.getImageState(0) $= "Empty")
        {
          if(%player.toolMag[%player.currTool] < %image.item.maxmag)
          {
            if($hl2Weapons::Ammo && %player.toolAmmo[%image.item.ammotype] < 1) { parent::serverCmdLight(%client); return; }
            %player.setImageAmmo(0,0);
            %player.Schedule(50,setImageAmmo,0,1);
          }
          else
            parent::serverCmdLight(%client);
        }
        return;
      } 
    }
    parent::serverCmdLight(%client);
  }

  function Armor::onCollision(%this,%obj,%col,%a,%b,%c,%d,%e,%f)
  {
    if(%col.dataBlock.reload $= "" && %col.dataBlock.ammoBox $= "" || $hl2Weapons::Ammo == 0)
    {
      if(%col.dataBlock.ammoBox)
      {
        return;
      }
      parent::onCollision(%this,%obj,%col,%a,%b,%c,%d,%e,%f);
      return;
    }

    if(%col.dataBlock.ammoBox && ( %col.dataBlock.ammoType !$= "" || %col.dataBlock.altAmmoType !$= "" ) && %col.canPickup && %obj.getDamagePercent() < 1 && minigameCanUse(%obj.client,%col))
    {
      if( %col.dataBlock.ammoType $= "ALL" )
      {
        for(%i=0; %i < %obj.dataBlock.maxTools; %i++)
        {
          %currTool = %obj.tool[%i];
          %ammoType = %currTool.ammotype;
          if(isObject(%currtool) && %obj.toolAmmo[%ammoType] < $hl2Weapons::MaxAmmo[%ammoType] && !%addedAmmo[%ammoType])
          {
            %newAmmo = $hl2Weapons::AddAmmo[%ammoType];
            if (%obj.toolAmmo[%ammoType] $= "") //not yet defined, give it baseline ammo (one mag)
            {
              %obj.toolAmmo[%ammoType] = %newAmmo;
            }
            if(%obj.toolAmmo[%ammoType] + $hl2Weapons::AddAmmo[%ammoType] > $hl2Weapons::MaxAmmo[%ammoType])
            {
              %newAmmo = $hl2Weapons::MaxAmmo[%ammoType] - %obj.toolAmmo[%ammoType];
              %obj.toolAmmo[%ammoType] = $hl2Weapons::MaxAmmo[%ammoType];
            }
            else
            {
              %obj.toolAmmo[%ammoType] += $hl2Weapons::AddAmmo[%ammoType];
            }

            %addedAmmo[%ammoType] = 1;
          }
          %item = %obj.tool[%obj.currTool];

          if( %item $= %currtool && %addedAmmo[%ammoType] )
          {
            hl2DisplayAmmo(%item.image, %obj, %obj.currTool);
          }
        }
      }
      else
      {
        for(%i=0; %i < %obj.dataBlock.maxTools; %i++)
        {
          %currTool = %obj.tool[%i];
          %ammoType = %currTool.ammotype;
          if(isObject(%currTool) && %ammoType $= %col.dataBlock.ammoType && %obj.toolAmmo[%ammoType] < $hl2Weapons::MaxAmmo[%ammoType] && !%addedAmmo[%ammoType])
          {
            %newAmmo = $hl2Weapons::AddAmmo[%ammoType] * 2; //double ammo given when picking up specific-type
            if (%obj.toolAmmo[%ammoType] $= "") //not yet defined, give it baseline ammo (one mag)
            {
              %obj.toolAmmo[%ammoType] = %newAmmo;
            }
            if(%obj.toolAmmo[%ammoType] + $hl2Weapons::AddAmmo[%ammoType] * 2 > $hl2Weapons::MaxAmmo[%ammoType])
            {
              %newAmmo = $hl2Weapons::MaxAmmo[%ammoType] - %obj.toolAmmo[%ammoType];
              %obj.toolAmmo[%ammoType] = $hl2Weapons::MaxAmmo[%ammoType];
            }
            else
            {
              %obj.toolAmmo[%ammoType] += $hl2Weapons::AddAmmo[%ammoType] * 2;
            }
            
            %addedAmmo[%ammoType] = 1;
            //commandToClient(%obj.client, 'centerPrint', "<just:right><font:Tahoma:22><color:DBA901>+" @ %newammo SPC %ammoType, 2);
          }
          %item = %obj.tool[%obj.currTool];

          if( %item $= %currTool && %addedAmmo[%ammoType] )
          {
            hl2DisplayAmmo(%item.image, %obj, %obj.currTool);
          }
             else
             {
               if(isObject(%currTool) && %currTool.altammotype $= %col.dataBlock.altammotype && %obj.toolAltMag[%currTool.altammotype] < %currTool.altmaxmag)
               {    
                 %newAmmo = %col.dataBlock.altAmmoAdd;
                 if(%obj.toolAltMag[%currTool.altammotype] + %col.dataBlock.altAmmoAdd > %currTool.altmaxmag)
                 {
                   %newAmmo = (%obj.toolAltMag[%currTool.altammotype] - %currTool.altmaxmag) * -1;
                   %obj.toolAltMag[%currTool.altammotype] = %currTool.altmaxmag;
                 }
                 else
                 {
                   %obj.toolAltMag[%currTool.altammotype] += %col.dataBlock.altAmmoAdd;
                 }
                 %item = %obj.tool[%obj.currTool];
                 if( %item $= %currTool )
                 {
                   hl2DisplayAmmo(%item.image, %obj, %obj.currTool);
                 }
               }
             }
        }
      }
      serverPlay3D(advReloadOut3Sound,%obj.getHackPosition());

      //parent::onCollision(%this,%obj,%col,%a,%b,%c,%d,%e,%f);

      if(isObject(%col.spawnBrick))
      {
        %col.fadeOut();
        %col.schedule(%col.spawnBrick.itemRespawnTime,fadein);
      }
      else
      {
        %col.schedule(10,delete);
      }
      return;
    }

    for(%i=0; %i < %obj.dataBlock.maxTools; %i++)
    {
      %currTool = %obj.tool[%i];
      %ammoType = %currTool.ammoType;
      if(isObject(%currTool) && %obj.toolAmmo[%ammoType] < $hl2Weapons::MaxAmmo[%ammoType])
      {
        if(%currTool.getName() $= %col.dataBlock.getName() && %col.canPickup && %obj.getDamagePercent() < 1 && minigameCanUse(%obj.client,%col))
        {
          if(isObject(%col.spawnBrick)) //can pick up ammo from spawned weapons
          {
            %newAmmo = $hl2Weapons::AddAmmo[%ammoType];
            if (%obj.toolAmmo[%ammoType] $= "") //not yet defined, give baseline ammo (one mag)
            {
              %obj.toolAmmo = %newAmmo;
            }
            if(%obj.toolAmmo[%ammoType] + $hl2Weapons::AddAmmo[%ammoType] > $hl2Weapons::MaxAmmo[%ammoType])
            {
              %newAmmo = $hl2Weapons::MaxAmmo[%ammoType] - %obj.toolAmmo[%ammoType];
              %obj.toolAmmo[%ammoType] = $hl2Weapons::MaxAmmo[%ammoType];
            }
            else
            {
              %obj.toolAmmo[%ammoType] += $hl2Weapons::AddAmmo[%ammoType];
            }

            serverPlay3D(advReloadOut3Sound,%obj.getHackPosition());
            //commandToClient(%obj.client, 'centerPrint', "<just:right><font:Tahoma:22><color:DBA901>+" @ %newAmmo SPC %ammoType, 2);

            %item = %obj.tool[%obj.currTool];
            if( %item $= %currTool )
            {
              hl2DisplayAmmo(%item.image, %obj, %obj.currTool);
            }

            %col.fadeOut();
            %col.schedule(%col.spawnBrick.itemRespawnTime,fadein);
            break;
          }
          else //pick ammo up from dropped items
          {
            %mag = %col.mag;
            if(%mag !$= 0)
            {
              if(%mag $= "")
              {
                %mag = $hl2Weapons::AddAmmo[%ammoType];
              }

              
              %newAmmo = %mag;
              if(%obj.toolAmmo[%ammoType] + %mag > $hl2Weapons::MaxAmmo[%ammoType])
              {
                %newAmmo = $hl2Weapons::MaxAmmo[%ammoType] - %obj.toolAmmo[%ammoType];
                %obj.toolAmmo[%ammoType] = $hl2Weapons::MaxAmmo[%ammoType];
              }
              else
              {
                %obj.toolAmmo[%ammoType] += %mag;
              }
              %col.mag = %mag - %newAmmo;
              serverPlay3D(advReloadOut3Sound,%obj.getHackPosition());

              if(%newAmmo > 0)
              {
                commandToClient(%obj.client, 'centerPrint', "<just:right><font:Tahoma:22><color:DBA901>+" @ %newAmmo SPC %ammoType, 2);
              }
              %item = %obj.tool[%obj.currTool];
              if( %item $= %currTool )
              {
                hl2DisplayAmmo(%item.image, %obj, %obj.currTool);
              }
              break;
              respawn(%col);
            }
          }
          %item = %obj.tool[%obj.currTool];
          if( %item $= %currTool )
          {
            hl2DisplayAmmo(%item.image, %obj, %obj.currTool);
          }
        }
      }
    }
    parent::onCollision(%this,%obj,%col,%a,%b,%c,%d,%e,%f);
  }
};
activatePackage(hl2AmmoSystem);
