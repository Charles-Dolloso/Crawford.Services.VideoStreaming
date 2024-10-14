using Crawford.Services.VideoStreaming.Domain.Dto.Request;
using Crawford.Services.VideoStreaming.Domain.Dto.Response;
using Crawford.Services.VideoStreaming.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Crawford.Services.VideoStreaming.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IVideoService _service;

        public VideoController(ILogger<VideoController> logger, IVideoService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Retrieves a list of all uploaded videos.
        /// </summary>
        /// <returns>List of videos.</returns>
        /// <response code="200">Successfully retrieved the list of videos.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of all videos.", Description = "Retrieves a list of all uploaded videos.")]
        [ProducesResponseType(typeof(IEnumerable<VideoResponse>), 200)]
        public async Task<IActionResult> GetVideos()
        {
            _logger.LogInformation("Retrieving list of all videos.");

            var videos = await _service.GetVideoList();
            return Ok(videos);
        }

        /// <summary>
        /// Streams a video by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the video.</param>
        /// <returns>Video file path or error message.</returns>
        /// <response code="200">Successfully retrieved the video file.</response>
        /// <response code="404">Video not found.</response>
        /// <response code="400">Error occurred while streaming the video.</response>
        [HttpGet("stream/{id}")]
        [SwaggerOperation(Summary = "Streams a video by its ID.", Description = "Streams a video by its unique identifier.")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> StreamVideo(Guid id)
        {
            _logger.LogInformation("Streaming video with ID: {Id}", id);

            var video = await _service.GetVideoByID(id);
            if (video == null)
            {
                _logger.LogWarning("Video not found with ID: {Id}", id);
                return NotFound("Video not found.");
            }

            try
            {
                _logger.LogInformation("Returning video file path for ID: {Id}", id);
                return Ok(video.FilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while streaming video with ID: {Id}", id);
                return BadRequest("An error occurred while streaming the video.");
            }
        }

        /// <summary>
        /// Uploads a video file and returns its metadata.
        /// </summary>
        /// <param name="dto">The video file and metadata details.</param>
        /// <returns>Success or error message with upload result.</returns>
        /// <response code="200">Video successfully uploaded.</response>
        /// <response code="400">Invalid video file or missing file.</response>
        /// <response code="500">Error occurred while uploading the video.</response>
        [HttpPost("upload")]
        [SwaggerOperation(Summary = "Uploads a video file.", Description = "Uploads a video file and returns its metadata.")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UploadVideo([FromForm] VideoUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                _logger.LogWarning("No file provided for upload.");
                return BadRequest("No file provided.");
            }

            // Validate file size and type
            if (!_service.ValidateFile(dto.File))
            {
                _logger.LogWarning("Invalid file provided. Only MP4, AVI, and MOV under 100MB are allowed.");
                return BadRequest("Invalid file. Only MP4, AVI, and MOV files under 100MB are allowed.");
            }

            // Attempt to upload video and generate metadata
            var result = await _service.UploadVideoAsync(dto);
            if (!result.Success)
            {
                _logger.LogError("Video upload failed.");
                return StatusCode(500, "Video upload failed.");
            }

            _logger.LogInformation("Video uploaded successfully. Message: {Message}", result.Message);
            return Ok(result.Message);
        }
    }
}