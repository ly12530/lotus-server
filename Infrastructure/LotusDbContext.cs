using Core.Domain;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Infrastructure
{
    public class LotusDbContext : DbContext
    {
        public LotusDbContext(DbContextOptions<LotusDbContext> options) : base(options) { }
        public DbSet<Request> Requests { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer
            modelBuilder.Entity<Customer>().Property(customer => customer.Name).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.EmailAddress).IsRequired();
            modelBuilder.Entity<Customer>().HasIndex(customer => customer.EmailAddress).IsUnique();
            modelBuilder.Entity<Customer>().HasData(
                new Customer {Id = 1, Name = "Pieter", EmailAddress = "pieter@test.test", Password = HashPassword("SuperEnjoying!123", 12)},
                new Customer {Id = 2, Name = "Jorik", EmailAddress = "kek@double.you", Password = HashPassword("NotEnjoying!321", 12)}
            );

            // Request
            modelBuilder.Entity<Request>().Property(request => request.Date).IsRequired();
            modelBuilder.Entity<Request>().Property(request => request.StartTime).IsRequired();
            modelBuilder.Entity<Request>().Property(request => request.EndTime).IsRequired();
            modelBuilder.Entity<Request>().OwnsOne(request => request.Address);
            modelBuilder.Entity<Request>().Property(request => request.Title).IsRequired();
            modelBuilder.Entity<Request>().HasOne(request => request.DesignatedUser).WithMany(user => user.Jobs);
            // TODO Create some seeddata requests
            
            // User
            modelBuilder.Entity<User>().Property(user => user.UserName).IsRequired();
            modelBuilder.Entity<User>().Property(user => user.EmailAddress).IsRequired();
            modelBuilder.Entity<User>().HasIndex(user => user.EmailAddress).IsUnique();
            modelBuilder.Entity<User>().Property(user => user.Role).IsRequired();
            modelBuilder.Entity<User>().Property(user => user.Password).IsRequired();
            modelBuilder.Entity<User>().HasData(
                new User {Id = 1, UserName = "Admin", EmailAddress = "admin@lotus.nl", Role = Role.Administrator, Password = HashPassword("ChangeMe!123", 12)},
                new User {Id = 2, UserName = "Jeff Memberson", EmailAddress = "jeff@memberson.nl", Role = Role.Member, Password = HashPassword("NothingToFearAbout55", 12)},
                new User {Id = 3, UserName = "Pad Jetty", EmailAddress = "pad.jetty@okayvoorzee.nl", Role = Role.PenningMaster, Password = HashPassword("HellYeIkDoeNOOITMe11", 12)},
                new User {Id = 4, UserName = "Sensu Overdijk", EmailAddress = "sensu.overdijk@hotmail.nl", Role = Role.BettingCoordinator, Password = HashPassword("UwUNuthuSumu88", 12)},
                new User {Id = 5, UserName = "Bill Versteen", EmailAddress = "bill.versteen@zig.nl", Role = Role.Instructor, Password = HashPassword("OhNoNotUAgain>:(", 12)}
            );
        }
    }
}