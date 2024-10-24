using System;

namespace FormsCreator.Core.DTOs.Comment
{
	public class CommentRequestDto
	{
		public Guid TemplateId { get; set; }

		public Guid UserId { get; set; }

		public string Content { get; set; } = string.Empty;
	}
}