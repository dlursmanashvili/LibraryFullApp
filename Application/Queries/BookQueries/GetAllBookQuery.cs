using Application.Queries.AutrhorQueries;
using Application.Shared;
using Shared;

namespace Application.Queries.BookQueries;

public class GetAllBookQuery : Query<List<GetBookQueryResultItem>?>
{
    public override async Task<QueryExecutionResult<List<GetBookQueryResultItem>?>> Execute()
    {
        var result = _appContext.Books.Where(x => x.IsDeleted == false).AsQueryable();

        if (result.IsNotNull())
        {
            return await Ok(result.Select(x => new GetBookQueryResultItem
            {
                Id = x.Id,
               Title = x.Title,
               Rating = x.Rating,
               PublishDate = x.PublishDate,
               PathImg = x.PathImg,
               Description = x.Description,
               BookStatus = x.BookStatus,               
            }).ToList());
        }
        return await Ok(null);
    }
}
public class GetBookQueryResultItem
{
    public string Title { get; set; }
    public int Id { get; set; }
    public string? Description { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public int BookStatus { get; set; }
}
