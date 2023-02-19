//We start off with our dependencies and our package’s that we are using
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Jochum.GeolocationSB.Data;
using Jochum.GeolocationSB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SQLite.SQLite3;

/*static async Task ProcessRepositoriesAsync(HttpClient client)
{
}*/

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

        // PUT api/Locations ads data to de database ID is an primary key with an auto increment
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Locations Location)
        {
            if (Location == null || Location.Id == 0 || String.IsNullOrEmpty(Location.Straat) || String.IsNullOrEmpty(Location.HuisNummer) || String.IsNullOrEmpty(Location.Plaats) || String.IsNullOrEmpty(Location.Land) || String.IsNullOrEmpty(Location.PostCode))
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

        //Search: function/ascending sorting al fileds staring left to right
        [HttpGet("search ascending order")]
        public async Task<ActionResult<Locations>> Search(String query)
        {
            var Locations = Context.Locations.Where((loc =>
                    loc.HuisNummer.Contains(query) || loc.Straat.Contains(query) ||
                    loc.PostCode.Contains(query) || loc.Plaats.Contains(query) ||
                    loc.Land.Contains(query)
                )).OrderBy(c => c.Id).ThenBy(c => c.HuisNummer).ThenBy(c => c.Straat).ThenBy(c => c.PostCode).ThenBy(c => c.Plaats).ThenBy(c => c.Land).ToList();

            return Ok(Locations);
        }

        // Search: function/descending sorting al fileds staring left to right
        [HttpGet("search descending order sorting everyfield")]
        public async Task<ActionResult<Locations>> search(String query)
        {
            var Locations = Context.Locations.Where((loc =>
         loc.HuisNummer.Contains(query) || loc.Straat.Contains(query) ||
         loc.PostCode.Contains(query) || loc.Plaats.Contains(query) ||
         loc.Land.Contains(query)
     )).OrderBy(c => c.Id).ThenBy(c => c.HuisNummer).ThenBy(c => c.Straat).ThenBy(c => c.PostCode).ThenBy(c => c.Plaats).ThenBy(c => c.Land).ToList();
            Locations.Reverse();
            return Ok(Locations);
        }

        // Search: function/descending id and HuisNummer
        [HttpGet("search descending order sorting via the HuisNummer")]
        public async Task<ActionResult<Locations>> _search(String query)
        {
            var Locations = Context.Locations.Where((loc =>
         loc.HuisNummer.Contains(query) || loc.Straat.Contains(query) ||
         loc.PostCode.Contains(query) || loc.Plaats.Contains(query) ||
         loc.Land.Contains(query)
     )).OrderBy(c => c.HuisNummer).ToList();
            Locations.Reverse();
            return Ok(Locations);
        }

        // Search: function/ascending ID and Huisnummer
        [HttpGet("search ascending order sorting the HuisNummer")]
        public async Task<ActionResult<Locations>> __search(String query)
        {
            var Locations = Context.Locations.Where((loc =>
         loc.HuisNummer.Contains(query) || loc.Straat.Contains(query) ||
         loc.PostCode.Contains(query) || loc.Plaats.Contains(query) ||
         loc.Land.Contains(query)
     )).OrderBy(c => c.HuisNummer).ToList();
            return Ok(Locations);
        }

        // Search: function/descending Id
        [HttpGet("search descending order sorting ID")]
        public async Task<ActionResult<Locations>> Idsearch(String query)
        {
            var Locations = Context.Locations.Where((loc =>
         loc.HuisNummer.Contains(query) || loc.Straat.Contains(query) ||
         loc.PostCode.Contains(query) || loc.Plaats.Contains(query) ||
         loc.Land.Contains(query)
     )).OrderBy(c => c.Id).ToList();
            Locations.Reverse();
            return Ok(Locations);
        }

        // Search: function/ascending id
        [HttpGet("search ascending order sorting ID")]
        public async Task<ActionResult<Locations>> _Idsearch(String query)
        {
            var Locations = Context.Locations.Where((loc =>
         loc.HuisNummer.Contains(query) || loc.Straat.Contains(query) ||
         loc.PostCode.Contains(query) || loc.Plaats.Contains(query) ||
         loc.Land.Contains(query)
     )).OrderBy(c => c.Id).ToList();
            Locations.Reverse();
            return Ok(Locations);
        }
        [HttpGet("get street")]
        private static async Task<ActionResult<Locations>> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            uri = "https://api.positionstack.com/v1/forward?access_key=a97cb9accc1ba0517bf4b7e8c0a29135&query = 1600 Pennsylvania Ave NW, Washington DC";
            try
            {
                return await httpClient.GetFromJsonAsync<ActionResult<Locations>>(uri);
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }
    }

    //results > latitude	Returns the latitude coordinate associated with the location result.
    //results > longitude Returns the longitude coordinate associated with the location result.

        /*      [HttpGet("get longitude and latitude")]
               public async Task<IActionResult> longitude()
               {
                 Uri HttpClient.BaseAddress = new Uri("https://api.positionstack.com/v1/forward")
                        ? access_key = a97cb9accc1ba0517bf4b7e8c0a29135
                        & AsyncCallback = Query_;

                   var Query_ = await Context.Query_.ToListAsync();
                   Query_.Reverse();
                   return Ok(Query_);
               }
               static async Task ProcessRepositoriesAsync(HttpClient client)
               {
                   var json = await client.GetStringAsync(
                       "https://api.positionstack.com/v1/forward");

                   Console.Write(json);
               }   */
}
        
    


//,  loc.Straat.Contains(query),
//    loc.PostCode.Contains(query),  loc.Plaats.Contains(query),
//                 loc.Land.Contains(query)

