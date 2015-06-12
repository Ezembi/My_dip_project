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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string _user, string _pass, string _database, string _ip)
        {
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            InitializeComponent();
        }

        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Добавление протокола осмотра места происшествия. В протоколе описываются все действия следователя (дознавателя), а также всё обнаруженное при осмотре в той последовательности, в какой производились осмотр, и в том виде, в каком обнаруженное наблюдалось в момент осмотра. В протоколе перечисляются и описываются все предметы, изъятые при осмотре (Статья 180 УПК РФ).";
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Протокол осмотра трупа. Протокол составляется аналогично протоколу осмотра места происшествия, за исключением того, что в протоколе осмотра трупа обязательно в качестве специалиста указывается судебно — медицинский эксперт или врач, а также указывается куда труп будет направлен после осмотра (Статья 178 УПК РФ).";
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Протокол личного обыска. Протокол составляется, при личном досмотре (обыске) подозреваемого при наличие достаточных оснований полагать, что у подозреваемого  находятся предметы, документы, ценности, имеющие значение для уголовного дела (Статья 184 УПК РФ). Также при добавлении протокола необходимо будет добавить постановление о производстве личного обыска подозреваемого (обвиняемого) в случаях, не терпящих отлагательств";
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Протокол осмотра местности, жилища, иного помещения. Протокол составляется при производстве осмотра, в целях обнаружения следов преступления, выяснения других обстоятельств, имеющих значение для уголовного дела (Статья 176 УПК РФ). Также при добавлении протокола необходимо будет добавить постановление о производстве осмотра жилища в случаях, не терпящих отлагательств";
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Протокол обыска (выемки). Протокол составляется при производстве обыска при наличие достаточных оснований полагать, что в каком-либо месте или у какого-либо лица могут находиться орудия, оборудования или иные средства совершения преступления, предметы, документы и ценности, которые могут иметь значение для уголовного дела (Статья 182,183 УПК РФ). Также при добавлении протокола необходимо будет добавить постановление о производстве обыска (выемки) в случаях, не терпящих отлагательств";
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Добавление уголоного дела, материала проверки.";
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Просмотр / редактирование / удаление уголоных дел, материалов проверки. Приктипление протокола к определённому уголоному делу, материалу проверки.";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form1 f = new Form1(User, Password, Database, Ip, "Материалы проверки / уголовные дела", "delo", new string[] { "Номер материала", "Дата поступления материала", "Номер дела", "Дата возбуждения дела", "Уполномоченный", "Подразделение" }, new string[] { "PK_Dela", "Nomer_materiala", "DateofM", "Nomer_dela", "DateofV", "pk_polise", "PK_Raiona" }, "polise", new string[] { "pk_polise", "surname", "Pname", "second_name" }, "spravochnik_pod", new string[] { "PK_Raiona", "Nazv" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            CriminalAffair f = new CriminalAffair(User, Password, Database, Ip, "");
            f.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Protocol f = new Protocol(User, Password, Database, Ip, "", "1", "", "");
            f.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Protocol f = new Protocol(User, Password, Database, Ip, "", "2", "", "");
            f.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Resolution r;
            r = new Resolution(User, Password, Database, Ip, "", "2", "", "");
            r.ShowDialog();
            if (r.pk_postanov != "")
            {
                Protocol p = new Protocol(User, Password, Database, Ip, "", "3", r.pk_postanov, "");
                p.ShowDialog();
            }
            this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Resolution r;
            r = new Resolution(User, Password, Database, Ip, "", "5", "", "");
            r.ShowDialog();
            if (r.pk_postanov != "")
            {
                Protocol p = new Protocol(User, Password, Database, Ip, "", "5", r.pk_postanov, "");
                p.ShowDialog();
            }
            this.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Resolution r;
            r = new Resolution(User, Password, Database, Ip, "", "1", "", "");
            r.ShowDialog();
            if (r.pk_postanov != "")
            {
                Protocol p = new Protocol(User, Password, Database, Ip, "", "4", r.pk_postanov, "");
                p.ShowDialog();
            }
            this.Visible = true;
        }

        private void button8_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Добавление / просмотр / редактирование / удаление информации в справочниках.";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DirectoriesForm df = new DirectoriesForm(User,Password,Database,Ip);
            df.ShowDialog();
            this.Visible = true;
        }

        private void button9_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "Добавление / просмотр / редактирование / удаление информации в протоколах.";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            SelectProtocol sp = new SelectProtocol(User, Password, Database, Ip, "", false);
            sp.ShowDialog();
            this.Visible = true;
        }
    }
}
