﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace P.Data.Models
{
    public class Log
    {
        public int ID { get; set; }

        [ForeignKey("Specification")]
        public int SpecificationID { get; set; }

        public virtual Specification Specification { get; set; }



        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LogDate { get; set; }

        [Required]
        [DisplayName("Job Number")]
        public string JobNumber { get; set; }

        [Required]
        [DisplayName("Operator")]
        public string Operator { get; set; }

        [Required]
        [DisplayName("Serial #")]
        public int? SerialNumber { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }

        [Required]
        [MaxLength(256)]
        [Index(IsUnique = true)]
        [DisplayName("File Name")]
        public string UploadName { get; set; }

        public byte[] UploadData { get; set; }

        public Guid UploadGuid { get; set; }

        [JsonIgnore]
        public virtual List<SkippedSerialNumber> SkippedSerialNumbers { get; set; }

        [NotMapped]
        [DisplayName("Skipped Serial Numbers")]
        public string FormattedSerialNumbers => string.Join(", ", this.SkippedSerialNumbers?.Select(s => s.SerialNumber));

        [JsonIgnore]
        public virtual List<Measurement> Measurements { get; set; }

        [NotMapped]
        [DisplayName("Part Number")]
        public string PartNumber => this.Specification?.PartNumber;

        [NotMapped]
        public string FileName => $"{this.LogDate:yyyy-MM-dd_HH.mm}_{this.UploadGuid}.mdb";

        [NotMapped]
        [DisplayName("Skipped Serial Numbers")]
        public string SerialNumbers { get; set; }

        [NotMapped]
        public string StatusColor
        {
            get
            {
                if (!this.Measurements.Any())
                    return "primary";

                if (!this.Measurements.Any(m => m.HasMeasurements))
                    return "warning";

                if (this.Measurements.Any(m => m.HasMeasurements && !m.IsInSpecification()))
                    return "danger";

                return "success";
            }
        }
    }
}
