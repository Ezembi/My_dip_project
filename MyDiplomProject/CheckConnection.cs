using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql;
using MySql.Data.MySqlClient;


namespace MyDiplomProject
{
    public partial class CheckConnection : Form
    {
        public CheckConnection()
        {
            InitializeComponent();
        }

        public MySqlConnection mycon;   // коннектор б/д
        public MySqlCommand cmd;        // sql комманды для работы с б/д

        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                User = textBox3.Text;        // имя пользователя, для доступа к б/д
                Password = textBox4.Text;    // пароль пользователя
                Database = textBox1.Text;    // название б/д
                Ip = textBox2.Text;

                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;Convert Zero Datetime=True;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();

                if (mycon.State != ConnectionState.Open)
                {
                    MessageBox.Show("Нет подключениея к базе данных!");
                    throw new Exception();
                }

                MessageBox.Show("Успешное подключение к базе данных!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                checkBox1.Enabled = button2.Enabled = true;
                button1.Enabled = false;

            }
            catch
            {
                MessageBox.Show("Нет доступа к базе данных!\nПроверьпе параметры подключения и повторите попытку.", "Ошибка подключения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                try
                {
                    using (StreamWriter r = File.CreateText("connection.cfg"))
                    {
                        r.WriteLine(User);
                        r.WriteLine(Password);
                        r.WriteLine(Database);
                        r.WriteLine(Ip);
                        r.Close();
                    }
                }
                catch {
                    MessageBox.Show("Не удалось сохранить параметры подключения!\nВозможно файл защищён от записи, либо у Вас нет прав на запись файла.", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }

            this.Visible = false;
            Form1 f = new Form1(User, Password, Database, Ip, "Материалы проверки / уголовные дела", "delo", new string[] { "Номер материала", "Дата поступления материала", "Номер дела", "Дата возбуждения дела", "Уполномоченный", "Подразделение" }, new string[] { "PK_Dela", "Nomer_materiala", "DateofM", "Nomer_dela", "DateofV", "pk_polise", "PK_Raiona" }, "polise", new string[] { "pk_polise", "surname", "Pname", "second_name" }, "spravochnik_pod", new string[] { "PK_Raiona", "Nazv" });
            f.ShowDialog();
            this.Close();
        }

        private void CheckConnection_Load(object sender, EventArgs e)
        {
            using (StreamReader r = File.OpenText("connection.cfg"))
            {
                User = r.ReadLine();
                Password = r.ReadLine();
                Database = r.ReadLine();
                Ip = r.ReadLine();
                r.Close();
            }

            try
            {
                textBox3.Text = User;        // имя пользователя, для доступа к б/д
                textBox4.Text = Password;    // пароль пользователя
                textBox1.Text = Database;    // название б/д
                textBox2.Text = Ip;

                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;Convert Zero Datetime=True;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();

                if (mycon.State != ConnectionState.Open)
                {
                    MessageBox.Show("Нет подключениея к базе данных!");
                    throw new Exception();
                }

                this.Visible = false;
                Form1 f = new Form1(User, Password, Database, Ip, "Материалы проверки / уголовные дела", "delo", new string[] { "Номер материала", "Дата поступления материала", "Номер дела", "Дата возбуждения дела", "Уполномоченный", "Подразделение" }, new string[] { "PK_Dela", "Nomer_materiala", "DateofM", "Nomer_dela", "DateofV", "pk_polise", "PK_Raiona" }, "polise", new string[] { "pk_polise", "surname", "Pname", "second_name" }, "spravochnik_pod", new string[] { "PK_Raiona", "Nazv" });
                f.ShowDialog();
                this.Close();

            }
            catch
            {
                MessageBox.Show("Нет доступа к базе данных!\nПроверьпе параметры подключения и повторите попытку.", "Ошибка подключения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.textBox2, "Обычно адрес локального сервера: 127.0.0.1");
        }
    }
}
