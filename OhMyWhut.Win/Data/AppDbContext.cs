using System;
using Microsoft.EntityFrameworkCore;

namespace OhMyWhut.Win.Data
{
    internal class AppDbContext : DbContext
    {
        internal DbSet<DetailCourse> AllCourses { get; set; }

        internal DbSet<MyCourse> MyCourses { get; set; }

        internal DbSet<Book> Books { get; set; }

        internal DbSet<Preference> Preferences { get; set; }

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            options.UseSqlite($"Data Source={System.IO.Path.Join(path, "ohmywhut.db")}");
        }


        public record Preference(string Key, string Value);

        public record MyCourse(int Id,
                               string Name,
                               string Teacher,
                               string Position,
                               DayOfWeek DayOfWeek,
                               int StartWeek,
                               int EndWeek,
                               int StartSec,
                               int EndSec);

        public record DetailCourse(int Id,
                                   string SelectCode,
                                   string Code,
                                   string College,
                                   string Name,
                                   string Teachers,
                                   string Position,
                                   DayOfWeek DayOfWeek,
                                   int StartWeek,
                                   int EndWeek,
                                   int StartSec,
                                   int EndSec);

        public record ElectricFee(int Id,
                                  DateTimeOffset CreatedAt,
                                  float RemainPower,
                                  string RemainName,
                                  float TotalValue,
                                  string Unit,
                                  float MeterOverdue);

        public record Book(int Id, string Name, DateOnly BorrowDate, DateOnly ExpireDate);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Preference>(eb =>
            {
                eb.HasKey(x => x.Key);
                eb.Property(x => x.Key).HasMaxLength(64).IsRequired();
                eb.Property(x => x.Value).HasMaxLength(64).IsRequired();
            });

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
                eb.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
            });

            base.OnModelCreating(builder);
        }
    }
}
