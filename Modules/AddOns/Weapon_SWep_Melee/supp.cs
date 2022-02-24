function player::headExplosion(%pl,%flag)
{
	%pl.hideNode("headskin");
	for(%i=0;$hat[%i]!$="";%i++)
		%pl.hideNode($hat[%i]);
	for(%i=0;$accent[%i]!$="";%i++)
		%pl.hideNode($accent[%i]);
		
	%pl.mountImage(headBloodSprayImage,2);
	%pl.schedule(350,mountImage,headBloodTrickleImage,2);
	%pl.schedule(750,unMountImage,2);
}
function swol_calculateDamagePosition(%pl,%hitPos)
{
	if(%pl.isCrouched())
	{
		%center = vectorAdd(%pl.getWorldBoxCenter(),"0 0 -1.49");
		%vecTo = vectorNormalize(vectorSub(getWords(%hitPos,0,1) SPC 0,getWords(%center,0,1) SPC 0));
		%forward = %pl.getForwardVector();
		%dot = vectorDot(%vecTo,%forward);
		if(%dot < 0.3)
			return "legs";
		if(%dot < 0.6)
			return "lowbody";
		else
			return "head";
	}
	else
	{
		%z = getWord(%hitPos,2);
		%zWorld = getWord(%pl.getWorldBoxCenter(),2);
		%diff = %zWorld-%z;
		%diff = 2.6-(%diff-2.65);
		
		if(%diff > 1.8)
			return "head";
		if(%diff > 1.35)
			return "upbody";
		if(%diff > 0.8)
			return "lowbody";
		else
			return "legs";
	}
}
function vectorAngleRotate(%vec,%ref,%ang) //thanks luna i love you and your big math brain
{
	%x = getWord(%vec,0);
	%y = getWord(%vec,1);
	%z = getWord(%vec,2);
	%u = getWord(%ref,0);
	%v = getWord(%ref,1);
	%w = getWord(%ref,2);
	%cos = mCos(%ang);
	%sin = mSin(%ang);
	return %u*(%u*%x+%v*%y+%w*%z)*(1-%cos)+%x*%cos+(%v*%z-%w*%y)*%sin SPC %u*(%u*%x+%v*%y+%w*%z)*(1-%cos)+%y*%cos+(%w*%x-%u*%z)*%sin SPC %u*(%u*%x+%v*%y+%w*%z)*(1-%cos)+%z*%cos+(%u*%y-%v*%x)*%sin;
}
function player::getUpVectorHack(%pl)
{
	return getUpFromVec(%pl.getEyeVector());
}
function getUpFromVec(%vec)
{
	%x = getWord(%vec,0);
	%y = getWord(%vec,1);
	%z = getWord(%vec,2);
	%up = vectorNormalize((-%x*%z) SPC (-%z*%y) SPC ((%x*%x)+(%y*%y)));
	return %up;
}
function player::getEyeVectorHack(%pl) //thanks port i love you and your cute eyes
{
    %forward = %pl.getForwardVector();
    %eye = %pl.getEyeVector();

    %x = getWord(%eye, 0);
    %y = getWord(%eye, 1);

    %yaw = mATan(getWord(%forward,0),getWord(%forward,1));
    %pitch = mATan(getWord(%eye,2),mSqrt(%x*%x+%y*%y));

    return MatrixMulVector(MatrixCreateFromEuler(%pitch SPC 0 SPC -%yaw),"0 1 0");
}
function vectorRelativeShift(%forward,%up,%shift)
{
	return vectorAdd(vectorAdd(vectorScale(%forward,getWord(%shift,0)),vectorScale(vectorCross(%forward,%up),getWord(%shift,1))),vectorScale(%up,getWord(%shift,2)));
}