using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DanqnasQuests.Models.Enums;

namespace DanqnasQuests.Models
{
    public class Reward
    {
        public RewardType RewardType { get; set; }
        public int RewardQuantity { get; set; }
        public string ItemName { get; set; }
    }
}
