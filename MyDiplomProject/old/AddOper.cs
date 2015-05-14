using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyDiplomProject
{
    public partial class AddOper : Form
    {
        bool Lock = true;
        int items;
        string oldCell0;
        string oldCell1;
        string oldCell2;
        string oldCell3;
        string oldCell4;

        string User;
        string Password;

        public AddOper()
        {
            InitializeComponent();
        }

        public AddOper(string _User, string _Pass)
        {
            User = _User;
            Password = _Pass;
            InitializeComponent();
        }

        void UpDate()
        {
            //con = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + User + ";Password=" + Password + ";");
            //cmd = new OracleCommand("", con);
            //con.Open();

            //dataGridView1.Rows.Clear();

            //cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC";
            //dr = cmd.ExecuteReader();
            //this.Column6.Items.Clear();
            //while (dr.Read())
            //{
            //    this.Column6.Items.Add(dr[0].ToString());
            //}

            //cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_DOLGNOSTEI";
            //dr = cmd.ExecuteReader();
            //this.Column2.Items.Clear();
            //while (dr.Read())
            //{
            //    this.Column2.Items.Add(dr[0].ToString());
            //}

            //cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_ZVANII";
            //dr = cmd.ExecuteReader();
            //this.Column4.Items.Clear();
            //while (dr.Read())
            //{
            //    this.Column4.Items.Add(dr[0].ToString());
            //}

            //cmd.CommandText = "SELECT NOMER FROM SPRAVOCHNIK_NOMEROV";
            //dr = cmd.ExecuteReader();
            //this.Column3.Items.Clear();
            //while (dr.Read())
            //{
            //    this.Column3.Items.Add(dr[0].ToString());
            //}

            //cmd.CommandText = "SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE, POLISE.FIO, SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE, SPRAVOCHNIK_ZVANII.NAZVANIE, SPRAVOCHNIK_NOMEROV.NOMER FROM POLISE, SPRAVOCHNIK_DOLGNOSTEI,SPRAVOCHNIK_DOLGNOSTNIX_LIC,SPRAVOCHNIK_ZVANII,SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO = POLISE.PK_DOLGNOSTOE_LICO AND SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST = POLISE.PK_DOLGNOST AND SPRAVOCHNIK_ZVANII.PK_ZVANIE = POLISE.PK_ZVANIE AND SPRAVOCHNIK_NOMEROV.PK_NOMER = POLISE.PK_NOMER";

            //dr = cmd.ExecuteReader();
            //int i = 0;

            //dataGridView1.Enabled = false;

            //while (dr.Read())
            //{
            //    dataGridView1.Rows.Add();
            //    for (int j = 0; j < 5; j++)
            //    {
            //        dataGridView1.Rows[i].Cells[j].Value = dr[j].ToString();
            //    }
            //    i++;
            //}

            //dataGridView1.Enabled = true;
            //timer1.Start();
            //items = dataGridView1.Rows.Count;
        }

        private void AddOper_Load(object sender, EventArgs e)
        {
            UpDate();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //string str0, str1, str2, str3, str4;
            //bool NotNull = true;

            //if (!Lock)
            //{
            //    for (int i = 0; i < 5; i++)
            //        if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
            //            NotNull = false;

            //    if (NotNull)
            //    {
            //        str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // должностное лицо
            //        str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // фио
            //        str2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();    // должность
            //        str3 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();    // звание
            //        str4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();    // т. номер

            //        cmd.CommandText = "SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE, POLISE.FIO, SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE, SPRAVOCHNIK_ZVANII.NAZVANIE, SPRAVOCHNIK_NOMEROV.NOMER FROM POLISE, SPRAVOCHNIK_DOLGNOSTEI,SPRAVOCHNIK_DOLGNOSTNIX_LIC,SPRAVOCHNIK_ZVANII,SPRAVOCHNIK_NOMEROV WHERE POLISE.PK_DOLGNOSTOE_LICO = (SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE = '" + str0 + "') AND POLISE.FIO = '" + str1 + "' AND POLISE.PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + str2 + "') AND POLISE.PK_ZVANIE = (SELECT SPRAVOCHNIK_ZVANII.PK_ZVANIE FROM SPRAVOCHNIK_ZVANII WHERE SPRAVOCHNIK_ZVANII.NAZVANIE = '" + str3 + "') AND POLISE.PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + str4 + "')";
            //        dr = cmd.ExecuteReader();

            //        if (!dr.Read())
            //        {
            //            if (items != dataGridView1.Rows.Count)
            //            {
            //                cmd.CommandText = "INSERT INTO POLISE (PK_DOLGNOSTOE_LICO, FIO, PK_DOLGNOST, PK_ZVANIE, PK_NOMER) VALUES ( (SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE = '" + str0 +  "'), '" + str1 + "', (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + str2 + "'), (SELECT SPRAVOCHNIK_ZVANII.PK_ZVANIE FROM SPRAVOCHNIK_ZVANII WHERE SPRAVOCHNIK_ZVANII.NAZVANIE = '" + str3 + "'), (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + str4 + "') )";
            //                cmd.ExecuteNonQuery();
            //                items = dataGridView1.Rows.Count;
            //            }
            //            else
            //            {
            //                cmd.CommandText = "UPDATE POLISE SET PK_DOLGNOSTOE_LICO = (SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE = '" + str0 + "'), FIO = '" + str1 + "', PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + str2 + "'), PK_ZVANIE = (SELECT SPRAVOCHNIK_ZVANII.PK_ZVANIE FROM SPRAVOCHNIK_ZVANII WHERE SPRAVOCHNIK_ZVANII.NAZVANIE = '" + str3 + "'), PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + str4 + "') WHERE POLISE.PK_DOLGNOSTOE_LICO = (SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE = '" + oldCell0 + "') AND POLISE.FIO = '" + oldCell1 + "' AND POLISE.PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldCell2 + "') AND POLISE.PK_ZVANIE = (SELECT SPRAVOCHNIK_ZVANII.PK_ZVANIE FROM SPRAVOCHNIK_ZVANII WHERE SPRAVOCHNIK_ZVANII.NAZVANIE = '" + oldCell3 + "') AND POLISE.PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + oldCell4 + "')";
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Данный элемент уже присудствует в таблице!", "Изменение / добавление невозможно!");

            //            Lock = true;
            //            UpDate();
            //            Lock = false;
            //        }
            //    }
            //}
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (!Lock)
            //{
            //    if (e.RowIndex != dataGridView1.Rows.Count - 1)
            //    {
            //        bool NotNull = true;

            //        for (int i = 0; i < 5; i++)
            //            if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
            //                NotNull = false;

            //        if (NotNull)
            //        {
            //            oldCell0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // фио
            //            oldCell1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // должность
            //            oldCell2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();    // номер
            //            oldCell3 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();    // номер
            //            oldCell4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();    // номер

            //            button1.Enabled = true;
            //        }
            //    }
            //    else
            //        button1.Enabled = false;
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Lock = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Enabled = false;
            //if (MessageBox.Show("Удалть?", "Удаление", MessageBoxButtons.YesNo).ToString() == "Yes")
            //{
            //    cmd.CommandText = "SELECT * FROM POLISE, PROTOKOL WHERE POLISE.PK_POLISE = PROTOKOL.PK_POLISE AND POLISE.PK_DOLGNOSTOE_LICO = (SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE = '" + oldCell0 + "') AND POLISE.FIO = '" + oldCell1 + "' AND POLISE.PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldCell2 + "') AND POLISE.PK_ZVANIE = (SELECT SPRAVOCHNIK_ZVANII.PK_ZVANIE FROM SPRAVOCHNIK_ZVANII WHERE SPRAVOCHNIK_ZVANII.NAZVANIE = '" + oldCell3 + "') AND POLISE.PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + oldCell4 + "')";
            //    dr = cmd.ExecuteReader();
            //    if (!dr.Read())
            //    {
            //        cmd.CommandText = "DELETE FROM POLISE WHERE POLISE.PK_DOLGNOSTOE_LICO = (SELECT SPRAVOCHNIK_DOLGNOSTNIX_LIC.PK_DOLGNOSTOE_LICO FROM SPRAVOCHNIK_DOLGNOSTNIX_LIC WHERE SPRAVOCHNIK_DOLGNOSTNIX_LIC.NAZVANIE = '" + oldCell0 + "') AND POLISE.FIO = '" + oldCell1 + "' AND POLISE.PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldCell2 + "') AND POLISE.PK_ZVANIE = (SELECT SPRAVOCHNIK_ZVANII.PK_ZVANIE FROM SPRAVOCHNIK_ZVANII WHERE SPRAVOCHNIK_ZVANII.NAZVANIE = '" + oldCell3 + "') AND POLISE.PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + oldCell4 + "')";
            //        cmd.ExecuteNonQuery();

            //        Lock = true;
            //        UpDate();
            //        Lock = false;
            //    }
            //     else
            //        MessageBox.Show("Необходимо удалить или изменить все протоколы с данным уполномоченным!", "Удаление уполномоченного невозможно!");
            //}
        }
    }
}
