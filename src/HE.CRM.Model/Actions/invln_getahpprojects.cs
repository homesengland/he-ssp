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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getahpprojects")]
	public partial class invln_getahpprojectsRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_userid = "invln_userid";
			public const string invln_accountid = "invln_accountid";
			public const string invln_consortiumid = "invln_consortiumid";
		}
		
		public const string ActionLogicalName = "invln_getahpprojects";
		
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
		
		public invln_getahpprojectsRequest()
		{
			this.RequestName = "invln_getahpprojects";
			this.invln_userid = default(string);
			this.invln_accountid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getahpprojects")]
	public partial class invln_getahpprojectsResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_listOfAhpProjects = "invln_listOfAhpProjects";
		}
		
		public const string ActionLogicalName = "invln_getahpprojects";
		
		public invln_getahpprojectsResponse()
		{
		}
		
		public string invln_listOfAhpProjects
		{
			get
			{
				if (this.Results.Contains("invln_listOfAhpProjects"))
				{
					return ((string)(this.Results["invln_listOfAhpProjects"]));
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