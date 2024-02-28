using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using FakeXrmEasy;
using HE.CRM.AHP.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Common.Extensions;

namespace HE.CRM.AHP.Plugins.Tests.CustomApi
{
    [TestClass]
    public class GetSingleHomeTypePluginTests
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
        public void Test_1()
        {
            var homeTypeId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                {
                    new invln_HomeType()
                    {
                        Id = homeTypeId,
                        invln_application = appId.ToEntityReference<invln_HomeType>()
                    }
                },
                {
                    new invln_scheme()
                    {
                        Id = appId,
                        invln_organisationid = organizationId.ToEntityReference<Account>(),
                        invln_contactid =  userId.ToEntityReference<Contact>()
                    }
                },
                {
                    new Account()
                    {
                        Id = organizationId
                    }
                },
                {
                    new Contact()
                    {
                        Id = userId,
                        invln_externalid = userId.ToString()
                    }
                }
            });

            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_getsinglehometypeRequest.Fields.invln_hometypeid, homeTypeId.ToString()},
                { invln_getsinglehometypeRequest.Fields.invln_applicationid, appId.ToString()},
                { invln_getsinglehometypeRequest.Fields.invln_organisationid, organizationId.ToString()},
                { invln_getsinglehometypeRequest.Fields.invln_userid, userId.ToString()},
                { invln_getsinglehometypeRequest.Fields.invln_fieldstoretrieve, null}
            };

            fakedContext.ExecutePluginWithConfigurations<GetSingleHomeTypePlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_getsinglehometypeResponse.Fields.invln_hometype].ToString();
            Assert.IsNotNull(outputResponce);
            Assert.AreNotEqual("[]", outputResponce);
        }
    }
}
