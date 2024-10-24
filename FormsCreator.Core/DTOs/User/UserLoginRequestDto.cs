namespace FormsCreator.Core.DTOs.User
{
    public record UserLoginRequestDto
    {
        public string UserOrEmail { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool IsEmail => UserOrEmail?.Contains("@") ?? false;
    }
}
