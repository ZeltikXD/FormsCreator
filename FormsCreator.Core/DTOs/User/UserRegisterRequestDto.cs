﻿namespace FormsCreator.Core.DTOs.User
{
    public class UserRegisterRequestDto
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}