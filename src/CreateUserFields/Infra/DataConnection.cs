using CreateUserFields.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Infra
{
    public class DataConnection : IDataConnection
    {
        static DataConnection _instance;
        public DataConnection(SQLiteConnection connection)
        {
            DbConnection = connection;            
        }
        public IDbConnection DbConnection { get; set; }

        public static DataConnection Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("Não foi possível se conectar a base do aplicativo");
                return _instance;
            }

            
        }

        public static void SetConnection(DataConnection connection)
        {
            _instance = connection;
            _instance.DbConnection.Open();
        }
    }
}
