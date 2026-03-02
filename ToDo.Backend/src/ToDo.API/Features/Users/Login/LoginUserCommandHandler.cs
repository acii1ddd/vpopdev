using ToDo.API.Data.Interfaces;
using ToDo.API.Dtos.Auth;
using ToDo.API.Features.Users.Services;
using ToDO.Shared.CQRS;
using ToDo.Shared.Exceptions;

namespace ToDo.API.Features.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResult>;

public sealed record LoginUserResult(LoginResponse Response);

public sealed record LoginUserCommandHandler(
    IUserRepository UserRepository,
    IPasswordHasher PasswordHasher,
    IJwtTokenService TokenService) : ICommandHandler<LoginUserCommand, LoginUserResult>
{
    public async Task<LoginUserResult> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var findUser = await UserRepository.GetByEmailAsync(command.Email, ct);

        if (findUser == null) 
            throw new NotFoundException("Check your password or email.");

        if (!PasswordHasher.Verify(findUser.PasswordHash, command.Password))
            throw new NotFoundException("Check your password or email.");

        var newToken = TokenService.GenerateToken(findUser);

        return new LoginUserResult(new LoginResponse(newToken));
    }
}