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
    public partial class CriminalAffair : Form
    {
        public MySqlConnection mycon;   // коннектор б/д
        public MySqlCommand cmd;        // sql комманды для работы с б/д

        int items;          // количество записей в таблице на форме
        bool Lock = true;   // блокировка действий пользователя до полной загрузки формы
        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера
        int lastIndex = 0;  // для корректного постраничного отображения
        int count = 0;      // количество записей в таблице БД
        int rSize = 1;      // иправление бага WS
        string table;       // ТАБЛИЦА БАЗЫ ДАННЫХ
        string[] HeaderText;// название полей в таблице для отображения
        string[] DBHeader;  // название полей в таблице для sql запросов
        string PK_Dela;     // первичный ключ уголовного дела / материала проверки для выборки

        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader2; // название полей в таблице2 для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader3; // название полей в таблице2 для sql запросов, для сопудствующей информации

        bool change = false;

        public CriminalAffair()
        {
            InitializeComponent();
        }

        public CriminalAffair(string _user, string _pass, string _database, string _ip, string _PK_Dela)
        {
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            table = "delo";
            DBHeader = new string[] {"Nomer_materiala", "DateofM", "Nomer_dela", "DateofV","DateofPeredachi", "DateofClose", "Comment", "pk_polise", "PK_Raiona" };
            STable1 = "polise";
            DBSHeader1 = new string[] { "pk_polise", "surname", "Pname", "second_name" };
            STable2 = "spravochnik_pod";
            DBSHeader2 = new string[] { "PK_Raiona", "Nazv" };
            STable3 = "protokol";
            DBSHeader3 = new string[] { "pk_protokol", "Number", "data_sostav", "pk_polise"};
            
            PK_Dela = _PK_Dela;
            InitializeComponent();
        }

        private void CriminalAffair_Load(object sender, EventArgs e)
        {
            try
            {
                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;Convert Zero Datetime=True;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();
                if (PK_Dela == "")
                    this.Text = "Уголовное дело / материал проверки: Новый документ";
                else
                    this.Text = "Уголовное дело / материал проверки: " + PK_Dela;
                
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

        private void LoadData() //загруза основной информации в на форму
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
                    if (PK_Dela != "")
                    {
                        string sql;
                        //генерация sql запроса, для отображения данных из БД на форму
                        sql = "select * from " + table + " where PK_Dela = " + PK_Dela;
                        cmd = new MySqlCommand(sql, mycon);

                        //вополнение запроса
                        cmd.ExecuteNonQuery();

                        //выборка по запросу
                        da = new MySqlDataAdapter(cmd);
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            // материал
                            textBox1.Text = dr[1].ToString();
                            dateTimePicker1.Value = Convert.ToDateTime(dr[2].ToString());

                            if (dr[4].ToString() != "01.01.0001 0:00:00")
                            {
                                checkBox1.Checked = true;
                                // дело
                                textBox2.Text = dr[3].ToString();
                                dateTimePicker2.Value = Convert.ToDateTime(dr[4].ToString());

                                if (dr[5].ToString() != "01.01.0001 0:00:00")
                                {
                                    checkBox2.Checked = true;
                                    //суд
                                    dateTimePicker3.Value = Convert.ToDateTime(dr[5].ToString());
                                }
                                else
                                    checkBox2.Checked = false;
                            }
                            else
                                checkBox1.Checked = false;

                            if (dr[6].ToString() != "01.01.0001 0:00:00")
                            {
                                checkBox3.Checked = true;
                                //закрытие
                                dateTimePicker4.Value = Convert.ToDateTime(dr[6].ToString());
                            }
                            else
                                checkBox3.Checked = false;
                            


                            //coment
                            textBox7.Text = dr[7].ToString();

                            //уполномоченный
                            textBox4.Text = dr[8].ToString();

                            //подразделение
                            textBox6.Text = dr[9].ToString();
                            dr.Close();

                            // уполном
                            sql = "select ";
                            for (int j = 1; j < DBSHeader1.Length; j++)
                            {
                                if (j == 1)
                                    sql += DBSHeader1[j];
                                else
                                    sql += ", " + DBSHeader1[j];
                            }
                            // генерация sql комманды
                            sql += " from " + STable1 + " where " + DBSHeader1[0] + " = " + textBox4.Text;

                            //получение комманды и коннекта
                            cmd = new MySqlCommand(sql, mycon);

                            //вополнение запроса
                            cmd.ExecuteNonQuery();
                            da = new MySqlDataAdapter(cmd);

                            //получение выборки
                            dr = cmd.ExecuteReader();

                            // заполнения поля 
                            if (dr.Read())
                                textBox3.Text = dr[0].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString();
                            dr.Close();


                            // подразделение следственного комитета
                            sql = "select ";
                            for (int j = 1; j < DBSHeader2.Length; j++)
                            {
                                if (j == 1)
                                    sql += DBSHeader2[j];
                                else
                                    sql += ", " + DBSHeader2[j];
                            }
                            // генерация sql комманды
                            sql += " from " + STable2 + " where " + DBSHeader2[0] + " = " + textBox6.Text;
                            //получение комманды и коннекта
                            cmd = new MySqlCommand(sql, mycon);
                            //вополнение запроса
                            cmd.ExecuteNonQuery();
                            da = new MySqlDataAdapter(cmd);
                            //получение выборки
                            dr = cmd.ExecuteReader();
                            // заполнения поля
                            if (dr.Read())
                                textBox5.Text = dr[0].ToString();
                            dr.Close();

                            LoadTable();

                        }
                        else
                            throw new Exception();

                        toolStripProgressBar1.Visible = false;                          // убераем прогресс бар

                        // иправление бага WS
                        rSize = (rSize > 0) ? -1 : 1;
                        this.ClientSize = new System.Drawing.Size(this.ClientSize.Width + rSize, this.ClientSize.Height);
                        
                    }

                    Lock = false;
                    dataGridView1.Enabled = true;
                    CheakChecked();
                    FileSave();
                }
                else
                    MessageBox.Show("Нет подключениея к базе данных!");
            }
            catch
            {
                MessageBox.Show("Error:2\nОшибка при загрузке данных!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    sql += "select * from " + STable3 + " where PK_Dela = " + PK_Dela;

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
                    sql += "select * from " + STable3 + " where PK_Dela = " + PK_Dela;

                   // sql += "limit " + lastIndex.ToString() + ", " + comboBox1.SelectedItem.ToString();

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
                        dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();    // номер материала
                        dataGridView1.Rows[i].Cells[2].Value = Convert.ToDateTime(dr[2].ToString()).ToShortDateString();    // дата составления
                        dataGridView1.Rows[i].Cells[3].Value = dr[3].ToString();    // pk_polise (уполном)
                        dataGridView1.Rows[i].Cells[4].Value = dr[4].ToString();    // Уполномоченный

                        dataGridView1.Rows[i].Cells[5].Value = "Удалить";           // Удаление
                        dataGridView1.Rows[i].Cells[6].Value = "Открыть";           // Выбор

                        i++;
                        toolStripProgressBar1.Value = (i * 100) / count;
                    }
                    dr.Close();

                    // уполном
                    for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
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

                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];     //делаем последнюю ячейку активной
                    toolStripProgressBar1.Value = 100;

                    toolStripProgressBar1.Visible = false;                          // убераем прогресс бар

                    dataGridView1.Enabled = true;                                   // делаем таблицу доступной
                    items = dataGridView1.Rows.Count;
                }
                else
                    MessageBox.Show("Нет подключениея к базе данных!");
                Lock = false;
                dataGridView1.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Error:3\nОшибка при загрузке данных в таблицу!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void CriminalAffair_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 3 && e.KeyChar != 22 && e.KeyChar != 26)
                e.Handled = true;
        }

        private void CriminalAffair_Shown(object sender, EventArgs e)
        {
            //загрузка информации
            LoadData();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheakChecked();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheakChecked();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheakChecked();
        }

        private void CheakChecked()
        {
            textBox2.Enabled = dateTimePicker2.Enabled = checkBox1.Checked;
            if (checkBox1.Checked)
                dateTimePicker3.Enabled = checkBox2.Checked;
            else
                dateTimePicker3.Enabled = checkBox2.Enabled = checkBox1.Checked;

            dateTimePicker4.Enabled = checkBox3.Checked;
            FileChange();
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
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
                textBox3.Text = Rezult;
                textBox4.Text = PC_rezult;
            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
            }

            
        }

        private void textBox5_MouseClick(object sender, MouseEventArgs e)
        {
            //Подразделение следственного комитета
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, true, "Справочник подразделений следственного комитета", "spravochnik_pod", new string[] { "Название", "Идентификационный номер", "Район", "Город" }, new string[] { "PK_Raiona", "Nazv", "id_number", "Raion", "pk_gorod" }, "spravochnik_gorodov", new string[] { "pk_gorod", "nazvanie" });
            f.ShowDialog();

            string PC_rezult = "";  //значение первичного ключа
            string Rezult = "";     // соответствующее текстовое значение

            PC_rezult = f.PC_rezult;
            Rezult = f.Rezult;

            if (PC_rezult != null)
            {
                textBox5.Text = Rezult;
                textBox6.Text = PC_rezult;
            }
            else
            {
                textBox5.Text = "";
                textBox6.Text = "";
            }
        }

        private void CriminalAffair_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (change)
            {
                DialogResult FClose = MessageBox.Show("Все не сохранённые данные будут потнряны!", "Сохранить изменения перед выходом?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (FClose == DialogResult.Yes)
                {
                    SaveData();
                    e.Cancel = false;
                }
                else
                    if (FClose == DialogResult.No)
                        e.Cancel = false;
                    else
                        e.Cancel = true;
            }
        }

        private void SaveData() // сохранение внесённой информации
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;

            try
            {
                if (PK_Dela == "")
                {
                    // добавление
                    string sql = "";

                    sql = "insert into " + table + "(";

                    for (int i = 0; i < DBHeader.Length; i++)
                    {
                        if (i == 0)
                            sql += DBHeader[i];
                        else
                            sql += ", " + DBHeader[i];
                    }

                    sql += ") values (";

                    sql += "'" + textBox1.Text + "'" + ", ";    //номер материала
                    sql += "STR_TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";    //Дата поступления материала

                    if (checkBox1.Checked)
                    {
                        sql += "'" + textBox2.Text + "'" + ", ";    //номер дела
                        sql += "STR_TO_DATE('" + dateTimePicker2.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";    //Дата возбуждения уголовного дела
                        if (checkBox2.Checked)
                            sql += "STR_TO_DATE('" + dateTimePicker3.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";//Дата передачи уголоного дела в суд
                        else
                            sql += "STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата передачи уголоного дела в суд
                    }
                    else
                    {
                        sql += "'" + "'" + ", ";    //номер дела
                        sql += "STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата возбуждения уголовного дела
                        sql += "STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата передачи уголоного дела в суд
                    }

                    if (checkBox3.Checked)
                        sql += "STR_TO_DATE('" + dateTimePicker4.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";//Дата закрытия уголовного дела
                    else
                        sql += "STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата закрытия уголовного дела

                    sql += "'" + textBox7.Text + "'" + ", ";    //коментарий
                    sql += "'" + textBox4.Text + "'" + ", ";    //Уполномоченный по делу
                    sql += "'" + textBox6.Text + "'" + ")";    //Подразделение следственного комитета

                    // внесение информации в БД
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();


                    sql = "SELECT last_insert_id()";    // получение последнего внесённого первичного ключа
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                    da = new MySqlDataAdapter(cmd);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        PK_Dela = dr[0].ToString();
                    }
                    else
                        MessageBox.Show("Сохранение не удалось!\nНе удалось получить первичный ключ!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Text = "Уголовное дело / материал проверки: " + PK_Dela;
                }
                else
                {
                    //редактирование
                    string sql = "";

                    sql = "update " + table + " set ";

                    sql += DBHeader[0] + " = '" + textBox1.Text + "'" + ", ";    //номер материала
                    sql += DBHeader[1] + " = STR_TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";    //Дата поступления материала

                    if (checkBox1.Checked)
                    {
                        sql += DBHeader[2] + " = '" + textBox2.Text + "'" + ", ";    //номер дела
                        sql += DBHeader[3] + " = STR_TO_DATE('" + dateTimePicker2.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";    //Дата возбуждения уголовного дела
                        if (checkBox2.Checked)
                            sql += DBHeader[4] + " = STR_TO_DATE('" + dateTimePicker3.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";//Дата передачи уголоного дела в суд
                        else
                            sql += DBHeader[4] + " = STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата передачи уголоного дела в суд
                    }
                    else
                    {
                        sql += DBHeader[2] + " = '" + "'" + ", ";    //номер дела
                        sql += DBHeader[3] + " = STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата возбуждения уголовного дела
                        sql += DBHeader[4] + " = STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата передачи уголоного дела в суд
                    }

                    if (checkBox3.Checked)
                        sql += DBHeader[5] + " = STR_TO_DATE('" + dateTimePicker4.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";//Дата закрытия уголовного дела
                    else
                        sql += DBHeader[5] + " = STR_TO_DATE('', '%d.%m.%Y')" + ", "; //Дата закрытия уголовного дела

                    sql += DBHeader[6] + " = '" + textBox7.Text + "'" + ", ";    //коментарий
                    sql += DBHeader[7] + " = '" + textBox4.Text + "'" + ", ";    //Уполномоченный по делу
                    sql += DBHeader[8] + " = '" + textBox6.Text + "'" + " where PK_Dela = " + PK_Dela;    //Подразделение следственного комитета

                    // внесение информации в БД
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }

                FileSave();
            }
            catch
            {
                MessageBox.Show("Сохранение не удалось!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void FileChange()   //файл изменили, но не сохранили
        {
            if (PK_Dela == "")
                this.Text = "Уголовное дело / материал проверки: Новый документ *";
            else
                this.Text = "Уголовное дело / материал проверки: " + PK_Dela + "*";
            change = true;
        }

        private void FileSave()     //файл изменили и сохранили
        {
            if (PK_Dela == "")
                this.Text = "Уголовное дело / материал проверки: Новый документ";
            else
                this.Text = "Уголовное дело / материал проверки: " + PK_Dela;
            change = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void пРОТОКОЛОбыскавыемкиПостановлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Resolution f = new Resolution(User, Password, Database, Ip, PK_Dela, "5", "1","666666");
           f.ShowDialog();
        }

        private void протоколЛичногоОбыскаПостановлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Resolution f = new Resolution(User, Password, Database, Ip, PK_Dela, "2", "6","666666666");
            f.ShowDialog();
        }

        private void пРОТОКОЛОсмотраМестностиЖилищаИногоПомещенияПостановлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Resolution f = new Resolution(User, Password, Database, Ip, PK_Dela, "1", "10","6666666");
            f.ShowDialog();
        }

        private void протоколОсмотраМетаПроисшествияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Protocol f = new Protocol(User, Password, Database, Ip, PK_Dela, "1", "10", "666");
            f.ShowDialog();
        }
    }
}
