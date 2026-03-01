namespace ToDo.Shared.Exceptions;

public class NotFoundException(string message) : Exception(message);