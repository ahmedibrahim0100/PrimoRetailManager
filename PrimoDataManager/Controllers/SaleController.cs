using Microsoft.AspNet.Identity;
using PrimoDataManager.Library.DataAccess;
using PrimoDataManager.Library.Models;
using PrimoDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrimoDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userId);
        }

        //public List<ProductModel> Get()
        //{
        //    ProductData data = new ProductData();
        //    return data.GetProducts();
        //}
    }
}
