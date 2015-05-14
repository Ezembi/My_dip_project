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
    public partial class AddGorod : Form
    {
        OracleCommand cmd;
        OracleConnection con;
        OracleDataReader dr;
        int items;
        bool Lock = true;
        string oldValue;
        string User;
        string Password;

        public AddGorod()
        {
            InitializeComponent();
        }

        public AddGorod(string _user, string _pass)
        {
            User = _user;
            Password = _pass;
            InitializeComponent();
        }

        private void AddGorod_Load(object sender, EventArgs e)
        {
            con = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + User + ";Password=" + Password + ";");
            cmd = new OracleCommand("", con);
            con.Open();

            cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_GORODOV";
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

                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_GORODOV where NAZVANIE = '" + str + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {
                        cmd.CommandText = "insert into SPRAVOCHNIK_GORODOV (NAZVANIE) VALUES ('" + str + "')";
                        cmd.ExecuteNonQuery();
                        items = dataGridView1.Rows.Count;
                    }
                    else
                    {
                        MessageBox.Show("Данный элемент уже присудствует в таблице!", "Добавление невозможно!");

                        cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_GORODOV";
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
                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_GORODOV where NAZVANIE = '" + str + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {

                        cmd.CommandText = " UPDATE SPRAVOCHNIK_GORODOV set NAZVANIE = '" + str + "' where NAZVANIE = '" + oldValue + "'";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Данный элемент уже присудствует в таблице!", "Изменение невозможно!");

                        cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_GORODOV";
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
            button1.Enabled = false;

            if (MessageBox.Show("Удалть?", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            {
                cmd.CommandText = "SELECT * FROM PROTOKOL, SPRAVOCHNIK_GORODOV WHERE PROTOKOL.PK_GOROD = SPRAVOCHNIK_GORODOV.PK_GOROD AND SPRAVOCHNIK_GORODOV.NAZVANIE = '" + oldValue + "'";
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    cmd.CommandText = " DELETE FROM SPRAVOCHNIK_GORODOV WHERE NAZVANIE = '" + oldValue + "'";
                    cmd.ExecuteNonQuery();

                    dataGridView1.Rows.Clear();

                    cmd.CommandText = "SELECT NAZVANIE from SPRAVOCHNIK_GORODOV";
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
                     MessageBox.Show("Необходимо удалить или изменить все протоколы с данным городом!", "Удаление города невозможно!");

            }
        }
    }
}
