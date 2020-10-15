using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.API.Infrastructure.Configurations
{
    public class MyLibraryDatabaseConfig : IMyLibraryDatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
