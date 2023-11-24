using LocalAuthorityDto = HE.Common.IntegrationModel.PortalIntegrationModel.LocalAuthorityDto;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal sealed class LocalAuthorityDtoTestData
{
    public static readonly LocalAuthorityDto LocalAuthorityOne = new() { onsCode = "1", name = "Liverpool" };
    public static readonly LocalAuthorityDto LocalAuthorityTwo = new() { onsCode = "2", name = "London" };
    public static readonly LocalAuthorityDto LocalAuthorityThree = new() { onsCode = "3", name = "Manchester" };
    public static readonly LocalAuthorityDto LocalAuthorityFour = new() { onsCode = "4", name = "Brighton" };
    public static readonly LocalAuthorityDto LocalAuthorityFive = new() { onsCode = "5", name = "Leicester" };

    public static readonly IList<LocalAuthorityDto> LocalAuthoritiesDtoList = new List<LocalAuthorityDto>
    {
        LocalAuthorityOne,
        LocalAuthorityTwo,
        LocalAuthorityThree,
        LocalAuthorityFour,
        LocalAuthorityFive,
    };
}
