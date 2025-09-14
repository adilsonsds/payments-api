using Payments.Infra;

namespace Payments.Application.Commands.v1.DeleteProfile;

public class DeleteProfileCommandHandler(PaymentsDbContext dbContext) : ICommandHandler<DeleteProfileCommand>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task HandleAsync(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.Profiles.FindAsync([command.ProfileId], cancellationToken)
            ?? throw new KeyNotFoundException($"Profile with ID {command.ProfileId} not found.");

        var payments = _dbContext.Payments.Where(p => p.Profile.Id == command.ProfileId);
        _dbContext.Payments.RemoveRange(payments);

        _dbContext.Profiles.Remove(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}