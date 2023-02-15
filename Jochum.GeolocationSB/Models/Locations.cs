using Microsoft.EntityFrameworkCore;
using SQLite;

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

        //  ? == maybe NULL getting info through BAG.Viewer in Nl is possible with juts PostCode and HuisNummer
        public long Id { get; set; }
        public string? Straat { get; set; }
        public string HuisNummer { get; set; }
        public string PostCode { get; set; }
        public string? Plaats { get; set; }
        public string Land { get; set; }

    }
}
