namespace Coolector.Common.Encryption
{
    public interface IEncrypter
    {
        string GetRandomSecureKey();
        string GetSalt(string value);
        string GetHash(string value, string salt);
    }
}