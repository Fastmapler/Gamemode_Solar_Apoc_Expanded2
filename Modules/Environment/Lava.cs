datablock StaticShapeData(EOTWStaticLava)
{
	shapeFile = "./Shapes/Lava.dts";
};

function CreateLavaStatic()
{
	if (isObject($EOTW::LavaStatic))
		$EOTW::LavaStatic.delete();
	
	%shape = new StaticShape(LavaStatic)
	{
		dataBlock = EOTWStaticLava;
	};
	
	MissionCleanup.add(%shape);
	
	%shape.setNodeColor("ALL", "1 0.5 0 1");
	%shape.setTransform("0 0 53");
	%shape.setScale("64 64 1");
	
	talk(%shape);
	
	$EOTW::LavaStatic = %shape;
}
schedule(100, 0, "CreateLavaStatic");

function setLavaHeight(%height)
{
	$EOTW::LavaHeight = %height;

	if (%height <= 0)
		setNewWater("NONE");
	else
		setNewWater("Add-Ons/Water_BrickLava/bricklava.water");
	
	if (!isObject($EOTW::LavaStatic))
		CreateLavaStatic();
	
	$EOTW::LavaStatic.setTransform("0 0 " @ %height);

	if (isObject(EnvMaster))
		servercmdEnvGui_SetVar(EnvMaster, "WaterHeight", %height);
}