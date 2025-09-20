using Payments.Domain;
using Payments.Infra;

namespace Payments.Application.Commands.v1.CreateProfile;

public class CreateProfileCommandHandler(PaymentsDbContext context) : ICommandHandler<CreateProfileCommand, CreateProfileCommandResponse>
{
    private readonly PaymentsDbContext _context = context;

    public async Task<CreateProfileCommandResponse> HandleAsync(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = new Profile
        {
            Name = command.Name
        };

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProfileCommandResponse(profile.Id, profile.Name);
    }
}
