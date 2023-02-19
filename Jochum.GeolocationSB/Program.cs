using Microsoft.EntityFrameworkCore;
using Jochum.GeolocationSB.Models;
using Jochum.GeolocationSB;
using Jochum.GeolocationSB.Data;

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
