using Microsoft.EntityFrameworkCore;
using PercolateSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PercolateSample.Data
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext() : base() { }

        public SampleDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseInMemoryDatabase(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Person>()
                .HasKey(p => p.Id);
            builder.Entity<Person>()
                .HasData(new Person[] {
                    new Person { Id = 1, Name = "Cory", Age = 52, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 2, Name = "Rosie", Age = 13, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 3, Name = "Josiah", Age = 71, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 4, Name = "Josie", Age = 54, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 5, Name = "Fred", Age = 24, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 6, Name = "Scarlet", Age = 26, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 7, Name = "Keaton", Age = 74, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 8, Name = "Freya", Age = 34, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 9, Name = "Alistair", Age = 84, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 10, Name = "Amanda", Age = 42, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 11, Name = "Luke", Age = 69, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 12, Name = "Ellie", Age = 16, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 13, Name = "Harris", Age = 11, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 14, Name = "Vannesa", Age = 74, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 15, Name = "Jesse", Age = 26, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 16, Name = "Kate", Age = 28, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 17, Name = "Laurence", Age = 83, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 18, Name = "Kayla", Age = 72, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 19, Name = "Liam", Age = 5, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 20, Name = "Nina", Age = 33, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 21, Name = "Gary", Age = 57, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 22, Name = "Nancy", Age = 23, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 23, Name = "Theodore", Age = 12, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 24, Name = "Ashley", Age = 71, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 25, Name = "Hugh", Age = 65, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 26, Name = "Poppy", Age = 45, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 27, Name = "Byron", Age = 19, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 28, Name = "Michelle", Age = 29, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 29, Name = "Felix", Age = 36, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 30, Name = "Ruby", Age = 93, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 31, Name = "Frazer", Age = 31, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 32, Name = "Esme", Age = 50, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 33, Name = "Leo", Age = 40, CreatedDateTime = DateTime.UtcNow },
                    new Person { Id = 34, Name = "Emily", Age = 20, CreatedDateTime = DateTime.UtcNow },
                }
            );
        }
    }
}
