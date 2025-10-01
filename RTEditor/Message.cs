using System;

namespace RTEditor
{
	// Token: 0x0200043C RID: 1084
	public abstract class Message
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060031CB RID: 12747 RVA: 0x00150FFE File Offset: 0x0014F1FE
		public MessageType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x00151006 File Offset: 0x0014F206
		public Message(MessageType type)
		{
			this._type = type;
		}

		// Token: 0x04002020 RID: 8224
		private MessageType _type;
	}
}
