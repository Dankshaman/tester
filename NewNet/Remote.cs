using System;

namespace NewNet
{
	// Token: 0x020003A5 RID: 933
	[AttributeUsage(AttributeTargets.Method)]
	public class Remote : BaseNetworkAttribute
	{
		// Token: 0x06002BEF RID: 11247 RVA: 0x00134C9D File Offset: 0x00132E9D
		public Remote(Permission permission = Permission.Client, SendType sendType = SendType.ReliableBuffered, string validationFunction = null, SerializationMethod serializationMethod = SerializationMethod.Default)
		{
			this.permission = permission;
			this.sendType = sendType;
			this.validationFunction = validationFunction;
			this.serializationMethod = serializationMethod;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x00134CC9 File Offset: 0x00132EC9
		public Remote(Permission permission)
		{
			this.permission = permission;
			this.sendType = SendType.ReliableBuffered;
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00134CE6 File Offset: 0x00132EE6
		public Remote(SendType sendType)
		{
			this.permission = Permission.Client;
			this.sendType = sendType;
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x00134D03 File Offset: 0x00132F03
		public Remote(string validationFunction)
		{
			this.permission = Permission.Client;
			this.validationFunction = validationFunction;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x00134D20 File Offset: 0x00132F20
		public Remote(SerializationMethod serializationMethod)
		{
			this.permission = Permission.Client;
			this.serializationMethod = serializationMethod;
		}

		// Token: 0x04001DB1 RID: 7601
		public SendType sendType = SendType.ReliableBuffered;
	}
}
