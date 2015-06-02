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
    public partial class AddSpecMen : Form
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
        public string PC_rezult;    // "возвращаемое" формаой значение первичного ключа
        public string Rezult;       // "возвращаемое" формаой значение текстового поля
        bool IsSelect;      // true - форма вызвана для выбора элемента
        string STable;      // ТАБЛИЦА БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader; // название полей в таблице для sql запросов, для сопудствующей информации

        public AddSpecMen()
        {
            InitializeComponent();
        }

        public AddSpecMen(string _user, string _pass, string _database, string _ip, bool _Select, string _FormName, string _Table, string[] _HeaderText, string[] _DBHeader, string _STable, string[] _DBSHeader)
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
            STable = _STable;
            DBSHeader = _DBSHeader;
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

                    if(textBox4.Text != "")
                        sql += "and " + DBHeader[4] + " = '" + textBox4.Text + "' ";

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
                    if (textBox4.Text != "")
                        sql += "and " + DBHeader[4] + " = '" + textBox4.Text + "' ";
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
                        dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();    //фамилия
                        dataGridView1.Rows[i].Cells[2].Value = dr[2].ToString();    //имя
                        dataGridView1.Rows[i].Cells[3].Value = dr[3].ToString();    //отчество
                        dataGridView1.Rows[i].Cells[4].Value = dr[4].ToString();    //id_spec

                        dataGridView1.Rows[i].Cells[6].Value = "Удалить";           //Удаление
                        dataGridView1.Rows[i].Cells[7].Value = "Выбрать";           //Выбор

                        i++;
                        toolStripProgressBar1.Value = (i * 100) / count;
                    }
                    dr.Close();

                    // специализация (выборка информации из другой таблице)
                    for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
                    {
                        sql = "select ";
                        for (int j = 1; j < DBSHeader.Length; j++)
                        {
                            if (j == 1)
                                sql += DBSHeader[j];
                            else
                                sql += ", " + DBSHeader[j];
                        }
                        sql += " from " + STable + " where " + DBSHeader[0] + " = " + dataGridView1.Rows[ii].Cells[4].Value.ToString();
 
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                        da = new MySqlDataAdapter(cmd);
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                            dataGridView1.Rows[ii].Cells[5].Value = dr[0].ToString();
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

        private void AddSpecMen_Load(object sender, EventArgs e)
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
                for (int i = 0; i < HeaderText.Length; i++)
                {
                    dataGridView1.Columns[i + 1].HeaderText = HeaderText[i];
                }
                dataGridView1.Columns[HeaderText.Length+1].HeaderText = HeaderText[HeaderText.Length - 1];

                //скрытие лишних полей таблици
                if (HeaderText.Length == 1)
                {
                    for (int i = 2; i < dataGridView1.ColumnCount - 2; i++)
                        dataGridView1.Columns[i].Visible = false;
                    textBox2.Visible = false;
                }

                button7.Text = HeaderText[HeaderText.Length - 1];

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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
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
                            lastIndex = count - Convert.ToInt32(comboBox1.SelectedItem) + 1;
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

        private void AddSpecMen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 3 && e.KeyChar != 22 && e.KeyChar != 26)
                e.Handled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6 && e.RowIndex < dataGridView1.Rows.Count - 1)
                {
                    // удалеие
                    DialogResult del = MessageBox.Show("Вы действительно хотите удалить данный элемент?\nДанное действие необратимо!", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (del == System.Windows.Forms.DialogResult.Yes)
                    {
                        string DTable1 = ""; //таблица1 для проверки
                        string DPK1 = "";    //поле для проверки в табл 1
                        string sql;

                        switch (table)
                        {
                            case "spravochnik_pod": DTable1 = "Delo"; DPK1 = "PK_Raiona"; break;
                        }

                        if (!DontUse(DTable1, DPK1, e))
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
            }
            catch { MessageBox.Show("При удаление произошла ошибка!", "Удаление невозможно", MessageBoxButtons.OK, MessageBoxIcon.Error); }


            if (e.ColumnIndex == 7 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                // выбор
                PC_rezult = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                switch (table)
                {
                    case "spravochnik_pod": Rezult = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); break;
                    case "specialist": Rezult = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + " " + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + " " + dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(); break;
                }
               
                this.Close();
            }

            if (e.ColumnIndex == 5 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                // загрузка доп. информации
                AddDojnost f;
                switch (STable)
                {
                    case "spravochnik_oblastei_spec": f = new AddDojnost(User, Password, Database, Ip, true, "Справочник областей специализации", "spravochnik_oblastei_spec", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_special", "nazvanie", "id_number" }); break;
                    case "spravochnik_gorodov": f = new AddDojnost(User, Password, Database, Ip, true, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" }); break;
                    default: f = new AddDojnost(User, Password, Database, Ip, true, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" }); break;
                }
                
                f.ShowDialog();
                string rezult = "";
                rezult = f.PC_rezult;
                if (rezult != null)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[4].Value = rezult;
                }
                else
                    if (dataGridView1.Rows[e.RowIndex].Cells[4].Value != null)
                        rezult = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
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

        private void AddSpecMen_Shown(object sender, EventArgs e)
        {
            //загрузка информации
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lastIndex += Convert.ToInt32(comboBox1.SelectedItem);
            if (count - lastIndex < 0)
                lastIndex = count - Convert.ToInt32(comboBox1.SelectedItem);
            LoadData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lastIndex = count - Convert.ToInt32(comboBox1.SelectedItem);
            LoadData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lastIndex -= Convert.ToInt32(comboBox1.SelectedItem);
            if (lastIndex <= 0)
                lastIndex = 0;

            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheackButton();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddDojnost f;
            switch (STable)
            {
                case "spravochnik_oblastei_spec": f = new AddDojnost(User, Password, Database, Ip, true, "Справочник областей специализации", "spravochnik_oblastei_spec", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_special", "nazvanie", "id_number" }); break;
                case "spravochnik_gorodov": f = new AddDojnost(User, Password, Database, Ip, true, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" }); break;
                default: f = new AddDojnost(User, Password, Database, Ip, true, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" }); break;
            }

            f.ShowDialog();
            string rezult = "";
            rezult = f.PC_rezult;
            if (rezult != null)
            {
                textBox4.Text = rezult;
            }
            else
                textBox4.Text = "";
            LoadData();
        }
    }
}
