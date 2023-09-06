using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

public class IntegrationTestFixture<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public IntegrationTestFixture()
    {
        DataBag = new Dictionary<string, object>();
    }

    public IDictionary<string, object> DataBag { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
    }
}
