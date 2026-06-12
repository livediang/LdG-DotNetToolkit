using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCRUD.Application.DTOs;
using ProductCRUD.Domain.Entities;
using ProductCRUD.Domain.Interfaces;
using ProductCRUD.Domain.Pagination;

namespace ProductCRUD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var repo = _uow.Repository<Product>();
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] PagedRequest req)
        {
            var repo = _uow.Repository<Product>();
            var paged = await repo.QueryAsync(req);
            var dtos = paged.Items.Select(p => _mapper.Map<ProductDto>(p));
            return Ok(new PagedResult<ProductDto> { Items = dtos, Total = paged.Total, Page = paged.Page, PageSize = paged.PageSize });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            var repo = _uow.Repository<Product>();
            var product = _mapper.Map<Product>(dto);
            await repo.AddAsync(product);
            await _uow.CommitAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, _mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto dto)
        {
            var repo = _uow.Repository<Product>();
            var existing = await repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Map fields
            _mapper.Map(dto, existing);
            repo.Update(existing);

            try
            {
                await _uow.CommitAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Concurrency conflict" });
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var repo = _uow.Repository<Product>();
            var existing = await repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.IsDeleted = true; // soft delete
            repo.Update(existing);
            await _uow.CommitAsync();
            return NoContent();
        }
    }
}
