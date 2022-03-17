using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.SaveSystem;

namespace DanqnasQuests.Models
{
	class Story
	{
		[SaveableProperty(1)] public string QuestName { get; set; }
		[SaveableProperty(2)] public string QuestNameID { get; set; }
		[SaveableProperty(3)] public string PlayerDisposition { get; set; }
		[SaveableProperty(4)] public string SpecificInitiator { get; set; }
		[SaveableProperty(5)] public List<Target> Targets { get; set; }
		[SaveableProperty(6)] public string RelationRequirement { get; set; }
		[SaveableProperty(7)] public string QuestType { get; set; }
		[SaveableProperty(8)] public string Item { get; set; }
		[SaveableProperty(9)] public int ItemQty { get; set; }
		[SaveableProperty(10)] public string RewardType { get; set; }
		[SaveableProperty(11)] public int RewardQty { get; set; }
		[SaveableProperty(12)] public int TimeDays { get; set; }
		[SaveableProperty(13)] public string LogStart { get; set; }
		[SaveableProperty(14)] public string LogFinish { get; set; }
	}
}
