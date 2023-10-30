namespace SimpleGitShellrary.Utils.Validation;

public static partial class FileSystemValidationUtils
{
    public static void ThrowOnNonExistingDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory \"{directory}\" was not found.");
        }
    }
}
