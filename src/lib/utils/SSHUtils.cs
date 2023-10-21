using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Server.GitShell.Lib.Exceptions.SSH;
using Server.GitShell.Lib.Utils.Commands.SSH;

namespace Server.GitShell.Lib.Utils;

public static class SSHUtils 
{
    public static readonly string SSH_PATH = ".ssh";
    public static readonly string SSH_AUTORIZED_KEYS = Path.Combine(SSH_PATH, "authorized_keys");

    public static List<string> ReadKeys()
    {
        if (!Directory.Exists(SSH_PATH)) return new List<string>();
        string authorizedKeys = File.ReadAllText(SSH_AUTORIZED_KEYS);
        return new List<string>(authorizedKeys.Split("\n"))
            .FindAll(publicKey => !string.IsNullOrWhiteSpace(publicKey));
    }

    public static void AddKey(string? publicKey) 
    {
        if (!IsValidKey(publicKey)) throw new PublicKeyNotValidException();
        if (DoesKeyExist(publicKey)) throw new PublicKeyAlreadyExistsException();
        var keys = ReadKeys();
        keys.Add(publicKey);
        _WriteKeys(keys);
    }

    public static void RemoveKey(string publicKey)
    {   
        var keys = ReadKeys();
        if (!keys.Contains(publicKey)) throw new PublicKeyDoesNotExistException();
        keys.Remove(publicKey);
        _WriteKeys(keys);
    }

    public static bool IsValidKey([NotNullWhen(true)] string? publicKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey)) return false;
        var uuid = Guid.NewGuid();
        var tmpFile = $"id_rsa{ uuid }.pub";
        var writer = File.CreateText(tmpFile);
        writer.Write(publicKey);
        writer.Close();

        var sshKeygenFingerprintCommand = new SSHKeygenFingerprintCommand(tmpFile);
        var process = sshKeygenFingerprintCommand.Start();
        if (process.ExitCode != 0) 
        {
            File.Delete(tmpFile);
            return false;
        }
        File.Delete(tmpFile);
        return true;
    }

    public static bool DoesKeyExist(string publicKey) 
    {
        return ReadKeys().Contains(publicKey);
    }

    private static void _WriteKeys(IEnumerable<string> publicKeys)
    {
        if (!Directory.Exists(SSH_PATH)) Directory.CreateDirectory(SSH_PATH);
        File.WriteAllText(SSH_AUTORIZED_KEYS, string.Join("\n", publicKeys));
    }

    public static string Comment(string publicKey)
    {
        return new Regex(@"= (.*)$").Match(publicKey).Groups[1].ToString();
    }
}