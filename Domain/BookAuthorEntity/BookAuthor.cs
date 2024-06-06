using Domain.BaseModel;

namespace Domain.BookAuthorEntity;

public class BookAuthor : BaseEntity<int>
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
}
