using data_access_layer.Model.Db;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Type = data_access_layer.Model.Db.Type;

namespace data_access_layer.Context
{
    public class PrinterContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<RoomHistory> RoomHistories { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Type> Types { get; set; }

        public PrinterContext(DbContextOptions<PrinterContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Debug.WriteLine("PrinterContext OnConfiguring");
            optionsBuilder.UseSqlite($"Filename={Path.Combine(@"E:\work\Projects\print-data-crawler\", "PrinterDB.db")}");
            base.OnConfiguring(optionsBuilder);
        }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Debug.WriteLine("OnModelCreating");
            #region Constraint length restrictions
            // City name.
            modelBuilder.Entity<City>(entity =>
                entity.HasCheckConstraint("CK_City_Name", "length([Name]) < 64"));
            // Room history name.
            modelBuilder.Entity<RoomHistory>(entity =>
                entity.HasCheckConstraint("CK_RoomHistory_Name", "length([Name]) < 128"));
            // Organization name.
            modelBuilder.Entity<Organization>(entity =>
                entity.HasCheckConstraint("CK_Organization_Name", "length([Name]) < 128"));
            // Room name.
            modelBuilder.Entity<Room>(entity =>
                entity.HasCheckConstraint("CK_Room_Name", "length([Name]) < 128"));
            // Type name.
            modelBuilder.Entity<Type>(entity =>
                entity.HasCheckConstraint("CK_Type_Name", "length([Name]) < 128"));
            #endregion
        }
    }
}