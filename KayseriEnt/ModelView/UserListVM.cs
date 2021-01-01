using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayseriEnt.ModelView
{
    public class UserListVM
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int CardType { get; set; }
        public string CardID { get; set; }
        public int CardAttribute { get; set; }
        public string FacilityCode { get; set; }
        public string FingerID1 { get; set; }
        public string FingerID2 { get; set; }
        public string FPData { get; set; }
        public int UserDef { get; set; }
        public Nullable<int> Function { get; set; }
        public int Master { get; set; }
        public int BypassCard { get; set; }
        public Nullable<System.DateTime> startdate { get; set; }
        public Nullable<System.DateTime> enddate { get; set; }
        public string CardID26 { get; set; }
        public int IsTimezone { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public int zyrt { get; set; }
        public int isAPB { get; set; }
        public string Aciklama { get; set; }
    }
}
