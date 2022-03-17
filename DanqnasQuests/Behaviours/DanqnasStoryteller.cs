using DanqnasQuests.Models;
using DanqnasQuests.Quests;
using DanqnasQuests.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace DanqnasQuests.Behaviours
{
	class DanqnasStoryteller : CampaignBehaviorBase
	{

		protected void AddDialogs(CampaignGameStarter starter)
		{
			// Create a conditional dialog line for the player, but in the consequence we need to pick a quest to run
			starter.AddPlayerLine("DanqnasQuestInitiationDialoguePlayer", "hero_main_options", "DanqnasQuestInitiationDialogueNPC", "How are things going for you?", InitialDialogueCheck, null, 100, null, null);
			starter.AddDialogLine("DanqnasQuestInitiationDialogueNPC", "DanqnasQuestInitiationDialogueNPC", "DanqnasInitialPlayerResponse", "I could use a hand if you've got the time.", null, null, 100, null);
			starter.AddPlayerLine("DanqnasPlayerAcceptsMessengerQuest", "DanqnasInitialPlayerResponse", "DanqnasInitialQuestEndingSuccess", "We're on the move if that helps... (start messenger)", PlayerAllowsMessenger, StartQuestPickerMessenger, 100, null);
			starter.AddPlayerLine("DanqnasPlayerAcceptsMurderQuest", "DanqnasInitialPlayerResponse", "DanqnasInitialQuestEndingSuccess", "I have access to unmitigated violence... (start murder)", PlayerAllowsMurder, StartQuestPickerMurder, 100, null);
			starter.AddPlayerLine("DanqnasPlayerAcceptsDeliveryQuest", "DanqnasInitialPlayerResponse", "DanqnasInitialQuestEndingSuccess", "What's bothering you? (start delivery)", PlayerAllowsDelivery, StartQuestPickerDelivery, 100, null);
			starter.AddPlayerLine("DanqnasPlayerRejectsQuest", "DanqnasInitialPlayerResponse", "DanqnasInitialPlayerReject", "Sorry, I just remembered I have urgent business.", null, null, 100, null);
			starter.AddDialogLine("DanqnasQuestAcceptResponse", "DanqnasInitialQuestEndingSuccess", "hero_main_options", "Perhaps you can help...", null, null, 100, null);
			starter.AddDialogLine("DanqnasQuestRejectResponseNice", "DanqnasInitialPlayerReject", "hero_main_options", "These are busy times, friend.", RelationCheckNice, null, 100, null);
			starter.AddDialogLine("DanqnasQuestRejectResponseNasty", "DanqnasInitialPlayerReject", "hero_main_options", "Just like I expected.", RelationCheckNasty, null, 100, null);
		}

		public void StartQuestPickerMessenger()
		{
			StartQuestPicker("MESSENGER");
		}
		public void StartQuestPickerMurder()
		{
			StartQuestPicker("MURDER");
		}
		public void StartQuestPickerDelivery()
		{
			StartQuestPicker("DELIVERY");
		}

		public void StartQuestPicker(string PlayerChoice)
        {
			// Let's just quickly do a sanity check of the quests the player allows
			if (MenuConfig.Instance.DisableDeliveryQuests && MenuConfig.Instance.DisableMurderQuests && MenuConfig.Instance.DisableMessengerQuests && MenuConfig.Instance.DebuggingEnabled)
			{
				InformationManager.DisplayMessage(new InformationMessage("You have disallowed all quest types in settings."));
				return;
			}			

			// Great now let's filter those quests for the player choice
			List<Story> FilterByQuestType = new List<Story>();
			foreach (Story story in CurrentStory.Instance.CurrentStories)
			{
				if (story.QuestType == PlayerChoice)
				{
					FilterByQuestType.Add(story);
				}
			}

			// Remove any quests that are disabled for disposition
			if(MenuConfig.Instance.DisableNegativeDisposition)
            {
				List<Story> FilterByDisposition = new List<Story>();
				foreach(Story story in FilterByQuestType)
                {
					if(story.PlayerDisposition == "POSITIVE")
                    {
						FilterByDisposition.Add(story);
                    }						
                }
				FilterByQuestType = FilterByDisposition;
            }


			// TODO: @dan: 
			// If I understand this correctly this'll remove quests from specific Genders if they are disable in the options?
			// I'm not sure how useful this is. Why'd someone want to disable quests from a specific gender?

			// Remove any quests that are disabled for testosterone dependency
			/*if (!MenuConfig.Instance.DisableTestosteroneChecks)
			{
				List<Story> FilterByTestosterone = new List<Story>();


				foreach (Story story in FilterByQuestType)
				{
					if (story.InitiatorTestosteroneDominant == "NEUTRAL")
					{
						// Add all neutral quests
						FilterByTestosterone.Add(story);
					}
					else if (story.InitiatorTestosteroneDominant == "TRUE" && !Hero.OneToOneConversationHero.IsFemale)
                    {
						FilterByTestosterone.Add(story);
					}
					else if (story.InitiatorTestosteroneDominant == "FALSE" && Hero.OneToOneConversationHero.IsFemale)
					{
						FilterByTestosterone.Add(story);
					}
				}
				FilterByQuestType = FilterByTestosterone;
			}*/


			// TODO: @dan
			// Again I don't think this is useful.
			// With the new dialog system we could have targets in multiple factions.
			// So you'd have for example one dialog in your faction and the next in another.
			
			// Remove any quests that are disabled for faction
			/*if (!MenuConfig.Instance.DisableFactionChecks)
			{
				List<Story> FilterByFaction = new List<Story>();
				foreach (Story story in FilterByQuestType)
				{
					if (story.TargetSameFaction == "NEUTRAL")
					{
						// Add all neutral quests
						FilterByFaction.Add(story);
					}
					else if (story.TargetSameFaction == "TRUE" && Hero.OneToOneConversationHero.MapFaction == Hero.MainHero.MapFaction)
					{
						FilterByFaction.Add(story);
					}
					else if (story.TargetSameFaction == "FALSE" && Hero.OneToOneConversationHero.MapFaction != Hero.MainHero.MapFaction)
					{
						FilterByFaction.Add(story);
					}
				}
				FilterByQuestType = FilterByFaction;
			}*/

			// Remove any quests that are disabled for relation
			if (!MenuConfig.Instance.DisableRelationChecks)
			{
				List<Story> FilterByRelation = new List<Story>();
				foreach (Story story in FilterByQuestType)
				{
					if (story.RelationRequirement == "NEUTRAL")
					{
						// Add all neutral quests
						FilterByRelation.Add(story);
					}
					else if (story.RelationRequirement == "POSITIVE" && Hero.OneToOneConversationHero.GetRelationWithPlayer() >= 0)
					{
						FilterByRelation.Add(story);
					}
					else if (story.RelationRequirement == "NEGATIVE" && Hero.OneToOneConversationHero.GetRelationWithPlayer() < 0)
					{
						FilterByRelation.Add(story);
					}
				}
				FilterByQuestType = FilterByRelation;
			}

			/*
			// Remove any quests that are disabled for occupation			
			List<Story> FilterByOccupation = new List<Story>();
			foreach (Story story in FilterByQuestType)
			{
				if (story.Occupation == null || story.Occupation == string.Empty || story.Occupation == Hero.OneToOneConversationHero.Occupation.ToString())
				{
					FilterByOccupation.Add(story);
				}
			}
			FilterByQuestType = FilterByOccupation;
			*/

			// Let's check if there were any quests
			if (FilterByQuestType.Count < 1)
			{
				if (MenuConfig.Instance.DebuggingEnabled)
				{
					InformationManager.DisplayMessage(new InformationMessage("No quests of " + PlayerChoice + " types were found, download or add more quests!"));
				}
				return;
			}

			// Second, find any quest specific to the character that we can launch as a priority
			List<Story> FilterByHero = new List<Story>();
			List<Story> RemoveHeroSpecificQuests = new List<Story>();
			foreach (Story story in FilterByQuestType)
			{
				if (story.SpecificInitiator == Hero.OneToOneConversationHero.Name.ToString())
				{
					FilterByHero.Add(story);
				}
				else
                {
					RemoveHeroSpecificQuests.Add(story);
				}
			}
			FilterByQuestType = RemoveHeroSpecificQuests;

			// Let's check if there were any quests
			if (FilterByHero.Count < 1)
			{
				if (MenuConfig.Instance.DebuggingEnabled)
				{
					InformationManager.DisplayMessage(new InformationMessage("This hero has no specific quests...yet."));
				}				
			}

			// ALL other filtering needs to be done before we get here.  From here on out we should just be choosing the quest and implementing it.
			Story LaunchThisStory;
			if (FilterByHero.Count > 0)
            {
				// HERO SPECIFIC QUEST START
				LaunchThisStory = GetHeroStory(FilterByHero);
            }
			else
            {
				// RANDOM QUEST START
				LaunchThisStory = GetRandomStory(FilterByQuestType);
            }

			// Set up the gold reward here if that's what the quest specifies or give a nominal gold sum (configurable?)
			int GoldReward;
			if(LaunchThisStory.RewardType == "GOLD" && !MenuConfig.Instance.OverrideGoldReward)
            {
				GoldReward = LaunchThisStory.RewardQty;
            }
			else if (LaunchThisStory.RewardType == "RELATION")
            {
				GoldReward = 200;
            }
			else
            {
				GoldReward = MenuConfig.Instance.SetGoldReward;
            }

			Hero Target = null;
			// We allow many quests to have no set target, so we need to check and generate a target here
			if(LaunchThisStory.SpecificTarget == "HERO")
            {
				Target = Hero.OneToOneConversationHero;
            }
			else if(LaunchThisStory.SpecificTarget != string.Empty)
            {
				// Go through the alive heroes to find our specific champ
				List<Hero> heroes = Hero.AllAliveHeroes.ToList();
				int count = 0;
				while (Target == null && count < heroes.Count)
                {
					if (heroes[count].Name.ToString() == LaunchThisStory.SpecificTarget)
					{
						Target = heroes[count];
					}
					count++;
                }
				
				// Just in case they were dead or something, just grab a random hero as the target
				if (Target == null)
				{
					Target = GetMeAHero();
				}
            }
            else 
			{
				Target = GetMeAHero();
			}

			new DanqnaQuest(Hero.OneToOneConversationHero, GoldReward, Target, false, LaunchThisStory, LaunchThisStory.TimeDays).StartQuest();
		}

		private Story GetRandomStory(List<Story> Quests)
        {
			int QuestNumber;
			Story TheChosenOne = null;
			while (TheChosenOne == null)
			{
				Random rnd = new Random();
				QuestNumber = rnd.Next(0, Quests.Count);
				TheChosenOne = Quests[QuestNumber];                
			}

			return TheChosenOne;
        }

		private Story GetHeroStory(List<Story> Quests)
        {
			int QuestNumber;
			Story TheChosenOne = null;
			while (TheChosenOne == null)
			{
				Random rnd = new Random();
				QuestNumber = rnd.Next(0, (Quests.Count - 1));

				// We need to ensure we're not starting midway through a multipart quest
				if (Quests[QuestNumber].MultiPart)
				{
					// Make sure we already have a multipart quest database
					if (DanqnaQuest.PublicModifiableMultiPartQuests != null && DanqnaQuest.PublicModifiableMultiPartQuests.Count > 0)
					{
						// Check if this questname exists in the database
						for (int x = 0; x < DanqnaQuest.PublicModifiableMultiPartQuests.Count; x++)
						{
							// Check if the database has a quest of this name in it already
							if (DanqnaQuest.PublicModifiableMultiPartQuests[x][0] == Quests[QuestNumber].QuestName)
							{
								if (Quests[QuestNumber].PartNumber == (Convert.ToInt32(DanqnaQuest.PublicModifiableMultiPartQuests[x][1]) + 1))
								{
									// This quest is valid, we can continue
									TheChosenOne = Quests[QuestNumber];

									// don't forget to add the new multipart quest
									List<string> multiquest = new List<string>()
									{
										Quests[QuestNumber].QuestName,
										Quests[QuestNumber].PartNumber.ToString()
									};
									DanqnaQuest.PublicModifiableMultiPartQuests.Add(multiquest);
								}
							}
							else if (Quests[QuestNumber].PartNumber == 1)
							{
								TheChosenOne = Quests[QuestNumber];
							}
						}
					}
				}
				else
				{
					TheChosenOne = Quests[QuestNumber];
				}
			}

			return TheChosenOne;
		}

		// CONVERSATION CONDITIONALS

		public bool InitialDialogueCheck()
		{
			// Check if Hero has a quest already
			if (DanqnaQuest.PublicModifiableHeroesWithActiveQuests != null)
			{
				foreach (Hero hero in DanqnaQuest.PublicModifiableHeroesWithActiveQuests)
				{
					if (hero == Hero.OneToOneConversationHero)
					{
						return false;
					}
				}
			}

			if (Hero.OneToOneConversationHero == null)
			{
				return false;
			}

			return true;
		}

		public bool RelationCheckNice() => Hero.OneToOneConversationHero.GetRelationWithPlayer() >= 0

		public bool RelationCheckNasty() => Hero.OneToOneConversationHero.GetRelationWithPlayer() < 0;

		public bool PlayerAllowsMessenger() => !MenuConfig.Instance.DisableMessengerQuests;

		public bool PlayerAllowsMurder() => !MenuConfig.Instance.DisableMurderQuests;

		public bool PlayerAllowsDelivery() => !MenuConfig.Instance.DisableDeliveryQuests;

		/// <summary>
		/// Builds a List<> from the XML document to load stories into the game
		/// </summary>
		/// <returns>Returns true if it can successfully load and verify the Stories\Stories.xml file</returns>
		private bool ImportStoryXML()
        {
			// We're going to build the List<> before we commit it to the Instance
			List<Story> BuildingStories = new List<Story>();
			string _directorypath = Environment.CurrentDirectory;
			_directorypath = _directorypath.Replace("\\bin\\Win64_Shipping_Client", "") + "\\Modules\\DanqnasQuests\\bin\\Win64_Shipping_Client\\Stories\\";

			foreach (string xmlFile in Directory.EnumerateFiles(_directorypath, "*.xml"))
			{ 
				XmlDocument doc = new XmlDocument();				
				if (File.Exists(xmlFile))
				{
					doc.Load(xmlFile);
				}
				else
				{
					return false;
				}		

				foreach (XmlNode node in doc.DocumentElement.ChildNodes)
				{
					if (node.Name == "story")
					{
						// Checks whether we can continue with this node
						bool HasRequiredAttributes = true;

						// Check all the attributes exist and that the REQUIRED ones have values and set anotherone false if they do not

						string QuestName = string.Empty;
						if (node.Attributes["QuestName"] != null)
						{
							QuestName = node.Attributes["QuestName"]?.InnerText.Trim();
						}
						else { HasRequiredAttributes = false; }

						string PlayerDisposition = string.Empty;
						if (node.Attributes["PlayerDisposition"] != null)
						{
							PlayerDisposition = node.Attributes["PlayerDisposition"]?.InnerText.ToUpper().Trim();
						}
						else { HasRequiredAttributes = false; }

						string SpecificInitiator = string.Empty;
						if (node.Attributes["SpecificInitiator"] != null)
						{
							SpecificInitiator = node.Attributes["SpecificInitiator"]?.InnerText.ToUpper().Trim();
						}

						string InitiatorTestosteroneDominant = string.Empty;
						if (node.Attributes["InitiatorTestosteroneDominant"] != null)
						{
							InitiatorTestosteroneDominant = node.Attributes["InitiatorTestosteroneDominant"]?.InnerText.ToUpper().Trim();
						}

						//If it's empty, make it NEUTRAL
						if (InitiatorTestosteroneDominant == string.Empty)
						{
							InitiatorTestosteroneDominant = "NEUTRAL";
						}

						string Occupation = string.Empty;
						if (node.Attributes["Occupation"] != null)
						{
							Occupation = node.Attributes["Occupation"]?.InnerText.ToUpper().Trim();
						}
						if (Occupation == null || Occupation == string.Empty)
                        {
							Occupation = string.Empty;
                        }

						string SpecificTarget = string.Empty;
						if (node.Attributes["SpecificTarget"] != null)
						{
							SpecificTarget = node.Attributes["SpecificTarget"]?.InnerText.ToUpper().Trim();
						}

						string TargetTestosteroneDominant = string.Empty;
						if (node.Attributes["TargetTestosteroneDominant"] != null)
						{
							TargetTestosteroneDominant = node.Attributes["TargetTestosteroneDominant"]?.InnerText.ToUpper().Trim();
						}

						string TargetSameFaction = string.Empty;
						if (node.Attributes["TargetSameFaction"] != null)
						{
							TargetSameFaction = node.Attributes["TargetSameFaction"]?.InnerText.ToUpper().Trim();
						}

						//If it's empty, make it NEUTRAL
						if (TargetSameFaction == null || TargetSameFaction == string.Empty)
                        {
							TargetSameFaction = "NEUTRAL";
                        }

						string RelationRequirement = string.Empty;
						if (node.Attributes["RelationRequirement"] != null)
						{
							RelationRequirement = node.Attributes["RelationRequirement"]?.InnerText.ToUpper().Trim();
						}

						//If it's empty, make it NEUTRAL
						if (RelationRequirement == null || RelationRequirement == string.Empty)
                        {
							RelationRequirement = "NEUTRAL";
                        }

						string QuestType = string.Empty;
						if (node.Attributes["QuestType"] != null)
						{
							QuestType = node.Attributes["QuestType"]?.InnerText.ToUpper().Trim();
						}
						else { HasRequiredAttributes = false; }

						string Item = string.Empty;
						if (node.Attributes["Item"] != null)
						{
							Item = node.Attributes["Item"]?.InnerText.Trim();
						}
						else if (QuestType == "Delivery") { HasRequiredAttributes = false; }
						else { }

						int ItemQty = 1;
						if (Item != string.Empty && QuestType == "Delivery")
						{
							if (node.Attributes["ItemQty"] != null)
							{
								try
								{
									ItemQty = Convert.ToInt32(node.Attributes["ItemQty"]?.InnerText);
								}
								catch { }
							}
						}

						string RewardType = string.Empty;
						if (node.Attributes["RewardType"] != null)
						{
							RewardType = node.Attributes["RewardType"]?.InnerText.ToUpper().Trim();
						}
						else { HasRequiredAttributes = false; }

						int RewardQty = 0;
						if (node.Attributes["RewardQty"] != null)
						{
							try
							{
								RewardQty = Convert.ToInt32(node.Attributes["RewardQty"]?.InnerText);
							}
							catch { RewardQty = 0; }
						}

						int TimeDays = 0;
						if (node.Attributes["TimeDays"] != null)
						{
							try
							{
								TimeDays = Convert.ToInt32(node.Attributes["TimeDays"]?.InnerText);
							}
							catch { TimeDays = 0; }
						}

						string StartQuestPlayerDialogue = string.Empty;
						if (node.Attributes["StartQuestPlayerDialogue"] != null)
						{
							StartQuestPlayerDialogue = node.Attributes["StartQuestPlayerDialogue"]?.InnerText.Trim();
						}
						else { HasRequiredAttributes = false; }

						string StartQuestNPCDialogue = string.Empty;
						if (node.Attributes["StartQuestNPCDialogue"] != null)
						{
							StartQuestNPCDialogue = node.Attributes["StartQuestNPCDialogue"]?.InnerText.Trim();
						}
						else { HasRequiredAttributes = false; }

						string OptionalStartQuestPlayerDialogue = string.Empty;
						if (node.Attributes["OptionalStartQuestPlayerDialogue"] != null)
						{
							OptionalStartQuestPlayerDialogue = node.Attributes["OptionalStartQuestPlayerDialogue"]?.InnerText.Trim();
						}

						string OptionalStartQuestNPCDialogue = string.Empty;
						if (node.Attributes["OptionalStartQuestNPCDialogue"] != null)
						{
							OptionalStartQuestNPCDialogue = node.Attributes["OptionalStartQuestNPCDialogue"]?.InnerText.Trim();
						}

						string InterimQuestPlayerDialogue = string.Empty;
						if (node.Attributes["InterimQuestPlayerDialogue"] != null)
						{
							InterimQuestPlayerDialogue = node.Attributes["InterimQuestPlayerDialogue"]?.InnerText.Trim();
						}

						string InterimQuestNPCDialogue = string.Empty;
						if (node.Attributes["InterimQuestNPCDialogue"] != null)
						{
							InterimQuestNPCDialogue = node.Attributes["InterimQuestNPCDialogue"]?.InnerText.Trim();
						}

						string OptionalInterimQuestPlayerDialogue = string.Empty;
						if (node.Attributes["OptionalInterimQuestPlayerDialogue"] != null)
						{
							OptionalInterimQuestPlayerDialogue = node.Attributes["OptionalInterimQuestPlayerDialogue"]?.InnerText.Trim();
						}

						string OptionalInterimQuestNPCDialogue = string.Empty;
						if (node.Attributes["OptionalInterimQuestNPCDialogue"] != null)
						{
							OptionalInterimQuestNPCDialogue = node.Attributes["OptionalInterimQuestNPCDialogue"]?.InnerText.Trim();
						}

						string EndQuestPlayerDialogue = string.Empty;
						if (node.Attributes["EndQuestPlayerDialogue"] != null)
						{
							EndQuestPlayerDialogue = node.Attributes["EndQuestPlayerDialogue"]?.InnerText.Trim();
						}
						else { HasRequiredAttributes = false; }

						string EndQuestNPCDialogue = string.Empty;
						if (node.Attributes["EndQuestNPCDialogue"] != null)
						{
							EndQuestNPCDialogue = node.Attributes["EndQuestNPCDialogue"]?.InnerText.Trim();
						}
						else { HasRequiredAttributes = false; }

						string OptionalEndQuestPlayerDialogue = string.Empty;
						if (node.Attributes["OptionalEndQuestPlayerDialogue"] != null)
						{
							OptionalEndQuestPlayerDialogue = node.Attributes["OptionalEndQuestPlayerDialogue"]?.InnerText.Trim();
						}

						string OptionalEndQuestNPCDialogue = string.Empty;
						if (node.Attributes["OptionalEndQuestNPCDialogue"] != null)
						{
							OptionalEndQuestNPCDialogue = node.Attributes["OptionalEndQuestNPCDialogue"]?.InnerText.Trim();
						}

						string MultiPart = string.Empty;
						if (node.Attributes["MultiPart"] != null)
						{
							MultiPart = node.Attributes["MultiPart"]?.InnerText.ToUpper().Trim();
						}

						bool _isMultiPart = false;
						string MultiKey = string.Empty;
						int PartNumber = 0;
						int TotalParts = 0;

						// We only have to fill this out if MultiPart is true
						if (MultiPart == "TRUE" && SpecificInitiator.Trim() != string.Empty)
						{
							// Might as well pass it through as a bool
							_isMultiPart = true;

							if (node.Attributes["MultiKey"] != null)
							{
								MultiKey = node.Attributes["MultiKey"]?.InnerText.Trim();
							}
							else { HasRequiredAttributes = false; }

							if (node.Attributes["PartNumber"] != null)
							{
								PartNumber = Convert.ToInt32(node.Attributes["PartNumber"]?.InnerText.Trim());
							}

							if (node.Attributes["TotalParts"] != null)
							{
								TotalParts = Convert.ToInt32(node.Attributes["TotalParts"]?.InnerText);
							}
						}						

						string LogStart = string.Empty;
						if (node.Attributes["LogStart"] != null)
						{
							LogStart = node.Attributes["LogStart"]?.InnerText.Trim();
						}
						else { HasRequiredAttributes = false; }

						string LogFinish = string.Empty;
						if (node.Attributes["LogFinish"] != null)
						{
							LogFinish = node.Attributes["LogFinish"]?.InnerText.Trim();
						}
						else { HasRequiredAttributes = false; }

						string Quest_ID = QuestName.Replace(" ", string.Empty);

						//Fix the time if they didn't set it
						if (TimeDays == 0 || MenuConfig.Instance.OverrideTime)
						{
							if (MenuConfig.Instance.OverrideTime)
							{
								TimeDays = MenuConfig.Instance.SetTime;
							}
							else
							{
								TimeDays = 30;
							}
						}

						if (HasRequiredAttributes)
						{
							Story OurStory = new Story
							{
								QuestName = QuestName,
								QuestNameID = Quest_ID,
								PlayerDisposition = PlayerDisposition,
								SpecificInitiator = SpecificInitiator,
								InitiatorTestosteroneDominant = InitiatorTestosteroneDominant,
								Occupation = Occupation,
								SpecificTarget = SpecificTarget,
								TargetTestosteroneDominant = TargetTestosteroneDominant,
								TargetSameFaction = TargetSameFaction,
								RelationRequirement = RelationRequirement,
								QuestType = QuestType,
								Item = Item,
								ItemQty = ItemQty,
								RewardType = RewardType,
								RewardQty = RewardQty,
								TimeDays = TimeDays,
								StartQuestPlayerDialogue = StartQuestPlayerDialogue,
								StartQuestNPCDialogue = StartQuestNPCDialogue,
								OptionalStartQuestPlayerDialogue = OptionalStartQuestPlayerDialogue,
								OptionalStartQuestNPCDialogue = OptionalStartQuestNPCDialogue,
								InterimQuestPlayerDialogue = InterimQuestPlayerDialogue,
								InterimQuestNPCDialogue = InterimQuestNPCDialogue,
								OptionalInterimQuestPlayerDialogue = OptionalInterimQuestPlayerDialogue,
								OptionalInterimQuestNPCDialogue = OptionalInterimQuestNPCDialogue,
								EndQuestPlayerDialogue = EndQuestPlayerDialogue,
								EndQuestNPCDialogue = EndQuestNPCDialogue,
								OptionalEndQuestPlayerDialogue = OptionalEndQuestPlayerDialogue,
								OptionalEndQuestNPCDialogue = OptionalEndQuestNPCDialogue,
								MultiPart = _isMultiPart,
								MultiKey = MultiKey,
								PartNumber = PartNumber,
								TotalParts = TotalParts,
								LogStart = LogStart,
								LogFinish = LogFinish
							};

							BuildingStories.Add(OurStory);
						}
					}
				}
			}

			CurrentStory.Instance.CurrentStories = BuildingStories;

			if (CurrentStory.Instance.CurrentStories.Count() > 0)
			{
				return true;
			}
			else
            {
				return false;
            }
        }

		public Hero GetMeAHero()
		{
			List<Hero> alltraders = Hero.AllAliveHeroes.ToList();
			Hero ToHaveAndToHold = null;

			List<Hero> filter1 = alltraders.Where((a) => a.Occupation == Occupation.Lord && !a.IsChild && !a.Noncombatant).ToList();

			while (ToHaveAndToHold == null)
			{
				Random rnd = new Random();
				int ourluckyhero = rnd.Next(0, (filter1.Count() - 1));
				ToHaveAndToHold = filter1[ourluckyhero];
			}

			return ToHaveAndToHold;
		}

		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			// Check if we have a valid XML file and warn if we don't
			if (ImportStoryXML())
			{
				AddDialogs(campaignGameStarter);
			}
			else
			{
				InformationManager.DisplayMessage(new InformationMessage("Unable to validate the XML file"));
			}
		}

		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(OnSessionLaunched));
		}

		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}