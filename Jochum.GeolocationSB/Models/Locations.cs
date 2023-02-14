using SQLite;

namespace Jochum.GeolocationSB.Models
{
    public class Locations
    {
        public long? Id { get; set; }
        public string? Straat { get; set; }
        public string? HuisNummer { get; set; }
        public string? PostCode { get; set; }
        public string? Plaats { get; set; }
        public string? Land { get; set; }

    }
}
