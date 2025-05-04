using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("news")]
    [RequireLogin] // 👈 All routes in this controller require login
    public class NewsProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public NewsProxyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FinanceNewsService");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            await ForwardRequest("/news/news");

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) =>
            await ForwardRequest($"/news/news/{id}");

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object article) =>
            await ForwardPost("/news/news", article);

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] object article) =>
            await ForwardPut($"/news/news/{id}", article);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) =>
            await ForwardDelete($"/news/news/{id}");

        [HttpGet("search/{term}")]
        public async Task<IActionResult> Search(string term) =>
            await ForwardRequest($"/news/news/search/{term}");

        private async Task<IActionResult> ForwardRequest(string path)
        {
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        private async Task<IActionResult> ForwardPost(string path, object body)
        {
            var response = await _httpClient.PostAsJsonAsync(path, body);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        private async Task<IActionResult> ForwardPut(string path, object body)
        {
            var response = await _httpClient.PutAsJsonAsync(path, body);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        private async Task<IActionResult> ForwardDelete(string path)
        {
            var response = await _httpClient.DeleteAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}
