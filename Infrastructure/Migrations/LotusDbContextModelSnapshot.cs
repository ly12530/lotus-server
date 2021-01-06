﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(LotusDbContext))]
    partial class LotusDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Core.Domain.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EmailAddress = "pieter@test.test",
                            Name = "Pieter"
                        },
                        new
                        {
                            Id = 2,
                            EmailAddress = "kek@double.you",
                            Name = "Jorik"
                        });
                });

            modelBuilder.Entity("Core.Domain.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EndTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsExam")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("boolean");

                    b.Property<int>("LessonType")
                        .HasColumnType("integer");

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RequestUser", b =>
                {
                    b.Property<int>("RequestsId")
                        .HasColumnType("integer");

                    b.Property<int>("SubscribersId")
                        .HasColumnType("integer");

                    b.HasKey("RequestsId", "SubscribersId");

                    b.HasIndex("SubscribersId");

                    b.ToTable("RequestUser");
                });

            modelBuilder.Entity("Core.Domain.Request", b =>
                {
                    b.HasOne("Core.Domain.Customer", "Customer")
                        .WithMany("Requests")
                        .HasForeignKey("CustomerId");

                    b.OwnsOne("Core.Domain.Address", "Address", b1 =>
                        {
                            b1.Property<int>("RequestId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .UseIdentityByDefaultColumn();

                            b1.Property<string>("City")
                                .HasColumnType("text");

                            b1.Property<double[]>("Geometry")
                                .HasColumnType("double precision[]");

                            b1.Property<string>("Number")
                                .HasColumnType("text");

                            b1.Property<string>("Postcode")
                                .HasColumnType("text");

                            b1.Property<string>("Street")
                                .HasColumnType("text");

                            b1.HasKey("RequestId");

                            b1.ToTable("Requests");

                            b1.WithOwner()
                                .HasForeignKey("RequestId");
                        });

                    b.Navigation("Address");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("RequestUser", b =>
                {
                    b.HasOne("Core.Domain.Request", null)
                        .WithMany()
                        .HasForeignKey("RequestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("SubscribersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Customer", b =>
                {
                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
