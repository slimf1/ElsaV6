﻿// <auto-generated />
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ElsaContext))]
    partial class ElsaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("DataAccess.Models.BadgeModel", b =>
                {
                    b.Property<int>("BadgeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsTrophy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserModelUserID")
                        .HasColumnType("TEXT");

                    b.HasKey("BadgeID");

                    b.HasIndex("UserModelUserID");

                    b.ToTable("Badges");
                });

            modelBuilder.Entity("DataAccess.Models.UserModel", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Avatar")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("OnTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RegDate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccess.Models.BadgeModel", b =>
                {
                    b.HasOne("DataAccess.Models.UserModel", null)
                        .WithMany("Badges")
                        .HasForeignKey("UserModelUserID");
                });

            modelBuilder.Entity("DataAccess.Models.UserModel", b =>
                {
                    b.Navigation("Badges");
                });
#pragma warning restore 612, 618
        }
    }
}
