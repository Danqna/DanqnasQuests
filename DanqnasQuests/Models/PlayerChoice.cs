using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.SaveSystem;
using static DanqnasQuests.Models.Enums;

namespace DanqnasQuests.Models
{
    public class PlayerChoice
    {
        public string PlayerDialog { get; set; }
        public string NPCDialog { get; set; }
        public QuestActions Action { get; set; }

    }
}
