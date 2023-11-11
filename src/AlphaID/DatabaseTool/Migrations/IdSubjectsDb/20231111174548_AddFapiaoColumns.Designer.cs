﻿// <auto-generated />
using System;
using AlphaID.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace DatabaseTool.Migrations.IdSubjectsDb
{
    [DbContext(typeof(IdSubjectsDbContext))]
    [Migration("20231111174548_AddFapiaoColumns")]
    partial class AddFapiaoColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AlphaID.EntityFramework.NaturalPersonClaim", b =>
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

            modelBuilder.Entity("AlphaID.EntityFramework.NaturalPersonLogin", b =>
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

            modelBuilder.Entity("AlphaID.EntityFramework.NaturalPersonToken", b =>
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
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("NaturalPersonToken");
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

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Domicile")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("EstablishedAt")
                        .HasColumnType("date");

                    b.Property<Geometry>("Location")
                        .HasColumnType("geography");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Representative")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("TermBegin")
                        .HasColumnType("date");

                    b.Property<DateTime?>("TermEnd")
                        .HasColumnType("date");

                    b.Property<string>("Website")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTimeOffset>("WhenChanged")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("WhenCreated")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("WhenChanged");

                    b.HasIndex("WhenCreated");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("IDSubjects.Invitations.JoinOrganizationInvitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Accepted")
                        .HasColumnType("bit");

                    b.Property<int>("ExpectVisibility")
                        .HasColumnType("int");

                    b.Property<string>("InviteeId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Inviter")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OrganizationId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("WhenCreated")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("WhenExpired")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("InviteeId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("JoinOrganizationInvitation");
                });

            modelBuilder.Entity("IDSubjects.NaturalPerson", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Bio")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("CanEditPersonName")
                        .HasColumnType("bit");

                    b.Property<string>("ConcurrencyStamp")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Gender")
                        .HasColumnType("varchar(6)")
                        .HasComment("性别");

                    b.Property<string>("Locale")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NickName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTimeOffset?>("PasswordLastSet")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneticGivenName")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("PhoneticSurname")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("SecurityStamp")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TimeZone")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("WebSite")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTimeOffset>("WhenChanged")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("WhenCreated")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.HasIndex("WhenChanged");

                    b.HasIndex("WhenCreated");

                    b.ToTable("NaturalPerson");
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
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("BankName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<string>("Usage")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

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

                    b.Property<bool>("IsOwner")
                        .HasColumnType("bit");

                    b.Property<string>("Remark")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

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

            modelBuilder.Entity("IDSubjects.Payments.PersonBankAccount", b =>
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

            modelBuilder.Entity("IDSubjects.RealName.ChineseIdCardValidation", b =>
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

            modelBuilder.Entity("IDSubjects.RealName.RealNameInfo", b =>
                {
                    b.Property<string>("PersonId")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("AcceptedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("AcceptedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("PersonId");

                    b.ToTable("RealNameInfo");
                });

            modelBuilder.Entity("AlphaID.EntityFramework.NaturalPersonClaim", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AlphaID.EntityFramework.NaturalPersonLogin", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AlphaID.EntityFramework.NaturalPersonToken", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("IDSubjects.GenericOrganization", b =>
                {
                    b.OwnsOne("IDSubjects.FapiaoInfo", "Fapiao", b1 =>
                        {
                            b1.Property<string>("GenericOrganizationId")
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("Account")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Bank")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Contact")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("TaxPayerId")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.HasKey("GenericOrganizationId");

                            b1.ToTable("Organization");

                            b1.WithOwner()
                                .HasForeignKey("GenericOrganizationId");
                        });

                    b.OwnsOne("IDSubjects.BinaryDataInfo", "ProfilePicture", b1 =>
                        {
                            b1.Property<string>("GenericOrganizationId")
                                .HasColumnType("varchar(50)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasMaxLength(100)
                                .IsUnicode(false)
                                .HasColumnType("varchar(100)");

                            b1.Property<DateTimeOffset>("UpdateTime")
                                .HasColumnType("datetimeoffset");

                            b1.HasKey("GenericOrganizationId");

                            b1.ToTable("Organization");

                            b1.WithOwner()
                                .HasForeignKey("GenericOrganizationId");
                        });

                    b.OwnsMany("IDSubjects.OrganizationIdentifier", "Identifiers", b1 =>
                        {
                            b1.Property<string>("OrganizationId")
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("Type")
                                .HasColumnType("varchar(30)");

                            b1.Property<string>("Value")
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.HasKey("OrganizationId", "Type", "Value");

                            b1.ToTable("OrganizationIdentifier");

                            b1.WithOwner("Organization")
                                .HasForeignKey("OrganizationId");

                            b1.Navigation("Organization");
                        });

                    b.Navigation("Fapiao");

                    b.Navigation("Identifiers");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("IDSubjects.Invitations.JoinOrganizationInvitation", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Invitee")
                        .WithMany()
                        .HasForeignKey("InviteeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IDSubjects.GenericOrganization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invitee");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("IDSubjects.NaturalPerson", b =>
                {
                    b.OwnsOne("IDSubjects.AddressInfo", "Address", b1 =>
                        {
                            b1.Property<string>("NaturalPersonId")
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("Contact")
                                .HasMaxLength(20)
                                .IsUnicode(false)
                                .HasColumnType("varchar(20)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Locality")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("PostalCode")
                                .HasMaxLength(20)
                                .IsUnicode(false)
                                .HasColumnType("varchar(20)");

                            b1.Property<string>("Receiver")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Region")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Street1")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Street2")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Street3")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.HasKey("NaturalPersonId");

                            b1.ToTable("NaturalPerson");

                            b1.WithOwner()
                                .HasForeignKey("NaturalPersonId");
                        });

                    b.OwnsOne("IDSubjects.BinaryDataInfo", "ProfilePicture", b1 =>
                        {
                            b1.Property<string>("NaturalPersonId")
                                .HasColumnType("varchar(50)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasMaxLength(100)
                                .IsUnicode(false)
                                .HasColumnType("varchar(100)");

                            b1.Property<DateTimeOffset>("UpdateTime")
                                .HasColumnType("datetimeoffset");

                            b1.HasKey("NaturalPersonId");

                            b1.ToTable("NaturalPerson");

                            b1.WithOwner()
                                .HasForeignKey("NaturalPersonId");
                        });

                    b.OwnsOne("IDSubjects.PersonNameInfo", "PersonName", b1 =>
                        {
                            b1.Property<string>("NaturalPersonId")
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)");

                            b1.Property<string>("GivenName")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("MiddleName")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("SearchHint")
                                .HasMaxLength(60)
                                .HasColumnType("nvarchar(60)");

                            b1.Property<string>("Surname")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.HasKey("NaturalPersonId");

                            b1.HasIndex("FullName");

                            b1.HasIndex("SearchHint");

                            b1.ToTable("NaturalPerson");

                            b1.WithOwner()
                                .HasForeignKey("NaturalPersonId");
                        });

                    b.Navigation("Address");

                    b.Navigation("PersonName")
                        .IsRequired();

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("IDSubjects.OrganizationBankAccount", b =>
                {
                    b.HasOne("IDSubjects.GenericOrganization", "Organization")
                        .WithMany()
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

            modelBuilder.Entity("IDSubjects.Payments.PersonBankAccount", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.RealName.ChineseIdCardValidation", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("IDSubjects.ChineseName.ChinesePersonName", "ChinesePersonName", b1 =>
                        {
                            b1.Property<int>("ChineseIdCardValidationId")
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

                            b1.HasKey("ChineseIdCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIdCardValidationId");
                        });

                    b.OwnsOne("IDSubjects.RealName.ChineseIDCardImage", "ChineseIDCardImage", b1 =>
                        {
                            b1.Property<int>("ChineseIdCardValidationId")
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

                            b1.HasKey("ChineseIdCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIdCardValidationId");
                        });

                    b.OwnsOne("IDSubjects.RealName.ChineseIDCardInfo", "ChineseIDCard", b1 =>
                        {
                            b1.Property<int>("ChineseIdCardValidationId")
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

                            b1.Property<string>("Gender")
                                .IsRequired()
                                .HasColumnType("varchar(7)");

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

                            b1.HasKey("ChineseIdCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIdCardValidationId");
                        });

                    b.OwnsOne("IDSubjects.RealName.ValidationResult", "Result", b1 =>
                        {
                            b1.Property<int>("ChineseIdCardValidationId")
                                .HasColumnType("int");

                            b1.Property<bool>("Accepted")
                                .HasColumnType("bit");

                            b1.Property<DateTime>("ValidateTime")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Validator")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.HasKey("ChineseIdCardValidationId");

                            b1.ToTable("RealNameValidation");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIdCardValidationId");
                        });

                    b.Navigation("ChineseIDCard");

                    b.Navigation("ChineseIDCardImage");

                    b.Navigation("ChinesePersonName");

                    b.Navigation("Person");

                    b.Navigation("Result");
                });

            modelBuilder.Entity("IDSubjects.RealName.RealNameInfo", b =>
                {
                    b.HasOne("IDSubjects.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IDSubjects.GenericOrganization", b =>
                {
                    b.Navigation("UsedNames");
                });
#pragma warning restore 612, 618
        }
    }
}
