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
        string id_prot;     // номер варианта протокола
        string pk_postanov; // первичный ключ постановления
        string pk_protokol; // пномер протокола

        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации (уполномоченный)
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации (города)
        string[] DBSHeader2; // название полей в таблице2 для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (Вещественное доказательство)
        string[] DBSHeader3; // название полей в таблице3 для sql запросов, для сопудствующей информации

        string STable4;      // ТАБЛИЦА4 БАЗЫ ДАННЫХ для сопудствующей информации (Способы упаковки вещественных доказательств)
        string[] DBSHeader4; // название полей в таблице3 для sql запросов, для сопудствующей информации

        string STable5;      // ТАБЛИЦА5 БАЗЫ ДАННЫХ для сопудствующей информации (Справочник материалов, в которые упаковываю вещественные доказательства (полиэтилен, бумага и т.д.))
        string[] DBSHeader5; // название полей в таблице5 для sql запросов, для сопудствующей информации

        string STable6;      // ТАБЛИЦА6 БАЗЫ ДАННЫХ для сопудствующей информации (обыскиваемый)
        string[] DBSHeader6; // название полей в таблице6 для sql запросов, для сопудствующей информации

        string STable7;      // ТАБЛИЦА7 БАЗЫ ДАННЫХ для сопудствующей информации (проц. положение)

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

            table = "postanovlenie";
            DBHeader = new string[] { "Obosnovanie", "DateOfCreate", "plase", "street", "house", "room", "id_post", "pk_polise", "pk_gorod", "pk_prosecutor1", "pk_court1", "pk_prosecutor2", "pk_court2", "pk_dolgnost" };
            STable1 = "polise";
            DBSHeader1 = new string[] { "pk_polise", "surname", "Pname", "second_name" };
            STable2 = "spravochnik_gorodov";
            DBSHeader2 = new string[] { "pk_gorod", "nazvanie", "id_number" };
            STable3 = "vesh_dok";
            DBSHeader3 = new string[] { "pk_vesh_dok", "priznaki", "naiminovanie", "pk_material", "pk_ypakovka", "pk_protokol" };
            STable4 = "ypakovka";
            DBSHeader4 = new string[] { "pk_ypakovka", "nazvanie" };
            STable5 = "spravochnik_materialov";
            DBSHeader5 = new string[] { "pk_material", "material", "id_number" };

            STable6 = "peoples";
            DBSHeader6 = new string[] { "PK_people", "surname", "Pname", "second_name", "primichanie", "mystate", "pk_postanov", "pk_protokol", "pk_pol" };

            STable7 = "sp_pro_pol";

            delVeshDokList = new List<string>();

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

        private void SaveVeshDok(string STable, string[] DBSHeader, List<string> delList, System.Windows.Forms.DataGridView MyDataGridView, int MaxColums)  // сохранение вещественных доказательств(изъятого имущества)
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

                        sql += ", " + DBSHeader[MaxColums];

                        sql += ") values (";

                        for (int j = 1; j < MaxColums; j++)
                        {
                            if (MyDataGridView.Rows[i].Cells[j].Value != null && MyDataGridView.Rows[i].Cells[j].Value.ToString() != "")
                                sql += "'" + MyDataGridView.Rows[i].Cells[j].Value.ToString() + "',";
                            else
                                sql += "NULL,";
                        }

                        sql += pk_protokol + ")";

                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch
            {
                MessageBox.Show("Error:1\nНе удалось сохранить список изъятого имущества (вещественных доказательств)!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch { MessageBox.Show("Error:1\nНе удалось загрузить список изъятого имущества (вещественных доказательств)!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LoadData()
        {
            LoadVeshDok(STable3, dataGridView2);
        }

        private void loadFromOtherTable(string STable_, string[] DBSHeader_, System.Windows.Forms.DataGridView MyDataGridView,int from, int to)
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

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveVeshDok(STable3, DBSHeader3, delVeshDokList,dataGridView2,5);
            LoadVeshDok(STable3, dataGridView2);
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex < dataGridView2.Rows.Count - 1)
            {
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
                }
            }

            if (e.ColumnIndex == 5)
            {
                //материал упаковки
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник материалов упаковки", "spravochnik_materialov", new string[] { "Маериал", "Идентификационный номер" }, new string[] { "pk_material", "material", "id_number" });
                f.ShowDialog();

                dataGridView2.Rows[e.RowIndex].Cells[3].Value = f.PC_rezult;
                loadFromOtherTable(STable5, DBSHeader5, dataGridView2, 3, 5);  //Справочник материалов, в которые упаковываю вещественные доказательства
            }

            if (e.ColumnIndex == 6)
            {
                //способ упаковки
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник способов упаковки вещественных доказательств", "ypakovka", new string[] { "Способ" }, new string[] { "pk_ypakovka", "nazvanie" });
                f.ShowDialog();

                dataGridView2.Rows[e.RowIndex].Cells[4].Value = f.PC_rezult;
                loadFromOtherTable(STable4, DBSHeader4, dataGridView2, 4, 6);  //Справочник материалов, в которые упаковываю вещественные доказательства
            }
        }

        private void Protocol_Shown(object sender, EventArgs e)
        {
            //загрузка информации
            LoadData();
        }

        


    }
}
