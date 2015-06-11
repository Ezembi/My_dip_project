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
    public partial class DirectoriesForm : Form
    {

        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера

        public DirectoriesForm()
        {
            InitializeComponent();
        }

        public DirectoriesForm(string _user, string _pass, string _database, string _ip)
        {
            InitializeComponent();
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
        }

        #region Код для вызова любого из справочника

        /// <summary>
        /// Описание всех пунков menuStrip1
        /// </summary>

        private void справочникЗванийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник званий", "spravochnik_zvanii", new string[] { "Звание", "Идентификационный номер" }, new string[] { "pk_zvanie", "nazvanie", "id_number" });
            f.ShowDialog(); 
            this.Visible = true;
        }

        private void справочникСпособовУпаковкиВещДокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник способов упаковки вещественных доказательств", "ypakovka", new string[] { "Способ" }, new string[] { "pk_ypakovka", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникТехническихСредствToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник технических средств", "spravochnik_tex_sredstv", new string[] { "Техническое средство", "Идентификационный номер" }, new string[] { "pk_tex_sredstvo", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникОбластейСпециализацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник областей специализации", "spravochnik_oblastei_spec", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_special", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникПогодныхУсловийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник погодных условий", "spravochnik_pogodi", new string[] { "Погода" }, new string[] { "pk_pogoda", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникМатериаловУпаковкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник материалов упаковки", "spravochnik_materialov", new string[] { "Маериал", "Идентификационный номер" }, new string[] { "pk_material", "material", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникГоробовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникДолжностейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник должностей", "spravochnik_dolgnostei", new string[] { "Должность", "Идентификационный номер" }, new string[] { "pk_dolgnost", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникОсвещённостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник освещённости", "spravochnik_osveshennosti", new string[] { "Освещённость" }, new string[] { "pk_osveshennost", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникСпециалистовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, false, "Справочник специалистов", "specialist", new string[] { "Фамилия", "Имя", "Отчество", "Область специализации" }, new string[] { "pk_spec", "surname", "Pname", "second_name", "pk_special" }, "spravochnik_oblastei_spec", new string[] { "pk_special", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddOper f = new AddOper(User, Password, Database, Ip, false, "Справочник уполномоченных", "polise", new string[] { "Табельный номер", "Фамилия", "Имя", "Отчество", "Звание", "Должность", "Чин" }, new string[] { "pk_polise", "id_number", "surname", "Pname", "second_name", "pk_zvanie", "pk_dolgnost", "pk_chin" }, "spravochnik_zvanii", new string[] { "pk_zvanie", "nazvanie" }, "spravochnik_dolgnostei", new string[] { "pk_dolgnost", "nazvanie" }, "chin", new string[] { "pk_chin", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникПодразделенийСледственногоКомитетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, false, "Справочник подразделений следственного комитета", "spravochnik_pod", new string[] { "Название", "Район", "Идентификационный номер", "Город" }, new string[] { "PK_Raiona", "Nazv", "Raion", "id_number", "pk_gorod" }, "spravochnik_gorodov", new string[] { "pk_gorod", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникКлассныхЧиновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник классных чинов", "chin", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_chin", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникПроцессуальныхПоложенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник ппроцессуальных положений", "sp_pro_pol", new string[] { "Наименование положения", "Идентификационный номер" }, new string[] { "pk_pol", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        #endregion

    }
}
