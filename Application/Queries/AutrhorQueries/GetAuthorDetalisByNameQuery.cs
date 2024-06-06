using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.AutrhorQueries;

public class GetAuthorDetalisByNameQuery : Query<List<GetAuthorDetalisQueryResultItem>>
{
    public string FirstName { get; set; }

    public override async Task<QueryExecutionResult<List<GetAuthorDetalisQueryResultItem>>> Execute()
    {
        var query = _appContext.Authors.Where(x => x.IsDeleted == false && x.FirstName == FirstName).AsQueryable();
        if (query.Any())
        {
            var result =await (from auth in query
                          select new GetAuthorDetalisQueryResultItem
                          {
                              Id = auth.Id,
                              FirstName = auth.FirstName,
                              LastName = auth.LastName,
                              BirthDate = auth.BirthDate,
                              bookDetalis = (from ba in _appContext.BookAuthors
                                             join book in _appContext.Books on ba.BookId equals book.Id
                                             where ba.AuthorId == auth.Id && ba.IsDeleted == false && book.IsDeleted == false
                                             select new BookDetalisResultItem
                                             {
                                                 Id = book.Id,
                                                 BookStatus = book.BookStatus,
                                                 Description = book.Description,
                                                 PathImg = book.PathImg,
                                                 PublishDate = book.PublishDate,
                                                 Rating = book.Rating,
                                                 Title = book.Title

                                             }).ToList()
                          }).ToListAsync();
            return await Ok(result);
        }
        return await Ok(null);

    }
}
public class GetAuthorDetalisQueryResultItem
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<BookDetalisResultItem>? bookDetalis { get; set; }
}
public class BookDetalisResultItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public int BookStatus { get; set; }
}