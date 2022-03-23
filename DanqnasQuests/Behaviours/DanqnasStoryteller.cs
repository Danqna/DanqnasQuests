using DanqnasQuests.Models;
using DanqnasQuests.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.GauntletUI;
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
						// ID is FILENAME_STORYID. This HAS TO BE UNIQUE.
						Stories[i].Id = $"{finfo.Name}_{i}";
					}
				}
			}
			foreach(Story story in Stories)
            {
				var instance = QuestInstances?.FirstOrDefault(r => r?.Id == story.Id);
				if (instance == null) // Check if we have the story already saved. If not add it to the save
				{
					QuestInstances.Add(new QuestInstance()
					{
						Id = story.Id,
						StoryStage = 0
					});
				}


				foreach(Target target in story.Targets)
                {
                    for (int i = 0; i < target.Dialogs.Count; i++)
                    {
						Dialog diag = target.Dialogs[i];
						if (i == 0) // entry
                        {
							campaignGameStarter.AddPlayerLine($"{story.QuestName}_{i}_Dialog",
								"hero_main_options",
								$"{story.QuestName}_{i}_Dialog",
								diag.PlayerDialog,
								() => // condition
								{
									// Checks if the saved QuestInstance Stage is equal to the StoryStage of the dialog
									var stage = QuestInstances?.FirstOrDefault(r => r.Id == story.Id).StoryStage;
									return stage == diag.StoryStage;
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
									"hero_main_options",
									choice.NPCDialog,
									() => // condition
									{
										return true;
									}, 
									() => // consequence
									{
										if(choice.Action == Enums.QuestActions.ACCEPT)
                                        {
											AddLog()
											QuestInstances.FirstOrDefault(r => r.Id == story.Id).StoryStage = diag.StoryStage + 1;
											InformationManager.DisplayMessage(new InformationMessage("You accepted the quest"));
										} else
                                        {
											InformationManager.DisplayMessage(new InformationMessage("You aborted the quest"));
										}
									} // Action
									, 100, null);
							}
						}
					}
                }
			}

			// Check if we have a valid XML file and warn if we don't
			/*if (ImportStoryXML())
			{
				//AddDialogs(campaignGameStarter);
			}
			else
			{*/
			//InformationManager.DisplayMessage(new InformationMessage("Unable to validate the XML file"));
			//}
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