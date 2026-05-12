using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface IProductAction
    {
        List<ProductDto> GetAllForAdmin();
        List<ProductDto> GetAll();
        ProductDto? GetById(int id);
        ProductDto Create(ProductDto dto);
        ProductDto? Update(int id, ProductDto dto);
        ProductDto? SetActive(int id, bool isActive);
        bool Delete(int id);
    }
}
