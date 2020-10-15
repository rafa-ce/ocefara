using Microsoft.AspNetCore.Mvc;
using Moq;
using MyLibrary.API.Controllers;
using MyLibrary.API.Infrastructure.Repositories;
using MyLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.UnitTests
{
    public class BooksControllerTest
    {
        private List<Book> _booksMock = new List<Book>();
        private Book _bookMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;

        public BooksControllerTest()
        {
            _bookMock = new Book
            {
                Id = "5dc1039a1521eaa36835e541",
                Title = "O corpo encantado das ruas",
                ISBN = "978-85-200-1392-2",
                Pages = 176,
                PublicationYear = 2019,
                Publisher = "Civilização Brasileira",
                Edition = 5,
                Digital = false,
                Authors = new List<Author>() { new Author() { Name = "Luiz Antonio Simas" } }
            };

            _booksMock = new List<Book>();
            _booksMock.Add(_bookMock);

            _bookRepositoryMock = new Moq.Mock<IBookRepository>();
        }

        [Fact]
        public async Task Get_Ok()
        {
            _bookRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_booksMock);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Get();

            Assert.IsType<ActionResult<IEnumerable<Book>>>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, (actionResult.Result as OkObjectResult).StatusCode);
            Assert.Equal(1, (int)(((ObjectResult)actionResult.Result).Value as List<Book>).Count);
        }

        [Fact]
        public async Task Get_Empty()
        {
            _bookRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Book>());

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Get();

            Assert.IsType<ActionResult<IEnumerable<Book>>>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, (actionResult.Result as OkObjectResult).StatusCode);
            Assert.Equal(0, (int)(((ObjectResult)actionResult.Result).Value as List<Book>).Count);
        }

        [Fact]
        public async Task GetByISBN_Ok()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync(_bookMock);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Get("978-85-200-1392-2");

            Assert.IsType<ActionResult<Book>>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, (actionResult.Result as OkObjectResult).StatusCode);
            Assert.Equal(_bookMock.ISBN, (((ObjectResult)actionResult.Result).Value as Book).ISBN);
        }

        [Fact]
        public async Task GetByISBN_NotFound()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Get("978-85-200-1392-2");

            Assert.IsType<ActionResult<Book>>(actionResult);
            Assert.Equal((int)HttpStatusCode.NotFound, (actionResult.Result as NotFoundResult).StatusCode);
        }

        [Fact]
        public async Task GetByISBN_Null_BadRequest()
        {
            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Get(null);

            Assert.IsType<ActionResult<Book>>(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult.Result as BadRequestResult).StatusCode);
        }

        [Fact]
        public async Task GetByISBN_Empty_BadRequest()
        {
            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Get("");

            Assert.IsType<ActionResult<Book>>(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult.Result as BadRequestResult).StatusCode);
        }

        [Fact]
        public async Task Post_Created()
        {
            _bookRepositoryMock.Setup(x => x.CreateAsync(_bookMock)).Verifiable();

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Post(_bookMock);

            Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.Created, (actionResult as CreatedAtActionResult).StatusCode);
        }

        [Fact]
        public async Task Put_NoContent()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync(_bookMock);
            _bookRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<string>(), _bookMock)).Verifiable();

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Put("978-85-200-1392-2", _bookMock);

            Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NoContent, (actionResult as NoContentResult).StatusCode);
        }

        [Fact]
        public async Task Put_NotFound()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Put("978-85-200-1392-2", _bookMock);

            Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NotFound, (actionResult as NotFoundResult).StatusCode);
        }

        [Fact]
        public async Task Put_ISBNEmpty_BadRequest()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Put("", _bookMock);

            Assert.IsType<BadRequestResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as BadRequestResult).StatusCode);
        }

        [Fact]
        public async Task Put_ISBNNull_NotFound()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Put(null, _bookMock);

            Assert.IsType<BadRequestResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as BadRequestResult).StatusCode);
        }

        [Fact]
        public async Task Delete_NoContent()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync(_bookMock);
            _bookRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<string>())).Verifiable();

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Delete("978-85-200-1392-2");

            Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NoContent, (actionResult as NoContentResult).StatusCode);
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Delete("978-85-200-1392-2");

            Assert.IsType<NotFoundResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NotFound, (actionResult as NotFoundResult).StatusCode);
        }

        [Fact]
        public async Task Delete_ISBNEmpty_BadRequest()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Delete("");

            Assert.IsType<BadRequestResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as BadRequestResult).StatusCode);
        }

        [Fact]
        public async Task Delete_ISBNNull_NotFound()
        {
            _bookRepositoryMock.Setup(x => x.GetByISBNAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            BooksController controller = new BooksController(_bookRepositoryMock.Object);

            var actionResult = await controller.Delete(null);

            Assert.IsType<BadRequestResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as BadRequestResult).StatusCode);
        }
    }
}
