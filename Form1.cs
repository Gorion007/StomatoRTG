//#define TEST
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;

namespace ZdravotniZaznamStomatologie
{
    public partial class Form1 : Form
    {
        KartaPacient[] Pacient = new KartaPacient[1];
        KartaPacient SmazanaKarta = new KartaPacient(); // pro jedinou smazanou kartu
        Bitmap memorying; // buffery pro tisk stran dokumentu
        Bitmap memorying2;

        private int _vybranyPacient = 0;
        string _folderpath = @"ZdravotniZaznamyDatabaze";
        string _folderpathZaloha = @"ZdravotniZaznamyDatabaze\Zaloha";
        string _path = @"ZdravotniZaznamyDatabaze\pacient.bin";
        string _keyPath = @"ZdravotniZaznamyDatabaze\key.bin";
        string[] _key = new string[5];
        string _akceProgramu = "";
        int _UClength = 66;
        bool _administrator = false;

        // zprava tisku
        PrintPreviewDialog prntprvw = new PrintPreviewDialog();
        PrintDocument pntdoc = new PrintDocument();

        public Form1()
        {
            InitializeComponent();
            VytvoreniSlozky();
            ZmenaVelikostiKartaPacient(PredNahraniDTB());
            NahraniDTB();
            AktualizaceSeznamu(); // ukazka pro combobox
            NactiHeslo();
            PrehledAkciProgramu("spuštění databáze");
            //labelJmenoDatabaze.Text = "Jméno databáze: " + _path;

#if TEST
            tESTToolStripMenuItem.Visible = true;
#endif
        }

        private void tESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if TEST
            TESTfce();
#endif
        }

        private void TESTfce()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(_path, FileMode.Create)))
            {
                for (int naplneni = 0; naplneni < 500; naplneni++)
                {

                    // pouze 1 zapis
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());

                    bw.Write(naplneni.ToString());

                    // 2. strana
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());

                    // dodatek k User_C_1,2,3
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());
                    bw.Write(naplneni.ToString());

                    // cislo karty
                    bw.Write(1);

                    for (int uc = 0; uc < _UClength; uc++)
                    {
                        bw.Write(naplneni.ToString());
                        bw.Write(naplneni.ToString());
                        bw.Write(naplneni.ToString());
                    }

                }

                bw.Flush();
                bw.Close();
                PrehledAkciProgramu("Nahrání TEST databáze dokončeno");
            }
        }

        private void NactiHeslo()
        {
            try
            {
                using (BinaryReader br = new BinaryReader(File.Open(_keyPath, FileMode.Open)))
                {

                    _key[0] = br.ReadString();
                    _key[1] = br.ReadString();
                    _key[2] = br.ReadString();
                    _key[3] = br.ReadString();
                    _key[4] = br.ReadString();
                    br.Close();
                }
            }
            catch
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(_keyPath, FileMode.Create)))
                {
                    bw.Write("0");
                    bw.Write("0");
                    bw.Write("0");
                    bw.Write("0");
                    bw.Write("0");
                    bw.Flush();
                    bw.Close();
                }

                // nacti heslo do pameti
                _key[0] = "0";
                _key[1] = "0";
                _key[2] = "0";
                _key[3] = "0";
                _key[4] = "0";
            }
        }

        private void VytvoreniSlozky()
        {
            if (!Directory.Exists(_folderpath))
            {
                Directory.CreateDirectory(_folderpath);
                PrehledAkciProgramu("vytvoření složky: " + _folderpath);
            }

            if (!Directory.Exists(_folderpathZaloha))
            {
                Directory.CreateDirectory(_folderpathZaloha);
                PrehledAkciProgramu("vytvoření složky: " + _folderpathZaloha);
            }
        }

        // nahraje dtb a vraci pocet namerenych prvku
        // kdyz nemuze nahrat, vytvori DTB pacient.bin a vrati velikost 1;
        private int PredNahraniDTB()
        {
            int pocetZaznamu = 0; // pocet zaznamu v dtb
            string buffer = "";
            int buffer2 = 0;
            //string[] buffPole = new string[50];

            try
            {
                using (BinaryReader br = new BinaryReader(File.Open(_path, FileMode.Open)))
                {
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        // neni dulezite co, ale kolikrat
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();

                        br.ReadString();

                        // 2. strana
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();

                        // dodatek k User_C_1,2,3
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();
                        br.ReadString();

                        // cislo karty
                        br.ReadInt32();

                        for (int uc = 0; uc < _UClength; uc++)
                        {
                            br.ReadString();
                            br.ReadString();
                            br.ReadString();
                        }

                        pocetZaznamu += 1;
                    }
                    br.Close();
                }
            }
            catch
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(_path, FileMode.Create)))
                {
                    // pouze 1 zapis
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);

                    bw.Write(buffer);

                    // 2. strana
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);

                    // dodatek k User_C_1,2,3
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);
                    bw.Write(buffer);

                    // cislo karty
                    bw.Write(buffer2);

                    for (int uc = 0; uc < _UClength; uc++)
                    {
                        bw.Write(buffer);
                        bw.Write(buffer);
                        bw.Write(buffer);
                    }

                    pocetZaznamu = 0; // referencni zaznam
                    bw.Flush();
                    bw.Close();
                }
            }

            return pocetZaznamu;
        }

        // podle poctu nahranych prvku zvetsi pole Pacient 
        private void ZmenaVelikostiKartaPacient(int velikost)
        {
            if (velikost > Pacient.Length)
            {
                Array.Resize(ref Pacient, velikost);
            }

            // konstruktor pro ty, ktere nejsou vytvorene
            for (int i = 0; i < Pacient.Length; i++)
            {
                try
                {
                    if (Pacient[i].jmeno != "" && Pacient[i].prijmeni != "" && Pacient[i].rodneCislo != "") { /*nedelej nic*/}
                    else { Pacient[i] = new KartaPacient(); } // jinak nastav na vychozi
                }
                catch { Pacient[i] = new KartaPacient(); }
            }
        }

        // Nahrani prvku do pameti RAM
        private void NahraniDTB()
        {
            using (BinaryReader br = new BinaryReader(File.Open(_path, FileMode.Open)))
            {
                int zaznam = 0;

                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    Pacient[zaznam].prijmeni = br.ReadString();
                    Pacient[zaznam].jmeno = br.ReadString();
                    Pacient[zaznam].datumNarozeni = br.ReadString();
                    Pacient[zaznam].povolani = br.ReadString();
                    Pacient[zaznam].adresa = br.ReadString();
                    Pacient[zaznam].telefon = br.ReadString();
                    Pacient[zaznam].zamestnavatel = br.ReadString();
                    Pacient[zaznam].mistoNarozeni = br.ReadString();
                    Pacient[zaznam].zdravotnickeZarizeni = br.ReadString();
                    Pacient[zaznam].rodneCislo = br.ReadString();
                    Pacient[zaznam].zp = br.ReadString();

                    Pacient[zaznam].osetreni_diagnoza = br.ReadString();

                    // 2. strana
                    Pacient[zaznam].parodontopatie = br.ReadString();
                    Pacient[zaznam].onkolProhlidka = br.ReadString();
                    Pacient[zaznam].dutUstni = br.ReadString();
                    Pacient[zaznam].hygDutUstni = br.ReadString();
                    Pacient[zaznam].orthoAnomalie = br.ReadString();
                    Pacient[zaznam].dispenzarizace = br.ReadString();

                    // dodatek k User_C_1,2,3
                    Pacient[zaznam].datum1 = br.ReadString();
                    Pacient[zaznam].datum2 = br.ReadString();
                    Pacient[zaznam].datum3 = br.ReadString();
                    Pacient[zaznam].podp1 = br.ReadString();
                    Pacient[zaznam].podp2 = br.ReadString();
                    Pacient[zaznam].podp3 = br.ReadString();

                    // cislo karty
                    Pacient[zaznam].cisloKarty = br.ReadInt32();

                    for (int uc = 0; uc < _UClength; uc++)
                    {
                        Pacient[zaznam].User_C_1[uc] = br.ReadString();
                        Pacient[zaznam].User_C_2[uc] = br.ReadString();
                        Pacient[zaznam].User_C_3[uc] = br.ReadString();
                    }

                    zaznam += 1; // dalsi zaznam
                }
                br.Close();
            }
        }

        // ulozi zadana data do aktualni karty
        private void buttonUlozKartu_Click(object sender, EventArgs e)
        {
            // pokud uz je tato karta vytvorena
            for (int duplikat = 0; duplikat < Pacient.Length; duplikat++ )
            {
                if (Pacient[duplikat].prijmeni == textBoxPrijmeni.Text && Pacient[duplikat].jmeno == textBoxJmeno.Text && Pacient[duplikat].rodneCislo == textBoxRodneCislo.Text && Pacient[duplikat].cisloKarty.ToString() == textBoxCisloKarty.Text)
                {
                    // jsem prave na teto vybrane karte?
                    if (!(duplikat == _vybranyPacient))
                    {
                        MessageBox.Show("Tato karta je již vytvořena");
                        PrehledAkciProgramu("Tato karta je již vytvořena");
                        return;
                    }
                }
            }

            // podminka pro ukladani
            if (textBoxPrijmeni.Text == "" || textBoxJmeno.Text == "" || textBoxRodneCislo.Text == "")
            {
                MessageBox.Show("Chybí základní údaje: příjmení, jméno, datum narození");
                PrehledAkciProgramu("Chybí základní údaje: příjmení, jméno, datum narození");
            }
            else
            {
                Pacient[_vybranyPacient].prijmeni = textBoxPrijmeni.Text;
                Pacient[_vybranyPacient].jmeno = textBoxJmeno.Text;
                Pacient[_vybranyPacient].datumNarozeni = textBoxDatumNarozeni.Text;
                Pacient[_vybranyPacient].povolani = textBoxPovolani.Text;
                Pacient[_vybranyPacient].adresa = textBoxAdresa.Text;
                Pacient[_vybranyPacient].telefon = textBoxTelefon.Text;
                Pacient[_vybranyPacient].zamestnavatel = textBoxZamestnavatel.Text;
                Pacient[_vybranyPacient].mistoNarozeni = textBoxMistoNarozeni.Text;
                Pacient[_vybranyPacient].zdravotnickeZarizeni = textBoxZdravotnickeZarizeni.Text;
                Pacient[_vybranyPacient].rodneCislo = textBoxRodneCislo.Text;
                Pacient[_vybranyPacient].zp = textBoxZP.Text;

                Pacient[_vybranyPacient].osetreni_diagnoza = textBoxOsetreni.Text;

                // 2. strana
                Pacient[_vybranyPacient].parodontopatie = textBox_parodontopatie.Text;
                Pacient[_vybranyPacient].onkolProhlidka = textBox_onkoProhlidkaMekkeTkane.Text;
                Pacient[_vybranyPacient].dutUstni = textBox_dutinaUstni.Text;
                Pacient[_vybranyPacient].hygDutUstni = textBox_hygDutUstni.Text;
                Pacient[_vybranyPacient].orthoAnomalie = textBox_orthoAnomalie.Text;
                Pacient[_vybranyPacient].dispenzarizace = textBox_dispenzarizace.Text;

                // dodatek k User_C_1,2,3
                Pacient[_vybranyPacient].datum1 = textBoxDatum1.Text;
                Pacient[_vybranyPacient].datum2 = textBoxDatum2.Text;
                Pacient[_vybranyPacient].datum3 = textBoxDatum3.Text;
                Pacient[_vybranyPacient].podp1 = textBoxPodp1.Text;
                Pacient[_vybranyPacient].podp2 = textBoxPodp2.Text;
                Pacient[_vybranyPacient].podp3 = textBoxPodp3.Text;

                // cislo karty
                Pacient[_vybranyPacient].cisloKarty = Convert.ToInt32(textBoxCisloKarty.Text);

                // prevedeni dat do zasobniku z formulare 
                string[] buffer1 = userControlMain1.OdesliData();
                string[] buffer2 = userControlMain2.OdesliData();
                string[] buffer3 = userControlMain3.OdesliData();
                for (int uc = 0; uc < _UClength; uc++)
                {
                    Pacient[_vybranyPacient].User_C_1[uc] = buffer1[uc];
                    Pacient[_vybranyPacient].User_C_2[uc] = buffer2[uc];
                    Pacient[_vybranyPacient].User_C_3[uc] = buffer3[uc];
                }

                // ulozeni do databaze
                using (BinaryWriter bw = new BinaryWriter(File.Open(_path, FileMode.Create)))
                {
                    for (int i = 0; i < Pacient.Length; i++)
                    {
                        if (Pacient[i].prijmeni != "" || Pacient[i].jmeno != "" || Pacient[i].rodneCislo != "")
                        {
                            bw.Write(Pacient[i].prijmeni);
                            bw.Write(Pacient[i].jmeno);
                            bw.Write(Pacient[i].datumNarozeni);
                            bw.Write(Pacient[i].povolani);
                            bw.Write(Pacient[i].adresa);
                            bw.Write(Pacient[i].telefon);
                            bw.Write(Pacient[i].zamestnavatel);
                            bw.Write(Pacient[i].mistoNarozeni);
                            bw.Write(Pacient[i].zdravotnickeZarizeni);
                            bw.Write(Pacient[i].rodneCislo);
                            bw.Write(Pacient[i].zp);

                            bw.Write(Pacient[i].osetreni_diagnoza);

                            // 2. strana
                            bw.Write(Pacient[i].parodontopatie);
                            bw.Write(Pacient[i].onkolProhlidka);
                            bw.Write(Pacient[i].dutUstni);
                            bw.Write(Pacient[i].hygDutUstni);
                            bw.Write(Pacient[i].orthoAnomalie);
                            bw.Write(Pacient[i].dispenzarizace);

                            // dodatek k User_C_1,2,3
                            bw.Write(Pacient[i].datum1);
                            bw.Write(Pacient[i].datum2);
                            bw.Write(Pacient[i].datum3);
                            bw.Write(Pacient[i].podp1);
                            bw.Write(Pacient[i].podp2);
                            bw.Write(Pacient[i].podp3);

                            // cislo karty
                            bw.Write(Pacient[i].cisloKarty);

                            // ulozeni dat z controlu
                            for (int uc = 0; uc < _UClength; uc++)
                            {
                                bw.Write(Pacient[i].User_C_1[uc]);
                                bw.Write(Pacient[i].User_C_2[uc]);
                                bw.Write(Pacient[i].User_C_3[uc]);
                            }
                        }
                    }

                    bw.Flush();
                    bw.Close();

                    PrehledAkciProgramu("uložení databáze" + _path);
                }

                // aktualizuj combobox
                uložitZálohuToolStripMenuItem.Visible = true;
                AktualizaceSeznamu();
            }
        }

        // aktualizace comboboxu na zaklade zaznamu
        private void AktualizaceSeznamu()
        {
            comboBoxSeznamKaret.Items.Clear(); // vycisteni

            for (int i = 0; i < Pacient.Length; i++)
            {
                if (Pacient[i].prijmeni != "" && Pacient[i].jmeno != "" && Pacient[i].rodneCislo != "" && Pacient[i].cisloKarty != 0)
                {
                    comboBoxSeznamKaret.Items.Add(Pacient[i].prijmeni + " " + Pacient[i].jmeno + " " + Pacient[i].rodneCislo + " " + Pacient[i].cisloKarty.ToString());
                }
            }
        }

        // zmenil se vyber - ukaz dana data
        private void comboBoxSeznamKaret_TextChanged(object sender, EventArgs e)
        {
            // prochazeni seznamu pro nalezeni shody
            for (int i = 0; i < Pacient.Length; i++)
            {
                if ((Pacient[i].prijmeni + " " + Pacient[i].jmeno + " " + Pacient[i].rodneCislo + " " + Pacient[i].cisloKarty.ToString()) == comboBoxSeznamKaret.Text)
                {
                    _vybranyPacient = i;

                    textBoxPrijmeni.Text = Pacient[i].prijmeni;
                    textBoxJmeno.Text = Pacient[i].jmeno;
                    textBoxDatumNarozeni.Text = Pacient[i].datumNarozeni;
                    textBoxPovolani.Text = Pacient[i].povolani;
                    textBoxAdresa.Text = Pacient[i].adresa;
                    textBoxTelefon.Text = Pacient[i].telefon;
                    textBoxZamestnavatel.Text = Pacient[i].zamestnavatel;
                    textBoxMistoNarozeni.Text = Pacient[i].mistoNarozeni;
                    textBoxZdravotnickeZarizeni.Text = Pacient[i].zdravotnickeZarizeni;
                    textBoxRodneCislo.Text = Pacient[i].rodneCislo;
                    textBoxZP.Text = Pacient[i].zp;

                    textBoxOsetreni.Text = Pacient[i].osetreni_diagnoza;

                    // 2. strana
                    textBox_parodontopatie.Text = Pacient[i].parodontopatie;
                    textBox_onkoProhlidkaMekkeTkane.Text = Pacient[i].onkolProhlidka;
                    textBox_dutinaUstni.Text = Pacient[i].dutUstni;
                    textBox_hygDutUstni.Text = Pacient[i].hygDutUstni;
                    textBox_orthoAnomalie.Text = Pacient[i].orthoAnomalie;
                    textBox_dispenzarizace.Text = Pacient[i].dispenzarizace;

                    // dodatek k User_C_1,2,3
                    textBoxDatum1.Text = Pacient[i].datum1;
                    textBoxDatum2.Text = Pacient[i].datum2;
                    textBoxDatum3.Text = Pacient[i].datum3;
                    textBoxPodp1.Text = Pacient[i].podp1;
                    textBoxPodp2.Text = Pacient[i].podp2;
                    textBoxPodp3.Text = Pacient[i].podp3;

                    // cislo karty
                    textBoxCisloKarty.Text = Pacient[i].cisloKarty.ToString();

                    // prijmani dat z controlu
                    userControlMain1.PrijmiData(Pacient[i].User_C_1);
                    userControlMain2.PrijmiData(Pacient[i].User_C_2);
                    userControlMain3.PrijmiData(Pacient[i].User_C_3);

                    buttonRozsireniKarty.Visible = true;
                    buttonUlozKartu.Visible = true;
                    buttonSmaz.Visible = true;

                    // povoleni psani do karet
                    buttonUlozKartu.Visible = true;
                    textBoxPrijmeni.ReadOnly = false;
                    textBoxJmeno.ReadOnly = false;
                    textBoxDatumNarozeni.ReadOnly = false;
                    textBoxPovolani.ReadOnly = false;
                    textBoxAdresa.ReadOnly = false;
                    textBoxTelefon.ReadOnly = false;
                    textBoxZamestnavatel.ReadOnly = false;
                    textBoxMistoNarozeni.ReadOnly = false;
                    textBoxZdravotnickeZarizeni.ReadOnly = false;
                    textBoxRodneCislo.ReadOnly = false;
                    textBoxZP.ReadOnly = false;

                    textBoxOsetreni.ReadOnly = false;

                    // 2. strana
                    textBox_parodontopatie.ReadOnly = false;
                    textBox_onkoProhlidkaMekkeTkane.ReadOnly = false;
                    textBox_dutinaUstni.ReadOnly = false;
                    textBox_hygDutUstni.ReadOnly = false;
                    textBox_orthoAnomalie.ReadOnly = false;
                    textBox_dispenzarizace.ReadOnly = false;

                    userControlMain1.Visible = true;
                    userControlMain2.Visible = true;
                    userControlMain3.Visible = true;

                    // dodatek k User_C_1,2,3
                    textBoxDatum1.ReadOnly = false;
                    textBoxDatum2.ReadOnly = false;
                    textBoxDatum3.ReadOnly = false;
                    textBoxPodp1.ReadOnly = false;
                    textBoxPodp2.ReadOnly = false;
                    textBoxPodp3.ReadOnly = false;

                    uložitZálohuToolStripMenuItem.Visible = true;

                    PrehledAkciProgramu("vybrání karty: " + (_vybranyPacient + 1).ToString());
                    break;
                }
            }
        }

        // zprava tisku
        private void buttonTisk_Click(object sender, EventArgs e)
        {
            if (panelStrana2.Visible == false)
            {
                try
                {
                    Print(this.panelMain);
                    PrehledAkciProgramu("tisk karty: " + (_vybranyPacient + 1).ToString() + " přední strana");
                }
                catch
                {
                    PrehledAkciProgramu("CHYBA tisku karty: " + (_vybranyPacient + 1).ToString() + " přední strana");
                }
            }
            else if (panelStrana2.Visible == true)
            {
                try
                {
                    Print2(this.panelStrana2);
                    PrehledAkciProgramu("tisk karty: " + (_vybranyPacient + 1).ToString() + " zadní strana");
                }
                catch
                {
                    PrehledAkciProgramu("CHYBA tisku karty: " + (_vybranyPacient + 1).ToString() + " přední strana");
                }
            }

        }

        // print panel main
        public void Print(Panel pnl)
        {
            PrinterSettings ps = new PrinterSettings(); ;
            panelMain = pnl;
            Getprintarea(pnl);
            prntprvw.Document = pntdoc;
            pntdoc.PrintPage += new PrintPageEventHandler(pntdoc_PrintPage);
            prntprvw.ShowDialog();
        }

        public void pntdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            e.Graphics.DrawImage(memorying, ((pagearea.Width / 2) - this.panelMain.Width / 2), this.panelMain.Location.Y);
        }

        
        public void Getprintarea(Panel pnl)
        {
            memorying = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(memorying, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }

        // print panelStrana2
        public void Print2(Panel pnl)
        {
            PrinterSettings ps = new PrinterSettings(); ;
            panelStrana2 = pnl;
            Getprintarea2(pnl);
            prntprvw.Document = pntdoc;
            pntdoc.PrintPage += new PrintPageEventHandler(pntdoc_PrintPage2);
            prntprvw.ShowDialog();
        }

        public void pntdoc_PrintPage2(object sender, PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            e.Graphics.DrawImage(memorying2, ((pagearea.Width / 2) - this.panelStrana2.Width / 2), this.panelStrana2.Location.Y);
        }

        
        public void Getprintarea2(Panel pnl)
        {
            memorying2 = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(memorying2, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }

        private void buttonNovaKarta_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Pacient.Length; i++)
            {
                // karta je volna
                if (Pacient[i].jmeno == "" && Pacient[i].prijmeni == "" && Pacient[i].rodneCislo == "") // KLICE JSOU PRAZDNE
                {
                    textBoxPrijmeni.Text = "";
                    textBoxJmeno.Text = "";
                    textBoxDatumNarozeni.Text = "";
                    textBoxPovolani.Text = "";
                    textBoxAdresa.Text = "";
                    textBoxTelefon.Text = "";
                    textBoxZamestnavatel.Text = "";
                    textBoxMistoNarozeni.Text = "";
                    textBoxZdravotnickeZarizeni.Text = "";
                    textBoxRodneCislo.Text = "";
                    textBoxZP.Text = "";

                    textBoxOsetreni.Text = "";

                    // 2. strana
                    textBox_parodontopatie.Text = "";
                    textBox_onkoProhlidkaMekkeTkane.Text = "";
                    textBox_dutinaUstni.Text = "";
                    textBox_hygDutUstni.Text = "";
                    textBox_orthoAnomalie.Text = "";
                    textBox_dispenzarizace.Text = "";

                    userControlMain1.NastavNaVychozi();
                    userControlMain2.NastavNaVychozi();
                    userControlMain3.NastavNaVychozi();

                    // dodatek k User_C_1,2,3
                    textBoxDatum1.Text = "";
                    textBoxDatum2.Text = "";
                    textBoxDatum3.Text = "";
                    textBoxPodp1.Text = "";
                    textBoxPodp2.Text = "";
                    textBoxPodp3.Text = "";

                    // cislo karty - prvni zaznam
                    textBoxCisloKarty.Text = "1";

                    _vybranyPacient = i;
                    break;
                }

                // karta neni volna, je potreba ji rozsirit a psat na nove misto
                if (i == (Pacient.Length - 1))
                {
                    ZmenaVelikostiKartaPacient((Pacient.Length + 1)); // zmena velikosti karty

                    textBoxPrijmeni.Text = "";
                    textBoxJmeno.Text = "";
                    textBoxDatumNarozeni.Text = "";
                    textBoxPovolani.Text = "";
                    textBoxAdresa.Text = "";
                    textBoxTelefon.Text = "";
                    textBoxZamestnavatel.Text = "";
                    textBoxMistoNarozeni.Text = "";
                    textBoxZdravotnickeZarizeni.Text = "";
                    textBoxRodneCislo.Text = "";
                    textBoxZP.Text = "";

                    textBoxOsetreni.Text = "";

                    // 2. strana
                    textBox_parodontopatie.Text = "";
                    textBox_onkoProhlidkaMekkeTkane.Text = "";
                    textBox_dutinaUstni.Text = "";
                    textBox_hygDutUstni.Text = "";
                    textBox_orthoAnomalie.Text = "";
                    textBox_dispenzarizace.Text = "";

                    userControlMain1.NastavNaVychozi();
                    userControlMain2.NastavNaVychozi();
                    userControlMain3.NastavNaVychozi();

                    // dodatek k User_C_1,2,3
                    textBoxDatum1.Text = "";
                    textBoxDatum2.Text = "";
                    textBoxDatum3.Text = "";
                    textBoxPodp1.Text = "";
                    textBoxPodp2.Text = "";
                    textBoxPodp3.Text = "";

                    // cislo karty - prvni zaznam
                    textBoxCisloKarty.Text = "1";

                    //_vybranyPacient = (Pacient.Length - 1);
                    _vybranyPacient = (Pacient.Length - 1);
                    break;
                }

            }

            // po zalozeni nove karty vim jaky pacient je vybran
            buttonUlozKartu.Visible = true;
            textBoxPrijmeni.ReadOnly = false;
            textBoxJmeno.ReadOnly = false;
            textBoxDatumNarozeni.ReadOnly = false;
            textBoxPovolani.ReadOnly = false;
            textBoxAdresa.ReadOnly = false;
            textBoxTelefon.ReadOnly = false;
            textBoxZamestnavatel.ReadOnly = false;
            textBoxMistoNarozeni.ReadOnly = false;
            textBoxZdravotnickeZarizeni.ReadOnly = false;
            textBoxRodneCislo.ReadOnly = false;
            textBoxZP.ReadOnly = false;

            textBoxOsetreni.ReadOnly = false;

            // 2. strana
            textBox_parodontopatie.ReadOnly = false;
            textBox_onkoProhlidkaMekkeTkane.ReadOnly = false;
            textBox_dutinaUstni.ReadOnly = false;
            textBox_hygDutUstni.ReadOnly = false;
            textBox_orthoAnomalie.ReadOnly = false;
            textBox_dispenzarizace.ReadOnly = false;

            userControlMain1.Visible = true;
            userControlMain2.Visible = true;
            userControlMain3.Visible = true;

            // dodatek k User_C_1,2,3
            textBoxDatum1.ReadOnly = false;
            textBoxDatum2.ReadOnly = false;
            textBoxDatum3.ReadOnly = false;
            textBoxPodp1.ReadOnly = false;
            textBoxPodp2.ReadOnly = false;
            textBoxPodp3.ReadOnly = false;

            AktualizaceSeznamu();
            PrehledAkciProgramu("vytvoření nové karty pacienta");
        }

        // rozsireni vybrane karty
        private void buttonRozsireniKarty_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Pacient.Length; i++)
            {
                // karta je volna
                if (Pacient[i].jmeno == "" && Pacient[i].prijmeni == "" && Pacient[i].rodneCislo == "") // KLICE JSOU PRAZDNE
                {
                    textBoxOsetreni.Text = "";

                    // 2. strana
                    textBox_parodontopatie.Text = "";
                    textBox_onkoProhlidkaMekkeTkane.Text = "";
                    textBox_dutinaUstni.Text = "";
                    textBox_hygDutUstni.Text = "";
                    textBox_orthoAnomalie.Text = "";
                    textBox_dispenzarizace.Text = "";

                    userControlMain1.NastavNaVychozi();
                    userControlMain2.NastavNaVychozi();
                    userControlMain3.NastavNaVychozi();

                    // dodatek k User_C_1,2,3
                    textBoxDatum1.Text = "";
                    textBoxDatum2.Text = "";
                    textBoxDatum3.Text = "";
                    textBoxPodp1.Text = "";
                    textBoxPodp2.Text = "";
                    textBoxPodp3.Text = "";

                    // cislo karty - x zaznam
                    textBoxCisloKarty.Text = (Convert.ToInt32(textBoxCisloKarty.Text) + 1).ToString();

                    _vybranyPacient = i;
                    break;
                }

                // karta neni volna, je potreba ji rozsirit a psat na nove misto
                if (i == (Pacient.Length - 1))
                {
                    ZmenaVelikostiKartaPacient((Pacient.Length + 1)); // zmena velikosti karty
                    textBoxOsetreni.Text = "";

                    // 2. strana
                    textBox_parodontopatie.Text = "";
                    textBox_onkoProhlidkaMekkeTkane.Text = "";
                    textBox_dutinaUstni.Text = "";
                    textBox_hygDutUstni.Text = "";
                    textBox_orthoAnomalie.Text = "";
                    textBox_dispenzarizace.Text = "";

                    userControlMain1.NastavNaVychozi();
                    userControlMain2.NastavNaVychozi();
                    userControlMain3.NastavNaVychozi();

                    // dodatek k User_C_1,2,3
                    textBoxDatum1.Text = "";
                    textBoxDatum2.Text = "";
                    textBoxDatum3.Text = "";
                    textBoxPodp1.Text = "";
                    textBoxPodp2.Text = "";
                    textBoxPodp3.Text = "";

                    // cislo karty - x zaznam
                    textBoxCisloKarty.Text = (Convert.ToInt32(textBoxCisloKarty.Text) + 1).ToString();

                    //_vybranyPacient = (Pacient.Length - 1);
                    _vybranyPacient = (Pacient.Length - 1);
                    break;
                }

            }

            // po rozsireni nove karty vim jaky pacient je vybran
            buttonUlozKartu.Visible = true;
            textBoxPrijmeni.ReadOnly = false;
            textBoxJmeno.ReadOnly = false;
            textBoxDatumNarozeni.ReadOnly = false;
            textBoxPovolani.ReadOnly = false;
            textBoxAdresa.ReadOnly = false;
            textBoxTelefon.ReadOnly = false;
            textBoxZamestnavatel.ReadOnly = false;
            textBoxMistoNarozeni.ReadOnly = false;
            textBoxZdravotnickeZarizeni.ReadOnly = false;
            textBoxRodneCislo.ReadOnly = false;
            textBoxZP.ReadOnly = false;

            textBoxOsetreni.ReadOnly = false;

            // 2. strana
            textBox_parodontopatie.ReadOnly = false;
            textBox_onkoProhlidkaMekkeTkane.ReadOnly = false;
            textBox_dutinaUstni.ReadOnly = false;
            textBox_hygDutUstni.ReadOnly = false;
            textBox_orthoAnomalie.ReadOnly = false;
            textBox_dispenzarizace.ReadOnly = false;

            userControlMain1.Visible = true;
            userControlMain2.Visible = true;
            userControlMain3.Visible = true;

            // dodatek k User_C_1,2,3
            textBoxDatum1.ReadOnly = false;
            textBoxDatum2.ReadOnly = false;
            textBoxDatum3.ReadOnly = false;
            textBoxPodp1.ReadOnly = false;
            textBoxPodp2.ReadOnly = false;
            textBoxPodp3.ReadOnly = false;

            AktualizaceSeznamu();

            // vypnuti rozsireni dokud kartu nevyberu
            buttonRozsireniKarty.Visible = false;
            PrehledAkciProgramu("Rozšíření karty" + Pacient[_vybranyPacient].prijmeni + " " + Pacient[_vybranyPacient].jmeno + " " + Pacient[_vybranyPacient].rodneCislo);
        }

        private void buttonPrepnutiStranKarty_Click(object sender, EventArgs e)
        {
            if (panelStrana2.Visible == false)
            {
                panelStrana2.Visible = true;

                // nabidka pro druhou stranu formulare
                buttonZmenaTypuChrupu1.Visible = true;
                buttonZmenaTypuChrupu2.Visible = true;
                buttonZmenaTypuChrupu3.Visible = true;

                PrehledAkciProgramu("Přepnutí karty na zadní stranu");
            }
            else if (panelStrana2.Visible == true)
            {
                panelStrana2.Visible = false;

                // nabidka pro druhou stranu formulare
                buttonZmenaTypuChrupu1.Visible = false;
                buttonZmenaTypuChrupu2.Visible = false;
                buttonZmenaTypuChrupu3.Visible = false;

                PrehledAkciProgramu("Přepnutí karty na přední stranu");
            }
        }

        private void pouzeUkončitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

        private void buttonSmaz_Click(object sender, EventArgs e)
        {
            // prepsani karty do smazane
            SmazanaKarta.NakopirovaniKarty(Pacient[_vybranyPacient]);

            // mazani z databaze
            Pacient[_vybranyPacient].prijmeni = "";
            Pacient[_vybranyPacient].jmeno = "";
            Pacient[_vybranyPacient].datumNarozeni = "";
            Pacient[_vybranyPacient].povolani = "";
            Pacient[_vybranyPacient].adresa = "";
            Pacient[_vybranyPacient].telefon = "";
            Pacient[_vybranyPacient].zamestnavatel = "";
            Pacient[_vybranyPacient].mistoNarozeni = "";
            Pacient[_vybranyPacient].zdravotnickeZarizeni = "";
            Pacient[_vybranyPacient].rodneCislo = "";
            Pacient[_vybranyPacient].zp = "";

            Pacient[_vybranyPacient].osetreni_diagnoza = "";

            // 2. strana
            Pacient[_vybranyPacient].parodontopatie = "";
            Pacient[_vybranyPacient].onkolProhlidka = "";
            Pacient[_vybranyPacient].dutUstni = "";
            Pacient[_vybranyPacient].hygDutUstni = "";
            Pacient[_vybranyPacient].orthoAnomalie = "";
            Pacient[_vybranyPacient].dispenzarizace = "";

            // mazani z textboxu
            textBoxPrijmeni.Text = "";
            textBoxJmeno.Text = "";
            textBoxDatumNarozeni.Text = "";
            textBoxPovolani.Text = "";
            textBoxAdresa.Text = "";
            textBoxTelefon.Text = "";
            textBoxZamestnavatel.Text = "";
            textBoxMistoNarozeni.Text = "";
            textBoxZdravotnickeZarizeni.Text = "";
            textBoxRodneCislo.Text = "";
            textBoxZP.Text = "";

            textBoxOsetreni.Text = "";

            // 2. strana
            textBox_parodontopatie.Text = "";
            textBox_onkoProhlidkaMekkeTkane.Text = "";
            textBox_dutinaUstni.Text = "";
            textBox_hygDutUstni.Text = "";
            textBox_orthoAnomalie.Text = "";
            textBox_dispenzarizace.Text = "";

            userControlMain1.NastavNaVychozi();
            userControlMain2.NastavNaVychozi();
            userControlMain3.NastavNaVychozi();

            // dodatek k User_C_1,2,3
            textBoxDatum1.Text = "";
            textBoxDatum2.Text = "";
            textBoxDatum3.Text = "";
            textBoxPodp1.Text = "";
            textBoxPodp2.Text = "";
            textBoxPodp3.Text = "";

            textBoxCisloKarty.Text = "";

            // zakazani ukladani
            // po zalozeni nove karty vim jaky pacient je vybran
            buttonRozsireniKarty.Visible = false;
            buttonUlozKartu.Visible = false;
            textBoxPrijmeni.ReadOnly = true;
            textBoxJmeno.ReadOnly = true;
            textBoxDatumNarozeni.ReadOnly = true;
            textBoxPovolani.ReadOnly = true;
            textBoxAdresa.ReadOnly = true;
            textBoxTelefon.ReadOnly = true;
            textBoxZamestnavatel.ReadOnly = true;
            textBoxMistoNarozeni.ReadOnly = true;
            textBoxZdravotnickeZarizeni.ReadOnly = true;
            textBoxRodneCislo.ReadOnly = true;
            textBoxZP.ReadOnly = true;

            textBoxOsetreni.ReadOnly = true;

            // 2. strana
            textBox_parodontopatie.ReadOnly = true;
            textBox_onkoProhlidkaMekkeTkane.ReadOnly = true;
            textBox_dutinaUstni.ReadOnly = true;
            textBox_hygDutUstni.ReadOnly = true;
            textBox_orthoAnomalie.ReadOnly = true;
            textBox_dispenzarizace.ReadOnly = true;

            userControlMain1.Visible = false;
            userControlMain2.Visible = false;
            userControlMain3.Visible = false;

            // dodatek k User_C_1,2,3
            textBoxDatum1.ReadOnly = true;
            textBoxDatum2.ReadOnly = true;
            textBoxDatum3.ReadOnly = true;
            textBoxPodp1.ReadOnly = true;
            textBoxPodp2.ReadOnly = true;
            textBoxPodp3.ReadOnly = true;


            // ulozeni do databaze
            using (BinaryWriter bw = new BinaryWriter(File.Open(_path, FileMode.Create)))
            {
                for (int i = 0; i < Pacient.Length; i++)
                {
                    if (Pacient[i].prijmeni != "" || Pacient[i].jmeno != "" || Pacient[i].rodneCislo != "")
                    {
                        bw.Write(Pacient[i].prijmeni);
                        bw.Write(Pacient[i].jmeno);
                        bw.Write(Pacient[i].datumNarozeni);
                        bw.Write(Pacient[i].povolani);
                        bw.Write(Pacient[i].adresa);
                        bw.Write(Pacient[i].telefon);
                        bw.Write(Pacient[i].zamestnavatel);
                        bw.Write(Pacient[i].mistoNarozeni);
                        bw.Write(Pacient[i].zdravotnickeZarizeni);
                        bw.Write(Pacient[i].rodneCislo);
                        bw.Write(Pacient[i].zp);

                        bw.Write(Pacient[i].osetreni_diagnoza);

                        // 2. strana
                        bw.Write(Pacient[i].parodontopatie);
                        bw.Write(Pacient[i].onkolProhlidka);
                        bw.Write(Pacient[i].dutUstni);
                        bw.Write(Pacient[i].hygDutUstni);
                        bw.Write(Pacient[i].orthoAnomalie);
                        bw.Write(Pacient[i].dispenzarizace);

                        // dodatek k User_C_1,2,3
                        bw.Write(Pacient[i].datum1);
                        bw.Write(Pacient[i].datum2);
                        bw.Write(Pacient[i].datum3);
                        bw.Write(Pacient[i].podp1);
                        bw.Write(Pacient[i].podp2);
                        bw.Write(Pacient[i].podp3);

                        // cislo karty
                        bw.Write(Pacient[i].cisloKarty);

                        // ulozeni dat z controlu
                        for (int uc = 0; uc < _UClength; uc++)
                        {
                            bw.Write(Pacient[i].User_C_1[uc]);
                            bw.Write(Pacient[i].User_C_2[uc]);
                            bw.Write(Pacient[i].User_C_3[uc]);
                        }
                    }
                }

                bw.Flush();
                bw.Close();

                //PrehledAkciProgramu("uložení databáze" + _path);
            }

            AktualizaceSeznamu();
            PrehledAkciProgramu("smazání karty: " + (_vybranyPacient + 1).ToString());
        }

        private void uložitZálohuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string cas = dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString();
            string pathZaloha = @"ZdravotniZaznamyDatabaze\Zaloha\zaloha" + cas + ".bin";

            // ulozeni do databaze
            using (BinaryWriter bw = new BinaryWriter(File.Open(pathZaloha, FileMode.Create)))
            {
                try
                {
                    for (int i = 0; i < Pacient.Length; i++)
                    {
                        if (Pacient[i].prijmeni != "" || Pacient[i].jmeno != "" || Pacient[i].rodneCislo != "")
                        {
                            bw.Write(Pacient[i].prijmeni);
                            bw.Write(Pacient[i].jmeno);
                            bw.Write(Pacient[i].datumNarozeni);
                            bw.Write(Pacient[i].povolani);
                            bw.Write(Pacient[i].adresa);
                            bw.Write(Pacient[i].telefon);
                            bw.Write(Pacient[i].zamestnavatel);
                            bw.Write(Pacient[i].mistoNarozeni);
                            bw.Write(Pacient[i].zdravotnickeZarizeni);
                            bw.Write(Pacient[i].rodneCislo);
                            bw.Write(Pacient[i].zp);

                            bw.Write(Pacient[i].osetreni_diagnoza);

                            // 2. strana
                            bw.Write(Pacient[i].parodontopatie);
                            bw.Write(Pacient[i].onkolProhlidka);
                            bw.Write(Pacient[i].dutUstni);
                            bw.Write(Pacient[i].hygDutUstni);
                            bw.Write(Pacient[i].orthoAnomalie);
                            bw.Write(Pacient[i].dispenzarizace);

                            // dodatek k User_C_1,2,3
                            bw.Write(Pacient[i].datum1);
                            bw.Write(Pacient[i].datum2);
                            bw.Write(Pacient[i].datum3);
                            bw.Write(Pacient[i].podp1);
                            bw.Write(Pacient[i].podp2);
                            bw.Write(Pacient[i].podp3);

                            // cislo karty
                            bw.Write(Pacient[i].cisloKarty);

                            // ulozeni dat z controlu
                            for (int uc = 0; uc < _UClength; uc++)
                            {
                                bw.Write(Pacient[i].User_C_1[uc]);
                                bw.Write(Pacient[i].User_C_2[uc]);
                                bw.Write(Pacient[i].User_C_3[uc]);
                            }
                        }
                    }

                    bw.Flush();
                    bw.Close();

                    PrehledAkciProgramu("uložení zálohy: " + cas + ".bin");

                }
                catch
                {
                    PrehledAkciProgramu("záloha neobsahuje žádná data, nebude uložena");
                }
            }


        }

        // funkce nacte akci, prida k ni cas a vypise ji do textboxu
        // ukladani do --> _akceProgramu
        private void PrehledAkciProgramu(string akce)
        {
            DateTime dt = DateTime.Now;
            string cas = dt.Year.ToString() + "." + dt.Month.ToString() + "." + dt.Day.ToString() + ": " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();

            _akceProgramu = cas + ": " + akce + Environment.NewLine + _akceProgramu;

            textBoxOperaceProgramu.Text = _akceProgramu;
        }

        private void otevřeníZálohyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // vycisteni pameti pred nahranim
            for (int i = 0; i < Pacient.Length; i++)
            {
                Pacient[i].UvolneniKartyZPameti();
            }

            using (OpenFileDialog op = new OpenFileDialog())
            {
                try
                {
                    op.InitialDirectory = @"ZdravotniZaznamyDatabaze\Zaloha";
                    op.Filter = "bin files (*.bin)|*.bin";
                    op.FilterIndex = 1;
                    op.RestoreDirectory = true;

                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        _path = op.FileName; // prepsani cesty
                        ZmenaVelikostiKartaPacient(PredNahraniDTB());
                        NahraniDTB();
                        AktualizaceSeznamu(); // ukazka pro combobox
                        PrehledAkciProgramu("spuštění záložní databáze " + _path);
                    }
                }
                catch
                {
                    PrehledAkciProgramu("nepodařilo se nahrát vybranou databázi");
                }
            }

            //labelJmenoDatabaze.Text = "Jméno databáze: " + _path;
        }

        private void buttonHeslo_Click(object sender, EventArgs e)
        {
            string obsahHesla = textBoxHeslo.Text;

            if (Dekoder(obsahHesla) == true && buttonHeslo.Text == "Zadat heslo")
            {
                souborToolStripMenuItem.Enabled = true;
                zálohaToolStripMenuItem.Enabled = true;
                buttonNovaKarta.Enabled = true;
                buttonUlozKartu.Enabled = true;
                buttonTisk.Enabled = true;
                buttonPrepnutiStranKarty.Enabled = true;
                buttonSmaz.Enabled = true;
                comboBoxSeznamKaret.Enabled = true;

                // Nabidka zmizi
                textBoxHeslo.Visible = false;
                buttonHeslo.Visible = false;

                textBoxHeslo.Text = "";

                buttonHeslo.Text = "změnit heslo";
                PrehledAkciProgramu("heslo bylo úspěšně zadáno");
                return;
            }
            else if (buttonHeslo.Text == "změnit heslo")
            {
                Koder(obsahHesla);

                using (BinaryWriter bw = new BinaryWriter(File.Open(_keyPath, FileMode.Create)))
                {
                    bw.Write(_key[0]);
                    bw.Write(_key[1]);
                    bw.Write(_key[2]);
                    bw.Write(_key[3]);
                    bw.Write(_key[4]);
                    bw.Flush();
                    bw.Close();
                }

                // Nabidka zmizi
                textBoxHeslo.Visible = false;
                buttonHeslo.Visible = false;

                textBoxHeslo.Text = "";

                PrehledAkciProgramu("heslo bylo úspěšně změněno");
                return;
            }

            // zadni vratka
            else if (obsahHesla == "moronMofbone0")
            {
                souborToolStripMenuItem.Enabled = true;
                zálohaToolStripMenuItem.Enabled = true;
                buttonNovaKarta.Enabled = true;
                buttonUlozKartu.Enabled = true;
                buttonTisk.Enabled = true;
                buttonPrepnutiStranKarty.Enabled = true;
                buttonSmaz.Enabled = true;
                comboBoxSeznamKaret.Enabled = true;

                buttonHeslo.Text = "změnit heslo";
                PrehledAkciProgramu("heslo bylo úspěšně zadáno");

                // Nabidka zmizi
                textBoxHeslo.Visible = false;
                buttonHeslo.Visible = false;

                textBoxHeslo.Text = "";
                return;
            }

            PrehledAkciProgramu("zadání nesprávného hesla");
            textBoxHeslo.Text = "";

        }

        // zakodovani - podle kodu preda kody
        private void Koder(string kod) // kod = heslo ktere je potreba zakodovat
        {
            DateTime dt = DateTime.Now;
            //string[] kody = new string[5];
            char[] kodChar = kod.ToCharArray();
            // pole charu
            char[] n = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'ě', 'Ě', 'š', 'Š', 'č', 'Č', 'ř', 'Ř', 'ž', 'Ž', 'ý', 'Ý', 'í', 'Í', 'é', 'É', 'Ú', 'ú', '¨', '#', '@', '$', '%', '^', '&', '*', '(', ')' };
            int pocitadlo = 0;

            _key[0] = dt.Year.ToString();
            _key[1] = dt.Month.ToString();  // n ty clen v kterem je kod vlozen
            _key[2] = dt.Day.ToString();
            _key[3] = kodChar.Length.ToString(); // delka hesla
            _key[4] = "";


            for (int i = 0; i < 400; i++)
            {
                if ((pocitadlo < kodChar.Length) && (i % Convert.ToInt32(_key[1])) == 0)
                {
                    _key[4] += kodChar[pocitadlo];
                    pocitadlo += 1;
                }
                else
                {
                    _key[4] += n[Kostka(0, n.Length - 1, true)];
                }
            }
        }

        // vraci odkodovano / neodkodovano
        private bool Dekoder(string zadanyKod)
        {
            int nClen = Convert.ToInt32(_key[1]);
            int delkaHesla = Convert.ToInt32(_key[3]);

            try
            {
                char[] hesloTest = _key[4].ToCharArray();
            }
            catch
            {
                return false;
            }

            char[] heslo = _key[4].ToCharArray();


            string odkodovaneHeslo = "";
            int pocitadlo = 0;

            for (int i = 0; i < 400; i++)
            {
                if ((pocitadlo < delkaHesla) && (i % nClen) == 0)
                {
                    odkodovaneHeslo += heslo[i];
                    pocitadlo += 1;
                }
            }

            if (zadanyKod == odkodovaneHeslo) { return true; }
            else { return false; }
        }

        private int Kostka(int min, int max, bool pseudo)
        {
            Random rnd = new Random();

            if (pseudo == false)
            {
                System.Threading.Thread.Sleep(1);
            }
            else
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (true)
                {
                    //some other processing to do possible
                    if (stopwatch.ElapsedMilliseconds > 4) { break; }
                }
            }

            return rnd.Next(min, max + 1);
        }

        private void buttonZmenaTypuChrupu1_Click(object sender, EventArgs e)
        {
            if (userControlMain1.TypChrupu() == "detsky") { userControlMain1.TypChrupu("dospely"); PrehledAkciProgramu("přepnutí typu chrupu (1) na: dospělý"); }
            else if (userControlMain1.TypChrupu() == "dospely") { userControlMain1.TypChrupu("detsky"); PrehledAkciProgramu("přepnutí typu chrupu (1) na: dětský"); }
        }

        private void buttonZmenaTypuChrupu2_Click(object sender, EventArgs e)
        {
            if (userControlMain2.TypChrupu() == "detsky") { userControlMain2.TypChrupu("dospely"); PrehledAkciProgramu("přepnutí typu chrupu (1) na: dospělý"); }
            else if (userControlMain2.TypChrupu() == "dospely") { userControlMain2.TypChrupu("detsky"); PrehledAkciProgramu("přepnutí typu chrupu (1) na: dětský"); }
        }

        private void buttonZmenaTypuChrupu3_Click(object sender, EventArgs e)
        {
            if (userControlMain3.TypChrupu() == "detsky") { userControlMain3.TypChrupu("dospely"); PrehledAkciProgramu("přepnutí typu chrupu (1) na: dospělý"); }
            else if (userControlMain3.TypChrupu() == "dospely") { userControlMain3.TypChrupu("detsky"); PrehledAkciProgramu("přepnutí typu chrupu (1) na: dětský"); }
        }

        private void povolitZakázatPrávaAdministrátoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_administrator == false)
            {
                textBoxCisloKarty.ReadOnly = false;
                _administrator = true;
                PrehledAkciProgramu("práva administrátora zapnuta");
            }
            else if (_administrator == true)
            {
                textBoxCisloKarty.ReadOnly = true;
                _administrator = false;
                PrehledAkciProgramu("práva administrátora vypnuta");
            }
        }

        private void změnaHeslaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Nabidka se objeví
            textBoxHeslo.Visible = true;
            buttonHeslo.Visible = true;
            PrehledAkciProgramu("nabídka změny hesla");
        }

        private void odhlášeníToolStripMenuItem_Click(object sender, EventArgs e)
        {
            souborToolStripMenuItem.Enabled = false;
            zálohaToolStripMenuItem.Enabled = false;
            buttonNovaKarta.Enabled = false;
            buttonUlozKartu.Enabled = false;
            buttonTisk.Enabled = false;
            buttonPrepnutiStranKarty.Enabled = false;
            buttonSmaz.Enabled = false;
            comboBoxSeznamKaret.Enabled = false;

            // zmizi nabidka karet + nabidka druhe strany
            buttonRozsireniKarty.Visible = false;
            buttonZmenaTypuChrupu1.Visible = false;
            buttonZmenaTypuChrupu2.Visible = false;
            buttonZmenaTypuChrupu3.Visible = false;
            buttonUlozKartu.Visible = false;

            // Nabidka zmizi
            textBoxHeslo.Visible = true;
            buttonHeslo.Visible = true;

            textBoxHeslo.Text = "";

            buttonHeslo.Text = "Zadat heslo";
            PrehledAkciProgramu("odhlášení uživatele");

            AktualizaceSeznamu();
        }

        private void obnoveníPosledníSmazanéPoložkyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Pacient.Length; i++)
            {
                // karta je volna
                if (Pacient[i].jmeno == "" && Pacient[i].prijmeni == "" && Pacient[i].rodneCislo == "") // KLICE JSOU PRAZDNE
                {
                    _vybranyPacient = i;
                    break;
                }
                if (i == (Pacient.Length - 1))
                {
                    ZmenaVelikostiKartaPacient((Pacient.Length + 1)); // zmena velikosti karty
                    _vybranyPacient = (Pacient.Length - 1);
                    break;
                }
            }

            // po zalozeni nove karty vim jaky pacient je vybran
            buttonUlozKartu.Visible = true;
            textBoxPrijmeni.ReadOnly = false;
            textBoxJmeno.ReadOnly = false;
            textBoxDatumNarozeni.ReadOnly = false;
            textBoxPovolani.ReadOnly = false;
            textBoxAdresa.ReadOnly = false;
            textBoxTelefon.ReadOnly = false;
            textBoxZamestnavatel.ReadOnly = false;
            textBoxMistoNarozeni.ReadOnly = false;
            textBoxZdravotnickeZarizeni.ReadOnly = false;
            textBoxRodneCislo.ReadOnly = false;
            textBoxZP.ReadOnly = false;

            textBoxOsetreni.ReadOnly = false;

            // 2. strana
            textBox_parodontopatie.ReadOnly = false;
            textBox_onkoProhlidkaMekkeTkane.ReadOnly = false;
            textBox_dutinaUstni.ReadOnly = false;
            textBox_hygDutUstni.ReadOnly = false;
            textBox_orthoAnomalie.ReadOnly = false;
            textBox_dispenzarizace.ReadOnly = false;

            userControlMain1.Visible = true;
            userControlMain2.Visible = true;
            userControlMain3.Visible = true;

            // dodatek k User_C_1,2,3
            textBoxDatum1.ReadOnly = false;
            textBoxDatum2.ReadOnly = false;
            textBoxDatum3.ReadOnly = false;
            textBoxPodp1.ReadOnly = false;
            textBoxPodp2.ReadOnly = false;
            textBoxPodp3.ReadOnly = false;

            // ----------------------------------------------
            // doplneni karty
            textBoxPrijmeni.Text = SmazanaKarta.prijmeni;
            textBoxJmeno.Text = SmazanaKarta.jmeno;
            textBoxDatumNarozeni.Text = SmazanaKarta.datumNarozeni;
            textBoxPovolani.Text = SmazanaKarta.povolani;
            textBoxAdresa.Text = SmazanaKarta.adresa;
            textBoxTelefon.Text = SmazanaKarta.telefon;
            textBoxZamestnavatel.Text = SmazanaKarta.zamestnavatel;
            textBoxMistoNarozeni.Text = SmazanaKarta.mistoNarozeni;
            textBoxZdravotnickeZarizeni.Text = SmazanaKarta.zdravotnickeZarizeni;
            textBoxRodneCislo.Text = SmazanaKarta.rodneCislo;
            textBoxZP.Text = SmazanaKarta.zp;

            textBoxOsetreni.Text = SmazanaKarta.osetreni_diagnoza;

            // 2. strana
            textBox_parodontopatie.Text = SmazanaKarta.parodontopatie;
            textBox_onkoProhlidkaMekkeTkane.Text = SmazanaKarta.onkolProhlidka;
            textBox_dutinaUstni.Text = SmazanaKarta.dutUstni;
            textBox_hygDutUstni.Text = SmazanaKarta.hygDutUstni;
            textBox_orthoAnomalie.Text = SmazanaKarta.orthoAnomalie;
            textBox_dispenzarizace.Text = SmazanaKarta.dispenzarizace;


            userControlMain1.PrijmiData(SmazanaKarta.User_C_1);
            userControlMain2.PrijmiData(SmazanaKarta.User_C_2);
            userControlMain3.PrijmiData(SmazanaKarta.User_C_3);

            // dodatek k User_C_1,2,3
            textBoxDatum1.Text = SmazanaKarta.datum1;
            textBoxDatum2.Text = SmazanaKarta.datum2;
            textBoxDatum3.Text = SmazanaKarta.datum3;
            textBoxPodp1.Text = SmazanaKarta.podp1;
            textBoxPodp2.Text = SmazanaKarta.podp2;
            textBoxPodp3.Text = SmazanaKarta.podp3;

            // cislo karty - prvni zaznam
            textBoxCisloKarty.Text = SmazanaKarta.cisloKarty.ToString();


            AktualizaceSeznamu();
            PrehledAkciProgramu("obnovení smazané karty pacienta");
        }
    }
}