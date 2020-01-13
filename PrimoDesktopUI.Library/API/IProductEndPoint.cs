using System.Collections.Generic;
using System.Threading.Tasks;
using PrimoDesktopUI.Library.Models;

namespace PrimoDesktopUI.Library.API
{
    public interface IProductEndPoint
    {
        Task<List<ProductModel>> GetAll();
    }
}