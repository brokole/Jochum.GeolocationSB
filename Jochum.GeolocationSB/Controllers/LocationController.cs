using System;
using System.Linq;
using System.Threading.Tasks;
using Jochum.GeolocationSB.Data;
using Jochum.GeolocationSB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jochum.GeoProductsB.Controllers
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

            if (_context.Products.Count() == 0)
            {
                _context.Products.Add(new Locations { Id = 1, Straat = "", HuisNummer = "", PostCode = "", Plaats = "", Land = "" });
                _context.Products.Add(new Locations { Id = 2, Straat = "", HuisNummer = "", PostCode = "", Plaats = "", Land = "" });
                _context.Products.Add(new Locations { Id = 3, Straat = "", HuisNummer = "", PostCode = "", Plaats = "", Land = "" });
                _context.SaveChanges();
            }
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Products = await _context.Products.ToListAsync();
            Products.Reverse();
            return Ok(Products);
        }

        // GET api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Locations product)
        {
            if (product == null || product.Id != 0 || String.IsNullOrEmpty(product.Straat))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            await _context.Products.AddAsync(product);
            _context.SaveChanges();

            return Ok();
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Locations product)
        {
            if (product == null || product.Id == 0 || String.IsNullOrEmpty(product.Straat))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var oldProduct = _context.Products.SingleOrDefault(p => p.Id == product.Id);
            if (oldProduct == null)
            {
                return NotFound();
            }
            _context.Entry(oldProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}