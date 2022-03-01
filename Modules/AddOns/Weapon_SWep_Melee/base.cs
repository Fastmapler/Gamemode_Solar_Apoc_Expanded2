function generateSwingData(%pl,%anim,%c,%f)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	cancel(%pl.swingCheckSched);
	if(%anim $= "")
	{
		%pl.setTransform(%pl.getPosition() SPC "0 0 1" SPC 0);
		$mvPitch = $PI*2;
		schedule(500,0,eval,"$mvPitch = -$PI/2;");
		return;
	}
	if(!%f)
	{
		if($SwolMelee_SwingDataName !$= "" && $SwolMelee_SwingDataName !$= %anim)
		{
			error("Please record+clear swing data first");
			return;
		}
		$SwolMelee_SwingDataName = %anim;
		%ai.swingPrev[0] = "";
		%pl.swingCheckEnd = getSimTime()+(240*4);
		%swingWindup = $SwolMelee_AnimWindup[%anim]*4;
		%pl.swingStartTime = getSimTime();
		$SwolMelee_SwingDataWind = %swingWindup/4;
		%ai.playThread(1,$SwolMelee_Anim[%anim]);
		%pl.swingCheckSched = schedule(%swingWindup,%pl,generateSwingData,%pl,%anim,%c,1);
		
		echo("Expected " @ (mFloor(((%pl.swingCheckEnd-(%pl.swingStartTime+%swingWindup))/32)*1000)/1000));
	}
	else
	{
		if(getSimTime()-%pl.swingCheckEnd > 0)
		{
			$SwolMelee_SwingData[$SwolMelee_SwingDataCnt] = "";
			$SwolMelee_SwingDataCnt++;
			if(%c >= 1)
			{
				%pl.swingCheckSched = schedule(200,%pl,generateSwingData,%pl,%anim,%c--,0);
			}
			else
			{
				recordSwingData();
				clearSwingData();
			}
			return;
		}
		
		%up = MatrixMulVector(%ai.getSlotTransform(0),"0 0 1");
		%forward = %pl.getForwardVector();
		%ppos = %pl.getPosition();
		%chest = getWords(%pl.getPosition(),0,1) SPC 0;
	
		%posBase = %ai.getSlotTransform(0);

		%relativeShift = vectorRelativeShift(%forward,%up,"0 0" SPC (0));
		%pos = vectorAdd(%posBase,%relativeShift);
		%prev = %ai.swingPrev[0];
		%sub = vectorNormalize(vectorSub(%chest,getWords(%pos,0,1) SPC 0));
		%dot = vectorDot(%forward,%sub);
		%time = mFloor((getSimTime()-%pl.swingStartTime)/4);
		//if(%dot < -0.7)
		//{
			if(%prev $= "")
				%prev = %pos;
			%q = vectorSub(%pos,%ppos);
			%q = getWord(%q,0) SPC (getWord(%q,1)*-1) SPC getWord(%q,2);
			$SwolMelee_SwingData[$SwolMelee_SwingDataCnt] = %time SPC %q SPC %up;
			$SwolMelee_SwingDataCnt++;
			//SM_Debug.drawLine(%prev,%pos,"1 0 0").schedule(500,delete);
		//}
		//else
		//{
		//	$SwolMelee_SwingData[$SwolMelee_SwingDataCnt] = %time;
		//	$SwolMelee_SwingDataCnt++;
		//}
		
		%ai.swingPrev[0] = %pos;
		
		
		%pl.swingCheckSched = schedule(32,%pl,generateSwingData,%pl,%anim,%c,1);
	}
}
function clearSwingData()
{
	$SwolMelee_SwingDataName = "";
	$SwolMelee_SwingDataCnt = 0;
}
function recordSwingData()
{
	%fp = new fileObject();
	%fp.openForWrite("config/7/melee/data/" @ $SwolMelee_SwingDataName @ "_record.txt");
	%c = 0;
	for(%i=0;%i<$SwolMelee_SwingDataCnt;%i++)
	{
		%data = $SwolMelee_SwingData[%i];
		if(%data $= "")
		{
			%c = 0;
			%fp.writeLine("");
			continue;
		}
		%fp.writeLine("$SwolMelee_SwingData[\"" @ $SwolMelee_SwingDataName @ "\"," @ %c @ "] = \"" @ %data @ "\";");
		%c++;
	}
	%fp.close();
	%fp.delete();
}
function swolMelee_stunPlayer(%pl,%bool,%time,%flag,%q)
{
	if(%bool && %pl.isStunned)
		return;
	%pl.isStunned = %bool;
	if(%bool)
	{
		if(%pl.getState() $= "Dead")
			return;
		cancel(%pl.unStunSched);
		if(!%q)
		{
			if(isObject(%pl.meleeHand))
			{
				%pl.stunWep = %pl.meleeHand.getMountedImage(0);
				swolMelee_UnMount(%pl);
			}
			else
			{
				if(isObject(%pl.getMountedImage(0)))
				{
					%pl.stunWep = %pl.getMountedImage(0);
					%pl.unMountImage(0);
				}
			}
			%pl.playThread(1,root);
			%p = %time/2000;
			if(%p > 1)
				%p = 1;
			%pl.setWhiteOut(0.2*%p);
		}
		if(%flag)
		{
			//%pl.setActionThread("sit",1);
			if(%pl.getDatablock() != nameToId(playerFrozen))
				%pl.tmpDb = %pl.getDatablock();
			%pl.setVelocity("0 0" SPC getWord(%pl.getVelocity(),2));
			if(!%pl.isTrainingDummy && !%q)
				%pl.setDatablock(playerFrozen);
			if(!%q)
				%pl.mountImage(swolMelee_stunBImage,3);
			if(isObject(%cl = %pl.client))
			{
				%cl.setControlObject(%cl.camera);
				%cl.camera.setMode("corpse",%pl);
				%cl.camera.setTransform(%pl.getTransform());
			}
		}
		else
		{
			if(!%q)
				%pl.mountImage(swolMelee_stunAImage,3);
		}
		%pl.disableCmdSwap = 1;
		%pl.unStunSched = schedule(%time,%pl,swolMelee_stunPlayer,%pl,0,%flag,%flagB);
	}
	else
	{
		if(%pl.getMountedImage(3) && strPos(%pl.getMountedImage(3).getName(),"stun") != -1)
			%pl.unMountImage(3);
		if(%pl.getState() !$= "Dead")
		{
			if(%pl.tmpDb !$= "")
			{
				if(isObject(%cl = %pl.client))
					%cl.setControlObject(%pl);
				%pl.setDatablock(%pl.tmpDb);
				%pl.tmpDb = "";
			}
			%pl.disableCmdSwap = 0;
			if(!%q)
			{
				if(%pl.stunWep.item.isSMelee)
				{
					swolMelee_Mount(%pl,%pl.stunWep);
				}
				else
				{
					%pl.mountImage(%pl.stunWep,0);
					if(%pl.stunWep.armReady)
						%pl.playThread(1,armReadyRight);
				}
				%pl.stunWep = "";
			}
		}
	}
}

function swolMelee_UnMount(%pl,%ai)
{
	if(!isObject(%pl))
		return;
	if(!isObject(%ai))
		if(!isObject(%ai = %pl.meleeHand))
			return;
	//talk(%ai.isNodeVisible("RHand"));
	%pl.unHideNode((%ai.isNodeVisible("RHand") ? "RHand" : "RHook"));
	if(%ai.getMountedImage(0).item.smTwoHanded)
		%pl.unHideNode((%ai.isNodeVisible("LHand") ? "LHand" : "LHook"));
	cancel(%pl.meleeSwingResetAnimA);
	cancel(%pl.meleeSwingResetAnimB);
	%ai.isDeleting = 1;
	%ai.schedule(1,delete);
}
function swolMelee_Mount(%pl,%db)
{
	if(!isObject(%ai = %pl.meleeHand) || %pl.meleeHand.isDeleting)
	{
		%ai = new aiPlayer()
		{
			datablock = (%db.item.uiName $= "Fists" ? meleeFistsPlayer : meleeAnimPlayer);
			position = %pl.getPosition();
		};
		%ai.setDamageLevel(100);
		swolMelee_updateLook(%pl,1);
		%pl.mountObject(%ai,0);
		%pl.meleeHand = %ai;
	}
	if(%db.item.smTwoHanded)
	{
		%pl.playThread(1,armReadyBoth);
		%ai.playThread(1,rootB);
	}
	else
	{
		%ai.playThread(0,root);
		%pl.playThread(1,root);
	}
	%ai.mountImage(%db,0);
	schedule(1,%pl,swolMelee_updateLook,%pl,1);
}
function swolMelee_updateLook(%pl,%force)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	%item = %ai.getMountedImage(0).item;
	%tmp = (%pl.isNodeVisible("RHand") ? "RHand" : "RHook");
	if(!%ai.isNodeVisible(%tmp) || %force)
		%doUpdate = %tmp;
	if(%item.smTwoHanded)
	{
		%tmp = (%pl.isNodeVisible("LHand") ? "LHand" : "LHook");
		if(!%ai.isNodeVisible(%tmp) || %force)
			%doUpdateL = %tmp;
	}
	if(%doUpdate @ %doUpdateL !$= "")
	{
		%ai.hideNode("ALL");
		if(%doUpdate !$= "")
			%ai.unHideNode(%doUpdate);
		if(%doUpdateL !$= "")
			%ai.unHideNode(%doUpdateL);
	}
	if(%pl.rHandColor !$= "")
		%colUpdate = %pl.rHandColor;
	else if(isObject(%cl = %pl.client))
		%colUpdate = %cl.rHandColor;
	
	if(%item.smTwoHanded)
	{
		if(%pl.lHandColor !$= "")
			%colUpdateL = %pl.lHandColor;
		else if(isObject(%cl = %pl.client))
			%colUpdateL = %cl.lHandColor;
	}
		
	
	if(%colUpdate @ %colUpdateL !$= "")
	{
		if(%ai.currColorR !$= %colUpdate)
		{
			%ai.setNodeColor("RHand",%colUpdate);
			%ai.setNodeColor("RHook",%colUpdate);
			%ai.currColorR = %colUpdate;
		}
		if(%colUpdateL !$= "")
		{
			if(%ai.currColorL !$= %colUpdateL)
			{
				%ai.setNodeColor("LHand",%colUpdateL);
				%ai.setNodeColor("LHook",%colUpdateL);
				%ai.currColorL = %colUpdateL;
			}
		}
	}
	%pl.hideNode("RHand");
	%pl.hideNode("RHook");
	if(%item.smTwoHanded)
	{
		%pl.hideNode("LHand");
		%pl.hideNode("LHook");
	}
	if(isObject(%cl))
	{
		if(%item.uiName $= "Fists")
		{
			if(%cl.hat == 0)
			{
				%col = %cl.hatColor;
				if(%col !$= "")
				{
					%ai.unHideNode("RGlove");
					%ai.unHideNode("LGlove");
					%ai.setNodeColor("RGlove",%col);
					%ai.setNodeColor("LGlove",%col);
				}
			}
		}
	}
}
function swolMelee_doKick(%pl,%stage)
{
	cancel(%pl.kickSched);
	if(%pl.getState() $= "Dead")
		return;
	if(!isObject(%pl.meleeHand))
		return;
	if(%stage == 0)
	{
		if(getSimTime()-%pl.lastMeleeSwing > %item.smSwingDelay)
		{
			%pl.playThread(2,walk);
			%pl.playThread(3,plant);
			swolMelee_stunPlayer(%pl,1,300,1,1);
			%pl.kickSched = schedule(50,%pl,swolMelee_doKick,%pl,1);
			%pl.lastMeleeSwing = getSimTime()+800;
		}
	}
	else if(%stage == 1)
	{
		%pl.stopThread(2);
		%pl.kickSched = schedule(300,%pl,swolMelee_doKick,%pl,2);
		serverPlay3D(swolMelee_getSFX("foot_swing"),%pl.getPosition());
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::fxBrickObjectType | $TypeMasks::StaticObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType;
		%pos = vectorAdd(%pl.getPosition(),"0 0 0.3");
		%eye = %pl.getEyeVectorHack();
		%item = %pl.meleeHand.getMountedImage(0).item;
		%ray = containerRayCast(%pos,vectorAdd(%pos,vectorScale(%eye,2.2)),%mask,%pl,%pl.meleeHand);
		if(%ray)
		{
			%obj = getWord(%ray,0);
			if(%obj.getType() & $TypeMasks::PlayerObjectType)
			{
				serverPlay3D(swolMelee_getSFX("foot_hitPl"),%pl.getPosition());
				%exdb = meleeBloodHitProjectile;
				if(%obj.fistCombo[%pl] >= 2)
				{
					%force = 18;
					%dmg = 60;
				}
				else
				{
					%force = 10;
					%dmg = 30;
				}
				if(minigameCanDamage(%pl.client,%obj) == 1)
				{
					%obj.damage(%pl.client,%obj.getPosition(),%dmg*(1+%pl.steroidlevel),%item.smDamageType);
					%obj.addVelocity(vectorAdd(vectorScale(%eye,%force),"0 0 4"));
				}
			}
			else
			{
				serverPlay3D(swolMelee_getSFX("fist_hitEnv"),%pl.getPosition());
				%exdb = meleeGenericHitProjectile;
			}
			%p = new projectile()
			{
				datablock = %exdb;
				initialPosition = getWords(%ray,1,3);
				scale = "0.8 0.8 0.8";
			};
			%p.explode();
		}
	}
	else if(%stage == 2)
	{
		%pl.playThread(2,root);
	}
}
function swolMelee_iniSwingCheck(%pl)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	for(%i=0;%i<8;%i++)
	{
		%ai.swingPrev[%i] = "";
	}
	%swingWindup = $SwolMelee_AnimWindup[%pl.lastSwingAnim];
	swolMelee_playSwingSound(%pl);
	%pl.swingCheckEnd = getSimTime()+240;
	%item = %ai.getMountedImage(0).item;
	if(%item.uiName $= "Fists")
		%pl.swingCheckEnd = getSimTime()+180;
	%pl.swingStart = getSimTime();
	%pl.swingStep = 0;
	schedule(%swingWindup,%ai,swolMelee_swingCheck,%pl);
}
function swolMelee_playSwingSound(%pl)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	%item = %ai.getMountedImage(0).item;
	if(%item.smSwingSound !$= "")
		serverPlay3D(swolMelee_getSFX(%item.smSwingSound),%pl.getPosition());
}
function swolMelee_doBlock(%pl)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	
	%item = %ai.getMountedImage(0).item;
	if(%item.smTwoHanded)
		return;
	if(getSimTime()-%pl.lastMeleeSwing > %item.smSwingDelay && getSimTime()-%pl.lastMeleeBlock > 1500)
	{
		%pl.lastMeleeBlock = getSimTime();
		%pl.playThread(1,armReadyRight);
		%ai.playThread(1,block);
		cancel(%pl.meleeSwingEndSched);
		%pl.meleeSwingEndSched = schedule(500,%ai,swolMelee_swingEnd,%pl);
	}
}
function swolMelee_debugSwingLoop(%pl)
{
	if(isObject(%ai = %pl.meleeHand))
		swolMelee_doSwing(%pl);
	schedule(500,%pl,swolMelee_debugSwingLoop,%pl);
}
function swolMelee_doParry(%pl)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	cancel(%pl.meleeSwingEndSched);
	%pl.playThread(2,plant);
	%ai.playThread(1,parry);
	%pl.lastMeleeBlock = getSimTime()-1500;
	%pl.lastMeleeSwing = 0;
	%pl.meleeSwingEndSched = schedule(500,%ai,swolMelee_swingEnd,%pl);
}
function swolMelee_swingEnd(%pl)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
		
	if(%ai.getMountedImage(0).item.smTwoHanded)
	{
		//%pl.playThread(1,armReadyRight);
	}
	else
	{
		%pl.meleeSwingResetAnimA = %pl.schedule(100,playThread,1,root);
		%pl.meleeSwingResetAnimB = %pl.schedule(100,playThread,2,root);
		%ai.playThread(1,root);
		%pl.playThread(2,shiftTo);
	}
}

function swolMelee_swingCheck(%pl) //GOOD
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	//if(getSimTime()-%pl.swingCheckEnd > 0)
	//{
	//	%pl.meleeSwingEndSched = schedule(500,%ai,swolMelee_swingEnd,%pl);
	//	return;
	//}
	if(getSimTime()-%pl.lastGroundHit < 300)
	{
		%pl.meleeSwingEndSched = schedule(500,%ai,swolMelee_swingEnd,%pl);
		return;
	}
	cancel(%pl.swingCheckSched);
	%item = %ai.getMountedImage(0).item;
	%frame = $SwolMelee_SwingData[%pl.lastSwingAnim,%pl.swingStep*4];
	if(%frame $= "")
	{
		%pl.meleeSwingEndSched = schedule(500,%ai,swolMelee_swingEnd,%pl);
		return;
	}
	%forward = %pl.getEyeVectorHack();
	%up = %pl.getUpVectorHack();
	
	//%up = %ai.getUpVectorHack();
	//%forward = %ai.getEyeVectorHack();
	%ppos = %pl.getPosition();
	//%chest = getWords(%ppos,0,1) SPC 0;
	
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::fxBrickObjectType | $TypeMasks::StaticObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType;
	
	%upBase = "0 0 1";
	%forwardBase = "0 1 0";
	%rightBase = vectorCross(%forwardBase,%upBase);
	%time = getWord(%frame,0);
	%fV = getWords(%frame,1,3);
	%fVX = getWord(%fV,0);
	%fVY = getWord(%fV,1)*-1;
	%fVZ = getWord(%fV,2);
	%right = vectorCross(%forward,%up);
	%fV = vectorAngleRotate(%fVX SPC %fVY SPC %fVZ,vectorNormalize("-1 1 0"),-($PI/2));
	//%mirr = "1 0 0";
	//%fV = vectorSub(%fv,vectorScale(vectorScale(vectorDot(%fv,%mirr),%mirr),-2));
	//%fNorm = vectorNormalize(%fv);
	%upV = getWords(%frame,4,6);
	
	//%fV = vectorAngleRotate(vectorAngleRotate(%fV,%upBase,%forward),%forwardBase,%up);
	//%upV = vectorAngleRotate(vectorAngleRotate(%upV,%forwardBase,%forward),%upBase,%up);
	
	%zBase = vectorRelativeShift(%forward,%up,%upV);
	%posBase = vectorRelativeShift(vectorScale(%forward,1),%up,%fV);
	%pos = vectorAdd(%ppos,%posBase);
	
	%len = %item.smLength;
	for(%i=%len;%i>=0;%i--)
	{
		%lPos = vectorAdd(%pos,vectorScale(%zBase,0.5*%i));
		%prev = %ai.swingPrev[%i];
		if(%prev $= "")
			%prev = %lPos;
			
		%ray = containerRayCast(%prev,%lPos,%mask,%pl,%ai);
		if(%ray)
		{
			%hit = getWord(%ray,0);
			if(%x = (getSimTime()-%hit.lastHitBy[%pl] > 200))
			{
				%hit.currHitChain[%pl] = %i;
			}
			if(%i == %hit.currHitChain[%pl])
				swolMelee_hitObj(%pl,%hit,getWords(%ray,1,3),%x,%lPos);
		}
		SM_Debug.drawLine(%prev,%lPos,"1 0 0").schedule(500,delete);
		
		%ai.swingPrev[%i] = %lPos;
	}
	%pl.swingStep++;
	%pl.swingCheckSched = schedule(32,%ai,swolMelee_swingCheck,%pl);
}
function swolMelee_swingCheck(%pl)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	if(getSimTime()-%pl.swingCheckEnd > 0)
	{
		%pl.meleeSwingEndSched = schedule(500,%ai,swolMelee_swingEnd,%pl);
		return;
	}
	if(getSimTime()-%pl.lastGroundHit < 200)
	{
		%pl.meleeSwingEndSched = schedule(500,%ai,swolMelee_swingEnd,%pl);
		return;
	}
	cancel(%pl.swingCheckSched);
	%item = %ai.getMountedImage(0).item;
	
	%slot = 0;
	if(%isFists = (%pl.lastSwingAnim $= "F"))
	{
		%slot = (!%pl.lastFistSwing)+1;
	}
	else
	{
		
		%up = MatrixMulVector(%ai.getSlotTransform(%slot),"0 0 1");
	}
	//%forward = MatrixMulVector(%ai.getSlotTransform(0),"0 1 0");
	
	%forward = %pl.getEyeVectorHack();
	
	//%up = %ai.getUpVectorHack();
	//%forward = %ai.getEyeVectorHack();
	%chest = vectorAdd(%pl.getPosition(),"0 0 1");
	%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::fxBrickObjectType | $TypeMasks::StaticObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType;
	%posBase = %ai.getSlotTransform(%slot);
	for(%i=%item.smLength;%i>=0;%i--)
	{
		%relativeShift = vectorRelativeShift(%forward,%up,(%isFists ? (0.5*%i) SPC "0 0" : "0 0" SPC (0.5*%i)));
		%pos = vectorAdd(%posBase,%relativeShift);
		%prev = %ai.swingPrev[%i];
		%sub = vectorNormalize(vectorSub(%pos,%chest));
		
		%dot = vectorDot(%sub,%forward);
		if(mAbs(%dot) < 0.1)
			continue;
		//%dot = vectorDot(%eyeVec,vectorNormalize(vectorSub(getWords(%pos,0,1) SPC 0,%chest)));
		//if(%dot < 0.8)
		//	continue;
		if(%prev $= "")
			%prev = %pos;
			
		%ray = containerRayCast(%prev,%pos,%mask,%pl,%ai);
		if(%ray)
		{
			%hit = getWord(%ray,0);
			if(%x = (getSimTime()-%hit.lastHitBy[%pl] > 150))
			{
				%hit.currHitChain[%pl] = %i;
			}
			if(%i == %hit.currHitChain[%pl])
				swolMelee_hitObj(%pl,%hit,getWords(%ray,1,3),%x,%pos);
		}
		//SM_Debug.drawLine(%prev,%pos,"1 0 0").schedule(500,delete);
		
		%ai.swingPrev[%i] = %pos;
	}
	%pl.swingCheckSched = schedule(32,%ai,swolMelee_swingCheck,%pl);
}
function swolMelee_clearFistCombo(%pl,%hit)
{
	cancel(%hit.fistComboSched);
	%hit.fistCombo[%pl] = "";
}
function player::onMeleeHit_kurgan(%pl,%attacker,%pos)
{
	serverPlay3D(thereCanOnlyBeOneSound,%pos);
}
function aiPlayer::onMeleeHit_kurgan(%pl,%attacker,%pos)
{
	player::onMeleeHit_kurgan(%pl,%attacker,%pos);
}
function swolMelee_hitObj(%pl,%hit,%hitPos,%firstHit,%hitFrom)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	if(getSimTime()-%pl.disregardMeleeSwing < 500)
		return;
	%hit.lastHitBy[%pl] = getSimTime();
	%item = %ai.getMountedImage(0).item;
	%exScale = "1 1 1";
	if(%hit.getType() & $TypeMasks::PlayerObjectType)
	{
		%subTo = vectorNormalize(vectorSub(vectorAdd(%pl.getPosition(),"0 0 1"),vectorAdd(%hit.getPosition(),"0 0 1")));
		if(%firstHit)
		{
			if(getSimTime()-%hit.lastMeleeBlock < 300 && isObject(%hit.meleeHand) && %pl.lastSwingAnim !$= "F")
			{
				//SM_Debug.drawLine(vectorAdd(%hit.getPosition(),"0 0 1"),vectorAdd(%pl.getPosition(),"0 0 1"),"1 1 0");
				%dot = vectorDot(%subTo,%hit.getEyeVectorHack());
				if(%dot > 0.2)
				{
					%oA = %hit.meleeHand.getMountedImage(0).item.smMaterial;
					%oB = %item.smMaterial;
					serverPlay3D(swolMelee_getSFX(%oA @ "_" @ %oB @ "_Parry"),%hitFrom);
					swolMelee_doParry(%hit);
					%ai.playThread(1,recoilA);
					%pl.playThread(2,plant);
	
					%pl.lastMeleeSwing = getSimTime()+500;
					%pl.disregardMeleeSwing = getSimTime();
					swolMelee_stunPlayer(%pl,1,1700,1);
					%p = new projectile()
					{
						datablock = wrenchProjectile;
						initialPosition = vectorAdd(vectorAdd(%hit.getPosition(),"0 0 1"),%subTo);
						scale = "1 1 1";
					};
					%p.explode();
					cancel(%pl.swingCheckSched);
					return;
				}
			}
			if(%pl.lastSwingAnim $= "F")
			{
				%hit.fistCombo[%pl]++;
				cancel(%hit.fistComboSched);
				%hit.fistComboSched = schedule(500,%hit,swolMelee_clearFistCombo,%pl,%hit);
			}
			if(%item.smHitPlSound !$= "")
				serverPlay3d(swolMelee_getSFX(%item.smHitPlSound),%hit.getPosition());
			if(minigameCanDamage(%pl.client,%hit) == 1)
			{
				if(%item.smForce > 0)
				{
					%hit.addVelocity(vectorAdd(vectorScale(%pl.getEyeVectorHack(),%item.smForce),"0 0 4"));
				}
				%scale = 1;
				if($Pref::Swol_Melee_DamageMod !$= "")
					%scale = $Pref::Swol_Melee_DamageMod;
				//%hit.damage((isObject(%pl.client) ? %pl.client : %pl),%hit.getPosition(),%item.smDamage*%scale,%item.smDamageType);
				%hit.damage(%pl,%hit.getPosition(),%item.smDamage*%scale*(1+%pl.steroidlevel),%item.smDamageType);
				if(%hit.getState() !$= "Dead")
				{
					if(%item.smStun > 0)
					{
						swolMelee_stunPlayer(%hit,1,%item.smStun);
					}
				}
				if(isFunction(%hcn = %hit.getClassName(),"onMeleeHit"))
				{
					if(%hcn $= "Player")
						player::onMeleeHit(%hit,%pl,%hitPos);
					else if(%hcn $= "AiPlayer")
						aiPlayer::onMeleeHit(%hit,%pl,%hitPos);
					else
						eval(%hcn @ "::onMeleeHit(" @ (%hit|0) @ "," @ (%pl|0) @ ",\"" @ (getWord(%hitPos,0)|0) SPC (getWord(%hitPos,1)|0) SPC (getWord(%hitPos,2)|0) @ "\");");
				}
				if(isFunction(%hcn = %hit.getClassName(),%fu = ("onMeleeHit_" @ getSubStr(%item,0,strLen(%item)-4))))
				{
					eval(%hcn @ "::" @ %fu @ "(" @ (%hit|0) @ "," @ (%pl|0) @ ",\"" @ (getWord(%hitPos,0)|0) SPC (getWord(%hitPos,1)|0) SPC (getWord(%hitPos,2)|0) @ "\");");
				}
			}
		}
		if(%item.uiName $= "Fists")
			%exScale = 0.6;
		else if(%item.smTwoHanded && %item.smDamage > 60)
			%exScale = 2.5;
		else
			%exScale = 1.6;
		%exdb = meleeBloodHitProjectile;
		if(%item.mHitPlExplosion !$= "")
			%exdb = %item.mHitPlExplosion;
		
		if(%item.smDecapitate && %hit.getState() $= "Dead")
		{
			%bodyHit = swol_calculateDamagePosition(%hit,%hitPos);
			if(%bodyHit $= "head")
			{
				if(%hit.isNodeVisible("headskin"))
				{
					%hit.headExplosion();
				}
			}
		}
	}
	else
	{
		if(%firstHit)
		{
			if(getSimTime()-%pl.lastGroundHit > 100)
			{
				if(isFunction(%hit.getClassName(),"getDatablock"))
				{
					%hdb = %hit.getDatablock();
					if(isFunction(%hdb.getClassName(),"onMeleeHit"))
						%hdb.onMeleeHit(%hit,%pl,%hitPos);
				}
				if(isFunction(%hcn = %hit.getClassName(),"onMeleeHit"))
				{
					eval(%hcn @ "::onMeleeHit(" @ (%hit|0) @ "," @ (%pl|0) @ ",\"" @ (getWord(%hitPos,0)|0) SPC (getWord(%hitPos,1)|0) SPC (getWord(%hitPos,2)|0) @ "\");");
				}
				if(%item.smHitEnvSound !$= "")
					serverPlay3d(swolMelee_getSFX(%item.smHitEnvSound),%hitPos);
				
				%exdb = meleeGenericHitProjectile;
				if(%item.mHitExplosion !$= "")
					%exdb = %item.mHitExplosion;
				if(%pl.lastSwingAnim $= "A")
					%ai.playThread(1,recoilA);
				%pl.playThread(2,plant);
				%pl.lastGroundHit = getSimTime();
			}
		}
	}
	if(isObject(%exdb))
	{
		if(%subTo $= "")
			%initial = %hitPos;
		else
			%initial = vectorAdd(vectorScale(%subTo,0.4),getWords(%hit.getPosition(),0,1) SPC getWord(%hitPos,2));
		%p = new projectile()
		{
			datablock = %exdb;
			initialPosition = %initial;
			scale = %exScale SPC %exScale SPC %exScale;
		};
		%p.explode();
	}
}
function swolMelee_charge(%pl,%flag)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	%item = %ai.getMountedImage(0).item;
	if(%flag)
	{
		%pl.chargeReady = 1;
		%pl.chargeReadyTime = getSimTime();
		if(%pl.chargeRelease)
		{
			
			swolMelee_chargeRelease(%pl);
		}
		else
		{
			%pl.chargeReleaseAutoSched = schedule(700,%ai,swolMelee_chargeRelease,%pl,1);
		}
	}
	if(getSimTime()-%pl.lastMeleeSwing < %item.smSwingDelay)
		return;
	%pl.lastMeleeSwing = getSimTime();
	%pl.chargeRelease = 0;
	%pl.chargeStart = getSimTime();
	%swingType = %item.smSwingAnim;
		
	%ai.playThread(1,"ch" @ $SwolMelee_Anim[%swingType]);
	schedule(400,%ai,swolMelee_charge,%pl,1);
}
function swolMelee_chargeRelease(%pl,%flag)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	%item = %ai.getMountedImage(0).item;
	if(!%item.smTwoHanded)
		return;
	cancel(%pl.chargeReleaseAutoSched);
	%pl.meleeFullCharge = %flag;
	if(%flag)
		%pl.chargeReady = 1;
	if(%pl.chargeReady == 1)
	{
		%time = getSimTime()-%pl.chargeReadyTime;
		//%pl.playThread(2,plant);
		%pl.lastMeleeSwing = getSimTime();
		
		%swingType = %item.smSwingAnim;
		
		%ai.playThread(1,$SwolMelee_Anim[%swingType]);
		%pl.lastSwingAnim = %swingType;
		swolMelee_iniSwingCheck(%pl);
		%pl.chargeReady = 2;
	}
	else if(!%pl.chargeRelease)
	{
		%pl.chargeRelease = 1;
	}
}
function swolMelee_doSwing(%pl)
{
	if(!isObject(%ai = %pl.meleeHand))
		return;
	
	%item = %ai.getMountedImage(0).item;
	if(%item.smTwoHanded && %item.uiName !$= "Fists")
	{
		swolMelee_charge(%pl);
		return;
	}
	if((%tmp = getSimTime()-%pl.lastMeleeSwing) > %item.smSwingDelay && getSimTime()-%pl.lastMeleeBlock > 1500)
	{
		%pl.lastMeleeSwing = getSimTime();
		
		//SM_Debug.clear();
		
		%pl.playThread(2,plant);
		
		%swingType = %item.smSwingAnim;
		if(%swingType $= "F")
		{
			%pl.lastFistSwing = !%pl.lastFistSwing;
			%ai.playThread(1,"punch" @ (%pl.lastFistSwing ? "R" : "L"));
		}
		else
		{
			%ai.playThread(1,swing @ %swingType);
		}
		%pl.lastSwingAnim = %swingType;
		
		swolMelee_iniSwingCheck(%pl);
		if(!%item.smTwoHanded)
			if(!isEventPending(%pl.meleeSwingResetAnimA))
				%pl.playThread(1,armReadyRight);
		cancel(%pl.meleeSwingEndSched);
		cancel(%pl.meleeSwingResetAnimA);
	}
}
