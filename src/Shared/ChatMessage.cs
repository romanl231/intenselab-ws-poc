using MessagePack;

namespace Shared {

	[MessagePackObject]
	public class ChatMessage
	{
		[Key(0)]
		public string Sender { get; set; } = string.Empty;

		[Key(1)]
		public string Receiver { get; set; } = string.Empty;
		
		[Key(2)]
		public string Text {get; set; } = string.Empty;

		[Key(3)]
		public string Event { get; set; } = string.Empty;
	}
}