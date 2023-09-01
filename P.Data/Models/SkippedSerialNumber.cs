using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.Data.Models
{
    public class SkippedSerialNumber
    {
        public int ID { get; set; }

        [ForeignKey("Log")]
        public int LogID { get; set; }

        public int SerialNumber { get; set; }

        public virtual Log Log { get; set; }
    }
}
