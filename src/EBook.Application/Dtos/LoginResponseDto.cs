namespace EBook.Application.Dtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; } = null;
    public UserGetDto UserData { get; set; }
}
    