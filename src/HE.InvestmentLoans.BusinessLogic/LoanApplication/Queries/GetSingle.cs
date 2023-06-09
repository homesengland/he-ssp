using HE.InvestmentLoans.BusinessLogic.LoanApplication.Extensions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Queries;

public class GetSingle : IRequest<LoanApplicationViewModel>
{
    public Guid Id
    {
        get;
        set;
    }

    public class Handler : IRequestHandler<GetSingle, LoanApplicationViewModel>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<LoanApplicationViewModel> Handle(GetSingle request, CancellationToken cancellationToken)
        {
            var model = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(request.Id.ToString()) ?? throw new NotFoundException(nameof(LoanApplicationViewModel), request.Id);

            return Task.FromResult(model);
        }
    }
}
