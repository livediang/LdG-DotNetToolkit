using CRUD_EF_Core_API.Features.ObjGenres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_EF_Core_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController(GenresServices _genresServices) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _genresServices.GetAll();

            return Ok(new { Message = "Genres retrieved successfully", genres });
        }
    }
}
