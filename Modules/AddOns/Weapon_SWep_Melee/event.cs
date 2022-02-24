registerInputEvent(fxDTSBrick,onMeleeHit,"Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "Minigame Minigame");
function fxDtsBrick::onMeleeHit(%br,%pl,%pos)
{
	$InputTarget_["Self"] = %br;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl = %pl.client;
	if($Server::LAN)
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%cl);
	else
		$InputTarget_["MiniGame"] = ((%t = getMiniGameFromObject(%br)) == getMiniGameFromObject(%cl) ? %t : 0);
	%br.processInputEvent(onMeleeHit,%cl);
}