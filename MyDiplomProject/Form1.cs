using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql;
using MySql.Data.MySqlClient;

namespace MyDiplomProject
{
    public partial class Form1 : Form
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

        string STable1;      // ТАБЛИЦА1 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader1; // название полей в таблице1 для sql запросов, для сопудствующей информации

        string STable2;      // ТАБЛИЦА2 БАЗЫ ДАННЫХ для сопудствующей информации
        string[] DBSHeader2; // название полей в таблице2 для sql запросов, для сопудствующей информации

        public Form1()
        {
            User = "root";
            Password = "ezembi007";
            Database = "polise";
            Ip = "127.0.0.1";
            InitializeComponent();
        }

        public Form1(string _user, string _pass, string _database, string _ip, string _FormName, string _Table, string[] _HeaderText, string[] _DBHeader, string _STable1, string[] _DBSHeader1, string _STable2, string[] _DBSHeader2)
        {
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            table = _Table;
            HeaderText = _HeaderText;
            DBHeader = _DBHeader;
            FormName = _FormName;
            STable1 = _STable1;
            DBSHeader1 = _DBSHeader1;
            STable2 = _STable2;
            DBSHeader2 = _DBSHeader2;
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
                    sql += "and " + DBHeader[3] + " like '%" + textBox2.Text + "%' ";

                    if (textBox4.Text != "")
                        sql += "and " + DBHeader[5] + " = '" + textBox4.Text + "' ";
                    if (textBox6.Text != "")
                        sql += "and " + DBHeader[6] + " = '" + textBox6.Text + "' ";

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
                    sql += "and " + DBHeader[3] + " like '%" + textBox2.Text + "%' ";

                    if (textBox4.Text != "")
                        sql += "and " + DBHeader[5] + " = '" + textBox4.Text + "' ";
                    if (textBox6.Text != "")
                        sql += "and " + DBHeader[6] + " = '" + textBox6.Text + "' ";

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
                        dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();    // PC
                        dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();    // номер материала
                        dataGridView1.Rows[i].Cells[2].Value = Convert.ToDateTime(dr[2].ToString()).ToShortDateString();    // дата поступления материала
                        dataGridView1.Rows[i].Cells[3].Value = dr[3].ToString();    // номер дела

                        if(dr[4].ToString() != "01.01.0001 0:00:00")
                            dataGridView1.Rows[i].Cells[4].Value = Convert.ToDateTime(dr[4].ToString()).ToShortDateString();    // дата поступления дела
                        dataGridView1.Rows[i].Cells[5].Value = dr[8].ToString();    // pk_polise (уполном)
                        dataGridView1.Rows[i].Cells[6].Value = dr[9].ToString();    // PK_Raiona (подразделение следственного комитета)

                        dataGridView1.Rows[i].Cells[9].Value = "Удалить";           // Удаление
                        dataGridView1.Rows[i].Cells[10].Value = "Открыть";          // Выбор

                        i++;
                        toolStripProgressBar1.Value = (i * 100) / count;
                    }
                    dr.Close();

                    // уполном
                    for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
                    {
                        if (dataGridView1.Rows[ii].Cells[5].Value != null && dataGridView1.Rows[ii].Cells[5].Value.ToString() != "")
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
                                dataGridView1.Rows[ii].Cells[7].Value = dr[0].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString();
                            dr.Close();
                        }
                    }

                    // подразделение следственного комитета
                    for (int ii = 0; ii < dataGridView1.Rows.Count - 1; ii++)
                    {
                        if (dataGridView1.Rows[ii].Cells[6].Value != null && dataGridView1.Rows[ii].Cells[6].Value.ToString() != "")
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
                                dataGridView1.Rows[ii].Cells[8].Value = dr[0].ToString();
                            dr.Close();
                        }
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

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;Convert Zero Datetime=True;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();

                //авто выбор кол-ва элементов, для отображения
                comboBox1.SelectedIndex = 0;
                //button7.Text = HeaderText[HeaderText.Length - 1];

                this.Text = FormName;

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

        #region Код для вызова любого из справочника

        /// <summary>
        /// Описание всех пунков menuStrip1
        /// </summary>

        private void справочникЗванийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник званий", "spravochnik_zvanii", new string[] { "Звание", "Идентификационный номер" }, new string[] { "pk_zvanie", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникСпособовУпаковкиВещДокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник способов упаковки вещественных доказательств", "ypakovka", new string[] { "Способ" }, new string[] { "pk_ypakovka", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникТехническихСредствToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник технических средств", "spravochnik_tex_sredstv", new string[] { "Техническое средство", "Идентификационный номер" }, new string[] { "pk_tex_sredstvo", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникОбластейСпециализацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник областей специализации", "spravochnik_oblastei_spec", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_special", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникПогодныхУсловийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник погодных условий", "spravochnik_pogodi", new string[] { "Погода" }, new string[] { "pk_pogoda", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникМатериаловУпаковкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник материалов упаковки", "spravochnik_materialov", new string[] { "Маериал", "Идентификационный номер" }, new string[] { "pk_material", "material", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникГоробовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник городов", "spravochnik_gorodov", new string[] { "Город", "Идентификационный номер" }, new string[] { "pk_gorod", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникДолжностейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник должностей", "spravochnik_dolgnostei", new string[] { "Должность", "Идентификационный номер" }, new string[] { "pk_dolgnost", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникОсвещённостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник освещённости", "spravochnik_osveshennosti", new string[] { "Освещённость" }, new string[] { "pk_osveshennost", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникСпециалистовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, false, "Справочник специалистов", "specialist", new string[] { "Фамилия", "Имя", "Отчество", "Область специализации" }, new string[] { "pk_spec", "surname", "Pname", "second_name", "pk_special" }, "spravochnik_oblastei_spec", new string[] { "pk_special", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddOper f = new AddOper(User, Password, Database, Ip, false, "Справочник уполномоченных", "polise", new string[] { "Табельный номер", "Фамилия", "Имя", "Отчество", "Звание", "Должность", "Чин" }, new string[] { "pk_polise", "id_number", "surname", "Pname", "second_name", "pk_zvanie", "pk_dolgnost", "pk_chin" }, "spravochnik_zvanii", new string[] { "pk_zvanie", "nazvanie" }, "spravochnik_dolgnostei", new string[] { "pk_dolgnost", "nazvanie" }, "chin", new string[] { "pk_chin", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникПодразделенийСледственногоКомитетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, false, "Справочник подразделений следственного комитета", "spravochnik_pod", new string[] { "Название", "Район", "Идентификационный номер", "Город" }, new string[] { "PK_Raiona", "Nazv", "Raion", "id_number", "pk_gorod" }, "spravochnik_gorodov", new string[] { "pk_gorod", "nazvanie" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникКлассныхЧиновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник классных чинов", "chin", new string[] { "Название", "Идентификационный номер" }, new string[] { "pk_chin", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        private void справочникПроцессуальныхПоложенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, false, "Справочник ппроцессуальных положений", "sp_pro_pol", new string[] { "Наименование положения", "Идентификационный номер" }, new string[] { "pk_pol", "nazvanie", "id_number" });
            f.ShowDialog();
            this.Visible = true;
        }

        #endregion


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 3 && e.KeyChar != 22 && e.KeyChar != 26)
                e.Handled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                //удаление
                string sql = "";    // строка sql запросов
                DialogResult del = MessageBox.Show("Вы действительно хотите удалить дело?\nДанное действие необратимо!\nТакже будет удалена вся информация, связанная с делом (протоколы, постановления, вещественные доказательства, понятые и т.д.)", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (del == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {

                        MySqlDataAdapter da;
                        MySqlDataReader dr;
                        List<string> delPriticol = new List<string>();
                        List<string> delPost = new List<string>();

                        sql = "select pk_protokol,pk_postanov from protokol where PK_Dela = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
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
                            delPriticol.Add(dr[0].ToString());
                            delPost.Add(dr[1].ToString());
                        }
                        dr.Close();

                        for (int i = 0; i < delPriticol.Count; i++)
                        {

                            // поэлементное удаление вещественных доказательств
                            sql = " delete from vesh_dok where pk_protokol = " + delPriticol[i];
                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();

                            // поэлементное удаление понятых
                            sql = " delete from ponatoi where pk_protokol = " + delPriticol[i];
                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();

                            // поэлементное удаление тех средств
                            sql = " delete from r_tex_sredstv where pk_protokol = " + delPriticol[i];
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
                            sql = " delete from peoples where pk_protokol = " + delPriticol[i];
                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();

                            // поэлементное удаление "В ходе осмотра проводилась"
                            sql = " delete from spend where pk_protokol = " + delPriticol[i];
                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();

                            // поэлементное удаление "К протоколу прилагаются"
                            sql = " delete from apps where pk_protokol = " + delPriticol[i];
                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();

                            // поэлементное удаление протоколов
                            sql = " delete from protokol where pk_protokol = " + delPriticol[i];
                            cmd = new MySqlCommand(sql, mycon);
                            cmd.ExecuteNonQuery();
                        }
                        delPriticol.Clear(); // очистка списка протоклов для удаления
                        delPost.Clear();     // очистка списка постановлений для удаления

                        // удаление дела
                        sql = " delete from delo where PK_Dela = " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        cmd = new MySqlCommand(sql, mycon);
                        cmd.ExecuteNonQuery();
                        LoadData();
                    }
                    catch {
                        MessageBox.Show("Ошибка при удалении данных!\nВозможно у Вас нет доступа к базе данных!", "Ошибка удаления!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (e.ColumnIndex == 10 && e.RowIndex < dataGridView1.Rows.Count - 1 && e.RowIndex != -1)
            {
                // выбор дела
                this.Visible = false;
                CriminalAffair f = new CriminalAffair(User, Password, Database, Ip, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                f.ShowDialog();
                this.Visible = true;
                LoadData();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheackButton();
            LoadData();
        }

        private void создатьМатериалПроверкиУголовноеДелоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            CriminalAffair f = new CriminalAffair(User, Password, Database, Ip, "");
            f.ShowDialog();
            this.Visible = true;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            //уполномоченный
            AddOper f = new AddOper(User, Password, Database, Ip, true, "Справочник уполномоченных", "polise", new string[] { "Табельный номер", "Фамилия", "Имя", "Отчество", "Звание", "Должность", "Чин" }, new string[] { "pk_polise", "id_number", "surname", "Pname", "second_name", "pk_zvanie", "pk_dolgnost", "pk_chin" }, "spravochnik_zvanii", new string[] { "pk_zvanie", "nazvanie" }, "spravochnik_dolgnostei", new string[] { "pk_dolgnost", "nazvanie" }, "chin", new string[] { "pk_chin", "nazvanie" });
            f.ShowDialog();

            textBox4.Text = f.PC_rezult;
            textBox3.Text = f.Rezult;

            LoadData();
        }

        private void textBox5_MouseClick(object sender, MouseEventArgs e)
        {
            //Подразделение следственного комитета
            AddSpecMen f = new AddSpecMen(User, Password, Database, Ip, true, "Справочник подразделений следственного комитета", "spravochnik_pod", new string[] { "Название", "Район", "Идентификационный номер", "Город" }, new string[] { "PK_Raiona", "Nazv", "Raion", "id_number", "pk_gorod" }, "spravochnik_gorodov", new string[] { "pk_gorod", "nazvanie" });
            f.ShowDialog();

            textBox6.Text = f.PC_rezult;
            textBox5.Text = f.Rezult;

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

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
