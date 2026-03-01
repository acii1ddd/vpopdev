using ToDo.API.Features.Users.Services;
using Xunit.Abstractions;

namespace ToDo.Features.Tests;

public class PasswordHasherTests(ITestOutputHelper output)
{
    private readonly PasswordHasher _hasher = new();

    [Theory]
    [InlineData("password123")]
    public void GetTestHashForInitPassword(string password)
    {
        // Act
        var hash = _hasher.Hash(password);
        
        output.WriteLine(hash);
    }
    
    [Fact]
    public void Hash_ReturnsNonNullHash()
    {
        // Arrange
        const string password = "TestPassword123!";

        // Act
        var hash = _hasher.Hash(password);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_GeneratesDifferentHashesForSamePassword()
    {
        // Arrange
        const string password = "SamePassword123";

        // Act
        var hash1 = _hasher.Hash(password);
        var hash2 = _hasher.Hash(password);

        // Assert
        Assert.NotNull(hash1);
        Assert.NotNull(hash2);
        Assert.NotEqual(hash1, hash2);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("Str0ng!P@ssw0rd")]
    [InlineData("simple")]
    public void Verify_ReturnsTrueForValidPassword(string password)
    {
        // Arrange
        var hash = _hasher.Hash(password);

        // Act
        var isValid = _hasher.Verify(hash, password);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Verify_ReturnsFalseForInvalidPassword()
    {
        // Arrange
        const string correctPassword = "CorrectPassword123";
        const string wrongPassword = "WrongPassword456";
        var hash = _hasher.Hash(correctPassword);

        // Act
        var isValid = _hasher.Verify(hash, wrongPassword);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Verify_ReturnsFalseForEmptyPassword()
    {
        // Arrange
        const string password = "NotEmpty";
        var hash = _hasher.Hash(password);

        // Act
        var isValid = _hasher.Verify(hash, string.Empty);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Verify_ReturnsFalseForNullPassword()
    {
        // Arrange
        const string password = "ValidPassword";
        var hash = _hasher.Hash(password);

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => _hasher.Verify(hash, null!));
    }
}