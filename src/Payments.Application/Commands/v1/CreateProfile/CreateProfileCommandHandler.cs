using Payments.Domain;
using Payments.Infra;

namespace Payments.Application.Commands.v1.CreateProfile;

public class CreateProfileCommandHandler(PaymentsDbContext context) : ICommandHandler<CreateProfileCommand>
{
    private readonly PaymentsDbContext _context = context;

    public async Task HandleAsync(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = new Profile
        {
            Name = command.Name
        };

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
