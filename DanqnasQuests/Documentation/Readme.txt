<story
		QuestName="Beer for the Troops"										<!-- REQUIRED - Name will appear in quest log - any short string -->
		PlayerDisposition="Positive"										<!-- REQUIRED - How the player feels about NPC - Positive/Negative/Neutral for good/bad/ambivalent -->
		SpecificTarget="hero"											<!-- OPTIONAL - You can put in a name, leave blank for the mod to pick, or enter "hero" to be the person talking -->
		QuestType="Delivery"											<!-- REQUIRED - Delivery/Murder/Messenger -->
		Item="Beer"												<!-- OPTIONAL UNLESS QUESTTYPE = DELIVERY - decent list here https://www.naguide.com/mount-blade-ii-bannerlord-trading-goods-guide/ -->
		RewardType="Relation"											<!-- REQUIRED - Relation/Gold only for now, maybe Item one day -->
		StartQuestPlayerDialogue="You and your men look fatigued"						<!-- REQUIRED - Menu Option -->
		StartQuestNPCDialogue="It's been a long day for the troops and I, could you bring us some beer?"	<!-- REQUIRED -->		
		EndQuestPlayerDialogue="I've got that beer!"								<!-- REQUIRED -->
		EndQuestNPCDialogue="You're the best friend I've never had!"						<!-- REQUIRED -->		
		LogStart="Get some beer for the troops"									<!-- REQUIRED - Put your journal log for quest start here -->
		LogFinish="You provided beer for the troops"								<!-- REQUIRED - Put your journal log for quest end here -->
	/>

	COMPLEX QUEST UTILIZING FULL TEMPLATE

	<story
		QuestName="Give Temion A Sheep"									<!-- REQUIRED - Name will appear in quest log - any short string -->
		PlayerDisposition="Positive"									<!-- REQUIRED - How the player feels about NPC - Positive/Negative/Neutral for good/bad/ambivalent -->
		SpecificInitiator="Pharon"									<!-- OPTIONAL - limit the quest to only start by this hero -->
		InitiatorTestosteroneDominant="neutral"								<!-- OPTIONAL - true=yes, false=no, neutral=ignored - can limit quest to heroes by testosterone level -->
		Occupation="Lord"										<!-- OPTIONAL - We don't need to care if he's a Lord for this quest, this is for when you're creating a general quest -->
		SpecificTarget="Temion"										<!-- OPTIONAL - Can specify or let mod pick based on other criteria. If the target specified is dead this quest will never fire -->
		TargetTestosteroneDominant="true"								<!-- OPTIONAL - Given our start and end heroes are spelled out, again this is irrelevant -->
		TargetSameFaction="true"									<!-- OPTIONAL - true=in same faction, false=not shared faction, neutral=any faction -->
		RelationRequirement="neutral"									<!-- OPTIONAL - POSITIVE, NEGATIVE, NEUTRAL=ignored -->
		QuestType="Delivery"										<!-- REQUIRED - Delivery/Murder/Messenger -->
		Item="Sheep"											<!-- OPTIONAL UNLESS QUESTTYPE = DELIVERY - decent list here https://www.naguide.com/mount-blade-ii-bannerlord-trading-goods-guide/ -->
		ItemQty="1"											<!-- OPTIONAL - Will default to 1 if no whole number entered -->
		RewardType="Relation"										<!-- REQUIRED - Relation/Gold only for now, maybe Item one day -->
		RewardQty="0"											<!-- OPTIONAL - Set how much gold or relation manually, otherwise it will be automatically calculated -->
		TimeDays="30"											<!-- OPTIONAL - set days until quest expires, defaults to 30 if 0 or not a number -->
		StartQuestPlayerDialogue="What's that grin, Pharon?"												<!-- REQUIRED - Menu Option -->
		StartQuestNPCDialogue="I'm highly amused. I finally found a sheep that...you know what? Why don't you take this sheep to Temion for me?"	<!-- REQUIRED - Initiator -->
		OptionalStartQuestPlayerDialogue="Why this sheep in particular?"										<!-- OPTIONAL -->
		OptionalStartQuestNPCDialogue="Temion will definitely tell you! Come on, it'll be a laugh."							<!-- OPTIONAL - Initiator -->
		InterimQuestPlayerDialogue="I've still got that sheep to deliver."										<!-- OPTIONAL - but will be defaulted if left out -->
		InterimQuestNPCDialogue="I can't wait to see his face when you deliver it!"									<!-- OPTIONAL - Initiator- but will be defaulted if left out -->
		OptionalInterimQuestPlayerDialogue="So you're absolutely not gonna tell me about the sheep?"				<!-- OPTIONAL -->
		OptionalInterimQuestNPCDialogue="Trust me, Temion will tell you everything you need to know."				<!-- OPTIONAL - Initiator -->
		EndQuestPlayerDialogue="So, Pharon sent you this sheep..."								<!-- REQUIRED -->
		EndQuestNPCDialogue="He what now? That bastard! Oh he thinks this is funny doesn't he? Do you know about the sheep?"	<!-- REQUIRED - Target -->
		OptionalEndQuestPlayerDialogue="He said you'd tell me?"									<!-- OPTIONAL -->
		OptionalEndQuestNPCDialogue="I'll have his head! Look, I used to have a pet sheep. He's never let it go...still...this one does look similar. I might keep it, got those same cute li'l hoofs." <!-- OPTIONAL - Target -->
		MultiPart="true"										<!-- OPTIONAL - Requires a specific initiator or will be ignored -->
		MultiKey="Unique1234a8s8f8f8g8"									<!-- REQUIRED if MultiPart = true, and every part should share the key-->
		PartNumber="1"											<!-- REQUIRED if MultiPart = true, numeric 1,2,3..., must start at 1 and increment by 1 -->
		TotalParts="2"											<!-- REQUIRED if MultiPart = true, numeric whole number -->
		LogStart="Pharon's asked you to take a sheep to Temion"						<!-- REQUIRED - Put your journal log for quest start here -->
		LogFinish="Temion wasn't too happy to get the sheep"						<!-- REQUIRED - Put your journal log for quest end here -->
	/>
	
	Reduced complication multi-part quest 
	<story
		QuestName="Temion had a little lamb"
		PlayerDisposition="Positive"
		SpecificInitiator="Temion"
		SpecificTarget="Pharon"
		QuestType="Messenger"
		RewardType="Relation"
		StartQuestPlayerDialogue="You look like you've got something to ask me."
		StartQuestNPCDialogue="It's about the sheep. I can't let him have this. Go tell him that Temion will see him at Poros."
		InterimQuestPlayerDialogue="I haven't seen Pharon yet."
		InterimQuestNPCDialogue="No worries."
		EndQuestPlayerDialogue="So, that sheep thing? Temion said he would see you at Poros."
		EndQuestNPCDialogue="Ah, poor Temion. Not the best at witty comebacks, hey? My mum lives at Poros. Can't wait to see him and check in on his new pet! HAHA!"		
		MultiPart="true"
		MultiKey="Unique1234a8s8f8f8g8"
		PartNumber="2"
		TotalParts="2"
		LogStart="Give Temion's message to Pharon"
		LogFinish="You gave Temion's message to Pharon"
	/>