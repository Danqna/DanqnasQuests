using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanqnasQuests.Models
{
    public class Enums
    {
        public enum TargetType
        {
            Unspecified,
            Notable,
            Lord,
            Ruler,
            Bandit
        }

        public enum Gender
        {
            Unspecified,
            Male,
            Female
        }

        public enum Faction
        {
            Unspecified,
            SameFaction,
            OtherFaction
        }

        public enum RewardType
        {
            Gold,
            Relation,
            Influence,
            Renown
        }

        public enum QuestActions
        {
            None,
            Accept,
            Cancel,
            Finish,
            Betrayal
        }

        public enum LogEntryType
        {
            ProgressBar,
            Text,
            TwoWay
        }
    }
}
