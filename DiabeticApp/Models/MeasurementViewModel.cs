using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiabeticApp.Models
{
    public class MeasurementViewModel
    {
        public int Id { get; set; }
        public int Result { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
