using System;
using System.Collections.Generic;

namespace NewNet
{
	// Token: 0x020003A4 RID: 932
	public class BaseNetworkAttribute : Attribute
	{
		// Token: 0x04001DAD RID: 7597
		public Permission permission;

		// Token: 0x04001DAE RID: 7598
		public string validationFunction;

		// Token: 0x04001DAF RID: 7599
		public SerializationMethod serializationMethod;

		// Token: 0x04001DB0 RID: 7600
		public static Dictionary<string, Func<NetworkPlayer, bool>> validationFunctions = new Dictionary<string, Func<NetworkPlayer, bool>>();
	}
}
