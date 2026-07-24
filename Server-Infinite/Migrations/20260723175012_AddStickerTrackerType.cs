using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerInfinite.Migrations
{
    /// <inheritdoc />
    public partial class AddStickerTrackerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "Tracker1Type",
                table: "exvs2ob_ms_sticker",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "Tracker2Type",
                table: "exvs2ob_ms_sticker",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "Tracker3Type",
                table: "exvs2ob_ms_sticker",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "Tracker1Type",
                table: "exvs2ob_default_sticker_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "Tracker2Type",
                table: "exvs2ob_default_sticker_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "Tracker3Type",
                table: "exvs2ob_default_sticker_profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tracker1Type",
                table: "exvs2ob_ms_sticker");

            migrationBuilder.DropColumn(
                name: "Tracker2Type",
                table: "exvs2ob_ms_sticker");

            migrationBuilder.DropColumn(
                name: "Tracker3Type",
                table: "exvs2ob_ms_sticker");

            migrationBuilder.DropColumn(
                name: "Tracker1Type",
                table: "exvs2ob_default_sticker_profile");

            migrationBuilder.DropColumn(
                name: "Tracker2Type",
                table: "exvs2ob_default_sticker_profile");

            migrationBuilder.DropColumn(
                name: "Tracker3Type",
                table: "exvs2ob_default_sticker_profile");
        }
    }
}
