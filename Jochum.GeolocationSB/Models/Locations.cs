using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
}
