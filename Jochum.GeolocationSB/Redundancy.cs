
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


// trying to get the longitude and latitude
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


/*
      //First try at talking to an api

        public abstract class ClientAPI
        {
            protected readonly HttpClient Http;
            private readonly string BaseRoute;

            protected ClientAPI(string baseRoute, HttpClient http)
            {
                BaseRoute = baseRoute;
                Http = http;
            }

            protected async Task<TReturn> GetAsync<TReturn>(string relativeUri)
            {
                HttpResponseMessage res = await Http.GetAsync($"{BaseRoute}/{relativeUri}");
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadFromJsonAsync<TReturn>();
                }
                else
                {
                    string msg = await res.Content.ReadAsStringAsync();
                    Console.WriteLine(msg);
                    throw new Exception(msg);
                }
            }

            protected async Task<TReturn> PostAsync<TReturn, TRequest>(string relativeUri, TRequest request)
            {
                HttpResponseMessage res = await Http.PostAsJsonAsync<TRequest>($"{BaseRoute}/{relativeUri}", request);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadFromJsonAsync<TReturn>();
                }
                else
                {
                    string msg = await res.Content.ReadAsStringAsync();
                    Console.WriteLine(msg);
                    throw new Exception(msg);
                }
            }

            
            public class MySpecificAPI : ClientAPI
            {
                public MySpecificAPI(HttpClient http) : base("api/myspecificapi", http) { }
                public async Task<IEnumerable<Locations>> GetMyClassAsync(int ownerId)
            {
                try
                {
                    return await GetAsync<IEnumerable<Locations>>($"apiMethodName?ownerId={ownerId}");
                }
                catch (Exception e)
                {
                    return null;
                }
        }
        //   public async Task<ActionResult<Locations>> GetJsonHttpClient(string uri, HttpClient httpClient)
      //  {
         //   uri = "https://api.positionstack.com/v1/forward?access_key=a97cb9accc1ba0517bf4b7e8c0a29135&query = 1600 Pennsylvania Ave NW, Washington DC";
            
*/

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

/* DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);

   DataTable dataTable = dataSet.Locations["Table1"];

   Console.WriteLine(dataTable.Rows.Count);
   // 2

   foreach (DataRow row in dataTable.Rows)
   {
       Console.WriteLine(row["id"] + " - " + row["longitude"]);
   } */

/*// Make a request to the API and retrieve the JSON data
var httpClient = new HttpClient();
var response = await httpClient.GetAsync("http://api.positionstack.com/v1/forward?access_key=a97cb9accc1ba0517bf4b7e8c0a29135&query=74, eschersingel, 3544ml, utrecht");
var json = await response.Content.ReadAsStringAsync();

// Deserialize the JSON data into a C# object
var data = System.Text.Json.JsonSerializer.Deserialize<List<Locations>>(json);

// Use the SQLite library to insert the data into a database
using var connection = new SQLiteConnection("Data Source=data.db");
connection.Open();

using var transaction = connection.BeginTransaction();

foreach (var Query_ in data)
{
    using var command = connection.CreateCommand();
    command.CommandText = "INSERT INTO Locations ( latitude, longitude) VALUES ( @latitude, @longitude)";
   
    command.Parameters.AddWithValue("@latitude", Query_.Straat);
    command.Parameters.AddWithValue("@value", Query_.HuisNummer);
    command.ExecuteNonQuery();
}

transaction.Commit();

*/
