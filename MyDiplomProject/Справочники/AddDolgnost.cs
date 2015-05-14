using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;

namespace MyDiplomProject
{
    public partial class AddDolgnost : Form
    {
        OracleCommand cmd;
        OracleConnection con;
        OracleDataReader dr;
        int items;
        bool Lock = true;
        string oldValue;
        string User;
        string Password;

        public AddDolgnost()
        {
            InitializeComponent();
        }

        public AddDolgnost(string _user, string _pass)
        {
            User = _user;
            Password = _pass;
            InitializeComponent();
        }

        private void AddDolgnost_Load(object sender, EventArgs e)
        {
            con = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + User + ";Password=" + Password + ";");
            cmd = new OracleCommand("", con);
            con.Open();

            cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_DOLGNOSTEI";
            dr = cmd.ExecuteReader();

            dataGridView1.Rows.Clear();

            int i = 0;
            while (dr.Read())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();
                i++;
            }
            dataGridView1.Enabled = true;
            timer1.Start();
            items = dataGridView1.Rows.Count;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string str;

            if (!Lock)
            {
                if (items != dataGridView1.Rows.Count)
                {
                    str = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTEI where NAZVANIE = '" + str + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {
                        cmd.CommandText = "insert into SPRAVOCHNIK_DOLGNOSTEI (NAZVANIE) VALUES ('" + str + "')";
                        cmd.ExecuteNonQuery();
                        items = dataGridView1.Rows.Count;
                    }
                    else
                    {
                        MessageBox.Show("Данный элемент уже присудствует в таблице!", "Добавление невозможно!");

                        cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTEI";
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
                {
                    str = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTEI where NAZVANIE = '" + str + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {

                        cmd.CommandText = " UPDATE SPRAVOCHNIK_DOLGNOSTEI set NAZVANIE = '" + str + "' where NAZVANIE = '" + oldValue + "'";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Данный элемент уже присудствует в таблице!", "Изменение невозможно!");

                        cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTEI";
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
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!Lock)
            {
                if (e.RowIndex != dataGridView1.Rows.Count - 1)
                {
                    oldValue = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    button1.Enabled = true;
                }
                else
                    button1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Lock = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool del = true;
            button1.Enabled = false;

            if (MessageBox.Show("Удалть?", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            {
                cmd.CommandText = "SELECT * FROM KYRER, SPRAVOCHNIK_DOLGNOSTEI WHERE KYRER.PK_DOLGNOST = SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST AND SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldValue + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    del = false;
                    MessageBox.Show("Необходимо удалить или изменить всех лиц, передавших вещественные доказательства с данной должностью!", "Удаление данной должности невозможно!");
                }

                cmd.CommandText = "SELECT * FROM POLISE, SPRAVOCHNIK_DOLGNOSTEI WHERE POLISE.PK_DOLGNOST = SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST AND SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldValue + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    del = false;
                    MessageBox.Show("Необходимо удалить или изменить всех уполномоченных с данной должностью!", "Удаление данной должности невозможно!");
                }

                cmd.CommandText = "SELECT * FROM YRANITEL, SPRAVOCHNIK_DOLGNOSTEI WHERE YRANITEL.PK_DOLGNOST = SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST AND SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldValue + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    del = false;
                    MessageBox.Show("Необходимо удалить или изменить всех хранителей с данной должностью!", "Удаление данной должности невозможно!");
                }

                if (del)
                {
                    cmd.CommandText = " DELETE FROM SPRAVOCHNIK_DOLGNOSTEI WHERE NAZVANIE = '" + oldValue + "'";
                    cmd.ExecuteNonQuery();

                    dataGridView1.Rows.Clear();

                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_DOLGNOSTEI";
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
            }
        }
    }
}
