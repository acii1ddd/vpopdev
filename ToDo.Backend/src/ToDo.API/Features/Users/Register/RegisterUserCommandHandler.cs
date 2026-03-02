using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDo.API.Features.Users.Services;
using ToDO.Shared.CQRS;

namespace ToDo.API.Features.Users.Register;

public sealed record RegisterUserCommand(
    string Name,
    string Email,
    string Password
) : ICommand<RegisterUserResult>;

public sealed record RegisterUserResult;

public sealed record RegisterUserCommandHandler(IUserRepository UserRepository, IPasswordHasher PasswordHasher)
    : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var findUser = await UserRepository.GetByEmailAsync(command.Email, ct);

        if (findUser is not null)
        {
            throw new OperationCanceledException($"User with this email already exists: {command.Email}");
        }

        //todo add validations for user property
        
        var newUser = new User{
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            PasswordHash = PasswordHasher.Hash(command.Password),
        };
        
        await UserRepository.AddAsync(newUser, ct);

        return new RegisterUserResult();
    }
}