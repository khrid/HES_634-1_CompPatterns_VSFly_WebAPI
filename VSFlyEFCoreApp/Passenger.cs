using System;
using System.Collections.Generic;
using System.Text;

namespace VSFlyEFCoreApp
{
    public class Passenger:Person
    {
        public int Weight { get; set; }

        // Flight <---------------- Booking -------------------> Passenger
        public virtual ICollection<Booking> BookingSet { get; set; }
    }
}
