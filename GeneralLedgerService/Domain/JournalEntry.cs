using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralLedgerService.Domain;

public class JournalEntry
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public List<JournalEntryLine> Lines { get; set; } = new();

    public bool IsBalanced()
    {
        return Lines.Sum(l => l.Amount) == 0;
    }
}

public class JournalEntryLine
{
    public int Id { get; set; }
    public int JournalEntryId { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public decimal Amount { get; set; } // Positive for Debit, Negative for Credit
    public string Description { get; set; } = string.Empty;
}
