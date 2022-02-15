//sounds
datablock AudioProfile(brushPistolFireSound)
{
   filename    = "./Sounds/Fire/New/brushPistolFire.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock DebrisData(brushPistolLoaderDebris)
{
	shapeFile = "./shapes/weapons/brushPistolLoader.dts";
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
datablock ExplosionData(brushPistolLoaderExplosion)
{
	debris 					= brushPistolLoaderDebris;
	debrisNum 				= 1;
	debrisNumVariance 		= 0;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 1;
	debrisVelocityVariance 	= 0;
};
datablock ProjectileData(brushPistolLoaderProjectile)
{
	explosion = brushPistolLoaderExplosion;
};

datablock ExplosionData(brushPistolShellExplosion)
{
	debris 					= BAADPistolDebris;
	debrisNum 				= 1;
	debrisNumVariance 		= 0;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 1;
	debrisVelocityVariance 	= 0;
};
datablock ProjectileData(brushPistolShellProjectile)
{
	explosion = brushPistolShellExplosion;
};

//muzzle flash effects
datablock ProjectileData(brushPistolProjectile1 : gunProjectile)
{
   directDamage        = 35;//8;
   explosion           = QuietGunExplosion;
   impactImpulse       = 500;
   verticalImpulse     = 450;

   muzzleVelocity      = 200;
   velInheritFactor    = 0;

   armingDelay         = 00;
   lifetime            = 4000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false; 
   gravityMod = 0.0;

   particleEmitter     = advBigBulletTrailEmitter; //bulletTrailEmitter;
   headshotMultiplier = 3.5;
   sound = BAADWhiz1Sound;  
   uiName = "";	
};

datablock ProjectileData(brushPistolProjectile2 : brushPistolProjectile1)
{
   sound = BAADWhiz2Sound;
};

datablock ProjectileData(brushPistolProjectile3 : brushPistolProjectile1)
{
   sound = BAADWhiz3Sound;
};

//////////
// item //
//////////

datablock ItemData(brushPistolItem)
{
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

    // Basic Item Properties
   shapeFile = "./shapes/weapons/brushPistolItem.dts";
   rotate = false;
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   //gui stuff
   uiName = "AP - Brush Pistol";
   //iconName = "./icons/icon_Pistol";
   doColorShift = false;
   colorShiftColor = "0.25 0.25 0.25 1.000";

   maxmag = 6;
   ammotype = "Revolver";
   reload = true;

   nochamber = 1;

    // Dynamic properties defined by the scripts
   image = brushPistolImage;
   canDrop = true;
   
   shellCollisionThreshold = 2;
   shellCollisionSFX = WeaponSoftImpactSFX;

   itemPropsClass = "SimpleMagWeaponProps";
};

////////////////
//weapon image//
//////////////// 
datablock ShapeBaseImageData(brushPistolImage)
{
   // Basic Item properties
   shapeFile = "./shapes/weapons/brushPistol.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0.025 0.075";
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
   item = brushPistolItem;
   ammo = " ";
   projectile = brushPistolProjectile;
   projectileType = Projectile;

   casing = BAADPistolDebris;
   shellExitDir        = "0.25 -0.05 0.5";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 8.0;   
   shellVelocity       = 12.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = brushPistolItem.colorShiftColor;//"0.400 0.196 0 1.000";

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
   stateSequence[0]                 = "yup";
   stateTransitionOnTimeout[0]      = "AmmoCheckReady";
   stateSound[0]                    = "";

   stateName[1]                     = "Ready";
   stateTransitionOnTriggerDown[1]  = "Fire";
   stateTransitionOnNoAmmo[1]       = "Empty";
   stateAllowImageChange[1]         = true;
   stateSequence[1]                 = "root";

   stateName[2]                     = "Fire";
   stateTimeoutValue[2]             = 0.2;
   stateTransitionOnTimeout[2]      = "Smoke";
   stateFire[2]                     = true;
   stateAllowImageChange[2]         = false;
   stateSequence[2]                 = "Fire";
   stateScript[2]                   = "onFire";
   stateWaitForTimeout[2]           = true;
   stateEmitter[2]                  = advSmallBulletFireEmitter;
   stateEmitterTime[2]              = 0.05;
   stateEmitterNode[2]              = "muzzleNode";
   stateEjectShell[2]               = false;

   stateName[3]                     = "Smoke";
   stateEmitter[3]                  = advSmallBulletSmokeEmitter;
   stateEmitterTime[3]              = 0.05;
   stateEmitterNode[3]              = "muzzleNode";
   stateTimeoutValue[3]             = 0.001;
   stateTransitionOnTimeout[3]    = "AmmoCheck";
   stateWaitForTimeout[3]           = true;
   stateSound[3]                    = "";

   stateName[4]                     = "Cycle";
   stateTimeoutValue[4]             = 0.35;
   stateTransitionOnTriggerUp[4]      = "Ready";

   stateName[5]                     = "AmmoCheck";
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
   stateSound[9]                    = baadRevDryFireSound;
   stateSequence[9]                 = "fire";
   
   stateName[10]                     = "EmptyFireB";
   stateTransitionOnTimeout[10]  = "CockEmpty";
   stateAllowImageChange[10]         = false;
   stateTimeoutValue[10]             = 0.05;
   stateWaitForTimeout[10]           = false;
   stateSequence[10]                 = "untrig";
   
   stateName[11]                     = "ReloadA"; 
   stateTransitionOnTimeout[11]  = "ReloadB";
   stateAllowImageChange[11]         = true;
   stateScript[11]                   = "OnReloadA";
   stateTimeoutValue[11]             = 0.1;
   stateSequence[11]                 = "opencylinder";
   
   stateName[12]                     = "ReloadB";
   stateTransitionOnTimeout[12]  = "ReloadD";
   stateAllowImageChange[12]         = true;
   stateSequence[12]                 = "openedcylinder";
   stateScript[12]                   = "OnReloadB";
   stateTimeoutValue[12]             = 0.5;
     
   stateName[14]                     = "ReloadD";
   stateTransitionOnTimeout[14]  = "ReloadF";
   stateAllowImageChange[14]         = true;
   stateSequence[14]                 = "loaderinsert";
   stateScript[14]                   = "OnReloadD";
   stateTimeoutValue[14]             = 0.3;
   
   stateName[16]                     = "ReloadF";
   stateTransitionOnTimeout[16]  = "ReloadReady";
   stateAllowImageChange[16]         = true;
   stateSequence[16]                 = "closecylinder";
   stateScript[16]                   = "OnLoaderRemoved";
   stateTimeoutValue[16]             = 0.1;

   stateName[17]                     = "untrig";
   stateTimeoutValue[17]             = 0.05;
   stateSequence[17]                 = "untrig";
   stateTransitionOnTimeout[17]      = "Cock";
   stateTransitionOnNoAmmo[17]       = "CockEmpty";
   stateSound[17]                    = "";
   
   stateName[18]                     = "AmmoCheckReady";
   stateTransitionOnNoAmmo[18]       = "Empty";
   stateScript[18]                   = "onAmmoCheck";
   stateTransitionOnTimeout[18]      = "Ready";
   stateAllowImageChange[18]         = true;

   stateName[19]                     = "Cock";
   stateTimeoutValue[19]             = 0.15;
   stateTransitionOnTimeout[19]      = "Cycle";
   stateScript[19]                   = "onCock";
   stateSequence[19]                 = "cock";

   stateName[20]                     = "CockEmpty";
   stateTimeoutValue[20]             = 0.15;
   stateTransitionOnTimeout[20]      = "CycleEmpty";
   stateScript[20]                   = "onCock";
   stateSequence[20]                 = "cock";

   stateName[22]                     = "CycleEmpty";
   stateTimeoutValue[22]             = 0.35;
   stateTransitionOnTriggerUp[22]      = "Empty";
};

  ////// ammo display functions
function brushPistolImage::onMount( %this, %obj, %slot )
{
parent::onMount(%this,%obj,%slot); 
hl2DisplayAmmo(%this,%obj,%slot,0);
schedule(getRandom(0,50),0,serverPlay3D,BAADEquip @ getRandom(1,3) @ Sound,%obj.getPosition());
}

function brushPistolImage::onUnMount( %this, %obj, %slot )
{parent::onUnMount(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot,-1);}

function brushPistolImage::onAmmoCheck( %this, %obj, %slot )
{hl2AmmoCheck(%this,%obj,%slot); hl2DisplayAmmo(%this,%obj,%slot);}

  /////// reload functions
function brushPistolImage::onReloadStart( %this, %obj, %slot )
{
   hl2DisplayAmmo(%this,%obj,%slot);
}

function brushPistolImage::onReload( %this, %obj, %slot )
{
   hl2AmmoOnReload(%this,%obj,%slot); 
   hl2DisplayAmmo(%this,%obj,%slot);
}

function brushPistolImage::onEmptyFire( %this, %obj, %slot )
{
   %obj.playThread(2, plant);
}

function brushPistolImage::onEmpty(%this,%obj,%slot)
{
   if( $hl2Weapons::Ammo && %obj.toolAmmo[%this.item.ammotype] < 1 )
   {
      return;
   }

   if(%obj.toolMag[%obj.currTool] < 1)
   {
      serverCmdLight(%obj.client);
   }
}

function brushPistolImage::onFire( %this, %obj, %slot )
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

    %projectiles = "brushPistolProjectile1" TAB "brushPistolProjectile2" TAB "brushPistolProjectile3";
    %projectile = getField(%projectiles, getRandom(0, getFieldCount(%projectiles)-1));
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
	%obj.spawnExplosion(advLittleRecoilProjectile,"1.25 1.25 1.25");
            		
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

   %obj.playThread(2, activate);
   %obj.playThread(3, plant);
   serverPlay3D( brushPistolFireSound,%obj.getPosition());
}

function brushPistolProjectile1::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function brushPistolProjectile2::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}
function brushPistolProjectile3::damage( %this, %obj, %col, %fade, %pos, %normal ){   %damage = %this.directDamage;   %headshot = matchBodyArea( getHitbox( %obj, %col, %pos ), $headTest );   if ( %headshot )   {      %damage *= %this.headshotMultiplier;   }   %col.damage( %obj, %pos, %damage, %this.directDamageType );}

function brushPistolImage::OnCock(%this, %obj, %slot) 
{
   %obj.playThread(2,plant);
   schedule(0, 0, serverPlay3D, baadRevClickSound, %obj.getPosition());
}

function brushPistolImage::OnReloadA(%this, %obj, %slot) 
{
   %obj.playThread(2,shiftleft);
   schedule(0, 0, serverPlay3D, baadReload1Sound, %obj.getPosition());
}

function brushPistolImage::OnReloadB(%this, %obj, %slot) 
{
   %obj.schedule(0, "playThread", "2", "wrench");
   %obj.schedule(0, "playThread", "3", "shiftRight");
   schedule(0, 0, serverPlay3D, baadReload14Sound, %obj.getPosition());
   
          %up = %obj.getUpVectorHack();
          %forward = %obj.getEyeVectorHack();
		  for(%i=0;%i<6;%i++)
          {
          schedule(getRandom(250,350),0,serverPlay3D,BAADShellSMG @ getRandom(1,8) @ Sound,%obj.getPosition());
          %p = new projectile()
          {
              datablock = "brushPistol" @ (%obj.toolAmmo[%obj.currTool] <= 0 ? "Shell" : "") @ "Projectile";
              initialPosition = vectorAdd(%obj.getSlotTransform(0),vectorRelativeShift(%forward,%up,"0 -0.18 -0.4"));
          };
          %p.explode();
		  }
}

function brushPistolImage::OnReloadD(%this, %obj, %slot)
{
   %obj.schedule(0, "playThread", "2", "shiftUp");
   schedule(0, 0, serverPlay3D, baadReload11Sound, %obj.getPosition());
}

function brushPistolImage::OnLoaderRemoved(%this, %obj, %slot)
{
          %obj.schedule(0, "playThread", "2", "shiftRight");
          %obj.schedule(100, "playThread", "2", "plant");
          schedule(0, 0, serverPlay3D, baadReload2Sound, %obj.getPosition());
          schedule(getRandom(150,250),0,serverPlay3D,BAADMagDrop @ getRandom(1,3) @ Sound,%obj.getPosition());
          hl2AmmoOnReload(%this,%obj,%slot); 
          hl2DisplayAmmo(%this,%obj,%slot);
          %up = %obj.getUpVectorHack();
          %forward = %obj.getEyeVectorHack();
          %p = new projectile()
          {
              datablock = "brushPistol" @ (%obj.toolAmmo[%obj.currTool] <= 0 ? "Loader" : "") @ "Projectile";
              initialPosition = vectorAdd(%obj.getSlotTransform(0),vectorRelativeShift(%forward,%up,"0 -0.18 -0.3"));
          };
          %p.explode();
}
