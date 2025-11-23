namespace Payments.Domain;

public class Balance
{
    public int Id { get; set; }
    public required Profile Profile { get; set; }
    public required int Year { get; set; }
    public required int Month { get; set; }
    public required decimal PlannedAmount { get; set; }
    public required string Description { get; set; }
}
