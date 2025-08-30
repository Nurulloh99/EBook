namespace EBook.Domain.Entities;

public class User
{
    public long UserId { get; set; }               // Unikal user ID
    public string FirstName { get; set; }          // Ismi
    public string LastName { get; set; }           // Familiyasi
    public string UserName { get; set; }           // Laqabi
    public string Email { get; set; }              // Email adresi (unikal bo'lishi mumkin)
    public string Password { get; set; }       // Parol (hashlangan ko'rinishda)
    public string PhoneNumber { get; set; }        // Telefon raqami
    public string Salt { get; set; }

    public long? ConfirmerId { get; set; }
    public UserConfirmer? Confirmer { get; set; }

    public long RoleId { get; set; }               // FK: User qaysi rolda ekanligini bildiradi
    public Role Role { get; set; }                 // Navigation: Userning roli

    public ICollection<Book> Books { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
