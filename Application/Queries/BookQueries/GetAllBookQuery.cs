using Application.Queries.AutrhorQueries;
using Application.Shared;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.Xml.Linq;

namespace Application.Queries.BookQueries;

public class GetAllBookQuery : Query<List<GetBookQueryResultItem>?>
{
    public string? Title { get; set; }

    public override async Task<QueryExecutionResult<List<GetBookQueryResultItem>?>> Execute()
    {

        var result = Title.IsNullOrEmpty() ? _appContext.Books.Where(x => x.IsDeleted == false).AsQueryable() : _appContext.Books.Where(x => x.IsDeleted == false && x.Title.Contains(Title)).AsQueryable();
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
                BookinLibrary = x.BookinLibrary,               
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
    public string? PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public bool BookinLibrary { get; set; }
}
