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
    public partial class AddDojnost : Form
    {
        public MySqlConnection mycon;   // коннектор б/д
        public MySqlCommand cmd;        // sql комманды для работы с б/д

        int items;
        bool Lock = true;
        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера
        int lastIndex = 0;
        int count = 0;
        int rSize = 1;

        public AddDojnost()
        {
            InitializeComponent();
        }

        public AddDojnost(string _user, string _pass, string _database, string _ip)
        {
            User = _user;
            Password = _pass;
            Database = _database;
            Ip = _ip;
            InitializeComponent();
        }

        /**
        @brief загрузка информации из бд
        @return Функция не возвращает значение
        @param Указыватся без параметров
        */
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
                    //sql запрос
                    string a = "select * from spravochnik_dolgnostei where nazvanie like '%" + textBox1.Text + "%' and id_number like '%" + textBox2.Text + "%'";
                    cmd = new MySqlCommand(a, mycon);

                    //вополнение запроса
                    cmd.ExecuteNonQuery();

                    //выборка по запросу
                    da = new MySqlDataAdapter(cmd);
                    dr = cmd.ExecuteReader();

                    // подсчёт количества записей в таблице
                    count = 0;
                    while (dr.Read()) count++;
                    dr.Close();


                    //sql запрос
                    a = "select * from spravochnik_dolgnostei where nazvanie like '%" + textBox1.Text + "%' and id_number like '%" + textBox2.Text + "%' limit " + lastIndex.ToString() + ", " + comboBox1.SelectedItem.ToString();
                    cmd = new MySqlCommand(a, mycon);

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
                        dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();    //название
                        dataGridView1.Rows[i].Cells[2].Value = dr[2].ToString();    //id номер
                        dataGridView1.Rows[i].Cells[3].Value = "Удалить";           //Удаление
                        
                        i++;
                        toolStripProgressBar1.Value = (i * 100) / count;
                    }
                    dr.Close();

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

        /**
		@brief Подключение к б/д
	    */
        private void AddDojnost_Load(object sender, EventArgs e)
        {
            try
            {
                //Подключение к б/д
                string conn = "Server=" + Ip + ";Database=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";CharSet=cp1251;";
                mycon = new MySqlConnection(conn);

                //открытие подключения
                mycon.Open();
                comboBox1.SelectedIndex = 0;

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


        /**
		@brief добавление / редактирование информации в бд
	    */
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(dataGridView1.Rows.Count.ToString() + Lock.ToString());
            string str = "";
            bool isNull = false;
           //* 
            try
            {
                if (!Lock)
                {
                    // добавление информации
                    if (items != dataGridView1.Rows.Count)
                    {
                        for (int i = 1; i < dataGridView1.ColumnCount - 1; i++)
                            if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
                                isNull = true;

                        if (!isNull)
                        {
                            for (int i = 1; i < dataGridView1.ColumnCount - 1; i++)
                            {
                                if (i == 1)
                                    str += "'" + dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString() + "'";
                                else
                                    str += ",'" + dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString() + "'";
                            }

                            string a = "insert into spravochnik_dolgnostei (nazvanie, id_number) values (" + str + ")";
                            cmd = new MySqlCommand(a, mycon);
                            cmd.ExecuteNonQuery();
                            lastIndex = count - Convert.ToInt32(comboBox1.SelectedItem) + 1;
                            LoadData();
                        }
                    }
                    else
                    //редактирование информации
                    {
                        string a = "update spravochnik_dolgnostei set nazvanie = '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "', id_number = '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' where pk_dolgnost = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        cmd = new MySqlCommand(a, mycon);
                        cmd.ExecuteNonQuery();
                        LoadData();
                    }
                }//*/
            }
            catch
            {
                MessageBox.Show("Ошибка при изменении / добавлении данных!\nИзменения не были сохранены!", "Ошибка данных!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*button1.Enabled = false;

            if (MessageBox.Show("Удалть?", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            {
                cmd.CommandText = "SELECT * FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC, POLISE WHERE POLISE.PK_DOLGNOSTOE_LICO = SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO AND SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE = '" + oldValue + "'";
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    cmd.CommandText = " DELETE FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC WHERE NAZVANIE = '" + oldValue + "'";
                    cmd.ExecuteNonQuery();

                    dataGridView1.Rows.Clear();

                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTNIX_LIC";
                    dr = cmd.ExecuteReader();
                    int i = 0;
                    Lock = true;
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();
                        i++;
                    }
                    items = dataGridView1.Rows.Count;
                    Lock = false;
                }
                else
                    MessageBox.Show("Необходимо удалить или изменить всех уполномоченных с данным должностным лицом!", "Удаление данного должностного лица невозможно!");
            }*/
        }

        private void AddDojnost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',')
                e.Handled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                MessageBox.Show("Delete");
            }
        }

        /**
		@brief загрузка формы
	    */
        private void AddDojnost_Shown(object sender, EventArgs e)
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
    }
}
