using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.API.Infrastructure
{
    public interface IMyLibraryDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
