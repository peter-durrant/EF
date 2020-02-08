namespace Hdd.EfData.Model
{
    public class BoundedMeasurement : Measurement
    {
        public Status Status { get; set; }
        public double Upper { get; set; }
        public double Lower { get; set; }
    }
}
