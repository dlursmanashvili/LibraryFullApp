using Domain.MailModel;
using Domain.MailModel.IRepository;
using Domain.UserEntity;
using Domain.UserEntity.IRepository;
using Infrastructure.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared;
using System.Net.Mail;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    UserManager<User> _userManager;
    private readonly IConfiguration _config;
    private readonly ISendNotification _sendNotification;

    public UserRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider, UserManager<User> userManager, IConfiguration config, ISendNotification sendNotification)
           : base(applicationDbContext, serviceProvider)
    {
        _userManager = userManager;
        _config = config;
        _sendNotification = sendNotification;
    }

    public async Task<CommandExecutionResult> ActivateNewUser(string activateConfirmationID)
    {
        var employee = _ApplicationDbContext.Users.FirstOrDefault(x => x.ConfirmationId == activateConfirmationID);
        if (employee.IsNull())
        {
            return new CommandExecutionResult() { ErrorMessage = "confirmationId not valid , User not found   ", Success = false };
        }

        employee.IsActive = true;
        return await this.UpdateAsyncUser(employee);
    }

    public async Task<CommandExecutionResult> DeactiveNewUserAsync(User employee)
    {
        string secretPass = Guid.NewGuid().ToString();


        var userUpdateResult = await this.UpdateAsyncUser(employee);

        string subject = "Activate User";
        string link = $"{_config.GetSection("SendVerifyMailLinks:HostingAddress").Value}activatenewuser?confirmationId={secretPass}";

        string htmlBody = "<html>" +
                                 "<body>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<h1><strong>მომხმარებლის გააქტიურება</strong><h1>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<a href='" + link + "' target='_blank'>მომხმარებლის გასააქტიურებლად მიყევით ლინკს</a>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<p>მადლობა თანამშრომლობისთვის,</p>" +
                                    "<p>Tacsa</p>" +
                                "</body>" +
                               "</html>";

        AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

        var sendMailResult = await _sendNotification.SendMail(new SendMailRequest()
        {
            userMail = employee.Email,
            subject = subject,
            AlternateView = alternateView
        });
        if (sendMailResult.Success == false)
        {
            return sendMailResult;
        }

        if (userUpdateResult.Success == false)
        {
            return userUpdateResult;
        }

        return sendMailResult;
    }

    public async Task<CommandExecutionResult> ForgetPassword(User employee)
    {
        string subject = "პაროლის ცვლილება";
        string secretPass = Guid.NewGuid().ToString();
        string link = $"{_config.GetSection("SendVerifyMailLinks:HostingAddress").Value}ForgetPassword?param1={employee.Id}&param2={secretPass}";

        string htmlBody = "<html>" +
                                 "<body>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<h1><strong>პაროლის აღდგენა</strong><h1>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<a href='" + link + "' target='_blank'>პაროლის აღდგენისათვის მიყევით ბმულს</a>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<p>მადლობა თანამშრომლობისთვის,</p>" +
                                    "<p>Tacsa</p>" +
                                "</body>" +
                               "</html>";

        AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

        var sendMailResult = await _sendNotification.SendMail(new SendMailRequest()
        {
            userMail = employee.Email,
            subject = subject,
            AlternateView = alternateView
        });
        if (sendMailResult.Success == false)
        {
            return sendMailResult;
        }
        employee.ConfirmationId = secretPass;
        var userUpdateResult = await this.UpdateAsyncUser(employee);
        return sendMailResult;
    }

    public async Task<CommandExecutionResult> ForgetPasswordByAdmin(User employee, string AdminUserID)
    {
        string secretPass = Guid.NewGuid().ToString();
        string subject = "Reset Password";
        string link = $"{_config.GetSection("SendVerifyMailLinks:HostingAddress").Value}ForgetPassword/{secretPass}";

        string htmlBody = "<html>" +
                                 "<body>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<h1><strong>პაროლის აღდგენა</strong><h1>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<a href='" + link + "' target='_blank'>პაროლის აღდგენისათვის მიყევით ბმულს</a>" +
                                    "<br/>" +
                                    "<br/>" +
                                    "<p>მადლობა რეგისტრაციისათვის,</p>" +
                                    "<p>Tacsa</p>" +
                                "</body>" +
                               "</html>";

        AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

        var sendMailResult = await _sendNotification.SendMail(new SendMailRequest()
        {
            userMail = _ApplicationDbContext.Users.First(x => x.Id == AdminUserID).Email,
            subject = subject,
            AlternateView = alternateView
        });
        if (sendMailResult.Success == false)
        {
            return sendMailResult;
        }
        employee.ConfirmationId = secretPass;
        var userUpdateResult = await this.UpdateAsyncUser(employee);

        if (userUpdateResult.Success == false)
        {
            return userUpdateResult;
        }

        return sendMailResult;
    }

    public async Task<CommandExecutionResult> Registration(User user, string RoleName, bool SendMailSetting)
    {try
        {
        if (user == null)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Invalid user object" };
        }

            if (_ApplicationDbContext.Users.FirstOrDefault(x => x.IsActive && (x.Email == user.Email || x.UserName == user.UserName)).IsNotNull())
            {
                return new CommandExecutionResult() { Success = false, ErrorMessage = "მომხმარებლის სახელი უკვე დაკავებულია" };
            }

            if (await _userManager.FindByEmailAsync(user.Email) != null)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = $"მომხმარებლის მეილი {user.Email} უკვე დაკავებულია" };
        }
        var NewRole = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Name == RoleName);
        if (NewRole.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Invalid Role Name" };
        }

        
            if (_userManager == null)
            {
                return new CommandExecutionResult() { Success = false, ErrorMessage = "User Manager is not initialized" };
            }
            user.Id = Guid.NewGuid().ToString();
            string hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, user.PasswordHash);
            user.PasswordHash = hashedNewPassword;

            user.IsActive = false;

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded == false)
            {
                return new CommandExecutionResult() { Success = false, ErrorMessage = result.Errors.ToString() };
            }


            var roleResult = await _userManager.AddToRoleAsync(_ApplicationDbContext.Users.First(x => x.Email == user.Email), NewRole.Name);
            if (roleResult.Succeeded == false)
            {
                return new CommandExecutionResult() { Success = false, ErrorMessage = roleResult.Errors.ToString() };
            }

            await _ApplicationDbContext.SaveChangesAsync();

            if (SendMailSetting)
            {
                var mailSendResult = await this.DeactiveNewUserAsync(user);
            }
            else
            {
                user.IsActive = true;
            }


            return new CommandExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult()
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<CommandExecutionResult> UpdateAsyncUser(User user)
    {
        var employe = await _userManager.FindByIdAsync(user.Id.ToString());

        if (employe.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "User not found" };
        }

        try
        {
            employe.PNumber = user.PNumber;
            employe.FirstName = user.FirstName;
            employe.LastName = user.LastName;
            employe.PhoneNumber = user.PhoneNumber;
            employe.IsActive = user.IsActive;
            employe.IsOorganisation = user.IsOorganisation;

            await _userManager.UpdateAsync(employe);

            return new CommandExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult()
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<CommandExecutionResult> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "User not found" };
        }

        user.IsActive = false;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return new CommandExecutionResult() { Success = true };
        }
        else
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = string.Join(", ", result.Errors) };
        }
    }

    public async Task<User> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public Task<List<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<CommandExecutionResult> ResetPassword(string userId, string Password, string ConfirmationId)
    {
        var user = _ApplicationDbContext.Users.FirstOrDefault(x => x.Id == userId && x.ConfirmationId == ConfirmationId);

        if (user.IsNull() || user.IsActive == false) return new CommandExecutionResult() { ErrorMessage = "მომხმარებელი არააქტიურია", Success = false };
        string hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, Password);

        user.PasswordHash = hashedNewPassword;
        user.ConfirmationId = null;
        await _userManager.UpdateAsync(user);
        await _ApplicationDbContext.SaveChangesAsync();

        if (_ApplicationDbContext.Users.First(x => x.PasswordHash == hashedNewPassword).IsNull())
        {
            return new CommandExecutionResult() { ErrorMessage = "Passwrord change error", Success = false };
        }


        return new CommandExecutionResult() { Success = true };
    }

    public async Task<(List<User>? resultData, CommandExecutionResult resultCommand)> GetAllAsyncInternalUser()
    {
        var RootAdminRole = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Id == "2c5e174e-3b0e-446f-86af-483d56fd7210");
        var roleUser = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Id == "847aeaad-ec0e-4b85-b20e-1828fae116c9");

        if (RootAdminRole.IsNull())
        {
            return (null, new CommandExecutionResult() { ErrorMessage = "Root admin Role Not Found", Success = false });
        }

        if (RootAdminRole.IsNull())
        {
            return (null, new CommandExecutionResult() { ErrorMessage = "User Role Not Found", Success = false });
        }

        var FiltredUsersRoles = _ApplicationDbContext.UserRoles.Where(x => x.RoleId != RootAdminRole.Id && x.RoleId != roleUser.Id);
        if (FiltredUsersRoles.IsNull())
        {
            return (null, new CommandExecutionResult() { Success = true });
        }

        //var filtredUsers = await _ApplicationDbContext.Users.Where(x => x.IsActive == true && FiltredUsersRoles.Any(c => c.UserId == x.Id))?.ToListAsync();

        var filteredUsers = _ApplicationDbContext.Users
        .Where(x => x.IsActive == true && FiltredUsersRoles.Any(c => c.UserId == x.Id))
        .Select(user => new
        {
            User = user,
            RoleNames = _ApplicationDbContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_ApplicationDbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToList()
        })
        .ToList();

        // Here you would need to map the anonymous type to your User type, assuming User has a List<string> for RoleNames.
        var usersWithRoles = filteredUsers.Select(f =>
        {
            var user = f.User;
            user.PasswordHash = f.RoleNames[0]; // assuming User has a List<string> for RoleNames
            return user;
        }).ToList();

        return (usersWithRoles, new CommandExecutionResult() { Success = true });
    }



}