using Application.Commands.AuthorCommands;
using Application.Queries.AutrhorQueries;
using Application.Queries.BookQueries;
using Application.Shared;
using Domain.Entities.FileEntity;
using LibraryAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            var query = new GetbookDetalisById { BookId = id };
            var result = await _queryExecutor.Execute<GetbookDetalisById, GetbookDetalisQueryResultItem?>(query);

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
        public async Task<ActionResult> IndexAsync()
        {
            var query = new GetAllBookQuery();
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

        // POST: book/edit/{id}


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
