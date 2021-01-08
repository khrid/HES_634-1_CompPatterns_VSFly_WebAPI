using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSFlyEFCoreApp;
using VSFlyWebAPI.Models;

namespace VSFlyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {

        private readonly WWWingsContext _context;

        public StatisticsController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Statistics/5
        [HttpGet("{flightNo}")]
        //[Route("FlightTotalSalePrice")]
        public async Task<ActionResult<FlightTotalSalePrice>> GetFlightTotalSalePrice(int flightNo)
        {
            var flight = await _context.FlightSet.FindAsync(flightNo);
            var flightTotalSalePrice = new FlightTotalSalePrice();

            if (flight == null)
            {
                return NotFound();
            }
            else
            {
                flightTotalSalePrice.FlightNo = flightNo;
                var bookingList = await _context.BookingSet.ToListAsync();
                foreach (Booking b in bookingList)
                {
                    if (b.FlightNo == flightNo)
                    {
                        flightTotalSalePrice.TotalSalePrice += b.SalePrice;
                    }
                }
            }


            return flightTotalSalePrice;
        }

    }
}
