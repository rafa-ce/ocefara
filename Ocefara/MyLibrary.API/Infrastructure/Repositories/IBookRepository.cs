using MyLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.API.Infrastructure.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetByISBNAsync(string isbn);
        Task<List<Book>> GetAllAsync();
        Task CreateAsync(Book book);
        Task UpdateAsync(string isbn, Book bookIn);
        Task DeleteAsync(string isbn);
    }
}
