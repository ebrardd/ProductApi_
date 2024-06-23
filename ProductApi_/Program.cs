using Microsoft.AspNetCore.Hosting;


var builder = WebApplication.CreateBuilder(args);

namespace ProductApi_
{
 public class Program
 { 
     public static void Main(string[] args)
     {
         CreateHostBuilder(args).Build().Run();
     }

     public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder
                 .SuppressStatusMessages(true)
                     .UseStartup<Startup>();
             });
 }
} 

