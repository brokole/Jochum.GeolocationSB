using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Jochum.GeolocationSB.Models;
using Jochum.GeolocationSB;
using Jochum.GeolocationSB.Data;
using System.Net.Http.Headers;
using System.Data.SQLite;
using Dapper;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<LocationContext>(opt =>
    opt.UseInMemoryDatabase("SQLiteLocation"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SqliteContext>(opt => opt.UseSqlite("Data Source=SQLiteLocation.db"));
builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
var app = builder.Build();

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

   /* DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);

    DataTable dataTable = dataSet.Locations["Table1"];

    Console.WriteLine(dataTable.Rows.Count);
    // 2

    foreach (DataRow row in dataTable.Rows)
    {
        Console.WriteLine(row["id"] + " - " + row["longitude"]);
    } */
}
// Make a request to the API and retrieve the JSON data
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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
