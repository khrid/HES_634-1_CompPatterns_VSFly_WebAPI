using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSFlyEFCoreApp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VSFlyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PilotsController : ControllerBase
    {

        private readonly WWWingsContext _context;

        public PilotsController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Pilots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pilot>>> GetPilotSet()
        {
            var pilotList = await _context.PilotSet.ToListAsync();
            List<Pilot> pilotListTmp = new List<Pilot>();
            foreach (Pilot b in pilotList)
            {
                Pilot bM = new Pilot();
                bM.Surname = b.Surname;
                bM.GivenName = b.GivenName;
                bM.Salary = b.Salary;
                pilotListTmp.Add(bM);
            }
            return pilotListTmp;
        }
    }
}
