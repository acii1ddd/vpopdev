using FluentValidation.TestHelper;

using ToDo.API.Features.ToDos.GetTodo;

namespace ToDo.Features.Tests.ToDo.Validators;

public class GetTodoQueryValidatorTests
{
    private readonly GetTodoQueryValidator _validator = new();

    [Fact]
    public void Id_WhenValidGuid_ShouldNotHaveError()
    {
        // Arrange
        GetTodoQuery query = new(Guid.NewGuid());

        // Act & Assert
        TestValidationResult<GetTodoQuery>? result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Id_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        GetTodoQuery query = new(Guid.Empty);

        // Act & Assert
        TestValidationResult<GetTodoQuery>? result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id cannot be empty");
    }

    [Fact]
    public void Id_WhenNewGuid_ShouldNotHaveError()
    {
        // Arrange
        GetTodoQuery query = new(new Guid("11111111-1111-1111-1111-111111111111"));

        // Act & Assert
        TestValidationResult<GetTodoQuery>? result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("00000000000000000000000000000000")]
    public void Id_WhenZeroGuidString_ShouldHaveError(string guidString)
    {
        // Arrange
        Guid id = Guid.Parse(guidString);
        GetTodoQuery query = new(id);

        // Act & Assert
        TestValidationResult<GetTodoQuery>? result = _validator.TestValidate(query);
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
        GetTodoQuery query = new(id);

        // Act & Assert
        TestValidationResult<GetTodoQuery>? result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}