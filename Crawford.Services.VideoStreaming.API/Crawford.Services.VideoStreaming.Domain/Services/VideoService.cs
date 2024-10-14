using Crawford.Services.VideoStreaming.Domain.Dto.Request;
using Crawford.Services.VideoStreaming.Domain.Dto.Response;
using Crawford.Services.VideoStreaming.Domain.Entities;
using Crawford.Services.VideoStreaming.Domain.Interfaces;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Xabe.FFmpeg;

namespace Crawford.Services.VideoStreaming.Domain.Services
{
    public class VideoService : IVideoService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IVideoRepository _videoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<VideoService> _logger;

        public VideoService(
            IFileStorageService fileStorageService,
            IVideoRepository videoRepository,
            ICategoryRepository categoryRepository,
            ILogger<VideoService> logger)
        {
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
            _videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<VideoResponse>> GetVideoList()
        {
            var list = await _videoRepository.GetVideosAsync();
            _logger.LogInformation("Retrieved {Count} videos from the repository.", list.Count());

            return list.Select(x => new VideoResponse
            {
                ID = x.VideoID,
                Title = x.Title,
                Description = x.Description,
                Categories = x.Category.Name,
                ThumbnailPath = x.ThumbnailPath,
                FilePath = x.FilePath,
                UploadedAt = x.CreatedDateTime
            }).ToList();
        }

        public async Task<VideoResponse> GetVideoByID(Guid id)
        {
            var video = await _videoRepository.FindByIdAsync(id);

            if (video == null)
            {
                _logger.LogWarning("Video with ID {Id} not found.", id);
                throw new KeyNotFoundException("No Video Data Found.");
            }

            return new VideoResponse
            {
                ID = video.VideoID,
                Title = video.Title,
                Description = video.Description,
                Categories = video.Category.Name,
                ThumbnailPath = video.ThumbnailPath,
                FilePath = video.FilePath,
                UploadedAt = video.CreatedDateTime
            };
        }

        public async Task<(bool Success, string Message)> UploadVideoAsync(VideoUploadDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            _logger.LogInformation("Starting video upload process.");

            Guid videoID = Guid.NewGuid();

            // Upload video to cloud storage
            var videoUrl = await _fileStorageService.UploadFileAsync(dto.File);
            if (string.IsNullOrEmpty(videoUrl))
            {
                _logger.LogError("File upload failed.");
                return (false, "File upload failed.");
            }

            // Generate thumbnail from the video
            var thumbnailPath = await GenerateThumbnail(dto.File, videoID.ToString());

            // Get CategoryID or create new category
            Guid categoryID = await GetOrCreateCategory(dto.Categories);

            // Save video to the database
            var video = new Video
            {
                VideoID = videoID,
                Title = dto.Title,
                Description = dto.Description,
                CategoryID = categoryID,
                FilePath = videoUrl,
                ThumbnailPath = thumbnailPath,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow
            };

            try
            {
                await _videoRepository.AddAsync(video);
                await _videoRepository.SaveChangesAsync();
                _logger.LogInformation("Video {Title} uploaded successfully.", dto.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving video to the database.");
                return (false, "An error occurred while saving video data.");
            }

            return (true, "Video uploaded successfully.");
        }

        public bool ValidateFile(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var allowedExtensions = new[] { ".mp4", ".avi", ".mov" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            bool isValid = allowedExtensions.Contains(extension) && file.Length <= 104857600; // Max 100MB
            _logger.LogInformation("File validation result: {IsValid}", isValid);
            return isValid;
        }

        private async Task<string> GenerateThumbnail(IFormFile videoFile, string videoID)
        {
            _logger.LogInformation("Generating thumbnail for video {VideoID}.", videoID);

            string thumbnailName = $"{videoID}.jpg";
            string tempVideoPath = Path.Combine(Path.GetTempPath(), $"{videoFile.FileName}");
            string thumbnailPath = Path.Combine(Path.GetTempPath(), thumbnailName);

            // Save the uploaded video to a temporary location
            await using (var fileStream = new FileStream(tempVideoPath, FileMode.Create))
            {
                await videoFile.CopyToAsync(fileStream);
            }

            // Generate thumbnail (2 seconds into the video)
            using (var engine = new Engine())
            {
                var inputFile = new MediaFile { Filename = tempVideoPath };
                var outputFile = new MediaFile { Filename = thumbnailPath };

                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(2) };
                engine.GetThumbnail(inputFile, outputFile, options);
            }

            // Upload thumbnail to cloud storage
            var uploadedThumbnailPath = await _fileStorageService.UploadThumbnail(thumbnailPath, thumbnailName);

            // Clean up temporary files
            DeleteFiles(tempVideoPath, thumbnailPath);

            _logger.LogInformation("Thumbnail generated and uploaded for video {VideoID}.", videoID);
            return uploadedThumbnailPath;
        }

        private async Task<Guid> GetOrCreateCategory(string categoryName)
        {
            _logger.LogInformation("Checking category existence for {CategoryName}.", categoryName);

            var category = await _categoryRepository.FindByNameAsync(categoryName);
            if (category != null) return category.CategoryID;

            var newCategory = new Category
            {
                CategoryID = Guid.NewGuid(),
                Name = categoryName
            };

            await _categoryRepository.AddAsync(newCategory);
            await _categoryRepository.SaveChangesAsync();

            _logger.LogInformation("Category {CategoryName} created.", categoryName);
            return newCategory.CategoryID;
        }

        private void DeleteFiles(params string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Deleted file {FilePath}.", filePath);
                }
            }
        }
    }
}
