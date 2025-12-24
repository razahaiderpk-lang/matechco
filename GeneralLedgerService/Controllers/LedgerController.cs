using GeneralLedgerService.Domain;
using GeneralLedgerService.Services;
using GeneralLedgerService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeneralLedgerService.Controllers;

[Authorize]
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
    public async Task<ActionResult<DetailedTrialBalanceResponse>> GetTrialBalance([FromQuery] DateOnly? reportDate)
    {
        var targetDate = reportDate ?? DateOnly.FromDateTime(DateTime.Today);
        var result = await _ledgerService.GetDetailedTrialBalanceAsync(targetDate);
        return Ok(result);
    }

    [HttpGet("accounts/{accountId}/balance")]
    public async Task<ActionResult<decimal>> GetAccountBalance(int accountId)
    {
        var balance = await _ledgerService.GetAccountBalanceAsync(accountId);
        return Ok(balance);
    }

    [HttpGet("balance-sheet")]
    public async Task<ActionResult<BalanceSheetResponse>> GetBalanceSheet([FromQuery] DateOnly? asOfDate)
    {
        var targetDate = asOfDate ?? DateOnly.FromDateTime(DateTime.Today);
        var result = await _ledgerService.GetBalanceSheetAsync(targetDate);
        return Ok(result);
    }

    [HttpGet("accounts/{accountId}/ledger")]
    public async Task<ActionResult<AccountLedgerResponse>> GetAccountLedger(
        int accountId, 
        [FromQuery] DateOnly fromDate, 
        [FromQuery] DateOnly toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var result = await _ledgerService.GetAccountLedgerAsync(accountId, fromDate, toDate, page, pageSize);
        return Ok(result);
    }
}
