using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Crawford.Services.VideoStreaming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryID", "Name" },
                values: new object[] { new Guid("183df134-28ed-4928-a816-402b6019ab90"), "Sample" });

            migrationBuilder.InsertData(
                table: "Video",
                columns: new[] { "VideoID", "CategoryID", "CreatedDateTime", "Description", "FilePath", "ModifiedDateTime", "ThumbnailPath", "Title" },
                values: new object[,]
                {
                    { new Guid("1d9c377f-1b1b-4376-926c-21b71068b923"), new Guid("183df134-28ed-4928-a816-402b6019ab90"), new DateTime(2024, 10, 14, 13, 56, 49, 435, DateTimeKind.Local).AddTicks(5819), "Sample Video Only", "https://crawfordblobcontainer.blob.core.windows.net/videos/videoplayback %281%29.mp4", new DateTime(2024, 10, 14, 13, 56, 49, 435, DateTimeKind.Local).AddTicks(5820), "https://crawfordblobcontainer.blob.core.windows.net/videos/41a57cba-0bc8-477c-8ecd-ed9693804048.jpg", "Sample Video 2" },
                    { new Guid("664dc446-1688-434f-be27-58070d6dcf11"), new Guid("183df134-28ed-4928-a816-402b6019ab90"), new DateTime(2024, 10, 14, 13, 56, 49, 435, DateTimeKind.Local).AddTicks(5781), "Sample Video Only", "https://crawfordblobcontainer.blob.core.windows.net/videos/videoplayback.mp4", new DateTime(2024, 10, 14, 13, 56, 49, 435, DateTimeKind.Local).AddTicks(5799), "https://crawfordblobcontainer.blob.core.windows.net/videos/df2a3c37-ff4c-452c-89fc-f4cde425f1c8.jpg", "Sample Video 1" },
                    { new Guid("d6353fa5-9a5c-4682-8df5-846b54e791b3"), new Guid("183df134-28ed-4928-a816-402b6019ab90"), new DateTime(2024, 10, 14, 13, 56, 49, 435, DateTimeKind.Local).AddTicks(5835), "Sample Video Only", "https://crawfordblobcontainer.blob.core.windows.net/videos/videoplayback %282%29.mp4", new DateTime(2024, 10, 14, 13, 56, 49, 435, DateTimeKind.Local).AddTicks(5835), "https://crawfordblobcontainer.blob.core.windows.net/videos/0d319bc7-bdbd-4655-901b-a38f58ad833e.jpg", "Sample Video 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Video",
                keyColumn: "VideoID",
                keyValue: new Guid("1d9c377f-1b1b-4376-926c-21b71068b923"));

            migrationBuilder.DeleteData(
                table: "Video",
                keyColumn: "VideoID",
                keyValue: new Guid("664dc446-1688-434f-be27-58070d6dcf11"));

            migrationBuilder.DeleteData(
                table: "Video",
                keyColumn: "VideoID",
                keyValue: new Guid("d6353fa5-9a5c-4682-8df5-846b54e791b3"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: new Guid("183df134-28ed-4928-a816-402b6019ab90"));
        }
    }
}
