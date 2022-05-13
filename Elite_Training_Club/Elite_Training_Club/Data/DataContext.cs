using Elite_Training_Club.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elite_Training_Club.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Headquarter> Headquarters { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<TemporalSale> TemporalSales { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<SubscriptionsPlan> SubscriptionsPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique();
            modelBuilder.Entity<Headquarter>().HasIndex("Name", "CityId").IsUnique();
            modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique();
            modelBuilder.Entity<Plan>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<ProductCategory>().HasIndex("ProductId", "CategoryId").IsUnique();
            modelBuilder.Entity<Subscriptions>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<SubscriptionsPlan>().HasIndex("SubscriptionsId", "PlanId").IsUnique();

        }
    }
}
