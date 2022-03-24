using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace DanqnasQuests.Models
{
    public class CustomQuests : QuestBase
    {
        public override TextObject Title => new TextObject("Quest Title");

        public override bool IsRemainingTimeHidden => false;

        public string inputToken { get; set; }
        
        public Story Story { get; set; }
        public int StoryStage { get; set; } = 0;

        public static bool DialogsSetup = false;

        public CustomQuests(string questId, Hero questGiver, CampaignTime duration, int rewardGold, Story story) : 
            base(questId, questGiver, duration, rewardGold)
        {
            Story = story;
            SetDialogs();
        }

        protected override void InitializeQuestOnGameLoad()
        {
            SetDialogs();
        }

        protected override void SetDialogs()
        {
            if (Story is null)
                return;
        }
    }
}
