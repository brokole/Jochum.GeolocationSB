using System;
using Jochum.GeolocationSB.Models;
using Microsoft.EntityFrameworkCore;



namespace Jochum.GeolocationSB.Data
{
    public class SqliteContext : DbContext
    {
        public DbSet<Locations> Locations { get; set; }

        public SqliteContext(DbContextOptions<SqliteContext> options)
            : base(options)
        {
            // create table
            String createTable = "CREATE TABLE IF NOT EXISTS 'Locations' ('Id' INTEGER PRIMARY KEY AUTOINCREMENT, 'Straat' TEXT NOT NULL, 'HuisNummer' TEXT NOT NULL,'PostCode' TEXT NOT NULL,'Plaats' TEXT NOT NULL,'Land' TEXT NOT NULL);";
            this.Database.ExecuteSqlRaw(createTable);
            this.SaveChanges();
        }
    }
}
