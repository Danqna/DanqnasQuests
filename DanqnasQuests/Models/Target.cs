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
    class Target
    {
        /// <summary>
        /// Name of the NPC, can be null if it should be generated
        /// </summary>
        [SaveableProperty(1)] public string? Name { get; set; }
        [SaveableProperty(2)] public Occupation Occupation { get; set; }
        [SaveableProperty(3)] public Gender Gender { get; set; }
        [SaveableProperty(4)] public Faction Faction { get; set; }
        [SaveableProperty(4)] public List<Dialog> Dialogs { get; set; }

    }
#nullable disable
}
