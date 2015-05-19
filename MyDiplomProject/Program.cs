using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyDiplomProject
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1("root", "ezembi007", "polise", "127.0.0.1", "Материалы проверки / уголовные дела", "delo", new string[] { "Номер материала", "Дата поступления материала", "Номер дела", "Дата возбуждения дела", "Уполномоченный", "Подразделение" }, new string[] { "PK_Dela", "Nomer_materiala", "DateofM", "Nomer_dela", "DateofV", "pk_polise", "PK_Raiona" }, "polise", new string[] { "pk_polise", "surname", "Pname", "second_name" }, "spravochnik_pod", new string[] { "PK_Raiona", "Nazv" }));
        }
    }
}
