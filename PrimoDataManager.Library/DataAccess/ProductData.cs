using PrimoDataManager.Library.Internal.DataAccess;
using PrimoDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimoDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<ProductModel, dynamic>("spProduct_GetAll", new { }, "PrimoData");
            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<ProductModel, dynamic>("spProduct_GetById", new { Id = productId }, "PrimoData")
                .FirstOrDefault();
            return output;
        }
    }
}
