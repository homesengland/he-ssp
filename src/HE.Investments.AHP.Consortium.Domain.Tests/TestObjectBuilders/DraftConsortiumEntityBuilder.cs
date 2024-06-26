using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Organisation.ValueObjects;
using HE.Investments.Programme.Contract;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;

public class DraftConsortiumEntityBuilder : TestObjectBuilder<DraftConsortiumEntityBuilder, DraftConsortiumEntity>
{
    public DraftConsortiumEntityBuilder()
        : base(new DraftConsortiumEntity(
            new ConsortiumId("00000000-0000-1111-1111-111111111111"),
            new ConsortiumName("my consortium name"),
            new ProgrammeId("d5fe3baa-eeae-ee11-a569-0022480041cf"),
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
        Item.Members.Concat([new DraftConsortiumMember(organisation.Id, organisation.Name)]).ToList());
}
