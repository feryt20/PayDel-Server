using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayDel.Data.Migrations.DbCon
{
    public partial class gateinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EasyPays",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WalletGateId = table.Column<string>(nullable: false),
                    IsWallet = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Price = table.Column<int>(maxLength: 15, nullable: false),
                    Text = table.Column<string>(maxLength: 250, nullable: false),
                    IsCoupon = table.Column<bool>(nullable: false),
                    IsUserEmail = table.Column<bool>(nullable: false),
                    IsUserName = table.Column<bool>(nullable: false),
                    IsUserPhone = table.Column<bool>(nullable: false),
                    IsUserText = table.Column<bool>(nullable: false),
                    IsUserEmailRequired = table.Column<bool>(nullable: false),
                    IsUserNameRequired = table.Column<bool>(nullable: false),
                    IsUserPhoneRequired = table.Column<bool>(nullable: false),
                    IsUserTextRequired = table.Column<bool>(nullable: false),
                    UserEmailExplain = table.Column<string>(maxLength: 25, nullable: false),
                    UserNameExplain = table.Column<string>(maxLength: 25, nullable: false),
                    UserPhoneExplain = table.Column<string>(maxLength: 25, nullable: false),
                    UserTextExplain = table.Column<string>(maxLength: 25, nullable: false),
                    IsCountLimit = table.Column<bool>(nullable: false),
                    CountLimit = table.Column<int>(nullable: false),
                    ReturnSuccess = table.Column<string>(nullable: true),
                    ReturnFail = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyPays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EasyPays_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<long>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false),
                    IsSms = table.Column<bool>(nullable: false),
                    IsBlock = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Inventory = table.Column<int>(nullable: false),
                    InterMoney = table.Column<int>(nullable: false),
                    ExitMoney = table.Column<int>(nullable: false),
                    OnExitMoney = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gates",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDirect = table.Column<bool>(nullable: false),
                    Ip = table.Column<string>(maxLength: 500, nullable: true),
                    IsIp = table.Column<bool>(nullable: false),
                    WebsiteName = table.Column<string>(maxLength: 100, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    Grouping = table.Column<string>(maxLength: 50, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1000, nullable: false),
                    WalletId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gates_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EasyPays_UserId",
                table: "EasyPays",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Gates_WalletId",
                table: "Gates",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EasyPays");

            migrationBuilder.DropTable(
                name: "Gates");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
