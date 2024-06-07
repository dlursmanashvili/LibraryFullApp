using Application.Queries.BookQueries;
using Application.Shared;
using Shared;

namespace Application.Queries.AutrhorQueries;

public class GetOnebookAuthorsInfoQuery : Query<GetbookDetalisQueryResultItem?>
{
    public int BookId { get; set; }
    public override async Task<QueryExecutionResult<GetbookDetalisQueryResultItem?>> Execute()
    {
        var query = _appContext.Books.FirstOrDefault(x => x.IsDeleted == false && x.Id == BookId);
        if (query.IsNotNull())
        {


            var result = new GetbookDetalisQueryResultItem
            {
                Id = query.Id,
                Title = query.Title,
                Description = query.Description,
                PathImg = query.PathImg,
                Rating = query.Rating,
                PublishDate = query.PublishDate,
                BookinLibrary = query.BookinLibrary,
                AuthorDetails = (from ba in _appContext.BookAuthors
                                 join author in _appContext.Authors on ba.AuthorId equals author.Id
                                 where ba.BookId == query.Id && ba.IsDeleted == false && author.IsDeleted == false
                                 select new AuthorDelatisQueryResulItem
                                 {
                                     FirstName = author.FirstName,
                                     LastName = author.LastName,
                                     BirthDate = author.BirthDate
                                 }).ToList()
            };
            return await Ok(result);
        }


        return await Ok(null);
    }
}


