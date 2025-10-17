using Microsoft.EntityFrameworkCore;
using MicroTwitter.Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroTwitter.Tests
{
    public static  class DbContextFactory
    {

        public static AppDbContext CreateInMemory(string dbName = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString()) // unique per call
                .Options;

            return new AppDbContext(options);

        }
    }
}
