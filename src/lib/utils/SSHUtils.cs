using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using SimpleGitShell.Library.Exceptions.SSH;
using SimpleGitShell.Library.Utils.Processes.SSH;

namespace SimpleGitShell.Library.Utils;

public static partial class SSHUtils
{

    [GeneratedRegex("ssh-rsa AAAA[0-9A-Za-z+/]+[=]{0,3} ([^@]+@[^@]+)")]
    private static partial Regex CommentRegex();
    public static readonly string SSHPath = ".ssh";
    public static readonly string SSHAuthorizedKeys = Path.Combine(SSHPath, "authorized_keys");

    public static IList<string> ReadKeys()
    {
        if (!Directory.Exists(SSHPath))
        {
            return new List<string>();
        }

        string authorizedKeys = File.ReadAllText(SSHAuthorizedKeys);
        return authorizedKeys.Split("\n").ToList()
            .FindAll(publicKey => !string.IsNullOrWhiteSpace(publicKey));
    }

    public static void AddKey(string? publicKey)
    {
        if (!IsValidKey(publicKey))
        {
            throw new PublicKeyNotValidException();
        }

        if (DoesKeyExist(publicKey))
        {
            throw new PublicKeyAlreadyExistsException();
        }

        var keys = ReadKeys();
        keys.Add(publicKey);
        WriteKeys(keys);
    }

    public static void RemoveKey(string publicKey)
    {
        var keys = ReadKeys();
        if (!keys.Contains(publicKey))
        {
            throw new PublicKeyDoesNotExistException();
        }

        keys.Remove(publicKey);
        WriteKeys(keys);
    }

    public static bool IsValidKey([NotNullWhen(true)] string? publicKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            return false;
        }

        var uuid = Guid.NewGuid();
        var tmpFile = $"id_rsa{uuid}.pub";
        var writer = File.CreateText(tmpFile);
        writer.Write(publicKey);
        writer.Close();

        using (var sshKeygenFingerprintCommand = new SSHKeygenFingerprintProcess(tmpFile))
        {
            if (sshKeygenFingerprintCommand.Start() != 0)
            {
                File.Delete(tmpFile);
                return false;
            }
        }
        File.Delete(tmpFile);
        return true;
    }

    public static bool DoesKeyExist(string publicKey)
    {
        return ReadKeys().Contains(publicKey);
    }

    private static void WriteKeys(IEnumerable<string> publicKeys)
    {
        if (!Directory.Exists(SSHPath))
        {
            Directory.CreateDirectory(SSHPath);
        }

        File.WriteAllText(SSHAuthorizedKeys, string.Join("\n", publicKeys));
    }

    public static string Comment(string publicKey)
    {
        return CommentRegex().Match(publicKey).Groups[1].ToString();
    }
}
