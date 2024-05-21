namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumsList(IList<ConsortiumByMemberRole> Consortiums, string OrganisationName);
