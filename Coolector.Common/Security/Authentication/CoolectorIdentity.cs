using System.Security.Claims;
using System.Security.Principal;

namespace Coolector.Common.Security.Authentication
{
    public class CoolectorIdentity : ClaimsPrincipal
    {
        public CoolectorIdentity(string name) : base(new GenericIdentity(name))
        {
        }
    }
}