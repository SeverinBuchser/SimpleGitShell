using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using SimpleGitShell.Exceptions.SSH;
using SimpleGitShell.Utils.Processes.SSH;

namespace SimpleGitShell.Utils;

public static partial class SSHUtils
{
    private static readonly Regex _CommentRegex = new(@"ssh-rsa AAAA[0-9A-Za-z+/]+[=]{0,3} ([^@]+@[^@]+)");
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
        publicKey = ValidateKey(publicKey);

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

    public static string ValidateKey([NotNullWhen(true)] string? publicKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new PublicKeyNotValidException();
        }

        publicKey = publicKey.Replace("\"", string.Empty).Replace("'", string.Empty);

        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new PublicKeyNotValidException();
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
                throw new PublicKeyNotValidException();
            }
        }
        File.Delete(tmpFile);
        return publicKey;
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
        return _CommentRegex.Match(publicKey).Groups[1].ToString();
    }
}
