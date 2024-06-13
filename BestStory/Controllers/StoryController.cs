using BestStory.Models;
using Microsoft.AspNetCore.Mvc;

namespace BestStory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public StoryController(ILogger<WeatherForecastController> logger)
        {
            this._logger = logger;
        }
        [HttpGet]
        [ProducesResponseType<List<Story>>(StatusCodes.Status200OK)]
        public IActionResult GetStories()
        {
            return Ok(new List<Story>());
        }
    }
}
