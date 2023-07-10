using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using Microsoft.AspNetCore.Http;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Extensions;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Queries
{
    public class GetSingle : IRequest<LoanApplicationViewModel>
    {
        public Guid Id
        {
            get;
            set;
        }

        public class Handler : IRequestHandler<GetSingle, LoanApplicationViewModel>
        {
            private IHttpContextAccessor _httpContextAccessor;

            public Handler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }


            public async Task<LoanApplicationViewModel> Handle(GetSingle request, CancellationToken cancellationToken)
            {
                var model = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(request.Id.ToString());

                if (model == null)
                {
                    throw new NotFoundException(nameof(LoanApplicationViewModel), request.Id);
                }

                return model;
            }
        }
    }
}
