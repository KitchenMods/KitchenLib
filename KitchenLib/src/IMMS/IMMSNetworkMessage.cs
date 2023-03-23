using MessagePack;

namespace KitchenLib.IMMS
{
	[MessagePackObject]
	public struct IMMSNetworkMessage
	{
		[Key(0)]
		public int Id;
		[Key(1)]
		public string Channel;
		[Key(2)]
		public string Key;
		[Key(3)]
		public int Source;
		[Key(4)]
		public int Target;
		[Key(5)]
		public IMMSMessageType Type;
		[Key(6)]
		public object[] Arguments;
	}
}
