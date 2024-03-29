﻿// <auto-generated />
using System;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BonusSystemApplication.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230602154657_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Conclusion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("EmployeeComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FormId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsProposalForBonusPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ManagerComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("OverallKpi")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("FormId")
                        .IsUnique();

                    b.ToTable("Conclusions");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Definition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("ApproverId")
                        .HasColumnType("bigint");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<long>("FormId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsWpmHox")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<long?>("ManagerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Period")
                        .HasColumnType("int");

                    b.Property<long>("WorkprojectId")
                        .HasColumnType("bigint");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("FormId")
                        .IsUnique();

                    b.HasIndex("ManagerId");

                    b.HasIndex("WorkprojectId");

                    b.HasIndex("EmployeeId", "WorkprojectId", "Period", "Year")
                        .IsUnique();

                    b.ToTable("Definitions");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Department", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Form", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<bool>("AreObjectivesFrozen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("AreResultsFrozen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastSavedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastSavedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.GlobalAccess", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("DepartmentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long?>("WorkprojectId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkprojectId");

                    b.ToTable("GlobalAccess");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.LocalAccess", b =>
                {
                    b.Property<long>("FormId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("FormId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LocalAccess");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.ObjectiveResult", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("FormId")
                        .HasColumnType("bigint");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FormId", "Row")
                        .IsUnique();

                    b.ToTable("ObjectivesResults");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Position", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Abbreviation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEng")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameRus")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NameEng")
                        .IsUnique();

                    b.HasIndex("NameRus")
                        .IsUnique();

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Signatures", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("ForObjectivesApproverSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ForObjectivesEmployeeSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ForObjectivesIsRejectedByEmployee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("ForObjectivesIsSignedByApprover")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("ForObjectivesIsSignedByEmployee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("ForObjectivesIsSignedByManager")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ForObjectivesManagerSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ForResultsApproverSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ForResultsEmployeeSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ForResultsIsRejectedByEmployee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("ForResultsIsSignedByApprover")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("ForResultsIsSignedByEmployee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("ForResultsIsSignedByManager")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ForResultsManagerSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FormId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FormId")
                        .IsUnique();

                    b.ToTable("Signatures");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Team", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("DepartmentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstNameEng")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstNameRus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("LastNameEng")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastNameRus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MiddleNameRus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pid")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("PositionId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TeamId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("Pid")
                        .IsUnique();

                    b.HasIndex("PositionId");

                    b.HasIndex("TeamId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Workproject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Workprojects");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Conclusion", b =>
                {
                    b.HasOne("BonusSystemApplication.DAL.Entities.Form", "Form")
                        .WithOne("Conclusion")
                        .HasForeignKey("BonusSystemApplication.DAL.Entities.Conclusion", "FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Definition", b =>
                {
                    b.HasOne("BonusSystemApplication.DAL.Entities.User", "Approver")
                        .WithMany("ApproverFormDefinitions")
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BonusSystemApplication.DAL.Entities.User", "Employee")
                        .WithMany("EmployeeFormDefinitions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BonusSystemApplication.DAL.Entities.Form", "Form")
                        .WithOne("Definition")
                        .HasForeignKey("BonusSystemApplication.DAL.Entities.Definition", "FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BonusSystemApplication.DAL.Entities.User", "Manager")
                        .WithMany("ManagerFormDefinitions")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BonusSystemApplication.DAL.Entities.Workproject", "Workproject")
                        .WithMany("FormDefinitions")
                        .HasForeignKey("WorkprojectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Approver");

                    b.Navigation("Employee");

                    b.Navigation("Form");

                    b.Navigation("Manager");

                    b.Navigation("Workproject");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.GlobalAccess", b =>
                {
                    b.HasOne("BonusSystemApplication.DAL.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("BonusSystemApplication.DAL.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("BonusSystemApplication.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BonusSystemApplication.DAL.Entities.Workproject", "Workproject")
                        .WithMany()
                        .HasForeignKey("WorkprojectId");

                    b.Navigation("Department");

                    b.Navigation("Team");

                    b.Navigation("User");

                    b.Navigation("Workproject");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.LocalAccess", b =>
                {
                    b.HasOne("BonusSystemApplication.DAL.Entities.Form", "Form")
                        .WithMany("LocalAccesses")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BonusSystemApplication.DAL.Entities.User", "User")
                        .WithMany("LocalAccesses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.ObjectiveResult", b =>
                {
                    b.HasOne("BonusSystemApplication.DAL.Entities.Form", "Form")
                        .WithMany("ObjectivesResults")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("BonusSystemApplication.DAL.Entities.Objective", "Objective", b1 =>
                        {
                            b1.Property<long>("ObjectiveResultId")
                                .HasColumnType("bigint");

                            b1.Property<double?>("Challenge")
                                .HasColumnType("float");

                            b1.Property<string>("Description")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<bool>("IsKey")
                                .HasColumnType("bit");

                            b1.Property<bool>("IsMeasurable")
                                .HasColumnType("bit");

                            b1.Property<double?>("KpiUpperLimit")
                                .HasColumnType("float");

                            b1.Property<string>("Statement")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<double?>("Target")
                                .HasColumnType("float");

                            b1.Property<double?>("Threshold")
                                .HasColumnType("float");

                            b1.Property<string>("Unit")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<double?>("WeightFactor")
                                .HasColumnType("float");

                            b1.HasKey("ObjectiveResultId");

                            b1.ToTable("ObjectivesResults");

                            b1.WithOwner()
                                .HasForeignKey("ObjectiveResultId");
                        });

                    b.OwnsOne("BonusSystemApplication.DAL.Entities.Result", "Result", b1 =>
                        {
                            b1.Property<long>("ObjectiveResultId")
                                .HasColumnType("bigint");

                            b1.Property<double?>("Achieved")
                                .HasColumnType("float");

                            b1.Property<string>("KeyCheck")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<double?>("Kpi")
                                .HasColumnType("float");

                            b1.HasKey("ObjectiveResultId");

                            b1.ToTable("ObjectivesResults");

                            b1.WithOwner()
                                .HasForeignKey("ObjectiveResultId");
                        });

                    b.Navigation("Form");

                    b.Navigation("Objective")
                        .IsRequired();

                    b.Navigation("Result")
                        .IsRequired();
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Signatures", b =>
                {
                    b.HasOne("BonusSystemApplication.DAL.Entities.Form", "Form")
                        .WithOne("Signatures")
                        .HasForeignKey("BonusSystemApplication.DAL.Entities.Signatures", "FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.User", b =>
                {
                    b.HasOne("BonusSystemApplication.DAL.Entities.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BonusSystemApplication.DAL.Entities.Position", "Position")
                        .WithMany("Users")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BonusSystemApplication.DAL.Entities.Team", "Team")
                        .WithMany("Users")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Department");

                    b.Navigation("Position");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Department", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Form", b =>
                {
                    b.Navigation("Conclusion")
                        .IsRequired();

                    b.Navigation("Definition")
                        .IsRequired();

                    b.Navigation("LocalAccesses");

                    b.Navigation("ObjectivesResults");

                    b.Navigation("Signatures")
                        .IsRequired();
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Position", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Team", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.User", b =>
                {
                    b.Navigation("ApproverFormDefinitions");

                    b.Navigation("EmployeeFormDefinitions");

                    b.Navigation("LocalAccesses");

                    b.Navigation("ManagerFormDefinitions");
                });

            modelBuilder.Entity("BonusSystemApplication.DAL.Entities.Workproject", b =>
                {
                    b.Navigation("FormDefinitions");
                });
#pragma warning restore 612, 618
        }
    }
}
