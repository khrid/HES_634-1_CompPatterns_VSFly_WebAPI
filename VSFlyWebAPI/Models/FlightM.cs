using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyWebAPI.Models
{
    public class FlightM
    {
        public int FlightNo { get; set; }

        public string Departure { get; set; }

        public string Destination { get; set; }
        public DateTime Date { get; set; }


    }
}
