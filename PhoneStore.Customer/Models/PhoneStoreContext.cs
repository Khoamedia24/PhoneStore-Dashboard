using Microsoft.EntityFrameworkCore;

namespace PhoneStore.Customer.Models
{
    public class PhoneStoreContext : DbContext
    {
        public PhoneStoreContext(DbContextOptions<PhoneStoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<DiscountProgram> DiscountPrograms { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId);

                entity.HasOne(p => p.Discount)
                    .WithMany(d => d.Products)
                    .HasForeignKey(p => p.DiscountId);
            });

            // ProductImage configuration
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);
                entity.Property(e => e.ImageData).IsRequired();
                entity.Property(e => e.ImageMimeType).IsRequired().HasMaxLength(100);

                entity.HasOne(pi => pi.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(pi => pi.ProductId);

                entity.HasOne(pi => pi.Color)
                    .WithMany(c => c.ProductImages)
                    .HasForeignKey(pi => pi.ColorId);            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            // Color configuration
            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId);
                entity.Property(e => e.ColorName).HasMaxLength(100);
            });

            // DiscountProgram configuration
            modelBuilder.Entity<DiscountProgram>(entity =>
            {
                entity.HasKey(e => e.DiscountId);
                entity.Property(e => e.DiscountName).HasMaxLength(100);
            });

            // Customer configuration
            modelBuilder.Entity<CustomerEntity>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(15);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId);

                entity.HasOne(o => o.ShippingAddress)
                    .WithMany(sa => sa.Orders)
                    .HasForeignKey(o => o.ShippingAddressId);
            });

            // OrderDetail configuration
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);
                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailId");
                entity.Property(e => e.OrderId).HasColumnName("OrderId");
                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.ColorId).HasColumnName("ColorId");
                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                // Explicitly configure the price columns
                entity.Property(e => e.UnitPrice)
                    .HasColumnName("UnitPrice")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("TotalPrice")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(od => od.OrderId);

                entity.HasOne(od => od.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(od => od.ProductId);

                entity.HasOne(od => od.Color)
                    .WithMany(c => c.OrderDetails)
                    .HasForeignKey(od => od.ColorId);
            });

            // ShippingAddress configuration
            modelBuilder.Entity<ShippingAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId);
                entity.Property(e => e.RecipientName).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.AddressLine).HasMaxLength(500);
                entity.Property(e => e.Ward).HasMaxLength(100);
                entity.Property(e => e.District).HasMaxLength(100);
                entity.Property(e => e.Province).HasMaxLength(100);

                entity.HasOne(sa => sa.Customer)
                    .WithMany()
                    .HasForeignKey(sa => sa.CustomerId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
