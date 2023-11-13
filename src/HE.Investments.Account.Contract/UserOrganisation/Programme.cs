namespace HE.Investments.Account.Contract.UserOrganisation;

public record Programme(ProgrammeType Type, IList<UserApplication> Applications);
