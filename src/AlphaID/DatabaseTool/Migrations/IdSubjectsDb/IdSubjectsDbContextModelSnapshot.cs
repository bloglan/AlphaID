﻿// <auto-generated />
using System;
using AlphaId.EntityFramework.IdSubjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace DatabaseTool.Migrations.IdSubjectsDb
{
    [DbContext(typeof(IdSubjectsDbContext))]
    partial class IdSubjectsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AlphaId.EntityFramework.IdSubjects.PasswordHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("WhenCreated")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("WhenCreated");

                    b.ToTable("PasswordHistory");
                });

            modelBuilder.Entity("AlphaIdPlatform.Identity.NaturalPerson", b =>
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

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("FamilyName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .HasColumnType("varchar(6)")
                        .HasComment("性别");

                    b.Property<string>("GivenName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Locale")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NickName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTimeOffset?>("PasswordLastSet")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("PersonWhenChanged")
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

                    b.Property<string>("SearchHint")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

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

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("WhenChanged");

                    b.HasIndex("WhenCreated");

                    b.ToTable("ApplicationUser", (string)null);
                });

            modelBuilder.Entity("AlphaIdPlatform.Identity.OrganizationMember", b =>
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

            modelBuilder.Entity("AlphaIdPlatform.Invitations.JoinOrganizationInvitation", b =>
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
                        .HasColumnType("nvarchar(50)");

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

                    b.ToTable("JoinOrganizationInvitation");
                });

            modelBuilder.Entity("AlphaIdPlatform.Subjects.Organization", b =>
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

                    b.Property<DateOnly?>("EstablishedAt")
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

                    b.Property<DateOnly?>("TermBegin")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("TermEnd")
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

            modelBuilder.Entity("AlphaIdPlatform.Subjects.OrganizationUsedName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DeprecateTime")
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationUserClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

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

                    b.ToTable("ApplicationUserLogin", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RoleId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationUserInRole", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
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

                    b.ToTable("ApplicationUserToken", (string)null);
                });

            modelBuilder.Entity("AlphaIdPlatform.Identity.NaturalPerson", b =>
                {
                    b.OwnsOne("IdSubjects.BinaryDataInfo", "ProfilePicture", b1 =>
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

                            b1.HasKey("NaturalPersonId");

                            b1.ToTable("ApplicationUser");

                            b1.WithOwner()
                                .HasForeignKey("NaturalPersonId");
                        });

                    b.OwnsMany("AlphaIdPlatform.Identity.NaturalPersonBankAccount", "BankAccounts", b1 =>
                        {
                            b1.Property<string>("AccountNumber")
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("AccountName")
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("BankName")
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("NaturalPersonId")
                                .IsRequired()
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("PersonId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.HasKey("AccountNumber");

                            b1.HasIndex("NaturalPersonId");

                            b1.ToTable("NaturalPersonBankAccount");

                            b1.WithOwner()
                                .HasForeignKey("NaturalPersonId");
                        });

                    b.OwnsOne("IdSubjects.AddressInfo", "Address", b1 =>
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

                            b1.ToTable("ApplicationUser");

                            b1.WithOwner()
                                .HasForeignKey("NaturalPersonId");
                        });

                    b.Navigation("Address");

                    b.Navigation("BankAccounts");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("AlphaIdPlatform.Identity.OrganizationMember", b =>
                {
                    b.HasOne("AlphaIdPlatform.Subjects.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlphaIdPlatform.Identity.NaturalPerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AlphaIdPlatform.Subjects.Organization", b =>
                {
                    b.OwnsOne("AlphaIdPlatform.Subjects.FapiaoInfo", "Fapiao", b1 =>
                        {
                            b1.Property<string>("OrganizationId")
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

                            b1.HasKey("OrganizationId");

                            b1.ToTable("Organization");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.OwnsOne("IdSubjects.BinaryDataInfo", "ProfilePicture", b1 =>
                        {
                            b1.Property<string>("OrganizationId")
                                .HasColumnType("varchar(50)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasMaxLength(100)
                                .IsUnicode(false)
                                .HasColumnType("varchar(100)");

                            b1.HasKey("OrganizationId");

                            b1.ToTable("Organization");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.OwnsMany("AlphaIdPlatform.Subjects.OrganizationBankAccount", "BankAccounts", b1 =>
                        {
                            b1.Property<string>("AccountNumber")
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("AccountName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("BankName")
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<bool>("Default")
                                .HasColumnType("bit");

                            b1.Property<string>("OrganizationId")
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("Usage")
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)");

                            b1.HasKey("AccountNumber");

                            b1.HasIndex("OrganizationId");

                            b1.ToTable("OrganizationBankAccount");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.OwnsMany("AlphaIdPlatform.Subjects.OrganizationIdentifier", "OrganizationIdentifiers", b1 =>
                        {
                            b1.Property<string>("Value")
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Type")
                                .HasColumnType("varchar(30)");

                            b1.Property<string>("OrganizationId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .IsUnicode(false)
                                .HasColumnType("varchar(50)");

                            b1.HasKey("Value", "Type");

                            b1.HasIndex("OrganizationId");

                            b1.ToTable("OrganizationIdentifier");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.Navigation("BankAccounts");

                    b.Navigation("Fapiao");

                    b.Navigation("OrganizationIdentifiers");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("AlphaIdPlatform.Subjects.OrganizationUsedName", b =>
                {
                    b.HasOne("AlphaIdPlatform.Subjects.Organization", "Organization")
                        .WithMany("UsedNames")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AlphaIdPlatform.Identity.NaturalPerson", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AlphaIdPlatform.Identity.NaturalPerson", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlphaIdPlatform.Identity.NaturalPerson", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AlphaIdPlatform.Identity.NaturalPerson", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AlphaIdPlatform.Subjects.Organization", b =>
                {
                    b.Navigation("UsedNames");
                });
#pragma warning restore 612, 618
        }
    }
}
