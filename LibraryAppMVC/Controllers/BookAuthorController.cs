using Application.Commands.AuthorCommands;
using Application.Commands.BookAuthorcommands;
using Application.Shared;
using Domain.Entities.FileEntity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAppMVC.Controllers
{
    public class BookAuthorController : Controller
    {

        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor; // Assuming an interface for query execution

        public BookAuthorController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }


             // POST: book/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(AddBookAuthorCommand command)
        {

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



        // GET: BookAuthorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BookAuthorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookAuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

   

        // GET: BookAuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookAuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookAuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookAuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
