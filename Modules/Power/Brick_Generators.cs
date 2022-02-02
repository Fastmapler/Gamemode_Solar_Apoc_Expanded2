datablock fxDTSBrickData(brickEOTWSolarPanelData)
{
	brickFile = "./Bricks/SolarPanel.blb";
	category = "Solar Apoc";
	subCategory = "Power Source";
	uiName = "Solar Panel";
	energyGroup = "Source";
	energyMaxBuffer = 1600;
	loopFunc = "EOTW_SolarPanelLoop";
    inspectFunc = "EOTW_DefaultInspectLoop";
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWSolarPanelData"] = 0.85 TAB "7a7a7aff" TAB 64 TAB "Silver" TAB 64 TAB "Rosium" TAB 64 TAB "Teflon";

function fxDtsBrick::EOTW_SolarPanelLoop(%obj)
{
	if (%obj.energy $= "" || %obj.energy < 0)
		%obj.energy = 0;
	
    %wattage = 160;

	if ($EOTW::Time < 12)
	{
		%val = ($EOTW::Time / 12) * $pi;
	    %ang = ($EnvGuiServer::SunAzimuth / 180) * $pi;
	    %dir = vectorScale(mSin(%ang) * mCos(%val) SPC mCos(%ang) * mCos(%val) SPC mSin(%val), 512);
		%ray = containerRaycast(vectorAdd(%pos = %obj.getPosition(), %dir), %pos, $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType);
		if(!isObject(%hit = firstWord(%ray)) || %hit == %obj)
            %obj.ChangePower(%wattage / $EOTW::PowerTickRate);
	}
}