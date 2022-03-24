using DanqnasQuests.Models;
using DanqnasQuests.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace DanqnasQuests.Behaviours
{
	class DanqnasStoryteller : CampaignBehaviorBase
	{

		public string StoryPath = Environment.CurrentDirectory.Replace(@"\bin\Win64_Shipping_Client", @"\Modules\DanqnasQuests\bin\Win64_Shipping_Client\Stories");
		public List<Story> Stories = new List<Story>();

		[SaveableField(0)] public List<QuestInstance> QuestInstances = new List<QuestInstance>();

		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			#region generationtest
			/*
			var q = new Story()
			{
				Item = "123",
				ItemQty = 1,
				LogFinish = "Finished Log",
				LogStart = "Start Log",
				PlayerDisposition = "Positive",
				QuestName = "Quest name",
				QuestNameID = "asd1",
				QuestType = "MESSENGER",
				RelationRequirement = "10",
				RewardQty = 1,
				RewardType = "GOLD",
				SpecificInitiator = "Initiator",
				Targets = new List<Target>()
                {
					new Target()
                    {
						Faction = Enums.Faction.SameFaction,
						Gender = Enums.Gender.Male,
						Name = "Name of npc",
						Occupation = Enums.Occupation.Armorer,
						Dialogs = new List<Dialog>()
                        {
							new Dialog()
                            {
								NPCDialog = "Hello Friend",
								PlayerDialog = "How ya doin?",
								StoryStage = 1
                            },
							new Dialog()
							{
								NPCDialog = "You are back!",
								PlayerDialog = "Yes I am!",
								StoryStage = 2
							},
						}
                    }
                },
				TimeDays = 30
			};
			var storis = new List<Story>();
			storis.Add(q);
			XmlSerializer xml = new XmlSerializer(typeof(List<Story>));

			using (var sw = new StringWriter())
            {
				using (XmlWriter writer = XmlWriter.Create(sw))
                {
					xml.Serialize(writer, storis);
					var xmlOutput = sw.ToString();
					File.WriteAllText(StoryPath + @"\Storys.xml", xmlOutput);
				}
			}*/
			#endregion

			InformationManager.DisplayMessage(new InformationMessage("Preload"));
			foreach (string file in Directory.GetFiles(StoryPath))
			{
				FileInfo finfo = new FileInfo(file);
				XmlSerializer ser = new XmlSerializer(typeof(List<Story>));
				using (XmlReader read = XmlReader.Create(file))
				{
					Stories = (List<Story>)ser.Deserialize(read);
					for (int i = 0; i < Stories.Count; i++)
					{
						// ID is FILENAME_STORYID. THIS HAS TO BE UNIQUE.
						Stories[i].Id = $"{finfo.Name}_{i}";
					}
				}
			}

			foreach(Story story in Stories)
            {
				List<Hero> heroes = new List<Hero>();
				Random rand = new Random();
				int randomHeroIndex;
				// Setup Hero
				// Occupation is only applied when TargetType is Notable
				foreach(Target target in story.Targets)
                {
					if(target.Name is null)
                    {
						switch(target.TargetType)
                        {
							case Enums.TargetType.Lord:
								heroes = Hero.AllAliveHeroes.Where(r => r.IsNoble).ToList();
                                break;
							case Enums.TargetType.Notable:
								heroes = Hero.AllAliveHeroes.Where(r => r.IsNotable).ToList();
								if (target.Occupation != null)
									heroes = heroes.Where(r => r.Occupation == target.Occupation).ToList();
								break;
							case Enums.TargetType.Ruler:
								heroes = Hero.AllAliveHeroes.Where(r => r.IsFactionLeader).ToList();
								break;
							case Enums.TargetType.Bandit:
								heroes = Hero.AllAliveHeroes.Where(r => r.Occupation == Occupation.Bandit).ToList();
								break;
                        }

						// Filter based on Faction
						if (target.Faction == Enums.Faction.OtherFaction)
							heroes = heroes.Where(r => r.MapFaction != Hero.MainHero.MapFaction).ToList();
						else if (target.Faction == Enums.Faction.SameFaction)
							heroes = heroes.Where(r => r.MapFaction == Hero.MainHero.MapFaction).ToList();

						// Filter based on Gender
						if (target.Gender == Enums.Gender.Male)
							heroes = heroes.Where(r => !r.IsFemale).ToList();
						else if (target.Gender == Enums.Gender.Female)
							heroes = heroes.Where(r => r.IsFemale).ToList();

						// Filter based on Settlement
						if(target.Settlement != null)
							heroes = heroes.Where(r => r.CurrentSettlement == Settlement.FindFirst(r => r.Name.Contains(target.Settlement))).ToList();

						randomHeroIndex = rand.Next(0, heroes.Count() - 1);
						if (target.AllOfType)
							target.Hero = heroes;
						else
							target.Hero = new List<Hero>() { heroes[randomHeroIndex] };
					} else
                    {
						// Specific hero
						target.Hero = new List<Hero>() { Hero.FindFirst(r => r.Name.Contains(target.Name)) };
                    }
                }
				// End Setup Hero

				foreach (Target target in story.Targets)
                {

					// Setup Quests
					// Create a quest for each target
					foreach(Hero hero in target.Hero)
                    {
						string hashString = $"{story.Id}_{hero.Name}";

						var quest = new CustomQuests(hashString,
							hero,
							CampaignTime.DaysFromNow(10),
							100, story);

						var instance = QuestInstances?.FirstOrDefault(r => r?.Id == story.Id && r.Hero == hero);
						if (instance == null) // Check if we have the story already saved. If not add it to the save
						{
							QuestInstances.Add(new QuestInstance()
							{
								Id = story.Id,
								Hero = hero,
								Quest = quest,
								StoryStage = 0
							});
						} else
                        {
							// Quest already exists in QuestInstances
                        }
						target.Quests.Add(hero, quest);
                    }

                    for (int i = 0; i < target.Dialogs.Count; i++)
                    {
						Dialog diag = target.Dialogs[i];

						campaignGameStarter.AddPlayerLine($"{story.QuestName}_{i}_Dialog",
							"hero_main_options",
							$"{story.QuestName}_{i}_Dialog",
							diag.PlayerDialog,
							() => // condition
							{
								// Checks if the saved QuestInstance Stage is equal to the StoryStage of the dialog
								// and if we're currently talking with a hero matching the target(s)
								if (Hero.OneToOneConversationHero == null)
									return false;
								var stage = QuestInstances?.FirstOrDefault(r => r.Id == story.Id && r.Hero == Hero.OneToOneConversationHero && !r.QuestCompleted)?.StoryStage;
								if (stage is null)
                                {
									return false;
                                }
								return stage == diag.StoryStage && target.Hero.Contains(Hero.OneToOneConversationHero);
							}, 
							null, 100, null, null);
						InformationManager.DisplayMessage(new InformationMessage("added playerline with output " + $"{story.QuestName}_{i + 1}_Dialog"));

						InformationManager.DisplayMessage(new InformationMessage("adding dialog " + diag.NPCDialog));

						campaignGameStarter.AddDialogLine($"{story.QuestName}_{i}_Dialog_Response",
							$"{story.QuestName}_{i}_Dialog",
							$"{story.QuestName}_{i}_Dialog_Response",
							diag.NPCDialog,
							null, null, 100, null);

						InformationManager.DisplayMessage(new InformationMessage("added dialogline with input " + $"{story.QuestName}_{i}_Dialog" + " output " + $"{story.QuestName}_{i}_Dialog_Response"));
							
						for (int l = 0; l < diag.PlayerChoices.Count; l++)
						{
							PlayerChoice choice = diag.PlayerChoices[l];

							campaignGameStarter.AddPlayerLine($"{story.QuestName}_{i}_Dialog_{l}_Dialog",
								$"{story.QuestName}_{i}_Dialog_Response",
								$"{story.QuestName}_{i}_Dialog_Response_End_{l}",
								choice.PlayerDialog,
								() => true, null, 100, null, null);

							campaignGameStarter.AddDialogLine($"{story.QuestName}_{i}_Dialog_{l}_Dialog_Response",
								$"{story.QuestName}_{i}_Dialog_Response_End_{l}",
								$"{story.QuestName}_{i}_Dialog_Response_End_{l}_StartQuest",
								choice.NPCDialog,
								() => // condition
								{
									if (Hero.OneToOneConversationHero == null)
										return false;
									var stage = QuestInstances?.FirstOrDefault(r => r.Id == story.Id && r.Hero == Hero.OneToOneConversationHero && !r.QuestCompleted)?.StoryStage;
									if (stage is null)
									{
										return false;
									}
									return stage == diag.StoryStage && target.Hero.Contains(Hero.OneToOneConversationHero);
								}, 
								() => // consequence
								{
									var quest = target.Quests.FirstOrDefault(r => r.Key == Hero.OneToOneConversationHero).Value;
									var questInstance = QuestInstances.FirstOrDefault(r => r.Id == story.Id && r.Hero == Hero.OneToOneConversationHero);
									if (choice.LogEntry != null)
									{
										// TODO: Add variables for text (eg. heroes, settlements)
										switch (choice.LogEntry.Type)
										{
											case Enums.LogEntryType.Text:
												quest.AddLog(new TextObject(choice.LogEntry.Text, null));
												break;
											case Enums.LogEntryType.ProgressBar:
												quest.AddDiscreteLog(new TextObject(choice.LogEntry.Text, null),
													new TextObject(choice.LogEntry.Description, null),
													choice.LogEntry.Progress,
													choice.LogEntry.ProgressMax);
												break;
											case Enums.LogEntryType.TwoWay:
												quest.AddTwoWayContinuousLog(new TextObject(choice.LogEntry.Text, null),
													new TextObject(choice.LogEntry.Description, null),
													choice.LogEntry.Progress,
													choice.LogEntry.TwoWayRange);
												break;
										}
									}

									// Handle quest Action
									switch (choice.Action)
                                    {
										case Enums.QuestActions.Accept:
											questInstance.StoryStage = diag.StoryStage + 1;
											InformationManager.DisplayMessage(new InformationMessage("You accepted the quest"));
											quest.StartQuest();
											break;
										case Enums.QuestActions.Finish:
											quest.CompleteQuestWithSuccess();
											questInstance.QuestCompleted = true;
											break;
										case Enums.QuestActions.Cancel:
											quest.CompleteQuestWithCancel();
											questInstance.QuestCompleted = true;
											break;
										case Enums.QuestActions.Betrayal:
											quest.CompleteQuestWithBetrayal();
											questInstance.QuestCompleted = true;
											break;
                                    }
									
								}
								, 100, null);
						}
					}
                }
			}
		}

		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(OnSessionLaunched));
		}

        public override void SyncData(IDataStore dataStore)
        {
			dataStore.SyncData("QuestInstances", ref QuestInstances);
        }
    }
}