using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000471 RID: 1137
	[Serializable]
	public class EventCallback
	{
		// Token: 0x0600334D RID: 13133 RVA: 0x001561BF File Offset: 0x001543BF
		public void Execute(UnityEngine.Object Sender = null)
		{
			if (this.HasCallback() && Application.isPlaying)
			{
				this.Target.gameObject.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x001561E8 File Offset: 0x001543E8
		public bool HasCallback()
		{
			return this.Target != null && !string.IsNullOrEmpty(this.MethodName);
		}

		// Token: 0x040020D3 RID: 8403
		public MonoBehaviour Target;

		// Token: 0x040020D4 RID: 8404
		public string MethodName = string.Empty;
	}
}
