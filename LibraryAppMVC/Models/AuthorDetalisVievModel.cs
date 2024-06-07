namespace LibraryAppMVC.Models;

public class AuthorDetalisVievModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<BookDetails>? bookDetails { get; set; }

}
public class BookDetails
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public bool BookinLibrary { get; set; }
}
