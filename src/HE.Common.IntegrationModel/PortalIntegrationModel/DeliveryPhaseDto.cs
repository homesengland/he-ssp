using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Crm.Sdk.Messages;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class DeliveryPhaseDto
    {
        public string id { get; set; }
        public string applicationId { get; set; }
        public string name { get; set; }
        public System.DateTime? createdOn { get; set; }
        public string typeOfHomes { get; set; }
        public int? newBuildActivityType { get; set; }
        public int? rehabBuildActivityType { get; set; }
        public bool? isReconfigurationOfExistingProperties { get; set; }
        public bool? isCompleted { get; set; }
        public Dictionary<string, int?> numberOfHomes { get; set; } //Dictionary<HomeTypeId, NumberOfHomes>
        public DateTime? acquisitionDate { get; set; }
        public DateTime? acquisitionPaymentDate { get; set; }
        public DateTime? startOnSiteDate { get; set; }
        public DateTime? startOnSitePaymentDate { get; set; }
        public DateTime? completionDate { get; set; }
        public DateTime? completionPaymentDate { get; set; }
        public string requiresAdditionalPayments { get; set; }
    }
}
