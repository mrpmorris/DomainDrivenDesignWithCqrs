﻿// <auto-generated />
using System;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.DbMigrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220426165816_FKFromOrgToOrgType")]
    partial class FKFromOrgToOrgType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DomainDrivenDesignWithCqrs.AppLayer.DomainEntities.Organisation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("UX_Organisation_Name");

                    b.HasIndex("TypeId");

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("DomainDrivenDesignWithCqrs.AppLayer.DomainEntities.OrganisationType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("UX_OrganisationType_Name");

                    b.ToTable("OrganisationType");
                });

            modelBuilder.Entity("DomainDrivenDesignWithCqrs.AppLayer.DomainEntities.Organisation", b =>
                {
                    b.HasOne("DomainDrivenDesignWithCqrs.AppLayer.DomainEntities.OrganisationType", null)
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
