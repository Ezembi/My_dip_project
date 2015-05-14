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
    public partial class AddAct : Form
    {



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
