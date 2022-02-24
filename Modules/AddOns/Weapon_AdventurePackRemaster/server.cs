// adventure weapons
// old-school wwII type things except adventurey and fun

		exec("add-ons/weapon_gun/server.cs");

		exec("./Support_Hitbox.cs");
		exec("./Support_AltDatablock.cs");
		exec("./support_AmmoSystem.cs");
		exec("./Support_ItemProps.cs");
		exec("./Support_SFX.cs");
		exec("./Support_RaycastingWeapons.cs");
		exec("./Support_FractionRayCast.cs");

		$hl2Weapons::ShowAmmo = true;
		$hl2Weapons::Ammo = true;

		exec("./adventure_Effects.cs");
		exec("./adventure_Sounds.cs");
		exec("./adventure_AmmoTypes.cs");

		  //////// tier 1
		exec("./weapon_automaticPistol.cs");
		exec("./weapon_revolver.cs");
		exec("./weapon_singleShotgun.cs");

		  //////// tier 2
		exec("./weapon_servicePistol.cs");
		exec("./weapon_huntingShotgun.cs");
		exec("./weapon_brushPistol.cs");
		exec("./weapon_doubleShotgun.cs");

		  //////// tier 3
		exec("./weapon_leverShotgun.cs");
		exec("./weapon_automaticRifle.cs");
		exec("./weapon_huntingMagnum.cs");
		exec("./weapon_leverRifle.cs");

		  //////// tier 4
		exec("./weapon_serviceRifle.cs");
		exec("./weapon_riotShotgun.cs");
		exec("./weapon_combatRifle.cs");
		exec("./weapon_huntingRifle.cs");
		exec("./weapon_fieldRifle.cs");	
		exec("./weapon_machinePistol.cs");

		  //////// tier 5
		exec("./weapon_sniperRepeater.cs");
		exec("./weapon_pairedShotgun.cs");
		exec("./weapon_assaultRifle.cs");
		
		 //////// tier 6 (extra)
		//exec("./weapon_taser.cs");
		//exec("./Weapon_HMachineGun.cs");
		//exec("./weapon_modernRifle.cs");
		//exec("./weapon_modernPistol.cs");
		//exec("./weapon_patriotRifle.cs");
//		exec("./weapon_battleShotgun.cs");  not happy with how this one turned out, basically hunting shotgun reskin with minor stats and shitty model :C
		//exec("./weapon_automaticShotgun.cs"); 
		//exec("./weapon_covertPistol.cs");
		//exec("./weapon_microSMG.cs");
		//exec("./weapon_microSMGSilenced.cs");
		//exec("./weapon_desertPistol.cs");
		//exec("./weapon_modernAutoPistol.cs");
		//exec("./weapon_LFriendRifle.cs");
		//exec("./weapon_guerillaRifle.cs");
		//exec("./weapon_serviceRevolver.cs");
		//exec("./weapon_GMachineGun.cs"); //remake this one
		//exec("./weapon_servicePistolsil.cs");
		//exec("./weapon_carbineSMG.cs");
		//exec("./weapon_navalSMG.cs");
		//exec("./weapon_retroSMG.cs");
		//exec("./weapon_militaryMG.cs");
		//exec("./weapon_compactSMG.cs");
		//exec("./weapon_sniperRifle.cs");
		
		exec("./adventure_Items.cs");



/////////////////////////////////////////////////


function SimObject::getYaw(%this) {
	%vector = %this.getForwardVector();
	return mATan(getWord(%vector, 0), getWord(%vector, 1));
}

function SimObject::getPitch(%this) {
	%vector = %this.getMuzzleVector(0);

	%x = getWord(%vector, 0);
	%y = getWord(%vector, 1);
	%z = getWord(%vector, 2);

	return mATan(%z, mSqrt(%x * %x + %y * %y));
}

function Item::monitorCollisionSounds(%this, %before)
{
	cancel(%this.monitorCollisionSounds);

	%data = %this.getDatablock();
	%now = vectorLen(%this.getVelocity());

	if (%before - %now >= %data.shellCollisionThreshold)
		%data.shellCollisionSFX.play(%this.getPosition());

	%this.monitorCollisionSounds = %this.schedule(50, "monitorCollisionSounds", %now);
}

function eulerToAxis(%euler)
{
	%euler = VectorScale(%euler, $pi / 180);
	return getWords(MatrixCreateFromEuler(%euler), 3, 6);
}

package ComplexBulletPackage
{
	function ItemData::onAdd(%this, %item)
	{
		Parent::onAdd(%this, %item);
		if (%this.canPickup !$= "")
			%item.canPickup = %this.canPickup;
		if (isObject(%this.shellCollisionSFX))
			%item.monitorCollisionSounds();
		if (%item.isSpent)
		{
			if (!isObject(SpentShellGroup))
				new SimSet(SpentShellGroup);
			%item.spawnTime = getSimTime();
			SpentShellGroup.add(%item);
			while (SpentShellGroup.getCount() > $Pref::Server::ShellLimit)
			{
				//SpentShellGroup.getObject(0).delete(); //Simsets will reorder, so this is not viable
				%count = SpentShellGroup.getCount();

				for (%i = 0; %i < %count; %i++) {
					%decal = SpentShellGroup.getObject(%i);

					if (%decal.spawnTime < %best || %best $= "") {
						%best = %decal.spawnTime;
						%oldest = %decal;
					}
				}

				if (isObject(%oldest)) {
					%oldest.delete();
				}
			}
		}
	}

	function Player::activateStuff(%this)
	{
		Parent::activateStuff(%this);

		%a = %this.getEyePoint();
		%b = vectorAdd(%a, vectorScale(%this.getEyeVector(), 6));

		%mask =
			$TypeMasks::StaticObjectType |
			$TypeMasks::FxBrickObjectType |
			$TypeMasks::VehicleObjectType |
			$TypeMasks::ItemObjectType;

		%ray = containerRayCast(%a, %b, %mask);
		%pos = getWords(%ray, 1, 3);
		initContainerRadiusSearch(
			%pos, 0.2,
			$TypeMasks::ItemObjectType
		);
		while (isObject(%col = containerSearchNext()))
		{
			%data = %col.getDataBlock();

			if (!%data.isBullet || %col.isSpent)
				continue;

			if (%this.bulletCount[%data.bulletType] == -1) //Infinite ammo detected
				continue;

			if (%amt = getField(%func = %this.addBullets(%data, %data.amount), 0) > 0)
			{
				RevolverInsertSFX.play(getWords(%col.getTransform(), 0, 2));
				if (isObject(%this.client))
				{
					if ($Sim::Time - %this.lastCenterPrint[%data.bulletType] <= 2)
						%amt = %this.lastAmt + %amt;
					if (%data.useAmmoPool)
						commandToClient(%this.client, 'CenterPrint', "\c2+"@ %amt @"\c3 "@ %data.bulletType @" \c6bullets.\n\c6You now have \c3" @ %this.bulletCount[%data.bulletType] @" bullets.", 2);
					else
						commandToClient(%this.client, 'CenterPrint', "\c2+"@ %amt @"\c3 "@ %data.bulletType @" \c6bullets.", 2);
					%this.lastAmt = %amt;
					%this.lastCenterPrint[%data.bulletType] = $Sim::Time;
				}
				%typeamt[%data.bulletType] += %amt;
				%col.delete();
			}
		}
	}
};

activatePackage("ComplexBulletPackage");

datablock ExplosionData(advLittleRecoilExplosion)
{
   explosionShape = "";

   lifeTimeMS = 150;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
  camShakeFreq = "1 1 1";
  camShakeAmp = "0.1 0.3 0.2";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   lightstartradius = 2;
   lightEndRadius = 2;
   lightstartColor = "1.0 1.0 0.7";
   lightEndColor = "0 0 0";
};

datablock ProjectileData(advLittleRecoilProjectile)
{
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= advLittleRecoilExplosion;

};

datablock ExplosionData(advRecoilExplosion)
{
   explosionShape = "";

   lifeTimeMS = 150;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
  camShakeFreq = "2 2 2";
  camShakeAmp = "0.3 0.5 0.4";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   lightstartradius = 2;
   lightEndRadius = 2;
   lightstartColor = "1.0 1.0 0.7";
   lightEndColor = "0 0 0";
};

datablock ProjectileData(advRecoilProjectile)
{
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= advRecoilExplosion;

};

datablock ExplosionData(advBigRecoilExplosion)
{
   explosionShape = "";

   lifeTimeMS = 150;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
  camShakeFreq = "3 3 3";
  camShakeAmp = "0.6 0.8 0.7";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   lightstartradius = 2;
   lightEndRadius = 2;
   lightstartColor = "1.0 1.0 0.7";
   lightEndColor = "0 0 0";
};

datablock ProjectileData(advBigRecoilProjectile)
{
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= advBigRecoilExplosion;

};

datablock ExplosionData(advHugeRecoilExplosion)
{
   explosionShape = "";

   lifeTimeMS = 150;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
  camShakeFreq = "5 5 5";
  camShakeAmp = "1.1 1.3 1.2";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   lightstartradius = 2;
   lightEndRadius = 2;
   lightStartColor = "1.0 1.0 0.7";
   lightEndColor = "0 0 0";
};

datablock ProjectileData(advHugeRecoilProjectile)
{
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= advHugeRecoilExplosion;

};

datablock PlayerData(emptyPlayer : playerStandardArmor)
{
	shapeFile = "base/data/shapes/empty.dts";
	boundingBox = "0.01 0.01 0.01";
	crouchboundingBox = "0.01 0.01 0.01";
	deathSound = "";
	painSound = "";
	useCustomPainEffects = true;
	mountSound = "";
	jumpSound = "";
	uiName = "";
}; // swollow's code i take no credit
