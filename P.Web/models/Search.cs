using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using P.Data.Models;

namespace P.Web.Models
{
    public class Search
    {
        [DisplayName("Date")]
        public bool UseLogDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? LogDateMin { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? LogDateMax { get; set; }

        [DisplayName("Job Number")]
        public bool UseJobNumber { get; set; }
        public string JobNumber { get; set; }

        [DisplayName("Comments")]
        public bool UseComments { get; set; }
        public string Comments { get; set; }

        [DisplayName("Part Number")]
        public bool UsePartNumber { get; set; }
        public string PartNumber { get; set; }

        public List<Log> Results { get; set; }

        public Search() 
        { 
            Results = new List<Log>();
        }
    }
}