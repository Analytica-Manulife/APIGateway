using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

 [ApiController]
    [Route("stock")]
    [RequireLogin]
    public class StockMarketProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public StockMarketProxyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("StockMarketService");
        }

        // Portfolio Endpoints
        [HttpGet("portfolio/{accountId}")]
        public async Task<IActionResult> GetPortfolio(Guid accountId) =>
            await ForwardRequest($"/api/Portfolio/{accountId}");

        [HttpPost("portfolio/buy")]
        public async Task<IActionResult> BuyStock([FromBody] object request) =>
            await ForwardPost("/api/Portfolio/BuyStock", request);

        [HttpPost("portfolio/sell")]
        public async Task<IActionResult> SellStock([FromBody] object request) =>
            await ForwardPost("/api/Portfolio/SellStock", request);

        [HttpGet("portfolio/stockdata/{ticker}")]
        public async Task<IActionResult> GetStockData(string ticker) =>
            await ForwardRequest($"/api/Portfolio/GetStockData/{ticker}");

        [HttpPut("portfolio/stockdata")]
        public async Task<IActionResult> UpdateStockData([FromBody] object request) =>
            await ForwardPut("/api/Portfolio/UpdateStockData", request);

        // Stocks Endpoints
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStocks() =>
            await ForwardRequest("/api/Stocks");

        [HttpGet("single/{ticker}")]
        public async Task<IActionResult> GetStockByTicker(string ticker) =>
            await ForwardRequest($"/api/Stocks/{ticker}");

        // Internal forwarding helpers
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
    }