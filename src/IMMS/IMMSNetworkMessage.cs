using MessagePack;

namespace KitchenLib.IMMS
{
	[MessagePackObject]
	public struct IMMSNetworkMessage
	{
		[Key(0)]
		public int Id;
		[Key(1)]
		public long Timestamp;
		[Key(2)]
		public string Channel;
		[Key(3)]
		public string Key;
		[Key(4)]
		public int Source;
		[Key(5)]
		public int Target;
		[Key(6)]
		public IMMSMessageType Type;
		[Key(7)]
		public object[] Arguments;
	}
}
