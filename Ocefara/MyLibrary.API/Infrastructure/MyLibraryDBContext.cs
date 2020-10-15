using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyLibrary.API.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.API.Infrastructure
{
    public class MyLibraryDBContext : IMyLibraryDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MyLibraryDBContext(IOptions<MyLibraryDatabaseConfig> settings)
        {
            _mongoClient = new MongoClient(settings.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
