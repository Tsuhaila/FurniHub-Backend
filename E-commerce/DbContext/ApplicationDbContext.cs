using FurniHub.Models.UserModels;
using FurniHub.Models.Categories;
using Microsoft.EntityFrameworkCore;
using FurniHub.Models.ProductModels;
using FurniHub.Models.CartModels;
using FurniHub.Models.OrderModels;
using FurniHub.Models.WishlistModels;

namespace FurniHub
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }

        

        private readonly IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasDefaultValue("user");

            modelBuilder.Entity<Product>()
                .Property(p => p.Quantity)
                .HasDefaultValue(1);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Product>()
                .Property(p=>p.Rating)
                .HasDefaultValue(0);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(20, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.OfferPrice)
                .HasPrecision(20, 2);

            modelBuilder.Entity<User>()
                .HasOne(u=>u.Cart)
                .WithOne(c=>c.User)
                .HasForeignKey<Cart>(c => c.UserId);

           modelBuilder.Entity<Cart>()
                .HasMany(c=>c.CartItems)
                .WithOne(c=>c.Cart)
                .HasForeignKey(c=>c.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(c=>c.Product)
                .WithMany(p=>p.CartItems)
                .HasForeignKey(c=>c.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.userId);

            modelBuilder.Entity<Order>()
                .HasMany(o=>o.OrderItems)
                .WithOne(oi=>oi.Order)
                .HasForeignKey(oi=>oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi=>oi.Product)
                .WithMany()
                .HasForeignKey(oi=>oi.ProductId);

            modelBuilder.Entity<OrderItem>()
                .Property(o=>o.TotalPrice)
                .HasPrecision(20,2);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w=>w.Users)
                .WithMany(u=>u.Wishlists)
                .HasForeignKey(w=>w.UserId);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Products)
                .WithMany()
                .HasForeignKey(w => w.ProductId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
