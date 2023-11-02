using SimpleGitShell.Exceptions;
using SimpleGitShell.Utils.Validation;

namespace Tests.SimpleGitShell.Utils.Validation;

public class NameValidationUtilsTests
{
    [Theory]
    [InlineData("some_invalid_name")]
    [InlineData("$")]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData("no/dirs")]
    [InlineData("no.dots")]
    public void ThrowOnNameNotValidThrowsOnInvalidName(string name)
    {
        // Then Then
        Assert.Throws<InvalidNameException>(() => NameValidationUtils.ThrowOnNameNotValid("kind", name));
    }

    [Theory]
    [InlineData("CanContainLetters")]
    [InlineData("Can-Contain-Hyphens")]
    [InlineData("Can1Contain2Numbers")]
    public void ThrowOnNameNotValidDoesNotThrowOnValidName(string name)
    {
        NameValidationUtils.ThrowOnNameNotValid("kind", name);
    }
}
