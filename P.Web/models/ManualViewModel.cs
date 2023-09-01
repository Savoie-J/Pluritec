using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace P.Web.Models
{
    public class ManualViewModel
    {
        [Required]
        [DisplayName("Selected File")]
        public HttpPostedFileBase UploadedFile { get; set; }

        [Required]
        [DisplayName("Job Number")]
        public string JobNumber { get; set; }

        [Required]
        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }

        [Required]
        [DisplayName("Starting Serial Number")]
        public int? SerialNumber { get; set; }

        public string Comments { get; set; }

        [DisplayName("Skipped Serial Numbers")]
        public string SkippedSerialNumbers { get; set; }
    }
}