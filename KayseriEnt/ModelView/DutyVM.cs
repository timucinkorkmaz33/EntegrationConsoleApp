using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayseriEnt.ModelView
{
    public class DutyVM
    {
        public int Id { get; set; }
        public string TargetDevice { get; set; }
        public string Process { get; set; }
        public string userID { get; set; }
        public string Owner { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public Nullable<System.DateTime> ProcessDate { get; set; }
        public string TimeZoneID { get; set; }
        public string DeviceMessage { get; set; }
    }
}
