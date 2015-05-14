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
        string oldValue;
        string User;        // имя пользователя, для доступа к б/д
        string Password;    // пароль пользователя
        string Database;    // название б/д
        string Ip;          // ip сервера

        public AddDojnost()
        {
            InitializeComponent();
        }

        /**
		@brief загрузка формы и информации из бд
	    */
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
                if (mycon.State == ConnectionState.Open)
                {
                    //sql запрос
                    string a = "select * from spravochnik_dolgnostei";
                    cmd = new MySqlCommand(a, mycon);

                    //вополнение запроса
                    cmd.ExecuteNonQuery();

                    //выборка по запросу
                    da = new MySqlDataAdapter(cmd);
                    dr = cmd.ExecuteReader();

                    int count = 0;
                    while (dr.Read()) count++;
                    dr.Close();
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

                    toolStripProgressBar1.Value = 100;
                    
                    toolStripProgressBar1.Visible = false;

                    dataGridView1.Enabled = true;
                    items = dataGridView1.Rows.Count;
                    this.ClientSize = new System.Drawing.Size(this.Size.Width, this.Size.Height);
                    
                }
                else
                    MessageBox.Show("Нет подключениея к базе данных!");
                dataGridView1.Enabled = true;
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

                //открытик подключения
                mycon.Open();

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
            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
            string str;

           /* if (dataGridView1.Rows.Count < 1)
            {
                // добавление информации
                if (items != dataGridView1.Rows.Count)
                {
                    str = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTNIX_LIC where NAZVANIE = '" + str + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {
                        cmd.CommandText = "insert into SPRAVOCHNIK_DOLGNOSTNIX_LIC (NAZVANIE) VALUES ('" + str + "')";
                        cmd.ExecuteNonQuery();
                        items = dataGridView1.Rows.Count;
                    }
                    else
                    {
                        MessageBox.Show("Данный элемент уже присудствует в таблице!", "Добавление невозможно!");

                        cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTNIX_LIC";
                        dr = cmd.ExecuteReader();
                        int i = 0;
                        Lock = true;
                        dataGridView1.Rows.Clear();
                        while (dr.Read())
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();
                            i++;
                        }
                        items = dataGridView1.Rows.Count;
                        Lock = false;
                    }

                }
                else
                //редактирование информации
                {
                    str = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTNIX_LIC where NAZVANIE = '" + str + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {

                        cmd.CommandText = " UPDATE SPRAVOCHNIK_DOLGNOSTNIX_LIC set NAZVANIE = '" + str + "' where NAZVANIE = '" + oldValue + "'";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Данный элемент уже присудствует в таблице!", "Изменение невозможно!");

                        cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTNIX_LIC";
                        dr = cmd.ExecuteReader();
                        int i = 0;
                        Lock = true;
                        dataGridView1.Rows.Clear();
                        while (dr.Read())
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();
                            i++;
                        }
                        items = dataGridView1.Rows.Count;
                        Lock = false;
                    }

                }
            }//*/
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            /*if (!Lock)
            {
                if (e.RowIndex != dataGridView1.Rows.Count - 1)
                {
                    oldValue = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    button1.Enabled = true;
                }
                else
                    button1.Enabled = false;
            }*/
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

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 44)
                e.Handled = true;
        }

        private void AddDojnost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar < 65 || e.KeyChar > 90) && (e.KeyChar < 97 || e.KeyChar > 122) && (e.KeyChar < 'А' || e.KeyChar > 'Я') && (e.KeyChar < 'а' || e.KeyChar > 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё' && e.KeyChar != 17 && e.KeyChar != '.' && e.KeyChar != ',')
                e.Handled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex < dataGridView1.Rows.Count - 1)
                MessageBox.Show("Delete");
        }

        /**
		@brief загрузка формы
	    */
        private void AddDojnost_Shown(object sender, EventArgs e)
        {
            //загрузка информации
            LoadData();
        }
    }
}
