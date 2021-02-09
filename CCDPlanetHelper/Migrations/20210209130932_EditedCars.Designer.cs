﻿// <auto-generated />
using CCDPlanetHelper.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CCDPlanetHelper.Migrations
{
    [DbContext(typeof(BotData))]
    [Migration("20210209130932_EditedCars")]
    partial class EditedCars
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("CCDPlanetHelper.Database.Ad", b =>
                {
                    b.Property<long>("AdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("DateCreate")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Owner")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Server")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("AdId");

                    b.ToTable("Ads");
                });

            modelBuilder.Entity("CCDPlanetHelper.Database.CarInfo", b =>
                {
                    b.Property<long>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MaxSpeed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<long>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<long>("PriceDonate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TuningPacks")
                        .HasColumnType("TEXT");

                    b.HasKey("CarId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CCDPlanetHelper.Database.ReminderInfo", b =>
                {
                    b.Property<long>("ReminderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mouth")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Sent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReminderId");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("CCDPlanetHelper.Database.Report", b =>
                {
                    b.Property<long>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAnswered")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<long>("ModeratorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModeratorReply")
                        .HasColumnType("TEXT");

                    b.Property<long>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReportId");

                    b.ToTable("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}
