namespace HE.Investments.Organisation.CompaniesHouse.Contract;

public record CompaniesHouseErrorsResult(CompaniesHouseErrorItem[] Errors);

public record CompaniesHouseErrorItem(string Error, string Type);
