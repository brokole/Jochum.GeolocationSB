
// this is all redundancy

//Stuff from Location Controller

            // test with out db to make sure values aren't null
            // also it breaks everything if you enable it because the code will try to edit Id 1 2 and 3 to the current values and then change the values to the values bellow

            /*if (Context.Locations.Count() == 0)
            {
                Context.Locations.Add(new Locations { Id = 1, Straat = "1", HuisNummer = "2", PostCode = "3", Plaats = "4", Land = "5" });
                Context.Locations.Add(new Locations { Id = 2, Straat = "1", HuisNummer = "2", PostCode = "3", Plaats = "4", Land = "5" });
                Context.Locations.Add(new Locations { Id = 3, Straat = "1", HuisNummer = "2", PostCode = "3", Plaats = "4", Land = "5" });
                Context.SaveChanges();
            }  */
        
/*
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["StraatSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Straat_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var locations = from s in Context.Locations
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    locations = locations.OrderByDescending(s => s.Straat);
                    break;
                case "Date":
                    locations = locations.OrderBy(s => s.HuisNummer);
                    break;
                case "date_desc":
                    locations = locations.OrderByDescending(s => s.HuisNummer);
                    break;
                default:
                    locations = locations.OrderBy(s => s.PostCode);
                    break;
            }
            return View(await locations.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Index(string searchString)
{
    if (Context.Locations == null)
    {
        return Problem("Entity set 'LocationContext.Locations'  is null.");
    }

    var Location = from m in Context.Locations
                 select m;

    if (!String.IsNullOrEmpty(searchString))
    {
        Location = Location.Where(s => s.Straat!.Contains(searchString));
    }

    return View(await Location.ToListAsync());
}
*/

// redundant Startup.cs


/* using Jochum.GeolocationSB.Data;
using Microsoft.EntityFrameworkCore;

namespace Jochum.GeolocationSB
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<SqliteContext>(opt => opt.UseSqlite("Data Source=SQLiteLocation.db"));
            services.AddMvc();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();
        }
    }
}
*/

