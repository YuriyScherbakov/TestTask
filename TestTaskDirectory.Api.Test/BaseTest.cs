using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestTaskDirectory.Api.Test
{
    public abstract class BaseTest :
        IClassFixture<WebApplicationFactory<Startup>>
    {
        protected WebApplicationFactory<Startup> Factory;

        protected BaseTest(WebApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }
    }
}