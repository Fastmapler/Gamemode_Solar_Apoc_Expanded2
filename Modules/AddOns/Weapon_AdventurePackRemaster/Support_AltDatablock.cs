//Support_AltDatablock.cs
//Add and remove "alternate" datablocks from players for special abilities

function VectorRotateAxis(%vector,%axis,%angle)
{
	%x = getWord(%vector,0);
	%y = getWord(%vector,1);
	%z = getWord(%vector,2);
	%u = getWord(%axis,0);
	%v = getWord(%axis,1);
	%w = getWord(%axis,2);
	%ux = %u * %x;
	%uy = %u * %y;
	%uz = %u * %z;
	%vx = %v * %x;
	%vy = %v * %y;
	%vz = %v * %z;
	%wx = %w * %x;
	%wy = %w * %y;
	%wz = %w * %z;
	%sa = mSin(%angle);
	%ca = mCos(%angle);
	%nx = %u*(%ux+%vy+%wz)+(%x*(%v*%v+%w*%w)-%u*(%vy+%wz))*%ca+(-%wy+%vz)*%sa;
	%ny = %v*(%ux+%vy+%wz)+(%y*(%u*%u+%w*%w)-%v*(%ux+%wz))*%ca+(%wx-%uz)*%sa;
	%nz = %w*(%ux+%vy+%wz)+(%z*(%u*%u+%v*%v)-%w*(%ux+%vy))*%ca+(-%vx+%uy)*%sa;
	return(%nx SPC %ny SPC %nz);
}
function getAngleToPosition(%pos,%tpos)
{
	%z = getWord(%pos,2);
	%xs = getWord(%tpos,0);
	%ys = getWord(%tpos,1);
	%tpos = %xs SPC %ys SPC %z;
	%posX = getWord(%pos,0);
	%posY = getWord(%pos,1);

	%tposX = getWord(%tpos,0);
	%tposY = getWord(%tpos,1);

	%tx = %tposX - %posX;
	%ty = %tposY - %posY;
	%rad = mAtan(%ty,%tx);
	%angle = mFloor(%rad/$PI * 180)+180+90;
	%axis = eulerToAxis(0 SPC 0 SPC %angle);
}
function rotBetween(%a,%b)
{
	%v = vectorNormalize(vectorSub(%b,%a));
	%x = getWord(%v,0);
	%y = getWord(%v,1);
	%yaw = mATan(%x,%y) - $pi / 2;
	%pitch = 0 - mATan(getWord(%v,2),mSqrt(%x * %x + %y * %y));
	%xy = -90 - %pitch * 180 / $pi;
	%z = -90 - %yaw * 180 / $pi;
	return eulerToAxis(%xy SPC 0 SPC %z);
}
function eulerToAxis(%euler)
{
	%euler = VectorScale( %euler, $pi / 180 );
	%matrix = MatrixCreateFromEuler( %euler );
	return getWords( %matrix, 3, 6 );
}
function axisToEuler(%axis)
{
	%angleOver2 = getWord( %axis, 3 ) * 0.5;
	%angleOver2 = -%angleOver2;
	%sinThetaOver2 = mSin( %angleOver2 );
	%cosThetaOver2 = mCos( %angleOver2 );
	%q0 = %cosThetaOver2;
	%q1 = getWord( %axis, 0 ) * %sinThetaOver2;
	%q2 = getWord( %axis, 1 ) * %sinThetaOver2;
	%q3 = getWord( %axis, 2 ) * %sinThetaOver2;
	%q0q0 = %q0 * %q0;
	%q1q2 = %q1 * %q2;
	%q0q3 = %q0 * %q3;
	%q1q3 = %q1 * %q3;
	%q0q2 = %q0 * %q2;
	%q2q2 = %q2 * %q2;
	%q2q3 = %q2 * %q3;
	%q0q1 = %q0 * %q1;
	%q3q3 = %q3 * %q3;
	%m13 = 2.0 * ( %q1q3 - %q0q2 );
	%m21 = 2.0 * ( %q1q2 - %q0q3 );
	%m22 = 2.0 * %q0q0 - 1.0 + 2.0 * %q2q2;
	%m23 = 2.0 * ( %q2q3 + %q0q1 );
	%m33 = 2.0 * %q0q0 - 1.0 + 2.0 * %q3q3;
	return mRadToDeg( mAsin( %m23 ) ) SPC mRadToDeg( mAtan( -%m13, %m33 ) ) SPC mRadToDeg( mAtan( -%m21, %m22 ) );
}
function vectorAngleRotate(%vec,%ref,%ang)
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
	%muz = MatrixMulVector(%pl.getSlotTransform(0),"0 1 0");
	%forward = %pl.getForwardVector();
	%up = %pl.getUpVector();
	return vectorAngleRotate(%muz,vectorCross(%forward,%up),$PI/2);
}
function player::getEyeVectorHack(%pl)
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

function Player::pushDatablock(%this,%data)
{
	%data = %data.getID();
	
	if(%this.getState() $= "Dead")
		return;
	
	if(fileName(%this.dataBlock.shapeFile) !$= fileName(%data.shapeFile))
		return;
	
	if(%this.altDataID[%data.getID()] !$= "")
		return;
	
	if(%this.altDataNum == 0 || %this.altDataNum == 1)
	{
		%this.altDataNum = 1;
		%this.altData[0] = %this.dataBlock.getID();
		%this.altDataID[%this.dataBlock.getID()] = 0;
	}
	
	%this.altData[%this.altDataNum] = %data.getID();
	%this.altDataID[%data.getID()] = %this.altDataNum;
	%this.altDataNum++;
	
	
	%health = %this.dataBlock.maxDamage - %this.getDamageLevel();
	%flash = %this.getDamageFlash();
	%white = %this.getWhiteOut();
	
	%this.isChangingAltData = 1;
	
	%this.setDatablock(%data);
	%this.setHealth(%health);
	%this.setDamageFlash(%flash);
	%this.setWhiteOut(%white);
	
	%this.isChangingAltData = "";
}

function Player::popDatablock(%this,%data)
{
	if(%this.getState() $= "Dead")
		return;
	
	if(%data $= "")
	{
		%this.altDataNum--;
		
		%health = %this.dataBlock.maxDamage - %this.getDamageLevel();
		%flash = %this.getDamageFlash();
		%white = %this.getWhiteOut();
		
		%this.isChangingAltData = 1;
		
		%this.setDatablock(%this.altData[%this.altDataNum-1]);
		%this.setHealth(%health);
		%this.setDamageFlash(%flash);
		%this.setWhiteOut(%white);
		
		%this.isChangingAltData = "";
		
		%this.altDataID[%this.altData[%this.altDataNum]] = "";
		%this.altData[%this.altDataNum] = "";
	}
	else
	{
		%data = %data.getID();
		
		if(!isObject(%data) || %data.getClassName() !$= "PlayerData")
			return;
		
		if(%this.altDataID[%data] == 0)
			return;
		
		%id = %this.altDataID[%data.getID()];
		
		for(%i=%id;%i<%this.altDataNum;%i++)
		{
			%this.altDataID[%this.altData[%i]]--;
			%this.altData[%i] = %this.altData[%i+1];
		}
		
		%this.altDataID[%data] = "";
		%this.altDataNum--;
		
		if(%this.dataBlock.getID() == %data)
		{
			%health = %this.dataBlock.maxDamage - %this.getDamageLevel();
			%flash = %this.getDamageFlash();
			%white = %this.getWhiteOut();
			
			%this.isChangingAltData = 1;
			
			%this.setDatablock(%this.altData[%this.altDataNum-1]);
			%this.setHealth(%health);
			%this.setDamageFlash(%flash);
			%this.setWhiteOut(%white);
			
			%this.isChangingAltData = "";
		}
	}
}

function Player::getFirstDatablock(%this)
{
	if(%this.altDataNum == 0)
		return %this.dataBlock;
	else
		return %this.altData[0];
}

function Player::resetDatablock(%this)
{
	if(%this.getState() $= "Dead")
		return;
	
	%data = %this.getFirstDatablock();
	if(%data == %this.dataBlock)
		return;
	
	%health = %this.dataBlock.maxDamage - %this.getDamageLevel();
	%flash = %this.getDamageFlash();
	%white = %this.getWhiteOut();
	
	%this.isChangingAltData = 1;
	
	%this.setDatablock(%data);
	%this.setHealth(%health);
	%this.setDamageFlash(%flash);
	%this.setWhiteOut(%white);
	
	%this.isChangingAltData = "";
	
	for(%i=0;%i<%this.altDataNum;%i++)
	{
		%this.altDataID[%this.altData[%i]] = "";
		%this.altData[%i] = "";
	}
	
	%this.altDataNum = 0;
}

package AltDatablocks
{
	function Player::playPain(%this)
	{
		if(%this.isChangingAltData)
			return;
		
		Parent::playPain(%this);
	}
	
	function ShapeBase::setDatablock(%this,%data)
	{
		if(%this.getType() & $TypeMasks::PlayerObjectType && %this.altDataNum > 1 && !%this.isChangingAltData)
		{
			%this.altDataID[%this.dataBlock.getID()] = "";
			%this.altDataID[%data.getID()] = 0;
			%this.altData[0] = %data;
			return;
		}
		
		Parent::setDatablock(%this,%data);
	}
};activatePackage(AltDatablocks);