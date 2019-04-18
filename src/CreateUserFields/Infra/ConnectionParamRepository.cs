using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateUserFields.Domain;
using SimpleQuery;

namespace CreateUserFields.Infra
{
    public class ConnectionParamRepository : Domain.IConnectionParamRepository
    {
        private IDataConnection _dataConnection;

        public ConnectionParamRepository(IDataConnection connection)
        {
            _dataConnection = connection;
        }
        public ConnectionParam GetConnectionParam()
        {
            return _dataConnection.DbConnection.GetAll<ConnectionParam>().ToList().FirstOrDefault();
        }

        public void Save(ConnectionParam connectionParam)
        {
            if (connectionParam.Id == 0)
                _dataConnection.DbConnection.Insert<ConnectionParam>(connectionParam);
            else
                _dataConnection.DbConnection.Update<ConnectionParam>(connectionParam);
        }
    }
}
