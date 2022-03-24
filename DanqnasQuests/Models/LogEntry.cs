using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DanqnasQuests.Models.Enums;

namespace DanqnasQuests.Models
{
    public class LogEntry
    {
        public LogEntryType LogEntryType { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int TwoWayRange { get; set; }
        public int ProgressMax { get; set; }
        public int Progress { get; set; }
    }
}
