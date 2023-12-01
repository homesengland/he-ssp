using HE.Investment.AHP.Domain.Common.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.Common.Utils;

namespace HE.Investment.AHP.Domain.HomeTypes.Services;

public class DesignFileService : AhpFileServiceBase<DesignFileParams>
{
    public DesignFileService(IDocumentService documentService, IDateTimeProvider dateTimeProvider, IAccountUserContext userContext)
        : base(documentService, dateTimeProvider, userContext)
    {
    }

    protected override FileLocation GetFilesLocation(DesignFileParams fileParams)
    {
        // TODO: Load application Number and Home Type number from CRM
        // TODO: create directories when creating Application/HomeType
        return new FileLocation("AHP Application", "invln_scheme", $"{fileParams.ApplicationId}/Home Type/{fileParams.HomeTypeId}/external/Design Files");
    }
}
