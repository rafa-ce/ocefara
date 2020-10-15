using MyLibrary.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.FunctionalTests
{
    public class BookScenarios : ScenarioBase, IClassFixture<DatabaseFixture>
    {
        [Fact]
        public async Task GetAllBooks_and_OkStatusCode()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient().GetAsync(Get.Books());
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task GetBookByISBN_and_OkStatusCode()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient().GetAsync(Get.Books("978-85-200-1392-2"));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task GetBookByISBN_and_NotFoundStatusCode()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient().GetAsync(Get.Books("978-85-200-1392-3"));

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task PostBook_and_CreatedStatusCode()
        {
            var book = new Book()
            {
                ISBN = "978-85-93115-12-7",
                Title = "O eclipse do progressismo",
                PublicationYear = 2018,
                Pages = 275,
                Edition = 1,
                Publisher = "Elefante",
                Digital = false,
                Authors = new List<Author>()
                {
                    new Author() { Name = "José Correia Leite" },
                    new Author() { Name = "Janaina Uemura" },
                    new Author() { Name = "Filomena Siqueira" }
                }
            };

            using (var server = CreateServer())
            {
                var dadosJson = JsonConvert.SerializeObject(book);

                var content = new StringContent(dadosJson, Encoding.UTF8, "application/json");

                var response = await server.CreateClient().PostAsync(Post.CreateBook, content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task PutBook_and_NoContentStatusCode()
        {
            var book = new Book
            {
                Title = "O corpo encantado das ruas",
                ISBN = "978-85-200-1392-2",
                Pages = 176,
                PublicationYear = 2019,
                Publisher = "Civilização Brasileira",
                Edition = 5,
                Digital = true,
                Authors = new List<Author>() { new Author() { Name = "Luiz Antonio Simas" } }
            };

            using (var server = CreateServer())
            {
                var dadosJson = JsonConvert.SerializeObject(book);

                var content = new StringContent(dadosJson, Encoding.UTF8, "application/json");

                var response = await server.CreateClient().PutAsync(Put.UpdateBook(book.ISBN), content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task PutBook_and_NotFoundStatusCode()
        {
            var book = new Book()
            {
                ISBN = "978-85-250-5224-7",
                Title = "Fahrenheit 451",
                PublicationYear = 2012,
                Pages = 215,
                Edition = 2,
                Publisher = "Globo",
                Digital = false,
                Authors = new List<Author>()
                {
                    new Author() { Name = "Ray Bradbury" }
                }
            };

            using (var server = CreateServer())
            {
                var dadosJson = JsonConvert.SerializeObject(book);

                var content = new StringContent(dadosJson, Encoding.UTF8, "application/json");

                var response = await server.CreateClient().PutAsync(Put.UpdateBook(book.ISBN), content);

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task DeleteBook_and_NoContentStatusCode()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient().DeleteAsync(Delete.Book("978-85-7608-267-5"));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task DeleteBook_and_NotFoundStatusCode()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient().DeleteAsync(Delete.Book("978-85-250-5224-7"));

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
