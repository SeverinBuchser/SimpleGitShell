using Server.GitShell.Commands.SSH.Settings;
using Server.GitShell.Commands.SSH.User;
using Server.GitShell.Lib.Exceptions.SSH;
using Spectre.Console.Testing;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.SSH.User;

[Collection("File System Sequential")]
public class AddSSHUserCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<AddSSHUserCommand>();
        return app;
    }

    [Theory]
    [InlineData("ssh-rsa = hello")]
    [InlineData("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQDwWPacdjBDr= hello")]    
    [InlineData("ssh-rsa AAAB3NzaC1yc2EAAAADAQABAAABgQDwWPacdjBDrzWt2ddZchxa/axy0XznF6Wb6HPZQ8rBkOY8h74edi/pkmZm13SRNY8X6OYqBUaUYOTMek9KGsbyRtge2OyOwDZh0Hdt4RmsqwP2fvc8dnPYgMNAV4BU200JSuEJGC/2OEKAVIf+RkQcRZ9pnEQkjLSEc0zSvT7isPxyZktmhr1q23JvESWqrlMh1qAhOK2+nX230p5iV+qcH8EhNm9R48pD6wBNHZvGaYw0pNjS/YyjGDGWysf2PeDyYvcNhVk6e8Ce+/g+gMOHAJApEWtn596pFuTk8KgZuvwJMJ2L89M7xMGMFTUkLk6su3chqTfjjCDUB04Uf19ei9PJw0keXnFHpDVzQe6ST1aRz5S31aq989WWv26Jyw0edpUrmDSI3QN8foQYk2WcR2jWBfPT1R45UkUarUK937opo/XpuCFDY7mYhI5BZbQKGVbnmmoEkUiPQTmbvLgVxF89bFzJOlWZwC/fFtdUFOaSinMi6/M/U7b2nfTzxX8= hello")]
    [InlineData("  ")]
    [InlineData("")]
    public void Run_InvalidPublicKey_ThrowsPublicKeyNotValidException(string publicKey)
    {
        // Given
        var args = new string[]{publicKey};
        
        // When
        var result = App().RunAndCatch<PublicKeyNotValidException>(args);

        // Then
        Assert.IsType<PublicKeyNotValidException>(result.Exception);
    }

    [Theory]
    [InlineData("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQDwWPacdjBDrzWt2ddZchxa/axy0XznF6Wb6HPZQ8rBkOY8h74edi/pkmZm13SRNY8X6OYqBUaUYOTMek9KGsbyRtge2OyOwDZh0Hdt4RmsqwP2fvc8dnPYgMNAV4BU200JSuEJGC/2OEKAVIf+RkQcRZ9pnEQkjLSEc0zSvT7isPxyZktmhr1q23JvESWqrlMh1qAhOK2+nX230p5iV+qcH8EhNm9R48pD6wBNHZvGaYw0pNjS/YyjGDGWysf2PeDyYvcNhVk6e8Ce+/g+gMOHAJApEWtn596pFuTk8KgZuvwJMJ2L89M7xMGMFTUkLk6su3chqTfjjCDUB04Uf19ei9PJw0keXnFHpDVzQe6ST1aRz5S31aq989WWv26Jyw0edpUrmDSI3QN8foQYk2WcR2jWBfPT1R45UkUarUK937opo/XpuCFDY7mYhI5BZbQKGVbnmmoEkUiPQTmbvLgVxF89bFzJOlWZwC/fFtdUFOaSinMi6/M/U7b2nfTzxX8= hello")]
    [InlineData("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQCZXzJCzK603gtAPFkZIXHKfR/CP1Ng9hkbNq5GCsCjjZPdGVlQDjGb04DpYpMwpkk3gdvX9KVh3KyyTLNa8Auag5oyBsNtLmHJNuW1k/MpqZJ7QWHmpHErCP1pHe/5b3+OIMAYyIgw2j+pXSIt+TcnNB8Gfg7XHCiQ+L4UiWMrJy8D1FxWCWyGK2kQTGLot+lkx3/bNv1u0QpcwasB3Q4r3m+17+J+HKe5A1YlgA98yTF1fJrf1o1OzSS6n8x1IOeKbaecEWV4edw+gkOxzH7KD5VPRR6YHZk30FPE+V79mJlZCY913QHObZX6wWnv0z4pTwEEEZUkTQ3hvoqcmnMNXZuvH8vvujX5uTQGT867U3NPwEzS9tn3YNUogwe42uhuNRinIHlbVNmdXXw1M/u59ktGol5SOEWjTHy25TnODTKo287emF3M8cP/YXyWkaU8ca1bGLqvU4c2dZj7uhRkJu++/PbqqgHlK6l8k1RTypOK74a6pBUdqIw1uy9wd3c= hello")]
    public void Run_ValidPublicKeyWithNonExistingAuthorizedKeys_CreatesAuthorizedKeysWithPublicKey(string publicKey)
    {
        // Given
        var args = new string[]{publicKey};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(".ssh"));
        Assert.True(File.Exists(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS));
        Assert.Equal(publicKey + "\n", File.ReadAllText(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS));

        // Finall
        _DeleteDirectory(BaseSSHCommandSettings.SSH_PATH);
    }

    [Theory]
    [InlineData("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQDwWPacdjBDrzWt2ddZchxa/axy0XznF6Wb6HPZQ8rBkOY8h74edi/pkmZm13SRNY8X6OYqBUaUYOTMek9KGsbyRtge2OyOwDZh0Hdt4RmsqwP2fvc8dnPYgMNAV4BU200JSuEJGC/2OEKAVIf+RkQcRZ9pnEQkjLSEc0zSvT7isPxyZktmhr1q23JvESWqrlMh1qAhOK2+nX230p5iV+qcH8EhNm9R48pD6wBNHZvGaYw0pNjS/YyjGDGWysf2PeDyYvcNhVk6e8Ce+/g+gMOHAJApEWtn596pFuTk8KgZuvwJMJ2L89M7xMGMFTUkLk6su3chqTfjjCDUB04Uf19ei9PJw0keXnFHpDVzQe6ST1aRz5S31aq989WWv26Jyw0edpUrmDSI3QN8foQYk2WcR2jWBfPT1R45UkUarUK937opo/XpuCFDY7mYhI5BZbQKGVbnmmoEkUiPQTmbvLgVxF89bFzJOlWZwC/fFtdUFOaSinMi6/M/U7b2nfTzxX8= hello")]
    [InlineData("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQCZXzJCzK603gtAPFkZIXHKfR/CP1Ng9hkbNq5GCsCjjZPdGVlQDjGb04DpYpMwpkk3gdvX9KVh3KyyTLNa8Auag5oyBsNtLmHJNuW1k/MpqZJ7QWHmpHErCP1pHe/5b3+OIMAYyIgw2j+pXSIt+TcnNB8Gfg7XHCiQ+L4UiWMrJy8D1FxWCWyGK2kQTGLot+lkx3/bNv1u0QpcwasB3Q4r3m+17+J+HKe5A1YlgA98yTF1fJrf1o1OzSS6n8x1IOeKbaecEWV4edw+gkOxzH7KD5VPRR6YHZk30FPE+V79mJlZCY913QHObZX6wWnv0z4pTwEEEZUkTQ3hvoqcmnMNXZuvH8vvujX5uTQGT867U3NPwEzS9tn3YNUogwe42uhuNRinIHlbVNmdXXw1M/u59ktGol5SOEWjTHy25TnODTKo287emF3M8cP/YXyWkaU8ca1bGLqvU4c2dZj7uhRkJu++/PbqqgHlK6l8k1RTypOK74a6pBUdqIw1uy9wd3c= hello")]
    public void Run_ExistingAuthorizedKeysWithoutNewLine_AppendsPublicKeyOnNewLine(string publicKey)
    {
        // Given
        _CreateDirectory(BaseSSHCommandSettings.SSH_PATH);
        _CreateFile(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS, "some other key");
        var args = new string[]{publicKey};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(BaseSSHCommandSettings.SSH_PATH));
        Assert.True(File.Exists(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS));
        Assert.Equal("some other key\n" + publicKey + "\n", File.ReadAllText(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS));

        // Finall
        _DeleteDirectory(BaseSSHCommandSettings.SSH_PATH);
    }

    [Theory]
    [InlineData(
        "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQDwWPacdjBDrzWt2ddZchxa/axy0XznF6Wb6HPZQ8rBkOY8h74edi/pkmZm13SRNY8X6OYqBUaUYOTMek9KGsbyRtge2OyOwDZh0Hdt4RmsqwP2fvc8dnPYgMNAV4BU200JSuEJGC/2OEKAVIf+RkQcRZ9pnEQkjLSEc0zSvT7isPxyZktmhr1q23JvESWqrlMh1qAhOK2+nX230p5iV+qcH8EhNm9R48pD6wBNHZvGaYw0pNjS/YyjGDGWysf2PeDyYvcNhVk6e8Ce+/g+gMOHAJApEWtn596pFuTk8KgZuvwJMJ2L89M7xMGMFTUkLk6su3chqTfjjCDUB04Uf19ei9PJw0keXnFHpDVzQe6ST1aRz5S31aq989WWv26Jyw0edpUrmDSI3QN8foQYk2WcR2jWBfPT1R45UkUarUK937opo/XpuCFDY7mYhI5BZbQKGVbnmmoEkUiPQTmbvLgVxF89bFzJOlWZwC/fFtdUFOaSinMi6/M/U7b2nfTzxX8= hello",
        "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQCZXzJCzK603gtAPFkZIXHKfR/CP1Ng9hkbNq5GCsCjjZPdGVlQDjGb04DpYpMwpkk3gdvX9KVh3KyyTLNa8Auag5oyBsNtLmHJNuW1k/MpqZJ7QWHmpHErCP1pHe/5b3+OIMAYyIgw2j+pXSIt+TcnNB8Gfg7XHCiQ+L4UiWMrJy8D1FxWCWyGK2kQTGLot+lkx3/bNv1u0QpcwasB3Q4r3m+17+J+HKe5A1YlgA98yTF1fJrf1o1OzSS6n8x1IOeKbaecEWV4edw+gkOxzH7KD5VPRR6YHZk30FPE+V79mJlZCY913QHObZX6wWnv0z4pTwEEEZUkTQ3hvoqcmnMNXZuvH8vvujX5uTQGT867U3NPwEzS9tn3YNUogwe42uhuNRinIHlbVNmdXXw1M/u59ktGol5SOEWjTHy25TnODTKo287emF3M8cP/YXyWkaU8ca1bGLqvU4c2dZj7uhRkJu++/PbqqgHlK6l8k1RTypOK74a6pBUdqIw1uy9wd3c= hello"
    )]
    [InlineData(
        "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQDpm9Ww/EEBW3RJv8JgGcpJ77T/9VAR+YpGEwtp80Yv1fJtLpo2WtbTP0Wua7luoQRKjH9vZK9Q/LbP+NQwRPpqRt1jr6n4Y5s0hzQF8vJZtFufv03TRvoIt1v90JK3yobSNNPlr49//GevAahiaDKtM+HXwgmuiBRuoGf41UIdNV6h3h/gd8WR6oPS1DA2OhFIaGlHncOo7uE7kSQzHO6ceadAeaPW0YW2sdXwVP5Rn0DljfCv1TkbCBqiNSCjAQP4ivbRHC2SdHc/vri9K+3wdr/D6KqRRDk4E6w9nz5jn64PmqVjG57a7C62tch1lRMyX4rttQqm8G3tVW02WemICH+Mbh7GbKkxaxvG6kgKa1BhhaghK3n6qAyVB0/9VU8AOGWcArQjI4gzfTEjLU3OoRarf8RYu5FPA7pRjDhUXuDTXC2Z73TpZakJIKkPRxNPOcHEAvgLVtYcUjGRhM8E+VBT3TmNWXclm0ctTxN8T43Nu8zaPgDs0u4uMHGxq78= hello",
        "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQCMjGPvLdvbTMasGGcjXtD15puOgUtCDiA7lft2ii4MWNKdfJkINBUE4esaORYcb8EjWaxl6KBaRKSbjVM/a6/OvlqDuVTx6IO0pgUEOPqN12F3IlsGjZYO+qS+HOhacHf2asDAYQVGMu+xCNegnYINRx+tl6Dpz1kTIBzhEQoQfBILiuXWGT2DrDLQOOwoblb9gPM9Ac+DXbH5LXbGqbLCTVRkRApK+DBhG8yhe9OF7y20OXlzo2pVCW/fwJLEVaoc7cWRBjXaOr1+qkCoiplce5sNhND/YJWEEofc7huyyeWV3e6GxQpt6sVa7Bj2Q8MBpJ3Vu4SR+kQLIVBCYt5QPwCEn6v7xT+X5mPzX68Qo0jFjjucnlg6KOmE1zZQxscOSC0ppaF6HqS1Q7j249wYV1WtY2HAyquVEYo2pA8qK1KsTVVHGnei7VzO8us8zkjR5v35fX2KK/V73fV7oWI1zZvHw0XvgYYDwlOS5azoDverHz8wUi7Euk8mMDk+96E= hello",
        "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQDjY47YC9Gme3RjMu3qncA/CdvTZsYg3NxYwlNpjudmVLFKOhjVkkxwoC52avCdY0kyrnGdlIajfL5BTMFy+QNqwd0lGMWY/V9VYD3cYjMjwfjmLMkOXMCW+hVfQyOlVxUkSwi+TwuBT4+y0258YCMTwGuQTLOZ1PKjbWkErpw1KJDcUSuN1/T4ED/M4FkT4BRqWt4Ts+8BE+p8Qsv1XWgrrcnXI5ZXd4Zkel/DyFMCWcszeMz4Xb3F7JsDuEKjU1Eic13wfHrnQokVMjMclHG0Be+THY1XzevG9omR8Z3suwUlhqlBLgWZ/d4jdiTnl/CjIIYm3lI68XdVHGvsyur0WWAF6WaVkLFiTiKpJ0q9btrey5WaaKuajlJIe55cj5Z2/2sbAGSjgIINTeTUnSap+PvB3lstuCkQGnuV6J7RXSup0Yne4vhjdOI3kUSdtwfzy0nr813XStrM/SVKa2Vdqjz/renMfUbur90t174owbjT5d7nqCvQbMKl69OfGt0= hello"
    )]
    public void Run_ValidPublicKeysWithNonExistingAuthorizedKeys_CreatesAuthorizedKeysWithMultiplePublicKeys(params string[] publicKeys)
    {
        // Given
        var argsList = new List<string[]>();
        foreach (var publicKey in publicKeys)
        {
            argsList.Add(new string[]{publicKey});
        }
        
        
        // When
        var resultList = new List<CommandAppResult>();
        foreach(var args in argsList)
        {
            resultList.Add(App().Run(args));
        }

        // Then
        foreach(var result in resultList)
        {
            Assert.Equal(0, result.ExitCode);
        }
        Assert.True(Directory.Exists(BaseSSHCommandSettings.SSH_PATH));
        Assert.True(File.Exists(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS));
        var authorizedKeys = File.ReadAllText(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS);
        foreach(var publicKey in publicKeys)
        {
            Assert.Contains(publicKey + "\n", authorizedKeys);
        }

        // Finall
        _DeleteDirectory(BaseSSHCommandSettings.SSH_PATH);
    }

} 