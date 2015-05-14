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
    public partial class AddPeredal : Form
    {
        bool Lock = true;
        int items;
        string oldCell0;
        string oldCell1;
        string oldCell2;

        string User;
        string Password;

        public AddPeredal()
        {
            InitializeComponent();
        }

        public AddPeredal(string _User, string _Pass)
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

            //cmd.CommandText = "SELECT NAZVANIE FROM SPRAVOCHNIK_DOLGNOSTEI";
            //dr = cmd.ExecuteReader();
            //this.Column2.Items.Clear();
            //while (dr.Read())
            //{
            //    this.Column2.Items.Add(dr[0].ToString());
            //}

            //cmd.CommandText = "SELECT NOMER FROM SPRAVOCHNIK_NOMEROV";
            //dr = cmd.ExecuteReader();
            //this.Column3.Items.Clear();
            //while (dr.Read())
            //{
            //    this.Column3.Items.Add(dr[0].ToString());
            //}

            //cmd.CommandText = "SELECT KYRER.FIO,SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE,SPRAVOCHNIK_NOMEROV.NOMER FROM KYRER , SPRAVOCHNIK_DOLGNOSTEI, SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST = KYRER.PK_DOLGNOST AND SPRAVOCHNIK_NOMEROV.PK_NOMER = KYRER.PK_NOMER";

            //dr = cmd.ExecuteReader();
            //int i = 0;

            //dataGridView1.Enabled = false;

            //while (dr.Read())
            //{
            //    dataGridView1.Rows.Add();
            //    for (int j = 0; j < 3; j++)
            //    {
            //        dataGridView1.Rows[i].Cells[j].Value = dr[j].ToString();
            //    }
            //    i++;
            //}

            //dataGridView1.Enabled = true;
            //timer1.Start();
            //items = dataGridView1.Rows.Count;
        }

        private void AddPeredal_Load(object sender, EventArgs e)
        {
            UpDate();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //string str0, str1, str2;
            //bool NotNull = true;

            //if (!Lock)
            //{
            //    for (int i = 0; i < 3; i++)
            //        if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
            //            NotNull = false;
            //    if (NotNull)
            //    {
            //        str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // фио
            //        str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // должность
            //        str2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();    // т. номер

            //        cmd.CommandText = "SELECT KYRER.FIO,SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE,SPRAVOCHNIK_NOMEROV.NOMER FROM KYRER , SPRAVOCHNIK_DOLGNOSTEI, SPRAVOCHNIK_NOMEROV WHERE KYRER.FIO = '" + str0 + "' AND KYRER.PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + str1 + "') AND KYRER.PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + str2 + "')";
            //        dr = cmd.ExecuteReader();

            //        if (!dr.Read())
            //        {
            //            if (items != dataGridView1.Rows.Count)
            //            {
            //                cmd.CommandText = "INSERT INTO KYRER (FIO,PK_DOLGNOST,PK_NOMER) VALUES ('" + str0 + "', (SELECT PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + str1 + "'), (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + str2 + "'))";
            //                cmd.ExecuteNonQuery();
            //                items = dataGridView1.Rows.Count;
            //            }
            //            else
            //            {
            //                cmd.CommandText = "UPDATE KYRER SET FIO = '" + str0 + "', PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + str1 + "'), PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + str2 + "') WHERE FIO = '" + oldCell0 + "' AND PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldCell1 + "') AND PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + oldCell2 + "')";
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

            //        for (int i = 0; i < 3; i++)
            //            if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
            //                NotNull = false;

            //        if (NotNull)
            //        {
            //            oldCell0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // фио
            //            oldCell1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // должность
            //            oldCell2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();    // номер
                       
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
            //    cmd.CommandText = "SELECT * FROM KYRER, ACT WHERE KYRER.PK_KYRER = ACT.PK_KYRER AND KYRER.FIO = '" + oldCell0 + "' AND KYRER.PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldCell1 + "') AND KYRER.PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + oldCell2 + "')";
            //    dr = cmd.ExecuteReader();
            //    if (!dr.Read())
            //    {
            //        cmd.CommandText = "DELETE FROM KYRER WHERE KYRER.FIO = '" + oldCell0 + "' AND KYRER.PK_DOLGNOST = (SELECT SPRAVOCHNIK_DOLGNOSTEI.PK_DOLGNOST FROM SPRAVOCHNIK_DOLGNOSTEI WHERE SPRAVOCHNIK_DOLGNOSTEI.NAZVANIE = '" + oldCell1 + "') AND KYRER.PK_NOMER = (SELECT SPRAVOCHNIK_NOMEROV.PK_NOMER FROM SPRAVOCHNIK_NOMEROV WHERE SPRAVOCHNIK_NOMEROV.NOMER = '" + oldCell2 + "')";
            //        cmd.ExecuteNonQuery();

            //        Lock = true;
            //        UpDate();
            //        Lock = false;
            //    }
            //    else
            //        MessageBox.Show("Необходимо удалить или изменить всеакты приёма передачи с данным лицом!", "Удаление данного лица невозможно!");
            //}
        }
    }
}
