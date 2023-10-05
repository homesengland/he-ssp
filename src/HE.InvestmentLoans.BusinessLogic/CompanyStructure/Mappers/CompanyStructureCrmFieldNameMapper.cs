using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.CRM.Model;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;

public static class CompanyStructureCrmFieldNameMapper
{
    private static readonly string CompanyPurpose = $"{nameof(invln_Loanapplication.invln_CompanyPurpose).ToLowerInvariant()},";
    private static readonly string CompanyStructureInformation = $"{nameof(invln_Loanapplication.invln_Companystructureinformation).ToLowerInvariant()},";
    private static readonly string CompanyExperience = $"{nameof(invln_Loanapplication.invln_CompanyExperience).ToLowerInvariant()},";
    private static readonly string CompanyStructureCompletionStatus = $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}";

    public static string Map(CompanyStructureViewOption companyStructureViewOption)
    {
        var result = companyStructureViewOption switch
        {
            CompanyStructureViewOption.CompanyPurpose => CompanyPurpose,
            CompanyStructureViewOption.HomesBuilt => CompanyExperience,
            CompanyStructureViewOption.MoreInformationAboutOrganization => CompanyStructureInformation,
            CompanyStructureViewOption.GetAllFields => CompanyPurpose +
                                                       CompanyStructureInformation +
                                                       CompanyExperience,
            _ => string.Empty,
        };

        return result + CompanyStructureCompletionStatus;
    }
}
