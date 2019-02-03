using System;
using Challenge.NetCore.Books.Domain;
using Challenge.NetCore.Books.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Challenge.NetCore.Books.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BookService bookS = new BookService();
            //bookS.AddOne(new Book(){title = "New book", description = $"Something FANCY at {DateTime.Now}"});
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
