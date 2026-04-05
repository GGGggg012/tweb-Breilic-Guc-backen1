using AutoMapper; // ˜˜˜˜˜˜˜˜ ˜˜˜
using eAviaSales.BusinessLayer.Core.ProductActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Functions
{
    public class ProductFlow : ProductActions, IProductAction
    {
        // ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜˜˜
        public ProductFlow(IMapper mapper) : base(mapper) { }

        public List<ProductDto> GetAllForAdmin() => GetAllProductsForAdmin();
        public List<ProductDto> GetAll() => GetAllProducts();
        public ProductDto? GetById(int id) => GetProductById(id);
        public ProductDto Create(ProductDto dto) => CreateProduct(dto);
        public ProductDto? Update(int id, ProductDto dto) => UpdateProduct(id, dto);
        public ProductDto? SetActive(int id, bool isActive) => SetProductActive(id, isActive);
        public bool Delete(int id) => DeleteProduct(id);
    }
}