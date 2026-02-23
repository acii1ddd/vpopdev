using ToDo.API.Data.Models.Enums;

namespace ToDo.API.Dtos.ToDo;

public record PutToDoRequest(
    string Title,
    string Description,
    bool IsCompleted,
    Priority Priority
);