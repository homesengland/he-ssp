using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;

namespace HE.CRM.Common.OptionSetsMapping
{
    public class TenureMapp
    {
        public static string TenureMappToString(int value)
        {
            switch (value)
            {
                case (int)invln_Tenure.AffordableRent:
                    return "Affordable rent";
                case (int)invln_Tenure.SocialRent:
                    return "Social rent";
                case (int)invln_Tenure.SharedOwnership:
                    return "Shared ownership";
                case (int)invln_Tenure.RenttoBuy:
                    return "Rent to buy";
                case (int)invln_Tenure.HOLD:
                    return "HOLD";
                case (int)invln_Tenure.OPSO:
                    return "OPSO";
                default:
                    return null;
            }
        }
    }
}
