using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class eCommerceContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"server=MELIK\SQLEXPRESS;database=eCommerceEleven;trusted_connection=true;multipleactiveresultsets=true;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                .HasOne(x => x.User)
                .WithOne(x => x.UserDetail)
                .HasForeignKey<UserDetail>(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
               .HasOne(userDetail => userDetail.Country)
               .WithMany(country => country.UserDetails)
               .HasForeignKey(userDetail => userDetail.CountryId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                .HasOne(userDetail => userDetail.City)
                .WithMany(city => city.UserDetails)
                .HasForeignKey(userDetail => userDetail.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<City>()
                .HasOne(city => city.Country)
                .WithMany(country => country.Cities)
                .HasForeignKey(city => city.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Country>().ToTable("eCommerceCountries");

            modelBuilder.Entity<Role>().ToTable("eCommerceRoles");

            modelBuilder.Entity<Product>().HasIndex(product => product.Name);

            modelBuilder.Entity<User>().HasIndex(user => user.Username).IsUnique();

            modelBuilder.Entity<UserDetail>().HasIndex(userDetail => userDetail.Email).IsUnique();
        }
    }
}