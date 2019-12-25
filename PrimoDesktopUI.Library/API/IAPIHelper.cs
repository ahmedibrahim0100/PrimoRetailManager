using PrimoDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace PrimoDesktopUI.Library.API
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);

        Task GetLoggedInUserInfo(string token);
    }
}