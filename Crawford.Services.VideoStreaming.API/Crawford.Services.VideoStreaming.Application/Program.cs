using Azure.Storage.Blobs;
using Crawford.Services.VideoStreaming.Domain.Interfaces;
using Crawford.Services.VideoStreaming.Domain.Services;
using Crawford.Services.VideoStreaming.Infrastructure.Context;
using Crawford.Services.VideoStreaming.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Crawford.Services.VideoStreaming.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Video API",
                    Version = "v1",
                    Description = "An API for uploading, streaming, and listing videos.",
                });
            });

            #region Services and Repositories
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<IFileStorageService, FileStorageService>();
            // Register BlobServiceClient using the connection string from configuration
            builder.Services.AddSingleton(x =>
                new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));

            builder.Services.AddScoped<IVideoRepository, VideoRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<CrawfordDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
            #endregion

            var app = builder.Build();

            #region Database Migration using Code First
            using (var Scope = app.Services.CreateScope())
            {
                var context = Scope.ServiceProvider.GetRequiredService<CrawfordDbContext>();
                context.Database.Migrate();
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Video API v1");
                });
            }


            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}