namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class OrganizationDetailsDto
    {
        //COMPANY
        public string registeredCompanyName { get; set; }
        public string companyRegistrationNumber { get; set; }
        public string rpNumber { get; set; }

        //COMPANY ADDRESS
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string city { get; set; }
        public string postalcode { get; set; }
        public string country { get; set; }

        //COMPANY ADMIN CONTACT
        public string compayAdminContactName { get; set; }
        public string compayAdminContactEmail { get; set; }
        public string compayAdminContactTelephone { get; set; }
        public string organisationId { get; set; }
    }
}
