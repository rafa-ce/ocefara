using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.API.Infrastructure.Repositories;
using MyLibrary.API.Models;

namespace MyLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();

                return Ok(books);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{isbn}")]
        public async Task<ActionResult<Book>> Get(string isbn)
        {
            if (String.IsNullOrEmpty(isbn))
                return BadRequest();

            try
            {
                var book = await _bookRepository.GetByISBNAsync(isbn);

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            try
            {
                await _bookRepository.CreateAsync(book);

                return CreatedAtAction(nameof(Get), new { isbn = book.ISBN }, null);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("{isbn}")]
        public async Task<ActionResult> Put(string isbn, Book bookIn)
        {
            if (String.IsNullOrEmpty(isbn))
                return BadRequest();

            try
            {
                var book = await _bookRepository.GetByISBNAsync(isbn);

                if (book == null)
                    return NotFound();

                bookIn.Id = book.Id;

                await _bookRepository.UpdateAsync(isbn, bookIn);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{isbn}")]
        public async Task<ActionResult> Delete(string isbn)
        {
            if (String.IsNullOrEmpty(isbn))
                return BadRequest();

            try
            {
                var book = await _bookRepository.GetByISBNAsync(isbn);

                if (book == null)
                    return NotFound();

                await _bookRepository.DeleteAsync(isbn);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
