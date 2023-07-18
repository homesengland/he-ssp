using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Extensions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Commands;

public class Create : IRequest<LoanApplicationViewModel>
{
    public class Handler : IRequestHandler<Create, LoanApplicationViewModel>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTimeProvider _dateTime;

        public Handler(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
        {
            _httpContextAccessor = httpContextAccessor;
            _dateTime = dateTime;
        }

        public Task<LoanApplicationViewModel> Handle(Create request, CancellationToken cancellationToken)
        {
            var model = new LoanApplicationViewModel() { Timestamp = _dateTime.Now };
            model.AddNewSite();
            var result = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(model.ID.ToString());

            if (result == null)
            {
                _httpContextAccessor.HttpContext?.Session.Set(model.ID.ToString(), model);
            }

            return Task.FromResult(model);
        }
    }
}
