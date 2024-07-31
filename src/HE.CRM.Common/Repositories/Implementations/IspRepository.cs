using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using HE.CRM.Common.Repositories.Interfaces;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class IspRepository : CrmEntityRepository<invln_ISP, DataverseContext>, IIspRepository
    {
        public IspRepository(CrmRepositoryArgs args) : base(args)
        {
        }
        public invln_sendinternalcrmnotificationResponse ExecuteNotificatioRequest(invln_sendinternalcrmnotificationRequest request)
        {
            logger.Trace("ExecuteNotificationRequest");
            using (var ctx = new OrganizationServiceContext(service))
            {
                return (invln_sendinternalcrmnotificationResponse)ctx.Execute(request);
            }
        }

    }
}
