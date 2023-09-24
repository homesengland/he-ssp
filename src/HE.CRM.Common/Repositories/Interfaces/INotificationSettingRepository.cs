using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface INotificationSettingRepository : ICrmEntityRepository<invln_notificationsetting, DataverseContext>
    {
        invln_notificationsetting GetTemplateViaTypeName(string templateTypeName);
    }
}
