using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSFlyEFCoreApp;
using VSFlyWebAPI.Models;

namespace VSFlyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly WWWingsContext _context;

        public FlightsController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightM>>> GetFlightSet()
        {
            var flightList = await _context.FlightSet.ToListAsync();
            List<FlightM> listFlightM = new List<FlightM>();
            foreach (Flight f in flightList)
            {
                    FlightM fM = new FlightM();
                    fM.FlightNo = f.FlightNo;
                    fM.Date = f.Date;
                    fM.Departure = f.Departure;
                    fM.Destination = f.Destination;
                    listFlightM.Add(fM);
            }
            return listFlightM;
        }


        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightM>> GetFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);
            var flightM = new FlightM();

            if (flight == null)
            {
                return NotFound();
            } else
            {
                flightM.FlightNo = flight.FlightNo;
                flightM.Date = flight.Date;
                flightM.Departure = flight.Departure;
                flightM.Destination = flight.Destination;
                flightM.BasePrice = flight.BasePrice;
               
            }


            return flightM;
        }

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, Flight flight)
        {
            if (id != flight.FlightNo)
            {
                return BadRequest();
            }

            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Models.FlightM flight)
        {
            Flight f = new Flight();
            f.Pilot = _context.PilotSet.Find(1);
            f.Seats = flight.Seats;
            f.BasePrice = flight.BasePrice;
            f.Date = flight.Date;
            f.Departure = flight.Departure;
            f.Destination = flight.Destination;
            _context.FlightSet.Add(f);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string t = e.Message;
            }

            return CreatedAtAction("GetFlight", new { id = flight.FlightNo }, flight);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.FlightSet.Remove(flight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightExists(int id)
        {
            return _context.FlightSet.Any(e => e.FlightNo == id);
        }

        // -----------------------------------------

        // GET: api/FutureFlights
        [HttpGet]
        [Route("FutureFlights")]
        public async Task<ActionResult<IEnumerable<FlightM>>> GetFutureFlightsSet()
        {
            var flightList = await _context.FlightSet.ToListAsync();
            List<FlightM> listFlightM = new List<FlightM>();
            foreach (Flight f in flightList)
            {
                // do not return past flights
                if (f.Date >= DateTime.Now && !IsFlightFull(f))
                {
                    FlightM fM = new FlightM();
                    fM.FlightNo = f.FlightNo;
                    fM.Date = f.Date;
                    fM.Departure = f.Departure;
                    fM.Destination = f.Destination;
                    listFlightM.Add(fM);
                }
            }
            return listFlightM;
        }

        [HttpGet]
        [Route("GetSalePriceForFlight/{id}")]
        public async Task<ActionResult<double>> GetSalePriceForFlight(int id)
        {
            var salePrice = 0.0;
            var flight = await _context.FlightSet.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }
            else
            {
                // price calculation algorithm
                salePrice = ComputeSalePrice(GetFlightBookedSeats(flight), flight.Seats, flight.Date, flight.BasePrice);
            }

            return salePrice;
        }

        private int GetFlightBookedSeats(Flight flight)
        {
            int BookedSeats = 0;
            foreach (var b in _context.BookingSet)
            {
                if (b.FlightNo == flight.FlightNo) BookedSeats++;
            }
            return BookedSeats;
        }

        private Boolean IsFlightFull(Flight flight)
        {
            return (flight.Seats == GetFlightBookedSeats(flight));
        }


        private double ComputeSalePrice(int BookedSeats, short? FlightCapacity, DateTime FlightDeparture, double BasePrice) 
        {
            double capacity = (double)(BookedSeats / FlightCapacity * 100);
            //double SalePrice = BasePrice;
            double Modifier = 1;
            /*   1.	If the airplane is more than 80% full regardless of the date:
                    a. sale price = 150% of the base price
                 2.	If the plane is filled less than 20% less than 2 months before departure:
                    a. sale price = 80% of the base price
                 3.	If the plane is filled less than 50% less than 1 month before departure:
                    a. sale price = 70% of the base price
                 4.	4. In all other cases:
                    a. sale price = base price
            */
            if(capacity > 80) 
            {
                Modifier =  1.5;
            } 
            else
            {
                int delta = (DateTime.Now - FlightDeparture).Days;
                if (delta < 60)
                {
                    if (delta < 30)
                    {
                        if(capacity < 50)
                        {
                            Modifier = 0.7;
                        } 
                        else if (capacity < 20)
                        {
                            Modifier = 0.8;
                        }
                    }
                }
            }

            return Math.Round(BasePrice * Modifier, 2);
        }
    }
}
