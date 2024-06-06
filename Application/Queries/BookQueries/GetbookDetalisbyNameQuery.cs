using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.BookQueries;

public class GetbookDetalisbyNameQuery : Query<List<GetbookDetalisQueryResultItem>?>
{
    public string Title { get; set; }

    public override async Task<QueryExecutionResult<List<GetbookDetalisQueryResultItem>?>> Execute()
    {
        var query = _appContext.Books.Where(x => x.IsDeleted == false && x.Title == Title).AsQueryable();
        if (query.Any())
        {


            var result = await (from b in query                                
                                select new GetbookDetalisQueryResultItem
                                {
                                    Id = b.Id,
                                    Title = b.Title,
                                    Description = b.Description,
                                    PathImg = b.PathImg,
                                    Rating = b.Rating,
                                    PublishDate = b.PublishDate,
                                    BookStatus = b.BookStatus,
                                    AuthorDetails = (from ba in _appContext.BookAuthors
                                                     join author in _appContext.Authors on ba.AuthorId equals author.Id
                                                     where ba.BookId == b.Id && ba.IsDeleted == false && author.IsDeleted == false
                                                     select new AuthorDelatisQueryResulItem
                                                     {
                                                         FirstName = author.FirstName,
                                                         LastName = author.LastName,
                                                         BirthDate = author.BirthDate
                                                     }).ToList()
                                }).ToListAsync();

        }

        return await Ok(null);
    }
}
public class GetbookDetalisQueryResultItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public int BookStatus { get; set; }
    public List<AuthorDelatisQueryResulItem>? AuthorDetails { get; set; }
}
public class AuthorDelatisQueryResulItem
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}
