using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
     public class NotificationSettingRepository : CrmEntityRepository<invln_notificationsetting, DataverseContext>, INotificationSettingRepository
    {
        #region Constructors

        public NotificationSettingRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        #endregion

        #region Interface Implementation

        public invln_notificationsetting GetTemplateViaTypeName(string templateTypeName)
        {
            var qe = new QueryExpression(invln_notificationsetting.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(nameof(invln_notificationsetting.invln_subject).ToLower(), nameof(invln_notificationsetting.invln_templateid).ToLower(), nameof(invln_notificationsetting.invln_templatetypename).ToLower())
            };
            qe.Criteria.AddCondition(nameof(invln_notificationsetting.invln_templatetypename).ToLower(), ConditionOperator.Equal, templateTypeName);
            var result = this.service.RetrieveMultiple(qe);
            if(result != null && result.Entities.Count > 0)
            {
                var notSett = result.Entities.Select(x => x.ToEntity<invln_notificationsetting>());
                return notSett.FirstOrDefault();
            }

            return null;
        }

        #endregion
    }
}
