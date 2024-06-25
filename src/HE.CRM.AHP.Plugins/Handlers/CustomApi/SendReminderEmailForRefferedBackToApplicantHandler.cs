using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SendReminderEmailForRefferedBackToApplicantHandler : CrmActionHandlerBase<invln_sendreminderemailforrefferedbacktoapplicantRequest, DataverseContext>
    {
        private readonly IAhpApplicationRepository _applicationRepository;

        public SendReminderEmailForRefferedBackToApplicantHandler(IAhpApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public override bool CanWork()
        {
            if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            {
                return true;
            }
            else
            {
                TracingService.Trace("Today is Saturday or Sunday");
                return false;
            }


        }

        public override void DoWork()
        {
            TracingService.Trace("SendReminderEmailForRefferedBackToApplicantHandler");

            var calculatedDate = DateCalculation(5).ToString();
            TracingService.Trace($"calculatedDate: {calculatedDate}");

            var listOfApplications = _applicationRepository.GetListOfApplicationToSendReminder(calculatedDate);
            TracingService.Trace($"listOfApplications count: {listOfApplications.Count}");
            foreach (var application in listOfApplications)
            {
                TracingService.Trace($"application.invln_schemeId: {application.invln_schemeId}");
                CrmServicesFactory.Get<IApplicationService>().SendReminderEmailForRefferedBackToApplicant(application.invln_schemeId.Value);

                application.invln_lastemailsenton = DateTime.Now;
                _applicationRepository.Update(application);
            }
        }

        private DateTime DateCalculation(int daysDifference)
        {
            TracingService.Trace("DateCalculation");
            var calculatedDate = DateTime.Today.Date;
            TracingService.Trace($"Start Date: {calculatedDate}");
            var i = 1;
            while(i <= daysDifference)
            {
                calculatedDate = calculatedDate.AddDays(-1);
                if(calculatedDate.DayOfWeek != DayOfWeek.Sunday && calculatedDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    i++;
                }
            }
            TracingService.Trace($"End Date: {calculatedDate}");
            return calculatedDate;
        }
    }
}
