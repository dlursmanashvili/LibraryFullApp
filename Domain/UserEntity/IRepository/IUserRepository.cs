using Shared;

namespace Domain.UserEntity.IRepository;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id);
    Task<List<User>> GetAllAsync();
    Task<CommandExecutionResult> Registration(User user, string RoleName, bool SendMailSetting);
    Task<CommandExecutionResult> UpdateAsyncUser(User user);
    Task<CommandExecutionResult> DeleteAsync(string id);
    Task<CommandExecutionResult> ForgetPassword(User user);
    Task<CommandExecutionResult> ResetPassword(string userId, string Password, string ConfirmationId);
  
    Task<CommandExecutionResult> ForgetPasswordByAdmin(User employee, string AdminUserID);
    Task<CommandExecutionResult> DeactiveNewUserAsync(User employee);
    Task<CommandExecutionResult> ActivateNewUser(string activateConfirmationID);
}
