using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly ICart _cart;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IStringLocalizer<ProductService> _localizer;

        public ProductService(ICart cart, IProductRepository productRepository,
            IOrderRepository orderRepository, IStringLocalizer<ProductService> localizer)
        {
            _cart = cart;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _localizer = localizer;
        }

        public List<ProductViewModel> GetAllProductsViewModel()
        {
            IEnumerable<Product> productEntities = GetAllProducts();
            return MapToViewModel(productEntities);
        }

        private static List<ProductViewModel> MapToViewModel(IEnumerable<Product> productEntities)
        {
            var products = new List<ProductViewModel>();
            foreach (var product in productEntities)
            {
                products.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Stock = product.Quantity.ToString(),
                    Price = product.Price.ToString(CultureInfo.InvariantCulture),
                    Name = product.Name,
                    Description = product.Description,
                    Details = product.Details
                });
            }

            return products;
        }

        public List<Product> GetAllProducts()
        {
            var productEntities = _productRepository.GetAllProducts();
            return productEntities?.ToList();
        }

        public ProductViewModel GetProductByIdViewModel(int id)
        {
            var products = GetAllProductsViewModel().ToList();
            return products.Find(p => p.Id == id);
        }

        public Product GetProductById(int id)
        {
            var products = GetAllProducts().ToList();
            return products.Find(p => p.Id == id);
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _productRepository.GetProduct(id);
            return product;
        }

        public async Task<IList<Product>> GetProduct()
        {
            var products = await _productRepository.GetProduct();
            return products;
        }

        public void UpdateProductQuantities()
        {
            var cart = (Cart)_cart;
            foreach (var line in cart.Lines)
            {
                _productRepository.UpdateProductStocks(line.Product.Id, line.Quantity);
            }
        }

        /// <summary>
        /// Validates a ProductViewModel.
        /// </summary>
        public List<string> CheckProductModelErrors(ProductViewModel product)
        {
            var modelErrors = new List<string>();

            if (product == null)
            {
                modelErrors.Add(_localizer["ProductNull"]);
                return modelErrors;
            }
            
            if (product.Name == null || string.IsNullOrWhiteSpace(product.Name))
            {
                modelErrors.Add(_localizer["MissingName"]);
            }
            else
            {
                if (Regex.IsMatch(product.Name, @"\p{C}+")) // https://stackoverflow.com/a/40568888
                {
                    modelErrors.Add(_localizer["NameIllegalCharacter"]);
                }

                if (product.Name.Length <= 3)
                {
                    modelErrors.Add(_localizer["NameTooShort"]);
                }

                if (product.Name.Length > 100)
                {
                    modelErrors.Add(_localizer["NameTooLong"]);
                }
            }

            if (product.Price == null || string.IsNullOrWhiteSpace(product.Price))
            {
                modelErrors.Add(_localizer["MissingPrice"]);
            }

            if (!double.TryParse(product.Price, out var pc))
            {
                modelErrors.Add(_localizer["PriceNotANumber"]);
            }
            else
            {
                if (pc <= 0)
                    modelErrors.Add(_localizer["PriceNotGreaterThanZero"]);
            }

            if (product.Stock == null || string.IsNullOrWhiteSpace(product.Stock))
            {
                modelErrors.Add(_localizer["MissingStock"]);
            }

            if (!int.TryParse(product.Stock, out var qt))
            {
                modelErrors.Add(_localizer["StockNotAnInteger"]);
            }
            else
            {
                if (qt <= 0)
                    modelErrors.Add(_localizer["StockNotGreaterThanZero"]);
            }

            if (product.Details == null || string.IsNullOrWhiteSpace(product.Details))
            {
                modelErrors.Add(_localizer["MissingDetails"]);
            }
            else
            {
                if (Regex.IsMatch(product.Details, @"\p{C}+")) // https://stackoverflow.com/a/40568888
                {
                    modelErrors.Add(_localizer["DetailsIllegalCharacter"]);
                }

                if (product.Details.Length < 10)
                {
                    modelErrors.Add(_localizer["DetailsTooShort"]);
                }

                if (product.Details.Length > 200)
                {
                    modelErrors.Add(_localizer["DetailsTooLong"]);
                }
            }

            if (product.Description == null || string.IsNullOrWhiteSpace(product.Description))
            {
                modelErrors.Add(_localizer["MissingDescription"]);
            }
            else
            {
                if (Regex.IsMatch(product.Description, @"\p{C}+")) // https://stackoverflow.com/a/40568888
                {
                    modelErrors.Add(_localizer["DescriptionIllegalCharacter"]);
                }
                if (product.Description.Length < 10)
                {
                    modelErrors.Add(_localizer["DescriptionTooShort"]);
                }

                if (product.Description.Length > 100)
                {
                    modelErrors.Add(_localizer["DescriptionTooLong"]);
                }
            }

            return modelErrors;
        }

        public void SaveProduct(ProductViewModel product)
        {
            var productToAdd = MapToProductEntity(product);
            _productRepository.SaveProduct(productToAdd);
        }

        private static Product MapToProductEntity(ProductViewModel product)
        {
            var productEntity = new Product
            {
                Name = product.Name,
                Price = double.Parse(product.Price),
                Quantity = int.Parse(product.Stock),
                Description = product.Description,
                Details = product.Details
            };

            return productEntity;
        }

        public void DeleteProduct(int id)
        {
            // TODO what happens if a product has been added to a cart and has been later removed from the inventory ?
            // delete the product form the cart by using the specific method
            // => the choice is up to the student
            _cart.RemoveLine(GetProductById(id));

            _productRepository.DeleteProduct(id);
        }
    }
}