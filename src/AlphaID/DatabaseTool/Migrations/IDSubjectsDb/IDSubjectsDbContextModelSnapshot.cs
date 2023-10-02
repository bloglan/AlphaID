﻿// <auto-generated />
using System;
using AlphaIDEntityFramework.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatabaseTool.Migrations.IDSubjectsDb
{
    [DbContext(typeof(IDSubjectsDbContext))]
    partial class IDSubjectsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AlphaIDEntityFramework.EntityFramework.NaturalPersonImage", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("Photo")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhotoMimeType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("NaturalPersonImage");
                });

            modelBuilder.Entity("IDSubjects.GenericOrganization", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Contact")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Domicile")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("EstablishedAt")
                        .HasColumnType("date");

                    b.Property<string>("LegalPersonName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("TermBegin")
                        .HasColumnType("date");

                    b.Property<DateTime?>("TermEnd")
                        .HasColumnType("date");

                    b.Property<string>("USCI")
                        .HasMaxLength(18)
                        .HasColumnType("char(18)");

                    b.Property<DateTime>("WhenChanged")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("WhenCreated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("USCI")
                        .IsUnique()
                        .HasFilter("[USCI] IS NOT NULL");

                    b.HasIndex("WhenChanged");

                    b.HasIndex("WhenCreated");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("IDSubjects.NaturalPerson", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("LastName")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Mobile")
                        .HasMaxLength(14)
                        .IsUnicode(false)
                        .HasColumnType("varchar(14)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("NextRealNameValidTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("PasswordLastSet")
                        .HasColumnType("datetime2");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneticGivenName")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("PhoneticSearchHint")
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("PhoneticSurname")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime?>("RealNameValidTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Sex")
                        .HasColumnType("varchar(6)")
                        .HasComment("性别");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("WhenChanged")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("WhenCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("Mobile")
                        .IsUnique()
                        .HasFilter("[Mobile] IS NOT NULL");

                    b.HasIndex("Name");

                    b.HasIndex("PhoneticSearchHint");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.HasIndex("WhenChanged");

                    b.HasIndex("WhenCreated");

                    b.ToTable("NaturalPerson");
                });

            modelBuilder.Entity("IDSubjects.NaturalPersonClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("NaturalPersonClaim");
                });

            modelBuilder.Entity("IDSubjects.NaturalPersonLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("NaturalPersonLogin");
                });

            modelBuilder.Entity("IDSubjects.NaturalPersonToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Value")
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("NaturalPersonToken");
                });

            modelBuilder.Entity("IDSubjects.OrganizationAdministrator", b =>
                {
                    b.Property<string>("OrganizationId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PersonId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsOrganizationCreator")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("OrganizationId", "PersonId");

                    b.ToTable("OrganizationAdministrator");
                });

            modelBuilder.Entity("IDSubjects.OrganizationBankAccount", b =>
                {
                    b.Property<string>("AccountNumber")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("OrganizationId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("AccountName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("BankName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AccountNumber", "OrganizationId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganizationBankAccount");
                });

            modelBuilder.Entity("IDSubjects.OrganizationMember", b =>
                {
                    b.Property<string>("PersonId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("OrganizationId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Department")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Remark")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PersonId", "OrganizationId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganizationMember");
                });

            modelBuilder.Entity("IDSubjects.OrganizationUsedName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DeprecateTime")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("OrganizationId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganizationUsedName");
                });

            modelBuilder.Entity("IDSubjects.PersonBankAccount", b =>
                {
                    b.Property<string>("AccountNumber")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PersonId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("AccountName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("BankName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AccountNumber", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonBankAccount");
                });

            modelBuilder.Entity("IDSubjects.RealName.ChineseIDCardValidation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CommitTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PersonId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("RealNameValidation");
                });

            modelBuilder.Entity("AlphaIDEntityFramework.EntityFramework.NaturalPersonImage", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.NaturalPerson", b =>
                {
                    b.OwnsOne("IDSubjects.PersonChineseIDCardInfo", "ChineseIDCard", b1 =>
                        {
                            b1.Property<string>("NaturalPersonId")
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("Address")
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("CardNumber")
                                .IsRequired()
                                .HasMaxLength(18)
                                .IsUnicode(false)
                                .HasColumnType("varchar(18)");

                            b1.Property<string>("Ethnicity")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.HasKey("NaturalPersonId");

                            b1.ToTable("NaturalPerson");

                            b1.WithOwner()
                                .HasForeignKey("NaturalPersonId");
                        });

                    b.Navigation("ChineseIDCard");
                });

            modelBuilder.Entity("IDSubjects.NaturalPersonClaim", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.NaturalPersonLogin", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.NaturalPersonToken", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.OrganizationAdministrator", b =>
                {
                    b.HasOne("IDSubjects.GenericOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("IDSubjects.OrganizationBankAccount", b =>
                {
                    b.HasOne("IDSubjects.GenericOrganization", "Organization")
                        .WithMany("BankAccounts")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("IDSubjects.OrganizationMember", b =>
                {
                    b.HasOne("IDSubjects.GenericOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.OrganizationUsedName", b =>
                {
                    b.HasOne("IDSubjects.GenericOrganization", "Organization")
                        .WithMany("UsedNames")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("IDSubjects.PersonBankAccount", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany("BankAccounts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.RealName.ChineseIDCardValidation", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("IDSubjects.ChinesePersonName", "ChinesePersonName", b1 =>
                        {
                            b1.Property<int>("ChineseIDCardValidationId")
                                .HasColumnType("int");

                            b1.Property<string>("GivenName")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)");

                            b1.Property<string>("PhoneticGivenName")
                                .IsRequired()
                                .HasMaxLength(40)
                                .IsUnicode(false)
                                .HasColumnType("varchar(40)");

                            b1.Property<string>("PhoneticSurname")
                                .HasMaxLength(20)
                                .IsUnicode(false)
                                .HasColumnType("varchar(20)");

                            b1.Property<string>("Surname")
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)");

                            b1.HasKey("ChineseIDCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIDCardValidationId");
                        });

                    b.OwnsOne("IDSubjects.RealName.ChineseIDCardImage", "ChineseIDCardImage", b1 =>
                        {
                            b1.Property<int>("ChineseIDCardValidationId")
                                .HasColumnType("int");

                            b1.Property<byte[]>("IssuerFace")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<string>("IssuerFaceMimeType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.Property<byte[]>("PersonalFace")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<string>("PersonalFaceMimeType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.HasKey("ChineseIDCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIDCardValidationId");
                        });

                    b.OwnsOne("IDSubjects.RealName.ChineseIDCardInfo", "ChineseIDCard", b1 =>
                        {
                            b1.Property<int>("ChineseIDCardValidationId")
                                .HasColumnType("int");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("CardNumber")
                                .IsRequired()
                                .HasMaxLength(18)
                                .IsUnicode(false)
                                .HasColumnType("varchar(18)");

                            b1.Property<DateTime>("DateOfBirth")
                                .HasColumnType("date");

                            b1.Property<string>("Ethnicity")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<DateTime?>("Expires")
                                .HasColumnType("date");

                            b1.Property<DateTime>("IssueDate")
                                .HasColumnType("date");

                            b1.Property<string>("Issuer")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Sex")
                                .IsRequired()
                                .HasColumnType("varchar(7)");

                            b1.HasKey("ChineseIDCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIDCardValidationId");
                        });

                    b.OwnsOne("IDSubjects.RealName.ValidationResult", "Result", b1 =>
                        {
                            b1.Property<int>("ChineseIDCardValidationId")
                                .HasColumnType("int");

                            b1.Property<bool>("Accepted")
                                .HasColumnType("bit");

                            b1.Property<DateTime>("ValidateTime")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Validator")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.HasKey("ChineseIDCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIDCardValidationId");
                        });

                    b.Navigation("ChineseIDCard");

                    b.Navigation("ChineseIDCardImage");

                    b.Navigation("ChinesePersonName");

                    b.Navigation("Person");

                    b.Navigation("Result");
                });

            modelBuilder.Entity("IDSubjects.GenericOrganization", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("UsedNames");
                });

            modelBuilder.Entity("IDSubjects.NaturalPerson", b =>
                {
                    b.Navigation("BankAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
