﻿using AutoMapper;
using Caliburn.Micro;
using PrimoDesktopUI.Library.API;
using PrimoDesktopUI.Library.Helpers;
using PrimoDesktopUI.Library.Models;
using PrimoDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimoDesktopUI.ViewModels
{
    public class SalesViewModel: Screen
    {
        IProductEndPoint _productEndPoint;
        ISaleEndPoint _saleEndPoint;
        IConfigHelper _configHelper;
        IMapper _mapper;

        public SalesViewModel(IProductEndPoint productEndPoint, ISaleEndPoint saleEndPoint, IConfigHelper configHelper, IMapper mapper)
        {
            _productEndPoint = productEndPoint;
            _saleEndPoint = saleEndPoint;
            _configHelper = configHelper;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded (object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _products;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                //Make sure something is selected
                //Make sure there is an item quantity
                if(ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                ////HACK - There should be a better way of refreshing the cart display
                //Cart.Remove(existingItem);
                //Cart.Add(existingItem);
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();

            //TODO - Add clearing the SelectedCartItem if it doesn't do it itself

            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }


        private CartItemDisplayModel _selectedCartItem;

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }


        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                //Make sure something is selected
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;
            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public string SubTotal
        {
            get
            {
                //TODO - Replace with calculations
                return CalculateSubTotal().ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                //TODO - Replace with calculations
                return CalculateTax().ToString();
            }
        }

        public string Total
        {
            get
            {
                //TODO - Replace with calculations
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString();
            }
        }

  
        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                //Make sure there is something in the cart
                if(Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }
        
        public async Task CheckOut()
        {
            //Create a sale model & post to the api
            SaleModel sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }
            await _saleEndPoint.PostSale(sale);
            await ResetSalesViewModel();
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }

            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;
            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            //Old code using Foreach

            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
            //    }
            //}
            return taxAmount;
        }
    }
}
