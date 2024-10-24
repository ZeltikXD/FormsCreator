using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.TemplateAccess
{
	public class TemplateAccessRequestRangeDto
	{
		public Guid TemplateId { get; set; }

		public IList<Guid> UserIds { get; set; } = null!;
	}
}