using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.SaveSystem;

namespace DanqnasQuests.Models
{
    public class Dialog
    {
        public string NPCDialog { get; set; }
        /// <summary>
        /// Required state of the quest to trigger this dialog
        /// </summary>
        public int StoryStage { get; set; }
        public List<PlayerChoice> PlayerChoices { get; set; } = new List<PlayerChoice>();
        public string PlayerDialog { get; set; }
    }
}
