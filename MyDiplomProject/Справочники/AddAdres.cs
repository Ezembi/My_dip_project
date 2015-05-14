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
    public partial class AddAdres : Form
    {
        OracleCommand cmd;
        OracleConnection con;
        OracleDataReader dr;
        bool Lock = true;
        string oldYlica;
        string oldDom;
        int items;

        string User;
        string Password;

        public AddAdres()
        {
            InitializeComponent();
        }

        public AddAdres(string _User, string _Pass)
        {
            User = _User;
            Password = _Pass;
            InitializeComponent();
        }

        private void AddAdres_Load(object sender, EventArgs e)
        {
            con = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + User + ";Password=" + Password + ";");
            cmd = new OracleCommand("", con);
            con.Open();

            cmd.CommandText = "SELECT YLICA,NOMER_DOMA FROM SPRAVOCHNIK_ADRESOVV";
            dr = cmd.ExecuteReader();
            int i = 0;

            dataGridView1.Enabled = false;

            while (dr.Read())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();
                dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();
                i++;
            }

            dataGridView1.Enabled = true;
            timer1.Start();
            items = dataGridView1.Rows.Count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Lock = false;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!Lock)
            {
                if (e.RowIndex != dataGridView1.Rows.Count - 1)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null && dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
                    {
                        oldYlica = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        oldDom = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        button1.Enabled = true;
                    }
                }
                else
                    button1.Enabled = false;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string str0, str1;

            if (!Lock)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null && dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
                {
                    str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // Улица
                    str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // ДОМ

                    cmd.CommandText = "SELECT * FROM SPRAVOCHNIK_ADRESOVV WHERE NOMER_DOMA = '" + str1 + "' AND YLICA = '" + str0 + "'";
                    dr = cmd.ExecuteReader();

                    if (!dr.Read())
                    {
                        if (items != dataGridView1.Rows.Count)
                        {

                            cmd.CommandText = "INSERT INTO SPRAVOCHNIK_ADRESOVV (YLICA,NOMER_DOMA) VALUES ('" + str0 + "','" + str1 + "')";
                            cmd.ExecuteNonQuery();
                            items = dataGridView1.Rows.Count;
                        }
                        else
                        {
                            cmd.CommandText = "UPDATE SPRAVOCHNIK_ADRESOVV set YLICA = '" + str0 + "', NOMER_DOMA = '" + str1 + "' where YLICA = '" + oldYlica + "' and NOMER_DOMA = '" + oldDom + "'";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данный элемент уже присудствует в таблице!", "Изменение / добавление невозможно!");

                        cmd.CommandText = "SELECT YLICA,NOMER_DOMA FROM SPRAVOCHNIK_ADRESOVV";
                        dr = cmd.ExecuteReader();
                        int i = 0;
                        Lock = true;
                        dataGridView1.Rows.Clear();
                        while (dr.Read())
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();
                            dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();
                            i++;
                        }
                        items = dataGridView1.Rows.Count;
                        Lock = false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool del = true;
            button1.Enabled = false;

            if (MessageBox.Show("Удалть?", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            {
                cmd.CommandText = "SELECT * FROM ACT,SPRAVOCHNIK_ADRESOVV WHERE ACT.PK_ADRES = SPRAVOCHNIK_ADRESOVV.PK_ADRES AND SPRAVOCHNIK_ADRESOVV.YLICA = '" + oldYlica + "' AND SPRAVOCHNIK_ADRESOVV.NOMER_DOMA = '" + oldDom + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    del = false;
                    MessageBox.Show("Необходимо удалить или изменить все акты с данным адресом!", "Удаление данного адреса невозможно!");
                }

                cmd.CommandText = "SELECT * FROM PONATOI,SPRAVOCHNIK_ADRESOVV WHERE PONATOI.PK_ADRES = SPRAVOCHNIK_ADRESOVV.PK_ADRES AND SPRAVOCHNIK_ADRESOVV.YLICA = '" + oldYlica + "' AND SPRAVOCHNIK_ADRESOVV.NOMER_DOMA = '" + oldDom + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    del = false;
                    MessageBox.Show("Необходимо удалить или изменить всех понятых с данным адресом!", "Удаление данного адреса невозможно!");
                }

                if (del)
                {
                    cmd.CommandText = " DELETE FROM SPRAVOCHNIK_ADRESOVV where YLICA = '" + oldYlica + "' and NOMER_DOMA = '" + oldDom + "'";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT YLICA,NOMER_DOMA FROM SPRAVOCHNIK_ADRESOVV";
                    dr = cmd.ExecuteReader();
                    int i = 0;
                    Lock = true;
                    dataGridView1.Rows.Clear();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dr[0].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dr[1].ToString();
                        i++;
                    }
                    items = dataGridView1.Rows.Count;
                    Lock = false;
                }
            }
        }
    }
}
