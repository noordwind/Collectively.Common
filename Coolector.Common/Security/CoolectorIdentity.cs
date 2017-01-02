using System.Security.Claims;
using System.Security.Principal;

namespace Coolector.Common.Security
{
    public class CoolectorIdentity : ClaimsPrincipal
    {
        public CoolectorIdentity(string name) : base(new GenericIdentity(name))
        {
        }
    }
}