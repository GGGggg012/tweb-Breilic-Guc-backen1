using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductAction _products;

        public ProductController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _products = bl.GetProductAction();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll() => Ok(_products.GetAll());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            var product = _products.GetById(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create([FromBody] ProductDto dto) => StatusCode(201, _products.Create(dto));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int id, [FromBody] ProductDto dto)
        {
            var result = _products.Update(id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPatch("{id}/active")]
        [Authorize(Roles = "Admin")]
        public IActionResult SetActive(int id, [FromBody] SetActiveDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = _products.SetActive(id, dto.IsActive);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) => _products.Delete(id) ? NoContent() : NotFound();
    }
}
