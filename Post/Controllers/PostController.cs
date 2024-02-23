using Microsoft.AspNetCore.Mvc;
using Post.Services;

namespace Post.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostDataService _postDataService;

        public PostController(PostDataService postDataService)
        {
            //Instance of postDataservcie
            _postDataService = postDataService;
        }

        [HttpGet]
        async public Task<IActionResult> GetPost(string tag, string? direction, string? sortBy)
        {
            //Validate direction
            if (direction != null && direction != "asc" && direction != "desc")
            {
                return BadRequest("direction parameter is invalid");
            }

            //Validate orderBY
            if (sortBy != null && sortBy != "id" && sortBy != "reads" && sortBy != "likes" && sortBy != "popularity")
            {
                return BadRequest("sortBy parameter is invalid");
            }

            try
            {
                var postData = await _postDataService.GetPostDataAsync(tag, direction, sortBy);
                return Ok(postData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


    }
}
