extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;

public class DraftConsortiumEntityBuilder : TestObjectBuilder<DraftConsortiumEntityBuilder, DraftConsortiumEntity>
{
    public DraftConsortiumEntityBuilder()
        : base(new DraftConsortiumEntity(
            new ConsortiumId("00000000-0000-1111-1111-111111111111"),
            new ConsortiumName("my consortium name"),
            ProgrammeSlimTestData.AhpCmeProgramme,
            new DraftConsortiumMember(
                ConsortiumMemberTestData.CarqMember.Id,
                ConsortiumMemberTestData.CarqMember.OrganisationName),
            []))
    {
    }

    protected override DraftConsortiumEntityBuilder Builder => this;

    public DraftConsortiumEntityBuilder WithId(string id) => SetProperty(x => x.Id, new ConsortiumId(id));

    public DraftConsortiumEntityBuilder WithLeadPartner(InvestmentsOrganisation organisation) =>
        SetProperty(x => x.LeadPartner, new DraftConsortiumMember(organisation.Id, organisation.Name));

    public DraftConsortiumEntityBuilder WithMember(InvestmentsOrganisation organisation) => SetProperty(
        x => x.Members,
        Item.Members.Concat(new[] { new DraftConsortiumMember(organisation.Id, organisation.Name) }).ToList());
}
