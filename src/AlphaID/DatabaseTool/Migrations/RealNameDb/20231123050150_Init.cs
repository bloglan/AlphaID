﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseTool.Migrations.RealNameDb
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityDocument",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    WhenCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Discriminator = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sex = table.Column<string>(type: "varchar(7)", nullable: true),
                    Ethnicity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CardNumber = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    Expires = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealNameRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    WhenCommitted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: true),
                    Auditor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Discriminator = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PersonalSide_MimeType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PersonalSide_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PersonalSide_UpdateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IssuerSide_MimeType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IssuerSide_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IssuerSide_UpdateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Expires = table.Column<DateTime>(type: "date", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ethnicity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealNameRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityDocumentAttachment",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityDocumentAttachment", x => new { x.DocumentId, x.Name });
                    table.ForeignKey(
                        name: "FK_IdentityDocumentAttachment_IdentityDocument_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "IdentityDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealNameAuthentication",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PersonId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PersonName_Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PersonName_MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PersonName_GivenName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PersonName_FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PersonName_SearchHint = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ValidatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ValidatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Applied = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    DocumentId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealNameAuthentication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealNameAuthentication_IdentityDocument_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "IdentityDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealNameAuthentication_DocumentId",
                table: "RealNameAuthentication",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_RealNameAuthentication_PersonName_FullName",
                table: "RealNameAuthentication",
                column: "PersonName_FullName");

            migrationBuilder.CreateIndex(
                name: "IX_RealNameAuthentication_PersonName_SearchHint",
                table: "RealNameAuthentication",
                column: "PersonName_SearchHint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityDocumentAttachment");

            migrationBuilder.DropTable(
                name: "RealNameAuthentication");

            migrationBuilder.DropTable(
                name: "RealNameRequest");

            migrationBuilder.DropTable(
                name: "IdentityDocument");
        }
    }
}
