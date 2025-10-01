using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : MonoBehaviour
{
	// Token: 0x0600005C RID: 92 RVA: 0x00003BF0 File Offset: 0x00001DF0
	public GameObject Attach(GameObject prefab)
	{
		if (this.mPrefab != prefab)
		{
			this.mPrefab = prefab;
			if (this.mChild != null)
			{
				UnityEngine.Object.Destroy(this.mChild);
			}
			if (this.mPrefab != null)
			{
				Transform transform = base.transform;
				this.mChild = UnityEngine.Object.Instantiate<GameObject>(this.mPrefab, transform.position, transform.rotation);
				Transform transform2 = this.mChild.transform;
				transform2.parent = transform;
				transform2.localPosition = Vector3.zero;
				transform2.localRotation = Quaternion.identity;
				transform2.localScale = Vector3.one;
			}
		}
		return this.mChild;
	}

	// Token: 0x04000033 RID: 51
	public InvBaseItem.Slot slot;

	// Token: 0x04000034 RID: 52
	private GameObject mPrefab;

	// Token: 0x04000035 RID: 53
	private GameObject mChild;
}
