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
	
	
	/// <summary>
	/// Indicates model's recent training status
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum msdyn_iermlModeltrainingstatus
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Loadingdata = 100000007,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Nottrained = 100000000,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Publishcompleted = 100000006,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Publishfailed = 100000005,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Publishinprogress = 100000004,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Trainingcompleted = 100000002,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Trainingfailed = 100000003,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Traininginprogress = 100000001,
	}
}
#pragma warning restore CS1591
