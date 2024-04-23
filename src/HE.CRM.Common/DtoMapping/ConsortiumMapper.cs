using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace HE.CRM.Common.DtoMapping
{
    public class ConsortiumMapper
    {
        public static ConsortiumDto MapReguralEntityToDto(invln_Consortium consortium, List<invln_ConsortiumMember> consortiumMember)
        {
            var consortiumDto = new ConsortiumDto
            {
                id = consortium.Id.ToString(),
                name = consortium.invln_Name.ToString(),
                leadPartnerId = consortium.invln_LeadPartner.Id.ToString(),
                leadPartnerName = consortium.invln_LeadPartner.Name
            };
            var consortiumMemberDtos = new List<ConsortiumMemberDto>();
            foreach (var member in consortiumMember)
            {
                consortiumMemberDtos.Add(new ConsortiumMemberDto()
                {
                    id = member.invln_Partner.Id.ToString(),
                    name = member.invln_Partner.Name,
                    status = member.StatusCode.Value
                });
            }
            consortiumDto.members = consortiumMemberDtos;
            return consortiumDto;
        }
    }
}