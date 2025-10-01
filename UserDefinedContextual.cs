using System;
using UnityEngine;

// Token: 0x0200035F RID: 863
public class UserDefinedContextual : MonoBehaviour
{
	// Token: 0x060028D2 RID: 10450 RVA: 0x0011FF68 File Offset: 0x0011E168
	public void Init(string label, int id, NetworkPhysicsObject npo = null, Vector3? location = null)
	{
		this.Label = label;
		TextCode.LocalizeUIText(ref this.Label);
		UILabel component = base.GetComponent<UILabel>();
		component.text += this.Label;
		this.UserContextID = id;
		this.Target = npo;
		this.Location = location;
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x0011FFBC File Offset: 0x0011E1BC
	public void Commit()
	{
		if (this.Target)
		{
			this.Target.ContextualMenu(NetworkID.ID, this.UserContextID);
			return;
		}
		NetworkSingleton<UserDefinedContextualManager>.Instance.ContextualMenu(NetworkID.ID, this.UserContextID, this.Location);
	}

	// Token: 0x04001AD3 RID: 6867
	public string Label;

	// Token: 0x04001AD4 RID: 6868
	public int UserContextID;

	// Token: 0x04001AD5 RID: 6869
	public NetworkPhysicsObject Target;

	// Token: 0x04001AD6 RID: 6870
	public Vector3? Location;
}
