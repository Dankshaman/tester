using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NewNet
{
	// Token: 0x020003A7 RID: 935
	[RequireComponent(typeof(NetworkView))]
	public class NetworkBehavior : MonoBehaviour
	{
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x00134DAC File Offset: 0x00132FAC
		public NetworkView networkView
		{
			get
			{
				if (!this._networkView && this != null)
				{
					this._networkView = base.GetComponent<NetworkView>();
				}
				return this._networkView;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06002BFA RID: 11258 RVA: 0x00134DD6 File Offset: 0x00132FD6
		public string InternalName
		{
			get
			{
				if (!this.networkView)
				{
					return Utilities.RemoveCloneFromName(base.gameObject.name);
				}
				return this.networkView.InternalName;
			}
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00134E01 File Offset: 0x00133001
		protected void DirtySync([CallerMemberName] string callMember = null)
		{
			if (this.networkView)
			{
				this.networkView.DirtySync(this, callMember);
			}
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual void OnSync()
		{
		}

		// Token: 0x04001DB3 RID: 7603
		private NetworkView _networkView;
	}
}
