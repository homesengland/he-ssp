using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class ProgrammeMapper
    {
        public static ProgrammeDto MapRegularEntityToDto(invln_programme programme, List<MilestoneFrameworkItemDto> milestoneFrameworkItemDtoList)
        {
            var programmeDto = new ProgrammeDto()
            {
                name = programme.invln_programmename,
                startOn = programme.invln_programmestartdate,
                endOn = programme.invln_programmeenddate,
                startOnSiteStartDate = programme.invln_earlieststartonsitedateallowed,
                startOnSiteEndDate = programme.invln_lateststartonsitedateallowed,
                completionStartDate = programme.invln_earliestcompletiondateallowed,
                completionEndDate = programme.invln_latestcompletiondateallowed,
                assignFundingStartDate = programme.invln_fundingstartdate,
                assignFundingEndDate = programme.invln_fundingenddate,
            };

            if (milestoneFrameworkItemDtoList != null)
            {
                programmeDto.milestoneFrameworkItem = milestoneFrameworkItemDtoList;
            }

            return programmeDto;
        }

        public static ProgrammeDto MapRegularEntityToDto(invln_programme programme)
        {
            var programmeDto = new ProgrammeDto()
            {
                name = programme.invln_programmename,
                startOn = programme.invln_programmestartdate,
                endOn = programme.invln_programmeenddate,
                startOnSiteStartDate = programme.invln_earlieststartonsitedateallowed,
                startOnSiteEndDate = programme.invln_lateststartonsitedateallowed,
                completionStartDate = programme.invln_earliestcompletiondateallowed,
                completionEndDate = programme.invln_latestcompletiondateallowed,
                assignFundingStartDate = programme.invln_fundingstartdate,
                assignFundingEndDate = programme.invln_fundingenddate,
            };

            return programmeDto;
        }
    }
}
