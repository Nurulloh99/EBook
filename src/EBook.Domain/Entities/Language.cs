namespace EBook.Domain.Entities;

public class Language
{
    public long LanguageId { get; set; }
    public string LanguageName { get; set; }

    public ICollection<Book> Books { get; set; }
}
