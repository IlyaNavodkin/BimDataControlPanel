﻿// <auto-generated />
using System;
using BimDataControlPanel.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BimDataControlPanel.WEB.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230623065227_InitMainData")]
    partial class InitMainData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("BimDataControlPanel.DAL.Entities.Change", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ChangeTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserRevitName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Changes");
                });

            modelBuilder.Entity("BimDataControlPanel.DAL.Entities.Project", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Complete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RevitVersion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("BimDataControlPanel.DAL.Entities.ProjectUser", b =>
                {
                    b.Property<string>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("TEXT");

                    b.HasKey("ProjectId", "IdentityUserId");

                    b.ToTable("ProjectUsers");
                });

            modelBuilder.Entity("BimDataControlPanel.DAL.Entities.Change", b =>
                {
                    b.HasOne("BimDataControlPanel.DAL.Entities.Project", "Project")
                        .WithMany("Changes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BimDataControlPanel.DAL.Entities.ProjectUser", b =>
                {
                    b.HasOne("BimDataControlPanel.DAL.Entities.Project", "Project")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BimDataControlPanel.DAL.Entities.Project", b =>
                {
                    b.Navigation("Changes");

                    b.Navigation("ProjectUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
