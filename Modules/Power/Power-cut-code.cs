	//Set of sets
	if(!isObject(PowerAggregatorSet))
		new SimSet(PowerAggregatorSet);

	//if(!isObject(PowerAggregatorSchedules))
	//	new SimSet(PowerAggregatorSchedules);

	//Get all sets, clear them
	for(%i = 0; %i < PowerAggregatorSet.getCount(); %i++)
	{
		%set = PowerAggregatorSet.getObject(%i);
		%set.clear();
	}

	//Create new sets if needed
	%sets_needed = mCeil(PowerMasterLoopGroup.getCount() / $EOTW::ObjectsPerLoop);
	%sets_to_create = %sets_needed - PowerAggregatorSet.getCount();
	if(%sets_to_create > 0)
		for(%i = 0; %i < %sets_to_create; %i++)
			PowerAggregatorSet.add(new SimSet());

	for(%i = 0; %i < PowerMasterLoopGroup.getCount(); %i++)
	{
		//TODO: scheduling on a large SimSet is perhaps really bad?

		//PowerMasterLoopGroup.schedule(0, "IterateLoopCalled", %i, $EOTW::ObjectsPerLoop);
		//schedule(0, 0, IterateLoopCalled, PowerMasterLoopGroup.getObject(%i));
		//IterateLoopCalled(PowerMasterLoopGroup.getObject(%i));

		%obj = PowerMasterLoopGroup.getObject(%i);

		%setIndex = mFloor(%i / $EOTW::ObjectsPerLoop);
		%subset = PowerAggregatorSet.getObject(%setIndex);
		%subset.add(%obj);
	}

	//schedule out
	for(%i = 0; %i < PowerAggregatorSet.getCount(); %i++)
	{
		//schedule(0, 0, SuperIterateLoopCalled, PowerAggregatorSet.getObject(%i));
		SuperIterateLoopCalled(PowerAggregatorSet.getObject(%i));
	}
