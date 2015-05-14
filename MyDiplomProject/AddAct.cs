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
    public partial class AddAct : Form
    {

        OracleCommand cmd;
        OracleConnection con;
        OracleDataReader dr;

        string User;
        string Password;
        string pk_act;

        public AddAct()
        {
            InitializeComponent();
        }

        public AddAct(string _User, string _Pass, string _pk_act)
        {
            User = _User;
            Password = _Pass;
            pk_act = _pk_act;
            InitializeComponent();
        }

        void UpDate()
        {
            con = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + User + ";Password=" + Password + ";");
            cmd = new OracleCommand("", con);
            con.Open();
        }

        void LoadVeshDok()
        {

        }

        private void AddAct_Load(object sender, EventArgs e)
        {
            UpDate();

        }
    }
}
