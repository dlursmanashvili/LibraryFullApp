using Application.Queries.AutrhorQueries;
using Application.Shared;
using Shared;

namespace Application.Queries.BookQueries;

public class GetOneAuthorBooksByAuthorIDQuery : Query<GetAuthorDetalisQueryResultItem?>
{
    public int AuthorId { get; set; }
    public override async Task<QueryExecutionResult<GetAuthorDetalisQueryResultItem?>> Execute()
    {

        var auth = _appContext.Authors.FirstOrDefault(x => x.IsDeleted == false && x.Id == AuthorId);
        if (auth.IsNotNull())
        {
            var result = new GetAuthorDetalisQueryResultItem()
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
                                   BookinLibrary = book.BookinLibrary,
                                   Description = book.Description,
                                   PathImg = book.PathImg,
                                   PublishDate = book.PublishDate,
                                   Rating = book.Rating,
                                   Title = book.Title

                               }).ToList()};

            return await Ok(result);
        }
        return await Ok(null);
    }
}
