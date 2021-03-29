using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class TblAttendance
    {
        public int ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
        public DateTime DATE_TIME { get; set; }
        public DateTime CHECK_IN { get; set; }
        public DateTime? CHECK_OUT { get; set; }
        public int LATE_DURATION { get; set; }
        public string STATUS { get; set; }
        public double LATITUDE { get; set; }
        public double LONGITUDE { get; set; }
    }
}
