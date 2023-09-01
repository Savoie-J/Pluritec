using P.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace P.Web.Models
{
    public class DataFile
    {
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [DisplayName("Last Modified")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateCreated { get; set; }
    }
}