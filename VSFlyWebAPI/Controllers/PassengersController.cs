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
    public class PassengersController : ControllerBase
    {
        private readonly WWWingsContext _context;

        public PassengersController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Passengers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassengerM>>> GetBookingSet()
        {
            var passengerList = await _context.PassengerSet.ToListAsync();
            List<PassengerM> passengerListTmp = new List<PassengerM>();
            foreach (Passenger p in passengerList)
            {
                PassengerM pM = new PassengerM();
                pM.PersonID = p.PersonID;
                pM.GivenName = p.GivenName;
                pM.Surname = p.Surname;
                passengerListTmp.Add(pM);
            }
            return passengerListTmp;
        }

        // GET: api/Passengers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerM>> GetPassenger(int id)
        {
            var passenger = await _context.PassengerSet.FindAsync(id);
            var passengerM = new PassengerM();

            if (passenger == null)
            {
                return NotFound();
            } else
            {
                passengerM.PersonID = passenger.PersonID;
                passengerM.GivenName = passenger.GivenName;
                passengerM.Surname = passenger.Surname;
            }

            return passengerM;
        }

        // Not used
        // PUT: api/Passengers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, PassengerM passenger)
        {
            if (id != passenger.PersonID)
            {
                return BadRequest();
            }

            _context.Entry(passenger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Passengers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PassengerM>> PostPassengerM(PassengerM passengerM)
        {
            var passenger = new Passenger();

            if (passengerM == null)
            {
                return NotFound();
            }
            else
            {
                passenger.GivenName = passengerM.GivenName;
                passenger.Surname = passengerM.Surname;
            }

            _context.PassengerSet.Add(passenger);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(passenger.PersonID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPassenger", new { id = passenger.PersonID }, passenger);
        }

        // Not used
        // DELETE: api/Passengers/5
        /*
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            var passenger = await _context.PassengerSet.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }

            _context.PassengerSet.Remove(passenger);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool BookingExists(int id)
        {
            return _context.BookingSet.Any(e => e.FlightNo == id);
        }

        // ----------------------------

    }
}
