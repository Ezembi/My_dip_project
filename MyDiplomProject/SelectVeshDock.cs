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
    public partial class SelectVeshDock : Form
    {
        public MySqlConnection mycon;   // коннектор б/д
        public MySqlCommand cmd;        // sql комманды для работы с б/д

        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера

        string STable4;      // ТАБЛИЦА4 БАЗЫ ДАННЫХ для сопудствующей информации (Способы упаковки вещественных доказательств)
        string[] DBSHeader4; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable5;      // ТАБЛИЦА5 БАЗЫ ДАННЫХ для сопудствующей информации (Справочник материалов, в которые упаковываю вещественные доказательства (полиэтилен, бумага и т.д.))
        string[] DBSHeader5; // название полей в таблице для sql запросов, для сопудствующей информации

        string STable3;      // ТАБЛИЦА3 БАЗЫ ДАННЫХ для сопудствующей информации (Вещественное доказательство)
        string[] DBSHeader3; // название полей в таблице для sql запросов, для сопудствующей информации

        int lastIndex = 0;  // для корректного постраничного отображения
        int count = 0;      // количество записей в таблице БД

        public SelectVeshDock()
        {
            InitializeComponent();
        }

        public SelectVeshDock(string _user, string _pass, string _database, string _ip)
        {
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;

            STable3 = "vesh_dok";
            DBSHeader3 = new string[] { "pk_vesh_dok", "priznaki", "naiminovanie", "pk_material", "pk_ypakovka", "pk_protokol" };
            STable4 = "ypakovka";
            DBSHeader4 = new string[] { "pk_ypakovka", "nazvanie" };
            STable5 = "spravochnik_materialov";
            DBSHeader5 = new string[] { "pk_material", "material", "id_number" };

            InitializeComponent();
        }

        private void SelectVeshDock_Load(object sender, EventArgs e)
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

        private void LoadVeshDok(string STable, System.Windows.Forms.DataGridView MyDataGridView)   // загрузка списка вещественных доказательств(изъятого имущества)
        {
            MySqlDataAdapter da;
            MySqlDataReader dr;
            string sql = "";
            int i = 0;
            try
            {
                MyDataGridView.Rows.Clear();

                //грузим проживающих в данном жилом помещении лиц
                sql = "select * from " + STable;
                sql += " where priznaki like '%" + textBox1.Text + "%' ";
                sql += "and naiminovanie like '%" + textBox2.Text + "%' ";

                if (textBox4.Text != "")
                    sql += " and pk_material = " + textBox4.Text;

                if (textBox6.Text != "")
                    sql += " and pk_ypakovka = " + textBox6.Text;

                //получение комманды и коннекта
                cmd = new MySqlCommand(sql, mycon);
                //вополнение запроса
                cmd.ExecuteNonQuery();
                da = new MySqlDataAdapter(cmd);
                //получение выборки
                dr = cmd.ExecuteReader();
                // заполнения поля 
                // подсчёт количества записей в таблице
                count = 0;
                while (dr.Read()) count++;
                dr.Close();

                //грузим проживающих в данном жилом помещении лиц
                sql = "select * from " + STable;
                sql += " where priznaki like '%" + textBox1.Text + "%' ";
                sql += "and naiminovanie like '%" + textBox2.Text + "%' ";

                if (textBox4.Text != "")
                    sql += " and pk_material = " + textBox4.Text;

                if (textBox6.Text != "")
                    sql += " and pk_ypakovka = " + textBox6.Text;
                sql += " limit " + lastIndex.ToString() + ", " + comboBox2.SelectedItem.ToString();
                //получение комманды и коннекта
                cmd = new MySqlCommand(sql, mycon);
                //вополнение запроса
                cmd.ExecuteNonQuery();
                da = new MySqlDataAdapter(cmd);
                //получение выборки
                dr = cmd.ExecuteReader();
                // заполнения поля 
                toolStripProgressBar1.Value = 0;
                while (dr.Read())
                {
                    MyDataGridView.Rows.Add();
                    MyDataGridView.Rows[i].Cells[0].Value = dr[0].ToString();    // PC
                    MyDataGridView.Rows[i].Cells[1].Value = dr[1].ToString();    // признаки
                    MyDataGridView.Rows[i].Cells[2].Value = dr[2].ToString();    // наименование
                    MyDataGridView.Rows[i].Cells[3].Value = dr[3].ToString();    // pk_material
                    MyDataGridView.Rows[i].Cells[4].Value = dr[4].ToString();    // pk_ypakovka
                    MyDataGridView.Rows[i].Cells[7].Value = "Открыть";
                    MyDataGridView.Rows[i].Cells[8].Value = dr[6].ToString();    // pk_protokol

                    i++;
                    toolStripProgressBar1.Value = (i * 100) / count;
                }
                dr.Close();

                loadFromOtherTable(STable4, DBSHeader4, MyDataGridView, 4, 6);  //Способы упаковки вещественных доказательств
                loadFromOtherTable(STable5, DBSHeader5, MyDataGridView, 3, 5);  //Справочник материалов, в которые упаковываю вещественные доказательства

                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel1.Text = "Записей в базе: " + count;
                MyDataGridView.CurrentCell = MyDataGridView.Rows[i].Cells[1];     //делаем последнюю ячейку активной
                toolStripProgressBar1.Value = 100;

                toolStripProgressBar1.Visible = false;                          // убераем прогресс бар
                CheackButton();


            }
            catch (Exception e) { MessageBox.Show(e.Message + "Error:1\nНе удалось загрузить список изъятого имущества (вещественных доказательств)!\nВозможно у Вас нет доступа к базе данных!", "Ошибка сохранения!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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

        private void SelectVeshDock_Shown(object sender, EventArgs e)
        {
            LoadVeshDok(STable3, dataGridView2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lastIndex += Convert.ToInt32(comboBox2.SelectedItem);
            if (count - lastIndex < 0)
                lastIndex = count - Convert.ToInt32(comboBox2.SelectedItem);
            LoadVeshDok(STable3, dataGridView2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lastIndex = count - Convert.ToInt32(comboBox2.SelectedItem);
            LoadVeshDok(STable3, dataGridView2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lastIndex -= Convert.ToInt32(comboBox2.SelectedItem);
            if (lastIndex <= 0)
                lastIndex = 0;

            LoadVeshDok(STable3, dataGridView2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lastIndex = 0;
            LoadVeshDok(STable3, dataGridView2);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheackButton();
            LoadVeshDok(STable3, dataGridView2);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadVeshDok(STable3, dataGridView2);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            LoadVeshDok(STable3, dataGridView2);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            LoadVeshDok(STable3, dataGridView2);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            LoadVeshDok(STable3, dataGridView2);
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            //материал упаковки
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник материалов упаковки", "spravochnik_materialov", new string[] { "Маериал", "Идентификационный номер" }, new string[] { "pk_material", "material", "id_number" });
            f.ShowDialog();
            textBox3.Text = f.Rezult;
            textBox4.Text = f.PC_rezult;
        }

        private void textBox5_MouseClick(object sender, MouseEventArgs e)
        {
            //способ упаковки
            AddDojnost f = new AddDojnost(User, Password, Database, Ip, true, "Справочник способов упаковки вещественных доказательств", "ypakovka", new string[] { "Способ" }, new string[] { "pk_ypakovka", "nazvanie" });
            f.ShowDialog();
            textBox5.Text = f.Rezult;
            textBox6.Text = f.PC_rezult;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 7 && e.RowIndex < dataGridView2.Rows.Count - 1 && e.RowIndex != -1)
                {
                    string pk_pro = dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString();
                    string sql = "";
                    MySqlDataAdapter da;
                    MySqlDataReader dr;
                    sql = "select id_prot, PK_Dela, pk_postanov from protokol where pk_protokol = " + pk_pro;

                    //получение комманды и коннекта
                    cmd = new MySqlCommand(sql, mycon);
                    //вополнение запроса
                    cmd.ExecuteNonQuery();
                    da = new MySqlDataAdapter(cmd);
                    //получение выборки
                    dr = cmd.ExecuteReader();
                    // заполнения поля 
                    // подсчёт количества записей в таблице
                    if (dr.Read())
                    {
                        this.Visible = false;
                        Protocol f = new Protocol(User, Password, Database, Ip, dr[1].ToString(), dr[0].ToString(), dr[2].ToString(), pk_pro);
                        f.ShowDialog();
                        this.Visible = true;
                    }
                    dr.Close();
                    LoadVeshDok(STable3, dataGridView2);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // перебераем элементы на форме и удаляем текст из всех TextBox
            foreach (Control ctrl in Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.Text = "";
                }
            }

            LoadVeshDok(STable3, dataGridView2);
        }
    }
}
