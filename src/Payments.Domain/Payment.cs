namespace Payments.Domain;

public class Payment
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public string? Description { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime PaymentDate { get; set; }
    public required bool Completed { get; set; }
    public required Profile Profile { get; set; }
    public required DateTime CreatedAt { get; set; }
}