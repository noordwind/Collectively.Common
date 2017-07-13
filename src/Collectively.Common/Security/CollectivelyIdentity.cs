using System.Security.Claims;
using System.Security.Principal;

namespace Collectively.Common.Security
{
    public class CollectivelyIdentity : ClaimsPrincipal
    {
        public CollectivelyIdentity(string name) : base(new GenericIdentity(name))
        {
        }
    }
}