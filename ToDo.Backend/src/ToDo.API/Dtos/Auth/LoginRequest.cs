namespace ToDo.API.Dtos.Auth;

public sealed record LoginRequest(
    string Email,
    string Password
);