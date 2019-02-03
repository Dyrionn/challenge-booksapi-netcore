using Challenge.NetCore.Books.Api.Services.Interface;
using Challenge.NetCore.Books.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Challenge.NetCore.Books.Api.Controllers {
    [Route("api/")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Creates an book entity and returns it on response body adding the "how" to access the resource into the "Location" header
        /// </summary>
        /// <response code="201">Created</response>
        /// <param name="value">Book entity to add</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [HttpPost]
        [Route("book/")]
        public CreatedResult Post([FromBody] Book value)
        {
            var result = _bookService.AddOne(value);

            var path = string.Format("/api/books/{0}", result.id);

            return Created(path, result);
        }

        /// <summary>
        /// Search for a book by his id
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">Not Found</response>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(404)]
        [HttpGet("books/{id}")]
        public ActionResult<Book> Get(string id)
        {
            var result = _bookService.GetById(id);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("books/")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(_bookService.GetFromExternalSource());
        }
    }
}