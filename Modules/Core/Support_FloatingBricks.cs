// server.cs
// support for floating bricks

// RTB doesn't exist, assign default values
// if you don't have RTB, change these in console and they will save automatically upon exit
$Pref::FloatingBricks::Enabled = false;
$Pref::FloatingBricks::AdminOnly = false;
$Pref::FloatingBricks::Timeout = 0;

// audio files
datablock AudioProfile(FloatingPlantSound)
{
	filename = "./sounds/floatPlant.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(FloatingPlantErrorSound)
{
	filename = "./sounds/floatError.wav";
	description = AudioClose3d;
	preload = true;
};

// (modified) checkPlantingError from jes00 (thanks)
// @param fxDTSBrick %this   :   the brick to check
function fxDTSBrick::checkPlantingError(%this)
{
	// make sure brick isn't actually a real brick
	if(%this.isPlanted())
	{
		error("You must use a temp brick. Not a brick!");
		return -1;
	}

	// make a new "brick" to check the error that it returns on plant
	%brick = new fxDTSBrick()
	{
		datablock = %this.getDatablock();
		position = %this.getPosition();
		rotation = %this.rotation;
		angleID = %this.angleID;
	};

	// plant and delete the brick to check the error
	%error = %brick.plant();
	%brick.delete();
	
	// set the error
	switch$(%error)
	{
		case 0: 
			%type = 0;
			
		case 1: 
			%type = "overlap";
			
		case 2: 
			%type = "float";
			
		case 3: 
			%type = "stuck";
			
		case 4: 
			%type = "unstable";
			
		case 5: 
			%type = "buried";
			
		default: 
			%type = "forbidden";
	}
	
	return %type;
}

// core package
package FloatingBricks
{
	// @param GameConnection %this   :   the client using the command 
	function serverCmdPlantBrick(%this)
	{
		%r = Parent::serverCmdPlantBrick(%this);
		
		// check if floating bricks are enabled
		// check if player exists
		// check if player's ghost brick exists
		// check if the brick error is float
		if(!$Pref::FloatingBricks::Enabled || !isObject(%player = %this.player) || !isObject(%ghost = %player.tempBrick) || %ghost.checkPlantingError() !$= "float")
			return %r;

		%admin = %this.isAdmin;
		
		// check for admin pref & admin status
		if($Pref::FloatingBricks::AdminOnly && !%admin)
		{
			messageClient(%this, '', "\c0You are not an \c3Admin\c0.");
			
			return %r;
		}
		
		// check for timeout (admins are exempt from timeout restriction)
		if(!%admin && $Pref::FloatingBricks::Timeout > 0)
		{
			// convert seconds to milliseconds
			%milliseconds = $Pref::FloatingBricks::Timeout * 1000;
			
			if(getSimTime() - %player.floatingBrickTime < %milliseconds)
			{
				messageClient(%this, 'MsgPlantError_Flood');
				
				// convert time remaining to seconds
				%timeout = mCeil((%milliseconds - (getSimTime() - %player.floatingBrickTime)) / 1000);
				%timeout = %timeout @ "\c3" @ (%timeout > 1 ? " seconds" : " second");	
				
				commandtoClient(%this, 'CenterPrint', "\c0You must wait \c3" @ %timeout @ "\c0 before planting a floating brick again.", 3);
				
				return %r;
			}
		}

		%brick = new fxDTSBrick()
		{
			client = %this;
			position = %ghost.getPosition();
			dataBlock = %ghost.getDatablock().getName();
			angleID = %ghost.angleID;
			rotation = %ghost.rotation;
			printID = %ghost.printID;
			colorID = %ghost.colorID;
			colorFX = 0;
			shapeFX = 0;
			isPlanted = true;
			isFloatingBrick = true;
		};
		
		// add the brick to the player's brickgroup
		%group = nameToID("BrickGroup_" @ %this.bl_id);
		%group.add(%brick);
		
		%brick.plant();
		
		// not sure if this is required
		%brick.setTrusted(true);
		
		// add the brick to their undo stack and set their timeout
		%this.undoStack.push(%brick TAB "PLANT");
		%player.floatingBrickTime = getSimTime();
	}
	
	// @param fxDTSBrickData %this   :   the brick datablock that was planted
	// @param fxDTSBrick %brick   :   the brick object that was planted
	function fxDTSBrickData::onPlant(%this, %brick)
	{
		Parent::onPlant(%this, %brick);
		
		if(%brick.isFloatingBrick)
			%brick.isBaseplate = true;
	}
	
	// brick loading fix by zapk
	// @param fxDTSBrick %this   :   the brick to plant (called before onLoadPlant)
	function fxDTSBrick::plant(%this)
	{
		%this.forceBaseplate = %this.isBaseplate;
		
		return Parent::plant(%this);
	}

	// brick loading fix by zapk
	// @param fxDTSBrick %this   :   the brick that was loaded
	function fxDTSBrick::onLoadPlant(%this)
	{
		if(%this.forceBaseplate)
			%this.isBaseplate = true;

		Parent::onLoadPlant(%this);
	}
	
	// brick loading fix by zapk
	function serverLoadSaveFile_End()
	{	
		// loop through all of the brick groups
		for(%i = 0; %i < MainBrickGroup.getCount(); %i++)
		{
			%group = MainBrickGroup.getObject(%i);

			// loop through all of the bricks in that group
			for(%j = 0; %j < %group.getCount(); %j++)
			{
				%brick = %group.getObject(%j);
				
				// check if brick is floating
				if(%group.bl_id == 888888 || %brick.getNumDownBricks() == 0)
				{
					%brick.isFloatingBrick = true;
					%brick.isBaseplate = true;
					
					// fixes a strange bug
					if(%brick.getNumUpBricks() != 0)
						%brick.onToolBreak(); // wtf
				}
			}
		}
		
		return Parent::serverLoadSaveFile_End();
	}

	//// FLOATING BRICK SUPPORT ////
	function fxDTSBrick::killBrick(%this)
	{
		///Void function
		//"parent::killBrick(%this);" isn't necessary here, otherwise breaks new function below
		
		//Remove physical water zone, if added to (fill) stream brick
		if((%this.PTGStreamBr || %this.PTGStreamBrAux || %this.PTGStreamBrTert) && %this.PhysicalZone && isObject(%this.PhysicalZone))
		{
			%this.PhysicalZone.delete();
			%this.PhysicalZone = "";
		}

		//Check if brick is a generated detail, was planted by a player or is a stream source
		//Also, check if brick is not connected to the ground nor is already being destroyed using custom removal method ("!%this.PTGNullBr")
		if((%this.PTGDetailBr || %this.PTGPlayerBr || %this.getDataBlock().PTGStreamSrc) && !%this.PTGNullBr && !%this.hasPathToGround())
		{
			//If brick will destroy other bricks (otherwise use default function)
			if(%this.willCauseChainKill())
			{
				//don't check if chunk should be edited here, only for relative item funcs (some PTG functions use "killBrick" and "delete")
				
				//Custom removal of brick
				ServerPlay3D(brickBreakSound,%this.getPosition());
				%this.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3); //"0 0 8"
				%this.PTGNullBr = true; //PTGNullBr check is to prevent object from being constantly fake killed before it's actually deleted
				%this.scheduleNoQuota(500,delete);

				return;
			}
		}

		parent::killBrick(%this); 
		//return;
    }
	
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	 
	//// FLOATING BRICK SUPPORT ////
	function HammerImage::onHitObject(%this,%obj,%slot,%col,%d,%e)//,%f) //%f appears to be unused
	{
		//%parent = parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
		
		//"if(%col.willCauseChainKill" isn't necessary
		//%this = hammer
		//%obj = player or bot using hammer
		//%col = collision object / most of the time is a brick
		//%d = position of particle impact?
		//%e = angle of impact?
		
		//Prevent re-destroying same brick (while it's being removed using custom method)
		if((isObject(%col) && %col.PTGNullBrAux))
			return;

		//%BrTypePass = %col.PTGDetailBr || %col.PTGPlayerBr || %col.PTGStreamBrTert || %col.StreamBr || %col.StreamBrAux; // 
		%BrClPass = %col.getClassName() $= "fxDTSBrick";
		
		if(%BrClPass)
		{
			if(isObject(%downBr = %col.getDownBrick(0)))
				%downBrPTG = %downBr.PTGgenerated;
			if(isObject(%upBr = %col.getUpBrick(0)))
				%upBrPTG = %upBr.PTGgenerated;
		}
		
		//hammerHitSound doesn't play when the hammer is used on static shapes

		if(isObject(%ObjBrGroup = getBrickGroupFromObject(%col)))
			%ObjBrGroupisPub = %ObjBrGroup.getName() $= "BrickGroup_888888";
		
		//Use parent function only if col is not brick, if brick is attached to the ground or if a brick is planted above / below this brick (if not a detail)
			//don't return parent if brick is a boundary brick (to prevent being destroyed by hammer, unless allowed below)
		if(!isObject(%col) || !%BrClPass || (%col.hasPathToGround() && !col.PTGgenerated && (!%downBrPTG && !%upBrPTG))) //(!%BrTypePass || (%col.PTGPlayerBr && %col.hasPathToGround()) && !%col.ChBoundsPTG && !%col.PTGTerrainBr) || (!%col.PTGDetailBr && isObject(%col.getUpBrick(0)) && isObject(%col.getDownBrick(0))))
		{
			if(!%col.ChBoundsPTG)
				return parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
		}
		
		//////////////////////////////////////////////////
		
		//If object hit by hammer projectile is a brick
			//".PTGNullBr" check is to prevent object from being constantly hammered / fake-killed before it's actually deleted
	 	if(%brClPass && !%col.PTGNullBr && (!%downBr || !%upBr || !%col.PTGPlayerBr))
		{
			//%cl = %col.client; //this will get the client from the brick / object hammered
			%cl = %obj.client;

			if(!%col.PTGTerrainBr && !%col.ChBoundsPTG) //if(%BrTypePass)
			{
				//If brick was planted by player
				if(%col.PTGPlayerBr)
				{
					//If brick was planted by a player, saved to chunk file, then reloaded by generator
					if(%col.PTGGenerated)
					{
						//If brick is public
						if(%ObjBrGroupisPub)
						{
							ServerPlay3D(hammerHitSound,%col.getPosition());
							if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Player bricks (loaded from chunk saves) are under public ownership and can only be destroyed using the admin / desctructo wand",5);
							
							return;
						}
						
						//Otherwise, do trust check for loaded brick
						else if(isObject(%ObjBrGroup) && getTrustLevel(getBrickGroupFromObject(%cl),%ObjBrGroup) < 2)
						{
							%plyrN = %ObjBrGroup.name;
							%plyrID = %ObjBrGroup.bl_id;
							
							ServerPlay3D(hammerHitSound,%col.getPosition());
							if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0Insufficient Trust: <color:ffffff>\"" @ %plyrN @ "\" (ID:" @ %plyrID @ ") doesn't trust you enough to do that",5);
							return;
						}
					}
					
					//If brick was planted by a player
					else
					{
						%ClBrGroup = getBrickGroupFromObject(%cl);
						
						//Trust level check
						if(isObject(%ObjBrGroup) && getTrustLevel(%ClBrGroup,%ObjBrGroup) < 2)
						{
							%plyrN = %ObjBrGroup.name;
							%plyrID = %ObjBrGroup.bl_id;
							
							ServerPlay3D(hammerHitSound,%col.getPosition());
							if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0Insufficient Trust: <color:ffffff>\"" @ %plyrN @ "\" (ID:" @ %plyrID @ ") doesn't trust you enough to do that",5);
							
							return;
						}
					}
				}
				
				if(%col.PTGDetailBr)
				{
					//If option to allow destruction of details bricks under public ownership is disabled ("%obj.getClassName() $= "Player"" filters out bots / AIPlayer)
					if(!$PTG.DestroyPublicBr && %ObjBrGroupisPub)
					{
						ServerPlay3D(hammerHitSound,%col.getPosition());
						if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Detail bricks are under public ownership and can only be destroyed using the admin / desctructo wand",5);
						
						return;
					}
					
					//If detail bricks set to be destroyed by admin / desctructo wand only ("%obj.getClassName() $= "Player"" filters out bots / AIPlayer)
					if($PTG.PreventDestDetail)
					{
						ServerPlay3D(hammerHitSound,%col.getPosition());
						if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Detail bricks have been set to only be destroyed using the admin / desctructo wand",5);
						
						return;
					}
				}
				
				if(%col.PTGGenerated && %col.PTGPlayerBr && %ObjBrGroupisPub)
				{
					ServerPlay3D(hammerHitSound,%col.getPosition());
					if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Player bricks (loaded from chunk saves) are under public ownership and can only be destroyed using the admin / desctructo wand",5);
					
					return;
				}
				
				//Call onToolBreak method
				if(isObject(%ObjBrGroup) && isFunction(%col.onToolBreak(%col,%ObjBrGroup)))
					%col.onToolBreak(%col,%ObjBrGroup);
			
				//////////////////////////////////////////////////
			
				//If set up to set current chunk to "edited" when brick is hammered
				if(isObject(PTG_GlobalSO) && isObject(PTG_MainSO) && $PTG.chEditBrPPD && isObject(BrickGroup_Chunks))
				{
					%ChSize = mClamp($PTGm.chSize,16,256);
					%CHPosX = mFloor(getWord(%col.position,0) / %ChSize) * %ChSize;
					%CHPosY = mFloor(getWord(%col.position,1) / %ChSize) * %ChSize;
					%Chunk = "Chunk_" @ %CHPosX @ "_" @ %CHPosY;
					
					if(isObject(%Chunk))
						%Chunk.ChEditedPTG = true;
				}
				
				//Custom removal of brick
				ServerPlay3D(brickBreakSound,%col.getPosition());
				%col.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3); //"0 0 8"
				%col.PTGNullBr = true;
				%col.scheduleNoQuota(500,delete);
				
				//if brick is planted on top of a detail, remove that too (for details only - for easily clearing details from terrain for building)
				if(%col.PTGDetailBr && isObject(%upDetBr = %col.getUpBrick(0)))
				{
					//don't have to play "brickBreakSound" for this brick
					%upDetBr.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3); //"0 0 8"
					%upDetBr.PTGNullBr = true;
					%upDetBr.scheduleNoQuota(500,delete);
				}
				
				//return %parent;
			}
			
			//////////////////////////////////////////////////
			
			else
			{
				//If detail bricks set to be destroyed by admin / desctructo wand only
				if(%col.PTGTerrainBr)// && $PTG.PreventDestTerrain)
				{
					ServerPlay3D(hammerHitSound,%col.getPosition());
					
					if(isobject(%cl) && %obj.getClassName() $= "Player")
					{
						if(%ObjBrGroupisPub)
							commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Terrain bricks are under public ownership and can only be destroyed using the admin / desctructo wand",5);
						else
						{
							if($PTG.PreventDestTerrain)
								commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Terrain bricks have been set to only be destroyed using the admin / desctructo wand",5);
							else
								commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Terrain bricks can only be destroyed using the wand",5);
						}
					}
					return;
				}
				
				//If boundary bricks set to be destroyed by admin / desctructo wand only
				if(%col.ChBoundsPTG)// && $PTG.PreventDestBounds)
				{
					ServerPlay3D(hammerHitSound,%col.getPosition());
					
					if(isobject(%cl) && %obj.getClassName() $= "Player")
					{
						if(%ObjBrGroupisPub)
							commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Boundary bricks are under public ownership and can only be destroyed using the admin / desctructo wand",5);
						else
						{
							if($PTG.PreventDestBounds)
								commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Boundary bricks have been set to only be destroyed using the admin / desctructo wand",5);
							else
								commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Boundary bricks can only be destroyed using the wand",5);
						}
					}
					return;
				}
			}
		}
		else if(!%col.PTGNullBr)
			ServerPlay3D(hammerHitSound,%col.getPosition());

		//return parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
	 }
	 
	 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	 
	//// FLOATING BRICK SUPPORT ////
	function WandImage::onHitObject(%this,%obj,%slot,%col,%d,%e)//,%f) //%f appears to be unused
	{
		//%parent = parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
		
		//%this = wand
		//%obj = player / bot using wand(?)
		//%col = collision object / most of the time is a brick
		//%d = position of particle impact?
		//%e = angle of impact?
		
		//Prevent redestroying same brick (while it's being removed using custom method)
		if(isObject(%col) && %col.PTGNullBrAux)
			return;
		
		//%BrTypePass = %col.PTGDetailBr || %col.PTGPlayerBr || %col.PTGStreamBrTert || %col.StreamBr || %col.StreamBrAux;
		%BrClPass = %col.getClassName() $= "fxDTSBrick";
		
		if(%BrClPass)
		{
			if(isObject(%downBr = %col.getDownBrick(0)))
				%downBrPTG = %downBr.PTGgenerated;
			if(isObject(%upBr = %col.getUpBrick(0)))
				%upBrPTG = %upBr.PTGgenerated;
		}
		
		if(isObject(%ObjBrGroup = getBrickGroupFromObject(%col)))
			%ObjBrGroupisPub = %ObjBrGroup.getName() $= "BrickGroup_888888";
		
		//Use parent function only if col is not brick, or if brick is attached to the ground
		if(!isObject(%col) || !%BrClPass || (%col.hasPathToGround() && !%col.PTGgenerated && (!%downBrPTG && !%upBrPTG))) //%col.hasPathToGround()) // && !%BrTypePass && !%col.ChBoundsPTG && !%col.PTGTerrainBr))
		{
			if(!%col.ChBoundsPTG) //don't return parent if brick is a boundary brick (to prevent being destroyed by wand, unless allowed below)
				return parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
		}

		//////////////////////////////////////////////////
		
		//Clear inital brick using custom script (".PTGNullBrAux" is to prevent issues with ".PTGNullBr" set by another script before this one runs)
		if(%BrClPass && !%col.PTGNullBrAux)
		{
			//%cl = %col.client; //this will get the client from the brick / object hammered
			%cl = %obj.client;

			//If brick isn't generated terrain or boundaries
			if(!%col.PTGTerrainBr && !%col.ChBoundsPTG) //if(%BrTypePass)
			{
				//If brick was planted by player
				if(%col.PTGPlayerBr)
				{
					//If brick was planted by a player, saved to chunk file, then reloaded by generator
					if(%col.PTGGenerated)
					{
						//If brick is public
						if(%ObjBrGroupisPub)
						{
							//ServerPlay3D(hammerHitSound,%col.getPosition());
							if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Player bricks (loaded from chunk saves) are under public ownership and can only be destroyed using the admin / desctructo wand",5);
							return;
						}
						
						//Otherwise, do trust check for loaded brick
						else if(isObject(%ObjBrGroup) && getTrustLevel(getBrickGroupFromObject(%cl),%ObjBrGroup) < 2)
						{
							%plyrN = %ObjBrGroup.name;
							%plyrID = %ObjBrGroup.bl_id;
							
							//ServerPlay3D(hammerHitSound,%col.getPosition());
							if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0Insufficient Trust: <color:ffffff>\"" @ %plyrN @ "\" (ID:" @ %plyrID @ ") doesn't trust you enough to do that",5);
							return;
						}
					}
					
					//If brick was planted by a player
					else
					{
						%ClBrGroup = getBrickGroupFromObject(%cl);
						
						//Trust level check
						if(isObject(%ObjBrGroup) && getTrustLevel(%ClBrGroup,%ObjBrGroup) < 2)
						{
							%plyrN = %ObjBrGroup.name;
							%plyrID = %ObjBrGroup.bl_id;
							
							//ServerPlay3D(hammerHitSound,%col.getPosition());
							if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0Insufficient Trust: <color:ffffff>\"" @ %plyrN @ "\" (ID:" @ %plyrID @ ") doesn't trust you enough to do that",5);
							return;
						}
					}
				}
				
				//If brick is a biome detail
				if(%col.PTGDetailBr)
				{
					//If option to allow destruction of details bricks under public ownership is disabled
					if(!$PTG.DestroyPublicBr && getBrickGroupFromObject(%col).getName() $= "BrickGroup_888888")
					{
						if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Detail bricks are under public ownership and can only be destroyed using the admin / desctructo wand",5);
						return;
					}

					//If detail bricks set to be destroyed by admin / desctructo wand only ("%obj.getClassName() $= "Player"" filters out bots)
					if($PTG.PreventDestDetail)
					{
						if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Detail bricks have been set to only be destroyed using the admin / desctructo wand",5);
						return;
					}
				}
			}
			
			//If brick is generated terrain or boundaries
			else
			{			
				//If detail bricks set to be destroyed by admin / desctructo wand only
				if(%col.PTGTerrainBr && $PTG.PreventDestTerrain)
				{
					if(isobject(%cl) && %obj.getClassName() $= "Player")
					{
						if(getBrickGroupFromObject(%col).getName() $= "BrickGroup_888888")
							commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Terrain bricks are under public ownership and can only be destroyed using the admin / desctructo wand",5);
						else
							commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Terrain bricks have been set to only be destroyed using the admin / desctructo wand",5);
					}
					return;
				}
				
				//If detail bricks set to be destroyed by admin / desctructo wand only
				if(%col.ChBoundsPTG && $PTG.PreventDestBounds)
				{
					if(isobject(%cl) && %obj.getClassName() $= "Player")
					{
						if(getBrickGroupFromObject(%col).getName() $= "BrickGroup_888888")
							commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Boundary bricks are under public ownership and can only be destroyed using the admin / desctructo wand",5);
						else
							commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>Boundary bricks have been set to only be destroyed using the admin / desctructo wand",5);
					}
					return;
				}
			}
			
			//Call onToolBreak Method
			if(isObject(%ObjBrGroup) && isFunction(%col.onToolBreak(%col,%ObjBrGroup)))
				%col.onToolBreak(%col,%ObjBrGroup);
			
			//////////////////////////////////////////////////
			
			//If set up to set current chunk to "edited" when brick is destroyed by wand
			if(isObject(PTG_GlobalSO) && isObject(PTG_MainSO) && $PTG.chEditBrPPD && isObject(BrickGroup_Chunks))
			{
				%ChSize = mClamp($PTGm.chSize,16,256);
				%CHPosX = mFloor(getWord(%col.position,0) / %ChSize) * %ChSize;
				%CHPosY = mFloor(getWord(%col.position,1) / %ChSize) * %ChSize;
				%Chunk = "Chunk_" @ %CHPosX @ "_" @ %CHPosY;
				
				if(isObject(%Chunk))
					%Chunk.ChEditedPTG = true;
			}
			
			//Custom removal of brick
			ServerPlay3D(brickBreakSound,%col.getPosition());
			%col.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3); //"0 0 8"
			%col.PTGNullBrAux = true;
			%col.scheduleNoQuota(%app = 500,delete);
			
			%nextBr = %col.getUpBrick(0);
			
			//Clear all bricks above initial using custom removal script
			while(isObject(%nextBr) && %failSafe++ < 1000)
			{
				%oldBr = %nextBr;
				%nextBr = %oldBr.getUpBrick(0);
				
				ServerPlay3D(brickBreakSound,%oldBr.getPosition());
				%oldBr.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3); //"0 0 8"
				%oldBr.PTGNullBrAux = true;
				%oldBr.scheduleNoQuota(%app += mClamp($PTG.delay_subFuncMS,0,50),delete); //make sure lag isn't an issue here
			}
		}
		
		//return parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
	}
	 
	 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	 
	 //// FLOATING BRICK SUPPORT ////
	function AdminWandImage::onHitObject(%this,%obj,%slot,%col,%d,%e)//,%f) //%f appears to be unused
	{
		//%parent = parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
		//%this = wand
		//%obj = player / bot using wand(?)
		//%d = position of particle impact?
		//%e = angle of impact?
		
		//Prevent redestroying same brick (while it's being removed using custom method)
		if(isObject(%col) && %col.PTGNullBrAux)
			return;
		
		//%BrTypePass = %col.PTGDetailBr || %col.PTGPlayerBr || %col.PTGStreamBrTert || %col.StreamBr || %col.StreamBrAux;
		%BrClPass = %col.getClassName() $= "fxDTSBrick";
		
		if(%BrClPass)
		{
			if(isObject(%downBr = %col.getDownBrick(0)))
				%downBrPTG = %downBr.PTGgenerated;
			if(isObject(%upBr = %col.getUpBrick(0)))
				%upBrPTG = %upBr.PTGgenerated;
		}
		
		if(isObject(%ObjBrGroup = getBrickGroupFromObject(%col)))
			%ObjBrGroupisPub = %ObjBrGroup.getName() $= "BrickGroup_888888";
		
		//Use parent function only if col is not brick, or if brick is attached to the ground
		if(!isObject(%col) || !%BrClPass || (%col.hasPathToGround() && !%col.PTGgenerated && (!%downBrPTG && !%upBrPTG))) //%col.hasPathToGround()) // && !%BrTypePass && !%col.ChBoundsPTG && !%col.PTGTerrainBr))
		{
			if(!%col.ChBoundsPTG) //don't return parent if brick is a boundary brick (to prevent being destroyed by wand, unless allowed below)
				return parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
		}

		//Clear inital brick using custom script (".PTGNullBrAux" is to prevent issues with ".PTGNullBr" set by another script before this one runs)
		if(%BrClPass && !%col.PTGNullBrAux)
		{
			%cl = %obj.client;
			
			//If attempting to remove a public (PTG generated) brick and public brick destruction is disabled for wand
			if(%ObjBrGroupisPub && !%cl.destroyPublicBricks)
			{
				if(isobject(%cl) && %obj.getClassName() $= "Player") commandToClient(%cl,'centerPrint',"\c0P\c3T\c1G \c0ACTION DENIED: <color:ffffff>This brick is under public ownership, but the Desctructo Wand has public brick destruction disabled. Use the \c0/dpb <color:ffffff>chat command to re-enable it",5);
			}
			else
			{
				//If set up to set current chunk to "edited" when brick is destroyed by wand
				if(isObject(PTG_GlobalSO) && isObject(PTG_MainSO) && $PTG.chEditBrPPD && isObject(BrickGroup_Chunks))
				{
					%ChSize = mClamp($PTGm.chSize,16,256);
					%CHPosX = mFloor(getWord(%col.position,0) / %ChSize) * %ChSize;
					%CHPosY = mFloor(getWord(%col.position,1) / %ChSize) * %ChSize;
					%Chunk = "Chunk_" @ %CHPosX @ "_" @ %CHPosY;
					
					if(isObject(%Chunk))
						%Chunk.ChEditedPTG = true;
				}

				//Custom removal of brick
				ServerPlay3D(brickBreakSound,%col.getPosition());
				%col.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3); //"0 0 8"
				%col.PTGNullBrAux = true;
				%col.scheduleNoQuota(%app = 500,delete);
				
				%nextBr = %col.getUpBrick(0);
			
				//Clear all bricks above initial using custom removal script
				while(isObject(%nextBr) && %failSafe++ < 1000)
				{
					%oldBr = %nextBr;
					%nextBr = %oldBr.getUpBrick(0);
					
					ServerPlay3D(brickBreakSound,%oldBr.getPosition());
					%oldBr.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3); //"0 0 8"
					%oldBr.PTGNullBrAux = true;
					%oldBr.scheduleNoQuota(%app += mClamp($PTG.delay_subFuncMS,0,50),delete); //make sure lag isn't an issue here
				}
			}
		}
		
		//if /dpb option is disabled
		//remove stacked details
		//prevent chain-kill if brick underneath (for wand too)
		
		//return parent::onHitObject(%this,%obj,%slot,%col,%d,%e);//,%f);
	}
};
activatePackage("FloatingBricks");