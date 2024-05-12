using Microsoft.EntityFrameworkCore;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string Default_Scheme = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }

        public DbSet<Domain.OrderAggragate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggragate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggragate.Order>().ToTable("Orders", Default_Scheme);
            modelBuilder.Entity<Domain.OrderAggragate.OrderItem>().ToTable("OrderItems", Default_Scheme);

            modelBuilder.Entity<Domain.OrderAggragate.OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Domain.OrderAggragate.Order>().OwnsOne(o => o.Address).WithOwner();
        }
    }
}
