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
	
	
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum msdyn_liveworkitemeventtype
	{
		
		/// <summary>
		/// This event is triggered when the chat context is updated.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		ContextUpdated = 192350002,
		
		/// <summary>
		/// This event is triggered when a customer initiates a conversation, that is when a live work item is created.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Created = 192350001,
		
		/// <summary>
		/// This event is triggered when a chat is rejoined.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		CustomerRejoin = 192350003,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Default = 192350000,
		
		/// <summary>
		/// This event is triggered when a skill is updated during chat.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		SkillIdentified = 192350005,
	}
}
#pragma warning restore CS1591
