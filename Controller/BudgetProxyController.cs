using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("budget")]
[RequireLogin]
public class BudgetProxyController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public BudgetProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BudgetService");
    }

    [HttpGet("{accountId}/require-budget")]
    public async Task<IActionResult> CheckBudgetRequired(Guid accountId) =>
        await ForwardRequest($"/budget/budget/{accountId}/require-budget");
    
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetBudgetByAccountId(Guid accountId) =>
        await ForwardRequest($"/budget/budget/{accountId}");

    [HttpPost("{accountId}/generate")]
    public async Task<IActionResult> GenerateBudget(Guid accountId, [FromBody] object request) =>
        await ForwardPost($"/budget/budget/{accountId}/generate", request);

    [HttpPost("{accountId}/create")]
    public async Task<IActionResult> CreateBudget(Guid accountId, [FromBody] object budget) =>
        await ForwardPost($"/budget/budget/{accountId}/create", budget);

    [HttpPost("transaction/create")]
    public async Task<IActionResult> CreateTransaction([FromBody] object transaction) =>
        await ForwardPost($"/transection/transaction/create", transaction);
    
    [HttpGet("transaction/{accountId}")]
    public async Task<IActionResult> GetTransactionsByAccountId(Guid accountId) =>
        await ForwardRequest($"/transection/transaction/{accountId}");

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
}