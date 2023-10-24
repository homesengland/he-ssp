using System.Diagnostics;
using System.Text.Json;
using HE.Investment.AHP.BusinessLogic.FinancialDetails.Entities;
using HE.Investment.AHP.BusinessLogic.FinancialDetails.Repositories;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;

public class FinancialDetailsRepository : IFinancialDetailsRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public FinancialDetailsRepository(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<FinancialDetailsEntity> GetById(FinancialDetailsId financialDetailsId, CancellationToken cancellationToken)
    {
        var dataFromCookies = await Task.Run(() => _httpContextAccessor.HttpContext?.Request.Cookies[financialDetailsId.Value.ToString()]);
        var result = JsonSerializer.Deserialize<FinancialDetailsEntity>(dataFromCookies ?? string.Empty) ?? throw new NotFoundException($"Financial details entity with id{financialDetailsId.Value} was not found.");
        return result;
    }

    public async Task SaveAsync(FinancialDetailsEntity financialDetailsEntity, CancellationToken cancellationToken)
    {
        await Task.Run(() => _httpContextAccessor.HttpContext?.Response.Cookies.Append(financialDetailsEntity.Id.ToString(), JsonSerializer.Serialize(financialDetailsEntity)), cancellationToken);
    }
}
