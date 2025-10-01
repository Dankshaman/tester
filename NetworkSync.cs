using System;
using NewNet;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class NetworkSync : NetworkBehavior
{
	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x0600177C RID: 6012 RVA: 0x000A0798 File Offset: 0x0009E998
	// (set) Token: 0x0600177D RID: 6013 RVA: 0x000A07A0 File Offset: 0x0009E9A0
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int id_
	{
		get
		{
			return this.id;
		}
		set
		{
			if (value != this.id)
			{
				this.id = value;
				base.DirtySync("id_");
			}
		}
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000A07C0 File Offset: 0x0009E9C0
	private void Awake()
	{
		this.RefreshSetInactive();
		this.rigidbody = base.gameObject.AddComponent<Rigidbody>();
		this.rigidbody.isKinematic = true;
		if (Network.isServer)
		{
			UnityEngine.Object.Instantiate<GameObject>(base.transform.root.GetComponent<Pointer>().ShakeDetectorObject, base.transform.position, Quaternion.identity).transform.parent = base.transform;
		}
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x000A0831 File Offset: 0x0009EA31
	public void RefreshSetInactive()
	{
		if (base.networkView.isMine)
		{
			base.CancelInvoke("SetInactive");
			base.Invoke("SetInactive", 0.33f);
		}
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x000A085B File Offset: 0x0009EA5B
	private void SetInactive()
	{
		this.id_ = -1;
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000D78 RID: 3448
	private int id = -1;

	// Token: 0x04000D79 RID: 3449
	protected Rigidbody rigidbody;
}
