using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyWebAPI.Models
{
    public class BookingM
    {
        public string Surname { get; set; }

        public string GivenName { get; set; }

        public int FlightNo { get; set; }

        public double SalePrice { get; set; }
    }
}
