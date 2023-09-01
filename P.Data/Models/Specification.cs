using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.Data.Models
{
    public class Specification
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        [Index(IsUnique = true)]
        [DisplayName("Part Number")]
        public string PartNumber { get; set; }

        [DisplayName("Layer I")]
        public int? LayerOne { get; set; }

        [DisplayName("Layer II")]
        public int? LayerTwo { get; set; }

        [DisplayName("Tolerance")]
        public double? Tolerance { get; set; }

        [DisplayName("Offset")]
        public double? Offset { get; set; }

        [JsonIgnore]
        public virtual List<Log> Logs { get; set; }

        [NotMapped]
        public bool IsSetup => this.LayerOne.HasValue
                            && this.LayerTwo.HasValue
                            && this.Tolerance.HasValue
                            && this.Offset.HasValue;
    }
}
