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


            var bookAuthors = (from ba in _appContext.BookAuthors
                               join author in _appContext.Authors on ba.AuthorId equals author.Id
                               where ba.BookId == query.Id && ba.IsDeleted == false && author.IsDeleted == false
                               select new AuthorDelatisQueryResulItem
                               {
                                   Id = author.Id,
                                   FirstName = author.FirstName,
                                   LastName = author.LastName,
                                   BirthDate = author.BirthDate
                               }).ToList();

            var authorBookId = bookAuthors.Select(x => x.Id);

            var result = new GetbookDetalisQueryResultItem
            {
                Id = query.Id,
                Title = query.Title,
                Description = query.Description,
                PathImg = query.PathImg,
                Rating = query.Rating,
                PublishDate = query.PublishDate,
                BookinLibrary = query.BookinLibrary,
                AuthorDetails = bookAuthors,
                NoBookAuthor = authorBookId.IsNotNull() ?
                _appContext.Authors
                            .Where(x =>x.IsDeleted == false && !authorBookId.Contains(x.Id))
                            .Select(author => new AuthorDelatisQueryResulItem()
                            {
                                Id = author.Id,
                                FirstName = author.FirstName,
                                LastName = author.LastName,
                                BirthDate = author.BirthDate
                            }).ToList() :
                             _appContext.Authors
                                        .Where(x => x.IsDeleted == false)
                                        .Select(author => new AuthorDelatisQueryResulItem()
                                        {
                                            Id = author.Id,
                                            FirstName = author.FirstName,
                                            LastName = author.LastName,
                                            BirthDate = author.BirthDate
                                        }).ToList()
                            ,
            };
            return await Ok(result);
        }


        return await Ok(null);
    }
}


