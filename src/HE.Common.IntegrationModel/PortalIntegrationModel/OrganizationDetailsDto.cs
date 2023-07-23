using System;
using System.Collections.Generic;
using System.Text;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class OrganizationDetailsDto
    {
        //COMPANY
        public string registeredCompanyName { get; set; }
        public string companyRegistrationNumber { get; set; }

        //COMPANY ADDRESS
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string city { get; set; }
        public string postalcode { get; set; }
        public string country { get; set; }

        //LOGGED CONTACT
        public string contactName { get; set; }
        public string contactEmail { get; set; }
        public string contactTelephone { get; set; }
    }
}
