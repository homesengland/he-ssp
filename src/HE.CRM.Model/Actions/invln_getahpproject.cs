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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getahpproject")]
	public partial class invln_getahpprojectRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_userid = "invln_userid";
			public const string invln_accountid = "invln_accountid";
			public const string invln_ahpprojectid = "invln_ahpprojectid";
			public const string invln_heprojectid = "invln_heprojectid";
			public const string invln_consortiumid = "invln_consortiumid";
		}
		
		public const string ActionLogicalName = "invln_getahpproject";
		
		public string invln_userid
		{
			get
			{
				if (this.Parameters.Contains("invln_userid"))
				{
					return ((string)(this.Parameters["invln_userid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_userid"] = value;
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
		
		public string invln_ahpprojectid
		{
			get
			{
				if (this.Parameters.Contains("invln_ahpprojectid"))
				{
					return ((string)(this.Parameters["invln_ahpprojectid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_ahpprojectid"] = value;
			}
		}
		
		public string invln_heprojectid
		{
			get
			{
				if (this.Parameters.Contains("invln_heprojectid"))
				{
					return ((string)(this.Parameters["invln_heprojectid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_heprojectid"] = value;
			}
		}
		
		public string invln_consortiumid
		{
			get
			{
				if (this.Parameters.Contains("invln_consortiumid"))
				{
					return ((string)(this.Parameters["invln_consortiumid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_consortiumid"] = value;
			}
		}
		
		public invln_getahpprojectRequest()
		{
			this.RequestName = "invln_getahpproject";
			this.invln_userid = default(string);
			this.invln_accountid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getahpproject")]
	public partial class invln_getahpprojectResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_ahpProjectApplications = "invln_ahpProjectApplications";
		}
		
		public const string ActionLogicalName = "invln_getahpproject";
		
		public invln_getahpprojectResponse()
		{
		}
		
		public string invln_ahpProjectApplications
		{
			get
			{
				if (this.Results.Contains("invln_ahpProjectApplications"))
				{
					return ((string)(this.Results["invln_ahpProjectApplications"]));
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
