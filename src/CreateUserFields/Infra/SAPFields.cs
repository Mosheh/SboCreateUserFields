using CreateUserFields.Common;
using CreateUserFields.Domain;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Infra
{
    public class SAPFieldRepository : ISAPFieldRepository
    {
        private Company _company;

        public SAPFieldRepository(Company company)
        {
            _company = company;
        }
        public IEnumerable<FieldMD> GetAll(string tableName)
        {
            var sql = $@"SELECT
                    [FieldID],
                    [TableID],
                    [AliasID],
                    [Descr]
                    FROM
                    [CUFD]
                    WHERE
                    [TableID]='{tableName}'".TSqlToAnsi();
            var rs = _company.ExecuteRS(sql);
            var fields = new List<FieldMD>();
            while (!rs.EoF)
            {
                var field = new FieldMD
                {
                    FieldID = rs.Fields.Item(nameof(FieldMD.FieldID)).Value.To<int>(),
                    AliasID = rs.Fields.Item(nameof(FieldMD.AliasID)).Value.ToString(),
                    Descr = rs.Fields.Item(nameof(FieldMD.Descr)).Value.ToString(),
                    TableID = rs.Fields.Item(nameof(FieldMD.TableID)).Value.ToString()
                };
                fields.Add(field);
                rs.MoveNext();
            }

            return fields;
        }

        public void Remove(string tableName, int fieldId)
        {
            var fieldMd = _company.GetBusinessObject(BoObjectTypes.oUserFields) as UserFieldsMD;
            _company.Connect();
            if (fieldMd.GetByKey(tableName, fieldId))
            {
                var success = fieldMd.Remove() == 0;

                if (!success)
                {
                    var code = _company.GetLastErrorCode();
                    if (code == -1120)
                    {                        
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(fieldMd);
                    }
                    throw new Exception(_company.GetLastErrorDescription());
                }
            }

            
            System.Runtime.InteropServices.Marshal.ReleaseComObject(fieldMd);
        }
    }
}
