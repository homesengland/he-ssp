using HE.InvestmentLoans.BusinessLogic.LoanApplication.Extensions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Commands;

public class Update : IRequest<LoanApplicationViewModel>
{
    public LoanApplicationViewModel Model
    {
        get;
        set;
    }

    public Func<LoanApplicationViewModel, Task<bool>> TryUpdateModelAction
    {
        get;
        set;
    }

    public class Handler : IRequestHandler<Update, LoanApplicationViewModel>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTimeProvider _dateTime;

        public Handler(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
        {
            _httpContextAccessor = httpContextAccessor;
            _dateTime = dateTime;
        }

        public async Task<LoanApplicationViewModel> Handle(Update request, CancellationToken cancellationToken)
        {
            if (request.Model == null)
            {
                throw new NotFoundException(nameof(LoanApplicationViewModel), string.Empty);
            }

            request.Model.Timestamp = _dateTime.Now;

            if (request.TryUpdateModelAction != null)
            {
                _ = await request.TryUpdateModelAction(request.Model);
            }

            _httpContextAccessor.HttpContext?.Session.Set(request.Model.ID.ToString(), request.Model);

            return request.Model;
        }
    }
}
