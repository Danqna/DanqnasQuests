using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.SaveSystem;

namespace DanqnasQuests.Models
{
    public class QuestInstance
    {
        [SaveableProperty(1)] public string Id { get; set; }
        [SaveableProperty(2)] public int? StoryStage { get; set; }
    }
}
