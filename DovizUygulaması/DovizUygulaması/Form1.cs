using EntityLayer;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DovizUygulaması
{
    public partial class Form1 : Form
    {
        void Kasa()
        {
            List<EntityKasa> Kasa = LogicKasa.LLKasaGetir();
            foreach (var item in Kasa)
            {
                LblDolar.Text = item.Dolar.ToString();
                LblEuro.Text = item.Euro.ToString();
                LblPound.Text = item.Pound.ToString();
                LblJaponYen.Text = item.Japonyen.ToString();
                LblTurkLıra.Text=item.TurkLıra.ToString();
            }  
        }
        public Form1()
        {
            InitializeComponent();          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Kasa();
            comboBox1.Items.AddRange(new object[] { "$", "€", "£", "¥" });
            comboBox2.Items.AddRange(new object[] { "$", "€", "£", "¥" });
            //$ € £ ¥
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldosya = new XmlDocument();
            xmldosya.Load(bugun);

            //dolar alış
            string dolaralis =xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml; //buradaki değerler sitenin xml kodundaki tagları ifade eder
            LblDolarAlis.Text = dolaralis;
            //dolarsatış
            string dolarsatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml; 
            LblDolarSatis.Text = dolarsatis;

            //euro alış
            string euroalis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml; 
            LblEuroAlis.Text = euroalis;
            //euro satış
            string eurosatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml; 
            LblEuroSatis.Text = eurosatis;

            //paund alış
            string paundalis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteBuying").InnerXml; 
            LblPoundAlis.Text = paundalis;
            //paund satış
            string paundsatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteSelling").InnerXml; 
            LblPoundSatis.Text = paundsatis;

            //Japon Yeni alış
            string japonyenialis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='JPY']/BanknoteBuying").InnerXml;
            LblJYenAlis.Text = japonyenialis;
            //Japon Yeni satış
            string japonyenisatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='JPY']/BanknoteSelling").InnerXml;
            LblJYenSatis.Text = japonyenisatis;
        }

        private void TxtKur_TextChanged(object sender, EventArgs e)
        {
            TxtKur.Text = TxtKur.Text.Replace(".", ",");
        }

        private void BtnSatisYap_Click(object sender, EventArgs e)
        {
            //SATMA EYLEMİNDE TL ARTACAK COMBOBOX SEÇİMİNE GÖRE DÖVİZ AZALACAK
            //(miktar - kalan) kadar tl artacak ve tutar kadar döviz azalıcak
            
            EntityKasa ent = new EntityKasa();
            if (double.TryParse(LblDolar.Text, out double Dolar) &&
                double.TryParse(LblEuro.Text, out double Euro) &&
                double.TryParse(LblPound.Text, out double Pound) &&
                double.TryParse(LblTurkLıra.Text, out double TurkLıra) &&
                double.TryParse(LblJaponYen.Text, out double JaponYen) &&
                TxtMiktar.Text != ""
                )
            {
                //işlem
                double kur = Convert.ToDouble(TxtKur.Text);
                double miktar = Convert.ToInt32(TxtMiktar.Text);
                double tutar = Convert.ToDouble(miktar / kur);
                int tamTutar = (int)tutar; // Sadece tam sayı kısmını al
                TxtTutar.Text = tamTutar.ToString() + " " + comboBox2.SelectedItem.ToString() + " Ödeme yapınız";
                double kalan = miktar % kur;
                TxtKalan.Text = kalan.ToString("F2");

                //$ € £ ¥

                if (comboBox2.SelectedItem.ToString()== "$")
                {
                    ent.Dolar = Dolar-tutar;
                    ent.TurkLıra = TurkLıra +(miktar-kalan);
                    ent.Pound = Pound;
                    ent.Euro = Euro;
                    ent.Japonyen = JaponYen;

                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz Satıldı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (comboBox2.SelectedItem.ToString() == "€")
                {
                    ent.Dolar = Dolar ;
                    ent.TurkLıra = TurkLıra + (miktar - kalan);
                    ent.Pound = Pound;
                    ent.Euro = Euro - tutar;
                    ent.Japonyen = JaponYen;

                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz Satıldı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (comboBox2.SelectedItem.ToString() == "£")
                {
                    ent.Dolar = Dolar;
                    ent.TurkLıra = TurkLıra + (miktar - kalan);
                    ent.Pound = Pound - tutar;
                    ent.Euro = Euro;
                    ent.Japonyen = JaponYen;

                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz Satıldı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (comboBox2.SelectedItem.ToString() == "¥")
                {
                    ent.Dolar = Dolar;
                    ent.TurkLıra = TurkLıra + (miktar - kalan);
                    ent.Pound = Pound;
                    ent.Euro = Euro ;
                    ent.Japonyen = JaponYen - tutar;

                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz Satıldı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen döviz seçimi ve miktar girişi yapınız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }   

        private void BtnAlisYap_Click(object sender, EventArgs e)
        {
            //ALMA EYLEMİNDE TL AZALACAK VE SEÇİLEN DÖVİZ ARTIŞ YAŞAYACAK.
            //Tutar kadar tl azalacak ve miktar kadar döviz artacak.

            EntityKasa ent = new EntityKasa();
            if (double.TryParse(LblDolar.Text, out double Dolar) &&
                double.TryParse(LblEuro.Text, out double Euro) &&
                double.TryParse(LblPound.Text, out double Pound) &&
                double.TryParse(LblTurkLıra.Text, out double TurkLıra) &&
                double.TryParse(LblJaponYen.Text, out double JaponYen) &&
                TxtMiktar.Text != ""
                )
            {
                //işlem
                double kur, miktar, tutar;
                kur = Convert.ToDouble(TxtKur.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = kur * miktar;
                TxtTutar.Text = tutar.ToString() + " ₺ ödeme yapınız";

                //şartlar
                //$ € £ ¥
                if (comboBox1.SelectedItem.ToString()== "$")
                {
                    ent.Dolar = Dolar+miktar;
                    ent.TurkLıra = TurkLıra - tutar;
                    ent.Euro = Euro;
                    ent.Pound = Pound;
                    ent.Japonyen = JaponYen;
                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz alındı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (comboBox1.SelectedItem.ToString() == "€")
                {
                    ent.Dolar = Dolar;
                    ent.TurkLıra = TurkLıra - tutar;
                    ent.Euro = Euro + miktar;
                    ent.Pound = Pound;
                    ent.Japonyen = JaponYen;
                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz alındı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (comboBox1.SelectedItem.ToString() == "£")
                {
                    ent.Dolar = Dolar;
                    ent.TurkLıra = TurkLıra - tutar;
                    ent.Euro = Euro;
                    ent.Pound = Pound + miktar;
                    ent.Japonyen = JaponYen;
                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz alındı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (comboBox1.SelectedItem.ToString() == "¥")
                {
                    ent.Dolar = Dolar;
                    ent.TurkLıra = TurkLıra - tutar;
                    ent.Euro = Euro;
                    ent.Pound = Pound;
                    ent.Japonyen = JaponYen + miktar;
                    bool result = LogicKasa.LLKasaGuncelle(ent);
                    if (result)
                    {
                        MessageBox.Show("Döviz alındı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Kasa();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen döviz seçimi ve miktar girişi yapınız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnSatisYap.Enabled = false;
            if (comboBox1.SelectedItem.ToString()== "$")
            {
                comboBox2.Enabled = false;
                TxtKur.Text = LblDolarAlis.Text;
            }
            if (comboBox1.SelectedItem.ToString()== "€")
            {
                comboBox2.Enabled = false;
                TxtKur.Text = LblEuroAlis.Text;
            }
            if (comboBox1.SelectedItem.ToString() == "£")
            {
                comboBox2.Enabled = false;
                TxtKur.Text = LblPoundAlis.Text;
            }
            if (comboBox1.SelectedItem.ToString() == "¥")
            {
                comboBox2.Enabled = false;
                TxtKur.Text = LblJYenAlis.Text;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnAlisYap.Enabled= false;
            if (comboBox2.SelectedItem.ToString() == "$")
            {
                comboBox1.Enabled = false;
                TxtKur.Text = LblDolarSatis.Text;
            }
            if (comboBox2.SelectedItem.ToString() == "€")
            {
                comboBox1.Enabled = false;
                TxtKur.Text = LblEuroSatis.Text;
            }
            if (comboBox2.SelectedItem.ToString() == "£")
            {
                comboBox1.Enabled = false;
                TxtKur.Text = LblPoundSatis.Text;
            }
            if (comboBox2.SelectedItem.ToString() == "¥")
            {
                comboBox1.Enabled = false;
                TxtKur.Text = LblJYenSatis.Text;
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKalan.Text = "";
            TxtKur.Text = "";
            TxtMiktar.Text = "";
            TxtTutar.Text = "";
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;    
            BtnAlisYap.Enabled = true;
            BtnSatisYap.Enabled= true;
        }
    }
}


