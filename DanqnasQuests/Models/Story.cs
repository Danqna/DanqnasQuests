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
		[SaveableProperty(5)] public string InitiatorTestosteroneDominant { get; set; }
		[SaveableProperty(6)] public string Occupation { get; set; }
		[SaveableProperty(7)] public string SpecificTarget { get; set; }
		[SaveableProperty(8)] public string TargetTestosteroneDominant { get; set; }
		[SaveableProperty(9)] public string TargetSameFaction { get; set; }
		[SaveableProperty(10)] public string RelationRequirement { get; set; }
		[SaveableProperty(11)] public string QuestType { get; set; }
		[SaveableProperty(12)] public string Item { get; set; }
		[SaveableProperty(13)] public int ItemQty { get; set; }
		[SaveableProperty(14)] public string RewardType { get; set; }
		[SaveableProperty(15)] public int RewardQty { get; set; }
		[SaveableProperty(16)] public int TimeDays { get; set; }
		[SaveableProperty(17)] public string StartQuestPlayerDialogue { get; set; }
		[SaveableProperty(18)] public string StartQuestNPCDialogue { get; set; }
		[SaveableProperty(19)] public string OptionalStartQuestPlayerDialogue { get; set; }
		[SaveableProperty(20)] public string OptionalStartQuestNPCDialogue { get; set; }
		[SaveableProperty(21)] public string InterimQuestPlayerDialogue { get; set; }
		[SaveableProperty(22)] public string InterimQuestNPCDialogue { get; set; }
		[SaveableProperty(23)] public string OptionalInterimQuestPlayerDialogue { get; set; }
		[SaveableProperty(24)] public string OptionalInterimQuestNPCDialogue { get; set; }
		[SaveableProperty(25)] public string EndQuestPlayerDialogue { get; set; }
		[SaveableProperty(26)] public string EndQuestNPCDialogue { get; set; }
		[SaveableProperty(27)] public string OptionalEndQuestPlayerDialogue { get; set; }
		[SaveableProperty(28)] public string OptionalEndQuestNPCDialogue { get; set; }
		[SaveableProperty(29)] public bool MultiPart { get; set; }
		[SaveableProperty(30)] public string MultiKey { get; set; }
		[SaveableProperty(31)] public int PartNumber { get; set; }
		[SaveableProperty(32)] public int TotalParts { get; set; }
		[SaveableProperty(33)] public string LogStart { get; set; }
		[SaveableProperty(34)] public string LogFinish { get; set; }
	}
}
