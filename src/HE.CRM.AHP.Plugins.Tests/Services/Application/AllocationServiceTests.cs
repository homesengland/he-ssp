using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.CRM.AHP.Plugins.Services.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Services;

namespace HE.CRM.AHP.Plugins.Tests.Services.Application
{
    [TestClass]
    public class AllocationServiceTests : CrmServiceTestBase<IAllocationService>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CalculateGrantDetailsForClaims_AllocationUpdated()
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid()
            };

            var organisation = new Account()
            {
                Id = Guid.NewGuid()
            };

            var application = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_applicationid = "000001",
                invln_organisationid = organisation.ToEntityReference(),
                invln_contactid = contact.ToEntityReference(),
                invln_fundingrequired = new Money(200_000)
            };

            var allocation = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_organisationid = organisation.ToEntityReference(),
                invln_contactid = contact.ToEntityReference(),
                invln_isallocation = true,
                invln_fundingrequired = new Money(200_000)
            };

            var deliveryPhase1 = new invln_DeliveryPhase()
            {
                Id = Guid.NewGuid(),
                invln_Application = allocation.ToEntityReference()
            };

            var deliveryPhase2 = new invln_DeliveryPhase()
            {
                Id = Guid.NewGuid(),
                invln_Application = allocation.ToEntityReference(),
            };

            var claim2_1 = new invln_Claim()
            {
                Id = Guid.NewGuid(),
                invln_Application = application.ToEntityReference(),
                invln_Allocation = allocation.ToEntityReference(),
                invln_DeliveryPhase = deliveryPhase2.ToEntityReference(),
                invln_Milestone = new OptionSetValue((int)invln_Milestone.Acquisition),
                invln_AmountApportionedtoMilestone = new Money(10000),
                StatusCode = new OptionSetValue((int)invln_Claim_StatusCode.Approve)
            };

            var claim2_2 = new invln_Claim()
            {
                Id = Guid.NewGuid(),
                invln_Application = application.ToEntityReference(),
                invln_Allocation = allocation.ToEntityReference(),
                invln_DeliveryPhase = deliveryPhase2.ToEntityReference(),
                invln_Milestone = new OptionSetValue((int)invln_Milestone.Planning),
                invln_AmountApportionedtoMilestone = new Money(20000),
                StatusCode = new OptionSetValue((int)invln_Claim_StatusCode.Submitted)
            };

#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext.Initialize(new List<Entity>()
            {
                contact,
                organisation,
                application,
                allocation,
                deliveryPhase1,
                deliveryPhase2,
                claim2_1,
                claim2_2,
            });
#pragma warning restore CS0618 // Type or member is obsolete

            this.Asset();

            service.CalculateGrantDetails(allocation.Id);

            var allocationUpdated = fakedContext.CreateQuery<invln_scheme>().Where(x => x.Id == allocation.Id).First();

            Assert.AreEqual(200_000, allocationUpdated.invln_TotalGrantAllocated.Value, "Wrong invln_TotalGrantAllocated value.");
            Assert.AreEqual(10_000, allocationUpdated.invln_AmountPaid.Value, "Wrong invln_AmountPaid value.");
            Assert.AreEqual(190_000, allocationUpdated.invln_AmountRemaining.Value, "Wrong invln_AmountRemaining value.");
        }
    }
}
