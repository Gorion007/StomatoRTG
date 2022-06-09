using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZdravotniZaznamStomatologie
{
    class KartaPacient
    {
        public string prijmeni = "";
        public string jmeno = "";
        public string datumNarozeni = "";
        public string povolani = "";
        public string adresa = "";
        public string telefon = "";
        public string zamestnavatel = "";
        public string rodneCislo = "";
        public string zp = "";
        public string mistoNarozeni = "";
        public string zdravotnickeZarizeni = "";
        
        public string osetreni_diagnoza = ""; // hlavni popis karty

        // 2. strana
        public string parodontopatie = "";
        public string onkolProhlidka = "";
        public string dutUstni = "";
        public string hygDutUstni = "";
        public string orthoAnomalie = "";
        public string dispenzarizace = "";

        // dodatek k User_C_1,2,3
        public string datum1 = "";
        public string datum2 = "";
        public string datum3 = "";
        public string podp1 = "";
        public string podp2 = "";
        public string podp3 = "";

        public string[] User_C_1 = new string[66];
        public string[] User_C_2 = new string[66];
        public string[] User_C_3 = new string[66];

        // cislo karty
        public int cisloKarty = 0;

        public KartaPacient()
        {
            NastaveniKarty();
        }

        private void NastaveniKarty()
        {
            prijmeni = "";
            jmeno = "";
            datumNarozeni = "";
            povolani = "";
            adresa = "";
            telefon = "";
            zamestnavatel = "";
            rodneCislo = "";
            zp = "";
            mistoNarozeni = "";
            zdravotnickeZarizeni = "";

            osetreni_diagnoza = "";

            // 2. strana
            parodontopatie = "";
            onkolProhlidka = "";
            dutUstni = "";
            hygDutUstni = "";
            orthoAnomalie = "";
            dispenzarizace = "";

            // dodatek k User_C_1,2,3
            datum1 = "";
            datum2 = "";
            datum3 = "";
            podp1 = "";
            podp2 = "";
            podp3 = "";

            // cislo karty
            cisloKarty = 0;

            for (int i = 0; i < User_C_1.Length; i++)
            {
                User_C_1[i] = "";
                User_C_2[i] = "";
                User_C_3[i] = "";
            }
        }

        public void NakopirovaniKarty(KartaPacient kp)
        {
            this.prijmeni = kp.prijmeni;
            this.jmeno = kp.jmeno;
            this.datumNarozeni = kp.datumNarozeni;
            this.povolani = kp.povolani;
            this.adresa = kp.adresa;
            this.telefon = kp.telefon;
            this.zamestnavatel = kp.zamestnavatel;
            this.rodneCislo = kp.rodneCislo;
            this.zp = kp.zp;
            this.mistoNarozeni = kp.mistoNarozeni;
            this.zdravotnickeZarizeni = kp.zdravotnickeZarizeni;

            this.osetreni_diagnoza = kp.osetreni_diagnoza;

            // 2. strana
            this.parodontopatie = kp.parodontopatie;
            this.onkolProhlidka = kp.onkolProhlidka;
            this.dutUstni = kp.dutUstni;
            this.hygDutUstni = kp.hygDutUstni;
            this.orthoAnomalie = kp.orthoAnomalie;
            this.dispenzarizace = kp.dispenzarizace;

            // dodatek k User_C_1,2,3
            this.datum1 = kp.datum1;
            this.datum2 = kp.datum2;
            this.datum3 = kp.datum3;
            this.podp1 = kp.podp1;
            this.podp2 = kp.podp2;
            this.podp3 = kp.podp3;

            // cislo karty
            this.cisloKarty = kp.cisloKarty;

            for (int i = 0; i < User_C_1.Length; i++)
            {
                this.User_C_1[i] = kp.User_C_1[i];
                this.User_C_2[i] = kp.User_C_2[i];
                this.User_C_3[i] = kp.User_C_3[i];
            }
        }

        public void UvolneniKartyZPameti()
        {
            prijmeni = null;
            jmeno = null;
            datumNarozeni = null;
            povolani = null;
            adresa = null;
            telefon = null;
            zamestnavatel = null;
            rodneCislo = null;
            zp = null;
            mistoNarozeni = null;
            zdravotnickeZarizeni = null;

            osetreni_diagnoza = null;

            // 2. strana
            parodontopatie = null;
            onkolProhlidka = null;
            dutUstni = null;
            hygDutUstni = null;
            orthoAnomalie = null;
            dispenzarizace = null;

            // dodatek k User_C_1,2,3
            datum1 = null;
            datum2 = null;
            datum3 = null;
            podp1 = null;
            podp2 = null;
            podp3 = null;

            // cislo karty
            cisloKarty = 0;

            for (int i = 0; i < User_C_1.Length; i++)
            {
                User_C_1[i] = null;
                User_C_2[i] = null;
                User_C_3[i] = null;
            }
        }
    }
}
