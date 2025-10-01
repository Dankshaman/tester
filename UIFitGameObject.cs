using System;
using UnityEngine;

// Token: 0x020002CA RID: 714
public class UIFitGameObject : MonoBehaviour
{
	// Token: 0x06002318 RID: 8984 RVA: 0x000F9D94 File Offset: 0x000F7F94
	private void Awake()
	{
		NGUIHelper.FitGameObjectToUI(this.FitObject, base.transform, this.Offset, Vector3.one, this.Size, null, null);
	}

	// Token: 0x04001642 RID: 5698
	public GameObject FitObject;

	// Token: 0x04001643 RID: 5699
	public float Size = 100f;

	// Token: 0x04001644 RID: 5700
	public Vector3 Offset = Vector3.zero;
}
