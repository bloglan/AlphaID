﻿// <auto-generated />
using System;
using AlphaId.RealName.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatabaseTool.Migrations.RealNameDb
{
    [DbContext(typeof(RealNameDbContext))]
    [Migration("20231129145329_RenameRemarkColumn")]
    partial class RenameRemarkColumn
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

            modelBuilder.Entity("IdSubjects.RealName.IdentityDocument", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTimeOffset>("WhenCreated")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("IdentityDocument");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityDocument");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("IdSubjects.RealName.IdentityDocumentAttachment", b =>
                {
                    b.Property<string>("DocumentId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("DocumentId", "Name");

                    b.ToTable("IdentityDocumentAttachment");
                });

            modelBuilder.Entity("IdSubjects.RealName.RealNameAuthentication", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("Applied")
                        .HasColumnType("bit");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTimeOffset?>("ExpiresAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PersonId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Remarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset>("ValidatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ValidatedBy")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("RealNameAuthentication");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RealNameAuthentication");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("IdSubjects.RealName.Requesting.RealNameRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Accepted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("AuditTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Auditor")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PersonId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("WhenCommitted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("RealNameRequest");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RealNameRequest");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("IdSubjects.RealName.ChineseIdCardDocument", b =>
                {
                    b.HasBaseType("IdSubjects.RealName.IdentityDocument");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(18)
                        .IsUnicode(false)
                        .HasColumnType("varchar(18)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Ethnicity")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("Expires")
                        .HasColumnType("date");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("date");

                    b.Property<string>("Issuer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("varchar(7)");

                    b.ToTable("IdentityDocument");

                    b.HasDiscriminator().HasValue("ChineseIdCardDocument");
                });

            modelBuilder.Entity("IdSubjects.RealName.DocumentedRealNameAuthentication", b =>
                {
                    b.HasBaseType("IdSubjects.RealName.RealNameAuthentication");

                    b.Property<string>("DocumentId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasIndex("DocumentId");

                    b.ToTable("RealNameAuthentication");

                    b.HasDiscriminator().HasValue("DocumentedRealNameAuthentication");
                });

            modelBuilder.Entity("IdSubjects.RealName.Requesting.ChineseIdCardRealNameRequest", b =>
                {
                    b.HasBaseType("IdSubjects.RealName.Requesting.RealNameRequest");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(18)
                        .IsUnicode(false)
                        .HasColumnType("varchar(18)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Ethnicity")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("Expires")
                        .HasColumnType("date");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("date");

                    b.Property<string>("Issuer")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.ToTable("RealNameRequest");

                    b.HasDiscriminator().HasValue("ChineseIdCardRealNameRequest");
                });

            modelBuilder.Entity("IdSubjects.RealName.IdentityDocumentAttachment", b =>
                {
                    b.HasOne("IdSubjects.RealName.IdentityDocument", "Document")
                        .WithMany("Attachments")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("IdSubjects.RealName.RealNameAuthentication", b =>
                {
                    b.OwnsOne("IdSubjects.PersonNameInfo", "PersonName", b1 =>
                        {
                            b1.Property<string>("RealNameAuthenticationId")
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

                            b1.HasKey("RealNameAuthenticationId");

                            b1.HasIndex("FullName");

                            b1.HasIndex("SearchHint");

                            b1.ToTable("RealNameAuthentication");

                            b1.WithOwner()
                                .HasForeignKey("RealNameAuthenticationId");
                        });

                    b.Navigation("PersonName")
                        .IsRequired();
                });

            modelBuilder.Entity("IdSubjects.RealName.DocumentedRealNameAuthentication", b =>
                {
                    b.HasOne("IdSubjects.RealName.IdentityDocument", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("IdSubjects.RealName.Requesting.ChineseIdCardRealNameRequest", b =>
                {
                    b.OwnsOne("IdSubjects.BinaryDataInfo", "IssuerSide", b1 =>
                        {
                            b1.Property<int>("ChineseIdCardRealNameRequestId")
                                .HasColumnType("int");

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

                            b1.HasKey("ChineseIdCardRealNameRequestId");

                            b1.ToTable("RealNameRequest");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIdCardRealNameRequestId");
                        });

                    b.OwnsOne("IdSubjects.BinaryDataInfo", "PersonalSide", b1 =>
                        {
                            b1.Property<int>("ChineseIdCardRealNameRequestId")
                                .HasColumnType("int");

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

                            b1.HasKey("ChineseIdCardRealNameRequestId");

                            b1.ToTable("RealNameRequest");

                            b1.WithOwner()
                                .HasForeignKey("ChineseIdCardRealNameRequestId");
                        });

                    b.Navigation("IssuerSide")
                        .IsRequired();

                    b.Navigation("PersonalSide")
                        .IsRequired();
                });

            modelBuilder.Entity("IdSubjects.RealName.IdentityDocument", b =>
                {
                    b.Navigation("Attachments");
                });
#pragma warning restore 612, 618
        }
    }
}
