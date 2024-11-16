﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProcraftAPI.Data.Context;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    [DbContext(typeof(ProcraftDbContext))]
    partial class ProcraftDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProcraftAPI.Entities.Joins.ProcessUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProcessId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "ProcessId");

                    b.HasIndex("ProcessId");

                    b.ToTable("ProcessUser");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcessScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ScopeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Scope");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcraftProcess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ConclusionForecast")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("FinishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Progress")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ScopeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartForecast")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScopeId");

                    b.ToTable("Process");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ScopeAbility", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ProcessScopeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ScopeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProcessScopeId");

                    b.ToTable("Ability");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<string>("AuthenticationEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("AuthenticationEmail");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.UserAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("ProcraftAPI.Security.Authentication.ProcraftAuthentication", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Email");

                    b.ToTable("Authentication");
                });

            modelBuilder.Entity("ProcraftProcessProcraftUser", b =>
                {
                    b.Property<Guid>("ProcessesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("ProcessesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ProcraftProcessProcraftUser");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Joins.ProcessUser", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.ProcraftProcess", "Process")
                        .WithMany("ProcessesUsers")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcraftAPI.Entities.User.ProcraftUser", "User")
                        .WithMany("ProcessesUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Process");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcraftProcess", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.ProcessScope", "Scope")
                        .WithMany()
                        .HasForeignKey("ScopeId");

                    b.Navigation("Scope");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ScopeAbility", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.ProcessScope", null)
                        .WithMany("Abilities")
                        .HasForeignKey("ProcessScopeId");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftUser", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.User.UserAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcraftAPI.Security.Authentication.ProcraftAuthentication", "Authentication")
                        .WithMany()
                        .HasForeignKey("AuthenticationEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Authentication");
                });

            modelBuilder.Entity("ProcraftProcessProcraftUser", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.ProcraftProcess", null)
                        .WithMany()
                        .HasForeignKey("ProcessesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcraftAPI.Entities.User.ProcraftUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcessScope", b =>
                {
                    b.Navigation("Abilities");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcraftProcess", b =>
                {
                    b.Navigation("ProcessesUsers");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftUser", b =>
                {
                    b.Navigation("ProcessesUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
