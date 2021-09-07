using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Domain
{
    public interface ISettingRepository
    {
        void Save(Setting setting);
        Setting GetSetting();
    }
}
