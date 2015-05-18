using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql;
using MySql.Data.MySqlClient;

namespace MyDiplomProject
{
    public partial class Form1 : Form
    {


        string index = "";

        string User;
        string Password;
        string Database;
        string Ip;

        public Form1()
        {
            InitializeComponent();
        }

        void UpDatePro()
        {
            //con = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + User + ";Password=" + Password + ";");
            //cmd = new OracleCommand("", con);
            //con.Open();
            ///*
            //cmd.CommandText = "SELECT SPRAVOCHNIK_GORODOV.NAZVANIE ,TO_CHAR(PROTOKOL.DATA_SOSTAV,'DD.MM.YY'),PROTOKOL.VREMYA_NACHALA,PROTOKOL.VREMYA_OKONCH FROM PROTOKOL,SPRAVOCHNIK_GORODOV WHERE SPRAVOCHNIK_GORODOV.PK_GOROD(+) = PROTOKOL.PK_GOROD";

            //dr = cmd.ExecuteReader();
            //int i = 0;

            //listBox1.Items.Clear();

            //while (dr.Read())
            //{
            //    listBox1.Items.Add(dr[0].ToString() + ", " + dr[1].ToString() + " ( " + dr[2].ToString() + " - " + dr[3].ToString() + " )");
            //    i++;
            //}*/
            //cmd.CommandText = "SELECT PK_PROTOKOL FROM PROTOKOL";

            //dr = cmd.ExecuteReader();
            //int i = 0;

            //listBox1.Items.Clear();

            //while (dr.Read())
            //{
            //    listBox1.Items.Add(dr[0].ToString());
            //    i++;
            //}

            //button2.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string pk;

            //cmd.CommandText = "INSERT INTO PROTOKOL (DATA_SOSTAV) VALUES (NULL)";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "SELECT MAX(PK_PROTOKOL) FROM PROTOKOL";
            //dr = cmd.ExecuteReader();

            //if (dr.Read())
            //{
            //    pk = dr[0].ToString();
            //    Form f = new AddProtokol(User, Password, true, pk);
            //    f.ShowDialog();
            //    UpDatePro();
            //}
            //else
            //    MessageBox.Show("Ой! Что - то не так! 0(О_О)0");

        }

        private void button3_Click(object sender, EventArgs e)
        {
           /* Form f = new AddAct(User, Password, );
            f.ShowDialog();*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            User = "root";
            Password = "ezembi007";
            Database = "polise";
            Ip = "127.0.0.1";
            UpDatePro();
        }

        private void справочникЗванийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник званий", "spravochnik_zvanii", new string[] { "Звание", "Идентификационный номер" }, new string[] { "pk_zvanie", "nazvanie", "id_number" });
            f.ShowDialog();
        }

        private void справочникСпособовУпаковкиВещДокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник способов упаковки вещественных доказательств", "ypakovka", new string[] { "Способ" }, new string[] { "pk_ypakovka", "sposob" });
            f.ShowDialog();
        }

        private void справочникТехническихСредствToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddTexSred(User, Password);
            f.ShowDialog();
        }

        private void справочникОбластейСпециализацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник областей специализации", "spravochnik_oblastei_spec", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_special", "nazvanie", "id_number" });
            f.ShowDialog();
        }

        private void справочникПогодныхУсловийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddPogoda(User, Password);
            f.ShowDialog();
        }

        private void справочникМатериаловУпаковкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddMaterial(User, Password);
            f.ShowDialog();
            
        }

        private void справочникГоробовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" });
            f.ShowDialog();
        }

        private void справочникДолжностейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddDolgnost(User, Password);
            f.ShowDialog();
        }

        private void справочникДолжностныхЛицToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form f = new AddDojnost(User, Password, Database, Ip, "Справочник должностей", "spravochnik_dolgnostei", new string[] { "Должность", "Идентификационный номер" }, new string[] { "pk_dolgnost", "nazvanie", "id_number" });
            //f.ShowDialog();
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
        }

        private void справочникАдресовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddAdres(User, Password);
            f.ShowDialog();
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
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, false, "Справочник специалистов", "specialist", new string[] { "Фамилия", "Имя", "Отчество", "Область специализации" }, new string[] { "pk_spec", "surname", "Pname", "second_name", "pk_special" }, "spravochnik_oblastei_spec", new string[] { "pk_special", "nazvanie" });
            f.ShowDialog();
        }

        private void справочникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new AddOper(User, Password);
            f.ShowDialog();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Form f = new AddProtokol(User, Password, true, listBox1.SelectedItem.ToString());
            f.ShowDialog();
            UpDatePro();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //button2.Enabled = false;

            //if (MessageBox.Show("Вы уверены, что хотите удалить протокл?\nВсе вещественные доказательства описанные в данном протоколе также будут удалены!\nДанное действие нельзя отменить!", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            //{
            //    cmd.CommandText = "DELETE FROM DRYGIE_LICA WHERE PK_PROTOKOL = '" + index + "'";
            //    cmd.ExecuteNonQuery();

            //    cmd.CommandText = "DELETE FROM PONATOI WHERE PK_PROTOKOL = '" + index + "'";
            //    cmd.ExecuteNonQuery();

            //    cmd.CommandText = "DELETE FROM VESH_DOK WHERE PK_PROTOKOL = '" + index + "'";
            //    cmd.ExecuteNonQuery();

            //    cmd.CommandText = "DELETE FROM PROTOKOL WHERE PK_PROTOKOL = '" + index + "'";
            //    cmd.ExecuteNonQuery();
            //    UpDatePro();
            //}

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            button2.Enabled = true;
            index = listBox1.SelectedItem.ToString();
        }

        private void справочникПодразделенийСледственногоКомитетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, false, "Справочник подразделений следственного комитета", "spravochnik_pod", new string[] { "Название", "Идентификационный номер", "Район", "Город" }, new string[] { "PK_Raiona", "Nazv", "id_number", "Raion", "pk_gorod" }, "spravochnik_gorodov", new string[] { "pk_gorod", "nazvanie" });
            f.ShowDialog();
        }
    }
}
