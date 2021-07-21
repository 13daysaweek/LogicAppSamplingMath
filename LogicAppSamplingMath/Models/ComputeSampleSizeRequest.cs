using System;
using System.Collections.Generic;
using System.Text;

namespace LogicAppSamplingMath.Models
{
    public class ComputeSampleSizeRequest
    {
        public int SampleSize { get; set; }
        
        public int TotalNumberOfImages { get; set; }
    }
}
