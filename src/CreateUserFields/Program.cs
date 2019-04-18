using CreateUserFields.Infra;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateUserFields
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var sqlConn = new SQLiteConnection(@"Data Source=Data\CreateUserFields.db;Version=3;");
            var conn = new DataConnection(sqlConn);
            DataConnection.SetConnection(conn);
            Application.Run(new FormCreateUserFields());
        }
    }
}
