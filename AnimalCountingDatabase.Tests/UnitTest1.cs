using AnimalCountingDatabase.Api;
using AnimalCountingDatabase.Api.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AnimalCountingDatabase.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(2 == 2);
        }

        [Fact]
        public async Task CustomerIntegrationTest()
        {
            // Create DB Context
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            var context = new CustomerContext(optionsBuilder.Options);

            //Just to make sure: Delete all existing customers in the DB
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            //Create Controller
            var controller = new CustomerController(context);

            //Add Customer
            await controller.Add(new Customer() { CustomerName = "FooBar" });

            //Check: Does GetAll return the added Customer?
            var result = (await controller.GetAll()).ToArray();
            Assert.Single(result);
            Assert.Equal("FooBar", result[0].CustomerName);

        }
    }
}
