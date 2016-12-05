using System;

namespace Coolector.Common.Commands.Remarks
{
    public class DeleteRemark : IAuthenticatedCommand
    {
        public Request Request { get; set; }
        public string UserId { get; set; }
        public Guid RemarkId { get; set; }
    }
}