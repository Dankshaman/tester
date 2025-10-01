using System;

namespace NewNet
{
	// Token: 0x020003A6 RID: 934
	[AttributeUsage(AttributeTargets.Property)]
	public class Sync : BaseNetworkAttribute
	{
		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x00134D3D File Offset: 0x00132F3D
		public bool ignoreDirtyCheck { get; }

		// Token: 0x06002BF5 RID: 11253 RVA: 0x00134D45 File Offset: 0x00132F45
		public Sync(Permission permission = Permission.Owner, string validationFunction = null, SerializationMethod serializationMethod = SerializationMethod.Default, bool ignoreDirtyCheck = false)
		{
			this.permission = permission;
			this.validationFunction = validationFunction;
			this.serializationMethod = serializationMethod;
			this.ignoreDirtyCheck = ignoreDirtyCheck;
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00134D6A File Offset: 0x00132F6A
		public Sync(SerializationMethod serializationMethod)
		{
			this.permission = Permission.Owner;
			this.serializationMethod = serializationMethod;
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00134D80 File Offset: 0x00132F80
		public Sync(string validationFunction)
		{
			this.permission = Permission.Owner;
			this.validationFunction = validationFunction;
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x00134D96 File Offset: 0x00132F96
		public Sync(bool ignoreDirtyCheck)
		{
			this.permission = Permission.Owner;
			this.ignoreDirtyCheck = ignoreDirtyCheck;
		}
	}
}
