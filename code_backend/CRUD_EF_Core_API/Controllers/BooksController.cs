using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRUD_EF_Core_API.Features.ObjBooks;

namespace CRUD_EF_Core_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(BooksServices _booksServices) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _booksServices.GetAll();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _booksServices.GetById(id);

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody]CreateBook request)
        {
            await _booksServices.Add(request);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBook request)
        {
            await _booksServices.Update(request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _booksServices.Delete(id);

            return Ok();
        }
    }
}