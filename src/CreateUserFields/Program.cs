using CreateUserFields.Infra;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
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
            try
            {
                var sqlConn = new SQLiteConnection(@"Data Source=Data\CreateUserFields.db;Version=3;");
                var conn = new DataConnection(sqlConn);
                DataConnection.SetConnection(conn);
                Application.Run(new FormCreateUserFields());
            }
            catch (COMException ex)
            {
                MessageBox.Show($"DI API não instalada erro: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            
        }
    }
}
