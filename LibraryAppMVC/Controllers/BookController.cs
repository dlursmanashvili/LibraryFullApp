using Application.Commands.AuthorCommands;
using Application.Commands.BookAuthorcommands;
using Application.Queries.AutrhorQueries;
using Application.Queries.BookQueries;
using Application.Shared;
using Domain.Entities.FileEntity;
using LibraryAppMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAppMVC.Controllers
{
    [Route("book")]
    public class BookController : Controller
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor; // Assuming an interface for query execution

        public BookController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }



        [HttpPost("createBookAuThor")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> createBookAuThor(AddBookAuthorCommand command)
        {

            if (command.BookId == 0 || command.AuthorIds.Any(x => x == 0))
            {
                return RedirectToAction("Index", "Book");
            }

            var result = await _commandExecutor.Execute(command);

            if (result.Success)
            {
                // Redirect to a success page or login page
                return RedirectToAction("Index", "Book");
            }
            else
            {
                // Add error message to the model state and return the view
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(command);
            }
        }


        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditBookCommand command)
        {

            try
            {
                var result = await _commandExecutor.Execute(command);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Book");
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    return View(); // You may want to return to the same view with the error message
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(); // You may want to return to the same view with the error message
            }
        }
        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var query = new GetOnebookAuthorsInfoQuery { BookId = id };
            var result = await _queryExecutor.Execute<GetOnebookAuthorsInfoQuery, GetbookDetalisQueryResultItem?>(query);

            if (result.Success && result.Data != null)
            {
                var viewModel = new BookDetailsViewModel
                {
                    Id = result.Data.Id,
                    Title = result.Data.Title,
                    Description = result.Data.Description,
                    PathImg = result.Data.PathImg,
                    Rating = result.Data.Rating,
                    PublishDate = result.Data.PublishDate,
                    BookinLibrary = result.Data.BookinLibrary,
                    AuthorDetails = result.Data.AuthorDetails.Select(a => new AuthorDetailsViewModel
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        BirthDate = a.BirthDate
                    }).ToList(),
                    AuthorForDropDown = result.Data.NoBookAuthor.Select(a => new AuthorDetailsViewModel
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        BirthDate = a.BirthDate
                    }).ToList()
                };

                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: book/index
        [HttpGet("index")]
        public async Task<ActionResult> IndexAsync(string? title)
        {
            var query = new GetAllBookQuery { Title = title };
            var result = await _queryExecutor.Execute<GetAllBookQuery, List<GetBookQueryResultItem>?>(query);

            if (result.Success)
            {
                return View(result.Data);
            }
            return View(new List<GetBookQueryResultItem>());
        }


        // GET: book/create
        [HttpGet("create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: book/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(AddBookCommand command, IFormFile fileUpload)
        {

            if (fileUpload != null && fileUpload.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await fileUpload.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    command.File = new FileClass
                    {
                        base64String = Convert.ToBase64String(fileBytes),
                        ext = Path.GetExtension(fileUpload.FileName)
                    };
                }
            }
            //if (!ModelState.IsValid)
            //{
            //    return View(command); // Return with validation errors
            //}

            var result = await _commandExecutor.Execute(command);

            if (result.Success)
            {
                // Redirect to a success page or login page
                return RedirectToAction("Index", "Book");
            }
            else
            {
                // Add error message to the model state and return the view
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(command);
            }
        }

        // GET: book/edit/{id}
        [HttpGet("edit/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost("deleteBookAuthor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookAuthor(int BookId, List<int> AuthorIds)
        {
            if (BookId == 0 || AuthorIds.Any(x=> x == 0))
            {
                return RedirectToAction("Index", "Book");
            }
            try
            {
                var command = new DeleteBookAuthorcommand
                {
                    BookId = BookId,
                    AuthorIds = AuthorIds
                };

                var result = await _commandExecutor.Execute(command);

                if (result.Success)
                {
                    return RedirectToAction("Details", new { id = BookId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    return RedirectToAction("Details", new { id = BookId });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Details", new { id = BookId });
            }
        }
        //// GET: book/delete/{id}
        //[HttpGet("deleteBookAuthor/{id}")]
        //public async Task<IActionResult> DeleteBookAuthor(int BookId, List<int> AuthorIds)
        //{
        //    try
        //    {
        //        var command = new DeleteBookAuthorcommand
        //        {
        //            BookId = BookId,
        //            AuthorIds = AuthorIds
        //        };

        //        var result = await _commandExecutor.Execute(command);

        //        if (result.Success)
        //        {
        //            return RedirectToAction("Index", "Book");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, result.ErrorMessage);
        //            return RedirectToAction("Index", "Book");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //        return RedirectToAction("Index", "Book");
        //    }
        //}
        //public async Task<ActionResult> deleteBookAuthor(int id)
        //{
        //    try
        //    {
        //        var command = new DeleteBookAuthorcommand() { Id = id }; // Create a DeleteBookCommand with the provided book ID

        //        var result = await _commandExecutor.Execute(command);

        //        if (result.Success)
        //        {
        //            return RedirectToAction("Index", "Book");
        //        }
        //        else
        //        {
        //            // Handle the case where the deletion was not successful
        //            ModelState.AddModelError(string.Empty, result.ErrorMessage);
        //            // Redirect to the delete view to display error messages
        //            return RedirectToAction("index", "Book", new { id });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that occur during the deletion process
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //        // Redirect to the delete view to display error messages
        //        return RedirectToAction("index", "Book", new { id });
        //    }
        //}

        // GET: book/delete/{id}
        [HttpGet("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteBookCommand() { Id = id }; // Create a DeleteBookCommand with the provided book ID

                var result = await _commandExecutor.Execute(command);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Book");
                }
                else
                {
                    // Handle the case where the deletion was not successful
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    // Redirect to the delete view to display error messages
                    return RedirectToAction("Delete", "Book", new { id });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                ModelState.AddModelError(string.Empty, ex.Message);
                // Redirect to the delete view to display error messages
                return RedirectToAction("Delete", "Book", new { id });
            }
        }
    }
}
