using DanqnasQuests.Behaviours;
using DanqnasQuests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace DanqnasQuests.Quests
{
    internal class DanqnaQuest : QuestBase
    {
        [SaveableProperty(1)] public int DefaultReward { get; set; }
        [SaveableProperty(2)] public Hero Target { get; set; }
        [SaveableProperty(3)] public bool QuestRunning { get; set; }      
        [SaveableProperty(4)] public Story StoryQuest { get; set; }
        [SaveableProperty(5)] public int DialogueStage { get; set; }
        [SaveableProperty(6)] public List<Hero> HeroesWithActiveQuests { get; set; }
        /// <summary>
        /// MultiPartQuests[0][0] == Questname
        /// MultiPartQuests[0][1] == CurrentPart
        /// </summary>        
        [SaveableProperty(7)] public List<List<string>> MultiPartQuests { get; set; }

        public static List<Hero> PublicModifiableHeroesWithActiveQuests { get; set; }
        public static List<List<string>> PublicModifiableMultiPartQuests { get; set; }

        public override bool IsSpecialQuest => true;        

        // Log Entries
        private TextObject StartQuestLog
        {
            get
            {
                TextObject danqlog;
                if (StoryQuest.QuestType == "MURDER")
                {
                    danqlog = new TextObject("Murder target is: " + Target + " - " + StoryQuest.LogStart);
                }
                else
                {
                    danqlog = new TextObject(StoryQuest.LogStart);
                }

                // Add the questgiver name
                string newlog = "Giver: " + QuestGiver.Name.ToString() + ", Target: " + Target + " - " + danqlog.ToString();
                danqlog = new TextObject(newlog);

                return danqlog;
            }
        }

        private TextObject EndQuestLog
        {
            get
            {
                TextObject danqlog = new TextObject(StoryQuest.LogFinish);
                return danqlog;
            }
        }

        private TextObject FailQuestLog
        {
            get
            {
                TextObject danqlog = new TextObject("You can't win 'em all, champ.");
                return danqlog;
            }
        }

        /*private TextObject CancelQuestLog
        {
            get
            {
                TextObject danqlog = new TextObject("When you try your best but you don't succeed. When you get what you want, but not what you need.");
                return danqlog;
            }
        }*/

        // Quest initiation
        public DanqnaQuest(Hero questGiver, int reward, Hero senttarget, bool questrunning, Story NewQuest, int TimeDays) : base(NewQuest.QuestNameID, questGiver, duration: CampaignTime.DaysFromNow(TimeDays), rewardGold: reward)
        {
            DefaultReward = reward;
            Target = senttarget;
            QuestRunning = questrunning;
            StoryQuest = NewQuest;
            DialogueStage = 0;
            SetDialogs();
            AddActiveHero(QuestGiver);
            MultiPartQuests = PublicModifiableMultiPartQuests;       
            
            
            AddTrackedObject(QuestGiver);            
        }

        protected override void RegisterEvents()
        {
            CampaignEvents.TickEvent.AddNonSerializedListener(this, Tick);
        }

        protected void Tick(float dt)
        {
            MultiPartQuests = PublicModifiableMultiPartQuests;
        }

        protected override void SetDialogs()
        {
            string StartQuestPlayerDialogue = StoryQuest.StartQuestPlayerDialogue;
            string StartQuestNPCDialogue = StoryQuest.StartQuestNPCDialogue;
            string OptionalStartQuestPlayerDialogue = StoryQuest.OptionalStartQuestPlayerDialogue;
            string OptionalStartQuestNPCDialogue = StoryQuest.OptionalStartQuestNPCDialogue;
            string InterimQuestPlayerDialogue = StoryQuest.InterimQuestPlayerDialogue;
            string InterimQuestNPCDialogue = StoryQuest.InterimQuestNPCDialogue;
            string OptionalInterimQuestPlayerDialogue = StoryQuest.OptionalInterimQuestPlayerDialogue;
            string OptionalInterimQuestNPCDialogue = StoryQuest.OptionalInterimQuestNPCDialogue;
            string EndQuestPlayerDialogue = StoryQuest.EndQuestPlayerDialogue;
            string EndQuestNPCDialogue = StoryQuest.EndQuestNPCDialogue;
            string OptionalEndQuestPlayerDialogue = StoryQuest.OptionalEndQuestPlayerDialogue;
            string OptionalEndQuestNPCDialogue = StoryQuest.OptionalEndQuestNPCDialogue;

            if (OptionalStartQuestPlayerDialogue == string.Empty)
            {
                OptionalStartQuestPlayerDialogue = "I'll be off on that business.";
            }

            if (OptionalStartQuestNPCDialogue == string.Empty)
            {
                OptionalStartQuestNPCDialogue = "Right.";
            }

            if (InterimQuestPlayerDialogue == string.Empty)
            {
                InterimQuestPlayerDialogue = "I'm still working on that job.";
            }

            if (InterimQuestNPCDialogue == string.Empty)
            {
                InterimQuestNPCDialogue = "I'm sure it's in capable hands.";
            }

            if (OptionalInterimQuestNPCDialogue == string.Empty)
            {
                OptionalInterimQuestNPCDialogue = "Bye.";
            }

            if (OptionalInterimQuestPlayerDialogue == string.Empty)
            {
                OptionalInterimQuestPlayerDialogue = "I'll be off!";
            }

            if (OptionalEndQuestNPCDialogue == string.Empty)
            {
                OptionalEndQuestNPCDialogue = "I'll be seeing you around.";
            }

            if (OptionalEndQuestPlayerDialogue == string.Empty)
            {
                OptionalEndQuestPlayerDialogue = "Well I'll be on my way.";
            }


            DialogFlow StartQuestDialogue = DialogFlow.CreateDialogFlow(QuestManager.HeroMainOptionsToken, 110)
                .PlayerLine(new TextObject(StartQuestPlayerDialogue))
                .Condition(new ConversationSentence.OnConditionDelegate(InitialQuestDialogueCondition))
                .NpcLine(new TextObject(StartQuestNPCDialogue))
                .Consequence(new ConversationSentence.OnConsequenceDelegate(SetDialogueStage1))
                .PlayerLine(new TextObject(OptionalStartQuestPlayerDialogue))
                .NpcLine(new TextObject(OptionalStartQuestNPCDialogue))
                .CloseDialog()
                .GotoDialogState("hero_main_options");

            DialogFlow InterimQuestDialog = DialogFlow.CreateDialogFlow(QuestManager.HeroMainOptionsToken, 110)
                .PlayerLine(new TextObject(InterimQuestPlayerDialogue))
                .Condition(new ConversationSentence.OnConditionDelegate(InterimQuestDialogueCondition))                
                .NpcLine(new TextObject(InterimQuestNPCDialogue))
                .PlayerLine(new TextObject(OptionalInterimQuestPlayerDialogue))
                .Condition(() => Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == QuestGiver && DialogueStage == 1 && OptionalInterimQuestPlayerDialogue != string.Empty)
                .NpcLine(new TextObject(OptionalInterimQuestNPCDialogue))
                .Condition(() => OptionalInterimQuestNPCDialogue != string.Empty)
                .BeginPlayerOptions()
                    .PlayerOption(new TextObject("Actually, on second thought I give up."), null)
                        .NpcLine(new TextObject("So you are altering the deal? Very well then...I will pray that you do not alter it further."))
                        .Consequence(new ConversationSentence.OnConsequenceDelegate(FailTheQuest))
                    .CloseDialog()
                    .PlayerOption(new TextObject("I'll just be on my way then."), null)
                        .NpcLine(new TextObject("Au revoir, that's French for \"make like a bee.\""))
                    .CloseDialog()
                .EndPlayerOptions()
                .CloseDialog()
                .GotoDialogState("hero_main_options");

            DialogFlow EndQuestDialogue = DialogFlow.CreateDialogFlow(QuestManager.HeroMainOptionsToken, 110)
                .PlayerLine(new TextObject(EndQuestPlayerDialogue))
                .Condition(new ConversationSentence.OnConditionDelegate(PlayerSatisfiesQuest))
                .NpcLine(new TextObject(EndQuestNPCDialogue))
                .Consequence(new ConversationSentence.OnConsequenceDelegate(FinishQuest))
                .PlayerLine(new TextObject(OptionalEndQuestPlayerDialogue))
                .Condition(() => OptionalStartQuestPlayerDialogue != string.Empty)
                .NpcLine(new TextObject(OptionalEndQuestNPCDialogue))
                .Condition(() => OptionalStartQuestNPCDialogue != string.Empty)
                .CloseDialog()
                .GotoDialogState("hero_main_options");

            Campaign.Current.ConversationManager.AddDialogFlow(StartQuestDialogue, this);
            Campaign.Current.ConversationManager.AddDialogFlow(InterimQuestDialog, this);
            Campaign.Current.ConversationManager.AddDialogFlow(EndQuestDialogue, this);
        }

        private void FinishQuest()
        {
            CompleteQuestWithSuccess();
            GiveGoldAction.ApplyForQuestBetweenCharacters(QuestGiver, Hero.MainHero, DefaultReward, false);
            AddLog(EndQuestLog);
        }

        private void FailTheQuest()
        {
            CompleteQuestWithFail();
            AddLog(FailQuestLog);
        }

        protected override void InitializeQuestOnGameLoad()
        {         

            SetDialogs();
        }

        public override bool IsRemainingTimeHidden
        {
            get
            {
                return false;
            }
        }

        public override TextObject Title
        {
            get
            {
                TextObject parent = new TextObject(StoryQuest.QuestName);
                return parent;
            }
        }

        private void SetDialogueStage1()
        {
            AddLog(StartQuestLog);
            DialogueStage = 1;
        }

        private bool PlayerSatisfiesQuest()
        {
            if (Target == Hero.OneToOneConversationHero)
            {
                if (StoryQuest.QuestType == "DELIVERY")
                {
                    var beer = MBObjectManager.Instance.GetObject<ItemObject>(StoryQuest.Item);
                    int PlayerHoldingQty = MobileParty.MainParty.ItemRoster.GetItemNumber(beer);
                    if (PlayerHoldingQty >= StoryQuest.ItemQty)
                    {
                        return true;
                    }

                    // Some learning code, useful to figure out what I'm doing
                    /*
                    int idx = MobileParty.MainParty.ItemRoster.FindIndexOfItem(beer);
                    if (idx >= 0)
                    {
                        // drop one grain
                        mobileParty.ItemRoster.AddToCountsAtIndex(idx, -1);
                    }*/


                    //ItemObject 
                }
                else if (StoryQuest.QuestType == "MURDER")
                {
                    if (Hero.OneToOneConversationHero == QuestGiver && Target.IsDead)
                    {
                        return true;
                    }
                }
                else if (StoryQuest.QuestType == "MESSENGER")
                {
                    if (Hero.OneToOneConversationHero == Target)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        private bool InitialQuestDialogueCondition()
        { 
            if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == QuestGiver && DialogueStage == 0)
            {
                return true;
            }

            return false;
        }

        private bool InterimQuestDialogueCondition()
        {
            if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == QuestGiver && DialogueStage == 1 && !PlayerSatisfiesQuest())
            {
                return true;
            }
            return false;
        }

        private void AddActiveHero(Hero AdditionalHero)
        {
            if (HeroesWithActiveQuests == null)
            {
                HeroesWithActiveQuests = new List<Hero>();
            }

            HeroesWithActiveQuests.Add(AdditionalHero);
            PublicModifiableHeroesWithActiveQuests = HeroesWithActiveQuests;
        }

        private void RemoveActiveHero(Hero RemovalHero)
        {
            List<Hero> ReplaceList = new List<Hero>();
            foreach(Hero hero in HeroesWithActiveQuests)
            {
                if(hero != RemovalHero)
                {
                    ReplaceList.Add(hero);
                }
            }

            HeroesWithActiveQuests = ReplaceList;
            PublicModifiableHeroesWithActiveQuests = HeroesWithActiveQuests;
        }

        // Called after the quest is finished
        protected override void OnFinalize()
        {
            RemoveActiveHero(QuestGiver);
            base.OnFinalize();
        }        
    }
}