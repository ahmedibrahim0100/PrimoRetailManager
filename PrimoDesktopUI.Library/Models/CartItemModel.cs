using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimoDesktopUI.Library.Models
{
    public class CartItemModel
    {
        public ProductModel Product { get; set; }

        public int QuantityInCart { get; set; }



        //No further need for this property (DisplayText) because we already now have a CartItemDisplayModel in 
        //(cont.) PrimoDesktopUI.Models which has this property..

        //public string DisplayText
        //{
        //    get
        //    {
        //        return $"{Product.ProductName}({QuantityInCart})";
        //    }
        //}
    }
}
