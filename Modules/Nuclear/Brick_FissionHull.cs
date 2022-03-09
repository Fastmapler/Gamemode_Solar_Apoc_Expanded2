datablock fxDTSBrickData(brickMFRHullData)
{
	brickFile = "./Bricks/MFRHull.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Hull";

	//isFissionPart = 1;
};

function brickMFRHullData::onPlant(%this,%brick)
{
	Parent::onPlant(%this,%brick);

	%brick.CreateFissionHull();
}

function fxDtsBrick::CreateFissionHull(%obj)
{
	%fission = new ScriptObject()
	{
		class = "EOTW_FissionHull";
	};
	%obj.fissionParent = %fission;
	%fission.hullBrick = %obj;
}

function EOTW_FissionHull::GetFissionPart(%obj, %x, %y)
{
	if (isObject(%obj.fissionPart[%x, %y]))
		return %obj.fissionPart[%x, %y];
	else
	{
		%obj.fissionPart[%x, %y] = "";
		return -1;
	}
}

function EOTW_FissionHull::AddFissionPart(%obj, %part, %distCheck)
{
	%hull = %obj.hullBrick;
	%distCheck = %distCheck + 0;
	if (%part.getDownBrick(%distCheck) != %hull && %part.getDownBrick(%distCheck) != %hull)
	{
		%part.killBrick();
		return;
	}

	%part.fissionParent = %obj;
	%partData = %part.getDatablock();
	%volXRadius = %partData.brickSizeX / 4;
	%volyRadius = %partData.brickSizeY / 4;
	%placedPos = getWords(vectorSub(%hull.getPosition(), %part.getPosition()), 0, 1);

	for (%x = (%volXRadius * -1) + 0.5; %x <= %volXRadius; %x += 0.5)
		for (%y = (%volYRadius * -1) + 0.5; %y <= %volYRadius; %y += 0.5)
			%obj.fissionPart[getWord(%placedPos, 0) + %x, getWord(%placedPos, 1) +  %y] = %part.getID();
}