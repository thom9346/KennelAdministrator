using KennelIndexer.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace KennelIndexer.API.DbContexts
{
    public class PersonLibraryContext : DbContext
    {
        public PersonLibraryContext(DbContextOptions<PersonLibraryContext> options)
            :base(options)
        {

        }
        public DbSet<Person> People { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person()
                {
                    PersonId = Guid.Parse("05C6C2AB-C63B-4C78-818F-39565D57BCF5"),
                    FirstName = "Bob",
                    LastName = "The builder"
                },
                new Person()
                {
                    PersonId = Guid.Parse("05C6C2AB-C63B-4C78-818F-39565D57ACF5"),
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "Lygten 31",
                    ReasonsForBeingOnTheList = "Confirmed cases of animal abuse"

                }
                );

            modelBuilder.Entity<Picture>().HasData(
                new Picture
                {
                    PictureId = Guid.Parse("C54F132B-79AA-4986-AAB4-B0DA907CE843"),
                    PictureUri = "Resources/Images/256cbcfc-c699-48fa-9a87-edf1b8b71f6f.jpg",
                    PersonId = Guid.Parse("05C6C2AB-C63B-4C78-818F-39565D57BCF5")
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
