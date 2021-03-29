using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class AttendanceSummary
    {
        public int totalEmployee { get; set; }
        public int totalPresent { get; set; }
        public int totalAbsent { get; set; }
    }
}
