using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace CreateUserFields.Domain
{
    [Table("SAPTables")]
    public class SAPTables
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string System { get; set; }

        [NotMapped]
        public bool IsSystem
        {
            get { return System == "Y"; }
        }
    }
}
