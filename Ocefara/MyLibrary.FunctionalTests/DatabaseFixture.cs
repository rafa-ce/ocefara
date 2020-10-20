using MongoDB.Driver;
using MyLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.FunctionalTests
{
    public class DatabaseFixture : IDisposable
    {
        private MongoClient _mongoClient;

        public DatabaseFixture()
        {
            _mongoClient = new MongoClient(@"mongodb://127.0.0.1:27017");
            _mongoClient.DropDatabase("mylibrary");
            var books = _mongoClient.GetDatabase("mylibrary").GetCollection<Book>("books");
            books.InsertOne(new Book()
            {
                Title = "O corpo encantado das ruas",
                ISBN = "978-85-200-1392-2",
                Pages = 176,
                PublicationYear = 2019,
                Publisher = "Civilização Brasileira",
                Edition = 5,
                Digital = false,
                Authors = new List<Author>() { new Author() { Name = "Luiz Antonio Simas" } }
            });
            books.InsertOne(new Book()
            {
                Title = "Código limpo: habilidades práticas do Agile Software",
                ISBN = "978-85-7608-267-5",
                Pages = 456,
                PublicationYear = 2011,
                Publisher = "Alta Books",
                Edition = 1,
                Digital = false,
                Authors = new List<Author>() { new Author() { Name = "Robert C. Martin" } }
            });
        }

        public void Dispose()
        {
            _mongoClient.DropDatabase("mylibrary");
        }
    }
}
