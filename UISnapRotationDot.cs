using System;
using UnityEngine;

// Token: 0x0200033E RID: 830
public class UISnapRotationDot : MonoBehaviour
{
	// Token: 0x06002778 RID: 10104 RVA: 0x00118870 File Offset: 0x00116A70
	private void OnEnable()
	{
		this.SnapPoint = base.GetComponent<UIAttachToObject>().AttachTo.GetComponent<SnapPoint>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(this.SnapPoint.bRotate);
		}
	}

	// Token: 0x06002779 RID: 10105 RVA: 0x001188CC File Offset: 0x00116ACC
	private void Update()
	{
		if (this.Arrow.gameObject.activeSelf)
		{
			Vector3 eulerAngles = this.Arrow.transform.eulerAngles;
			eulerAngles.z = -this.SnapPoint.transform.eulerAngles.y;
			eulerAngles.z += Camera.main.transform.eulerAngles.y;
			this.Arrow.transform.eulerAngles = eulerAngles;
		}
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x0011894C File Offset: 0x00116B4C
	public void Right()
	{
		Vector3 eulerAngles = this.SnapPoint.transform.eulerAngles;
		eulerAngles.y += 15f;
		this.SnapPoint.transform.eulerAngles = eulerAngles;
		NetworkSingleton<SnapPointManager>.Instance.UpdateSnapPoint(this.SnapPoint);
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x0011899C File Offset: 0x00116B9C
	public void Left()
	{
		Vector3 eulerAngles = this.SnapPoint.transform.eulerAngles;
		eulerAngles.y -= 15f;
		this.SnapPoint.transform.eulerAngles = eulerAngles;
		NetworkSingleton<SnapPointManager>.Instance.UpdateSnapPoint(this.SnapPoint);
	}

	// Token: 0x040019E9 RID: 6633
	public UISprite Arrow;

	// Token: 0x040019EA RID: 6634
	private SnapPoint SnapPoint;
}
