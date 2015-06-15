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
    public partial class AddOper : Form
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
        string FormName;    // название формы
        public string PC_rezult;   // "возвращаемое" формаой значение первичного ключа
        public string Rezult;   // "возвращаемое" формаой значение полей
        bool IsSelect;      // true - форма вызвана для выбора элемента

        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader2; // название полей в таблице2 для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader3; // название полей в таблице3 для sql запросов, для сопудствующей информации

        public AddOper()
        {
            InitializeComponent();
        }

        public AddOper(string _user, string _pass, string _database, string _ip, bool _Select, string _FormName, string _Table, string[] _HeaderText, string[] _DBHeader, string _STable1, string[] _DBSHeader1, string _STable2, string[] _DBSHeader2, string _STable3, string[] _DBSHeader3)
        {
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            table = _Table;
            HeaderText = _HeaderText;
            DBHeader = _DBHeader;
            FormName = _FormName;
            IsSelect = _Select;
            STable1 = _STable1;
            DBSHeader1 = _DBSHeader1;
            STable2 = _STable2;
            DBSHeader2 = _DBSHeader2;
            STable3 = _STable3;
            DBSHeader3 = _DBSHeader3;
            InitializeComponent();
        }

        void LoadData()
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
                    sql += "select * from " + table + " where ";
                    sql += DBHeader[1] + " like '%" + textBox1.Text + "%' ";
                    sql += "and " + DBHeader[2] + " like '%" + textBox2.Text + "%' ";
                    sql += "and " + DBHeader[3] + " like '%" + textBox3.Text + "%' ";
                    sql += "and " + DBHeader[4] + " like '%" + textBox4.Text + "%' ";

                    if (textBox8.Text != "")
                        sql += "and " + DBHeader[5] + " = '" + textBox8.Text + "' ";
                    if (textBox9.Text != "")
                        sql += "and " + DBHeader[6] + " = '" + textBox9.Text + "' ";
                    if (textBox10.Text != "")
                        sql += "and " + DBHeader[7] + " = '" + textBox10.Text + "' ";

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
                    sql += "select * from " + table + " where ";
                    sql += DBHeader[1] + " like '%" + textBox1.Text + "%' ";
                    sql += "and " + DBHeader[2] + " like '%" + textBox2.Text + "%' ";
                    sql += "and " + DBHeader[3] + " like '%" + textBox3.Text + "%' ";
                    sql += "and " + DBHeader[4] + " like '%" + textBox4.Text + "%' ";

                    if (textBox8.Text != "")
                        sql += "and " + DBHeader[5] + " = '" + textBox8.Text + "' ";
                    if (textBox9.Text != "")
                        sql += "and " + DBHeader[6] + " = '" + textBox9.Text + "' ";
                    if (textBox10.Text != "")
                        sql += "and " + DBHeader[7] + " = '" + textBox10.Text + "' ";

                    sql += "limit " + lastIndex.ToString() + ", " + comboBox1.SelectedItem.ToString();

                    cmd = new MySqlCommand(sql, mycon);

                    //вополнение запроса
                    cmd.ExecuteNonQuery();

                    //выборка по запросу
                    da = new MySqlDataAdapter(cmd);
                    dr = cmd.ExecuteReader();


                    toolStripProgressBar1.Visible = true;
                    toolStripProgressBar1.Value = 0;
                    toolStripStatusLabel1.Text = "Записей в базе: " + count;

                    //запролене dataGridView1
                    int i = 0;
                    dataGridView1.Rows.Clear();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();    //PC
                        dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();    //таб. номер
                        dataGridView1.Rows[i].Cells[2].Value = dr[2].ToString();    //фамилия
                        dataGridView1.Rows[i].Cells[3].Value = dr[3].ToString();    //имя
                        dataGridView1.Rows[i].Cells[4].Value = dr[4].ToString();    //отчество
                        dataGridView1.Rows[i].Cells[5].Value = dr[5].ToString();    //id_zvanie
                        dataGridView1.Rows[i].Cells[6].Value = dr[6].ToString();    //id_dolgnost
                        dataGridView1.Rows[i].Cells[7].Value = dr[7].ToString();    //id_chin

                        dataGridView1.Rows[i].Cells[11].Value = "Удалить";           //Удаление
                        dataGridView1.Rows[i].Cells[12].Value = "Выбрать";           //Выбор

                        i++;
                        toolStripProgressBar1.Value = (i * 100) / count;
                    }
                    dr.Close();

                    // звание
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
                        sql += " from " + STable1 + " where " + DBSHeader1[0] + " = " + dataGridView1.Rows[ii].Cells[5].Value.ToString();

                        //получение комманды и коннекта
                        cmd = new MySqlCommand(sql, mycon);

                        //вополнение запроса
                        cmd.ExecuteNonQuery();
                        da = new MySqlDataAdapter(cmd);

                        //получение выборки
                        dr = cmd.ExecuteReader();

                        // заполнения поля 
                        if (dr.Read())
                            dataGridView1.Rows[ii].Cells[8].Value = dr[0].ToString();
                        dr.Close();
                    }

                    // должность
                    for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
                    {
                        sql = "select ";
                        for (int j = 1; j < DBSHeader2.Length; j++)
                        {
                            if (j == 1)
                                sql += DBSHeader2[j];
                            else
                                sql += ", " + DBSHeader2[j];
                        }
                        // генерация sql комманды
                        sql += " from " + STable2 + " where " + DBSHeader2[0] + " = " + dataGridView1.Rows[ii].Cells[6].Value.ToString();
                        //получение комманды и коннекта
                        cmd = new MySqlCommand(sql, mycon);
                        //вополнение запроса
                        cmd.ExecuteNonQuery();
                        da = new MySqlDataAdapter(cmd);
                        //получение выборки
                        dr = cmd.ExecuteReader();
                        // заполнения поля
                        if (dr.Read())
                            dataGridView1.Rows[ii].Cells[9].Value = dr[0].ToString();
                        dr.Close();
                    }

                    // чин
                    for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
                    {
                        sql = "select ";
                        for (int j = 1; j < DBSHeader3.Length; j++)
                        {
                            if (j == 1)
                                sql += DBSHeader3[j];
                            else
                                sql += ", " + DBSHeader3[j];
                        }
                        // генерация sql комманды
                        sql += " from " + STable3 + " where " + DBSHeader3[0] + " = " + dataGridView1.Rows[ii].Cells[7].Value.ToString();
                        //получение комманды и коннекта
                        cmd = new MySqlCommand(sql, mycon);
                        //вополнение запроса
                        cmd.ExecuteNonQuery();
                        da = new MySqlDataAdapter(cmd);
                        //получение выборки
                        dr = cmd.ExecuteReader();
                        // заполнения поля
                        if (dr.Read())
                            dataGridView1.Rows[ii].Cells[10].Value = dr[0].ToString();
                        dr.Close();
                    }

                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];     //делаем последнюю ячейку активной
                    toolStripProgressBar1.Value = 100;

                    toolStripProgressBar1.Visible = false;                          // убераем прогресс бар

                    dataGridView1.Enabled = true;                                   // делаем таблицу доступной
                    items = dataGridView1.Rows.Count;

                    // иправление бага WS
                    rSize = (rSize > 0) ? -1 : 1;
                    this.ClientSize = new System.Drawing.Size(this.ClientSize.Width + rSize, this.ClientSize.Height);
                }
                else
                    MessageBox.Show("Нет подключениея к базе данных!");
                Lock = false;
                dataGridView1.Enabled = true;
                CheackButton();
            }
            catch
            {
                MessageBox.Show("Ошибка при загрузке данных!\nВозможно у Вас нет доступа к базе данных!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddOper_Load(object sender, EventArgs e)
        {
            try
            {
                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();

                //авто выбор кол-ва элементов, для отображения
                comboBox1.SelectedIndex = 0;

                //зполнение шапки таблици
                for (int i = 0; i < 4; i++)
                {
                    dataGridView1.Columns[i + 1].HeaderText = HeaderText[i];
                }

                for (int i = 4; i < HeaderText.Length; i++)
                {
                    dataGridView1.Columns[i + 4].HeaderText = HeaderText[i];
                }

                //скрытие лишних полей таблици
                if (HeaderText.Length == 1)
                {
                    for (int i = 2; i < dataGridView1.ColumnCount - 2; i++)
                        dataGridView1.Columns[i].Visible = false;
                    textBox2.Visible = false;
                }

                //button7.Text = HeaderText[HeaderText.Length - 1];

                this.Text = FormName;

                this.Column5.Visible = IsSelect;

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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string str = "";    // ячейки табл. на форме
            string DBH = "";    // поля в табл. в БД 
            bool isNull = false;

            try
            {
                if (!Lock)
                {
                    // добавление информации
                    if (items != dataGridView1.Rows.Count)
                    {
                        for (int i = 0; i < DBHeader.Length - 1; i++)
                            if (dataGridView1.Rows[e.RowIndex].Cells[i + 1].Value == null)
                                isNull = true;

                        if (!isNull)
                        {
                            str = "";
                            DBH = "";
                            for (int i = 0; i < DBHeader.Length - 1; i++)
                            {
                                if (i == 0)
                                    str += "'" + dataGridView1.Rows[e.RowIndex].Cells[i + 1].Value.ToString() + "'";
                                else
                                    str += ",'" + dataGridView1.Rows[e.RowIndex].Cells[i + 1].Value.ToString() + "'";
                            }

                            for (int i = 1; i < DBHeader.Length; i++)
                            {
                                if (i == 1)
                                    DBH += DBHeader[i];
                                else
                                    DBH += ", " + DBHeader[i];
                            }

                            string sql = "insert into " + table + " (" + DBH + ") values (" + str + ")";
                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Данные " + str + " успешно добавлены!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lastIndex = 0;
                            if (lastIndex < 0)
                                lastIndex = 0;
                            LoadData();
                        }
                    }
                    else
                    //редактирование информации
                    {
                        str = "";
                        DBH = "";

                        for (int i = 1; i < DBHeader.Length; i++)
                        {
                            if (i == 1)
                                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
                                    str += DBHeader[i] + " = ''";
                                else
                                    str += DBHeader[i] + " = '" + dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString() + "'";
                            else
                                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
                                    str += ", " + DBHeader[i] + " = ''";
                                else
                                    str += ", " + DBHeader[i] + " = '" + dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString() + "'";
                        }

                        DBH = DBHeader[0] + " = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";

                        string sql = "update " + table + " set " + str + " where " + DBH;
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Данные " + str + " успешно изменены!", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при изменении / добавлении данных!\nИзменения не были сохранены!\nПустые поля в таблице не допускаются!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
            }
        }

        private void AddOper_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 3 && e.KeyChar != 22 && e.KeyChar != 26)
                e.Handled = true;
        }

        private bool DontUse(string DTable, string DPK, DataGridViewCellEventArgs e)
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            bool DellRezylt;
            string sql = "select * from " + DTable + " where " + DPK + " = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            cmd = new MySqlCommand(sql, mycon);
            //вополнение запроса
            cmd.ExecuteNonQuery();
            //выборка по запросу
            da = new MySqlDataAdapter(cmd);
            dr = cmd.ExecuteReader();
            DellRezylt = dr.Read();
            dr.Close();
            // true - используется в др табл
            // false - не используется в др табл
            return DellRezylt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                // удалеие
                DialogResult del = MessageBox.Show("Вы действительно хотите удалить данный элемент?\nДанное действие необратимо!", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    string DTable1 = ""; //таблица1 для проверки
                    string DPK1 = "";    //поле для проверки в табл 1
                    string DTable2 = ""; //таблица2 для проверки
                    string DPK2 = "";    //поле для проверки в табл 2
                    string DTable3 = ""; //таблица3 для проверки
                    string DPK3 = "";    //поле для проверки в табл 3
                    string sql;

                    DTable1 = "postanovlenie";  //постановление
                    DPK1 = "pk_polise";

                    DTable2 = "protokol";       //протокол
                    DPK2 = "pk_polise";

                    DTable3 = "delo";           //дело
                    DPK3 = "pk_polise";

                    if (!DontUse(DTable1, DPK1, e) && !DontUse(DTable2, DPK2, e) && !DontUse(DTable3, DPK3, e))
                    {
                        sql = "delete from " + table + " where " + DBHeader[0] + " = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Данные успешно удалены!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Удаление невозможно!\nНарушение целостности данных!", "Отказ удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (e.ColumnIndex == 12 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                //выбрать
                PC_rezult = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //ФИО
                Rezult = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + " " + dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() + " " + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                this.Close();
            }

            if (e.ColumnIndex == 8 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)    //звание
            {
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник званий", "spravochnik_zvanii", new string[] { "Звание", "Идентификационный номер" }, new string[] { "pk_zvanie", "nazvanie", "id_number" });
                f.ShowDialog();

                string PC_rezult = "";  //значение первичного ключа
                string Rezult = "";     // соответствующее текстовое значение

                PC_rezult = f.PC_rezult;
                Rezult = f.Rezult;

                if (PC_rezult != null && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[5].Value = PC_rezult;
                    if (e.RowIndex < dataGridView1.Rows.Count - 1)
                        dataGridView1.Rows[e.RowIndex].Cells[8].Value = Rezult;
                }
                else
                    if (dataGridView1.Rows[e.RowIndex].Cells[5].Value != null)
                    {
                        LoadData();
                    }
                
            }

            if (e.ColumnIndex == 9 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)    //должность
            {
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник должностей", "spravochnik_dolgnostei", new string[] { "Должность", "Идентификационный номер" }, new string[] { "pk_dolgnost", "nazvanie", "id_number" });
                f.ShowDialog();

                string PC_rezult = "";  //значение первичного ключа
                string Rezult = "";     // соответствующее текстовое значение

                PC_rezult = f.PC_rezult;
                Rezult = f.Rezult;

                if (PC_rezult != null && e.RowIndex < dataGridView1.Rows.Count - 1)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[6].Value = PC_rezult;
                    if (e.RowIndex < dataGridView1.Rows.Count - 1)
                        dataGridView1.Rows[e.RowIndex].Cells[9].Value = Rezult;
                }
                else
                    if (dataGridView1.Rows[e.RowIndex].Cells[6].Value != null)
                    {
                        LoadData();
                    }
                
            }

            if (e.ColumnIndex == 10 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)    //чин
            {
                AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник классных чинов", "chin", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_chin", "nazvanie", "id_number" });
                f.ShowDialog();

                string PC_rezult = "";  //значение первичного ключа
                string Rezult = "";     // соответствующее текстовое значение

                PC_rezult = f.PC_rezult;
                Rezult = f.Rezult;

                if (PC_rezult != null && e.RowIndex < dataGridView1.Rows.Count - 1)
                {
                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = PC_rezult;
                        if (e.RowIndex < dataGridView1.Rows.Count - 1)
                            dataGridView1.Rows[e.RowIndex].Cells[10].Value = Rezult;
                }
                else
                    if (dataGridView1.Rows[e.RowIndex].Cells[7].Value != null)
                    {
                        LoadData();
                    }
            }
        }

        private void AddOper_Shown(object sender, EventArgs e)
        {
            //загрузка информации
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // >
            lastIndex += Convert.ToInt32(comboBox1.SelectedItem);
            if (count - lastIndex < 0)
                lastIndex = count - Convert.ToInt32(comboBox1.SelectedItem);
            LoadData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // >>
            lastIndex = count - Convert.ToInt32(comboBox1.SelectedItem);
            LoadData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // <
            lastIndex -= Convert.ToInt32(comboBox1.SelectedItem);
            if (lastIndex <= 0)
                lastIndex = 0;

            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // <<
            lastIndex = 0;
            LoadData();
        }

        public void CheackButton()
        {
            if (lastIndex <= 0)
                button3.Enabled = button4.Enabled = false;
            else
                button3.Enabled = button4.Enabled = true;

            if (count - (lastIndex + Convert.ToInt32(comboBox1.SelectedItem)) <= 0)
                button5.Enabled = button6.Enabled = false;
            else
                button5.Enabled = button6.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheackButton();
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // перебераем элементы на форме и удаляем текст из всех TextBox
            foreach (Control ctrl in Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.Text = "";
                }
            }

            LoadData();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBox5_Enter(object sender, EventArgs e) //звание
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник званий", "spravochnik_zvanii", new string[] { "Звание", "Идентификационный номер" }, new string[] { "pk_zvanie", "nazvanie", "id_number" });
            f.ShowDialog();

            string PC_rezult = "";  //значение первичного ключа
            string Rezult = "";     // соответствующее текстовое значение

            PC_rezult = f.PC_rezult;
            Rezult = f.Rezult;

            if (PC_rezult != null)
            {
                textBox8.Text = PC_rezult;
                textBox5.Text = Rezult;
            }
            else
            {
                textBox8.Text = "";
                textBox5.Text = "";
            }
            LoadData();
        }

        private void textBox6_Enter(object sender, EventArgs e) //должность
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник должностей", "spravochnik_dolgnostei", new string[] { "Должность", "Идентификационный номер" }, new string[] { "pk_dolgnost", "nazvanie", "id_number" });
            f.ShowDialog();

            string PC_rezult = "";  //значение первичного ключа
            string Rezult = "";     // соответствующее текстовое значение

            PC_rezult = f.PC_rezult;
            Rezult = f.Rezult;

            if (PC_rezult != null)
            {
                textBox9.Text = PC_rezult;
                textBox6.Text = Rezult;
            }
            else
            {
                textBox9.Text = "";
                textBox6.Text = "";
            }
            LoadData();
        }

        private void textBox7_Enter(object sender, EventArgs e)    //чин
        {
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник классных чинов", "chin", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_chin", "nazvanie", "id_number" });
            f.ShowDialog();

            string PC_rezult = "";  //значение первичного ключа
            string Rezult = "";     // соответствующее текстовое значение

            PC_rezult = f.PC_rezult;
            Rezult = f.Rezult;

            if (PC_rezult != null)
            {
                textBox10.Text = PC_rezult;
                textBox7.Text = Rezult;
            }
            else
            {
                textBox10.Text = "";
                textBox7.Text = "";
            }
            LoadData();
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
