namespace Coolector.Common.Security
{
    public interface IJwtTokenHandler
    {
        string Create(string userId);
        JwtToken GetFromAuthorizationHeader(string authorizationHeader);
        bool IsValid(JwtToken token);
    }
}