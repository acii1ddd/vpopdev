using FluentValidation.TestHelper;

using ToDo.API.Data.Models.Enums;
using ToDo.API.Features.ToDos.AddTodo;

namespace ToDo.Features.Tests.ToDo.Validators;

public class AddTodoCommandValidatorTests
{
    private readonly AddTodoCommandValidator _validator = new();

    #region UserId Validation

    [Fact]
    public void UserId_WhenValidGuid_ShouldNotHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void UserId_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            "Valid description here",
            Priority.Medium,
            Guid.Empty
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("UserId cannot be empty");
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("00000000000000000000000000000000")]
    public void UserId_WhenZeroGuidString_ShouldHaveError(string guidString)
    {
        // Arrange
        var id = Guid.Parse(guidString);
        var command = new AddTodoCommand(
            "Valid Title",
            "Valid description here",
            Priority.Medium,
            id
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("UserId cannot be empty");
    }

    [Theory]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData("12345678-1234-1234-1234-123456789012")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void UserId_WhenNonZeroGuid_ShouldNotHaveError(string guidString)
    {
        // Arrange
        var id = Guid.Parse(guidString);
        var command = new AddTodoCommand(
            "Valid Title",
            "Valid description here",
            Priority.Medium,
            id
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    #endregion

    #region Title Validation

    [Fact]
    public void Title_WhenValid_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title 123",
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Title_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            string.Empty,
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required");
    }

    [Fact]
    public void Title_WhenNull_ShouldHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            null!,
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required");
    }

    [Fact]
    public void Title_WhenLessThan3Characters_ShouldHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Ab",
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must be at least 3 characters long");
    }

    [Fact]
    public void Title_WhenExactly3Characters_ShouldNotHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Abc",
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Title_WhenMoreThan256Characters_ShouldHaveError()
    {
        // Arrange
        var longTitle = new string('A', 257);
        var command = new AddTodoCommand(
            longTitle,
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title cannot exceed 256 characters");
    }

    [Fact]
    public void Title_WhenExactly256Characters_ShouldNotHaveError()
    {
        // Arrange
        var title = new string('A', 256);
        var command = new AddTodoCommand(
            title,
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData("Valid Title 123")]
    [InlineData("Test-Title_With!Special?Chars")]
    [InlineData("Название на русском")]
    [InlineData("123")]
    public void Title_WithValidCharacters_ShouldNotHaveError(string title)
    {
        // Arrange
        var command = new AddTodoCommand(
            title,
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData("Invalid@Title#")]
    [InlineData("Title$With%Special^Chars&")]
    [InlineData("Title<With>Invalid|Chars")]
    public void Title_WithInvalidCharacters_ShouldHaveError(string title)
    {
        // Arrange
        var command = new AddTodoCommand(
            title,
            "Valid description here",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title contains invalid characters");
    }

    #endregion

    #region Description Validation

    [Fact]
    public void Description_WhenEmpty_ShouldHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            string.Empty,
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required");
    }

    [Fact]
    public void Description_WhenNull_ShouldHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            null!,
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required");
    }

    [Fact]
    public void Description_WhenLessThan5Characters_ShouldHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            "Abcd",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be at least 5 characters long");
    }

    [Fact]
    public void Description_WhenExactly5Characters_ShouldNotHaveError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            "Abcde",
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Description_WhenMoreThan256Characters_ShouldHaveError()
    {
        // Arrange
        var longDescription = new string('D', 257);
        var command = new AddTodoCommand(
            "Valid Title",
            longDescription,
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description cannot exceed 256 characters");
    }

    [Fact]
    public void Description_WhenExactly256Characters_ShouldNotHaveError()
    {
        // Arrange
        var description = new string('D', 256);
        var command = new AddTodoCommand(
            "Valid Title",
            description,
            Priority.Medium,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    #endregion

    #region Priority Validation

    [Theory]
    [InlineData(Priority.Low)]
    [InlineData(Priority.Medium)]
    [InlineData(Priority.High)]
    public void Priority_WhenValidEnumValue_ShouldNotHaveError(Priority priority)
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            "Valid description here",
            priority,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Fact]
    public void Priority_WhenInvalidEnumValue_ShouldHaveError()
    {
        // Arrange
        var invalidPriority = (Priority)999;
        var command = new AddTodoCommand(
            "Valid Title",
            "Valid description here",
            invalidPriority,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Priority);
    }

    #endregion

    #region Complex Scenarios

    [Fact]
    public void Command_WhenAllFieldsValid_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Task Title",
            "This is a valid description with enough characters",
            Priority.High,
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Command_WhenMultipleFieldsInvalid_ShouldHaveAllErrors()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Ab", // Too short
            "Abcd", // Too short
            (Priority)999, // Invalid enum
            Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.Priority);
    }

    [Fact]
    public void Command_WhenInvalidUserIdAndValidFields_ShouldHaveUserIdError()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Valid Title",
            "Valid description here",
            Priority.Medium,
            Guid.Empty
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Fact]
    public void Command_WhenInvalidUserIdAndInvalidFields_ShouldHaveAllErrors()
    {
        // Arrange
        var command = new AddTodoCommand(
            "Ab",
            "Abcd",
            (Priority)999,
            Guid.Empty
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.Priority);
    }

    #endregion
}