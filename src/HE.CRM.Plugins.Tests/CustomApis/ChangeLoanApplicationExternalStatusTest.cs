using DataverseModel;
using FakeItEasy;
using FakeXrmEasy;
using HE.CRM.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Tests.CustomApis
{
    [TestClass]
    public class ChangeLoanApplicationExternalStatusTest
    {
        private XrmFakedContext fakedContext;
        private XrmFakedPluginExecutionContext pluginContext;

        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
        }

        [TestMethod]
        public void ChangeLoanApplicationExternalStatus_NoLoanApplicationWithGivenIdExists_ShouldThrowError()
        {
            Exception exception = null;
            try
            {
                var request = new invln_changeloanapplicationexternalstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), Guid.NewGuid().ToString() },
                    {nameof(request.invln_statusexternal), (int)invln_ExternalStatus.Draft }
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeLoanApplicationExternalStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustNotHaveHappened();
        }

        [TestMethod]
        public void ChangeLoanApplicationExternalStatus_LoanApplicationWithGivenIdExistsWithStatusEqNull_ShouldChangeStatus()
        {
            invln_Loanapplication application = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),

            };
            fakedContext.Initialize(new List<Entity> { application });
            Exception exception = null;
            try
            {
                var request = new invln_changeloanapplicationexternalstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), application.Id.ToString() },
                    {nameof(request.invln_statusexternal), (int)invln_ExternalStatus.Draft }
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeLoanApplicationExternalStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
        }
        [TestMethod]
        public void ChangeLoanApplicationExternalStatus_ChangeStatusToNull_ShouldThrowError()
        {
            invln_Loanapplication application = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),

            };
            fakedContext.Initialize(new List<Entity> { application });
            Exception exception = null;
            try
            {
                var request = new invln_changeloanapplicationexternalstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), application.Id.ToString() },
                    {nameof(request.invln_statusexternal), null }
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeLoanApplicationExternalStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustNotHaveHappened();
        }
        [TestMethod]
        [DataRow((int)invln_ExternalStatus.Applicationdeclined)]
        [DataRow((int)invln_ExternalStatus.Approvedsubjecttocontract)]
        [DataRow((int)invln_ExternalStatus.ContractSignedsubjecttoCP)]
        [DataRow((int)invln_ExternalStatus.CPssatisfied)]
        [DataRow((int)invln_ExternalStatus.Draft)]
        [DataRow((int)invln_ExternalStatus.Holdrequested)]
        [DataRow((int)invln_ExternalStatus.Inactive)]
        [DataRow((int)invln_ExternalStatus.Induediligence)]
        [DataRow((int)invln_ExternalStatus.Loanavailable)]
        [DataRow((int)invln_ExternalStatus.Onhold)]
        [DataRow((int)invln_ExternalStatus.Referredbacktoapplicant)]
        [DataRow((int)invln_ExternalStatus.Submitted)]
        [DataRow((int)invln_ExternalStatus.Underreview)]
        [DataRow((int)invln_ExternalStatus.Withdrawn)]
        public void ChangeLoanApplicationExternalStatus_ChangeStatusFromDraftToNewValue_ShouldUpdateStatus(int newStatus)
        {
            invln_Loanapplication application = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),
                invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Draft),
            };
            fakedContext.Initialize(new List<Entity> { application });
            Exception exception = null;
            try
            {
                var request = new invln_changeloanapplicationexternalstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), application.Id.ToString() },
                    {nameof(request.invln_statusexternal), newStatus }
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeLoanApplicationExternalStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (newStatus == (int)invln_ExternalStatus.Withdrawn)
            {
                Assert.IsNotNull(exception);
                A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustNotHaveHappened();
            }
            else
            {
                Assert.IsNull(exception);
                A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
            }
        }
        [TestMethod]
        [DataRow((int)invln_ExternalStatus.Applicationdeclined)]
        [DataRow((int)invln_ExternalStatus.Approvedsubjecttocontract)]
        [DataRow((int)invln_ExternalStatus.ContractSignedsubjecttoCP)]
        [DataRow((int)invln_ExternalStatus.CPssatisfied)]
        [DataRow((int)invln_ExternalStatus.Draft)]
        [DataRow((int)invln_ExternalStatus.Holdrequested)]
        [DataRow((int)invln_ExternalStatus.Inactive)]
        [DataRow((int)invln_ExternalStatus.Induediligence)]
        [DataRow((int)invln_ExternalStatus.Loanavailable)]
        [DataRow((int)invln_ExternalStatus.Onhold)]
        [DataRow((int)invln_ExternalStatus.Referredbacktoapplicant)]
        [DataRow((int)invln_ExternalStatus.Submitted)]
        [DataRow((int)invln_ExternalStatus.Underreview)]
        [DataRow((int)invln_ExternalStatus.Withdrawn)]
        public void ChangeLoanApplicationExternalStatus_ChangeStatusFromSubmittedToNewValue_ShouldUpdateStatus(int newStatus)
        {
            invln_Loanapplication application = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),
                invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Submitted),
            };
            fakedContext.Initialize(new List<Entity> { application });
            Exception exception = null;
            try
            {
                var request = new invln_changeloanapplicationexternalstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), application.Id.ToString() },
                    {nameof(request.invln_statusexternal), newStatus }
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeLoanApplicationExternalStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (newStatus == (int)invln_ExternalStatus.Submitted || newStatus == (int)invln_ExternalStatus.Inactive)
            {
                Assert.IsNotNull(exception);
                A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustNotHaveHappened();
            }
            else
            {
                Assert.IsNull(exception);
                A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
            }
        }
        [TestMethod]
        [DataRow((int)invln_ExternalStatus.Applicationdeclined)]
        [DataRow((int)invln_ExternalStatus.Approvedsubjecttocontract)]
        [DataRow((int)invln_ExternalStatus.ContractSignedsubjecttoCP)]
        [DataRow((int)invln_ExternalStatus.CPssatisfied)]
        [DataRow((int)invln_ExternalStatus.Draft)]
        [DataRow((int)invln_ExternalStatus.Holdrequested)]
        [DataRow((int)invln_ExternalStatus.Inactive)]
        [DataRow((int)invln_ExternalStatus.Induediligence)]
        [DataRow((int)invln_ExternalStatus.Loanavailable)]
        [DataRow((int)invln_ExternalStatus.Onhold)]
        [DataRow((int)invln_ExternalStatus.Referredbacktoapplicant)]
        [DataRow((int)invln_ExternalStatus.Submitted)]
        [DataRow((int)invln_ExternalStatus.Underreview)]
        [DataRow((int)invln_ExternalStatus.Withdrawn)]
        public void ChangeLoanApplicationExternalStatus_ChangeStatusFromInactiveToNewValue_ShouldUpdateStatus(int newStatus)
        {
            invln_Loanapplication application = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),
                invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatus.Inactive),
            };
            fakedContext.Initialize(new List<Entity> { application });
            Exception exception = null;
            try
            {
                var request = new invln_changeloanapplicationexternalstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), application.Id.ToString() },
                    {nameof(request.invln_statusexternal), newStatus }
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeLoanApplicationExternalStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (newStatus == (int)invln_ExternalStatus.Submitted || newStatus == (int)invln_ExternalStatus.Inactive || newStatus == (int)invln_ExternalStatus.Withdrawn)
            {
                Assert.IsNotNull(exception);
                A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustNotHaveHappened();
            }
            else
            {
                Assert.IsNull(exception);
                A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
            }
        }
    }
}
