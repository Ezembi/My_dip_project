using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql;


namespace MyDiplomProject
{
    public partial class SelectProtocol : Form
    {
        public MySqlConnection mycon;   // коннектор б/д
        public MySqlCommand cmd;        // sql комманды для работы с б/д

        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера

        bool Lock = true;   // блокировка действий пользователя до полной загрузки формы
        string PK_Dela;     // первичный ключ уголовного дела / материала проверки для выборки
        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации (уполномоченный)
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации
        string STable3;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (протокол)
        string[] DBSHeader3; // название полей в таблице2 для sql запросов, для сопудствующей информации
        int count = 0;      // количество записей в таблице БД
        bool isNull;        // для отображения столбца Выбрать, для прикрепление протокола к делу
        int lastIndex = 0;  // для корректного постраничного отображения

        public SelectProtocol()
        {
            InitializeComponent();
        }

        public SelectProtocol(string _user, string _pass, string _database, string _ip, string _PK_Dela, bool _isNull)
        {
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            isNull = _isNull;
            STable1 = "polise";
            DBSHeader1 = new string[] { "pk_polise", "surname", "Pname", "second_name" };
            STable3 = "protokol";
            DBSHeader3 = new string[] { "pk_protokol", "Number", "data_sostav", "pk_polise" };

            PK_Dela = _PK_Dela;
            InitializeComponent();
        }

        private void SelectProtocol_Load(object sender, EventArgs e)
        {
            try
            {
                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;Convert Zero Datetime=True;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();
                comboBox2.SelectedIndex = 0;

                if (mycon.State != ConnectionState.Open)
                {
                    MessageBox.Show("Нет подключениея к базе данных!");
                    throw new Exception();
                }

            }
            catch
            {
                MessageBox.Show("Error:1\nНет доступа к базе данных!", "Ошибка подключения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTable()//загрузка информации в таблицу
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;

            //проверка подключения к бд
            try
            {
                dataGridView1.Enabled = false;
                Lock = true;
                if (mycon.State == ConnectionState.Open)
                {
                    //генерация sql запроса, для подсчёта кол-ва элементов в БД
                    string sql;
                    sql = "";
                    sql += "select * from " + STable3 + " where ";

                    if(isNull)
                        sql += "PK_Dela is null";

                    if(textBox2.Text != "")
                        if (isNull)
                            sql += " and pk_polise = " + textBox2.Text;
                        else
                            sql += " pk_polise = " + textBox2.Text + " and ";

                    if (isNull)
                        sql += " and ";

                    sql += " mesto_peibitiya like '%" + textBox3.Text + "%' ";

                    if(comboBox1.SelectedIndex != -1)
                        sql += " and id_prot = " + (comboBox1.SelectedIndex + 1).ToString();


                    cmd = new MySqlCommand(sql, mycon);

                    //вополнение запроса
                    cmd.ExecuteNonQuery();

                    //выборка по запросу
                    da = new MySqlDataAdapter(cmd);
                    dr = cmd.ExecuteReader();

                    // подсчёт количества записей в таблице
                    count = 0;
                    while (dr.Read()) count++;
                    dr.Close();


                    //генерация sql запроса, для отображения данных из БД на форму
                    sql = "";
                    sql += "select * from " + STable3 + " where ";

                    if (isNull)
                        sql += "PK_Dela is null";

                    if (textBox2.Text != "")
                        if (isNull)
                            sql += " and pk_polise = " + textBox2.Text;
                        else
                            sql += " pk_polise = " + textBox2.Text + " and ";

                    if (isNull)
                        sql += " and ";

                    sql += " mesto_peibitiya like '%" + textBox3.Text + "%' ";

                    if (comboBox1.SelectedIndex != -1)
                        sql += " and id_prot = " + (comboBox1.SelectedIndex + 1).ToString() + " ";

                     sql += "limit " + lastIndex.ToString() + ", " + comboBox2.SelectedItem.ToString();

                    cmd = new MySqlCommand(sql, mycon);

                    //вополнение запроса
                    cmd.ExecuteNonQuery();

                    //выборка по запросу
                    da = new MySqlDataAdapter(cmd);
                    dr = cmd.ExecuteReader();


                    toolStripProgressBar1.Visible = true;
                    toolStripProgressBar1.Value = 0;
                    toolStripStatusLabel1.Text = "Записей в базе: " + count;

                    //запролене dataGridView1 "pk_protokol", "Number", "data_sostav", "pk_polise"
                    int i = 0;
                    dataGridView1.Rows.Clear();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();    // PC
                        dataGridView1.Rows[i].Cells[1].Value = Convert.ToDateTime(dr[1].ToString()).ToShortDateString();    // дата составления

                        switch (dr[7].ToString())
                        {
                            case "1":
                                {
                                    dataGridView1.Rows[i].Cells[2].Value = "Протокол осмотра мета происшествия";
                                } break;
                            case "2":
                                {
                                    dataGridView1.Rows[i].Cells[2].Value = "Протокол осмотра трупа";
                                } break;
                            case "3":
                                {
                                    dataGridView1.Rows[i].Cells[2].Value = "Протокол личного обыска";
                                } break;
                            case "4":
                                {
                                    dataGridView1.Rows[i].Cells[2].Value = "Протокол осмотра местности, жилища, иного помещения";
                                } break;
                            case "5":
                                {
                                    dataGridView1.Rows[i].Cells[2].Value = "Протокол обыска (выемки)";
                                } break;
                        }

                        dataGridView1.Rows[i].Cells[3].Value = dr[20].ToString();    // pk_polise (уполном)

                        dataGridView1.Rows[i].Cells[5].Value = dr[4].ToString();           // место прибытия
                        dataGridView1.Rows[i].Cells[6].Value = "Выбрать";           // Выбор
                        dataGridView1.Rows[i].Cells[7].Value = "Открыть";           // Выбор

                        dataGridView1.Rows[i].Cells[8].Value = dr[21].ToString();    // pk_postanov (постановление)
                        dataGridView1.Rows[i].Cells[9].Value = dr[7].ToString();    // тип ротокола

                        i++;
                        toolStripProgressBar1.Value = (i * 100) / count;
                    }
                    dr.Close();

                    // уполном
                    for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
                    {
                        if (dataGridView1.Rows[ii].Cells[3].Value.ToString() != "" && dataGridView1.Rows[ii].Cells[3].Value != null)
                        {
                            sql = "select ";
                            for (int j = 1; j < DBSHeader1.Length; j++)
                            {
                                if (j == 1)
                                    sql += DBSHeader1[j];
                                else
                                    sql += ", " + DBSHeader1[j];
                            }
                            // генерация sql комманды
                            sql += " from " + STable1 + " where " + DBSHeader1[0] + " = " + dataGridView1.Rows[ii].Cells[3].Value.ToString();

                            //получение комманды и коннекта
                            cmd = new MySqlCommand(sql, mycon);

                            //вополнение запроса
                            cmd.ExecuteNonQuery();
                            da = new MySqlDataAdapter(cmd);

                            //получение выборки
                            dr = cmd.ExecuteReader();

                            // заполнения поля 
                            if (dr.Read())
                                dataGridView1.Rows[ii].Cells[4].Value = dr[0].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString();
                            dr.Close();
                        }
                    }

                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];     //делаем последнюю ячейку активной
                    toolStripProgressBar1.Value = 100;

                    toolStripProgressBar1.Visible = false;                          // убераем прогресс бар

                    dataGridView1.Enabled = true;                                   // делаем таблицу доступной
                }
                else
                    MessageBox.Show("Нет подключениея к базе данных!");
                Lock = false;
                dataGridView1.Enabled = true;
                CheackButton();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                MessageBox.Show("Error:3\nОшибка при загрузке данных в таблицу!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                string sql = "";

                //редактирование

                sql = "";
                sql = "update " + STable3 + " set ";

                sql += "PK_Dela = '" + PK_Dela + "'";


                sql += " where pk_protokol = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                // внесение информации в БД
                cmd = new MySqlCommand(sql, mycon);
                cmd.ExecuteNonQuery();
                LoadTable();
            }

            if (e.ColumnIndex == 7 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                this.Visible = false;
                Protocol f = new Protocol(User, Password, Database, Ip, PK_Dela, dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                f.ShowDialog();
                this.Visible = true;
            }
            LoadTable();
        }

        private void SelectProtocol_Shown(object sender, EventArgs e)
        {
            if (isNull)
                this.Column5.Visible = true;
            LoadTable();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Text = textBox2.Text = textBox3.Text = "";
            LoadTable();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //уполномоченный
            AddOper f = new AddOper(User, Password, Database, Ip, true, "Справочник уполномоченных", "polise", new string[] { "Табельный номер", "Фамилия", "Имя", "Отчество", "Звание", "Должность", "Чин" }, new string[] { "pk_polise", "id_number", "surname", "Pname", "second_name", "pk_zvanie", "pk_dolgnost", "pk_chin" }, "spravochnik_zvanii", new string[] { "pk_zvanie", "nazvanie" }, "spravochnik_dolgnostei", new string[] { "pk_dolgnost", "nazvanie" }, "chin", new string[] { "pk_chin", "nazvanie" });
            f.ShowDialog();

            string PC_rezult = "";  //значение первичного ключа
            string Rezult = "";     // соответствующее текстовое значение

            PC_rezult = f.PC_rezult;
            Rezult = f.Rezult;

            if (PC_rezult != null)
            {
                textBox1.Text = Rezult;
                textBox2.Text = PC_rezult;
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
            LoadTable();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lastIndex += Convert.ToInt32(comboBox2.SelectedItem);
            if (count - lastIndex < 0)
                lastIndex = count - Convert.ToInt32(comboBox2.SelectedItem);
            LoadTable();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lastIndex = count - Convert.ToInt32(comboBox2.SelectedItem);
            LoadTable();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lastIndex -= Convert.ToInt32(comboBox2.SelectedItem);
            if (lastIndex <= 0)
                lastIndex = 0;

            LoadTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lastIndex = 0;
            LoadTable();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheackButton();
            LoadTable();
        }

        public void CheackButton()
        {
            if (lastIndex <= 0)
                button3.Enabled = button4.Enabled = false;
            else
                button3.Enabled = button4.Enabled = true;

            if (count - (lastIndex + Convert.ToInt32(comboBox2.SelectedItem)) <= 0)
                button5.Enabled = button6.Enabled = false;
            else
                button5.Enabled = button6.Enabled = true;
        }
        private void button5_MouseEnter(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.button5, "Следующая страница");
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.button6, "Последняя страница");
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.button4, "Предидущая страница");
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.button3, "Первая страница");
        }
    }
}
