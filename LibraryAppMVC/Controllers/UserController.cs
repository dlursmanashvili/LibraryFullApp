using Application.Commands.UserCommands;
using Application.Queries.UserQuery;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using static Application.Queries.UserQuery.LoginQuery;

namespace LibraryAppMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor; // Assuming an interface for query execution

        public UserController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command); // Return with validation errors
            }

            var result = await _commandExecutor.Execute(command);

            if (result.Success)
            {
                // Redirect to a success page or login page
                return RedirectToAction("RegistrationSuccess");
            }
            else
            {
                // Add error message to the model state and return the view
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(command);
            }
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginQuery query)
        {
            if (query.Email == null && query.Password == null) // Handle potential null query object
            {
                return View(); // Return login view (replace with appropriate action)
            }

            var loginResult = await _queryExecutor.Execute<LoginQuery, LoginQueryResult>(query);

            if (loginResult.Success)
            {
                // Login successful (redirect to protected area, store token)
                return RedirectToAction("Index", "Book");  // Replace with desired action
            }
            else
            {
                // Login failed (add error to ModelState, return login view)
                //ModelState.AddModelError(string.Empty, loginResult.ErrorMessage);
                return View();  // Return login view (replace with appropriate action)
            }
        }
    }
}
