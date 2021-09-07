using CreateUserFields.Domain;
using SimpleQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Infra
{
    public class SettingRepository : ISettingRepository
    {
        private IDataConnection _dataConnection;
        public SettingRepository(IDataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public Setting GetSetting()
        {
            var setting = _dataConnection.DbConnection.Select<Setting>(c => c.Language == "English").LastOrDefault();
            if (setting == null)
            {
                setting = new Setting { Language = "English" };
                Save(setting);
            }
            return setting;
        }

        public void Save(Setting setting)
        {
            if (setting.Id == 0)
                _dataConnection.DbConnection.Insert(setting);
            else
                _dataConnection.DbConnection.Update(setting);
        }
    }
}
