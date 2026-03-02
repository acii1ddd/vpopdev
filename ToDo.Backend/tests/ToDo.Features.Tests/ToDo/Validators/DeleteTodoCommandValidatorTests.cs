using FluentValidation.TestHelper;

using ToDo.API.Features.ToDos.DeleteTodo;

namespace ToDo.Features.Tests.ToDo.Validators;

public class DeleteTodoCommandValidatorTests
{
    private readonly DeleteTodoCommandValidator _validator = new();

    [Fact]
    public void Id_WhenValidGuid_ShouldNotHaveError()
    {
        // Arrange
        DeleteTodoCommand command = new(Guid.NewGuid());

        // Act & Assert
        TestValidationResult<DeleteTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Id_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        DeleteTodoCommand command = new(Guid.Empty);

        // Act & Assert
        TestValidationResult<DeleteTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id cannot be empty");
    }

    [Fact]
    public void Id_WhenNewGuid_ShouldNotHaveError()
    {
        // Arrange
        DeleteTodoCommand command = new(new Guid("11111111-1111-1111-1111-111111111111"));

        // Act & Assert
        TestValidationResult<DeleteTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("00000000000000000000000000000000")]
    public void Id_WhenZeroGuidString_ShouldHaveError(string guidString)
    {
        // Arrange
        Guid id = Guid.Parse(guidString);
        DeleteTodoCommand command = new(id);

        // Act & Assert
        TestValidationResult<DeleteTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id cannot be empty");
    }

    [Theory]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData("12345678-1234-1234-1234-123456789012")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void Id_WhenNonZeroGuid_ShouldNotHaveError(string guidString)
    {
        // Arrange
        Guid id = Guid.Parse(guidString);
        DeleteTodoCommand command = new(id);

        // Act & Assert
        TestValidationResult<DeleteTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}