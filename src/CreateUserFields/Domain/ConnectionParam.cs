using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Domain
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Connection")]
    public class ConnectionParam
    {
        public int Id { get; set; }
        public int ServerType { get; set; }
        public string Server { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public string CompanyDb { get; set; }
        public string SAPUser { get; set; }
        public string SAPPassword { get; set; }
    }
}
