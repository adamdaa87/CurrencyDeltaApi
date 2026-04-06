using Microsoft.AspNetCore.Mvc;
using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Services;
using CurrencyDeltaApi.Validation;

namespace CurrencyDeltaApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class CurrencyDeltaController : ControllerBase
{
    private readonly IRequestValidator _validator;
    private readonly ICurrencyDeltaService _service;

    public CurrencyDeltaController(IRequestValidator validator, ICurrencyDeltaService service)
    {
        _validator = validator;
        _service = service;
    }

    /// <summary>
    /// Computes exchange-rate deltas between two dates for a set of currencies
    /// relative to a baseline currency.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CurrencyDeltaRequest request,CancellationToken ct)
    {
        // Validation throws CurrencyValidationException on failure,
        // which the global middleware converts to a 400 response.
        var validated = _validator.Validate(request);

        var deltas = await _service.GetDeltasAsync(validated, ct);

        return Ok(deltas);
    }
}
