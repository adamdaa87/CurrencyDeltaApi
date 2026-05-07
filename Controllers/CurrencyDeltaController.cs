using Microsoft.AspNetCore.Mvc;
using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Services;
using CurrencyDeltaApi.Validation;

namespace CurrencyDeltaApi.Controllers;

// API-controller som hanterar valutakursdeltan
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

    // Beräknar valutakursförändringar mellan två datum
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CurrencyDeltaRequest request,CancellationToken ct)
    {
        // Validera request (kastar CurrencyValidationException vid fel)
        var validated = _validator.Validate(request);

        var deltas = await _service.GetDeltasAsync(validated, ct);

        return Ok(deltas);
    }
}
