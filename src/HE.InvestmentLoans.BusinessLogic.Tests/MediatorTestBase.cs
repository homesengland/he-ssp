using HE.InvestmentLoans.BusinessLogic.ViewModel;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace HE.InvestmentLoans.BusinessLogic.Tests
{
  
    public class MediatorTestBase
    {
        public ServiceProvider serviceProvider;

        public MediatorTestBase()
        {
            SetupHttpContextAccessorWithUrl("http://localhost");
            var services = new ServiceCollection();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoanApplicationViewModel).Assembly);
            });
            services.AddValidatorsFromAssemblyContaining<LoanApplicationViewModel>();
            services.AddTransient<IHttpContextAccessor>(x => _HttpContextAccessor.Object);
            AddAditionalServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        readonly Mock<IHttpContextAccessor> _HttpContextAccessor = new Mock<IHttpContextAccessor>(MockBehavior.Strict);

        public virtual void AddAditionalServices(ServiceCollection collection)
        {

        }


        void SetupHttpContextAccessorWithUrl(string currentUrl)
        {
            var httpContext = new DefaultHttpContext();
            var session = new TestSession();
            httpContext.Session = session;
            setRequestUrl(httpContext.Request, currentUrl);

            _HttpContextAccessor
              .SetupGet(accessor => accessor.HttpContext)
              .Returns(httpContext);

            static void setRequestUrl(HttpRequest httpRequest, string url)
            {
                UriHelper
                  .FromAbsolute(url, out var scheme, out var host, out var path, out var query,
                    fragment: out var _);

                httpRequest.Scheme = scheme;
                httpRequest.Host = host;
                httpRequest.Path = path;
                httpRequest.QueryString = query;
            }
        }

        private class TestSession : ISession
        {
            public TestSession()
            {
                Id = Guid.NewGuid().ToString();
            }

            private readonly Dictionary<string, byte[]> _store
                = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

            public string Id { get; set; }

            public bool IsAvailable { get; } = true;

            public IEnumerable<string> Keys { get { return _store.Keys; } }

            public void Clear()
            {
                _store.Clear();
            }

            public Task CommitAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }

            public Task LoadAsync(CancellationToken cancellationToken)
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

            public bool TryGetValue(string key, [NotNullWhen(true)] out byte[] value)
            {
                return _store.TryGetValue(key, out value);
            }
        }



    }
}