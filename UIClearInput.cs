using System;
using UnityEngine;

// Token: 0x0200028A RID: 650
public class UIClearInput : MonoBehaviour
{
	// Token: 0x06002175 RID: 8565 RVA: 0x000F1320 File Offset: 0x000EF520
	private void Awake()
	{
		this.input = base.GetComponent<UIInput>();
		if (!this.input)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		GameObject prefab = Resources.Load<GameObject>("Clear Input");
		this.clearXButton = base.gameObject.AddChild(prefab).GetComponent<UIButton>();
		UIWidget component = this.input.GetComponent<UIWidget>();
		UIWidget component2 = this.clearXButton.GetComponent<UIWidget>();
		component2.depth = component.depth + 1;
		Vector3[] localCorners = component.localCorners;
		Vector3 localPosition = new Vector3(localCorners[3].x - (float)(component2.width / 2), (localCorners[2].y + localCorners[3].y) / 2f, localCorners[3].z);
		this.clearXButton.transform.localPosition = localPosition;
		this.clearXButton.transform.RoundLocalPosition();
		EventDelegate.Add(this.input.onChange, new EventDelegate.Callback(this.OnInputChange));
		EventDelegate.Add(this.clearXButton.onClick, new EventDelegate.Callback(this.OnClickClearXButton));
		this.OnInputChange();
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x000F1448 File Offset: 0x000EF648
	private void Update()
	{
		Vector3[] localCorners = this.input.GetComponent<UIWidget>().localCorners;
		UIWidget component = this.clearXButton.GetComponent<UIWidget>();
		Vector3 localPosition = new Vector3(localCorners[3].x - (float)(component.width / 2), (localCorners[2].y + localCorners[3].y) / 2f, localCorners[3].z);
		this.clearXButton.transform.localPosition = localPosition;
		this.clearXButton.transform.RoundLocalPosition();
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x000F14DC File Offset: 0x000EF6DC
	private void OnDestroy()
	{
		EventDelegate.Remove(this.input.onChange, new EventDelegate.Callback(this.OnInputChange));
		if (this.clearXButton)
		{
			EventDelegate.Remove(this.clearXButton.onClick, new EventDelegate.Callback(this.OnClickClearXButton));
		}
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x000F1530 File Offset: 0x000EF730
	private void OnInputChange()
	{
		this.clearXButton.gameObject.SetActive(!string.IsNullOrEmpty(this.input.value));
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x000F1555 File Offset: 0x000EF755
	private void OnClickClearXButton()
	{
		this.input.value = "";
	}

	// Token: 0x040014C3 RID: 5315
	private UIInput input;

	// Token: 0x040014C4 RID: 5316
	private UIButton clearXButton;
}
