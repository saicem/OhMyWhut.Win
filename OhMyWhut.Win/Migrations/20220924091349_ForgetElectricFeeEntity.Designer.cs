﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OhMyWhut.Win.Data;

#nullable disable

namespace OhMyWhut.Win.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220924091349_ForgetElectricFeeEntity")]
    partial class ForgetElectricFeeEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("OhMyWhut.Win.Data.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("BorrowDate")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("ExpireDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("OhMyWhut.Win.Data.DetailCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("College")
                        .HasColumnType("TEXT");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EndSec")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EndWeek")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .HasColumnType("TEXT");

                    b.Property<string>("SelectCode")
                        .HasColumnType("TEXT");

                    b.Property<int>("StartSec")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StartWeek")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Teachers")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AllCourses");
                });

            modelBuilder.Entity("OhMyWhut.Win.Data.ElectricFee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<float>("MeterOverdue")
                        .HasColumnType("REAL");

                    b.Property<string>("RemainName")
                        .HasMaxLength(4)
                        .HasColumnType("TEXT")
                        .IsFixedLength();

                    b.Property<float>("RemainPower")
                        .HasColumnType("REAL");

                    b.Property<float>("TotalValue")
                        .HasColumnType("REAL");

                    b.Property<string>("Unit")
                        .HasMaxLength(4)
                        .HasColumnType("TEXT")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.ToTable("ElectricFees");
                });

            modelBuilder.Entity("OhMyWhut.Win.Data.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("datetime()");

                    b.Property<string>("Data")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Type");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("OhMyWhut.Win.Data.MyCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EndSec")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EndWeek")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<int>("StartSec")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StartWeek")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Teacher")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MyCourses");
                });

            modelBuilder.Entity("OhMyWhut.Win.Data.Preference", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("Preferences");
                });
#pragma warning restore 612, 618
        }
    }
}
