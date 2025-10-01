using System;

namespace RTEditor
{
	// Token: 0x02000439 RID: 1081
	public class VertexSnappingDisabledMessage : Message
	{
		// Token: 0x060031C4 RID: 12740 RVA: 0x00150FD6 File Offset: 0x0014F1D6
		public VertexSnappingDisabledMessage() : base(MessageType.VertexSnappingDisabled)
		{
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x00150FE0 File Offset: 0x0014F1E0
		public static void SendToInterestedListeners()
		{
			VertexSnappingDisabledMessage message = new VertexSnappingDisabledMessage();
			MonoSingletonBase<MessageListenerDatabase>.Instance.SendMessageToInterestedListeners(message);
		}
	}
}
