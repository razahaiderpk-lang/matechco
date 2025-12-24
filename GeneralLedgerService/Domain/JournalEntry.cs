using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralLedgerService.Domain;

public class JournalEntry
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public List<JournalEntryLine> Lines { get; set; } = new();

    public bool IsBalanced()
    {
        return Lines.Sum(l => l.Amount) == 0;
    }

    public void Validate()
    {
        if (Lines.Count < 2)
        {
            throw new InvalidOperationException("A journal entry must have at least two lines.");
        }

        if (Lines.Any(l => l.Amount == 0))
        {
            throw new InvalidOperationException("A journal entry line cannot have an amount of zero.");
        }

        if (!IsBalanced())
        {
            throw new InvalidOperationException("Journal entry is not balanced. Total Debits must equal Total Credits.");
        }
    }
}

public class JournalEntryLine
{
    public int Id { get; set; }
    public int JournalEntryId { get; set; }
    public JournalEntry? JournalEntry { get; set; }
    public int AccountId { get; set; }
    public Account? Account { get; set; }
    public decimal Amount { get; set; } // Positive for Debit, Negative for Credit
    public string Description { get; set; } = string.Empty;
}
