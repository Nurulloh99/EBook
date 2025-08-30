namespace EBook.Application.Dtos;

public class UserCreateDto
{
    public string FirstName { get; set; }          // Ismi
    public string LastName { get; set; }           // Familiyasi
    public string UserName { get; set; }           // Laqabi
    public string Email { get; set; }              // Email adresi (unikal bo'lishi mumkin)
    public string Password { get; set; }       // Parol (hashlangan ko'rinishda)
    public string PhoneNumber { get; set; }        // Telefon raqami
}
