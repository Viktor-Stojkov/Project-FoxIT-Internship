using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectMVC_FoxIT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMVC_FoxIT
{
    public class Program
    {
        private readonly WorkOrdersContext _context;

        public Program( WorkOrdersContext context)
        {
            _context = context;
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //public void InitCustomerEdb()
        //{
        //    foreach (var customer in _context.Customers)
        //    {
        //        Random random = new Random()
        //    }
        //}
    }
}
