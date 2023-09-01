using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.Data.Models
{
    public class Layer
    {
        public int ID { get; set; }

        [ForeignKey("Measurement")]
        public int MeasurementID { get; set; }

        public virtual Measurement Measurement { get; set; }

        public int NLayer { get; set; }

        [DisplayName("X11")]
        public double ABSposX11 { get; set; }

        [DisplayName("Y11")]
        public double ABSposY11 { get; set; }

        [DisplayName("X12")]
        public double ABSposX12 { get; set; }

        [DisplayName("Y12")]
        public double ABSposY12 { get; set; }

        [DisplayName("X21")]
        public double ABSposX21 { get; set; }

        [DisplayName("Y21")]
        public double ABSposY21 { get; set; }

        [DisplayName("X22")]
        public double ABSposX22 { get; set; }

        [DisplayName("Y22")]
        public double ABSposY22 { get; set; }
    }
}
