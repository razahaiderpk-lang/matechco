using GeneralLedgerService.Domain;
using Xunit;

namespace GeneralLedgerService.Tests;

public class JournalEntryTests
{
    [Fact]
    public void IsBalanced_ShouldReturnTrue_WhenDebitsEqualCredits()
    {
        // Arrange
        var entry = new JournalEntry
        {
            Lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = 1, Amount = 100 }, // Debit
                new JournalEntryLine { AccountId = 2, Amount = -100 } // Credit
            }
        };

        // Act
        var result = entry.IsBalanced();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsBalanced_ShouldReturnFalse_WhenDebitsDoNotEqualCredits()
    {
        // Arrange
        var entry = new JournalEntry
        {
            Lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountId = 1, Amount = 100 }, // Debit
                new JournalEntryLine { AccountId = 2, Amount = -50 }  // Credit (Unbalanced)
            }
        };

        // Act
        var result = entry.IsBalanced();

        // Assert
        Assert.False(result);
    }
}
