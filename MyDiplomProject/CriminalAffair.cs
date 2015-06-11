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
        List<string> delPriticol;       // список протоколов для удаления
        List<string> delPost;           // список постановлений для удаления

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

        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации (уполномоченный)
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (справочник подразделений)
        string[] DBSHeader2; // название полей в таблице2 для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (протокол)
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
            DBHeader = new string[] { "Nomer_materiala", "DateofM", "Nomer_dela", "DateofV", "DateofPeredachi", "DateofClose", "Comment", "pk_polise", "PK_Raiona" };
            STable1 = "polise";
            DBSHeader1 = new string[] { "pk_polise", "surname", "Pname", "second_name" };
            STable2 = "spravochnik_pod";
            DBSHeader2 = new string[] { "PK_Raiona", "Nazv" };
            STable3 = "protokol";
            DBSHeader3 = new string[] { "pk_protokol", "Number", "data_sostav", "pk_polise" };
            delPriticol = new List<string>();
            delPost = new List<string>();

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
                    this.Text = "Уголовное дело / материал проверки";

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
                            loadFromOtherFormMultiItems(STable1, DBSHeader1, textBox4, textBox3);
                            // подразделение следственного комитета
                            loadFromOtherFormOneItem(STable2, DBSHeader2, textBox6, textBox5);


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
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + "\n\n\n\nError:2\nОшибка при загрузке данных!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadFromOtherFormMultiItems(string STable_, string[] DBSHeader_, TextBox from, TextBox to)
        {
            try
            {
                if (from.Text != "")
                {
                    MySqlDataAdapter da;
                    MySqlDataReader dr;
                    string sql;
                    sql = "select ";
                    for (int j = 1; j < DBSHeader_.Length; j++)
                    {
                        if (j == 1)
                            sql += DBSHeader_[j];
                        else
                            sql += ", " + DBSHeader_[j];
                    }
                    // генерация sql комманды
                    sql += " from " + STable_ + " where " + DBSHeader_[0] + " = " + from.Text;
                    //получение комманды и коннекта
                    cmd = new MySqlCommand(sql, mycon);
                    //вополнение запроса
                    cmd.ExecuteNonQuery();
                    da = new MySqlDataAdapter(cmd);
                    //получение выборки
                    dr = cmd.ExecuteReader();
                    // заполнения поля
                    if (dr.Read())
                        to.Text = dr[0].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString();
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString()); 
            }
        }

        private void loadFromOtherFormOneItem(string STable_, string[] DBSHeader_, TextBox from, TextBox to)
        {
            try
            {
                if (from.Text != "")
                {
                    MySqlDataAdapter da;
                    MySqlDataReader dr;
                    string sql;
                    sql = "select ";
                    for (int j = 1; j < DBSHeader_.Length; j++)
                    {
                        if (j == 1)
                            sql += DBSHeader_[j];
                        else
                            sql += ", " + DBSHeader_[j];
                    }
                    // генерация sql комманды
                    sql += " from " + STable_ + " where " + DBSHeader_[0] + " = " + from.Text;
                    //получение комманды и коннекта
                    cmd = new MySqlCommand(sql, mycon);
                    //вополнение запроса
                    cmd.ExecuteNonQuery();
                    da = new MySqlDataAdapter(cmd);
                    //получение выборки
                    dr = cmd.ExecuteReader();
                    // заполнения поля
                    if (dr.Read())
                        to.Text = dr[0].ToString();
                    dr.Close();
                }
            }
            catch
            { }
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

                        dataGridView1.Rows[i].Cells[5].Value = "Удалить";           // Удаление
                        dataGridView1.Rows[i].Cells[6].Value = "Открыть";           // Выбор

                        dataGridView1.Rows[i].Cells[7].Value = dr[21].ToString();    // pk_postanov (постановление)
                        dataGridView1.Rows[i].Cells[8].Value = dr[7].ToString();    // тип ротокола

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
                    items = dataGridView1.Rows.Count;
                }
                else
                    MessageBox.Show("Нет подключениея к базе данных!");
                Lock = false;
                dataGridView1.Enabled = true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
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
            textBox2.Enabled = dateTimePicker2.Enabled = checkBox1.Checked; // уголовное дело заведено
            checkBox2.Enabled = checkBox1.Checked;                          // передано в суд
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
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, true, "Справочник подразделений следственного комитета", "spravochnik_pod", new string[] { "Название", "Район", "Идентификационный номер", "Город" }, new string[] { "PK_Raiona", "Nazv", "Raion", "id_number", "pk_gorod" }, "spravochnik_gorodov", new string[] { "pk_gorod", "nazvanie" });
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
                string sql = "";    // строка sql запросов
                for (int i = 0; i < delPriticol.Count; i++)
                {

                    // поэлементное удаление вещественных доказательств
                    sql = " delete from vesh_dok where " + DBSHeader3[0] + " = " + delPriticol[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                    // поэлементное удаление понятых
                    sql = " delete from ponatoi where " + DBSHeader3[0] + " = " + delPriticol[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                    // поэлементное удаление тех средств
                    sql = " delete from r_tex_sredstv where " + DBSHeader3[0] + " = " + delPriticol[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                    if (delPost[i] != "")
                    {
                        // поэлементное удаление постановленй
                        sql = " delete from postanovlenie where pk_postanov = " + delPost[i];
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();

                        // поэлементное удаление людей из постановление
                        sql = " delete from peoples where pk_postanov = " + delPost[i];
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }

                    // поэлементное удаление людей из протокола
                    sql = " delete from peoples where " + DBSHeader3[0] + " = " + delPriticol[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                    // поэлементное удаление "В ходе осмотра проводилась"
                    sql = " delete from spend where " + DBSHeader3[0] + " = " + delPriticol[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                    // поэлементное удаление "К протоколу прилагаются"
                    sql = " delete from apps where " + DBSHeader3[0] + " = " + delPriticol[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                    // поэлементное удаление протоколов
                    sql = " delete from " + STable3 + " where " + DBSHeader3[0] + " = " + delPriticol[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }
                delPriticol.Clear(); // очистка списка протоклов для удаления
                delPost.Clear();     // очистка списка постановлений для удаления

                if (PK_Dela == "")
                {
                    // добавление

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

                    if (textBox4.Text != "")
                        sql += "'" + textBox4.Text + "'" + ", ";    //Уполномоченный по делу
                    else
                        sql += "NULL" + ", ";    //Уполномоченный по делу

                    if (textBox6.Text != "")
                        sql += "'" + textBox6.Text + "'" + ")";    //Подразделение следственного комитета
                    else
                        sql += "NULL" + ")";    //Подразделение следственного комитета

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
                    dr.Close();
                    this.Text = "Уголовное дело / материал проверки";
                }
                else
                {
                    //редактирование

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

                    if (textBox4.Text != "")
                        sql += DBHeader[7] + " = '" + textBox4.Text + "'" + ", ";    //Уполномоченный по делу
                    else
                        sql += DBHeader[7] + " = NULL" + ", ";    //Уполномоченный по делу

                    if (textBox6.Text != "")
                        sql += DBHeader[8] + " = '" + textBox6.Text + "'";    //Подразделение следственного комитета
                    else
                        sql += DBHeader[8] + " = NULL";

                    sql += " where PK_Dela = " + PK_Dela;

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
                this.Text = "Уголовное дело / материал проверки*";
            change = true;
        }

        private void FileSave()     //файл изменили и сохранили
        {
            if (PK_Dela == "")
                this.Text = "Уголовное дело / материал проверки: Новый документ";
            else
                this.Text = "Уголовное дело / материал проверки";
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
            SaveData();
            Resolution r;
            if (textBox2.Text != "")
                r = new Resolution(User, Password, Database, Ip, textBox2.Text, "5", "", "");
            else
                r = new Resolution(User, Password, Database, Ip, textBox1.Text, "5", "", "");
            r.ShowDialog();

            if (r.pk_postanov != "")
            {
                Protocol p = new Protocol(User, Password, Database, Ip, PK_Dela, "5", r.pk_postanov, "");
                p.ShowDialog();
            }
            LoadData();
        }

        private void протоколЛичногоОбыскаПостановлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
            Resolution r;
            if (textBox2.Text != "")
                r = new Resolution(User, Password, Database, Ip, textBox2.Text, "2", "", "");
            else
                r = new Resolution(User, Password, Database, Ip, textBox1.Text, "2", "", "");

            r.ShowDialog();

            if (r.pk_postanov != "")
            {
                Protocol p = new Protocol(User, Password, Database, Ip, PK_Dela, "3", r.pk_postanov, "");
                p.ShowDialog();
            }
            LoadData();
        }

        private void пРОТОКОЛОсмотраМестностиЖилищаИногоПомещенияПостановлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
            Resolution r;
            if (textBox2.Text != "")
                r = new Resolution(User, Password, Database, Ip, textBox2.Text, "1", "", "");
            else
                r = new Resolution(User, Password, Database, Ip, textBox1.Text, "1", "", "");
            r.ShowDialog();

            if (r.pk_postanov != "")
            {
                Protocol p = new Protocol(User, Password, Database, Ip, PK_Dela, "4", r.pk_postanov, "");
                p.ShowDialog();
            }
            LoadData();
        }

        private void протоколОсмотраМетаПроисшествияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
            this.Visible = false;
            Protocol f = new Protocol(User, Password, Database, Ip, PK_Dela, "1", "", "");
            f.ShowDialog();
            this.Visible = true;
            LoadData();
        }

        private void протоколОсмотраТрупаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
            this.Visible = false;
            Protocol f = new Protocol(User, Password, Database, Ip, PK_Dela, "2", "", "");
            f.ShowDialog();
            this.Visible = true;
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                DialogResult del = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        delPriticol.Add(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        delPost.Add(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                }
                FileChange();
            }

            if (e.ColumnIndex == 6 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                this.Visible = false;
                Protocol f = new Protocol(User, Password, Database, Ip, PK_Dela, dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                f.ShowDialog();
                this.Visible = true;
            }
            LoadTable();
        }

        private void прикрепитьСуществующийПротоколToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
            this.Visible = false;
            SelectProtocol sp = new SelectProtocol(User, Password, Database, Ip, PK_Dela);
            sp.ShowDialog();
            this.Visible = true;
            LoadTable();
        }
    }
}
