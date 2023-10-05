using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.CRM.Model;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;

public static class CompanyStructureCrmFieldNameMapper
{
    private static readonly string ExternalStatus = $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},";
    private static readonly string CompanyPurpose = $"{nameof(invln_Loanapplication.invln_CompanyPurpose).ToLowerInvariant()},";
    private static readonly string CompanyStructureInformation = $"{nameof(invln_Loanapplication.invln_Companystructureinformation).ToLowerInvariant()},";
    private static readonly string CompanyExperience = $"{nameof(invln_Loanapplication.invln_CompanyExperience).ToLowerInvariant()},";

    private static readonly string CompanyStructureCompletionStatus =
        $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}";

    public static string Map(CompanyStructureFieldsSet companyStructureFieldsSet)
    {
        var result = companyStructureFieldsSet switch
        {
            CompanyStructureFieldsSet.GetEmpty => ExternalStatus,
            CompanyStructureFieldsSet.CompanyPurpose => CompanyPurpose,
            CompanyStructureFieldsSet.HomesBuilt => CompanyExperience,
            CompanyStructureFieldsSet.MoreInformationAboutOrganization => CompanyStructureInformation,
            CompanyStructureFieldsSet.GetAllFields => ExternalStatus +
                                                      CompanyPurpose +
                                                      CompanyStructureInformation +
                                                      CompanyExperience,
            CompanyStructureFieldsSet.SaveAllFields => CompanyPurpose +
                                                       CompanyStructureInformation +
                                                       CompanyExperience,
            _ => string.Empty,
        };

        return result + CompanyStructureCompletionStatus;
    }
}
