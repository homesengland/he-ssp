using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Application.Factories;

public interface IApplicationSummaryViewModelFactory
{
    Task<ApplicationSummaryViewModel> GetDataAndCreate(string applicationId, IUrlHelper urlHelper, bool isReadOnly, CancellationToken cancellationToken);
}
