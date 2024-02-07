using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmFields;

internal static class AccountEntity
{
    public const string Name = "account";

    public static ColumnSet AllColumns()
    {
        return new ColumnSet(
            Properties.CompanyName,
            Properties.CompanyNumber,
            Properties.AddressLine1,
            Properties.AddressLine2,
            Properties.AddressLine3,
            Properties.City,
            Properties.PostalCode,
            Properties.Country,
            Properties.PrimaryContactId,
            Properties.UnregisteredBody);
    }

    public static class Properties
    {
        public const string CompanyName = "name";

        public const string CompanyNumber = "he_companieshousenumber";

        public const string AddressLine1 = "address1_line1";

        public const string AddressLine2 = "address1_line2";

        public const string AddressLine3 = "address1_line3";

        public const string City = "address1_city";

        public const string PostalCode = "address1_postalcode";

        public const string Country = "address1_country";

        public const string County = "address1_county";

        public const string PrimaryContactId = "primarycontactid";

        public const string UnregisteredBody = "invln_unregisteredbody";
    }
}
