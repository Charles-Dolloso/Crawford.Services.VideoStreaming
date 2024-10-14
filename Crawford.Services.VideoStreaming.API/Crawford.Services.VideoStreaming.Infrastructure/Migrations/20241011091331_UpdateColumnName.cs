using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crawford.Services.VideoStreaming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Thumbnail",
                table: "Video",
                newName: "ThumbnailPath");

            migrationBuilder.RenameColumn(
                name: "PathUrl",
                table: "Video",
                newName: "FilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailPath",
                table: "Video",
                newName: "Thumbnail");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Video",
                newName: "PathUrl");
        }
    }
}
