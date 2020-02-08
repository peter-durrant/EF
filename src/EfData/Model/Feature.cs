using System;
using System.Collections.Generic;

namespace Hdd.EfData.Model
{
    public class Feature
    {
        public int Id { get; set; }
        public int FeatureType { get; set; }
        public DateTime Timestamp { get; set; }
        public Status Status { get; set; }
        public ICollection<Measurement> Measurements { get; set; }
    }
}
