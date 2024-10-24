using FormsCreator.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents a customizable template or form that can be created and filled out by users.
    /// A template consists of a title, description, and optionally an image, and can be either public or private.
    /// Templates are associated with a specific creator and topic.
    /// </summary>
    public class Template : EntityUpdate
    {
        /// <summary>
        /// Gets or sets the title of the template.
        /// The title is a short, descriptive name that helps users identify the purpose of the template.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the template.
        /// The description provides more detailed information about the template's purpose and content.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the image associated with the template.
        /// This image can serve as a visual representation or illustration for the template. 
        /// </summary>
        public string? Image_Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the template is public.
        /// If <see langword="true"/>, the template can be accessed and filled out by any authenticated user. 
        /// If <see langword="false"/>, only specified users can access the template.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the template.
        /// This associates the template with the user that originally authored it.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the creator of the template. This navigational property links to the user entity.
        /// </summary>
        public User Creator { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the topic of the template.
        /// This associates the template with the topic that categorize it.
        /// </summary>
        public Guid TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic of the template. This navigational property links to the topic entity.
        /// </summary>
        public Topic Topic { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user tags of the template. This navigational property links to a list of the tag entity.
        /// </summary>
        public ICollection<Tag> Tags { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Form> Forms { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = null!;
        public ICollection<TemplateAccess> UsersAllowed { get; set; } = null!;
    }
}
