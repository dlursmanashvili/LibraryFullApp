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

        public IActionResult Registration()
        {
            return View();
        }

      
    }
}
