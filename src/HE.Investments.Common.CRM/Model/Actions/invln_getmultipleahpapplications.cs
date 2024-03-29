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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getmultipleahpapplications")]
	public partial class invln_getmultipleahpapplicationsRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string inlvn_userid = "inlvn_userid";
			public const string invln_organisationid = "invln_organisationid";
			public const string invln_appfieldstoretrieve = "invln_appfieldstoretrieve";
		}
		
		public const string ActionLogicalName = "invln_getmultipleahpapplications";
		
		public string inlvn_userid
		{
			get
			{
				if (this.Parameters.Contains("inlvn_userid"))
				{
					return ((string)(this.Parameters["inlvn_userid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["inlvn_userid"] = value;
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
		
		public string invln_appfieldstoretrieve
		{
			get
			{
				if (this.Parameters.Contains("invln_appfieldstoretrieve"))
				{
					return ((string)(this.Parameters["invln_appfieldstoretrieve"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_appfieldstoretrieve"] = value;
			}
		}
		
		public invln_getmultipleahpapplicationsRequest()
		{
			this.RequestName = "invln_getmultipleahpapplications";
			this.inlvn_userid = default(string);
			this.invln_organisationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getmultipleahpapplications")]
	public partial class invln_getmultipleahpapplicationsResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_ahpapplications = "invln_ahpapplications";
		}
		
		public const string ActionLogicalName = "invln_getmultipleahpapplications";
		
		public invln_getmultipleahpapplicationsResponse()
		{
		}
		
		public string invln_ahpapplications
		{
			get
			{
				if (this.Results.Contains("invln_ahpapplications"))
				{
					return ((string)(this.Results["invln_ahpapplications"]));
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
