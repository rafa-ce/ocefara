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
        /// <response code="500">Falha no servidor ou no banco de dados</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Book>), 200)]
        [ProducesResponseType(500)]
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
        /// <param name="isbn">ISBN do livro</param>
        /// <returns>Um livro com o ISBN informado</returns>
        /// <response code="200">Um livro com o ISBN informado</response>
        /// <response code="400">ISBN inválido</response>
        /// <response code="404">Livro não encontrado</response>
        /// <response code="500">Falha no servidor ou no banco de dados</response>
        [HttpGet("{isbn}")]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
        /// Adicionar um novo livro
        /// </summary>
        /// <param name="book">Dados do livro</param>
        /// <returns></returns>
        /// <response code="201">Livro criado com sucesso</response>
        /// <response code="500">Falha no servidor ou no banco de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
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
        /// Atualizar um livro existente
        /// </summary>
        /// <param name="isbn">ISBN do livro</param>
        /// <param name="bookIn">Dados do livro atualizado</param>
        /// <returns></returns>
        /// <response code="204">Livro atualizado com sucesso</response>
        /// <response code="404">Livro não encontrado</response>
        /// <response code="500">Falha no servidor ou no banco de dados</response>
        [HttpPut("{isbn}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
        /// Excluir um livro
        /// </summary>
        /// <param name="isbn">ISBN do livro</param>
        /// <returns></returns>
        /// <response code="204">Livro excluído com sucesso</response>
        /// <response code="404">Livro não encontrado</response>
        /// <response code="500">Falha no servidor ou no banco de dados</response>
        [HttpDelete("{isbn}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
