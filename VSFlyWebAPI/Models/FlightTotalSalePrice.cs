using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyWebAPI.Models
{
    public class FlightTotalSalePrice
    {
        public int FlightNo { get; set; }

        public double TotalSalePrice { get; set; }

        public int PassengersCount { get; set; }

    }
}
