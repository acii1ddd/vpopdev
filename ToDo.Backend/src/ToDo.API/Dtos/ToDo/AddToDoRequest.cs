using ToDo.API.Data.Models.Enums;

namespace ToDo.API.Dtos.ToDo;

public record AddToDoRequest(
    string Title,
    string Description,
    Priority Priority
);
