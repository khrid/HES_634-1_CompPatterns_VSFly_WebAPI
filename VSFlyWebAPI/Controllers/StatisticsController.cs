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


        // GET: api/Statistics/FlightTotalSalePrice/5
        [HttpGet("FlightTotalSalePrice/{flightNo}")]
        public async Task<ActionResult<FlightTotalSalePrice>> GetFlightTotalSalePrice(int flightNo)
        {
            var flight = await _context.FlightSet.FindAsync(flightNo);
            var flightTotalSalePrice = new FlightTotalSalePrice();
            flightTotalSalePrice.PassengersCount = 0;

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
                        flightTotalSalePrice.PassengersCount++;
                    }
                }
            }

            return flightTotalSalePrice;
        }


        // GET: api/Statistics/DestinationAvgSalePrice/LDN
        [HttpGet("DestinationAvgSalePrice/{destination}")]
        public async Task<ActionResult<DestinationAvgSalePrice>> GetDestinationAvgSalePrice(string destination)
        {
            List<Flight> listFlight = await GetDestinationFlightsList(destination);

            int passengersCount = 0;
            double totalSalePrice = 0;
            ActionResult<FlightTotalSalePrice> flightTotalSalePriceTmp;

            foreach (Flight f in listFlight)
            {
                flightTotalSalePriceTmp = await GetFlightTotalSalePrice(f.FlightNo);
                totalSalePrice += flightTotalSalePriceTmp.Value.TotalSalePrice;
                passengersCount += flightTotalSalePriceTmp.Value.PassengersCount;
            }

            if (passengersCount > 0)
            {
                DestinationAvgSalePrice destinationAvgSalePrice = new DestinationAvgSalePrice();
                destinationAvgSalePrice.Destination = destination;
                destinationAvgSalePrice.AvgSalePrice = Math.Round(totalSalePrice / (double)passengersCount, 2);
                return destinationAvgSalePrice;
            }
            else
            {
                return NotFound();
            }

        }


        // GET: api/Statistics/DestinationsBookings/LDN
        [HttpGet("DestinationBookings/{destination}")]
        public async Task<ActionResult<DestinationBookings>> GetDestinationBookings(string destination)
        {
            DestinationBookings destinationBookings = new DestinationBookings();
            destinationBookings.Destination = destination;

            List<Flight> listFlight = await GetDestinationFlightsList(destination);

            foreach (Flight f in listFlight)
            {
                var bookingList = await _context.BookingSet.ToListAsync();
                foreach (Booking b in bookingList)
                {
                    if (b.FlightNo == f.FlightNo)
                    {
                        BookingM bookingM = new BookingM();
                        bookingM.FlightNo = b.FlightNo;
                        bookingM.SalePrice = b.SalePrice;
                        bookingM.GivenName = b.Passenger.GivenName;
                        bookingM.Surname = b.Passenger.Surname;
                        destinationBookings.BookingSet.Add(bookingM);
                    }
                }

            }
            return destinationBookings;
        }


        private async Task<List<Flight>> GetDestinationFlightsList(string destination)
        {
            var flightList = await _context.FlightSet.ToListAsync();
            List<Flight> listFlight = new List<Flight>();

            foreach (Flight f in flightList)
            {
                if (f.Destination.ToUpper().Equals(destination.ToUpper()))
                {
                    listFlight.Add(f);
                }
            }

            return listFlight;
        }

    }
}
