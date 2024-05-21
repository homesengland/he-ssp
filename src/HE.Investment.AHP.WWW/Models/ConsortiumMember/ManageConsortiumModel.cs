using HE.Investments.AHP.Consortium.Contract;

namespace HE.Investment.AHP.WWW.Models.ConsortiumMember;

public record ManageConsortiumModel(ConsortiumDetails Details, string OrganisationName, bool CanManageConsortium);
