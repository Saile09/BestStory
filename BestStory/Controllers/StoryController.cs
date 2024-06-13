using BestStory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BestStory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly ILogger<StoryController> _logger;

        public StoryController(ILogger<StoryController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType<List<Story>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStories(int quantity, bool isOrderingDesc = false)
        {
            _logger.LogInformation("GetStories start");

            if (quantity == 0)
                return BadRequest("quantity must be greater than 0");

            var storyIds = await GetStoryIds();
            var stories = new List<Story>();

            if (storyIds.Count == 0)
                return BadRequest("There are not stories ids");

            if (storyIds.Count < quantity)
                return BadRequest("quantity must not be greater than count ids");

            foreach (var id in storyIds.Take(quantity))
            {
                var story = await GetStoryDetail(id);
                stories.Add(story);
            }

            stories = isOrderingDesc
                                    ? [.. stories.OrderByDescending(storie => storie.Score)]
                                    : [.. stories.OrderBy(storie => storie.Score)];

            _logger.LogInformation("GetStories finish");
            return Ok(stories ?? []);
        }

        private static JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private static async Task<List<int>> GetStoryIds()
        {
            var storiIds = new List<int>();
            var idsUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
            var client = new HttpClient();
            var httpResponse = await client.GetAsync(idsUrl);
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                storiIds = JsonSerializer.Deserialize<List<int>>(content);
            }
            return storiIds ?? [];
        }

        private static async Task<Story> GetStoryDetail(int id)
        {
            var url = $"https://hacker-news.firebaseio.com/v0/item/{id}.json";
            var client = new HttpClient();
            var httpResponse = await client.GetAsync(url);
            var story = new Story();
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                story = JsonSerializer.Deserialize<Story>(content, GetOptions());
            }
            return story ?? new Story();
        }
    }
}
