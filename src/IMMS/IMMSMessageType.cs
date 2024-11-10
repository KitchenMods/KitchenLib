namespace KitchenLib.IMMS
{
	/// <summary>
	/// Specifies the client-server direction of a message.
	/// </summary>
	public enum IMMSMessageType
	{
		/// <summary>
		/// Send from a mod to another mod within the same client.
		/// </summary>
		Internal,
		/// <summary>
		/// Sent from host to a specific client.
		/// </summary>
		HostToClient,
		/// <summary>
		/// Sent from a specific client to host.
		/// </summary>
		ClientToHost,
		/// <summary>
		/// Sent from specific client to another client.
		/// </summary>
		ClientToClient
	}
}
