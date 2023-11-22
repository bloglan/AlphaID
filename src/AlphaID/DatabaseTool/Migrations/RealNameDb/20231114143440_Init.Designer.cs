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
    [Migration("20231114143440_Init")]
    partial class Init
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

            modelBuilder.Entity("IdSubjects.RealName.IdentityDocument", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("WhenCreated")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("IdentityDocument");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityDocument");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("IdSubjects.RealName.RealNameState", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("AcceptedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("AcceptedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("RealNameState");
                });

            modelBuilder.Entity("IdSubjects.RealName.RealNameValidation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DocumentId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RealNameStateId")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("RealNameStateId");

                    b.ToTable("RealNameValidation");
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

            modelBuilder.Entity("IdSubjects.RealName.IdentityDocumentAttachment", b =>
                {
                    b.HasOne("IdSubjects.RealName.IdentityDocument", "Document")
                        .WithMany("Attachments")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("IdSubjects.RealName.RealNameValidation", b =>
                {
                    b.HasOne("IdSubjects.RealName.IdentityDocument", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IdSubjects.RealName.RealNameState", null)
                        .WithMany("Validations")
                        .HasForeignKey("RealNameStateId");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("IdSubjects.RealName.IdentityDocument", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("IdSubjects.RealName.RealNameState", b =>
                {
                    b.Navigation("Validations");
                });
#pragma warning restore 612, 618
        }
    }
}
