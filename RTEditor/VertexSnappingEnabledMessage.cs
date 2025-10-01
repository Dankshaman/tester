using System;

namespace RTEditor
{
	// Token: 0x02000438 RID: 1080
	public class VertexSnappingEnabledMessage : Message
	{
		// Token: 0x060031C2 RID: 12738 RVA: 0x00150FAF File Offset: 0x0014F1AF
		public VertexSnappingEnabledMessage() : base(MessageType.VertexSnappingEnabled)
		{
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x00150FB8 File Offset: 0x0014F1B8
		public static void SendToInterestedListeners()
		{
			VertexSnappingEnabledMessage message = new VertexSnappingEnabledMessage();
			MonoSingletonBase<MessageListenerDatabase>.Instance.SendMessageToInterestedListeners(message);
		}
	}
}
