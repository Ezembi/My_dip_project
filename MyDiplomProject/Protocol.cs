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
    public partial class Protocol : Form
    {
        public MySqlConnection mycon;   // коннектор б/д
        public MySqlCommand cmd;        // sql комманды для работы с б/д

        List<string> delVeshDokList;    // список вещ доков для удаления
        List<string> delPeoples;        // список лиц для удаления
        List<string> delDevise;         // список тех средств для удаления

        List<string> delSpend;          // список "В ходе осмотра проводилась" для удаления
        List<string> delApps;           // список "К протоколу прилагаются" для удаления

        bool Lock = true;   // блокировка действий пользователя до полной загрузки формы
        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера
        int rSize = 1;      // иправление бага WS
        string table;       // ТАБЛИЦА БАЗЫ ДАННЫХ
        string[] DBHeader;  // название полей в таблице для sql запросов

        string PK_Dela;     // внешний ключ уголовного дела / материала проверки для выборки
        string id_prot;     // номер варианта протокола
        string id_psot;     // номер варианта постановления
        string pk_postanov; // первичный ключ постановления
        string pk_protokol; // пномер протокола

        string STable0;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (город)
        string[] DBSHeader0; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable1;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (погода)
        string[] DBSHeader1; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (освещённость)
        string[] DBSHeader2; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (Вещественное доказательство)
        string[] DBSHeader3; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable4;      // ТАБЛИЦА4 БАЗЫ ДАННЫХ для сопудствующей информации (Способы упаковки вещественных доказательств)
        string[] DBSHeader4; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable5;      // ТАБЛИЦА5 БАЗЫ ДАННЫХ для сопудствующей информации (Справочник материалов, в которые упаковываю вещественные доказательства (полиэтилен, бумага и т.д.))
        string[] DBSHeader5; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable6;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (другие люди)
        string[] DBSHeader6; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable7;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (уполномоченный)
        string[] DBSHeader7; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable8;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (понятой)
        string[] DBSHeader8; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable9;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (постановление)
        string[] DBSHeader9; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable10;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (разшивачная таблица с тех средствами)
        string[] DBSHeader10; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable11;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (тех средства)
        string[] DBSHeader11; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable12;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (специалист)
        string[] DBSHeader12; // название полей в таблице для sql запросов, для сопудствующей информации

        bool change = false;

        public Protocol()
        {
            InitializeComponent();
        }

        public Protocol(string _user, string _pass, string _database, string _ip, string _PK_Dela, string _id_prot, string _pk_postanov, string _pk_protokol)
        {

            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            PK_Dela = _PK_Dela;
            id_prot = _id_prot;
            pk_postanov = _pk_postanov;
            pk_protokol = _pk_protokol;

            table = "protokol";
            DBHeader = new string[] { "pk_protokol", "data_sostav", "Vremya_nachala", "vremya_okonch", "mesto_peibitiya", "coobshenie", "predmet_osmotra", "id_prot", "Zayavleniya", "zamechaniya", "I_look", "Sposob_izyatiya", "temperature", "dead_go", "cel_obiska", "otdali", "pk_gorod", "pk_pogoda", "pk_osveshennost", "pk_spec", "pk_polise", "pk_postanov", "PK_Dela", "install" };

            STable0 = "spravochnik_gorodov";
            DBSHeader0 = new string[] { "pk_gorod", "nazvanie", "id_number" };
            STable1 = "spravochnik_pogodi";
            DBSHeader1 = new string[] { "pk_pogoda", "nazvanie" };
            STable2 = "spravochnik_osveshennosti";
            DBSHeader2 = new string[] { "pk_osveshennost", "nazvanie" };
            STable3 = "vesh_dok";
            DBSHeader3 = new string[] { "pk_vesh_dok", "priznaki", "naiminovanie", "pk_material", "pk_ypakovka", "pk_protokol" };
            STable4 = "ypakovka";
            DBSHeader4 = new string[] { "pk_ypakovka", "nazvanie" };
            STable5 = "spravochnik_materialov";
            DBSHeader5 = new string[] { "pk_material", "material", "id_number" };
            STable6 = "peoples";
            DBSHeader6 = new string[] { "PK_people", "surname", "Pname", "second_name", "primichanie", "mystate", "pk_postanov", "pk_protokol", "pk_pol" };
            STable7 = "polise";
            DBSHeader7 = new string[] { "pk_polise","surname", "Pname", "second_name"};
            STable8 = "ponatoi";
            DBSHeader8 = new string[] { "pk_ponatoi", "surname", "Pname", "second_name", "street", "house", "room", "pk_protokol" };
            STable9 = "postanovlenie";
            DBSHeader9 = new string[] { "Obosnovanie", "DateOfCreate", "plase", "street", "house", "room", "id_post", "pk_polise", "pk_gorod", "pk_prosecutor1", "pk_court1", "pk_prosecutor2", "pk_court2", "pk_dolgnost" };
            STable10 = "r_tex_sredstv";
            DBSHeader10 = new string[] { "pc_r", "pk_tex_sredstvo", "pk_protokol" };
            STable11 = "spravochnik_tex_sredstv";
            DBSHeader11 = new string[] { "pk_tex_sredstvo", "nazvanie", "id_number" };
            STable12 = "specialist";
            DBSHeader12 = new string[] { "pk_spec", "surname", "Pname", "second_name" };

            delVeshDokList = new List<string>();
            delPeoples = new List<string>();
            delDevise = new List<string>();
            delSpend = new List<string>();
            delApps = new List<string>();

            InitializeComponent();
        }

        private void Protocol_Load(object sender, EventArgs e)
        {
            try
            {
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

            }
            catch
            {
                MessageBox.Show("Error:1\nНет доступа к базе данных!", "Ошибка подключения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveTables(string STable, string[] DBSHeader, string lastPcName, string PcNum, List<string> delList, System.Windows.Forms.DataGridView MyDataGridView, int MaxColums)  // сохранение вещественных доказательств(изъятого имущества)
        {
            try
            {
                string sql = "";    // строка sql запросов
                for (int i = 0; i < delList.Count; i++)
                {
                    // поэлементное удаление вещественных доказательств(изъятого имущества)
                    sql = " delete from " + STable + " where " + DBSHeader[0] + " = " + delList[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }
                delList.Clear(); // очистка списка вещественных доказательств(изъятого имущества) для удаления

                for (int i = 0; i < MyDataGridView.Rows.Count - 1; i++)
                {
                    if (MyDataGridView.Rows[i].Cells[0].Value != null)
                    {
                        //изменение
                        sql = "update " + STable + " set ";
                        for (int j = 1; j < MaxColums; j++)
                        {
                            if (MyDataGridView.Rows[i].Cells[j].Value != null && MyDataGridView.Rows[i].Cells[j].Value.ToString() != "")
                                sql += DBSHeader[j] + " = '" + MyDataGridView.Rows[i].Cells[j].Value.ToString() + "'";  //если ячейка заполнена
                            else
                                sql += DBSHeader[j] + " = NULL";    // если ячейка пуста

                            if (j != MaxColums - 1)
                                sql += ",";
                        }
                        sql += " where " + DBSHeader[0] + " = " + MyDataGridView.Rows[i].Cells[0].Value.ToString();
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //добавление
                        sql = "";
                        sql = "insert into " + STable + "(";
                        for (int j = 1; j < MaxColums; j++)
                        {
                            if (j == 1)
                                sql += DBSHeader[j];
                            else
                                sql += ", " + DBSHeader[j];
                        }

                        sql += ", " + lastPcName;

                        sql += ") values (";

                        for (int j = 1; j < MaxColums; j++)
                        {
                            if (MyDataGridView.Rows[i].Cells[j].Value != null && MyDataGridView.Rows[i].Cells[j].Value.ToString() != "")
                                sql += "'" + MyDataGridView.Rows[i].Cells[j].Value.ToString() + "',";
                            else
                                sql += "NULL,";
                        }

                        sql += PcNum + ")";

                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch
            {
                MessageBox.Show("Error:" + STable + "\nНе удалось сохранить список!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadVeshDok(string STable, System.Windows.Forms.DataGridView MyDataGridView)   // загрузка списка вещественных доказательств(изъятого имущества)
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            int i = 0;
            bool LocLock = Lock;
            try
            {
                if (pk_protokol != "")
                {
                    Lock = true;
                    MyDataGridView.Rows.Clear();
                    Lock = LocLock;

                    //грузим проживающих в данном жилом помещении лиц
                    sql = "select * from " + STable + " where pk_protokol = " + pk_protokol;
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
                        MyDataGridView.Rows.Add();
                        MyDataGridView.Rows[i].Cells[0].Value = dr[0].ToString();    // PC
                        MyDataGridView.Rows[i].Cells[1].Value = dr[1].ToString();    // признаки
                        MyDataGridView.Rows[i].Cells[2].Value = dr[2].ToString();    // наименование
                        MyDataGridView.Rows[i].Cells[3].Value = dr[3].ToString();    // pk_material
                        MyDataGridView.Rows[i].Cells[4].Value = dr[4].ToString();    // pk_ypakovka
                        MyDataGridView.Rows[i].Cells[7].Value = "Удалить";           // Удаление

                        i++;
                    }
                    dr.Close();

                    loadFromOtherTable(STable4, DBSHeader4, MyDataGridView, 4, 6);  //Способы упаковки вещественных доказательств
                    loadFromOtherTable(STable5, DBSHeader5, MyDataGridView, 3, 5);  //Справочник материалов, в которые упаковываю вещественные доказательства
                }


            }
            catch { MessageBox.Show("Error:1\nНе удалось загрузить список изъятого имущества (вещественных доказательств)!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LoadPeopels()  // загрузка списка других лиц
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            int i = 0;
            bool LocLock = Lock;
            try
            {
                if (pk_protokol != "")
                {
                    Lock = true;
                    dataGridView3.Rows.Clear();
                    Lock = LocLock;

                    //грузим проживающих в данном жилом помещении лиц
                    sql = "select * from " + STable6 + " where pk_protokol = " + pk_protokol;
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
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[i].Cells[0].Value = dr[0].ToString();    // PC
                        dataGridView3.Rows[i].Cells[1].Value = dr[1].ToString();    // фамилия
                        dataGridView3.Rows[i].Cells[2].Value = dr[2].ToString();    // имя
                        dataGridView3.Rows[i].Cells[3].Value = dr[3].ToString();    // отчество
                        dataGridView3.Rows[i].Cells[4].Value = dr[4].ToString();    // примечание
                        dataGridView3.Rows[i].Cells[5].Value = "Удалить";           // Удаление

                        i++;
                    }
                    dr.Close();
                }
            }
            catch  { MessageBox.Show("\nНе удалось загрузить других лиц!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LoadAppsSpend(string appTable, System.Windows.Forms.DataGridView MyDataGridView)  // загрузка В ходе осмотра проводилась / приложения к протоколу
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            int i = 0;
            bool LocLock = Lock;
            try
            {
                if (pk_protokol != "")
                {
                    Lock = true;
                    MyDataGridView.Rows.Clear();
                    Lock = LocLock;

                    //грузим проживающих в данном жилом помещении лиц
                    sql = "select * from " + appTable +" where pk_protokol = " + pk_protokol;
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
                        MyDataGridView.Rows.Add();
                        MyDataGridView.Rows[i].Cells[0].Value = dr[0].ToString();    // PC
                        MyDataGridView.Rows[i].Cells[1].Value = dr[1].ToString();    // наименование
                        MyDataGridView.Rows[i].Cells[2].Value = "Удалить";           // Удаление

                        i++;
                    }
                    dr.Close();
                }
            }
            catch { MessageBox.Show("\nНе удалось загрузить приложения к протоколу!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void SaveAppsSpend(string STable, string[] DBSHeader, List<string> delList, System.Windows.Forms.DataGridView MyDataGridView)  // сохранение вещественных доказательств(изъятого имущества)
        {
            try
            {
                string sql = "";    // строка sql запросов
                for (int i = 0; i < delList.Count; i++)
                {
                    // поэлементное удаление вещественных доказательств(изъятого имущества)
                    sql = " delete from " + STable + " where " + DBSHeader[0] + " = " + delList[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }
                delList.Clear(); // очистка списка вещественных доказательств(изъятого имущества) для удаления

                for (int i = 0; i < MyDataGridView.Rows.Count - 1; i++)
                {
                    if (MyDataGridView.Rows[i].Cells[0].Value != null)
                    {
                        //изменение
                        sql = "update " + STable + " set ";

                        if (MyDataGridView.Rows[i].Cells[1].Value != null && MyDataGridView.Rows[i].Cells[1].Value.ToString() != "")
                            sql += DBSHeader[1] + " = '" + MyDataGridView.Rows[i].Cells[1].Value.ToString() + "'";  //если ячейка заполнена

                        sql += " where " + DBSHeader[0] + " = " + MyDataGridView.Rows[i].Cells[0].Value.ToString();
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //добавление
                        sql = "";
                        sql = "insert into " + STable + "(";

                        sql += DBSHeader[1];
                        sql += ", pk_protokol";

                        sql += ") values (";

                            if (MyDataGridView.Rows[i].Cells[1].Value != null && MyDataGridView.Rows[i].Cells[1].Value.ToString() != "")
                                sql += "'" + MyDataGridView.Rows[i].Cells[1].Value.ToString() + "',";
                        sql += "'" + pk_protokol + "'";

                        sql += ")";

                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch
            {
                MessageBox.Show("Error:" + STable + "\nНе удалось сохранить приложения к протоколу!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void SavePonyatoi(TextBox surname, TextBox pname, TextBox second_name, TextBox street, TextBox house, TextBox room, TextBox pk_ponatoi) // сохранение понятого
        {
            try
            {
                string sql = "";    // строка sql запросов

                if (pk_ponatoi.Text != "")
                {
                    //изменение
                    sql = "update " + STable8 + " set ";
                    sql += DBSHeader8[1] + " = '" + surname.Text + "', ";
                    sql += DBSHeader8[2] + " = '" + pname.Text + "', ";
                    sql += DBSHeader8[3] + " = '" + second_name.Text + "', ";
                    sql += DBSHeader8[4] + " = '" + street.Text + "', ";
                    sql += DBSHeader8[5] + " = '" + house.Text + "', ";
                    sql += DBSHeader8[6] + " = '" + room.Text + "'";
                    sql += " where " + DBSHeader8[0] + " = " + pk_ponatoi.Text;
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    //добавление
                    sql = "";
                    sql = "insert into " + STable8 + "(";
                    for (int j = 1; j < 7; j++)
                    {
                        if (j == 1)
                            sql += DBSHeader8[j];
                        else
                            sql += ", " + DBSHeader8[j];
                    }

                    sql += ", pk_protokol) values (";

                    sql += "'" + surname.Text + "', ";
                    sql += "'" + pname.Text + "', ";
                    sql += "'" + second_name.Text + "', ";
                    sql += "'" + street.Text + "', ";
                    sql += "'" + house.Text + "', ";
                    sql += "'" + room.Text + "', ";
                    sql += "'" + pk_protokol + "'";
                    sql += ")";

                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();

                }

            }
            catch
            {
                MessageBox.Show("Error:" + STable8 + "\nНе удалось сохранить понятых!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPonyatoi() //загрузка понятого
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            try
            {
                if (pk_protokol != "")
                {
                    //грузим проживающих в данном жилом помещении лиц
                    sql = "select * from " + STable8 + " where pk_protokol = " + pk_protokol;
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
                        //первый понятой
                        textBox39.Text = dr[0].ToString();    // PC
                        textBox13.Text = dr[1].ToString();    // фамилия
                        textBox14.Text = dr[2].ToString();    // имя
                        textBox15.Text = dr[3].ToString();    // отчество
                        textBox16.Text = dr[4].ToString();    // улица
                        textBox17.Text = dr[5].ToString();    // дом
                        textBox18.Text = dr[6].ToString();    // квартира
                    }

                    if (dr.Read())
                    {
                        //второй понятой
                        textBox40.Text = dr[0].ToString();    // PC
                        textBox24.Text = dr[1].ToString();    // фамилия
                        textBox23.Text = dr[2].ToString();    // имя
                        textBox22.Text = dr[3].ToString();    // отчество
                        textBox21.Text = dr[4].ToString();    // улица
                        textBox20.Text = dr[5].ToString();    // дом
                        textBox19.Text = dr[6].ToString();    // квартира
                    }
                    dr.Close();
                }
            }
            catch { MessageBox.Show("\nНе удалось загрузить понятых!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LoadCrimeMan() //загрузка подозреваемого
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            try
            {
                if (pk_postanov != "")
                {
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
                    if (dr.Read())
                    {
                        textBox34.Text = dr[1].ToString();    // фамилия
                        textBox33.Text = dr[2].ToString();    // имя
                        textBox32.Text = dr[3].ToString();    // отчество

                    }
                    dr.Close();
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }

        private void LoadDataResolution() //загрузка даты постановления
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            try
            {
                if (pk_postanov != "")
                {
                    //грузим проживающих в данном жилом помещении лиц
                    sql = "select " + DBSHeader9[1] + ", " + DBSHeader9[7] + " from " + STable9 + " where pk_postanov = " + pk_postanov;
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
                        if (dr[0].ToString() != "01.01.0001 0:00:00")   //дата составления
                            dateTimePicker4.Value = Convert.ToDateTime(dr[0].ToString());
                        id_psot = dr[0].ToString();
                    }
                    dr.Close();
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
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
            catch
            { }
        }

        private void LoadData()
        {
            LoadProtocol();
            LoadVeshDok(STable3, dataGridView2);
            LoadPeopels();
            LoadPonyatoi();
            LoadDevise();
            LoadAppsSpend("spend", dataGridView4);                                      //В ходе осмотра проводилась
            LoadAppsSpend("apps", dataGridView5);                                       //К протоколу прилагаются
            loadFromOtherFormOneItem(STable0, DBSHeader0, textBox2, textBox1);          // загрузка города
            loadFromOtherFormOneItem(STable1, DBSHeader1, textBox7, textBox6);          // загрузка погоды
            loadFromOtherFormOneItem(STable1, DBSHeader1, textBox7, textBox38);         // загрузка погоды
            loadFromOtherFormOneItem(STable2, DBSHeader2, textBox8, textBox9);          // загрузка освещённости
            loadFromOtherFormOneItem(STable2, DBSHeader2, textBox8, textBox36);         // загрузка освещённости

            loadFromOtherFormMultiItems(STable7, DBSHeader7, textBox11, textBox12);     // загрузка уполномоченного
            loadFromOtherFormMultiItems(STable12, DBSHeader12, textBox26, textBox25);   // загрузка специалиста


            switch (id_prot)
            {
                case "1":
                    {
                        // протокол осмотра места происшествия
                        label48.Visible = textBox35.Visible = panel1.Visible = panel2.Visible = panel3.Visible = panel4.Visible = panel5.Visible = panel6.Visible = panel7.Visible = panel8.Visible = false;
                    } break;
                case "2":
                    {
                        // протокол осмотра трупа
                        panel2.Visible = panel3.Visible = panel4.Visible = panel5.Visible = panel6.Visible = panel7.Visible = panel8.Visible = false;
                    } break;
                case "3":
                    {
                        // протокол личного обыска
                        label2.Text = "Обыск начат в";
                        label3.Text = "Обыск окончен в";
                        label6.Text = "Точное место проведения личного обыска";
                        label48.Visible = textBox35.Visible = panel1.Visible = panel3.Visible = panel4.Visible = panel7.Visible = panel8.Visible = false;
                        LoadCrimeMan();
                        LoadDataResolution();
                    } break;
                case "4":
                    {
                        // протокол осмотра местности, жилища, иного помещения
                        label6.Text = "Адрес проведения осмотра";
                        label48.Visible = textBox35.Visible = panel1.Visible = panel3.Visible = panel4.Visible = panel5.Visible = panel6.Visible = false;
                        LoadDataResolution();
                    } break;
                case "5":
                    {
                        // протокол обыска (выемки)
                        label2.Text = "Обыск начат в";
                        label3.Text = "Обыск окончен в";
                        label6.Text = "Место проведения обыска (где именно)";
                        label48.Visible = textBox35.Visible = panel5.Visible = panel6.Visible = panel7.Visible = panel8.Visible = false;
                        LoadDataResolution();
                    } break;

            }
        }

        private void SaveDevise()   // сохранение списка тех средств
        {
            try
            {
                string sql = "";    // строка sql запросов
                for (int i = 0; i < delDevise.Count; i++)
                {
                    // поэлементное удаление тех средств
                    sql = " delete from " + STable10 + " where " + DBSHeader10[0] + " = " + delDevise[i];
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }
                delDevise.Clear(); // очистка списка вещественных доказательств(изъятого имущества) для удаления

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        //изменение
                        sql = "update " + STable10 + " set ";
                        sql += DBSHeader10[1] + " = '" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
                        sql += " where " + DBSHeader10[0] + " = " + dataGridView1.Rows[i].Cells[0].Value.ToString();
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //добавление
                        sql = "";
                        sql = "insert into " + STable10 + "(";
                        for (int j = 1; j < 3; j++)
                        {
                            if (j == 1)
                                sql += DBSHeader10[j];
                            else
                                sql += ", " + DBSHeader10[j];
                        }

                        sql += ") values (";

                        if (dataGridView1.Rows[i].Cells[1].Value != null && dataGridView1.Rows[i].Cells[1].Value.ToString() != "")
                            sql += "'" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
                        else
                            sql += "NULL";

                        sql += ", '" + pk_protokol + "'";

                        sql += ")";

                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch
            {
                MessageBox.Show("\nНе удалось сохранить технических средств!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDevise()   // загрузка списка тех средств
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            //проверка подключения к бд
            try
            {
                if (pk_protokol != "")
                {
                    if (mycon.State == ConnectionState.Open)
                    {
                        string sql;
                        //генерация sql запроса, для отображения данных из БД на форму
                        sql = "";
                        sql += "select * from " + STable10 + " where pk_protokol = " + pk_protokol;

                        cmd = new MySqlCommand(sql, mycon);

                        //вополнение запроса
                        cmd.ExecuteNonQuery();

                        //выборка по запросу
                        da = new MySqlDataAdapter(cmd);
                        dr = cmd.ExecuteReader();

                        //запролене dataGridView1
                        int i = 0;
                        dataGridView1.Rows.Clear();
                        while (dr.Read())
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();    //PC
                            dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();    //фамилия
                            dataGridView1.Rows[i].Cells[3].Value = "Удалить";           //Удаление
                            i++;
                        }
                        dr.Close();

                        // название тех средства (выборка информации из другой таблице)
                        for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
                        {
                            sql = "select " + DBSHeader11[1];

                            sql += " from " + STable11 + " where " + DBSHeader11[0] + " = " + dataGridView1.Rows[ii].Cells[1].Value.ToString();

                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();
                            da = new MySqlDataAdapter(cmd);
                            dr = cmd.ExecuteReader();
                            if (dr.Read())
                                dataGridView1.Rows[ii].Cells[2].Value = dr[0].ToString();
                            dr.Close();
                        }

                        // иправление бага WS
                        rSize = (rSize > 0) ? -1 : 1;
                        this.ClientSize = new System.Drawing.Size(this.ClientSize.Width + rSize, this.ClientSize.Height);
                    }
                    else
                        MessageBox.Show("Нет подключениея к базе данных!");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при загрузке списка технических средств!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveProtocol() //сохранение протокола
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;

            try
            {
                if (pk_protokol == "")
                {
                    // добавление
                    string sql = "";

                    sql = "insert into " + table + "(";

                    for (int i = 1; i < DBHeader.Length; i++)
                    {
                        if (i == 1)
                            sql += DBHeader[i];
                        else
                            sql += ", " + DBHeader[i];
                    }

                    sql += ") values (";

                    sql += "STR_TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";       //data_sostav
                    sql += "STR_TO_DATE('" + dateTimePicker2.Value.ToLongTimeString() + "', '%k:%i:%s')" + ", ";        //Vremya_nachala
                    sql += "STR_TO_DATE('" + dateTimePicker3.Value.ToLongTimeString() + "', '%k:%i:%s')" + ", ";        //vremya_okonch
                    sql += "'" + textBox4.Text + "', ";                                                                 //mesto_peibitiya
                    sql += "'" + textBox3.Text + "', ";                                                                 //coobshenie
                    sql += "'" + textBox5.Text + "', ";                                                                 //predmet_osmotra
                    sql += "'" + id_prot + "', ";                                                                       //id_prot
                    sql += "'" + textBox28.Text + "', ";                                                                //Zayavleniya
                    sql += "'" + textBox27.Text + "', ";                                                                //zamechaniya
                    sql += "'" + textBox37.Text + "', ";                                                                //I_look
                    if(radioButton1.Checked)
                        sql += "'0', ";                                                                                 //Sposob_izyatiya
                    else
                        sql += "'1', ";                                                                                 //Sposob_izyatiya
                    sql += "'" + textBox35.Text + "', ";                                                                //temperature
                    sql += "'" + textBox29.Text + "', ";                                                                //dead_go
                    sql += "'" + textBox30.Text + "', ";                                                                //cel_obiska
                    sql += "'" + textBox31.Text + "', ";                                                                //otdali

                    if (textBox2.Text != "")
                        sql += "'" + textBox2.Text + "', ";                                                                 //pk_gorod
                    else
                        sql += "NULL, ";

                    if (textBox7.Text != "")
                        sql += "'" + textBox7.Text + "', ";                                                                 //pk_pogoda
                    else
                        sql += "NULL, ";

                    if (textBox8.Text != "")
                        sql += "'" + textBox8.Text + "', ";                                                                 //pk_osveshennost
                    else
                        sql += "NULL, ";

                    if (textBox26.Text != "")
                        sql += "'" + textBox26.Text + "', ";                                                                 //pk_spec
                    else
                        sql += "NULL, ";

                    if (textBox11.Text != "")
                        sql += "'" + textBox11.Text + "', ";                                                                 //pk_polise
                    else
                        sql += "NULL, ";

                    if (pk_postanov == "")
                        sql += "NULL, ";                                                                                //pk_postanov
                    else
                        sql += "'" + pk_postanov + "', ";

                    if (PK_Dela == "")
                        sql += "NULL, ";                                                                                //PK_Dela
                    else
                        sql += "'" + PK_Dela + "', ";

                    sql += "'" + textBox10.Text + "'";


                    sql += ")";

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
                        pk_protokol = dr[0].ToString();
                    }
                    else
                        MessageBox.Show("Error:3\nСохранение не удалось!\nНе удалось получить первичный ключ!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    dr.Close();

                }
                else
                {
                    string sql = "";

                    //редактирование

                    sql = "";
                    sql = "update " + table + " set ";
                    sql += DBHeader[1] + " = STR_TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "', '%d.%m.%Y')" + ", ";       //data_sostav
                    sql += DBHeader[2] + " = STR_TO_DATE('" + dateTimePicker2.Value.ToLongTimeString() + "', '%k:%i:%s')" + ", ";        //Vremya_nachala
                    sql += DBHeader[3] + " = STR_TO_DATE('" + dateTimePicker3.Value.ToLongTimeString() + "', '%k:%i:%s')" + ", ";        //vremya_okonch
                    sql += DBHeader[4] + " = '" + textBox4.Text + "', ";                                                                 //mesto_peibitiya
                    sql += DBHeader[5] + " = '" + textBox3.Text + "', ";                                                                 //coobshenie
                    sql += DBHeader[6] + " = '" + textBox5.Text + "', ";                                                                 //predmet_osmotra
                    sql += DBHeader[7] + " = '" + id_prot + "', ";                                                                       //id_prot
                    sql += DBHeader[8] + " = '" + textBox28.Text + "', ";                                                                //Zayavleniya
                    sql += DBHeader[9] + " = '" + textBox27.Text + "', ";                                                                //zamechaniya
                    sql += DBHeader[10] + " = '" + textBox37.Text + "', ";                                                                //I_look
                    if (radioButton1.Checked)
                        sql += DBHeader[11] + " = '0', ";                                                                                 //Sposob_izyatiya
                    else
                        sql += DBHeader[11] + " = '1', ";                                                                                 //Sposob_izyatiya
                    sql += DBHeader[12] + " = '" + textBox35.Text + "', ";                                                                //temperature
                    sql += DBHeader[13] + " = '" + textBox29.Text + "', ";                                                                //dead_go
                    sql += DBHeader[14] + " = '" + textBox30.Text + "', ";                                                                //cel_obiska
                    sql += DBHeader[15] + " = '" + textBox31.Text + "', ";                                                                //otdali

                    if (textBox2.Text != "")
                        sql += DBHeader[16] + " = '" + textBox2.Text + "', ";                                                                 //pk_gorod
                    else
                        sql += DBHeader[16] + " = NULL,";

                    if (textBox7.Text != "")
                        sql += DBHeader[17] + " = '" + textBox7.Text + "', ";                                                                 //pk_pogoda
                    else
                        sql += DBHeader[17] + " = NULL,";

                    if (textBox8.Text != "")
                        sql += DBHeader[18] + " = '" + textBox8.Text + "', ";                                                                 //pk_osveshennost
                    else
                        sql += DBHeader[18] + " = NULL,";

                    if (textBox26.Text != "")
                        sql += DBHeader[19] + " = '" + textBox26.Text + "', ";                                                                 //pk_spec
                    else
                        sql += DBHeader[19] + " = NULL,";

                    if (textBox11.Text != "")
                        sql += DBHeader[20] + " = '" + textBox11.Text + "', ";                                                                 //pk_polise
                    else
                        sql += DBHeader[20] + " = NULL,";

                    if (pk_postanov == "")
                        sql += DBHeader[21] + " = NULL, ";                                                                                //pk_postanov
                    else
                        sql += DBHeader[21] + " = '" + pk_postanov + "', ";

                    if (PK_Dela == "")
                        sql += DBHeader[22] + " = NULL, ";                                                                                //PK_Dela
                    else
                        sql += DBHeader[22] + " = '" + PK_Dela + "', ";

                    sql += DBHeader[23] + " = '" + textBox10.Text + "'"; 

                    sql += " where pk_protokol = " + pk_protokol;

                    // внесение информации в БД
                    cmd = new MySqlCommand(sql, mycon);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                MessageBox.Show("Error:4\nСохранение не удалось!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProtocol()   // загрузка протокола
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            //проверка подключения к бд
            try
            {
                if (pk_protokol != "")
                {
                    if (mycon.State == ConnectionState.Open)
                    {
                        string sql;
                        //генерация sql запроса, для отображения данных из БД на форму
                        sql = "";
                        sql += "select * from " + table + " where pk_protokol = " + pk_protokol;
                        cmd = new MySqlCommand(sql, mycon);
                        //вополнение запроса
                        cmd.ExecuteNonQuery();
                        //выборка по запросу
                        da = new MySqlDataAdapter(cmd);
                        dr = cmd.ExecuteReader();
                        //запролене протокола
                        if (dr.Read())
                        {
                            dateTimePicker1.Value = Convert.ToDateTime(dr[1].ToString());   //data_sostav
                            dateTimePicker2.Value = Convert.ToDateTime(dr[2].ToString());   //Vremya_nachala
                            dateTimePicker3.Value = Convert.ToDateTime(dr[3].ToString());   //vremya_okonch
                            textBox4.Text = dr[4].ToString();                               //mesto_peibitiya
                            textBox3.Text = dr[5].ToString();                               //coobshenie
                            textBox5.Text = dr[6].ToString();                               //predmet_osmotra
                            id_prot = dr[7].ToString();                                     //id_prot
                            textBox28.Text = dr[8].ToString();                              //Zayavleniya
                            textBox27.Text = dr[9].ToString();                              //zamechaniya

                            textBox37.Text = dr[10].ToString();                               //I_look

                            if (dr[11].ToString() == "0")                                    //Sposob_izyatiya
                                radioButton1.Checked = radioButton4.Checked = true;
                            else
                                radioButton3.Checked = radioButton2.Checked = true;

                            textBox35.Text = dr[12].ToString();                               //temperature
                            textBox29.Text = dr[13].ToString();                               //dead_go
                            textBox30.Text = dr[14].ToString();                               //cel_obiska
                            textBox31.Text = dr[15].ToString();                               //otdali
                            textBox2.Text = dr[16].ToString();                                //pk_gorod
                            textBox7.Text = dr[17].ToString();                                //pk_pogoda
                            textBox8.Text = dr[18].ToString();                               //pk_osveshennost
                            textBox26.Text = dr[19].ToString();                               //pk_spec
                            textBox11.Text = dr[20].ToString();                               //pk_polise
                            pk_postanov = dr[21].ToString();                               //pk_postanov
                            PK_Dela = dr[22].ToString();                               //PK_Dela
                            textBox10.Text = dr[23].ToString(); 
                        }
                        dr.Close();
                    }
                    else
                        MessageBox.Show("Нет подключениея к базе данных!");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при загрузке данных!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       

        private void loadFromOtherTable(string STable_, string[] DBSHeader_, System.Windows.Forms.DataGridView MyDataGridView, int from, int to)
        {
            try
            {
                if (mycon.State != ConnectionState.Open)
                {
                    MessageBox.Show("Нет подключениея к базе данных!");
                    throw new Exception();
                }
                MySqlDataAdapter da;
                MySqlDataReader dr;
                string sql;
                for (int i = 0; i < MyDataGridView.Rows.Count - 1; i++)
                {
                    if (MyDataGridView.Rows[i].Cells[from].Value != null && MyDataGridView.Rows[i].Cells[from].Value.ToString() != "")
                    {
                        sql = "select ";
                        for (int j = 1; j < DBSHeader_.Length; j++)
                        {
                            if (j == 1)
                                sql += DBSHeader_[j];
                            else
                                sql += ", " + DBSHeader_[j];
                        }
                        // генерация sql комманды
                        sql += " from " + STable_ + " where " + DBSHeader_[0] + " = " + MyDataGridView.Rows[i].Cells[from].Value.ToString();

                        //получение комманды и коннекта
                        cmd = new MySqlCommand(sql, mycon);

                        //вополнение запроса
                        cmd.ExecuteNonQuery();
                        da = new MySqlDataAdapter(cmd);

                        //получение выборки
                        dr = cmd.ExecuteReader();

                        // заполнения поля 
                        if (dr.Read())
                            MyDataGridView.Rows[i].Cells[to].Value = dr[0].ToString();
                        dr.Close();
                    }
                    else
                        MyDataGridView.Rows[i].Cells[to].Value = "";
                }
            }
            catch (Exception e)
            { MessageBox.Show(e.ToString() + "Error in table " + STable_); }
        }

        private void SaveData()
        {
            SaveProtocol();
            LoadProtocol();

            SaveTables(STable3, DBSHeader3, "pk_protokol", pk_protokol, delVeshDokList, dataGridView2, 5);   //сохранение вещественных дказательств (изьятого имущетсва)
            LoadVeshDok(STable3, dataGridView2);

            SaveTables(STable6, DBSHeader6, "pk_protokol", pk_protokol, delPeoples, dataGridView3, 5);    //сохранение иных (других) лиц
            LoadPeopels();

            SaveAppsSpend("spend", new string[] { "pk_spend", "nazvanie" }, delSpend, dataGridView4);     //сохранение "В ходе осмотра проводилась"
            LoadAppsSpend("spend", dataGridView4);                                                        //В ходе осмотра проводилась

            SaveAppsSpend("apps", new string[] { "pk_apps", "nazvanie" }, delApps, dataGridView5);        //сохранение "К протоколу прилагаются"
            LoadAppsSpend("apps", dataGridView5);                                                         //К протоколу прилагаются

            SavePonyatoi(textBox13, textBox14, textBox15, textBox16, textBox17, textBox18, textBox39);
            SavePonyatoi(textBox24, textBox23, textBox22, textBox21, textBox20, textBox19, textBox40);

            SaveDevise();
            LoadDevise();

            FileSave();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex < dataGridView2.Rows.Count - 1 && e.RowIndex != -1)
            {
                //удаление
                DialogResult del = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dataGridView2.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        dataGridView2.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        delVeshDokList.Add(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                        dataGridView2.Rows.RemoveAt(e.RowIndex);
                    }
                    FileChange();
                }
            }

            if (e.ColumnIndex == 5 && e.RowIndex != -1)
            {
                //материал упаковки
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник материалов упаковки", "spravochnik_materialov", new string[] { "Маериал", "Идентификационный номер" }, new string[] { "pk_material", "material", "id_number" });
                f.ShowDialog();

                dataGridView2.Rows[e.RowIndex].Cells[3].Value = f.PC_rezult;
                loadFromOtherTable(STable5, DBSHeader5, dataGridView2, 3, 5);  //Справочник материалов, в которые упаковываю вещественные доказательства
                FileChange();
            }

            if (e.ColumnIndex == 6 && e.RowIndex != -1)
            {
                //способ упаковки
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник способов упаковки вещественных доказательств", "ypakovka", new string[] { "Способ" }, new string[] { "pk_ypakovka", "nazvanie" });
                f.ShowDialog();

                dataGridView2.Rows[e.RowIndex].Cells[4].Value = f.PC_rezult;
                loadFromOtherTable(STable4, DBSHeader4, dataGridView2, 4, 6);  //Справочник материалов, в которые упаковываю вещественные доказательства
                FileChange();
            }
        }

        private void Protocol_Shown(object sender, EventArgs e)
        {
            //загрузка информации
            LoadData();
            FileSave();
        }

        private void dataGridView3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > 0)
                dataGridView3.Rows[e.RowIndex - 1].Cells[5].Value = "Удалить";           // Удаление
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > 0)
                dataGridView2.Rows[e.RowIndex - 1].Cells[7].Value = "Удалить";           // Удаление
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex < dataGridView3.Rows.Count - 1 && e.RowIndex != -1)
            {
                DialogResult del = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dataGridView3.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        // если в базе ещё нет данного элемента
                        // то удаляем только строку
                        dataGridView3.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        // если элемент в базе есть, 
                        // то удаляем строку и запись в бд
                        delPeoples.Add(dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());
                        dataGridView3.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void Protocol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 3 && e.KeyChar != 22 && e.KeyChar != 26)
                e.Handled = true;
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //Место составления
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" });
            f.ShowDialog();
            textBox1.Text = f.Rezult;
            textBox2.Text = f.PC_rezult;
        }

        private void textBox6_MouseClick(object sender, MouseEventArgs e)
        {
            // погода
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник погодных условий", "spravochnik_pogodi", new string[] { "Погода" }, new string[] { "pk_pogoda", "nazvanie" });
            f.ShowDialog();
            textBox38.Text = textBox6.Text = f.Rezult;
            textBox7.Text = f.PC_rezult;
        }

        private void textBox9_MouseClick(object sender, MouseEventArgs e)
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник освещённости", "spravochnik_osveshennosti", new string[] { "Освещённость" }, new string[] { "pk_osveshennost", "nazvanie" });
            f.ShowDialog();
            textBox36.Text = textBox9.Text = f.Rezult;
            textBox8.Text = f.PC_rezult;
        }

        private void textBox25_MouseClick(object sender, MouseEventArgs e)
        {
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, true, "Справочник специалистов", "specialist", new string[] { "Фамилия", "Имя", "Отчество", "Область специализации" }, new string[] { "pk_spec", "surname", "Pname", "second_name", "pk_special" }, "spravochnik_oblastei_spec", new string[] { "pk_special", "nazvanie" });
            f.ShowDialog();
            textBox25.Text = f.Rezult;
            textBox26.Text = f.PC_rezult;
        }

        private void textBox12_MouseClick(object sender, MouseEventArgs e)
        {
            AddOper f = new AddOper(User, Password, Database, Ip, true, "Справочник уполномоченных", "polise", new string[] { "Табельный номер", "Фамилия", "Имя", "Отчество", "Звание", "Должность", "Чин" }, new string[] { "pk_polise", "id_number", "surname", "Pname", "second_name", "pk_zvanie", "pk_dolgnost", "pk_chin" }, "spravochnik_zvanii", new string[] { "pk_zvanie", "nazvanie" }, "spravochnik_dolgnostei", new string[] { "pk_dolgnost", "nazvanie" }, "chin", new string[] { "pk_chin", "nazvanie" });
            f.ShowDialog();
            textBox12.Text = f.Rezult;
            textBox11.Text = f.PC_rezult;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool isNull = false;
            if (e.ColumnIndex == 3 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
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
                        delDevise.Add(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }

                    FileChange();
                }
            }

            if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[1].Value == null)
                    isNull = true;
                //тех средство
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник технических средств", "spravochnik_tex_sredstv", new string[] { "Техническое средство", "Идентификационный номер" }, new string[] { "pk_tex_sredstvo", "nazvanie", "id_number" });
                f.ShowDialog();

                if (f.PC_rezult == null)
                    isNull = false;


                if (isNull)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "Удалить";
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = f.PC_rezult;
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = f.Rezult;
                    FileSave();
                }

                if(dataGridView1.Rows[e.RowIndex].Cells[1].Value != null && f.PC_rezult != null)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "Удалить";
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = f.PC_rezult;
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = f.Rezult;
                    FileChange();
                }
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = radioButton3.Checked;
            FileChange();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = radioButton4.Checked;
            FileChange();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = radioButton1.Checked;
            FileChange();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton3.Checked = radioButton2.Checked;
            FileChange();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                MySqlDataAdapter da;
                MySqlDataReader dr;
                string sql = "";

                //грузим проживающих в данном жилом помещении лиц
                sql = "select Nomer_materiala, Nomer_dela from delo where PK_Dela = " + PK_Dela;
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
                    Resolution r;
                    if(dr[1].ToString() != "")
                        r = new Resolution(User, Password, Database, Ip, dr[1].ToString(), id_psot, pk_postanov, pk_protokol);  // Nomer_dela
                    else
                        r = new Resolution(User, Password, Database, Ip, dr[0].ToString(), id_psot, pk_postanov, pk_protokol);  // Nomer_materiala
                    r.ShowDialog();
                }
                dr.Close();
            }
            catch {
                Resolution r = new Resolution(User, Password, Database, Ip, PK_Dela, id_psot, pk_postanov, pk_protokol);
                r.ShowDialog();
            }

            

            switch (id_prot)
            {
                case "3":
                    {
                        // протокол личного обыска
                        LoadCrimeMan();
                        LoadDataResolution();
                    } break;
                case "4":
                    {
                        // протокол осмотра местности, жилища, иного помещения
                        LoadDataResolution();
                    } break;
                case "5":
                    {
                        // протокол обыска (выемки)
                        LoadDataResolution();
                    } break;
            }
        }

        private void Protocol_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (pk_postanov != "" && pk_protokol == "")
            {
                DialogResult FClose = MessageBox.Show("Все не сохранённые данные будут потеряны!\nТакже не будет сохранено постановление!", "Сохранить изменения перед выходом?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (FClose == DialogResult.Yes)
                {
                    SaveData();
                    e.Cancel = false;
                }
                else
                    if (FClose == DialogResult.No)
                    {
                        try
                        {
                            //удаляем постановление без протокола
                            string sql;
                            sql = "delete from " + STable9;
                            sql += " where pk_postanov = " + pk_postanov;
                            //получение комманды и коннекта
                            cmd = new MySqlCommand(sql, mycon);
                            //вополнение запроса
                            cmd.ExecuteNonQuery();
                        }
                        catch { }

                        e.Cancel = false;
                    }
                    else
                        e.Cancel = true;
            }

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

        private void FileChange()   //файл изменили, но не сохранили
        {
            switch (id_prot)
            {
                case "1":
                    {
                        this.Text = "Протокол осмотра мета происшествия";
                    } break;
                case "2":
                    {
                        this.Text = "Протокол осмотра трупа";
                    } break;
                case "3":
                    {
                        this.Text = "Протокол личного обыска";
                    } break;
                case "4":
                    {
                        this.Text = "Протокол осмотра местности, жилища, иного помещения";
                    } break;
                case "5":
                    {
                        this.Text = "Протокол обыска (выемки)";
                    } break;
            }
            if (pk_protokol == "")
                this.Text += ": Новый документ *";
            else
                this.Text += ": *";
            change = true;
        }

        private void FileSave()     //файл изменили и сохранили
        {
            switch (id_prot)
            {
                case "1":
                    {
                        this.Text = "Протокол осмотра мета происшествия";
                    } break;
                case "2":
                    {
                        this.Text = "Протокол осмотра трупа";
                    } break;
                case "3":
                    {
                        this.Text = "Протокол личного обыска";
                    } break;
                case "4":
                    {
                        this.Text = "Протокол осмотра местности, жилища, иного помещения";
                    } break;
                case "5":
                    {
                        this.Text = "Протокол обыска (выемки)";
                    } break;
            }
            if (pk_protokol == "")
                this.Text += ": Новый документ";
            else
                this.Text += "";
            change = false;
        }

        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            FileChange();
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            FileChange();
        }

        private void dataGridView4_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > 0)
                dataGridView4.Rows[e.RowIndex - 1].Cells[2].Value = "Удалить";           // Удаление
            FileChange();

        }

        private void dataGridView5_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > 0)
                dataGridView5.Rows[e.RowIndex - 1].Cells[2].Value = "Удалить";           // Удаление
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex < dataGridView4.Rows.Count - 1 && e.RowIndex != -1)
            {
                DialogResult del = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dataGridView4.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        // если в базе ещё нет данного элемента
                        // то удаляем только строку
                        dataGridView4.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        // если элемент в базе есть, 
                        // то удаляем строку и запись в бд
                        delSpend.Add(dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString());
                        dataGridView4.Rows.RemoveAt(e.RowIndex);
                    }
                    FileChange();
                }
            }
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex < dataGridView5.Rows.Count - 1 && e.RowIndex != -1)
            {
                DialogResult del = MessageBox.Show("Вы действительно хотите удалить данный элемент?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dataGridView5.Rows[e.RowIndex].Cells[0].Value == null)
                    {
                        // если в базе ещё нет данного элемента
                        // то удаляем только строку
                        dataGridView5.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        // если элемент в базе есть, 
                        // то удаляем строку и запись в бд
                        delApps.Add(dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString());
                        dataGridView5.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            FileChange();
        }

    }
}
