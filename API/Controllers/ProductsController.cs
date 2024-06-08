using API.DTO;
using API.Errors;
using AutoMapper;
using Core.Entites;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _brandRepository;
        private readonly IGenericRepository<ProductType> _typeRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository,
            IGenericRepository<ProductBrand> brandRepository,
            IGenericRepository<ProductType> typeRepository,
            IMapper mapper)
        {
            _productRepository = productRepository ??
                throw new ArgumentNullException(nameof(productRepository));
            _brandRepository = brandRepository ??
                throw new ArgumentNullException(nameof(brandRepository));
            _typeRepository = typeRepository ??
                throw new ArgumentNullException(nameof(typeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _productRepository.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepository.GetEntityWithSpecAsync(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _brandRepository.GetAllAsync();

            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _typeRepository.GetAllAsync();

            return Ok(types);
        }
    }
}