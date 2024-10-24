using System;

namespace FormsCreator.Core.DTOs.TemplateAccess
{
    public class TemplateAccessRequestDto
    {
        public Guid TemplateId { get; set; }

        public Guid UserId { get; set; }
    }
}
