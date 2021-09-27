using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Domain
{
    public interface ISAPFieldRepository
    {
        IEnumerable<FieldMD> GetAll(string tableName);
        void Remove(string tableName, int fieldId);
    }

    public class FieldMD
    {
        public int FieldID { get; set; }
        public string AliasID { get; set; }
        public string Descr { get; set; }
        public string TableID { get; set; }
    }
}
