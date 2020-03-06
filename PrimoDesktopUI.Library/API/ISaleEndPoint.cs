using System.Threading.Tasks;
using PrimoDesktopUI.Library.Models;

namespace PrimoDesktopUI.Library.API
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}