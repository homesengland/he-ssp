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

    //private readonly IFinancialDetailsServiceAsync _serviceClient;

    public FinancialDetailsRepository(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)//, IFinancialDetailsServiceAsync serviceClient)
    {
        _httpContextAccessor = httpContextAccessor;
        //_serviceClient = serviceClient;
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

        //if (projectToSave.IsNewlyCreated)
        //{
        //    await CreateNewProject(applicationProjects, projectToSave, cancellationToken);
        //}
        //else if (projectToSave.IsSoftDeleted)
        //{
        //    await DeleteProject(projectToSave, cancellationToken);
        //}
        //else
        //{
        //    await UpdateProject(applicationProjects, projectId, projectToSave, cancellationToken);
        //}

    }

    // private async Task UpdateProject(FinancialDetailsEntity financialDetailsEntity, CancellationToken cancellationToken)
    // {
    //     await Task.Run(() => _httpContextAccessor.HttpContext?.Response.Cookies.Append(financialDetailsEntity.Id.ToString(), JsonSerializer.Serialize(financialDetailsEntity)), cancellationToken);
    // }
}
