using System.Collections.Generic;
using System.Threading.Tasks;
using PrimoDesktopUI.Library.Models;

namespace PrimoDesktopUI.Library.API
{
    public interface IUserEndPoint
    {
        Task<List<UserModel>> GetAll();

        Task<Dictionary<string, string>> GetAllRoles();

        Task AddUserToRole(string userId, string roleName);

        Task RemoveUserFromRole(string userId, string roleName);
    }
}