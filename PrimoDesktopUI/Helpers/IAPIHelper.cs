using System.Threading.Tasks;
using PrimoDesktopUI.Models;

namespace PrimoDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}