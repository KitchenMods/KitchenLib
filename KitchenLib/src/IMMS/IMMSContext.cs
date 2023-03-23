using MessagePack;

namespace KitchenLib.IMMS
{
	/// <summary>
	/// Context of the message, including channel and sender/target.
	/// </summary>
	[MessagePackObject]
	public struct IMMSContext
	{
		/// <summary>
		/// The channel of the message.
		/// </summary>
		[Key(0)]
		public string Channel;

		/// <summary>
		/// The ID of the client that this message came from, or -1 if this is an Internal message.
		/// </summary>
		[Key(1)]
		public int Source;

		/// <summary>
		/// The ID of the intended recipient of this message, or -1 if not applicable.
		/// </summary>
		[Key(2)]
		public int Target;

		/// <summary>
		/// The direction of the message.
		/// </summary>
		[Key(3)]
		public IMMSMessageType Type;
	}
}
