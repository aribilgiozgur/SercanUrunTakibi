using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SercanUrunTakibi.Models
{
    public class Measurement : BaseClass
    {
        public int MeasurementId { get; set; }
        public String MeasurementText { get; set; }

        public Measurement(int measurementId)
        {

        }
    }
}