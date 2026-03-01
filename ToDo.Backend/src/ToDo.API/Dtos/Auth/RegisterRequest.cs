namespace ToDo.API.Dtos.Auth;

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);
