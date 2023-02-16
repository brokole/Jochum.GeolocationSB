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
  /*      public List<Locations> GetLocation(QueryObject query)
        {
            var _locations = Context.Locations.AsQueryable();

            if (!string.IsNullOrEmpty(query.Straat))
            {
                _locations = _locations.Where(e => e.Straat.Contains(query.Straat));
            }

            var ColumnsMap = new Dictionary<string, Expression<Func<Straat, object>>>
            {
                ["Locations"] = c => c.Straat,
                ["abbr"] = c => c.
                       };
            Locations = locations.ApplyOrdering(query, ColumnsMap);

            //Do paging as you have done earlier

            return employees.ToList();
        }
  
        //This function orders based on the key and Expression Function You pass

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, QueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (string.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
                return query;

            return queryObj.IsSortAscending ? query.OrderBy(columnsMap[queryObj.SortBy]) : query.OrderByDescending(columnsMap[queryObj.SortBy]);
        }
  */
    }
}