using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayseriEnt.ModelView
{
   public class NizamiyeVM
    {
        public string SICIL { get; set; }
        public int BOLUM_KODU { get; set; }
        public string ID_TIP { get; set; }
        public string ADI { get; set; }
        public string SOYADI { get; set; }
        public byte[] RESIM { get; set; }
        public string TC_KIMLIK_NO { get; set; }
        public Nullable<int> UNVAN_KODU { get; set; }
        public Nullable<int> FIILI_UNVAN_YERI_KODU { get; set; }
        public Nullable<short> DURUM_KODU { get; set; }
        public string MIFARE { get; set; }
        public string BANKA_KART { get; set; }
        public string OGS_ID { get; set; }
        public string PLAKA { get; set; }
        public string MARKA { get; set; }
        public string MODEL { get; set; }
        public string RENK { get; set; }
        public string RUHSAT_SAHIBI { get; set; }
        public Nullable<System.DateTime> KART_SKT { get; set; }
        public string MODEL_YILI { get; set; }
        public string ONAY { get; set; }
        public string KAYIP_CALINTI { get; set; }
        public string GIRIS_ENGELLI { get; set; }
        public Nullable<System.DateTime> ISTEK_TARIHI { get; set; }
        public Nullable<byte> TURU { get; set; }
        public string YAKINLIGI { get; set; }
        public string ACIKLAMA { get; set; }
        public string GUVENLIK_NOTU { get; set; }
        public string BOLUM_ADI { get; set; }
    }
}
