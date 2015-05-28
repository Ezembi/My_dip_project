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
using System.Windows;

namespace MyDiplomProject
{
    public partial class Resolution : Form
    {

        public MySqlConnection mycon;   // коннектор б/д
        public MySqlCommand cmd;        // sql комманды для работы с б/д

        List<string> delList;

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

        string PK_Dela;     // внешний ключ уголовного дела / материала проверки для выборки
        string id_post;     // номер варианта постановления
        string pk_postanov; // первичный ключ постановления
        string pk_protokol; // пномер протокола

        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации (уполномоченный)
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (города)
        string[] DBSHeader2; // название полей в таблице2 для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (должности)
        string[] DBSHeader3; // название полей в таблице3 для sql запросов, для сопудствующей информации

        string STable4;      // ТАБЛИЦА4 БАЗЫ ДАННЫХ для сопудствующей информации (прокуратура)
        string[] DBSHeader4; // название полей в таблице3 для sql запросов, для сопудствующей информации

        string STable5;      // ТАБЛИЦА5 БАЗЫ ДАННЫХ для сопудствующей информации (суд)
        string[] DBSHeader5; // название полей в таблице5 для sql запросов, для сопудствующей информации

        string STable6;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (обыскиваемый)
        string[] DBSHeader6; // название полей в таблице6 для sql запросов, для сопудствующей информации

        string STable7;      // ТАБЛИЦА7 БАЗЫ ДАННЫХ для сопудствующей информации (проц. положение)

        bool change = false;

        public Resolution()
        {
            InitializeComponent();
        }

        public Resolution(string _user, string _pass, string _database, string _ip, string _PK_Dela, string _id_post, string _pk_postanov, string _pk_protokol)
        {

            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            PK_Dela = _PK_Dela;
            id_post = _id_post;
            pk_postanov = _pk_postanov;
            pk_protokol = _pk_protokol;

            table = "postanovlenie";
            DBHeader = new string[] { "Obosnovanie", "DateOfCreate", "plase", "street", "house", "room", "id_post", "pk_polise", "pk_gorod", "pk_prosecutor1", "pk_court1", "pk_prosecutor2", "pk_court2", "pk_dolgnost" };
            STable1 = "polise";
            DBSHeader1 = new string[] { "pk_polise", "surname", "Pname", "second_name" };
            STable2 = "spravochnik_gorodov";
            DBSHeader2 = new string[] { "pk_gorod", "nazvanie", "id_number" };
            STable3 = "spravochnik_dolgnostei";
            DBSHeader3 = new string[] { "pk_dolgnost", "nazvanie", "id_number" };
            STable4 = "prosecutor";
            DBSHeader4 = new string[] { "pk_prosecutor", "nazvanie", "id_number" };
            STable5 = "court";
            DBSHeader5 = new string[] { "pk_court", "nazvanie", "id_number" };
            STable6 = "peoples";
            DBSHeader6 = new string[] { "PK_people", "surname", "Pname", "second_name", "primichanie", "mystate", "pk_postanov", "pk_protokol", "pk_pol" };

            STable7 = "sp_pro_pol";

            delList = new List<string>();

            InitializeComponent();
        }

        private void Resolution_Load(object sender, EventArgs e)
        {
            try
            {
                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;Convert Zero Datetime=True;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();

                /*if (PK_Dela == "")
                    this.Text = "Уголовное дело / материал проверки: Новый документ";
                else
                    this.Text = "Уголовное дело / материал проверки: " + PK_Dela;*/

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
                    if (pk_postanov != "")
                    {
                        string sql;
                        //генерация sql запроса, для отображения данных из БД на форму
                        sql = "select * from " + table + " where pk_postanov = " + pk_postanov;
                        cmd = new MySqlCommand(sql, mycon);

                        //вополнение запроса
                        cmd.ExecuteNonQuery();

                        //выборка по запросу
                        da = new MySqlDataAdapter(cmd);
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            textBox6.Text = dr[1].ToString();   //обоснование

                            if (dr[2].ToString() != "01.01.0001 0:00:00")   //дата составления
                                dateTimePicker1.Value = Convert.ToDateTime(dr[2].ToString());

                            id_post = dr[7].ToString();             // номер варианта постановления
                            textBox4.Text = dr[8].ToString();       // Уполномоченный по делу
                            textBox2.Text = dr[9].ToString();       // город
                            textBox9.Text = dr[10].ToString();      // прокурор 1
                            textBox11.Text = dr[11].ToString();     // суд 1
                            textBox15.Text = dr[12].ToString();     // прокурор 2
                            textBox17.Text = dr[13].ToString();     // суд 2
                            textBox13.Text = dr[14].ToString();     // должность
                            textBox25.Text = dr[4].ToString();      // улица
                            textBox24.Text = dr[5].ToString();      // дом
                            textBox23.Text = dr[6].ToString();      // квартира
                            textBox7.Text = dr[3].ToString();       // произвести обыск (выемку) в жилище
                            textBox5.Text = pk_protokol;


                            dr.Close();

                            switch (id_post)
                            {
                                // Протокол осмотра местности, жилища, иного помещения
                                case "1":
                                    {
                                        LoadTable();
                                        this.Text = "Протокол осмотра местности, жилища, иного помещения";
                                    } break;

                                // Протокол личного обыска
                                case "2":
                                    {
                                        panel2.Visible = dataGridView1.Visible = label21.Visible = false;
                                        this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 135);

                                        //грузим обыскиваемого
                                        #region обыскиваемый
                                        sql = "select * from " + STable6 + " where pk_postanov = " + pk_postanov;
                                        //получение комманды и коннекта
                                        cmd = new MySqlCommand(sql, mycon);
                                        //вополнение запроса
                                        cmd.ExecuteNonQuery();
                                        da = new MySqlDataAdapter(cmd);
                                        //получение выборки
                                        dr = cmd.ExecuteReader();
                                        // заполнения поля 
                                        if (dr.Read())
                                        {
                                            textBox20.Text = dr[1].ToString(); //фамилия
                                            textBox21.Text = dr[2].ToString(); //имя
                                            textBox22.Text = dr[3].ToString(); //отчество
                                            textBox19.Text = dr[8].ToString(); //процесс. положение
                                        }
                                        dr.Close();
                                        #endregion

                                        //грузим процесс. положение
                                        #region процесс. положение
                                        if (textBox19.Text != "")
                                        {
                                            sql = "select * from " + STable7 + " where pk_pol = " + textBox19.Text;
                                            //получение комманды и коннекта
                                            cmd = new MySqlCommand(sql, mycon);
                                            //вополнение запроса
                                            cmd.ExecuteNonQuery();
                                            da = new MySqlDataAdapter(cmd);
                                            //получение выборки
                                            dr = cmd.ExecuteReader();
                                            // заполнения поля 
                                            if (dr.Read())
                                            {
                                                textBox18.Text = dr[1].ToString(); //фамилия
                                            }
                                            dr.Close();
                                        }
                                        #endregion

                                        this.Text = "Протокол личного обыска";
                                    } break;

                                // Протокол обыска (выемки)
                                case "5":
                                    {
                                        panel1.Visible = panel2.Visible = dataGridView1.Visible = label21.Visible = false;
                                        this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 135);
                                        this.Text = "Протокол обыска (выемки)";
                                    } break;
                            }


                            //грузим названия по id_шникам
                            #region уполномоченный
                            if (textBox4.Text != "")
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
                            }
                            #endregion

                            // город
                            loadFromOtherTable(STable2, DBSHeader2, textBox2, textBox1);
                            // должность
                            loadFromOtherTable(STable3, DBSHeader3, textBox13, textBox12);
                            // прокурор №1
                            loadFromOtherTable(STable4, DBSHeader4, textBox9, textBox8);
                            // прокурор №2
                            loadFromOtherTable(STable4, DBSHeader4, textBox15, textBox14);
                            // суд №1
                            loadFromOtherTable(STable5, DBSHeader5, textBox11, textBox10);
                            // суд №2
                            loadFromOtherTable(STable5, DBSHeader5, textBox17, textBox16);

                        }
                        else
                            throw new Exception();

                        // иправление бага WS
                        rSize = (rSize > 0) ? -1 : 1;
                        this.ClientSize = new System.Drawing.Size(this.ClientSize.Width + rSize, this.ClientSize.Height);

                    }
                    else
                    {
                        switch (id_post)
                        {
                            // Протокол осмотра местности, жилища, иного помещения
                            case "1": break;
                            // Протокол личного обыска
                            case "2":
                                {
                                    panel2.Visible = dataGridView1.Visible = label21.Visible = false;
                                    this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 135);
                                } break;
                            // Протокол обыска (выемки)
                            case "5":
                                {
                                    panel1.Visible = panel2.Visible = dataGridView1.Visible = label21.Visible = false;
                                    this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 135);
                                } break;
                        }
                    }

                    Lock = false;
                    dataGridView1.Enabled = true;
                    //FileSave();
                }
                else
                    MessageBox.Show("Нет подключениея к базе данных!");
            }
            catch
            {
                MessageBox.Show("Error:2\nОшибка при загрузке данных!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTable()
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            int i = 0;
            bool LocLock = Lock;
            try
            {
                Lock = true;
                dataGridView1.Rows.Clear();
                Lock = LocLock;

                //грузим проживающих в данном жилом помещении лиц
                sql = "select * from " + STable6 + " where pk_postanov = " + pk_postanov;
                //получение комманды и коннекта
                cmd = new MySqlCommand(sql, mycon);
                //вополнение запроса
                cmd.ExecuteNonQuery();
                da = new MySqlDataAdapter(cmd);
                //получение выборки
                dr = cmd.ExecuteReader();
                // заполнения поля 
                while (dr.Read())
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();    // PC
                    dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();    // фамилия
                    dataGridView1.Rows[i].Cells[2].Value = dr[2].ToString();    // имя
                    dataGridView1.Rows[i].Cells[3].Value = dr[3].ToString();    // отчество
                    dataGridView1.Rows[i].Cells[4].Value = "Удалить";           // Удаление

                    i++;
                }
                dr.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }

        private void loadFromOtherTable(string STable_, string[] DBSHeader_, TextBox from, TextBox to)
        {
            try
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
            catch
            { }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void SaveData() // сохранение внесённой информации
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;

            try
            {
                if (pk_postanov == "")
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

                    sql += "'" + textBox6.Text + "'" + ", ";    //Obosnovanie
                    sql += "STR_TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";    //DateOfCreate
                    sql += "'" + textBox7.Text + "'" + ", ";     //plase
                    sql += "'" + textBox25.Text + "'" + ", ";    //street
                    sql += "'" + textBox24.Text + "'" + ", ";    //house
                    sql += "'" + textBox23.Text + "'" + ", ";    //room
                    sql += "'" + id_post + "'" + ", ";           //id_post

                    if (textBox4.Text != "")
                        sql += "'" + textBox4.Text + "'" + ", ";    //pk_polise
                    else
                        sql += "NULL" + ", ";

                    if (textBox2.Text != "")
                        sql += "'" + textBox2.Text + "'" + ", ";    //pk_gorod
                    else
                        sql += "NULL" + ", ";

                    if (textBox9.Text != "")
                        sql += "'" + textBox9.Text + "'" + ", ";    //pk_prosecutor1
                    else
                        sql += "NULL" + ", ";

                    if (textBox11.Text != "")
                        sql += "'" + textBox11.Text + "'" + ", ";    //pk_court1
                    else
                        sql += "NULL" + ", ";

                    if (textBox15.Text != "")
                        sql += "'" + textBox15.Text + "'" + ", ";    //pk_prosecutor2
                    else
                        sql += "NULL" + ", ";

                    if (textBox17.Text != "")
                        sql += "'" + textBox17.Text + "'" + ", ";    //pk_court2
                    else
                        sql += "NULL" + ", ";

                    if (textBox13.Text != "")
                        sql += "'" + textBox13.Text + "')";    //pk_dolgnost
                    else
                        sql += "NULL)";

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
                        pk_postanov = dr[0].ToString();
                    }
                    else
                        MessageBox.Show("Error:3\nСохранение не удалось!\nНе удалось получить первичный ключ!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //this.Text = "Уголовное дело / материал проверки: " + PK_Dela;

                    dr.Close();

                    if (id_post == "1")
                    {
                        string tabl = "", end_sql = "";
                        sql = "";
                        sql = "insert into " + STable6 + "(";
                        for (int i = 1; i < 4; i++)
                        {
                            if (i == 1)
                                sql += DBSHeader6[i];
                            else
                                sql += ", " + DBSHeader6[i];
                        }
                        sql += ", " + DBSHeader6[5];    //mystate
                        sql += ", " + DBSHeader6[6];    //pk_postanov

                        sql += ") values (";

                        end_sql = "";
                        end_sql += "'1', ";                             //mystate
                        end_sql += "'" + pk_postanov + "')";            //pk_postanov

                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            tabl = "";
                            for (int j = 1; j < 4; j++)
                            {
                                if (dataGridView1.Rows[i].Cells[j].Value != null)
                                    tabl += "'" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "'";
                                else
                                    tabl += "NULL";

                                if (i != 3)
                                    tabl += ",";
                            }

                            cmd = new MySqlCommand(sql + tabl + end_sql, mycon);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (id_post == "2")
                    {
                        sql = "";
                        sql = "insert into " + STable6 + "(";
                        for (int i = 1; i < 4; i++)
                        {
                            if (i == 1)
                                sql += DBSHeader6[i];
                            else
                                sql += ", " + DBSHeader6[i];
                        }
                        sql += ", " + DBSHeader6[5];    //mystate
                        sql += ", " + DBSHeader6[6];    //pk_postanov
                        sql += ", " + DBSHeader6[8];    //pk_pol

                        sql += ") values (";

                        sql += "'" + textBox20.Text + "'" + ", ";    //surname
                        sql += "'" + textBox21.Text + "'" + ", ";    //Pname
                        sql += "'" + textBox22.Text + "'" + ", ";    //second_name
                        sql += "'1', ";                              //mystate
                        sql += "'" + pk_postanov + "', ";            //pk_postanov

                        if (textBox19.Text != "")
                            sql += "'" + textBox19.Text + "')";       //pk_pol
                        else
                            sql += "NULL)";

                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }

                }
                else
                {
                    string sql = "";

                    for (int i = 0; i < delList.Count; i++)
                    {
                        sql = " delete from " + STable6 + " where " + DBSHeader6[0] + " = " + delList[i];
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                    delList.Clear();

                    //редактирование

                    sql = "";
                    sql = "update " + table + " set ";
                    sql += DBHeader[0] + " = '" + textBox6.Text + "'" + ", ";    //Obosnovanie
                    sql += DBHeader[1] + " = STR_TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";    //DateOfCreate
                    sql += DBHeader[2] + " = '" + textBox7.Text + "'" + ", ";     //plase
                    sql += DBHeader[3] + " = '" + textBox25.Text + "'" + ", ";    //street
                    sql += DBHeader[4] + " = '" + textBox24.Text + "'" + ", ";    //house
                    sql += DBHeader[5] + " = '" + textBox23.Text + "'" + ", ";    //room
                    sql += DBHeader[6] + " = '" + id_post + "'" + ", ";           //id_post

                    if (textBox4.Text != "")
                        sql += DBHeader[7] + " = '" + textBox4.Text + "'" + ", ";     //pk_polise
                    else
                        sql += DBHeader[7] + " = NULL" + ", ";

                    if (textBox2.Text != "")
                        sql += DBHeader[8] + " = '" + textBox2.Text + "'" + ", ";     //pk_gorod
                    else
                        sql += DBHeader[8] + " = NULL" + ", ";

                    if (textBox9.Text != "")
                        sql += DBHeader[9] + " = '" + textBox9.Text + "'" + ", ";     //pk_prosecutor1
                    else
                        sql += DBHeader[9] + " = NULL" + ", ";

                    if (textBox11.Text != "")
                        sql += DBHeader[10] + " = '" + textBox11.Text + "'" + ", ";     //pk_court1
                    else
                        sql += DBHeader[10] + " = NULL" + ", ";

                    if (textBox15.Text != "")
                        sql += DBHeader[11] + " = '" + textBox15.Text + "'" + ", ";     //pk_prosecutor2
                    else
                        sql += DBHeader[11] + " = NULL" + ", ";

                    if (textBox17.Text != "")
                        sql += DBHeader[12] + " = '" + textBox17.Text + "'" + ", ";     //pk_court2
                    else
                        sql += DBHeader[12] + " = NULL" + ", ";

                    if (textBox13.Text != "")
                        sql += DBHeader[13] + " = '" + textBox13.Text + "'";     //pk_dolgnost
                    else
                        sql += DBHeader[13] + " = NULL";

                    sql += " where pk_postanov = " + pk_postanov;    //Постановление

                    // внесение информации в БД
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                    if (id_post == "1")
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[0].Value != null)
                            {
                                //изменение
                                sql = "update " + STable6 + " set ";
                                for (int j = 1; j < 4; j++)
                                {
                                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                                        sql += DBSHeader6[j] + " = '" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "',";
                                    else
                                        sql += DBSHeader6[j] + " = NULL,";
                                }
                                sql += DBSHeader6[5] + " = '1'";                              //mystate
                                sql += " where " + DBSHeader6[0] + " = " + dataGridView1.Rows[i].Cells[0].Value.ToString();
                                cmd = new MySqlCommand(sql, mycon);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                //добавление
                                string tabl = "", end_sql = "";
                                sql = "";
                                sql = "insert into " + STable6 + "(";
                                for (int j = 1; j < 4; j++)
                                {
                                    if (j == 1)
                                        sql += DBSHeader6[j];
                                    else
                                        sql += ", " + DBSHeader6[j];
                                }
                                sql += ", " + DBSHeader6[5];    //mystate
                                sql += ", " + DBSHeader6[6];    //pk_postanov

                                sql += ") values (";

                                end_sql = "";
                                end_sql += "'1', ";                             //mystate
                                end_sql += "'" + pk_postanov + "')";            //pk_postanov

                                tabl = "";
                                for (int j = 1; j < 4; j++)
                                {
                                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                                        tabl += "'" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "',";
                                    else
                                        tabl += "NULL,";
                                }

                                sql += tabl + end_sql;
                                cmd = new MySqlCommand(sql, mycon);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        LoadTable();
                    }

                    if (id_post == "2")
                    {
                        sql = "";
                        sql = "update " + STable6 + " set ";
                        sql += DBSHeader6[1] + " = '" + textBox20.Text + "'" + ", ";    //surname
                        sql += DBSHeader6[2] + " = '" + textBox21.Text + "'" + ", ";    //Pname
                        sql += DBSHeader6[3] + " = '" + textBox22.Text + "'" + ", ";    //second_name
                        sql += DBSHeader6[5] + " = '1', ";                              //mystate

                        if (textBox19.Text != "")
                            sql += DBSHeader6[8] + " = '" + textBox19.Text + "'";       //pk_pol
                        else
                            sql += DBSHeader6[8] + " = " + "NULL";
                        sql += " where pk_postanov = " + pk_postanov;                   //Постановление
                        // внесение информации в БД
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                }

                FileSave();
            }
            catch
            {
                MessageBox.Show("Error:4\nСохранение не удалось!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox2, textBox1);
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            AddOper f = new AddOper(User, Password, Database, Ip, true, "Справочник уполномоченных", "polise", new string[] { "Табельный номер", "Фамилия", "Имя", "Отчество", "Звание", "Должность", "Чин" }, new string[] { "pk_polise", "id_number", "surname", "Pname", "second_name", "pk_zvanie", "pk_dolgnost", "pk_chin" }, "spravochnik_zvanii", new string[] { "pk_zvanie", "nazvanie" }, "spravochnik_dolgnostei", new string[] { "pk_dolgnost", "nazvanie" }, "chin", new string[] { "pk_chin", "nazvanie" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox4, textBox3);
        }

        private void textBox8_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Органы прокуратуры", "prosecutor", new string[] { "Наименование органа прокуратуры", "Идентификационный номер" }, new string[] { "pk_prosecutor", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox9, textBox8);
        }

        private void textBox10_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Наименование суда", "court", new string[] { "Наименование суда", "Идентификационный номер" }, new string[] { "pk_court", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox11, textBox10);
        }

        private void textBox12_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник должностей", "spravochnik_dolgnostei", new string[] { "Должность", "Идентификационный номер" }, new string[] { "pk_dolgnost", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox13, textBox12);
        }

        private void textBox14_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Органы прокуратуры", "prosecutor", new string[] { "Наименование органа прокуратуры", "Идентификационный номер" }, new string[] { "pk_prosecutor", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox15, textBox14);
        }

        private void textBox16_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Наименование суда", "court", new string[] { "Наименование суда", "Идентификационный номер" }, new string[] { "pk_court", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox17, textBox16);
        }

        private void loadFromOtherForm(string PC_rezult, string Rezult, TextBox PC, TextBox Rez)
        {
            if (PC_rezult != null)
            {
                Rez.Text = Rezult;
                PC.Text = PC_rezult;
            }
            else
            {
                Rez.Text = PC.Text = "";
            }
        }

        private void Resolution_Shown(object sender, EventArgs e)
        {
            LoadData();
            FileSave();
        }

        private void textBox18_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник ппроцессуальных положений", "sp_pro_pol", new string[] { "Наименование положения", "Идентификационный номер" }, new string[] { "pk_pol", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult, f.Rezult, textBox19, textBox18);
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > 0)
                dataGridView1.Rows[e.RowIndex - 1].Cells[4].Value = "Удалить";           // Удаление
        }

        private void Resolution_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 3 && e.KeyChar != 22 && e.KeyChar != 26)
                e.Handled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                DialogResult del = MessageBox.Show("Подтверждение удаления", "Вы действительно хотите удалить данный элемент?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        delList.Add(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                }
                FileChange();
            }

        }

        private void FileChange()   //файл изменили, но не сохранили
        {
            switch (id_post)
            {
                // Протокол осмотра местности, жилища, иного помещения
                case "1": this.Text = "Протокол осмотра местности, жилища, иного помещения*"; break;

                // Протокол личного обыска
                case "2": this.Text = "Протокол личного обыска*"; break;

                // Протокол обыска (выемки)
                case "5": this.Text = "Протокол обыска (выемки)*"; break;
            }
            change = true;
        }

        private void FileSave()     //файл изменили и сохранили
        {
            switch (id_post)
            {
                // Протокол осмотра местности, жилища, иного помещения
                case "1": this.Text = "Протокол осмотра местности, жилища, иного помещения"; break;

                // Протокол личного обыска
                case "2": this.Text = "Протокол личного обыска"; break;

                // Протокол обыска (выемки)
                case "5": this.Text = "Протокол обыска (выемки)"; break;
            }
            change = false;
        }

        private void Resolution_FormClosing(object sender, FormClosingEventArgs e)
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

        private void textBox1_TextChanged(object sender, EventArgs e)
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FileChange();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            FileChange();
        }
    }
}
