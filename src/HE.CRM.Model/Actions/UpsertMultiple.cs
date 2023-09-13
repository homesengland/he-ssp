#pragma warning disable CS1591
// Code Generated by DLaB.ModelBuilderExtensions
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
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/Contracts")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("UpsertMultiple")]
	public partial class UpsertMultipleRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string Targets = "Targets";
		}
		
		public const string ActionLogicalName = "UpsertMultiple";
		
		public Microsoft.Xrm.Sdk.EntityCollection Targets
		{
			get
			{
				if (this.Parameters.Contains("Targets"))
				{
					return ((Microsoft.Xrm.Sdk.EntityCollection)(this.Parameters["Targets"]));
				}
				else
				{
					return default(Microsoft.Xrm.Sdk.EntityCollection);
				}
			}
			set
			{
				this.Parameters["Targets"] = value;
			}
		}
		
		public UpsertMultipleRequest()
		{
			this.RequestName = "UpsertMultiple";
			this.Targets = default(Microsoft.Xrm.Sdk.EntityCollection);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/Contracts")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("UpsertMultiple")]
	public partial class UpsertMultipleResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string Results = "Results";
		}
		
		public const string ActionLogicalName = "UpsertMultiple";
		
		public UpsertMultipleResponse()
		{
		}

        //public Microsoft.Xrm.Sdk.Messages.UpsertResponse[] Results
        //{
        //    get
        //    {
        //        if (this.Results.Contains("Results"))
        //        {
        //            return ((Microsoft.Xrm.Sdk.Messages.UpsertResponse[])(this.Results["Results"]));
        //        }
        //        else
        //        {
        //            return default(Microsoft.Xrm.Sdk.Messages.UpsertResponse[]);
        //        }
        //    }
        //}
    }
}
#pragma warning restore CS1591
