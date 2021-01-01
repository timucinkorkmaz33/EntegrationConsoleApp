using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using appCS;
using EntityFramework.BulkInsert.Extensions;
using KayseriEnt.KayseriView;
using KayseriEnt.Model;
using KayseriEnt.ModelView;
using KirikkaleEntegrasyon.RefreshTime;

namespace KayseriEnt
{
    class Program
    {
        public static KAYSERIData ent = new KAYSERIData();
        public static NAR_KGSEntities data = new NAR_KGSEntities();
        static ConfigValues rfr;
        static void Main(string[] args)
        {
            while (true)
            {
                DateTime t = DateTime.Now;
                rfr = new ConfigValues();
                if (ConfigValues.ReCalculate() == "1" || ((t.Hour == rfr.SicilRefreshTime.WorkingHour && t.Minute == rfr.SicilRefreshTime.WorkingMinute)))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Entegrasyon Başlatılıyor...");
                   
                    Entegrasyon();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Entegrasyon Tamamlandı...");
                }
               

                if (ConfigValues.ReCalculate() == "1")
                {
                    ConfigValues.SetReCalculateStatusAsCompeleted();
                }
            }
        }




        static void Entegrasyon()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
          
            List<Tbl_SicilArac> aracls = new List<Tbl_SicilArac>();
            List<NizamiyeVM> nzmList = (from i in data.view_temmuz_nizamiye
                                       
                                        select new NizamiyeVM
                                        {
                                            ACIKLAMA = i.ACIKLAMA,
                                            ADI = i.ADI,
                                            BANKA_KART = i.BANKA_KART,
                                            UNVAN_KODU=i.UNVAN_KODU,
                                            //BOLUM_KODU = i.BOLUM_KODU,
                                            DURUM_KODU = i.DURUM_KODU,
                                          //  FIILI_UNVAN_YERI_KODU = i.FIILI_UNVAN_YERI_KODU,
                                            GIRIS_ENGELLI = i.GIRIS_ENGELLI,
                                            RESIM = i.RESIM,
                                            GUVENLIK_NOTU = i.GUVENLIK_NOTU,
                                            ID_TIP = i.ID_TIP,
                                            ISTEK_TARIHI = i.ISTEK_TARIHI,
                                            KART_SKT = i.KART_SKT,
                                            KAYIP_CALINTI = i.KAYIP_CALINTI,
                                            MARKA = i.MARKA,
                                            ONAY = i.ONAY,
                                            MIFARE = i.MIFARE,
                                            MODEL = i.MODEL,
                                            MODEL_YILI = i.MODEL_YILI,
                                            OGS_ID = i.OGS_ID,
                                            PLAKA = i.PLAKA,
                                            RENK = i.RENK,
                                            RUHSAT_SAHIBI = i.RUHSAT_SAHIBI,
                                            SICIL = i.SICIL,
                                            SOYADI = i.SOYADI,
                                            TC_KIMLIK_NO = i.TC_KIMLIK_NO,
                                            TURU = i.TURU,
                                            BOLUM_ADI=i.BOLUM_ADI,
                                            YAKINLIGI = i.YAKINLIGI
                                        }).ToList();

            List<BirimVM> birims = (from i in ent.Tbl_Birimler
                                    select new BirimVM
                                    {
                                        BirimAdi = i.BirimAdi,
                                        Id = i.Id,
                                        BirimKodu = i.BirimKodu,
                                        ParentId = i.ParentId
                                    }).ToList();
            List<SicilVM> Sicillst = (from i in ent.Sicil
                                      join x in ent.UserList on i.UserID equals x.UserID
                                     
                                      select new SicilVM
                                      {

                                          Ad = i.Ad,
                                          Adres = i.Adres,
                                          AltFirma = i.AltFirma,
                                          AmirId = i.AmirId,
                                          AylikCalismaSaati = i.AylikCalismaSaati,
                                          Bilgi = i.Bilgi,
                                          bitistarih = i.bitistarih,
                                          BirimId = i.BirimId,
                                          Bolum = i.Bolum,
                                          CepTelefon = i.CepTelefon,
                                          Cinsiyet = i.Cinsiyet,
                                          Deleted = i.Deleted,
                                          Soyad = i.Soyad,
                                          SicilNo = i.SicilNo,
                                          CikisTarih = i.CikisTarih,
                                          ExpireDate = i.ExpireDate,
                                          GirisTarih = i.GirisTarih,
                                          Firma = i.Firma,
                                          Gorev = i.Gorev,
                                          ID = i.ID,
                                          PersonelNo = i.PersonelNo,
                                          UserID = i.UserID,
                                          Pozisyon = i.Pozisyon,
                                          KartNo = x.CardID26,
                                        
                                      }).ToList();
            List<BolumVM> bolums = (from b in ent.cbo_Bolum
                                    select new BolumVM {
                                        Ad=b.Ad,
                                        ID=b.ID,
                                        PeriyodID=b.PeriyodID
                                    }).ToList();

            List<SicilAracVM> lstArac = (from i in ent.Tbl_SicilArac
                                         select new SicilAracVM
                                         {
                                             Aciklama = i.Aciklama,
                                             AracPlaka = i.AracPlaka,
                                             AracSicilId = i.AracSicilId,
                                             CepTelefonu = i.CepTelefonu,
                                             DateCreated = i.DateCreated,
                                             Deleted = i.Deleted,
                                             RuhsatNo = i.RuhsatNo,
                                             SicilId = i.SicilId,


                                         }).ToList();
            var kartbaslangic = 0;

            Sicil scl = new Sicil();
            UserList ul = new UserList();
            SicilVM Sclvm = new SicilVM();
            SicilAracVM aracvm = new SicilAracVM();
            Tbl_SicilArac sicilarac = new Tbl_SicilArac();
            List<UserList> users = new List<UserList>();
            List<Sicil> sicils = new List<Sicil>();
            Tbl_Birimler birimler = new Tbl_Birimler();
            BirimVM br = new BirimVM();
            UInt32 integer_value = 0;
            int? unvanKodu = 0;
            string Picid = "";
            var IdforUserId = NextUserlistID();
            var NextSicilIDs = NextSicilID();
            var KartNo = "";
            int k = 0;
            foreach (var i in nzmList)
            {
                Console.WriteLine("Kalan Kayıt Sayısı:" + (nzmList.Count - k));
                k++;

                var birimkontrol = birims.Where(u => u.BirimAdi == i.BOLUM_ADI).FirstOrDefault();
                if (birimkontrol == null)
                {
                    birimler = new Tbl_Birimler();
                    birimler.BirimAdi = i.BOLUM_ADI;
                    birimler.Id = NextBirimID();
                    birimler.BirimKodu = birimler.Id;
                    birimler.ParentId = 2;
                    ent.Tbl_Birimler.Add(birimler);
                    ent.SaveChanges();

                    br = new BirimVM();
                    br.BirimAdi = birimler.BirimAdi;
                    br.Id = birimler.Id;
                    br.BirimKodu = birimler.BirimKodu;
                    br.ParentId = birimler.ParentId;
                    birims.Add(br);
                }
                var birimid = birims.Where(u => u.BirimAdi == i.BOLUM_ADI).FirstOrDefault().Id;

                var bolumid = bolums.Where(u => u.Ad == i.ID_TIP).FirstOrDefault().ID;

                if (i.ID_TIP == "PERSONEL")
                {
                    unvanKodu = i.UNVAN_KODU;
                }
                else
                {
                    unvanKodu = 0;
                }
              

                if (i.MIFARE != null && i.MIFARE!="" && i.MIFARE != "YOK" && i.MIFARE.Length<=8)
                {
                    var sicilsorgula = Sicillst.Where(u => u.SicilNo == i.SICIL && u.Ad == i.ADI && u.Soyad == u.Soyad && u.KartNo == CardNoConverter(i.MIFARE)).FirstOrDefault();
                    if (sicilsorgula == null)//yeni sicil kayıt
                    {
                        integer_value = Convert.ToUInt32(i.MIFARE, 16);
                        ul = new UserList();
                        ul.ID = IdforUserId;
                        ul.UserID = CharacterFixer.dynamicfix(ul.ID.ToString(), 8);
                        ul.CardType = 0;
                        ul.CardAttribute = 1;
                        ul.FacilityCode = "0";
                        ul.CardID = CharacterFixer.dynamicfix(integer_value.ToString(), 15);
                        ul.CardID26 =CardNoConverter(i.MIFARE);
                        ul.UserDef = 1;
                        ul.Master = 0;
                        ul.BypassCard = 1;
                        ul.IsTimezone = 0;
                        ul.Deleted = false;
                        //ent.UserList.Add(ul);
                        //ent.SaveChanges();

                        //sicil//
                        scl = new Sicil();
                        scl.Ad = i.ADI;
                        scl.ID = NextSicilIDs;
                        scl.Soyad = i.SOYADI == null ? "sicil" : i.SOYADI;
                        scl.UserID = ul.UserID;
                        scl.GirisTarih = DateTime.Now;
                        scl.SicilNo = i.SICIL == null ? "1" : i.SICIL.ToString();
                        //scl.CikisTarih = Convert.ToDateTime("2000-01-01");
                        scl.ExpireDate = DateTime.Now.AddYears(50);
                        scl.PersonelNo = i.TC_KIMLIK_NO == null ? "0" : i.TC_KIMLIK_NO;
                        scl.BirimId = birimid;
                        scl.Pozisyon = unvanKodu;
                        scl.Gorev = 0;
                        scl.Bolum = 0;
                        scl.Firma = 0;
                        //ent.Sicil.Add(scl);
                        //ent.SaveChanges();
                        ///sicilid atandı
                        //sicilId = scl.ID;

                        Sclvm = new SicilVM();
                        Sclvm.Ad = scl.Ad;
                        Sclvm.ID = NextSicilIDs;
                        Sclvm.Soyad = i.SOYADI == null ? "sicil" : i.SOYADI;
                        Sclvm.UserID = ul.UserID;
                        Sclvm.GirisTarih = DateTime.Now;
                        Sclvm.SicilNo = i.SICIL == null ? "111111111" : i.SICIL.ToString();
                        Sclvm.KartNo =CardNoConverter(i.MIFARE);
                        //scl.CikisTarih = Convert.ToDateTime("2000-01-01");
                        Sclvm.ExpireDate = DateTime.Now.AddYears(50);
                        // scl.PersonelNo = i.TC.ToString();
                        Sclvm.BirimId = birimid;
                        Sclvm.Gorev = 0;
                        Sclvm.Bolum = 0;
                        Sclvm.Firma = 0;


                        users.Add(ul);
                        sicils.Add(scl);
                        Sicillst.Add(Sclvm);
                        IdforUserId++;
                        NextSicilIDs++;
                    }

                    else//kayıt var
                    {
                        if (i.RESIM != null)
                        {
                            MemoryStream ms = new MemoryStream(i.RESIM, 0, i.RESIM.Length);
                            ms.Position = 0;
                            ms.Write(i.RESIM, 0, i.RESIM.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            image.Save(@"C:/inetpub/wwwroot/web_app/SicilResim\" + sicilsorgula.ID + ".jpg");
                            Picid = "/" + sicilsorgula.ID + ".jpg";
                        }
                        if (i.GIRIS_ENGELLI == "1" && sicilsorgula.CikisTarih == null)
                        {
                            var sorgu = ent.Sicil.Where(u => u.ID == sicilsorgula.ID).FirstOrDefault();
                            sorgu.CikisTarih = DateTime.Now;
                            sorgu.OKod2 = i.GUVENLIK_NOTU;
                            sorgu.PictureId = Picid;
                            //sorgu.Pozisyon = unvanKodu;
                            ent.SaveChanges();
                            ent.TumPersonelTerminallereYasakVer(sicilsorgula.UserID);
                        }
                        else if (i.GIRIS_ENGELLI != "1" && sicilsorgula.CikisTarih != null)
                        {
                            var sorgu = ent.Sicil.Where(u => u.ID == sicilsorgula.ID).FirstOrDefault();
                            sorgu.CikisTarih = null;
                            sorgu.OKod2 = i.GUVENLIK_NOTU;
                            //sorgu.Pozisyon = unvanKodu;
                            sorgu.PictureId = Picid;
                            ent.SaveChanges();
                            ent.TumPersonelTerminallereYetkiVer(sicilsorgula.UserID);
                        }
                        else
                        {
                            var sorgu = ent.Sicil.Where(u => u.ID == sicilsorgula.ID).FirstOrDefault();
                            if (sorgu != null)
                            {
                                //integer_value = Convert.ToUInt32(i.MIFARE, 16);
                                var skt = Convert.ToDateTime(i.KART_SKT);
                                sorgu.OKod2 = i.GUVENLIK_NOTU;
                                sorgu.PictureId = Picid;
                                // sorgu.Pozisyon = unvanKodu;
                                sorgu.Bolum = bolumid;
                                if (i.KART_SKT != null)
                                {
                                    sorgu.ExpireDate = skt;
                                }
                                ent.SaveChanges();
                            }
                        }
                        Picid = "";

                    }
                }
                if (i.BANKA_KART != null && i.BANKA_KART!="" && i.BANKA_KART!="YOK" && i.BANKA_KART.Length <= 8)
                {
                    var sicilsorgula = Sicillst.Where(u => u.SicilNo == i.SICIL && u.Ad == i.ADI && u.Soyad == u.Soyad && u.KartNo == CardNoConverter(i.BANKA_KART)).FirstOrDefault();
                    if (sicilsorgula == null)//yeni sicil kayıt
                    {
                        integer_value = Convert.ToUInt32(i.BANKA_KART, 16);
                        ul = new UserList();
                        ul.ID = IdforUserId;
                        ul.UserID = CharacterFixer.dynamicfix(ul.ID.ToString(), 8);
                        ul.CardType = 0;
                        ul.CardAttribute = 1;
                        ul.FacilityCode = "0";
                        ul.CardID = CharacterFixer.dynamicfix(integer_value.ToString(), 15);
                        ul.CardID26 = CardNoConverter(i.BANKA_KART);
                        ul.UserDef = 1;
                        ul.Master = 0;
                        ul.BypassCard = 1;
                        ul.IsTimezone = 0;
                        ul.Deleted = false;
                        //ent.UserList.Add(ul);
                        //ent.SaveChanges();

                        //sicil//
                        scl = new Sicil();
                        scl.Ad = i.ADI;
                        scl.ID = NextSicilIDs;
                        scl.Soyad = i.SOYADI == null ? "sicil" : i.SOYADI;
                        scl.UserID = ul.UserID;
                        scl.GirisTarih = DateTime.Now;
                        scl.SicilNo = i.SICIL == null ? "1" : i.SICIL.ToString();
                        //scl.CikisTarih = Convert.ToDateTime("2000-01-01");
                        scl.ExpireDate = DateTime.Now.AddYears(50);
                        scl.PersonelNo = i.TC_KIMLIK_NO == null ? "0" : i.TC_KIMLIK_NO;
                        scl.BirimId = birimid;
                        scl.Pozisyon = unvanKodu;
                        scl.Gorev = 0;
                        scl.Bolum = 0;
                        scl.Firma = 0;
                        //ent.Sicil.Add(scl);
                        //ent.SaveChanges();
                        ///sicilid atandı
                        //sicilId = scl.ID;

                        Sclvm = new SicilVM();
                        Sclvm.Ad = scl.Ad;
                        Sclvm.ID = NextSicilIDs;
                        Sclvm.Soyad = i.SOYADI == null ? "sicil" : i.SOYADI;
                        Sclvm.UserID = ul.UserID;
                        Sclvm.GirisTarih = DateTime.Now;
                        Sclvm.SicilNo = i.SICIL == null ? "111111111" : i.SICIL.ToString();
                        Sclvm.KartNo = CardNoConverter(i.BANKA_KART);
                        //scl.CikisTarih = Convert.ToDateTime("2000-01-01");
                        Sclvm.ExpireDate = DateTime.Now.AddYears(50);
                        // scl.PersonelNo = i.TC.ToString();
                        Sclvm.BirimId = birimid;
                        Sclvm.Gorev = 0;
                        Sclvm.Bolum = 0;
                        Sclvm.Firma = 0;


                        users.Add(ul);
                        sicils.Add(scl);
                        Sicillst.Add(Sclvm);
                        IdforUserId++;
                        NextSicilIDs++;
                    }
                    else//kayıt var
                    {
                        if (i.RESIM != null)
                        {
                            MemoryStream ms = new MemoryStream(i.RESIM, 0, i.RESIM.Length);
                            ms.Position = 0;
                            ms.Write(i.RESIM, 0, i.RESIM.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            image.Save(@"C:/inetpub/wwwroot/web_app/SicilResim\" + sicilsorgula.ID + ".jpg");
                            Picid = "/" + sicilsorgula.ID + ".jpg";
                        }
                        if (i.GIRIS_ENGELLI == "1" && sicilsorgula.CikisTarih == null)
                        {
                            var sorgu = ent.Sicil.Where(u => u.UserID == sicilsorgula.UserID).FirstOrDefault();
                            sorgu.CikisTarih = DateTime.Now;
                            sorgu.OKod2 = i.GUVENLIK_NOTU;
                            sorgu.PictureId = Picid;
                            //sorgu.Pozisyon = unvanKodu;
                            ent.SaveChanges();
                            ent.TumPersonelTerminallereYasakVer(sicilsorgula.UserID);
                        }
                        else if (i.GIRIS_ENGELLI != "1" && sicilsorgula.CikisTarih != null)
                        {
                            var sorgu = ent.Sicil.Where(u => u.ID == sicilsorgula.ID).FirstOrDefault();
                            sorgu.CikisTarih = null;
                            sorgu.OKod2 = i.GUVENLIK_NOTU;
                            sorgu.PictureId = Picid;
                            //sorgu.Pozisyon = unvanKodu;
                            ent.SaveChanges();
                            ent.TumPersonelTerminallereYetkiVer(sicilsorgula.UserID);
                        }
                        else
                        {
                            var sorgu = ent.Sicil.Where(u => u.ID == sicilsorgula.ID).FirstOrDefault();
                            if (sorgu != null)
                            {
                                var skt = Convert.ToDateTime(i.KART_SKT);
                                sorgu.OKod2 = i.GUVENLIK_NOTU;
                                sorgu.PictureId = Picid;
                                sorgu.Bolum = bolumid;
                                //sorgu.Pozisyon = unvanKodu;
                                if (i.KART_SKT != null)
                                {
                                    sorgu.ExpireDate = skt;
                                }
                                ent.SaveChanges();
                            }
                        }
                        Picid = "";
                    }
                }
                if (i.MARKA != null && i.MODEL != null)
                {
                    var aracsorgula = Sicillst.Where(u => u.Ad == i.PLAKA).FirstOrDefault();
                    if (aracsorgula == null)
                    {

                        if (i.OGS_ID != null)
                        {
                            kartbaslangic = i.OGS_ID.Length - 8;
                            KartNo = i.OGS_ID.Substring(kartbaslangic,8);
                        }
                        else
                        {
                            KartNo = "0";
                        }

                        ul = new UserList();
                        ul.ID = IdforUserId;
                        ul.UserID = CharacterFixer.dynamicfix(ul.ID.ToString(), 8);
                        ul.CardType = 0;
                        ul.CardAttribute = 1;
                        ul.FacilityCode = "0";
                        ul.CardID = CharacterFixer.dynamicfix(KartNo, 15);
                        ul.CardID26 = CharacterFixer.dynamicfix(KartNo, 16);
                        ul.UserDef = 9;
                        ul.Master = 0;
                        ul.BypassCard = 1;
                        ul.IsTimezone = 0;
                        ul.Deleted = false;
                        //ent.UserList.Add(ul);
                        //ent.SaveChanges();

                        //sicil//
                        scl = new Sicil();
                        scl.Ad = i.PLAKA;
                        scl.ID = NextSicilIDs;
                        scl.Soyad = i.MODEL == null ? "sicil" : i.MODEL;
                        scl.UserID = ul.UserID;
                        scl.GirisTarih = DateTime.Now;
                        scl.SicilNo = i.SICIL == null ? "1" : i.SICIL.ToString();
                        //scl.CikisTarih = Convert.ToDateTime("2000-01-01");
                        scl.ExpireDate = DateTime.Now.AddYears(50);
                        scl.PersonelNo = i.TC_KIMLIK_NO == null ? "0" : i.TC_KIMLIK_NO;
                        scl.BirimId = birimid;
                        scl.Pozisyon = unvanKodu;
                        scl.Gorev = 0;
                        scl.Bolum = 0;
                        scl.Firma = 0;
                        //ent.Sicil.Add(scl);
                        //ent.SaveChanges();
                        ///sicilid atandı
                        //sicilId = scl.ID;

                        Sclvm = new SicilVM();
                        Sclvm.Ad = scl.Ad;
                        Sclvm.ID = NextSicilIDs;
                        Sclvm.Soyad = i.MODEL == null ? "sicil" : i.MODEL;
                        Sclvm.UserID = ul.UserID;
                        Sclvm.GirisTarih = DateTime.Now;
                        Sclvm.SicilNo = i.SICIL == null ? "111111111" : i.SICIL.ToString();
                        //Sclvm.KartNo = ;
                        //scl.CikisTarih = Convert.ToDateTime("2000-01-01");
                        Sclvm.ExpireDate = DateTime.Now.AddYears(50);
                        // scl.PersonelNo = i.TC.ToString();
                        Sclvm.BirimId = birimid;
                        Sclvm.Gorev = 0;
                        Sclvm.Bolum = 0;
                        Sclvm.Firma = 0;


                        users.Add(ul);
                        sicils.Add(scl);
                        Sicillst.Add(Sclvm);
                        IdforUserId++;
                        NextSicilIDs++;

                        sicilarac = new Tbl_SicilArac();
                        sicilarac.SicilId = Sicillst.Where(u => u.SicilNo == i.SICIL).FirstOrDefault().ID;
                        sicilarac.AracSicilId = Sicillst.Where(u => u.Ad == i.PLAKA).FirstOrDefault().ID;
                        sicilarac.Aciklama = i.ACIKLAMA;
                        sicilarac.AracPlaka = i.PLAKA;
                        sicilarac.Deleted = false;
                        sicilarac.DateCreated = DateTime.Now;
                        aracls.Add(sicilarac);

                        aracvm = new SicilAracVM();
                        aracvm.SicilId = sicilarac.SicilId;
                        aracvm.AracSicilId = sicilarac.AracSicilId;
                        aracvm.AracPlaka = sicilarac.AracPlaka;
                        lstArac.Add(aracvm);


                    }
                    else//arac kayıt var
                    {
                        if (i.GIRIS_ENGELLI == "1" && aracsorgula.CikisTarih == null)
                        {
                            var sorgula = ent.Sicil.Where(u => u.SicilNo == i.SICIL && u.Ad == i.MARKA && u.Soyad == i.MODEL).FirstOrDefault();
                            sorgula.GirisTarih = DateTime.Now;
                            sorgula.OKod2 = i.GUVENLIK_NOTU;
                            //sorgu.Pozisyon = unvanKodu;
                            ent.SaveChanges();
                            ent.TumRFIDTerminallereYasakVer(sorgula.UserID);
                        }
                        else if (i.GIRIS_ENGELLI != "1" && aracsorgula.CikisTarih != null)
                        {
                            var sorgula = ent.Sicil.Where(u => u.SicilNo == i.SICIL && u.Ad == i.MARKA && u.Soyad == i.MODEL).FirstOrDefault();
                            sorgula.GirisTarih = null;
                            sorgula.OKod2 = i.GUVENLIK_NOTU;
                            //sorgu.Pozisyon = unvanKodu;
                            ent.SaveChanges();
                            ent.TumRFIDTerminallereYetkiVer(sorgula.UserID);
                        }

                        var sorgu = ent.Sicil.Where(u => u.Ad == i.PLAKA).FirstOrDefault();
                        if (sorgu != null)
                        {
                            var skt = Convert.ToDateTime(i.KART_SKT);
                            sorgu.Ad = i.PLAKA;
                            sorgu.OKod2 = i.GUVENLIK_NOTU;
                            sorgu.Bolum = bolumid;
                            //sorgu.Pozisyon = unvanKodu;
                            if (i.KART_SKT != null)
                            {
                                sorgu.ExpireDate = skt;
                            }

                            var arac = ent.Tbl_SicilArac.Where(u => u.AracPlaka == i.PLAKA).FirstOrDefault();
                            arac.SicilId = ent.Sicil.Where(x => x.SicilNo == aracsorgula.SicilNo).OrderBy(u => u.ID).FirstOrDefault().ID;
                            arac.AracSicilId = ent.Sicil.Where(a => a.Ad == i.PLAKA).FirstOrDefault().ID;
                            arac.Marka = i.MARKA;
                            arac.Model = i.MODEL;

                            ent.SaveChanges();

                        }



                    }
                    }

            }
            BulkInsertUserlist(users);
            BulkInsertSicil(sicils);
            BulkInsertArac(aracls);

            var rfiduser = users.Where(u => u.UserDef == 9).ToList();
            var personeluser = users.Where(u => u.UserDef == 1).ToList();
            foreach(var userid in personeluser)
            {
                ent.TumPersonelTerminallereYetkiVer(userid.UserID);
            }
            foreach (var rf in rfiduser)
            {
                ent.TumRFIDTerminallereYetkiVer(rf.UserID);
            }

        }

       

        public static int NextBirimID()
        {
            if (ent.Tbl_Birimler.Select(x => x.Id).FirstOrDefault() != 0)
            {
                var birimid = ent.Tbl_Birimler.Select(x => x.Id).Max();
                if (birimid == 0)
                    return 1;
                else
                {
                    return Convert.ToInt32(birimid.ToString()) + 1;
                }
            }
            else { return 1; }
        }

        public static int NextUserlistID()
        {
            if (ent.UserList.Select(x => x.ID).FirstOrDefault() != 0)
            {
                var Userid = ent.UserList.Select(x => x.ID).Max();
                if (Userid == 0)
                    return 1;
                else
                {
                    return Convert.ToInt32(Userid.ToString()) + 1;
                }
            }
            else { return 1; }

        }
        public static int NextSicilID()
        {
            if (ent.Sicil.Select(x => x.ID).FirstOrDefault() != 0)
            {
                var sicilid = ent.Sicil.Select(x => x.ID).Max();
                if (sicilid == 0)
                    return 1;
                else
                {
                    return Convert.ToInt32(sicilid.ToString()) + 1;
                }
            }
            else { return 1; }
        }
        public static void BulkInsertUserlist(List<UserList> userlist)
        {
            KAYSERIData entity = new KAYSERIData();
            entity.BulkInsert(userlist);
        }
        public static void BulkInsertSicil(List<Sicil> sicil)
        {
            KAYSERIData entity = new KAYSERIData();
            entity.BulkInsert(sicil);
        }
        public static void BulkInsertArac(List<Tbl_SicilArac> arac)
        {
            KAYSERIData entity = new KAYSERIData();
            entity.BulkInsert(arac);
        }
       
        public static void BulkInsertDuty(List<Duty> duty)
        {
            KAYSERIData entity = new KAYSERIData();
            entity.BulkInsert(duty);
        }
        public static string ConvertToHex(ulong val)
        {
            string hexValue = val.ToString("X");

            string v = String.Format("{0:X16}", hexValue);
            return CharacterFixer.dynamicfix(v, 16);
        }
        public static string CardNoConverter(string HexCardNo)//card nolaro kendi içinde cevirme
        {
            if (HexCardNo.Length < 8)
            {
                while (true)
                    if (HexCardNo.Length != 8)
                    {
                        HexCardNo = "0" + HexCardNo;
                    }
                    else
                    {
                        break;
                    }
            }
            string[] reversebytwo = new string[4];
            reversebytwo[0] = HexCardNo.Substring(0, 2);
            reversebytwo[1] = HexCardNo.Substring(2, 2);
            reversebytwo[2] = HexCardNo.Substring(4, 2);
            reversebytwo[3] = HexCardNo.Substring(6, 2);


            for (int i = 0; i < reversebytwo.Length; i++)
            {
                reversebytwo[i] = fixlentotwo(reversebytwo[i]);
            }

            string[] result = new string[] { reversebytwo[3], reversebytwo[2], reversebytwo[1], reversebytwo[0] };
            string bits = result[0] + "" + result[1] + "" + result[2] + "" + result[3];
            //uint decValue = uint.Parse(bits, System.Globalization.NumberStyles.HexNumber); //Convert.ToInt32(bits, 16);
            return dynamicfix(bits.ToString(), 16);
            //728e6835 =0896044658
            //35688E72

        }

        public static string dynamicfix(string str, int fixcount)
        {
            int l = str.Length;
            if (l == fixcount)
                return str;
            int zerocount = fixcount - l;
            string sifirekle = string.Empty;
            for (int i = 0; i < zerocount; i++)
            {
                sifirekle += "0";
            }
            return sifirekle + str;
        }
        private static string fixlentotwo(string val)
        {
            if (val.Length < 2)
            {
                while (val.Length < 2)
                {
                    val = "0" + val;
                }
            }
            return val;
        }
    }
}
