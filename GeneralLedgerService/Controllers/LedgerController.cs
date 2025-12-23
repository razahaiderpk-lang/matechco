using GeneralLedgerService.Domain;
using GeneralLedgerService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeneralLedgerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LedgerController : ControllerBase
{
    private readonly ILedgerService _ledgerService;

    public LedgerController(ILedgerService ledgerService)
    {
        _ledgerService = ledgerService;
    }

    [HttpPost("entries")]
    public async Task<ActionResult<JournalEntry>> PostEntry([FromBody] JournalEntry entry, [FromHeader(Name = "X-Request-ID")] string? requestId)
    {
        // Note: Idempotency with X-Request-ID could be implemented here or via middleware
        try
        {
            var result = await _ledgerService.PostEntryAsync(entry);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("trial-balance")]
    public async Task<ActionResult<Dictionary<string, decimal>>> GetTrialBalance()
    {
        var result = await _ledgerService.GetTrialBalanceAsync();
        return Ok(result);
    }

    [HttpGet("accounts/{accountId}/balance")]
    public async Task<ActionResult<decimal>> GetAccountBalance(int accountId)
    {
        var balance = await _ledgerService.GetAccountBalanceAsync(accountId);
        return Ok(balance);
    }
}
