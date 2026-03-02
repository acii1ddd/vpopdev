using FluentValidation.TestHelper;

using ToDo.API.Data.Models.Enums;
using ToDo.API.Features.ToDos.PutTodo;

namespace ToDo.Features.Tests.ToDo.Validators;

public class PutTodoCommandValidatorTests
{
    private readonly PutTodoCommandValidator _validator = new();

    #region Id Validation

    [Fact]
    public void Id_WhenValidGuid_ShouldNotHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid());

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Id_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.Empty);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id cannot be empty");
    }

    #endregion

    #region Title Validation (Partial Update)

    [Fact]
    public void Title_WhenNull_ShouldNotHaveError()
    {
        // Arrange - частичное обновление без Title
        PutTodoCommand command = new(Guid.NewGuid());

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Title_WhenValid_ShouldNotHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), "Valid Title");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Title_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), string.Empty);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title cannot be empty");
    }

    [Fact]
    public void Title_WhenLessThan3Characters_ShouldHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), "Ab");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must be at least 3 characters long");
    }

    [Fact]
    public void Title_WhenExactly3Characters_ShouldNotHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), "Abc");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Title_WhenMoreThan256Characters_ShouldHaveError()
    {
        // Arrange
        string longTitle = new('A', 257);
        PutTodoCommand command = new(Guid.NewGuid(), longTitle);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title cannot exceed 256 characters");
    }

    [Fact]
    public void Title_WhenExactly256Characters_ShouldNotHaveError()
    {
        // Arrange
        string title = new('A', 256);
        PutTodoCommand command = new(Guid.NewGuid(), title);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData("Valid Title 123")]
    [InlineData("Test-Title_With!Special?Chars")]
    [InlineData("На русском")]
    public void Title_WithValidCharacters_ShouldNotHaveError(string title)
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), title);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData("Invalid@Title#")]
    [InlineData("Title$With%Special")]
    public void Title_WithInvalidCharacters_ShouldHaveError(string title)
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), title);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title contains invalid characters");
    }

    #endregion

    #region Description Validation (Partial Update)

    [Fact]
    public void Description_WhenNull_ShouldNotHaveError()
    {
        // Arrange - partial update without Description
        PutTodoCommand command = new(Guid.NewGuid(), Description: null);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Description_WhenValid_ShouldNotHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), Description: "Valid description text");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Description_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), Description: string.Empty);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description cannot be empty");
    }

    [Fact]
    public void Description_WhenLessThan5Characters_ShouldHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), Description: "Abcd");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be at least 5 characters long");
    }

    [Fact]
    public void Description_WhenExactly5Characters_ShouldNotHaveError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), Description: "Abcde");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Description_WhenMoreThan256Characters_ShouldHaveError()
    {
        // Arrange
        string longDescription = new('D', 257);
        PutTodoCommand command = new(Guid.NewGuid(), Description: longDescription);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description cannot exceed 256 characters");
    }

    [Fact]
    public void Description_WhenExactly256Characters_ShouldNotHaveError()
    {
        // Arrange
        string description = new('D', 256);
        PutTodoCommand command = new(Guid.NewGuid(), Description: description);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    #endregion

    #region Priority Validation (Partial Update)

    [Fact]
    public void Priority_WhenNull_ShouldNotHaveError()
    {
        // Arrange - patrial update without Priority
        PutTodoCommand command = new(Guid.NewGuid(), Priority: null);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Theory]
    [InlineData(Priority.Low)]
    [InlineData(Priority.Medium)]
    [InlineData(Priority.High)]
    public void Priority_WhenValidEnumValue_ShouldNotHaveError(Priority priority)
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), Priority: priority);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Fact]
    public void Priority_WhenInvalidEnumValue_ShouldHaveError()
    {
        // Arrange
        const Priority invalidPriority = (Priority)999;
        PutTodoCommand command = new(Guid.NewGuid(), Priority: invalidPriority);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Priority)
            .WithErrorMessage("Invalid priority value");
    }

    #endregion

    #region IsCompleted Validation (Partial Update)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsCompleted_WhenAnyBoolValue_ShouldNotHaveError(bool? isCompleted)
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), IsCompleted: isCompleted);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.IsCompleted);
    }

    [Fact]
    public void IsCompleted_WhenNull_ShouldNotHaveError()
    {
        // Arrange - частичное обновление без IsCompleted
        PutTodoCommand command = new(Guid.NewGuid(), IsCompleted: null);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.IsCompleted);
    }

    #endregion

    #region Complex Scenarios (Partial Update)

    [Fact]
    public void Command_WhenOnlyIdProvided_ShouldNotHaveAnyErrors()
    {
        // Arrange - минимальное обновление, только Id
        PutTodoCommand command = new(Guid.NewGuid());

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Command_WhenPartialUpdateWithValidTitle_ShouldNotHaveErrors()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), "Updated Title");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Command_WhenPartialUpdateWithValidPriority_ShouldNotHaveErrors()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), Priority: Priority.High);

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Command_WhenPartialUpdateWithInvalidTitle_ShouldHaveOnlyTitleError()
    {
        // Arrange
        PutTodoCommand command = new(Guid.NewGuid(), "Ab");

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
        result.ShouldNotHaveValidationErrorFor(x => x.IsCompleted);
    }

    [Fact]
    public void Command_WhenFullUpdateWithAllValidFields_ShouldNotHaveErrors()
    {
        // Arrange
        PutTodoCommand command = new(
            Guid.NewGuid(),
            "Valid Title",
            "Valid description here",
            true,
            Priority.High
        );

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Command_WhenFullUpdateWithMultipleInvalidFields_ShouldHaveAllErrors()
    {
        // Arrange
        PutTodoCommand command = new(
            Guid.NewGuid(),
            "Ab",
            "Abcd",
            Priority: (Priority)999
        );

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.Priority);
    }

    [Fact]
    public void Command_WhenInvalidIdAndValidFields_ShouldHaveIdError()
    {
        // Arrange
        PutTodoCommand command = new(
            Guid.Empty,
            "Valid Title",
            Priority: Priority.Low
        );

        // Act & Assert
        TestValidationResult<PutTodoCommand>? result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    #endregion
}