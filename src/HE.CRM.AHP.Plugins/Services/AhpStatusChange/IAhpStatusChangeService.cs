using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.AHP.Plugins.Services.AhpStatusChange
{
    public interface IAhpStatusChangeService : ICrmService
    {
        void SendNotificationOnAhpStatusChangeCreate(invln_AHPStatusChange target);
        void SendInternalCrmNotification(invln_AHPStatusChange statusChange, invln_scheme ahpApplication, string statusLabel);
    }
}
