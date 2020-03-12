using PrimoDataManager.Library.Internal.DataAccess;
using PrimoDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimoDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this DRY/SOLID/Better

            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            //Start filling the sale detail models we will save in the database

            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get the information about this product
                //Question: Why are we getting productInfo from DB?
                //Answer: To validate calculations of taxes and purchace price at server side to
                //get protection from probable bad behaviour of some users
                 
                var productInfo = products.GetProductById(detail.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The Product Id of {detail.ProductId} could not be found in the database");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }
                details.Add(detail);
            }

            //Fill in the available information

            //Create the sale model

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;


            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("PrimoData");

                    //Save the sale model
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    //Get the Id from the sale model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup",
                        new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                    //Finish Filling in the sale detail models
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;

                        //Save the sale detail models
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollBackTransaction();
                    throw;
                }
            }
        }



        //public List<ProductModel> GetProducts()
        //{
        //    SqlDataAccess sql = new SqlDataAccess();
        //    var output = sql.LoadData<ProductModel, dynamic>("spProduct_GetAll", new { }, "PrimoData");
        //    return output;
        //}
    }
}
