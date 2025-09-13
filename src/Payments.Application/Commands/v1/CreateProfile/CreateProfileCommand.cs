namespace Payments.Application.Commands.v1.CreateProfile;

public record CreateProfileCommand(
    string Name
) : ICommand;
