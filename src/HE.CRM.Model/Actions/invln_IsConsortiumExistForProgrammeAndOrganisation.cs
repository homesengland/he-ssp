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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_IsConsortiumExistForProgrammeAndOrganisation")]
	public partial class invln_IsConsortiumExistForProgrammeAndOrganisationRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_programmeid = "invln_programmeid";
			public const string invln_organisationid = "invln_organisationid";
		}
		
		public const string ActionLogicalName = "invln_IsConsortiumExistForProgrammeAndOrganisation";
		
		public string invln_programmeid
		{
			get
			{
				if (this.Parameters.Contains("invln_programmeid"))
				{
					return ((string)(this.Parameters["invln_programmeid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_programmeid"] = value;
			}
		}
		
		public string invln_organisationid
		{
			get
			{
				if (this.Parameters.Contains("invln_organisationid"))
				{
					return ((string)(this.Parameters["invln_organisationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_organisationid"] = value;
			}
		}
		
		public invln_IsConsortiumExistForProgrammeAndOrganisationRequest()
		{
			this.RequestName = "invln_IsConsortiumExistForProgrammeAndOrganisation";
			this.invln_programmeid = default(string);
			this.invln_organisationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_IsConsortiumExistForProgrammeAndOrganisation")]
	public partial class invln_IsConsortiumExistForProgrammeAndOrganisationResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_isconsortiumexist = "invln_isconsortiumexist";
		}
		
		public const string ActionLogicalName = "invln_IsConsortiumExistForProgrammeAndOrganisation";
		
		public invln_IsConsortiumExistForProgrammeAndOrganisationResponse()
		{
		}
		
		public bool invln_isconsortiumexist
		{
			get
			{
				if (this.Results.Contains("invln_isconsortiumexist"))
				{
					return ((bool)(this.Results["invln_isconsortiumexist"]));
				}
				else
				{
					return default(bool);
				}
			}
		}
	}
}
#pragma warning restore CS1591
