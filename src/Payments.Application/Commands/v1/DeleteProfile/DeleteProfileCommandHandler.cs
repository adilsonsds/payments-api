using Payments.Infra;

namespace Payments.Application.Commands.v1.DeleteProfile;

public class DeleteProfileCommandHandler(PaymentsDbContext dbContext) : ICommandHandler<DeleteProfileCommand, DeleteProfileCommandResponse>
{
    private readonly PaymentsDbContext _dbContext = dbContext;

    public async Task<DeleteProfileCommandResponse> HandleAsync(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.Profiles.FindAsync([command.ProfileId], cancellationToken)
            ?? throw new KeyNotFoundException($"Profile with ID {command.ProfileId} not found.");

        var response = new DeleteProfileCommandResponse(profile.Id, profile.Name);

        var payments = _dbContext.Payments.Where(p => p.Profile.Id == command.ProfileId);
        _dbContext.Payments.RemoveRange(payments);

        _dbContext.Profiles.Remove(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}