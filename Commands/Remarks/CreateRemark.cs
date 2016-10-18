using System;
using Coolector.Common.Commands.Remarks.Models;

namespace Coolector.Common.Commands.Remarks
{
    public class CreateRemark : IAuthenticatedCommand
    {
        public string UserId { get; set; }
        public Guid CategoryId { get; set; }
        public RemarkFile Photo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}