using System;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly SqliteContext Context;
        private readonly HttpContext CurrentContext;

        public LocationController(SqliteContext context, IHttpContextAccessor httpContextAccessor)
        {
            Context = context;
            CurrentContext = httpContextAccessor.HttpContext;

        }
        // GET: api/Locations
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Locations = await Context.Locations.ToListAsync();
            Locations.Reverse();
            return Ok(Locations);  
        }

        // GET api/Locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var Location = await Context.Locations.FirstOrDefaultAsync(p => p.Id == id);
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
            if (Location == null || Location.Id != 0 || String.IsNullOrEmpty(Location.Straat) || String.IsNullOrEmpty(Location.HuisNummer) || String.IsNullOrEmpty(Location.Plaats) || String.IsNullOrEmpty(Location.Land) || String.IsNullOrEmpty(Location.PostCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            await Context.Locations.AddAsync(Location);
            Context.SaveChanges();

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

            var oldLocation = Context.Locations.SingleOrDefault(p => p.Id == Location.Id);
            if (oldLocation == null)
            {
                return NotFound();
            }
            Context.Entry(oldLocation).CurrentValues.SetValues(Location);
            await Context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Location = Context.Locations.SingleOrDefault(p => p.Id == id);
            if (Location == null)
            {
                return NotFound();
            }

            Context.Locations.Remove(Location);
            await Context.SaveChangesAsync();
            return Ok();
        }
       [HttpGet("GetAll ascending order")]
        public async Task<ActionResult<Locations>> GetAll()
        {
            var Locations = Context.Locations.OrderBy(c => c.Id).ToList();

            return Ok(Locations);
        }
        // GET: api/Locations/descending
        [HttpGet("GetAll descending order")]
        public async Task<ActionResult<Locations>> Getall()
        {
            var Locations = Context.Locations.OrderBy(c => c.Id).ToList();
            Locations.Reverse();
            return Ok(Locations);
        }
        //Search: function
        [HttpPut("search")]
        public async Task<ActionResult<Locations>> Search()
        {
            var Locations = Context.Locations.OrderBy(c => c.Id).ToList();

            return Ok(Locations);
        }
    }
}
