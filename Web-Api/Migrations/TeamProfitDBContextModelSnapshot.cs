﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web_Api.Data;

#nullable disable

namespace Web_Api.Migrations
{
    [DbContext(typeof(TeamProfitDBContext))]
    partial class TeamProfitDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Web_Api.Model.Cost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("Amounts")
                        .HasColumnType("numeric");

                    b.Property<int>("IdUser")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdUser");

                    b.ToTable("Costs");
                });

            modelBuilder.Entity("Web_Api.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Web_Api.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateRegistration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int?>("IdTeam")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("PriceWork")
                        .HasColumnType("numeric");

                    b.Property<string>("Salt")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IdTeam");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Web_Api.Model.Cost", b =>
                {
                    b.HasOne("Web_Api.Model.User", "User")
                        .WithMany("Costs")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web_Api.Model.User", b =>
                {
                    b.HasOne("Web_Api.Model.Team", "Team")
                        .WithMany("Users")
                        .HasForeignKey("IdTeam");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Web_Api.Model.Team", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Web_Api.Model.User", b =>
                {
                    b.Navigation("Costs");
                });
#pragma warning restore 612, 618
        }
    }
}
