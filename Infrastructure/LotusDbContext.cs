using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class LotusDbContext : DbContext
    {
        public LotusDbContext(DbContextOptions<LotusDbContext> options) : base(options) { }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestDate> RequestDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Request
            modelBuilder.Entity<Request>().HasOne(request => request.RequestDate)
                .WithOne(requestDate => requestDate.Request);
        }
    }
}