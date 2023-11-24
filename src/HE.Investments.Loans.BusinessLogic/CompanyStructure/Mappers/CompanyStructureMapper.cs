using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.Mappers;

public static class CompanyStructureMapper
{
    public static CompanyPurpose? MapCompanyPurpose(bool? companyPurpose)
    {
        if (!companyPurpose.HasValue)
        {
            return null;
        }

        return CompanyPurpose.New(companyPurpose.Value);
    }

    public static bool? MapCompanyPurpose(CompanyPurpose? companyPurpose)
    {
        if (companyPurpose is null)
        {
            return null;
        }

        return companyPurpose.IsSpv;
    }

    public static OrganisationMoreInformation? MapMoreInformation(string? moreInformation)
    {
        if (string.IsNullOrWhiteSpace(moreInformation))
        {
            return null;
        }

        return new OrganisationMoreInformation(moreInformation);
    }

    public static HomesBuilt? MapHomesBuild(int? homesBuild)
    {
        if (homesBuild is null)
        {
            return null;
        }

        return new HomesBuilt(homesBuild.Value);
    }
}
