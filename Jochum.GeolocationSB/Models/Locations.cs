using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace Jochum.GeolocationSB.Models
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options)
            : base(options)
        {
        }

        public DbSet<Locations> locations { get; set; } = null!;
    }
    public class Locations
    {
        //[DisallowNull]
        //  ? == maybe NULL getting info through BAG.Viewer in Nl makes it possible with juts PostCode and HuisNummer
        public long Id { get; set; }
        public string Straat { get; set; }
        public string HuisNummer { get; set; }
        public string PostCode { get; set; }
        public string Plaats { get; set; }
        public string Land { get; set ; }
    }

    public class QueryObject
    {
        public long Id { get; set; }
        public string Straat { get; set; }
        public string HuisNummer { get; set; }
        public string PostCode { get; set; }
        public string Plaats { get; set; }
        public string Land { get; set; }
    }

    // for parsing the json readout to the database
    public class Query_
    {
        private string Longitude { get; set; }
        private string latitude { get; set; }
     // private string Type { get; set; }
     // private string name { get; set; }
        private string number { get; set; }
        private string Postal_code { get; set; }
        private string street { get; set; }
        private string region { get; set; }
        private string Country { get; set; }
        private string locality { get; set; }
    }



}
