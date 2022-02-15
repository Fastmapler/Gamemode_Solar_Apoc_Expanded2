//sounds

datablock AudioProfile(LFriendRifleFireLoopSound)
{
   filename    = "./Sounds/Fire/New/ScarfaceFireLoop.wav";
   description = BAADFireLoop3D;
   preload = true;
};

datablock AudioProfile(LFriendRifleFireSound) 
{
   filename    = "./Sounds/Fire/LFriendRifle.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(LFriendRifleLauncherFireSound) 
{
   filename    = "./Sounds/Fire/New/M203Fire.wav";
   description = AudioClose3d;
   preload = true;
};

datablock DebrisData(LFriendRifleClipDebris)
{
	shapeFile = "./shapes/weapons/LFriendRifleMag.dts";
	lifetime = 2.0;
	minSpinSpeed = 700.0;
	maxSpinSpeed = 800.0;
	elasticity = 0.5;
	friction = 0.1;
	numBounces = 3;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
  doColorShift = false;
  colorShiftColor = "0.4 0.4 0.4 1.000";
};
datablock ExplosionData(LFriendRifleClipExplosion)
{
	debris 					= LFriendRifleClipDebris;
	debrisNum 				= 1;
	debrisNumVariance 		= 0;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 1;
	debrisVelocityVariance 	= 0;
};
datablock ProjectileData(LFriendRifleClipProjectile)
{
	explosion = LFriendRifleClipExplosion;
};

//////////
// item //
//////////

datablock ItemData(LFriendRifleItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/LFriendRifle.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "AP - L. Friend Rifle";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 50;
   ammotype = "Machine Rifle";
   reload = true;
   
   altmaxmag = 4;
   altammotype = "riflenade"; //for ammo item reference

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = LFriendRifleImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponHardImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
//////////////// 
datablock ShapeBaseImageData(LFriendRifleImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/LFriendRifle.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0.01 0.05";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = LFriendRifleItem;
   ammo = " ";
   projectile = LFriendRifleProjectile;
   projectileType = Projectile;

   casing = BAADRifleDebris;
   shellExitDir        = "0.25 0 0.25";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 8.0;   
   shellVelocity       = 10.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = LFriendRifleItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
   stateName[0]                     = "Activate";
   stateTimeoutValue[0]             = 0.5;
   stateScript[0]                   = "onAmmoCheck";
   stateSequence[0]                 = "magin";
   stateTransitionOnTimeout[0]      = "AmmoCheckReady";
   stateSound[0]                    = "";

   stateName[1]                     = "Ready";
   stateTransitionOnTriggerDown[1]  = "Fire";
   stateTransitionOnNoAmmo[1]       = "Empty";
   stateAllowImageChange[1]         = true;
   stateSequence[1]                 = "root";

   stateName[2]                     = "Fire";
   stateTimeoutValue[2]             = 0.075;
   stateTransitionOnTimeout[2]      = "Smoke";
   stateFire[2]                     = true;
   stateAllowImageChange[2]         = false;
   stateSequence[2]                 = "Fire";
   stateScript[2]                   = "onFire";
   stateWaitForTimeout[2]           = true;
   stateEmitter[2]                  = advSmallBulletFireEmitter;
   stateEmitterTime[2]              = 0.05;
   stateEmitterNode[2]              = "muzzleNode";
   stateEjectShell[2]               = true;

   stateName[3]                     = "Smoke";
   stateEmitter[3]                  = advSmallBulletSmokeEmitter;
   stateEmitterTime[3]              = 0.05;
   stateEmitterNode[3]              = "muzzleNode";
   stateTimeoutValue[3]             = 0.001;
   stateTransitionOnTimeout[3]    = "AmmoCheck";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTimeoutValue[4]             = 0.15;
   stateTransitionOnTimeout[4]      = "Ready";
   stateSound[4]                    = "";

   stateName[5]                     = "AmmoCheck";
   stateTransitionOnNoAmmo[5]       = "untrignoanim";
   stateTransitionOnTimeout[5]  = "Fire";
   stateTransitionOnTriggerUp[5]      = "untrig";
   stateAllowImageChange[5]         = true;
   stateScript[5]                   = "onAmmoCheck";

   stateName[6]                     = "Reload";
   stateTransitionOnTimeout[6]      = "ReloadA";
   stateTimeoutValue[6]             = 0.01;
   stateAllowImageChange[6]         = true;
   stateScript[6]                   = "onReloadStart";
   stateSound[6]                    = advSound;

   stateName[7]                     = "ReloadReady";
   stateTransitionOnTimeout[7]      = "Ready";
   stateTimeoutValue[7]             = 0.1;
   stateAllowImageChange[7]         = true;
   stateScript[7]                   = "onReload";

   stateName[8]                     = "Empty";
   stateTransitionOnTriggerDown[8]  = "EmptyFireA";
   stateAllowImageChange[8]         = true;
   stateScript[8]                   = "onEmpty";
   stateTransitionOnAmmo[8]         = "Reload";
   //stateSequence[8]                 = "noammo";

   stateName[9]                     = "EmptyFireA";
   stateTransitionOnTriggerUp[9]    = "EmptyFireB";
   stateScript[9]                   = "onEmptyFire";
   stateTimeoutValue[9]             = 0.05;
   stateAllowImageChange[9]         = false;
   stateWaitForTimeout[9]           = false;
   stateSound[9]                    = baadEmptySound;
   stateSequence[9]                 = "trig";
   
   stateName[10]                     = "EmptyFireB";
   stateTransitionOnTimeout[10]  = "Empty";
   stateAllowImageChange[10]         = false;
   stateTimeoutValue[10]             = 0.05;
   stateWaitForTimeout[10]           = false;
   stateSequence[10]                 = "untrig";
   
   stateName[11]                     = "ReloadA"; 
   stateTransitionOnTimeout[11]  = "ReloadB";
   stateAllowImageChange[11]         = true;
   stateScript[11]                   = "OnClipRemoved";
   stateTimeoutValue[11]             = 0.7;
   stateSequence[11]                 = "magout";
     
   stateName[12]                     = "ReloadB";
   stateTransitionOnTimeout[12]  = "ReloadC";
   stateAllowImageChange[12]         = true;
   stateTimeoutValue[12]             = 0.2;
   stateScript[12]                   = "OnReloadB";
   stateSequence[12]                 = "magin";
     
   stateName[13]                     = "ReloadC";
   stateTransitionOnTimeout[13]  = "ReloadReady";
   stateAllowImageChange[13]         = true;
   stateScript[13]                   = "OnReloadC";
   stateTimeoutValue[13]             = 0.25;
   stateSequence[13]                 = "bolt";

   stateName[14]                     = "untrig";
   stateTimeoutValue[14]             = 0.05;
   stateScript[14]                   = "onUntrig";
   stateSequence[14]                 = "untrig";
   stateTransitionOnTimeout[14]      = "Ready";
   stateSound[14]                    = "";
   
   stateName[15]                     = "AmmoCheckReady";
   stateTransitionOnNoAmmo[15]       = "Empty";
   stateScript[15]                   = "onAmmoCheck";
   stateTransitionOnTimeout[15]      = "Ready";
   stateAllowImageChange[15]         = true;
   
   stateName[20]                     = "untrignoanim";
   stateScript[20]                   = "onUntrig";
   stateTransitionOnTimeout[20]      = "EmptyFireB";
   stateTransitionOnTriggerDown[20]      = "Empty";
};

  ////// ammo display functions

function LFriendRifleImage::onMount( %this, %obj, %slot )
{
   parent::onMount(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot,0);
   schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function LFriendRifleImage::onUnMount( %this, %obj, %slot )
{
    parent::onUnMount(%this,%obj,%slot); 
    hl2DisplayAmmo(%this,%obj,%slot,-1);
	if(%obj.isPlayingLoopingAudio[%this,%slot+1])
	{
		%obj.isPlayingLoopingAudio[%this,%slot+1] = 0;
		%obj.stopAudio(%slot+1);
		serverPlay3D(assaultRifleFireEndSound,%obj.getPosition());
	}
}

function LFriendRifleImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function LFriendRifleImage::onReloadStart( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function LFriendRifleImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function LFriendRifleImage::onEmptyFire( %this, %obj, %slot )
{
	if(%obj.isPlayingLoopingAudio[%this,%slot+1])
	{
		%obj.isPlayingLoopingAudio[%this,%slot+1] = 0;
		%obj.stopAudio(%slot+1);
		serverPlay3D(assaultRifleFireEndSound,%obj.getPosition());
	}
   %obj.playThread(2, plant);
}

function LFriendRifleImage::onUntrig( %this, %obj, %slot )
{
	if(%obj.isPlayingLoopingAudio[%this,%slot+1])
	{
		%obj.isPlayingLoopingAudio[%this,%slot+1] = 0;
		%obj.stopAudio(%slot+1);
		serverPlay3D(assaultRifleFireEndSound,%obj.getPosition());
	}
}

function LFriendRifleImage::onEmpty(%this,%obj,%slot)
{
   if( $hl2Weapons::Ammo && %obj.toolAmmo[%this.item.ammotype] < 1 )
   {
      return;
   }

   if(%obj.toolMag[%obj.currTool] < 1)
   {
      serverCmdLight(%obj.client);
   }
   
	if(%obj.isPlayingLoopingAudio[%this,%slot+1])
	{
		%obj.isPlayingLoopingAudio[%this,%slot+1] = 0;
		%obj.stopAudio(%slot+1);
		serverPlay3D(assaultRifleFireEndSound,%obj.getPosition());
	}
   
}

function LFriendRifleImage::onFire( %this, %obj, %slot )
{
   if(%obj.getDamagePercent() >= 1)
   return;

   %obj.toolMag[%obj.currTool] -= 1;

   if(%obj.toolMag[%obj.currTool] < 1)
   {
      %obj.toolMag[%obj.currTool] = 0;
      %obj.setImageAmmo(0,0);
   }
   hl2DisplayAmmo(%this,%obj,%slot);

    %projectiles = "modernRifleProjectile1" TAB "modernRifleProjectile2" TAB "modernRifleProjectile3";
    %projectile = getField(%projectiles, getRandom(0, getFieldCount(%projectiles)-1));
	%spread = 0.0015;
	%shellcount = 1;

  	%fvec = %obj.getForwardVector();
  	%fX = getWord(%fvec,0);
  	%fY = getWord(%fvec,1);
  
  	%evec = %obj.getEyeVector();
  	%eX = getWord(%evec,0);
  	%eY = getWord(%evec,1);
  	%eZ = getWord(%evec,2);
  
  	%eXY = mSqrt(%eX*%eX+%eY*%eY);
  
  	%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
	//%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%aimVec,"-1")));
	%obj.spawnExplosion(advLittleRecoilProjectile,"0.5 0.5 0.5");
            		
	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}

   %obj.playThread(2, plant);
   if(!%obj.isPlayingLoopingAudio[%this,%slot+1])
   {
    	%obj.playAudio(%slot+1,LFriendRifleFireLoopSound);
    	%obj.isPlayingLoopingAudio[%this,%slot+1] = 1;
   }
   if(getSimTime()-%obj.lastShellSound > 800 || getRandom(0,1) == 0)
   {
   %obj.lastShellSound = getSimTime();		
   schedule(getRandom(500,600),0,serverPlay3D,BAADShellRifle @ getRandom(1,6) @ Sound,%obj.getPosition());
   }
}

function LFriendRifleImage::OnClipRemoved(%this, %obj, %slot)
{
      	  %obj.playThread(2,shiftDown);
          schedule(getRandom(250,350),0,serverPlay3D,BAADMagDrop @ getRandom(1,3) @ Sound,%obj.getPosition());
          schedule(0, 0, serverPlay3D, baadReload6Sound, %obj.getPosition());
          %up = %obj.getUpVectorHack();
          %forward = %obj.getEyeVectorHack();

          %p = new projectile()
          {
              datablock = "LFriendRifle" @ (%obj.toolAmmo[%obj.currTool] <= 0 ? "Clip" : "") @ "Projectile";
              initialPosition = vectorAdd(%obj.getSlotTransform(0),vectorRelativeShift(%forward,%up,"0.4 0 -0.5"));
          };
          %p.explode();
}

function LFriendRifleImage::OnReloadB(%this, %obj, %slot) 
{
   schedule(0, 0, serverPlay3D, baadReload9Sound, %obj.getPosition());
   %obj.playThread(2,shiftUp);
}

function LFriendRifleImage::OnReloadC(%this, %obj, %slot)
{
   schedule(0, 0, serverPlay3D, baadSlide3Sound, %obj.getPosition());
   %obj.schedule(75, "playThread", "2", "plant");
   %obj.schedule(0, "playThread", "3", "leftRecoil");
}

package LFriendRifleGL
{
	function Armor::onTrigger(%this, %obj, %slot, %val)
	{
		if(%obj.getMountedImage(0) $= LFriendRifleImage.getID() && %slot $= 4 && %val)
		{
			if(%obj.getImageState(0) $= "Ready" || %obj.getImageState(0) $= "Empty")
			{
				%image = %obj.getMountedimage(0);
				if(%obj.toolAltMag[%obj.tool[%obj.currtool].altammotype] <= 0 || $Sim::Time - %obj.lastSMGNade < 1)
				{
//					serverPlay3D(advReloadTap1Sound,%obj.getPosition());
					return;
				}
				hl2DisplayAmmo(%this, %obj, %obj.currTool);
				%obj.unMountImage(0);
				%obj.mountImage(LFriendRifleAltFireImage, 0);
				%obj.lastSMGNade = $Sim::Time;
			}
		}
		Parent::onTrigger(%this, %obj, %slot, %val);
	}
};   
ActivatePackage(LFriendRifleGL);

datablock ShapeBaseImageData(LFriendRifleAltFireImage)
{
   shapeFile = "./shapes/weapons/LFriendRifle.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0.01 0.05";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "10 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = LFriendRifleItem;
   ammo = " ";
   projectile = tankshellProjectile;
   projectileType = Projectile;

   casing = a;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = LFriendRifleItem.colorShiftColor;//"0.400 0.196 0 1.000";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.75;
	stateTransitionOnTimeout[0]      = "Fire";

	stateName[1]                     = "Fire";
	stateTransitionOnTimeout[1]      = "Wait";  
	stateTimeoutValue[1]             = 0.15;
	stateFire[1]                     = true;
	stateAllowImageChange[1]         = false;
	stateSequence[1]                 = "gltrig";
	stateScript[1]                   = "onFire";
	stateWaitForTimeout[1]           = true;
	
	stateName[2]                     = "Wait";
	stateTimeoutValue[2]             = 0.2;
	stateTransitionOnTimeout[2]      = "Done";

	stateName[3]                = "Done";
	stateAllowImageChange[3]       = true;
	stateSequence[3]            = "Ready";
	stateScript[3]              = "onDone";
};

function LFriendRifleAltFireImage::onMount( %this, %obj, %slot )
{
     %obj.playThread(2, plant);
     %obj.playThread(3, shiftAway);
     schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
	 parent::onMount(%this,%obj,%slot);
	 hl2DisplayAmmo(%this,%obj,%slot,0);
}
function LFriendRifleAltFireImage::onUnMount( %this, %obj, %slot )
{
	 parent::onUnMount(%this,%obj,%slot);
	 hl2DisplayAmmo(%this,%obj,%slot,-1);
}

function LFriendRifleAltFireImage::onFire(%this, %obj, %slot)
{
  if(%obj.getDamagePercent() >= 1)
   return;
   hl2DisplayAmmo(%this,%obj,%slot);

    %projectile = tankshellProjectile;
	%spread = 0.0001;
	%shellcount = 1;

  	%fvec = %obj.getForwardVector();
  	%fX = getWord(%fvec,0);
  	%fY = getWord(%fvec,1);
  
  	%evec = %obj.getEyeVector();
  	%eX = getWord(%evec,0);
  	%eY = getWord(%evec,1);
  	%eZ = getWord(%evec,2);
  
  	%eXY = mSqrt(%eX*%eX+%eY*%eY);
  
  	%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
	//%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%aimVec,"-1")));
	%obj.spawnExplosion(advLittleRecoilProjectile,"0.25 0.25 0.25");
            		
	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	serverPlay3D(LFriendRifleLauncherFireSound,%obj.getPosition());
	%obj.playThread(2, jump);
	%obj.playThread(3, plant);
   %obj.schedule(275, "playThread", "2", "plant");
   %obj.schedule(275, "playThread", "3", "shiftTo");
	%obj.toolAltMag[%obj.tool[%obj.currtool].altammotype]--;
	hl2DisplayAmmo(%this, %obj, %slot);
}

function LFriendRifleAltFireImage::onDone(%this, %obj, %slot)
{
	%obj.mountImage(LFriendRifleImage, 0);
}
