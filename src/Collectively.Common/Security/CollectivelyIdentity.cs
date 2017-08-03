using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Collectively.Common.Security
{
    public class CollectivelyIdentity : ClaimsPrincipal
    {
        private readonly IEnumerable<Claim> _claims;
        public string Role { get; }
        public string State { get; }
        public override IEnumerable<Claim> Claims => _claims;

        public CollectivelyIdentity(string name, string role, string state,
            IEnumerable<Claim> claims) 
            : base(new GenericIdentity(name))
        {
            Role = role;
            State = state;
            _claims = claims;
        }
    }
}