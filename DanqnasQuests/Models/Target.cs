using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem;
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
        public Occupation? Occupation { get; set; }
        public bool AllOfType { get; set; }
        public string? Settlement { get; set; }
        public Enums.TargetType TargetType { get; set; }
        public Gender Gender { get; set; }
        public Faction Faction { get; set; }
        public List<Dialog> Dialogs { get; set; } = new List<Dialog>();
        [XmlIgnore]
        public List<Hero> Hero { get; set; } = new List<Hero>();
        [XmlIgnore]
        public Dictionary<Hero, QuestBase> Quests { get; set; } = new Dictionary<Hero, QuestBase>();
    }
#nullable disable
}
