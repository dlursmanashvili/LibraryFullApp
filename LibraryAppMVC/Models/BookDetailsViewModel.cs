namespace LibraryAppMVC.Models;

// Create a view model to represent the data to be displayed on the details page
public class BookDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public bool BookinLibrary { get; set; }
    public List<AuthorDetailsViewModel>? AuthorDetails { get; set; }
}

public class AuthorDetailsViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}

