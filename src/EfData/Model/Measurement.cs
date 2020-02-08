using System;

namespace Hdd.EfData.Model
{
    public class Measurement
    {
        public int Id { get; set; }
        public int MeasurementType { get; set; }
        public DateTime Timestamp { get; set; }
        public int Instance { get; set; }
        public double Actual { get; set; }
        public double Nominal { get; set; }
    }
}
