using System;
using System.Collections.Generic;

namespace Hdd.EfData.Model
{
    public class Program
    {
        public int Id { get; set; }
        public int ProgramType { get; set; }
        public DateTime Timestamp { get; set; }
        public Status Status { get; set; }
        public ICollection<Feature> Features { get; set; }
    }
}
