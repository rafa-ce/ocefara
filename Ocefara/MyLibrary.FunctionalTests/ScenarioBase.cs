using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using MyLibrary.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.FunctionalTests
{
    public class ScenarioBase
    {
        public TestServer CreateServer()
        {
            return new TestServer(
                new WebHostBuilder()
                .ConfigureAppConfiguration(cb => {
                    cb.AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<Startup>());
        }

        public static class Get
        {
            public static string Books(string isbn = "")
            {
                return string.Format("/api/books/{0}", isbn);
            }
        }

        public static class Post
        {
            public static string CreateBook = "api/books";
        }

        public static class Put
        {
            public static string UpdateBook(string isbn = "")
            {
                return string.Format("/api/books/{0}", isbn);
            }
        }

        public static class Delete
        {
            public static string Book(string isbn = "")
            {
                return string.Format("/api/books/{0}", isbn);
            }
        }
    }
}
