using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayseriEnt.ModelView
{
    class SicilAracVM
    {
        public int Id { get; set; }
        public int SicilId { get; set; }
        public int AracSicilId { get; set; }
        public string AracPlaka { get; set; }
        public string RuhsatNo { get; set; }
        public string Aciklama { get; set; }
        public string CepTelefonu { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    }
}
