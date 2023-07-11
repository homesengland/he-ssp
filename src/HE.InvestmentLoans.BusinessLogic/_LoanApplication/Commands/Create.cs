using HE.InvestmentLoans.BusinessLogic._LoanApplication.Extensions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands;

public class Create : IRequest<LoanApplicationViewModel>
{
    public class Handler : IRequestHandler<Create, LoanApplicationViewModel>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<LoanApplicationViewModel> Handle(Create request, CancellationToken cancellationToken)
        {
            var model = new LoanApplicationViewModel() { Timestamp = DateTime.Now };
            var result = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(model.ID.ToString());

            if (result == null)
            {
                _httpContextAccessor.HttpContext?.Session.Set(model.ID.ToString(), model);
            }

            return Task.FromResult(model);
        }
    }
}
