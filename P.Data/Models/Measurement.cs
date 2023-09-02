using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace P.Data.Models
{
    public class Measurement
    {
        public int ID { get; set; }

        [ForeignKey("Log")]
        public int LogID { get; set; }

        public virtual Log Log { get; set; }



        public DateTime Time { get; set; }

        public virtual List<Layer> Layers { get; set; }


        [NotMapped]
        public Specification Specification => this.Log?.Specification;

        [NotMapped]
        public double Offset => this.Specification.Offset ?? 0;

        [NotMapped]
        private int Layer1Num => this.Specification?.LayerOne ?? -1;

        [NotMapped]
        public Layer Layer1 => this.Layers.FirstOrDefault(l => l.NLayer == this.Layer1Num);

        [NotMapped]
        private int Layer2Num => this.Specification?.LayerTwo ?? -1;

        [NotMapped]
        public Layer Layer2 => this.Layers.FirstOrDefault(l => l.NLayer == this.Layer2Num);

        [NotMapped]
        public bool HasMeasurements => this.Layer1 != null && this.Layer2 != null;

        [NotMapped]
        [DisplayName("X11")]
        public double ABSposX11 => Math.Abs(this.Layer1.ABSposX11 - this.Layer2.ABSposX11);

        [NotMapped]
        [DisplayName("X12")]
        public double ABSposX12 => Math.Abs(this.Layer1.ABSposX12 - this.Layer2.ABSposX12);

        [NotMapped]
        [DisplayName("X21")]
        public double ABSposX21 => Math.Abs(this.Layer1.ABSposX21 - this.Layer2.ABSposX21);

        [NotMapped]
        [DisplayName("X22")]
        public double ABSposX22 => Math.Abs(this.Layer1.ABSposX22 - this.Layer2.ABSposX22);

        [NotMapped]
        [DisplayName("Y11")]
        public double ABSposY11 => Math.Abs(this.Layer1.ABSposY11 - this.Layer2.ABSposY11) - this.Offset;

        [NotMapped]
        [DisplayName("Y12")]
        public double ABSposY12 => Math.Abs(this.Layer1.ABSposY12 - this.Layer2.ABSposY12) - this.Offset;

        [NotMapped]
        [DisplayName("Y21")]
        public double ABSposY21 => Math.Abs(this.Layer1.ABSposY21 - this.Layer2.ABSposY21) - this.Offset;

        [NotMapped]
        [DisplayName("Y22")]
        public double ABSposY22 => Math.Abs(this.Layer1.ABSposY22 - this.Layer2.ABSposY22) - this.Offset;

        public bool IsInSpecification()
        {
            if (!this.Specification.IsSetup)
                return true;

            double tolerance = this.Specification.Tolerance.Value;

            if (ABSposX11 > tolerance)
                return false;

            if (ABSposY11 > tolerance)
                return false;

            if (ABSposX12 > tolerance)
                return false;

            if (ABSposY12 > tolerance)
                return false;

            if (ABSposX21 > tolerance)
                return false;

            if (ABSposY21 > tolerance)
                return false;

            if (ABSposX22 > tolerance)
                return false;

            if (ABSposY22 > tolerance)
                return false;

            return true;
        }
    }
}
