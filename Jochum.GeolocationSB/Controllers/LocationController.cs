using System;
using System.Linq;
using System.Threading.Tasks;
using Jochum.GeolocationSB.Data;
using Jochum.GeolocationSB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jochum.GeoLocationsB.Controllers
{
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly SqliteContext _context;
        private readonly HttpContext _currentContext;

        public LocationController(SqliteContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _currentContext = httpContextAccessor.HttpContext;

            if (_context.Locations.Count() == 0)
            {
                _context.Locations.Add(new Locations { Id = 1, Straat = "", HuisNummer = "", PostCode = "", Plaats = "", Land = "" });
                _context.Locations.Add(new Locations { Id = 2, Straat = "", HuisNummer = "", PostCode = "", Plaats = "", Land = "" });
                _context.Locations.Add(new Locations { Id = 3, Straat = "", HuisNummer = "", PostCode = "", Plaats = "", Land = "" });
                _context.SaveChanges();
            }
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Locations = await _context.Locations.ToListAsync();
            Locations.Reverse();
            return Ok(Locations);
        }

        // GET api/Locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var Location = await _context.Locations.FirstOrDefaultAsync(p => p.Id == id);
            if (Location == null)
            {
                return NotFound();
            }
            return Ok(Location);
        }

        // POST api/Locations
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Locations Location)
        {
            if (Location == null || Location.Id != 0 || String.IsNullOrEmpty(Location.Straat))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            await _context.Locations.AddAsync(Location);
            _context.SaveChanges();

            return Ok();
        }

        // PUT api/Locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Locations Location)
        {
            if (Location == null || Location.Id == 0 || String.IsNullOrEmpty(Location.Straat))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var oldLocation = _context.Locations.SingleOrDefault(p => p.Id == Location.Id);
            if (oldLocation == null)
            {
                return NotFound();
            }
            _context.Entry(oldLocation).CurrentValues.SetValues(Location);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Location = _context.Locations.SingleOrDefault(p => p.Id == id);
            if (Location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(Location);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}