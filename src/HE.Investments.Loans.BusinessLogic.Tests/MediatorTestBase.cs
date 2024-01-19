using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using HE.Investments.Common.Utils;
using HE.Investments.Loans.BusinessLogic.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests;

public class MediatorTestBase
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new(MockBehavior.Strict);
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new(MockBehavior.Strict);

    [SuppressMessage("Usage", "CA2214", Justification = "Allowed in tests")]
    public MediatorTestBase()
    {
        SetupHttpContextAccessorWithUrl("http://localhost");
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoanApplicationViewModel).Assembly));
        services.AddValidatorsFromAssemblyContaining<LoanApplicationViewModel>();
        services.AddTransient(x => _httpContextAccessor.Object);

        _dateTimeProvider.Setup(d => d.Now).Returns(new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Unspecified));
        services.AddSingleton(x => _dateTimeProvider.Object);

        AddAditionalServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    protected ServiceProvider ServiceProvider { get; set; }

    public virtual void AddAditionalServices(ServiceCollection collection)
    {
    }

    private void SetupHttpContextAccessorWithUrl(string currentUrl)
    {
        var httpContext = new DefaultHttpContext();
        var session = new TestSession();
        httpContext.Session = session;
        SetRequestUrl(httpContext.Request, currentUrl);

        _httpContextAccessor
          .SetupGet(accessor => accessor.HttpContext)
          .Returns(httpContext);

        static void SetRequestUrl(HttpRequest httpRequest, string url)
        {
            UriHelper
              .FromAbsolute(
                url,
                out var scheme,
                out var host,
                out var path,
                out var query,
                fragment: out var _);

            httpRequest.Scheme = scheme;
            httpRequest.Host = host;
            httpRequest.Path = path;
            httpRequest.QueryString = query;
        }
    }

    private sealed class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new(StringComparer.OrdinalIgnoreCase);

        public TestSession()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public bool IsAvailable { get; } = true;

        public IEnumerable<string> Keys => _store.Keys;

        public void Clear()
        {
            _store.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public void Remove(string key)
        {
            _store.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _store[key] = value;
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out byte[] value)
        {
            return _store.TryGetValue(key, out value);
        }
    }
}
