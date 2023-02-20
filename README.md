# Jochum.GeolocationSB

SB Project omtrent geolocatie
Dependecies of NuGet: 
 ```
    Microsoft.EntityFrameworkCore
    Microsoft.EntityFrameworkCore.InMemory
    Microsoft.EntityFrameworkCore.Sqlite
    sqlite-net-pcl
    Swashbuckle.AspNetCore
 ```  
Dont forget them

So for starters I chose .net 6 as my main framework.
Why .net 6 well it’s the long term support version of .net and works almost completely differently than .net 4, in which I learned to code the little .net I knew
So it felt like starting from scratch working 8 hours a day and in the evening learning .net 6 and working till I have to go to bed.
It has been quite the mountainous task.
But I’m proud of how far I have come I learned a lot and it was a lot of fun and I enjoyed myself.
So here we go what did I make.

Let’s start with the Controller.
We start off with our dependencies and our package’s.
Next we give it a route so we can access the Controller through a URL and read out the data.
 ```[Route("api/[controller]")] ```

Following the route we create our Controller Class with which we shall give functions for the API to use.
We are using a SqLite database which we create in our SqliteContext.cs file in the Data folder. This database we need to be able to call, for any context we might need for editing and storing data in our database. So, we make a context variable that gives us our context(information). Of course this is the context in our database but we might want to enter data into our database so we need to be able to get our http request to the database. For this purpose, we create a HttpContext to read out the data and send it to our database.

Next we need to be able to Get the data stored in our database.
We call on this data through:
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
Great now we can Get everything or get specific data by using an Id.
But what if I want to add something to our database. This can be done through the  ```[HttpPost] ``` function. Of course we don’t want any empty fields in our database so when filling in the data make sure to fill in every bit. If done incorrectly and a field is left NULL you will get a Status400BadRequest.
The code to added the data in the database is as follows:
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
(this could have been done a lot nicer but this was the easiest way that I knew how)
Next up maybe when we entered data we made a spelling mistake well then we need to have an edit function. For this edit function we use  ```[HttpPut("{id}")] ```. Which will allow us to get data from the database through an Id and then overwrite the existing data in the database. Again we cant have a Empty field so if left empty you will get a  ```Status400BadRequest ```.If per chance someone else deleted the Id you where editing we would not be able to sent the data and we need to tell the user that there data isn’t there anymore. So if the data has gone missing in the time that you have been editing it will send you a notFound() result. The code is as follows:
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
Next a user might no longer want there address in the database so we should be able to delete there data (normally you would Hash the data add a time stamp of the moment of request and put that info in the place of the actual data. Long live the AVG) In this case we are going to simply delete the id and all the data connected to the Id. Of course we again cannot delete something that doesn’t exist so we check again does the Id exist yes alright lets delete it otherwise return  ```NotFound() ```. The code Is as follows:
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
Next we might have users that are living in the same street
So we might need to sort all our data for a street or look at everyone that lives on Number 12 in every street for this we need a search function and a sort function. I wasn’t able to create a Sorting function that would sort every field one by one how I wanted it but I did my best. This took the most amount of time and I am the most proud of it that why there are 6 of them. Each one sorts it in a different way some ascending or descending some by id some by  ```HuisNummer ```.
I had a blast figuring out how to do it in the most compact way I could think of. I will only show one example:
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

Now for the less exciting stuff.
Locations.cs is the Model that hold the variables for the controller and the Query. It also sets the context for the Database.

Program.cs is the startup file that starts the services that we need.

SqliteContext.cs Creates the database if there isn’t one already

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

After that i would need to put 2 locations into a database and then sent de info from that databse via Id Query to positionstack get the Logitude and latitude of both locations and sent that information to openstreetmap to get the distance between both points, and send that information togheter with the Location details from the database to the user.

I got as far as getting all the info from http://api.positionstack.com/v1/forward 
But then I got stuck. I asked friends around me how I could send the data as a query to OpenStreetMaps But I would have to first get 2 separate Gets and get 4 sets of coordinates to then ask the distance so it would be a timer and all be async. And I found my Limit this is where my Knowledge of .net stops.

Iam spending the rest of my 2 days reading up on .net and if I Learn something useful for this case I will add it and maybe finish the rest of it. But for Now Iam proud How far I have come and Proud of the code I have written and hope to learn a lot more in the future.







