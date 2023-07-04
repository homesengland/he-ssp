using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Enums;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.CRM.Model;
using MediatR;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands
{
    public class GetUserRolesFromCrm : IRequest<ContactRolesDto>
    {
        public string contactExternalId
        {
            get;
            set;
        }

        public string contactEmail
        {
            get;
            set;
        }

        public class Handler : IRequestHandler<GetUserRolesFromCrm, ContactRolesDto>
        {
            private IOrganizationService _serviceClient;

            public Handler(IOrganizationService serviceClient)
            {
                _serviceClient = serviceClient;
            }

            public async Task<ContactRolesDto> Handle(GetUserRolesFromCrm request, CancellationToken cancellationToken)
            {
                var req = new invln_getcontactroleRequest()
                {
                    invln_contactemail = request.contactEmail,
                    invln_contactexternalid = request.contactExternalId,
                    invln_portaltype = "858110001"
                };

                var resp = (invln_getcontactroleResponse)_serviceClient.Execute(req);
                if (resp.invln_portalroles != null)
                {
                    return JsonSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);
                }

                return null;
            }
        }
    }
}
