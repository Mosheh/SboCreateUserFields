using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Domain
{
    public interface ISAPTableRepository
    {
        void Add(SAPTables sapTable);
        void Remove(int id);
        List<SAPTables> GetAll();
    }
}
