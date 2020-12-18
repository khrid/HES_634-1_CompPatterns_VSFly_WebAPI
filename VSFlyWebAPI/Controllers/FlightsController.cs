using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreApp2020E;

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
        public async Task<ActionResult<IEnumerable<Models.FlightM>>> GetFlightSet()
        {
            var flightList =  await _context.FlightSet.ToListAsync();
            List<Models.FlightM> listFlightM = new List<Models.FlightM>();
            foreach( Flight f in flightList)
            {
                Models.FlightM fM = new Models.FlightM();
                fM.Date = f.Date;
                fM.Departure = f.Departure;
                fM.Destination = f.Destination;
                listFlightM.Add(fM);
            }
            return listFlightM;
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight;
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
            f.Seats = 300;
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
    }
}
