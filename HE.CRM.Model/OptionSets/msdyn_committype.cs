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
	/// Resource booking Commitment Type
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum msdyn_committype
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Canceled = 192350004,
		
		/// <summary>
		/// Denotes a state of confirmed booking of a resource.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		HardBook = 192350001,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		None = 192350000,
		
		/// <summary>
		/// Denotes a state when a resource is proposed by a Resource manager to fulfill a resource request.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Proposed = 192350003,
		
		/// <summary>
		/// Denotes a state when a resource is tentatively booked.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		SoftBook = 192350002,
	}
}
#pragma warning restore CS1591
