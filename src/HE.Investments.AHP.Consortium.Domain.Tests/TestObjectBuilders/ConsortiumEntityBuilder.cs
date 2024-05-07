extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.TestsUtils;
using HE.Investments.TestsUtils.TestFramework;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;

public class ConsortiumEntityBuilder : TestObjectBuilder<ConsortiumEntityBuilder, ConsortiumEntity>
{
    public ConsortiumEntityBuilder()
        : base(new ConsortiumEntity(
            new ConsortiumId("00000000-0000-1111-1111-111111111111"),
            new ConsortiumName("my consortium name"),
            ProgrammeSlimTestData.AhpCmeProgramme,
            ConsortiumMemberTestData.CarqMember,
            []))
    {
    }

    protected override ConsortiumEntityBuilder Builder => this;

    public ConsortiumEntityBuilder WithId(string id) => SetProperty(x => x.Id, new ConsortiumId(id));

    public ConsortiumEntityBuilder WithLeadPartner(InvestmentsOrganisation organisation) =>
        SetProperty(x => x.LeadPartner, new ConsortiumMember(organisation.Id, organisation.Name, ConsortiumMemberStatus.Active));

    public ConsortiumEntityBuilder WithActiveMember(InvestmentsOrganisation organisation)
    {
        List<ConsortiumMember> members = [.. Item.Members, new ConsortiumMember(organisation.Id, organisation.Name, ConsortiumMemberStatus.Active)];
        PrivatePropertySetter.SetPrivateField(Item, "_members", members);

        return this;
    }
}
