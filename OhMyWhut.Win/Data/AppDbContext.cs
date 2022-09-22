using System;
using Microsoft.EntityFrameworkCore;
using OhMyWhut.Win.Migrations;

namespace OhMyWhut.Win.Data
{
    public class AppDbContext : DbContext
    {
        internal DbSet<DetailCourse> AllCourses { get; set; }

        internal DbSet<MyCourse> MyCourses { get; set; }

        internal DbSet<Book> Books { get; set; }

        internal DbSet<Preference> Preferences { get; set; }

        internal DbSet<Log> Logs { get; set; }

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }

        public void AddLog(string name, string data)
        {
            _ = Logs.AddAsync(new Log
            {
                Name = name,
                Data = data,
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={System.IO.Path.Join(App.DataFolder, "app.db")}");
        }

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

            builder.Entity<Log>(eb =>
            {
                eb.HasKey(x => x.Id);
                eb.HasIndex(x => x.Name);
                eb.Property(x => x.Name).HasMaxLength(64);
                eb.Property(x => x.CreatedAt).HasDefaultValueSql("datetime()");
            });

            base.OnModelCreating(builder);
        }
    }
}
