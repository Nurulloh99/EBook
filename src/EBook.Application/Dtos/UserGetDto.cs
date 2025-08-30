namespace EBook.Application.Dtos;

public class UserGetDto
{
    public long UserId { get; set; }               // Unikal user ID
    public string FirstName { get; set; }          // Ismi
    public string LastName { get; set; }           // Familiyasi
    public string UserName { get; set; }           // Laqabi
    public string Email { get; set; }              // Email adresi (unikal bo'lishi mumkin)
    public string PhoneNumber { get; set; }        // Telefon raqami
    public long RoleId { get; set; }
    public string RoleName { get; set; }
}
