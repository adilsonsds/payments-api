namespace Payments.Application.Commands.v1.DeleteProfile;

public record DeleteProfileCommand(int ProfileId) : ICommand;
