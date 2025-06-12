using Microsoft.EntityFrameworkCore;

namespace RestaurantDomain.Model;

public partial class RestaurantContext : DbContext
{
    public RestaurantContext() { }

    public RestaurantContext(DbContextOptions<RestaurantContext> options)
        : base(options) { }

    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Dish> Dishes { get; set; } = null!;
    public virtual DbSet<DishOrder> DishOrders { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<Table> Tables { get; set; } = null!;
    public virtual DbSet<Worker> Workers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning Move connection string out of source code
        => optionsBuilder.UseSqlServer(
            "Server=Victus\\SQLEXPRESS;Database=restaurant;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* ---------- Client ---------- */
        modelBuilder.Entity<Client>(e =>
        {
            e.ToTable("Client");
            e.Property(p => p.Id).ValueGeneratedNever();
            e.Property(p => p.FirstName).HasMaxLength(200).IsUnicode(false);
            e.Property(p => p.LastName).HasMaxLength(200).IsUnicode(false);
        });

        /* ---------- Dish ---------- */
        modelBuilder.Entity<Dish>(e =>
        {
            e.ToTable("DIsh");
            e.Property(p => p.Id).ValueGeneratedNever().HasColumnName("id");
            e.Property(p => p.Name).HasMaxLength(200).IsUnicode(false);
            e.Property(p => p.Price).HasColumnType("money");
        });

        /* ---------- DishOrder ---------- */
        modelBuilder.Entity<DishOrder>(e =>
        {
            e.ToTable("DishOrder");

            e.Property(p => p.Id)              // ⬅️  IDENTITY(1,1)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            e.Property(p => p.Quantity).IsRequired();

            e.HasOne(d => d.Dish)
                .WithMany(p => p.DishOrders)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DishOrder_DIsh");

            e.HasOne(d => d.Order)
                .WithMany(p => p.DishOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DishOrder_Order");
        });

        /* ---------- Order ---------- */
        modelBuilder.Entity<Order>(e =>
        {
            e.ToTable("Order");
            e.Property(p => p.Id).ValueGeneratedNever().HasColumnName("id");
            e.Property(p => p.DateTime).HasColumnType("datetime");
            e.Property(p => p.Sum).HasColumnType("money");

            e.HasOne(d => d.Client)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Client");

            e.HasOne(d => d.Table)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Tables");
        });

        /* ---------- Table ---------- */
        modelBuilder.Entity<Table>(e =>
        {
            e.ToTable("Tables");
            e.Property(p => p.Id).ValueGeneratedNever().HasColumnName("id");
            e.Property(p => p.Number);
        });

        /* ---------- Worker ---------- */
        modelBuilder.Entity<Worker>(e =>
        {
            e.ToTable("Worker");
            e.Property(p => p.Id).ValueGeneratedNever().HasColumnName("id");
            e.Property(p => p.FirstName).HasMaxLength(200).IsUnicode(false);
            e.Property(p => p.LastName).HasMaxLength(200).IsUnicode(false);
            e.Property(p => p.CityName).HasMaxLength(200).IsUnicode(false);
            e.Property(p => p.StreetName).HasMaxLength(200).IsUnicode(false);

            e.HasOne(d => d.Client)
                .WithMany(p => p.Workers)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Worker_Client");
        });
    }
}
