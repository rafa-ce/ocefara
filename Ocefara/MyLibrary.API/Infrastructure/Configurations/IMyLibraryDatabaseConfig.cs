using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.API.Infrastructure.Configurations
{
    public interface IMyLibraryDatabaseConfig
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
