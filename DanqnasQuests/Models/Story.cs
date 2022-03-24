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
	public class Story
	{
		[XmlIgnore]
		public string Id { get; set; }
		public string QuestName { get; set; }
		public string QuestNameID { get; set; }
		public string PlayerDisposition { get; set; }
		public string SpecificInitiator { get; set; }
		public List<Target> Targets { get; set; } = new List<Target>();
		public string RelationRequirement { get; set; }
		public string QuestType { get; set; }
		public string Item { get; set; }
		public int ItemQty { get; set; }
		public List<Reward> Rewards { get; set; } = new List<Reward>();
		public int TimeDays { get; set; }
		public string LogStart { get; set; }
		public string LogFinish { get; set; }
	}
}
