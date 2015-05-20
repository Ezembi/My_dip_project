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
    public partial class Resolution : Form
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

        string PK_Dela;     // внешний ключ уголовного дела / материала проверки для выборки
        string id_post;     // номер варианта постановления
        string pk_postanov; // первичный ключ постановления

        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации (уполномоченный)
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (города)
        string[] DBSHeader2; // название полей в таблице2 для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (должности)
        string[] DBSHeader3; // название полей в таблице3 для sql запросов, для сопудствующей информации

        string STable4;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (прокуратура)
        string[] DBSHeader4; // название полей в таблице2 для sql запросов, для сопудствующей информации

        string STable5;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (суд)
        string[] DBSHeader5; // название полей в таблице2 для sql запросов, для сопудствующей информации

        bool change = false;

        public Resolution()
        {
            InitializeComponent();
        }

        public Resolution(string _user, string _pass, string _database, string _ip, string _PK_Dela, string _id_post, string _pk_postanov)
        {

            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip; 
            PK_Dela = _PK_Dela;
            id_post = _id_post;
            pk_postanov = _pk_postanov;

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
                MessageBox.Show("Нет доступа к базе данных!", "Ошибка подключения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                            dr.Close();

                            switch (id_post)
                            {
                                // Протокол осмотра местности, жилища, иного помещения
                                case "1": break;
                                // Протокол личного обыска
                                case "2": break;
                                // Протокол обыска (выемки)
                                case "5":
                                    {
                                        panel1.Visible = panel2.Visible = dataGridView1.Visible = label21.Visible = false;
                                        this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 135);
                                    } break;
                            }


                            //грузим названия по id_шникам
                            #region уполномоченный
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

                            LoadTable();
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
                            case "2": break;
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
                MessageBox.Show("Ошибка при загрузке данных!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTable()
        {

        }

        private void loadFromOtherTable(string STable_, string[] DBSHeader_, TextBox from, TextBox to)
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
                    sql += "'" + textBox4.Text + "'" + ", ";     //pk_polise
                    sql += "'" + textBox2.Text + "'" + ", ";     //pk_gorod
                    sql += "'" + textBox9.Text + "'" + ", ";     //pk_prosecutor1
                    sql += "'" + textBox11.Text + "'" + ", ";    //pk_court1
                    sql += "'" + textBox15.Text + "'" + ", ";     //pk_prosecutor2
                    sql += "'" + textBox17.Text + "'" + ", ";     //pk_court2
                    sql += "'" + textBox13.Text + "'" + ")";     //pk_dolgnost

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
                        MessageBox.Show("Сохранение не удалось!\nНе удалось получить первичный ключ!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //this.Text = "Уголовное дело / материал проверки: " + PK_Dela;
                }
                else
                {
                    //редактирование
                    string sql = "";

                    sql = "update " + table + " set ";
                    sql += DBHeader[0] + " = '" + textBox6.Text + "'" + ", ";    //Obosnovanie
                    sql += DBHeader[1] + " = STR_TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";    //DateOfCreate
                    sql += DBHeader[2] + " = '" + textBox7.Text + "'" + ", ";     //plase
                    sql += DBHeader[3] + " = '" + textBox25.Text + "'" + ", ";    //street
                    sql += DBHeader[4] + " = '" + textBox24.Text + "'" + ", ";    //house
                    sql += DBHeader[5] + " = '" + textBox23.Text + "'" + ", ";    //room
                    sql += DBHeader[6] + " = '" + id_post + "'" + ", ";           //id_post
                    sql += DBHeader[7] + " = '" + textBox4.Text + "'" + ", ";     //pk_polise
                    sql += DBHeader[8] + " = '" + textBox2.Text + "'" + ", ";     //pk_gorod
                    sql += DBHeader[9] + " = '" + textBox9.Text + "'" + ", ";     //pk_prosecutor1
                    sql += DBHeader[10] + " = '" + textBox11.Text + "'" + ", ";    //pk_court1
                    sql += DBHeader[11] + " = '" + textBox15.Text + "'" + ", ";     //pk_prosecutor2
                    sql += DBHeader[12] + " = '" + textBox17.Text + "'" + ", ";     //pk_court2
                    sql += DBHeader[13] + " = '" + textBox13.Text + "'";     //pk_dolgnost

                    sql += " where pk_postanov = " + pk_postanov;    //Постановление

                    // внесение информации в БД
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }

                //FileSave();
            }
            catch
            {
                MessageBox.Show("Сохранение не удалось!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" });
            f.ShowDialog();
            loadFromOtherForm(f.PC_rezult,f.Rezult, textBox2, textBox1);
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
        }
    }
}
