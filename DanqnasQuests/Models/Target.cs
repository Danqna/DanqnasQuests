using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.SaveSystem;
using static DanqnasQuests.Models.Enums;

namespace DanqnasQuests.Models
{
#nullable enable
    public class Target
    {
        /// <summary>
        /// Name of the NPC, can be null if it should be generated
        /// </summary>
        public string? Name { get; set; }
        public Occupation Occupation { get; set; }
        public Gender Gender { get; set; }
        public Faction Faction { get; set; }
        public List<Dialog> Dialogs { get; set; } = new List<Dialog>();

    }
#nullable disable
}
