namespace Payments.Domain;

public class PlannedBalance
{
    public int Id { get; set; }
    public required Profile Profile { get; set; }
    public required int Year { get; set; }
    public required int Month { get; set; }
    public required decimal Amount { get; set; }
    public required string Category { get; set; }
}
