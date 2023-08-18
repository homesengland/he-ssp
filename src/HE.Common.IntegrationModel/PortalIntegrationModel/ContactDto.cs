using System;
using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class ContactDto
    {
        public string contactId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string contactExternalId { get; set; }
        public string jobTitle { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
    }
}
