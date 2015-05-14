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
    public partial class AddSpecMen : Form
    {

        bool Lock = true;
        int items;
        string oldCell0;
        string oldCell1;

        string User;
        string Password;

        public AddSpecMen()
        {
            InitializeComponent();
        }

        public AddSpecMen(string _User, string _Pass)
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

            //cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_OBLASTEI_SPEC";
            //dr = cmd.ExecuteReader();
            //this.Column2.Items.Clear();
            //while (dr.Read())
            //{
            //    this.Column2.Items.Add(dr[0].ToString());
            //}

            //cmd.CommandText = "SELECT SPECIALIST.FIO,SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE FROM SPECIALIST , SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL = SPECIALIST.PK_SPECIAL";

            //dr = cmd.ExecuteReader();
            //int i = 0;

            //dataGridView1.Enabled = false;

            //while (dr.Read())
            //{
            //    dataGridView1.Rows.Add();
            //    for (int j = 0; j < 2; j++)
            //    {
            //        dataGridView1.Rows[i].Cells[j].Value = dr[j].ToString();
            //    }
            //    i++;
            //}

            //dataGridView1.Enabled = true;
            //timer1.Start();
            //items = dataGridView1.Rows.Count;
        }

        private void AddSpecMen_Load(object sender, EventArgs e)
        {
            UpDate();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //string str0, str1;
            //bool NotNull = true;

            //if (!Lock)
            //{
            //    for (int i = 0; i < 2; i++)
            //        if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
            //            NotNull = false;
            //    if (NotNull)
            //    {
            //        str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // фио
            //        str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // должность

            //        cmd.CommandText = "SELECT SPECIALIST.FIO,SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE FROM SPECIALIST , SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPECIALIST.FIO = '" + str0 + "' AND SPECIALIST.PK_SPECIAL = (SELECT SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL FROM SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE = '" + str1 + "')";
            //        dr = cmd.ExecuteReader();

            //        if (!dr.Read())
            //        {
            //            if (items != dataGridView1.Rows.Count)
            //            {
            //                cmd.CommandText = "INSERT INTO SPECIALIST (FIO,PK_SPECIAL) VALUES ('" + str0 + "', (SELECT PK_SPECIAL FROM SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE = '" + str1 + "'))";
            //                cmd.ExecuteNonQuery();
            //                items = dataGridView1.Rows.Count;
            //            }
            //            else
            //            {
            //                cmd.CommandText = "UPDATE SPECIALIST SET FIO = '" + str0 + "', PK_SPECIAL = (SELECT SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL FROM SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE = '" + str1 + "') WHERE FIO = '" + oldCell0 + "' AND PK_SPECIAL = (SELECT SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL FROM SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE = '" + oldCell1 + "')";
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

            //        for (int i = 0; i < 2; i++)
            //            if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
            //                NotNull = false;

            //        if (NotNull)
            //        {
            //            oldCell0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // фио
            //            oldCell1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // должность
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
            //    cmd.CommandText = "SELECT * FROM SPECIALIST, PROTOKOL WHERE PROTOKOL.PK_SPEC = SPECIALIST.PK_SPEC AND SPECIALIST.FIO = '" + oldCell0 + "' AND SPECIALIST.PK_SPECIAL = (SELECT SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL FROM SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE = '" + oldCell1 + "')";
            //    dr = cmd.ExecuteReader();
            //    if (!dr.Read())
            //    {
            //        cmd.CommandText = "DELETE FROM SPECIALIST WHERE SPECIALIST.FIO = '" + oldCell0 + "' AND SPECIALIST.PK_SPECIAL = (SELECT SPRAVOCHNIK_OBLASTEI_SPEC.PK_SPECIAL FROM SPRAVOCHNIK_OBLASTEI_SPEC WHERE SPRAVOCHNIK_OBLASTEI_SPEC.NAZVANIE = '" + oldCell1 + "')";
            //        cmd.ExecuteNonQuery();

            //        Lock = true;
            //        UpDate();
            //        Lock = false;
            //    }
            //    else
            //        MessageBox.Show("Необходимо удалить или изменить все протоколы с данным специалистом!", "Удаление данного специалиста невозможно!");
            //}
        }
    }
}
