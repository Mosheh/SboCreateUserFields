using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateUserFields.Domain;
using SimpleQuery;

namespace CreateUserFields.Infra
{
    public class SAPTableRepository : Domain.ISAPTableRepository
    {
        private IDataConnection _dataConnection;

        public SAPTableRepository(IDataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }
        public void Add(SAPTables sapTable)
        {
            _dataConnection.DbConnection.Insert<SAPTables>(sapTable);
        }

        public List<SAPTables> GetAll()
        {
            return _dataConnection.DbConnection.GetAll<SAPTables>().ToList();
        }

        public void Remove(int id)
        {
            var table = _dataConnection.DbConnection.Select<SAPTables>(c => c.Id == id).FirstOrDefault();
            if (table == null)
                throw new Exception($"Registro {id} não encontrado");

            _dataConnection.DbConnection.Delete(table);
        }
    }
}
