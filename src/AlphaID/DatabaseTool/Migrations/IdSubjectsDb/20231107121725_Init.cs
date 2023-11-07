﻿using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DatabaseTool.Migrations.IdSubjectsDb
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaturalPerson",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    SecurityStamp = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhenCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenChanged = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneticSurname = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PhoneticGivenName = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    PhoneticSearchHint = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    Sex = table.Column<string>(type: "varchar(6)", nullable: true, comment: "性别"),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    PasswordLastSet = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Avatar_MimeType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Avatar_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Locale = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TimeZone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Address_Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Street1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Street2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Street3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Receiver = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_Contact = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Address_PostalCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalPerson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Usci = table.Column<string>(type: "char(18)", maxLength: 18, nullable: true),
                    WhenCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenChanged = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Domicile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EstablishedAt = table.Column<DateTime>(type: "date", nullable: true),
                    TermBegin = table.Column<DateTime>(type: "date", nullable: true),
                    TermEnd = table.Column<DateTime>(type: "date", nullable: true),
                    Representative = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Location = table.Column<Geometry>(type: "geography", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NaturalPersonClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    ClaimValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalPersonClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaturalPersonClaim_NaturalPerson_UserId",
                        column: x => x.UserId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaturalPersonLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalPersonLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_NaturalPersonLogin_NaturalPerson_UserId",
                        column: x => x.UserId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaturalPersonToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalPersonToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_NaturalPersonToken_NaturalPerson_UserId",
                        column: x => x.UserId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonBankAccount",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PersonId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBankAccount", x => new { x.AccountNumber, x.PersonId });
                    table.ForeignKey(
                        name: "FK_PersonBankAccount_NaturalPerson_PersonId",
                        column: x => x.PersonId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealNameInfo",
                columns: table => new
                {
                    PersonId = table.Column<string>(type: "varchar(50)", nullable: false),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AcceptedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealNameInfo", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_RealNameInfo_NaturalPerson_PersonId",
                        column: x => x.PersonId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealNameValidation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ChineseIDCard_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChineseIDCard_Sex = table.Column<string>(type: "varchar(7)", nullable: true),
                    ChineseIDCard_Ethnicity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChineseIDCard_DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    ChineseIDCard_Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChineseIDCard_CardNumber = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: true),
                    ChineseIDCard_Issuer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChineseIDCard_IssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    ChineseIDCard_Expires = table.Column<DateTime>(type: "date", nullable: true),
                    ChineseIDCardImage_PersonalFace = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ChineseIDCardImage_PersonalFaceMimeType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ChineseIDCardImage_IssuerFace = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ChineseIDCardImage_IssuerFaceMimeType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ChinesePersonName_Surname = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ChinesePersonName_GivenName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ChinesePersonName_PhoneticSurname = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    ChinesePersonName_PhoneticGivenName = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    CommitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result_Validator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Result_ValidateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result_Accepted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealNameValidation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealNameValidation_NaturalPerson_PersonId",
                        column: x => x.PersonId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinOrganizationInvitation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InviteeId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OrganizationId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    WhenCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenExpired = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Inviter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpectVisibility = table.Column<int>(type: "int", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinOrganizationInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinOrganizationInvitation_NaturalPerson_InviteeId",
                        column: x => x.InviteeId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinOrganizationInvitation_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationBankAccount",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    OrganizationId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationBankAccount", x => new { x.AccountNumber, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_OrganizationBankAccount_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationMember",
                columns: table => new
                {
                    OrganizationId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PersonId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationMember", x => new { x.PersonId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_OrganizationMember_NaturalPerson_PersonId",
                        column: x => x.PersonId,
                        principalTable: "NaturalPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationMember_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUsedName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeprecateTime = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUsedName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationUsedName_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoinOrganizationInvitation_InviteeId",
                table: "JoinOrganizationInvitation",
                column: "InviteeId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinOrganizationInvitation_OrganizationId",
                table: "JoinOrganizationInvitation",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_Email",
                table: "NaturalPerson",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_Name",
                table: "NaturalPerson",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_NormalizedEmail",
                table: "NaturalPerson",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_NormalizedUserName",
                table: "NaturalPerson",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_PhoneNumber",
                table: "NaturalPerson",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_PhoneticSearchHint",
                table: "NaturalPerson",
                column: "PhoneticSearchHint");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_UserName",
                table: "NaturalPerson",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_WhenChanged",
                table: "NaturalPerson",
                column: "WhenChanged");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPerson_WhenCreated",
                table: "NaturalPerson",
                column: "WhenCreated");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPersonClaim_UserId",
                table: "NaturalPersonClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalPersonLogin_UserId",
                table: "NaturalPersonLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Name",
                table: "Organization",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Usci",
                table: "Organization",
                column: "Usci",
                unique: true,
                filter: "[USCI] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_WhenChanged",
                table: "Organization",
                column: "WhenChanged");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_WhenCreated",
                table: "Organization",
                column: "WhenCreated");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationBankAccount_OrganizationId",
                table: "OrganizationBankAccount",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_OrganizationId",
                table: "OrganizationMember",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUsedName_OrganizationId",
                table: "OrganizationUsedName",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonBankAccount_PersonId",
                table: "PersonBankAccount",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RealNameValidation_PersonId",
                table: "RealNameValidation",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoinOrganizationInvitation");

            migrationBuilder.DropTable(
                name: "NaturalPersonClaim");

            migrationBuilder.DropTable(
                name: "NaturalPersonLogin");

            migrationBuilder.DropTable(
                name: "NaturalPersonToken");

            migrationBuilder.DropTable(
                name: "OrganizationBankAccount");

            migrationBuilder.DropTable(
                name: "OrganizationMember");

            migrationBuilder.DropTable(
                name: "OrganizationUsedName");

            migrationBuilder.DropTable(
                name: "PersonBankAccount");

            migrationBuilder.DropTable(
                name: "RealNameInfo");

            migrationBuilder.DropTable(
                name: "RealNameValidation");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "NaturalPerson");
        }
    }
}
