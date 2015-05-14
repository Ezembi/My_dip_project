using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace MyDiplomProject
{
    public partial class AddProtokol : Form
    {
        bool New;
        string User;
        string Password;
        string pk_protokol;



        bool Lock = true;
        bool ponatie = false;

        string oldVDCell0;
        string oldVDCell1;
        string oldVDCell2;
        string oldVDCell3;

        string oldLCell0;
        string oldLCell1;

        int VeshDokItems;
        int LicaItems;

        public AddProtokol()
        {
            InitializeComponent();
        }

        public AddProtokol(string _User, string _Pass, bool _new, string _pk_pro)
        {
            User = _User;
            Password = _Pass;
            New = _new;
            pk_protokol = _pk_pro;
            InitializeComponent();
        }

        void AddPonatoi()
        {
            //if (!Lock)
            //{
            //    if (textBox3.Text != "" && textBox4.Text != "")
            //        if (comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
            //        {
            //            cmd.CommandText = "DELETE FROM PONATOI WHERE PK_PROTOKOL = '" + pk_protokol + "'";
            //            cmd.ExecuteNonQuery();

            //            string s1 = "", s2 = "", s3 = "", s4 = "";

            //            int i = 0;

            //            cmd.CommandText = "SELECT YLICA,NOMER_DOMA FROM SPRAVOCHNIK_ADRESOVV";
            //            dr = cmd.ExecuteReader();

            //            while (dr.Read())
            //            {
            //                s1 = dr[0].ToString();
            //                s2 = dr[1].ToString();
            //                if (i == comboBox3.SelectedIndex)
            //                    break;
            //            }
            //            cmd.CommandText = "INSERT INTO PONATOI (FIO,PK_ADRES ,PK_PROTOKOL) VALUES ('" + textBox3.Text + "', (SELECT PK_ADRES FROM SPRAVOCHNIK_ADRESOVV WHERE SPRAVOCHNIK_ADRESOVV.YLICA = '" + s1 + "' AND SPRAVOCHNIK_ADRESOVV.NOMER_DOMA = '" + s2.ToString() + "'),'" + pk_protokol + "' )";
            //            cmd.ExecuteNonQuery();

            //            i = 0;

            //            cmd.CommandText = "SELECT YLICA,NOMER_DOMA FROM SPRAVOCHNIK_ADRESOVV";
            //            dr = cmd.ExecuteReader();

            //            while (dr.Read())
            //            {
            //                s3 = dr[0].ToString();
            //                s4 = dr[1].ToString();
            //                if (i == comboBox4.SelectedIndex)
            //                    break;
            //            }
            //            cmd.CommandText = "INSERT INTO PONATOI (FIO,PK_ADRES ,PK_PROTOKOL) VALUES ('" + textBox4.Text + "', (SELECT PK_ADRES FROM SPRAVOCHNIK_ADRESOVV WHERE SPRAVOCHNIK_ADRESOVV.YLICA = '" + s3 + "' AND SPRAVOCHNIK_ADRESOVV.NOMER_DOMA = '" + s4.ToString() + "'),'" + pk_protokol + "' )";
            //            cmd.ExecuteNonQuery();
            //        }
            //}
        }

        void UpDate()
        {
            //con = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + User + ";Password=" + Password + ";");
            //cmd = new OracleCommand("", con);
            //con.Open();

            //UpdateGorod();
            //UpdatePolise();
            //UpdateAdress();
            //UpdatePodoga();
            //UpdateSvet();
            //UpdateSpec();
            //UpdateTexCredstvo();
            //UpdateYpacovka();
            //UpdateSposobYpacovki();
            //UpdateVeshDok();
            //UpdateLica();
        }
        /*
        void UpdateGorod()
        {
            cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_GORODOV";
            dr = cmd.ExecuteReader();

            comboBox1.Items.Clear();

            comboBox1.SelectedIndex = -1;
            comboBox1.Text = null;

            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());
            }
        }

        void UpdateSpec()
        {
            cmd.CommandText = "SELECT SPECIALIST.FIO FROM SPECIALIST";
            dr = cmd.ExecuteReader();

            comboBox7.Items.Clear();

            comboBox7.SelectedIndex = -1;
            comboBox7.Text = null;

            while (dr.Read())
            {
                comboBox7.Items.Add(dr[0].ToString());
            }
        }

        void UpdateSvet()
        {
            cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_OSVESHENNOSTI";
            dr = cmd.ExecuteReader();

            comboBox6.Items.Clear();

            comboBox6.SelectedIndex = -1;
            comboBox6.Text = null;

            while (dr.Read())
            {
                comboBox6.Items.Add(dr[0].ToString());
            }
        }

        void UpdatePodoga()
        {
            cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_POGODI";
            dr = cmd.ExecuteReader();

            comboBox5.Items.Clear();

            comboBox5.SelectedIndex = -1;
            comboBox5.Text = null;

            while (dr.Read())
            {
                comboBox5.Items.Add(dr[0].ToString());
            }
        }

        void UpdateAdress()
        {
            cmd.CommandText = "SELECT YLICA,NOMER_DOMA FROM SPRAVOCHNIK_ADRESOVV";
            dr = cmd.ExecuteReader();

            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;

            comboBox3.Text = null;
            comboBox4.Text = null;

            while (dr.Read())
            {
                comboBox3.Items.Add(dr[0].ToString() + " (" + dr[1].ToString() + ")");
                comboBox4.Items.Add(dr[0].ToString() + " (" + dr[1].ToString() + ")");
            }
        }

        void UpdatePolise()
        {
            cmd.CommandText = "SELECT POLISE.FIO FROM POLISE";
            dr = cmd.ExecuteReader();

            comboBox2.Items.Clear();

            comboBox2.SelectedIndex = -1;
            comboBox2.Text = null;

            while (dr.Read())
            {
                comboBox2.Items.Add(dr[0].ToString());
            }
        }

        void UpdateYpacovka()
        {
            cmd.CommandText = "SELECT MATERIAL FROM SPRAVOCHNIK_MATERIALOV";
            dr = cmd.ExecuteReader();

            this.Column4.Items.Clear();

            while (dr.Read())
            {
                this.Column4.Items.Add(dr[0].ToString());
            }
        }

        void UpdateTexCredstvo()
        {
            cmd.CommandText = "SELECT TEX_SREDSTVO from SPRAVOCHNIK_TEX_SREDSTV";
            dr = cmd.ExecuteReader();

            comboBox8.Items.Clear();

            comboBox8.SelectedIndex = -1;
            comboBox8.Text = null;

            while (dr.Read())
            {
                comboBox8.Items.Add(dr[0].ToString());
            }
        }

        void UpdateSposobYpacovki()
        {
            cmd.CommandText = "SELECT SPOSOB FROM YPAKOVKA";
            dr = cmd.ExecuteReader();

            this.Column8.Items.Clear();

            while (dr.Read())
            {
                this.Column8.Items.Add(dr[0].ToString());
            }
        }

        void UpdateVeshDok()
        {
            

            UpdateSposobYpacovki();
            UpdateYpacovka();
            dataGridView2.Rows.Clear();


            cmd.CommandText = "SELECT VESH_DOK.PRIZNAKI,VESH_DOK.NAIMINOVANIE,SPRAVOCHNIK_MATERIALOV.MATERIAL, YPAKOVKA.SPOSOB FROM VESH_DOK, SPRAVOCHNIK_MATERIALOV,YPAKOVKA WHERE VESH_DOK.PK_PROTOKOL = '" + pk_protokol + "' AND VESH_DOK.PK_MATERIAL = SPRAVOCHNIK_MATERIALOV.PK_MATERIAL AND VESH_DOK.PK_YPAKOVKA = YPAKOVKA.PK_YPAKOVKA";

            dr = cmd.ExecuteReader();
            int i = 0;

            dataGridView2.Enabled = false;

            while (dr.Read())
            {
                dataGridView2.Rows.Add();
                for (int j = 0; j < 4; j++)
                {
                    dataGridView2.Rows[i].Cells[j].Value = dr[j].ToString();
                }
                i++;
            }

            dataGridView2.Enabled = true;
            timer1.Start();
            VeshDokItems = dataGridView2.Rows.Count;
        }

        void UpdateLica()
        {
            dataGridView3.Rows.Clear();


            cmd.CommandText = "SELECT FIO,PRIMICHANIE FROM DRYGIE_LICA WHERE PK_PROTOKOL = '" + pk_protokol + "'";

            dr = cmd.ExecuteReader();
            int i = 0;

            dataGridView3.Enabled = false;

            while (dr.Read())
            {
                dataGridView3.Rows.Add();
                for (int j = 0; j < 2; j++)
                {
                    dataGridView3.Rows[i].Cells[j].Value = dr[j].ToString();
                }
                i++;
            }

            dataGridView3.Enabled = true;
            timer1.Start();
            LicaItems = dataGridView3.Rows.Count;
        }

        void LoadProtokol()
        {

            cmd.CommandText = "SELECT TO_CHAR(DATA_SOSTAV,'DD.MM.YY HH24:MI:SS'),TO_CHAR(VREMYA_NACHALA,'DD.MM.YY HH24:MI:SS'),TO_CHAR(VREMYA_OKONCH,'DD.MM.YY HH24:MI:SS'),MESTO_PEIBITIYA,SOOBSHIL,COOBSHENIE,PREDMET_OSMOTRA,(SELECT SPRAVOCHNIK_POGODI.NAZVANIE FROM SPRAVOCHNIK_POGODI,PROTOKOL WHERE SPRAVOCHNIK_POGODI.PK_POGODA = PROTOKOL.PK_POGODA AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'),(SELECT SPRAVOCHNIK_GORODOV.NAZVANIE FROM SPRAVOCHNIK_GORODOV,PROTOKOL WHERE SPRAVOCHNIK_GORODOV.PK_GOROD = PROTOKOL.PK_GOROD AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'),(SELECT SPRAVOCHNIK_OSVESHENNOSTI.NAZVANIE FROM SPRAVOCHNIK_OSVESHENNOSTI, PROTOKOL WHERE SPRAVOCHNIK_OSVESHENNOSTI.PK_OSVESHENNOST = PROTOKOL.PK_OSVESHENNOST AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'),(SELECT SPRAVOCHNIK_TEX_SREDSTV.TEX_SREDSTVO FROM SPRAVOCHNIK_TEX_SREDSTV, PROTOKOL WHERE SPRAVOCHNIK_TEX_SREDSTV.PK_TEX_SREDSTVO = PROTOKOL.PK_TEX_SREDSTVO AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'),(SELECT SPECIALIST.FIO FROM SPECIALIST , SPRAVOCHNIK_OBLASTEI_SPEC, PROTOKOL WHERE SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL = SPECIALIST.PK_SPECIAL AND SPECIALIST.PK_SPEC = PROTOKOL.PK_SPEC AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'),(SELECT SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE FROM SPECIALIST , SPRAVOCHNIK_OBLASTEI_SPEC, PROTOKOL WHERE SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL = SPECIALIST.PK_SPECIAL AND SPECIALIST.PK_SPEC = PROTOKOL.PK_SPEC AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'),(SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE FROM POLISE,SPRAVOCHNIK_DOLGNOSTNIX_LIC,PROTOKOL WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO = POLISE.PK_DOLGNOSTOE_LICO AND PROTOKOL.PK_POLISE = POLISE.PK_POLISE AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'),(SELECT POLISE.FIO FROM POLISE,SPRAVOCHNIK_DOLGNOSTNIX_LIC,PROTOKOL WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO = POLISE.PK_DOLGNOSTOE_LICO AND PROTOKOL.PK_POLISE = POLISE.PK_POLISE AND PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "') FROM PROTOKOL WHERE PK_PROTOKOL = '" + pk_protokol + "'";
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                if (dr[0].ToString() != "")
                    dateTimePicker1.Value = Convert.ToDateTime(dr[0]);
                if (dr[1].ToString() != "")
                    dateTimePicker2.Value = Convert.ToDateTime(dr[1]);
                if (dr[2].ToString() != "")
                    dateTimePicker3.Value = Convert.ToDateTime(dr[2]);
                textBox2.Text = dr[3].ToString();
                textBox6.Text = dr[4].ToString();
                textBox1.Text = dr[5].ToString();
                textBox5.Text = dr[6].ToString();
                comboBox5.SelectedText = dr[7].ToString();
                comboBox1.SelectedText = dr[8].ToString();
                comboBox6.SelectedText = dr[9].ToString();
                comboBox8.SelectedText = dr[10].ToString();
                comboBox7.SelectedText = dr[11].ToString();
                comboBox2.SelectedText = dr[14].ToString();
                UpdateVeshDok();
                UpdateLica();
            }
            else
                MessageBox.Show("Ой! Что-то не так!");

            cmd.CommandText = "SELECT PONATOI.FIO,SPRAVOCHNIK_ADRESOVV.YLICA,SPRAVOCHNIK_ADRESOVV.NOMER_DOMA  FROM PONATOI,SPRAVOCHNIK_ADRESOVV WHERE PONATOI.PK_ADRES = SPRAVOCHNIK_ADRESOVV.PK_ADRES AND PK_PROTOKOL = '" + pk_protokol + "'";
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                textBox3.Text = dr[0].ToString();
                comboBox3.SelectedText = dr[1].ToString() + " (" + dr[2].ToString() + ")";
            }
            if (dr.Read())
            {
                textBox4.Text = dr[0].ToString();
                comboBox4.SelectedText = dr[1].ToString() + " (" + dr[2].ToString() + ")";
            }
        }
        */

        private void AddProtokol_Load(object sender, EventArgs e)
        {
            Lock = true;

            UpDate();
            this.Text += " " + pk_protokol;
           // LoadProtokol();

            Lock = false;
        }

        private void справочникЗванийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddZvanie(User, Password);
            f.ShowDialog();
        }

        private void справочникСпособовУпаковкиВещДокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddYpak(User, Password);
            f.ShowDialog();

          //  UpdateSposobYpacovki();
        }

        private void справочникТехническихСредствToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddTexSred(User, Password);
            f.ShowDialog();

          //  UpdateTexCredstvo();
        }

        private void справочникОбластейСпециализацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddSpec(User, Password);
            f.ShowDialog();

          //  UpdateSpec();
        }

        private void справочникПогодныхУсловийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddPogoda(User, Password);
            f.ShowDialog();

           // UpdatePodoga();
        }

        private void справочникМатериаловУпаковкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddMaterial(User, Password);
            f.ShowDialog();

           // UpdateYpacovka();
        }

        private void справочникГоробовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddGorod(User, Password);
            f.ShowDialog();

          //  UpdateGorod();
        }

        private void справочникДолжностейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddDolgnost(User, Password);
            f.ShowDialog();
        }

        private void справочникДолжностныхЛицToolStripMenuItem_Click(object sender, EventArgs e)
        {
          //  Form f = new AddDojnost(User, Password);
          //  f.ShowDialog();

          //  UpdatePolise();
        }

        private void справочникТабкльныхНомеровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddSotrudnik(User, Password);
            f.ShowDialog();
        }

        private void справочникОсвещённостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddSvet(User, Password);
            f.ShowDialog();

          //  UpdateSvet();
        }

        private void справочникАдресовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddAdres(User, Password);
            f.ShowDialog();
           // UpdateAdress();
        }

        private void справочникЛицПередавшихВещДокНаХранниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddPeredal(User, Password);
            f.ShowDialog();
        }

        private void справочникХранителейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddSotrud(User, Password);
            f.ShowDialog();
        }

        private void справочникСпециалистовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddSpecMen(User, Password);
            f.ShowDialog();

           // UpdateSpec();
        }

        private void справочникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddOper(User, Password);
            f.ShowDialog();
           // UpdatePolise();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedText.ToString() == "")
                MessageBox.Show("da null on!");
            else
            {
                MessageBox.Show(comboBox1.SelectedText.ToString() + comboBox1.SelectedIndex.ToString());
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //string str0, str1, str2, str3;
            //bool NotNull = true;

            //if (!Lock)
            //{
            //    for (int i = 0; i < 4; i++)
            //        if (dataGridView2.Rows[e.RowIndex].Cells[i].Value == null)
            //            NotNull = false;
            //    if (NotNull)
            //    {
            //        str0 = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();    // признаки
            //        str1 = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();    // наименование
            //        str2 = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();    // упаковка
            //        str3 = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();    // способ

            //        cmd.CommandText = "SELECT * FROM VESH_DOK, PROTOKOL, SPRAVOCHNIK_MATERIALOV,YPAKOVKA WHERE VESH_DOK.PK_PROTOKOL = '" + pk_protokol + "' AND VESH_DOK.NAIMINOVANIE = '" + str1 + "' AND VESH_DOK.PRIZNAKI = '" + str0 + "' AND VESH_DOK.PK_MATERIAL = (SELECT SPRAVOCHNIK_MATERIALOV.PK_MATERIAL FROM SPRAVOCHNIK_MATERIALOV WHERE SPRAVOCHNIK_MATERIALOV.MATERIAL = '" + str2 + "') AND VESH_DOK.PK_YPAKOVKA = (SELECT YPAKOVKA.PK_YPAKOVKA FROM YPAKOVKA WHERE YPAKOVKA.SPOSOB = '" + str3 + "')";
            //        dr = cmd.ExecuteReader();

            //        if (!dr.Read())
            //        {
            //            if (VeshDokItems != dataGridView2.Rows.Count)
            //            {
            //                cmd.CommandText = "INSERT INTO VESH_DOK (PRIZNAKI,NAIMINOVANIE,PK_MATERIAL, PK_YPAKOVKA, PK_PROTOKOL) VALUES ('" + str0 + "', '" + str1 + "',(SELECT SPRAVOCHNIK_MATERIALOV.PK_MATERIAL FROM SPRAVOCHNIK_MATERIALOV WHERE SPRAVOCHNIK_MATERIALOV.MATERIAL = '" + str2 + "'), (SELECT YPAKOVKA.PK_YPAKOVKA FROM YPAKOVKA WHERE YPAKOVKA.SPOSOB = '" + str3 + "'), '" + pk_protokol + "')";
            //                cmd.ExecuteNonQuery();
            //                VeshDokItems = dataGridView2.Rows.Count;
            //            }
            //            else
            //            {
            //                cmd.CommandText = "UPDATE VESH_DOK SET PRIZNAKI = '" + str0 + "', NAIMINOVANIE = '" + str1 + "', PK_MATERIAL = (SELECT SPRAVOCHNIK_MATERIALOV.PK_MATERIAL FROM SPRAVOCHNIK_MATERIALOV WHERE SPRAVOCHNIK_MATERIALOV.MATERIAL = '" + str2 + "'), PK_YPAKOVKA = (SELECT YPAKOVKA.PK_YPAKOVKA FROM YPAKOVKA WHERE YPAKOVKA.SPOSOB = '" + str3 + "') WHERE VESH_DOK.PK_PROTOKOL = '" + pk_protokol + "' AND VESH_DOK.NAIMINOVANIE = '" + oldVDCell0 + "' AND VESH_DOK.PRIZNAKI = '" + oldVDCell1 + "' AND VESH_DOK.PK_MATERIAL = (SELECT SPRAVOCHNIK_MATERIALOV.PK_MATERIAL FROM SPRAVOCHNIK_MATERIALOV WHERE SPRAVOCHNIK_MATERIALOV.MATERIAL = '" + oldVDCell2 + "') AND VESH_DOK.PK_YPAKOVKA = (SELECT YPAKOVKA.PK_YPAKOVKA FROM YPAKOVKA WHERE YPAKOVKA.SPOSOB = '" + oldVDCell3 + "')";
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Данный элемент уже присудствует в таблице!", "Изменение / добавление невозможно!");

            //            Lock = true;
            //            UpdateVeshDok();
            //            Lock = false;
            //        }
            //    }
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Lock = false;
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (!Lock)
            //{
            //    if (e.RowIndex != dataGridView2.Rows.Count - 1)
            //    {
            //        bool NotNull = true;

            //        for (int i = 0; i < 4; i++)
            //            if (dataGridView2.Rows[e.RowIndex].Cells[i].Value == null)
            //                NotNull = false;

            //        if (NotNull)
            //        {
            //            oldVDCell0 = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();    // признаки
            //            oldVDCell1 = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();    // наименование
            //            oldVDCell2 = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();    // упаковка
            //            oldVDCell3 = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();    // способ
            //            button2.Enabled = true;
            //        }
            //    }
            //    else
            //        button2.Enabled = false;
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //button2.Enabled = false;
            //if (MessageBox.Show("Удалть?", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            //{

            //    cmd.CommandText = "DELETE FROM VESH_DOK WHERE VESH_DOK.PK_PROTOKOL = '" + pk_protokol + "' AND VESH_DOK.NAIMINOVANIE = '" + oldVDCell1 + "' AND VESH_DOK.PRIZNAKI = '" + oldVDCell0 + "' AND VESH_DOK.PK_MATERIAL = (SELECT SPRAVOCHNIK_MATERIALOV.PK_MATERIAL FROM SPRAVOCHNIK_MATERIALOV WHERE SPRAVOCHNIK_MATERIALOV.MATERIAL = '" + oldVDCell2 + "') AND VESH_DOK.PK_YPAKOVKA = (SELECT YPAKOVKA.PK_YPAKOVKA FROM YPAKOVKA WHERE YPAKOVKA.SPOSOB = '" + oldVDCell3 + "')";
            //    cmd.ExecuteNonQuery();

            //    Lock = true;
            //    UpdateVeshDok();
            //    Lock = false;
            //}
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //string str0, str1;
            //bool NotNull = true;

            //if (!Lock)
            //{
            //    for (int i = 0; i < 2; i++)
            //        if (dataGridView3.Rows[e.RowIndex].Cells[i].Value == null)
            //            NotNull = false;
            //    if (NotNull)
            //    {
            //        str0 = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();    // фио
            //        str1 = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();    // примечание

            //        cmd.CommandText = "SELECT * FROM DRYGIE_LICA WHERE DRYGIE_LICA.FIO = '" + str0 + "' AND DRYGIE_LICA.PRIMICHANIE = '" + str1 + "' AND DRYGIE_LICA.PK_PROTOKOL = '" + pk_protokol + "'";
            //        dr = cmd.ExecuteReader();

            //        if (!dr.Read())
            //        {
            //            if (LicaItems != dataGridView3.Rows.Count)
            //            {
            //                cmd.CommandText = "INSERT INTO DRYGIE_LICA (FIO, PRIMICHANIE, PK_PROTOKOL) VALUES ('" + str0 + "','" + str1 + "','" + pk_protokol + "')";
            //                cmd.ExecuteNonQuery();
            //                LicaItems = dataGridView3.Rows.Count;
            //            }
            //            else
            //            {
            //                cmd.CommandText = "UPDATE DRYGIE_LICA SET FIO = '" + str0 + "', PRIMICHANIE = '" + str1 + "' WHERE FIO = '" + oldLCell0 + "' AND PRIMICHANIE  = '" + oldLCell1 + "' AND PK_PROTOKOL = '" + pk_protokol + "'";
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Данный элемент уже присудствует в таблице!", "Изменение / добавление невозможно!");

            //            Lock = true;
            //            UpdateLica();
            //            Lock = false;
            //        }
            //    }
            //}
        }

        private void dataGridView3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (!Lock)
            //{
            //    if (e.RowIndex != dataGridView3.Rows.Count - 1)
            //    {
            //        bool NotNull = true;

            //        for (int i = 0; i < 2; i++)
            //            if (dataGridView3.Rows[e.RowIndex].Cells[i].Value == null)
            //                NotNull = false;

            //        if (NotNull)
            //        {
            //            oldLCell0 = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();    // фио
            //            oldLCell1 = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();    // примечание
            //            button3.Enabled = true;
            //        }
            //    }
            //    else
            //        button3.Enabled = false;
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //button3.Enabled = false;
            //if (MessageBox.Show("Удалть?", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            //{

            //    cmd.CommandText = "DELETE FROM DRYGIE_LICA WHERE FIO = '" + oldLCell0 + "' AND PRIMICHANIE  = '" + oldLCell1 + "' AND PK_PROTOKOL = '" + pk_protokol + "'";
            //    cmd.ExecuteNonQuery();

            //    Lock = true;
            //    UpdateLica();
            //    Lock = false;
            //}
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET PK_GOROD = (SELECT SPRAVOCHNIK_GORODOV.PK_GOROD FROM SPRAVOCHNIK_GORODOV WHERE SPRAVOCHNIK_GORODOV.NAZVANIE = '" + comboBox1.SelectedItem.ToString() + "') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET PK_polise = (SELECT PK_polise FROM POLISE WHERE FIO = '" + comboBox2.SelectedItem.ToString() + "') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET COOBSHENIE = '" + textBox1.Text + "' WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET SOOBSHIL = '" + textBox6.Text + "' WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET MESTO_PEIBITIYA = '" + textBox2.Text + "' WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET PREDMET_OSMOTRA = '" + textBox5.Text + "' WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET PK_POGODA = (SELECT PK_POGODA FROM SPRAVOCHNIK_POGODI WHERE NAZVANIE = '" + comboBox5.SelectedItem.ToString() + "') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET PK_OSVESHENNOST = (SELECT PK_OSVESHENNOST FROM SPRAVOCHNIK_OSVESHENNOSTI WHERE NAZVANIE = '" + comboBox6.SelectedItem.ToString() + "') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void comboBox8_SelectedValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET PK_TEX_SREDSTVO = (SELECT PK_TEX_SREDSTVO FROM SPRAVOCHNIK_TEX_SREDSTV WHERE TEX_SREDSTVO = '" + comboBox8.SelectedItem.ToString() + "') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET PK_SPEC = (SELECT PK_SPEC FROM SPECIALIST WHERE FIO = '" + comboBox7.SelectedItem.ToString() + "') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET VREMYA_NACHALA = TO_DATE('" + dateTimePicker2.Value.ToString() + "','DD/MM/YY HH24:MI:SS') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            //cmd.CommandText = "UPDATE PROTOKOL SET VREMYA_OKONCH = TO_DATE('" + dateTimePicker3.Value.ToString() + "','DD/MM/YY HH24:MI:SS') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            //cmd.CommandText = "UPDATE PROTOKOL SET DATA_SOSTAV = TO_DATE('" + dateTimePicker1.Value.ToString() + "','DD/MM/YY HH24:MI:SS') WHERE PROTOKOL.PK_PROTOKOL = '" + pk_protokol + "'";
            //cmd.ExecuteNonQuery();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            AddPonatoi();
        }
        
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            AddPonatoi();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            AddPonatoi();
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            AddPonatoi();
        }
        
    }
}
