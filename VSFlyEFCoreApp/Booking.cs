using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace VSFlyEFCoreApp
{
    public class Booking
    {   
        // flight <-> passenger
        public int FlightNo { get; set; } 
        public int PassengerID { get; set; }
        public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set;  }
    }
}
