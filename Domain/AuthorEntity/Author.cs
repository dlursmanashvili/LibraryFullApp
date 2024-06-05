using Domain.BaseModel;

namespace Domain.AuthorEntity;

public class Author : BaseEntity<int>
{
    public string FirstName  { get; set; }
    public string LastName  { get; set; }
    public DateTime BirthDate  { get; set; }
}
