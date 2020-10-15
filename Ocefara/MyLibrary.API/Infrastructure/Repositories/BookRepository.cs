using MongoDB.Driver;
using MyLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.API.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private const string CollectionName = "books";

        private readonly IMyLibraryDBContext _context;
        private IMongoCollection<Book> _dbCollection;

        public BookRepository(IMyLibraryDBContext context)
        {
            _context = context;
            _dbCollection = _context.GetCollection<Book>(CollectionName);
        }
        public async Task CreateAsync(Book book)
        {
            await _dbCollection.InsertOneAsync(book);
        }

        public async Task DeleteAsync(string isbn)
        {
            await _dbCollection.DeleteOneAsync(b => b.ISBN == isbn);
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _dbCollection.Find(b => true).ToListAsync();
        }

        public async Task<Book> GetByISBNAsync(string isbn)
        {
            return await _dbCollection.Find<Book>(b => b.ISBN == isbn).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string isbn, Book bookIn)
        {
            await _dbCollection.ReplaceOneAsync(b => b.ISBN == isbn, bookIn);
        }
    }
}
