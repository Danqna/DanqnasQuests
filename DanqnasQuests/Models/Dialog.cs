using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.SaveSystem;

namespace DanqnasQuests.Models
{
    class Dialog
    {
        [SaveableProperty(1)] public string PlayerDialog { get; set; }
        [SaveableProperty(2)] public string NPCDialog { get; set; }
        /// <summary>
        /// Required state of the quest to trigger this dialog
        /// </summary>
        [SaveableProperty(3)] public int StoryStage { get; set; }
    }
}
