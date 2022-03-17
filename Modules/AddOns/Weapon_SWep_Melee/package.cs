package swol_melee_v2
{
	function observer::onTrigger(%db,%cam,%slot,%bool)
	{
		if(isObject(%cl = %cam.getControllingClient()))
		{
			if(isObject(%pl = %cl.player))
			{
				if(%pl.isStunned)
				{
					return;
				}
			}
		}
		parent::onTrigger(%db,%cam,%slot,%bool);
	}
	function weapon::onUse(%db,%pl,%slot)
	{
		if(%db.isSMelee)
		{
			%im = %pl.getMountedImage(0);
			%pl.unMountImage(0);
			if(%im.armReady)
				%pl.playThread(1,root);
		}
		else
		{
			if(isObject(%pl.meleeHand))
				swolMelee_UnMount(%pl);
		}
		parent::onUse(%db,%pl,%slot);
	}
	
	function serverCmdDropTool(%cl,%tool)
	{
		if(isObject(%pl = %cl.player))
			if(%tool == %pl.currTool)
				if(%pl.tool[%pl.currTool].isSMelee)
					if(isObject(%pl.meleeHand))
						swolMelee_UnMount(%pl);
		parent::serverCmdDropTool(%cl,%tool);
	}
	function player::mountImage(%pl,%im,%slot)
	{
		if(!%pl.getDatablock().isEmptyPlayer)
		{
			if(%slot == 0)
			{
				if(%im.item.isSMelee)
				{
					swolMelee_Mount(%pl,%im);
					return;
				}
			}
		}
		return parent::mountImage(%pl,%im,%slot);
	}
	function player::unMountImage(%pl,%slot)
	{
		if(%slot == 0)
		{
			if(isObject(%pl.meleeHand))
				swolMelee_UnMount(%pl);
		}
		return parent::unMountImage(%pl,%slot);
	}
	function serverCmdUnUseTool(%cl)
	{
		if(getSimTime()-%cl.lastUseToolCmdTime < 150)
			return;
		parent::serverCmdUnUseTool(%cl,%tool);
	}
	function serverCmdUseTool(%cl,%tool)
	{
		if(!isObject(%pl = %cl.player))
			return;
		if(%pl.disableCmdSwap)
		{
			%pl.selTool = %tool;
			return;
		}
		%cl.lastUseToolCmdTime = getSimTime();
		parent::serverCmdUseTool(%cl,%tool);
	}
	function gameConnection::applyBodyParts(%cl,%o)
	{
		%ret = parent::applyBodyParts(%cl,%o);
		if(isObject(%pl = %cl.player))
		{
			if(isObject(%pl.meleeHand))
			{
				swolMelee_updateLook(%pl);
			}
		}
		return %ret;
	}
	function player::removeBody(%pl,%x)
	{
		if(%pl.getDatablock().isEmptyPlayer)
			return;
		return parent::removeBody(%pl,%x);
	}
	function armor::onTrigger(%db,%pl,%slot,%bool)
	{
		if(%bool)
		{
			if(!%slot)
			{
				if(isObject(%pl.meleeHand))
				{
					swolMelee_doSwing(%pl);
					return;
				}
			}
			else if(%slot == 4)
			{
				if(isObject(%pl.meleeHand))
				{
					if(%pl.meleeHand.getMountedImage(0).item.uiName $= "Fists")
					{
						swolMelee_doKick(%pl);
					}
					else
					{
						swolMelee_doBlock(%pl);
					}
					parent::onTrigger(%db,%pl,%slot,%bool);
					return;
				}
			}
		}
		else
		{
			if(!%slot)
			{
				if(isObject(%pl.meleeHand))
				{
					swolMelee_chargeRelease(%pl);
					return;
				}
			}
		}
		parent::onTrigger(%db,%pl,%slot,%bool);
	}
	function armor::onMount(%db,%pl,%obj,%node)
	{
		if(%db.isEmptyPlayer)
		{
			%pl.setTransform("0 0 0 0 0 0 0");
			return;
		}
		return parent::onMount(%db,%pl,%obj,%node);
	}
	function armor::onUnMount(%db,%pl,%obj,%node)
	{
		if(%db.deleteOnDrop)
		{
			if(%db == nameToId("meleeAnimPlayer") || %db == nameToId("meleeFistsPlayer"))
			{
				swolMelee_UnMount(%obj,%pl);
			}
			else
			{
				%pl.schedule(1,delete);
			}
			return parent::onUnMount(%db,%pl,%obj,%node);
		}
		return parent::onUnMount(%db,%pl,%obj,%node);
	}
	function player::setNodeColor(%pl,%node,%col)
	{
		parent::setNodeColor(%pl,%node,%col);
		if(%node $= "RHand" || %node $= "RHook")
			%pl.rHandColor = %col;
		else if(%node $= "LHand" || %node $= "LHook")
			%pl.lHandColor = %col;
	}
};
activatePackage(swol_melee_v2);