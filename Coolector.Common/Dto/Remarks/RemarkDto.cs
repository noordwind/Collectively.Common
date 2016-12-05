using System;
using System.Collections.Generic;
using Coolector.Common.Dto.General;

namespace Coolector.Common.Dto.Remarks
{
    public class RemarkDto
    {
        public Guid Id { get; set; }
        public RemarkAuthorDto Author { get; set; }
        public RemarkCategoryDto Category { get; set; }
        public LocationDto Location { get; set; }
        public List<FileDto> Photos { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public RemarkAuthorDto Resolver { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public bool Resolved { get; set; }
    }
}