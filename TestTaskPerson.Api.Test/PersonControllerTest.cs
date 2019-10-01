using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestTaskPerson.Api.Controllers;
using TestTaskPerson.Api.DAL;
using Xunit;
using Xunit.Abstractions;

namespace TestTaskPerson.Api.Test
{
    public class PersonControllerTest : BaseTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        private readonly HttpClient client;

        public PersonControllerTest(WebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper) : base(factory)
        {
            this.testOutputHelper = testOutputHelper;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions() { BaseAddress = new Uri("http://localhost:5000/api/") });
        }

        /// <summary>
        /// Tests <see cref="PersonController.GetAll"/> 
        /// </summary>
        [Fact]
        public async Task GetAllPersons()
        {
            var result = await client.GetAsync("persons/get-all");
            var persons = await TestExtensions.DeserializeResult<List<Person>>(result);

            // check that persons are stored in DB.
            persons.Count.Should().BeGreaterThan(3);

            // check files ordered by name correctly
            persons.First().Name.Should().NotBeNull();

            foreach (var person in persons)
            {
                testOutputHelper.WriteLine(person.Surname);
            }
        }

        /// <summary>
        /// Tests <see cref="PersonController.GetByFilter"/> 
        /// </summary>
        [Fact]
        public async Task GetByFilter()
        {
            var result = await client.GetAsync("persons/get-by-filter?fieldName=Name&fieldValue=A&isContains=true");
            var persons = await TestExtensions.DeserializeResult<List<Person>>(result);

            // check that method filters by current criteria
            persons.Count.Should().BeGreaterThan(0);

            var names = persons.Select(x => x.Name).ToList();
            names.Should().NotBeNull();
        }

        /// <summary>
        /// Tests <see cref="PersonController.GetPagedQuery"/> 
        /// </summary>
        [Fact]
        public async Task PagedRequest()
        {
            var pagedRequest = new PagedRequest
            {
                Asc = true,
                Page = 1,
                PageSize = 5,
                OrderByField = "Name"
            };

            var result = await client.PostAsync("persons/get-paged-list", TestExtensions.SerializeRequest(pagedRequest));
            var dataSourceResult = await TestExtensions.DeserializeResult<DataSourceResult<Person>>(result);
            dataSourceResult.Items.Count().Should().Be(5);
            dataSourceResult.Total.Should().BeGreaterThan(5);

            testOutputHelper.WriteLine($"request: (Asc:{pagedRequest.Asc}, Page:{pagedRequest.Page}, PageSize:{pagedRequest.PageSize}, OrderByField:{pagedRequest.OrderByField})");
        }

        /// <summary>
        /// Tests <see cref="PersonController.GetDuplicateNames"/> 
        /// </summary>
        [Fact]
        public async Task GetDuplicateNames()
        {
            var result = await client.GetAsync("persons/get-duplicate-names/0");
            var duplicateNames = await TestExtensions.DeserializeResult<List<string>>(result);
            duplicateNames.Count.Should().BeGreaterThan(3);

            var result2 = await client.GetAsync("persons/get-duplicate-names/5");
            var duplicateNames2 = await TestExtensions.DeserializeResult<List<string>>(result2);
            duplicateNames2.Count.Should().Be(0);

            testOutputHelper.WriteLine("");
        }
    }
}
