using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class LotusDbContext : DbContext
    {
        public LotusDbContext(DbContextOptions<LotusDbContext> options) : base(options) { }
        public DbSet<Request> Requests { get; set; }
        
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer
            modelBuilder.Entity<Customer>().Property(customer => customer.Name).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.EmailAddress).IsRequired();
            modelBuilder.Entity<Customer>().HasData(
                new Customer {Id = 1, Name = "Pieter", EmailAddress = "pieter@test.test"},
                new Customer {Id = 2, Name = "Jorik", EmailAddress = "kek@double.you"}
            );

            // Request
            modelBuilder.Entity<Request>().Property(request => request.Location).IsRequired();
            modelBuilder.Entity<Request>().Property(request => request.Date).IsRequired();
            modelBuilder.Entity<Request>().Property(request => request.StartTime).IsRequired();
            modelBuilder.Entity<Request>().Property(request => request.EndTime).IsRequired();

            // User
            modelBuilder.Entity<User>().Property(user => user.UserName).IsRequired();
            modelBuilder.Entity<User>().Property(user => user.EmailAddress).IsRequired();
        }
    }
}