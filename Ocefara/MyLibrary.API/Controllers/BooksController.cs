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

        /// <summary>
        /// Obter todos os livros
        /// </summary>
        /// <returns>Uma lista de livros</returns>
        /// <response code="200">Retorna uma lista de livros</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Obter livro pelo ISBN
        /// </summary>
        /// <param name="isbn"></param>
        /// <response code="200">Retorna o livro com o ISBN informado</response>
        /// <response code="400">ISBN não informado</response>
        /// <response code="404">Não foi encontrado livro com o ISBN informado</response>
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

        /// <summary>
        /// Criar um livro
        /// </summary>
        /// <param name="book"></param>
        /// <response code="201">Livro criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Editar campos de livro
        /// </summary>
        /// <param name="isbn"></param>
        /// <param name="bookIn"></param>
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

        /// <summary>
        /// Excluir livro pelo ISBN
        /// </summary>
        /// <param name="isbn"></param>
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
