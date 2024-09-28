using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeklyReminder.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailSendingFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReminderEnabled",
                table: "Schedules",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReminderEnabled",
                table: "Schedules");
        }
    }
}
