#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataverseModel
{
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getloanapplicationsforaccountandcontact")]
	public partial class invln_getloanapplicationsforaccountandcontactRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_usehetables = "invln_usehetables";
			public const string invln_accountid = "invln_accountid";
			public const string invln_externalcontactid = "invln_externalcontactid";
		}
		
		public const string ActionLogicalName = "invln_getloanapplicationsforaccountandcontact";
		
		public string invln_usehetables
		{
			get
			{
				if (this.Parameters.Contains("invln_usehetables"))
				{
					return ((string)(this.Parameters["invln_usehetables"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_usehetables"] = value;
			}
		}
		
		public string invln_accountid
		{
			get
			{
				if (this.Parameters.Contains("invln_accountid"))
				{
					return ((string)(this.Parameters["invln_accountid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_accountid"] = value;
			}
		}
		
		public string invln_externalcontactid
		{
			get
			{
				if (this.Parameters.Contains("invln_externalcontactid"))
				{
					return ((string)(this.Parameters["invln_externalcontactid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_externalcontactid"] = value;
			}
		}
		
		public invln_getloanapplicationsforaccountandcontactRequest()
		{
			this.RequestName = "invln_getloanapplicationsforaccountandcontact";
			this.invln_accountid = default(string);
			this.invln_externalcontactid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getloanapplicationsforaccountandcontact")]
	public partial class invln_getloanapplicationsforaccountandcontactResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_loanapplications = "invln_loanapplications";
		}
		
		public const string ActionLogicalName = "invln_getloanapplicationsforaccountandcontact";
		
		public invln_getloanapplicationsforaccountandcontactResponse()
		{
		}
		
		public string invln_loanapplications
		{
			get
			{
				if (this.Results.Contains("invln_loanapplications"))
				{
					return ((string)(this.Results["invln_loanapplications"]));
				}
				else
				{
					return default(string);
				}
			}
		}
	}
}
#pragma warning restore CS1591
