//Can we just find the folder names automatically?
$EOTW::Modules = "Core Environment Fauna Matter Player Power Tools Weapons AddOns";

//"-2048 -2048 2048 2048"
function getMapArea()
{
	return (getWord($EOTW::WorldBounds, 2) - getWord($EOTW::WorldBounds, 0)) * (getWord($EOTW::WorldBounds, 3) - getWord($EOTW::WorldBounds, 1));
}

for (%i = 0; %i < getWordCount($EOTW::Modules); %i++)
	exec("./Modules/" @ getWord($EOTW::Modules, %i) @ "/" @ getWord($EOTW::Modules, %i) @ ".cs");

//One of our goals is to keep the code as organized as possible. We split each section of code into their own
//folders. Even though loading more files may take longer, the better code organization and readability is worth it.

//We also should try to keep external add-on requirements to a mininum, barring default blockland add-ons. This will make
//Solar apoc much easier to run, especially when setting up new servers.