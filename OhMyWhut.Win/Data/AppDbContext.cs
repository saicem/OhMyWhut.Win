using System.IO;
using Microsoft.EntityFrameworkCore;
using Windows.ApplicationModel;
using Windows.Storage;

namespace OhMyWhut.Win.Data
{
    public class AppDbContext : DbContext
    {
        internal DbSet<DetailCourse> AllCourses { get; set; }

        internal DbSet<MyCourse> MyCourses { get; set; }

        internal DbSet<Book> Books { get; set; }

        internal DbSet<ElectricFee> ElectricFees { get; set; }

        internal DbSet<Log> Logs { get; set; }

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string demoDatabasePath = Package.Current.InstalledLocation.Path + @"\Assets\app.db";
            string databasePath = ApplicationData.Current.LocalFolder.Path + @"\app.db";
            if (!File.Exists(databasePath))
            {
                File.Copy(demoDatabasePath, databasePath);
            }
            options.UseSqlite($"Data Source={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MyCourse>(eb =>
            {
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Name).HasMaxLength(32);
                eb.Property(x => x.Position).HasMaxLength(32);
                eb.Property(x => x.Teacher).HasMaxLength(256);
            });

            builder.Entity<DetailCourse>(eb =>
            {
                eb.HasKey(x => x.Id);
            });

            builder.Entity<Book>(eb =>
            {
                eb.HasKey(x => x.Id);
            });

            builder.Entity<ElectricFee>(eb =>
            {
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Unit).HasMaxLength(4).IsFixedLength(true);
                eb.Property(x => x.RemainName).HasMaxLength(4).IsFixedLength(true);
            });

            builder.Entity<Log>(eb =>
            {
                eb.HasKey(x => x.Id);
                eb.HasIndex(x => x.Type);
            });

            base.OnModelCreating(builder);
        }
    }
}
