using Application.Commands.UserCommands;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryAppMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ICommandExecutor _commandExecutor;

        public UserController(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
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
    }
}
