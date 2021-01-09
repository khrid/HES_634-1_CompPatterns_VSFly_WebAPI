using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyWebAPI.Models
{
    public class DestinationBookings
    {

        public string Destination { get; set; }

        public List<BookingM> BookingSet { get; set; }

        public DestinationBookings()
        {
            BookingSet = new List<BookingM>();
        }

    }
}
