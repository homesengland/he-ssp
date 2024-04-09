using HE.Investment.AHP.Contract.Application;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Application.Factories;

public interface IApplicationSummaryViewModelFactory
{
    Task<ApplicationSummaryViewModel> GetDataAndCreate(AhpApplicationId applicationId, IUrlHelper urlHelper, CancellationToken cancellationToken);
}
