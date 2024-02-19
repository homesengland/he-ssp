#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HE.Investments.Common.CRM.Model
{
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getsingleloanapplicationforaccountandcontact")]
	public partial class invln_getsingleloanapplicationforaccountandcontactRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_fieldstoretrieve = "invln_fieldstoretrieve";
			public const string invln_accountid = "invln_accountid";
			public const string invln_externalcontactid = "invln_externalcontactid";
			public const string invln_loanapplicationid = "invln_loanapplicationid";
		}
		
		public const string ActionLogicalName = "invln_getsingleloanapplicationforaccountandcontact";
		
		public string invln_fieldstoretrieve
		{
			get
			{
				if (this.Parameters.Contains("invln_fieldstoretrieve"))
				{
					return ((string)(this.Parameters["invln_fieldstoretrieve"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_fieldstoretrieve"] = value;
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
		
		public string invln_loanapplicationid
		{
			get
			{
				if (this.Parameters.Contains("invln_loanapplicationid"))
				{
					return ((string)(this.Parameters["invln_loanapplicationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_loanapplicationid"] = value;
			}
		}
		
		public invln_getsingleloanapplicationforaccountandcontactRequest()
		{
			this.RequestName = "invln_getsingleloanapplicationforaccountandcontact";
			this.invln_accountid = default(string);
			this.invln_externalcontactid = default(string);
			this.invln_loanapplicationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getsingleloanapplicationforaccountandcontact")]
	public partial class invln_getsingleloanapplicationforaccountandcontactResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_loanapplication = "invln_loanapplication";
		}
		
		public const string ActionLogicalName = "invln_getsingleloanapplicationforaccountandcontact";
		
		public invln_getsingleloanapplicationforaccountandcontactResponse()
		{
		}
		
		public string invln_loanapplication
		{
			get
			{
				if (this.Results.Contains("invln_loanapplication"))
				{
					return ((string)(this.Results["invln_loanapplication"]));
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
