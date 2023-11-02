using System.Text.Json;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils;
using Microsoft.AspNetCore.Http;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

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
        await Task.Run(() => _httpContextAccessor.HttpContext?.Response.Cookies.Append(financialDetailsEntity.FinancialDetailsId.ToString(), JsonSerializer.Serialize(financialDetailsEntity)), cancellationToken);
    }
}
