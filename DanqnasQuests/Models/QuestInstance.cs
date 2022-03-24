using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace DanqnasQuests.Models
{
    public class QuestInstance
    {
        [SaveableProperty(1)] public string Id { get; set; }
        [SaveableProperty(2)] public int StoryStage { get; set; }
        /// <summary>
        /// Indicates if the quest has been completed (either successful or not)
        /// </summary>
        [SaveableProperty(3)] public bool QuestCompleted { get; set; } = false;
        [SaveableProperty(4)] public Hero Hero { get; set; }
        public QuestBase Quest { get; set; }
    }
}
