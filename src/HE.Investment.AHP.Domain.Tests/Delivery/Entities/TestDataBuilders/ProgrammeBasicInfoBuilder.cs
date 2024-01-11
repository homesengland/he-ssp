using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class ProgrammeBasicInfoBuilder
{
    public ProgrammeBasicInfo Build()
    {
        return new(new DateOnly(2023, 1, 1), new DateOnly(2027, 1, 1));
    }
}
