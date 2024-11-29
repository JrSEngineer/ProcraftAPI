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

            modelBuilder.Entity("ProcraftAPI.Entities.Joins.StepUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StepId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "StepId");

                    b.HasIndex("StepId");

                    b.ToTable("StepUser");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcraftProcess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("FinishForecast")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FinishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ManagerId")
                        .HasColumnType("uuid");

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

                    b.HasIndex("ManagerId");

                    b.HasIndex("ScopeId");

                    b.ToTable("Process");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Scope.ProcessScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Scope");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Scope.ScopeAbility", b =>
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

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Step.ProcessAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Duration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FinishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Progress")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("StepId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("StepId");

                    b.HasIndex("UserId");

                    b.ToTable("Action");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Step.ProcessStep", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("FinishForecast")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProcessId")
                        .HasColumnType("uuid");

                    b.Property<int>("Progress")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartForecast")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProcessId");

                    b.ToTable("Step");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcessManager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Manager");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("GroupId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.UserAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AddressNumber")
                        .HasColumnType("integer");

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

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Authentication");
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

            modelBuilder.Entity("ProcraftAPI.Entities.Joins.StepUser", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.Step.ProcessStep", "Step")
                        .WithMany("StepUsers")
                        .HasForeignKey("StepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcraftAPI.Entities.User.ProcraftUser", "User")
                        .WithMany("StepUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Step");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcraftProcess", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.User.ProcessManager", "Manager")
                        .WithMany("Processes")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcraftAPI.Entities.Process.Scope.ProcessScope", "Scope")
                        .WithMany()
                        .HasForeignKey("ScopeId");

                    b.Navigation("Manager");

                    b.Navigation("Scope");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Scope.ScopeAbility", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.Scope.ProcessScope", null)
                        .WithMany("Abilities")
                        .HasForeignKey("ProcessScopeId");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Step.ProcessAction", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.Step.ProcessStep", null)
                        .WithMany("Actions")
                        .HasForeignKey("StepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcraftAPI.Entities.User.ProcraftUser", null)
                        .WithMany("Actions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Step.ProcessStep", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.Process.ProcraftProcess", "Process")
                        .WithMany("Steps")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Process");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftUser", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.User.UserAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcraftAPI.Entities.User.ProcraftGroup", null)
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("ProcraftAPI.Security.Authentication.ProcraftAuthentication", b =>
                {
                    b.HasOne("ProcraftAPI.Entities.User.ProcraftUser", "User")
                        .WithOne("Authentication")
                        .HasForeignKey("ProcraftAPI.Security.Authentication.ProcraftAuthentication", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.ProcraftProcess", b =>
                {
                    b.Navigation("ProcessesUsers");

                    b.Navigation("Steps");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Scope.ProcessScope", b =>
                {
                    b.Navigation("Abilities");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.Process.Step.ProcessStep", b =>
                {
                    b.Navigation("Actions");

                    b.Navigation("StepUsers");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcessManager", b =>
                {
                    b.Navigation("Processes");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftGroup", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("ProcraftAPI.Entities.User.ProcraftUser", b =>
                {
                    b.Navigation("Actions");

                    b.Navigation("Authentication")
                        .IsRequired();

                    b.Navigation("ProcessesUsers");

                    b.Navigation("StepUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
