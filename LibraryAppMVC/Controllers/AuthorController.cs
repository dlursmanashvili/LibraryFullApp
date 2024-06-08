using Application.Commands.AuthorCommands;
using Application.Commands.AutrhorCommands;
using Application.Queries.AutrhorQueries;
using Application.Queries.BookQueries;
using Application.Shared;
using LibraryAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibraryAppMVC.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor; // Assuming an interface for query execution

        public AuthorController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }
        // GET: AuthorController
        [HttpGet("index")]
        public async Task<ActionResult> IndexAsync(string? name)
        {
            var query = new GetAllAuthorByNameQuery { Name = name };
            var result = await _queryExecutor.Execute<GetAllAuthorByNameQuery, List<GetAuthorQueryResultItem>?>(query);


            if (result.Success)
            {
                return View(result.Data);
            }
            return View(new List<GetAuthorQueryResultItem>());
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var query = new GetOneAuthorBooksByAuthorIDQuery { AuthorId = id };
            var result = await _queryExecutor.Execute<GetOneAuthorBooksByAuthorIDQuery, GetAuthorDetalisQueryResultItem?>(query);

            if (result.Success && result.Data != null)
            {

                var viewModel = new AuthorDetalisVievModel()
                {
                    Id = result.Data.Id,
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    BirthDate = result.Data.BirthDate,
                    bookDetails = result.Data.bookDetalis.Select(x => new BookDetails()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        PathImg = x.PathImg,
                        Rating = x.Rating,
                        PublishDate = x.PublishDate,
                        BookinLibrary = x.BookinLibrary,
                    }).ToList()
                };              

                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(AddAutrhorCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command); // Return with validation errors
            }

            var result = await _commandExecutor.Execute(command);

            if (result.Success)
            {
                // Redirect to a success page or login page
                return RedirectToAction("Index", "Author");
            }
            else
            {
                // Add error message to the model state and return the view
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(command);
            }
        }

        [HttpGet("edit/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthorController/Edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditAutrhorCommand command)
        {
            try
            {
                var result = await _commandExecutor.Execute(command);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Author");
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


        [HttpGet("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteAutrhorCommand() { Id = id }; // Create a DeleteBookCommand with the provided book ID

                var result = await _commandExecutor.Execute(command);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Author");
                }
                else
                {
                    // Handle the case where the deletion was not successful
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    // Redirect to the delete view to display error messages
                    return RedirectToAction("Delete", "Author", new { id });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                ModelState.AddModelError(string.Empty, ex.Message);
                // Redirect to the delete view to display error messages
                return RedirectToAction("Delete", "Author", new { id });
            }
        }
    }
}
