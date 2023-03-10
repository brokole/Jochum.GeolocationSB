# Jochum.GeolocationSB

SB Project omtrent geolocatie
Dependecies of NuGet: 
 ```
    Microsoft.EntityFrameworkCore
    Microsoft.EntityFrameworkCore.InMemory
    Microsoft.EntityFrameworkCore.Sqlite
    Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
    sqlite-net-pcl
    Swashbuckle.AspNetCore
    dapper
    System.Data.SQLite.Core
    Newtonsoft.Json
 ```  


I chose .net 6 as my main framework, because it's the long term support version. However, it works almost completely differently than .Net 4, in which I learned to code the little .Net I knew (which was limited to the context of a Unity engine). Furthermore, I had never written an API before, that just wasn't part of the game development curriculum. So it felt like starting from scratch... a nice challenge but also quite an arduous task. But I’m somewhat proud of how far I have come, I learned a lot and it was a lot of fun, and mostly I enjoyed myself. So here we go, what did I make.

Let’s start with the Controller. We start off with our dependencies and our packages. Next we give it a route so we can access the Controller through a URL and read out the data. [Route("api/[controller]")] 

Following the route we create our Controller Class with which we shall give functions for the API to use. We are using a SQLite database which we create in our SqliteContext.cs file in the Data folder. This database we need to be able to call, for any context we might need for editing and storing data in our database. So, we make a context variable that gives us our context(information). Of course this is the context in our database but we might want to enter data into our database so we need to be able to get our http request to the database. For this purpose, we create a HttpContext to read out the data and send it to our database.

Next we need to be able to Get the data stored in our database. We call on this data through:
```
[HttpGet] 
public async Task<IActionResult> Get()
{
             var Locations = await Context.Locations.ToListAsync();
             Locations.Reverse();
             return Ok(Locations);
 }

In which we ask for the data stored in our database and to list the data out to make it readable. 

Next we do the same thing but we want to get only 1 result and get through an Id.
Of course if there is no Id it should tell us so:

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
```
Next, we can Get everything or get specific data by using an Id. But what if I want to add something to our database. This can be done through the [HttpPost]  function. Of course we don’t want any empty fields in our database so when filling in the data make sure to fill in every bit, this should be validated at the frontend. If done incorrectly and a field is left NULL you will get a Status400BadRequest. The code to add the data to the database is as follows:
```
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
 ```
(Which could probably have been done nicer, but this was the easiest way that I knew how to do it). Next up maybe when we entered data we made a mistake, so we need to have an edit function. For this edit function we use [HttpPut("{id}")] . That will allow us to get data from the database through an Id and then overwrite the existing data in the database. Again we can't have an Empty field so if left empty you will get a Status400BadRequest .If per chance someone else deleted the Id you where editing we would not be able to sent the data and we need to tell the user that there data isn’t there anymore. So if the data has gone missing in the time that you have been editing it will send you a notFound() result. The code is as follows:
```      
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
 ```

Next, a user might no longer want their address in the database so we should be able to delete data (normally you would Hash the data, add a time stamp of the moment of request and put that info in the place of the actual data. Long live the AVG) In this case we are going to simply delete the id and all the data connected to the Id. Of course we again cannot delete something that doesn’t exist so we check again does the Id exist, if yes then lets delete it, otherwise return NotFound() . The code Is as follows:

 ```
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
  ```

Next we might have users that are living in the same street, so we might need to sort all our data for a street or look at everyone that lives on Number 12 in every street. For this we need a search function and a sort function. I wasn’t able to create a Sorting function that would sort every field one by one how I wanted it, but I did my best. This took the most amount of time and I am the most proud of it that why there are 6 of them. Each one sorts it in a different way, some ascending or descending, some by id some by HuisNummer . I had a blast figuring out how to do it in the most compact way I could think of. I will only show one example:

 ```
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
 ```
This is how the API functions and makes you able to call and get differing information by calling separate functions connected to the API.

Now for the less exciting stuff. Locations.cs is the Model that hold the variables for the controller and the Query. It also sets the context for the Database.

Program.cs is the startup file that starts the services that we need.

SqliteContext.cs Creates the database if there isn’t one already.

Redundancy.cs is full of old code that I changed made better or simply didn’t need anymore.

While working on the Project I had a lot of fun but in the end I couldnt finish all of it. I got the necessary info from the API. using the following code:

```
//getting the info from the External api
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

await ProcessRepositoriesAsync(client);

static async Task ProcessRepositoriesAsync(HttpClient client)
{
    var json = await client.GetStringAsync(
    "http://api.positionstack.com/v1/forward?access_key=a97cb9accc1ba0517bf4b7e8c0a29135&query=74, eschersingel, 3544ml, utrecht");

    Console.Write(json);
}
```

![Screenshot_1](https://user-images.githubusercontent.com/22211391/219872155-bc85b120-81e2-462f-84b6-77e56393fd9b.png)

After that i would need to put 2 locations into a database and then send the info from that database via Id Query to positionstack to get the Longitude and latitude of both locations, and send that information to openstreetmap to get the distance between both points (assuming the required distance would be traveling by road and not 'as the crow flies', in which case a calculation based on Pythagoras would be sufficient), and send that information together with the Location details from the database to the user.

I got as far as getting all the info from http://api.positionstack.com/v1/forward But then I got stuck. I googled how I could send the data as a query to OpenStreetMaps and found some answers, but I would have to first get 2 separate Gets and get 4 sets of coordinates to then ask the distance so it would be a timer and all be async. And I found my limit: this is where my knowledge of .net stops (in part because in the mean time I had fallen ill so my concentration was sub-zero).

I'm spending the rest of my 2 days reading up on .net and if I learn something that's applicable for this case I will add it and maybe finish the rest of it. But for now, I'm proud how far I have come and the code I have written, and hope to learn a lot more in the future.






