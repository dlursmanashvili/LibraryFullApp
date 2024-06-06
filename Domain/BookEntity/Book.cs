using Domain.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.BookEntity;

public class Book : BaseEntity<int>
{
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public int BookStatus { get; set; }

}

