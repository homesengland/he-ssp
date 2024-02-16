using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class MilestoneFrameWorkItemMapper
    {
        public static MilestoneFrameworkItemDto MapRegularEntityToDto(invln_milestoneframeworkitem milestoneframeworkitem)
        {
            var milestoneframeworkitemDto = new MilestoneFrameworkItemDto()
            {
                name = milestoneframeworkitem.invln_milestoneframeworkitemname,
                programme = milestoneframeworkitem.invln_programmeId.Id.ToString(),
                milestone = milestoneframeworkitem.invln_milestone.Value,
                isMilestonePayable =  milestoneframeworkitem.invln_ismilestonepayable,
                percentPaid = milestoneframeworkitem.invln_percentagepaidonmilestone,
                minimumValue = milestoneframeworkitem.invln_MinimumValue.Value, 
            };
            return milestoneframeworkitemDto;
        }

        public static List<MilestoneFrameworkItemDto> MapMilestoneFrameworkItemListToDto(List<invln_milestoneframeworkitem> milestoneFrameworkItemList)
        {
            if (milestoneFrameworkItemList.Count > 0)
            {
                var milestoneFrameworkItemListDto = new List<MilestoneFrameworkItemDto>();
                foreach (var milestoneFrameworkItem in milestoneFrameworkItemList)
                {
                    milestoneFrameworkItemListDto.Add(MapRegularEntityToDto(milestoneFrameworkItem));
                }
                return milestoneFrameworkItemListDto;
            }
            return null;
        }
    }
}

