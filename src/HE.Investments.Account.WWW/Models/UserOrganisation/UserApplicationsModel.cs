namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record UserApplicationsModel(string Header, IList<ApplicationBasicDetailsModel> Applications, string Action, string Controller);
