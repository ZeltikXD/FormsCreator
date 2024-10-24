﻿using FormsCreator.Core.DTOs.Question;
using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.DTOs.User;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Template
{
    public class TemplateUpdateRequestDto<TImage> where TImage : class
    {
        private bool _deleteCurrentImage = false;

        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TImage? Image { get; set; }

        public bool DeleteCurrentImage { get {
                if (Image is not null) return true;
                return _deleteCurrentImage;
            } set { _deleteCurrentImage = value; } }

        public bool IsPublic { get; set; }

        public Guid TopicId { get; set; }

        public IList<QuestionRequestDto> Questions { get; set; } = null!;

        public IList<TagDto> Tags { get; set; } = null!;

        public IList<UserRequestDto> Users { get; set; } = null!;
    }
}