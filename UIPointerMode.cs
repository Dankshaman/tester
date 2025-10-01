using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class UIPointerMode : MonoBehaviour
{
	// Token: 0x06002684 RID: 9860 RVA: 0x001128D8 File Offset: 0x00110AD8
	private void Start()
	{
		this.ThisBoxCollider = base.GetComponent<BoxCollider2D>();
		this.ThisUIButton = base.GetComponent<UIButton>();
		this.ThisLabelObject = NGUITools.GetChildLabel(base.gameObject).gameObject;
		if (base.gameObject.GetComponentInChildren<UIScrollView>())
		{
			BoxCollider2D[] componentsInChildren = base.gameObject.GetComponentInChildren<UIScrollView>().gameObject.GetComponentsInChildren<BoxCollider2D>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject != base.gameObject)
				{
					UIPointerMode.ExpandButtonStruct item = default(UIPointerMode.ExpandButtonStruct);
					item.Button = componentsInChildren[i].gameObject;
					item.ButtonTransform = componentsInChildren[i].transform;
					item.StartlocalPosition = componentsInChildren[i].transform.localPosition;
					this.ExpandButtonStructs.Add(item);
					if (!componentsInChildren[i].GetComponent<UIButton>())
					{
						componentsInChildren[i].gameObject.SetActive(false);
					}
				}
			}
		}
		this.HideExpand();
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x001129D0 File Offset: 0x00110BD0
	private void Update()
	{
		bool flag = UICamera.HoveredUIObject == base.gameObject;
		bool flag2 = this.CheckSecondaryInUse();
		if (PlayerScript.Pointer && PermissionsOptions.CheckIfAllowedInMode(this.ThisPointerMode, -1))
		{
			if (PlayerScript.PointerScript.CurrentPointerMode == this.ThisPointerMode || flag2)
			{
				if (new Colour(this.ThisUIButton.defaultColor) != Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonHighlightA])
				{
					this.ThisUIButton.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonHighlightA];
				}
			}
			else if (new Colour(this.ThisUIButton.defaultColor) != Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal])
			{
				this.ThisUIButton.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
			}
			this.ThisBoxCollider.enabled = true;
		}
		else
		{
			if (new Colour(this.ThisUIButton.defaultColor) != Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral])
			{
				this.ThisUIButton.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			}
			this.ThisBoxCollider.enabled = false;
		}
		if (flag2 || flag)
		{
			this.ShowExpand();
		}
		else
		{
			bool flag3 = false;
			for (int i = 0; i < this.ExpandButtonStructs.Count; i++)
			{
				UIPointerMode.ExpandButtonStruct expandButtonStruct = this.ExpandButtonStructs[i];
				if (UICamera.HoveredUIObject == expandButtonStruct.Button)
				{
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				this.HideExpand();
			}
			else
			{
				this.ShowExpand();
			}
		}
		bool flag4 = flag && (zInput.CurrentControlType == ControlType.Keyboard || zInput.CurrentControlType == ControlType.Touch);
		if (this.ThisLabelObject.activeSelf != flag4)
		{
			this.ThisLabelObject.SetActive(flag4);
		}
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x00112BB0 File Offset: 0x00110DB0
	private void OnClick()
	{
		if (PlayerScript.Pointer)
		{
			if (NetworkSingleton<Turns>.Instance.CanInteract(PlayerScript.PointerScript.PointerColorLabel))
			{
				PlayerScript.PointerScript.CurrentPointerMode = this.ThisPointerMode;
				return;
			}
			Chat.LogError("Cannot select this tool because it's not your turn.", true);
		}
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x00112BF0 File Offset: 0x00110DF0
	private bool CheckSecondaryInUse()
	{
		if (PlayerScript.Pointer)
		{
			for (int i = 0; i < this.ThisSecondaryModes.Length; i++)
			{
				if (PlayerScript.PointerScript.CurrentPointerMode == this.ThisSecondaryModes[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002688 RID: 9864 RVA: 0x00112C34 File Offset: 0x00110E34
	public void ShowExpand()
	{
		for (int i = 0; i < this.ExpandButtonStructs.Count; i++)
		{
			UIPointerMode.ExpandButtonStruct expandButtonStruct = this.ExpandButtonStructs[i];
			expandButtonStruct.ButtonTransform.localPosition = Vector3.Lerp(expandButtonStruct.ButtonTransform.localPosition, expandButtonStruct.StartlocalPosition, 10f * Time.deltaTime);
		}
		if (this.Lines != null)
		{
			this.Lines.transform.localPosition = Vector3.Lerp(this.Lines.transform.localPosition, new Vector3(0f, 0f, 0f), 15f * Time.deltaTime);
		}
	}

	// Token: 0x06002689 RID: 9865 RVA: 0x00112CE4 File Offset: 0x00110EE4
	private void HideExpand()
	{
		for (int i = 0; i < this.ExpandButtonStructs.Count; i++)
		{
			UIPointerMode.ExpandButtonStruct expandButtonStruct = this.ExpandButtonStructs[i];
			expandButtonStruct.ButtonTransform.localPosition = Vector3.Lerp(expandButtonStruct.ButtonTransform.localPosition, new Vector3(-150f, 24f, 0f), 10f * Time.deltaTime);
		}
		if (this.Lines != null)
		{
			this.Lines.transform.localPosition = Vector3.Lerp(this.Lines.transform.localPosition, new Vector3(-200f, 0f, 0f), 15f * Time.deltaTime);
		}
	}

	// Token: 0x0400191B RID: 6427
	public PointerMode ThisPointerMode;

	// Token: 0x0400191C RID: 6428
	public PointerMode[] ThisSecondaryModes;

	// Token: 0x0400191D RID: 6429
	public GameObject Lines;

	// Token: 0x0400191E RID: 6430
	private BoxCollider2D ThisBoxCollider;

	// Token: 0x0400191F RID: 6431
	private UIButton ThisUIButton;

	// Token: 0x04001920 RID: 6432
	private GameObject ThisLabelObject;

	// Token: 0x04001921 RID: 6433
	private List<UIPointerMode.ExpandButtonStruct> ExpandButtonStructs = new List<UIPointerMode.ExpandButtonStruct>();

	// Token: 0x0200077D RID: 1917
	private struct ExpandButtonStruct
	{
		// Token: 0x04002C6A RID: 11370
		public GameObject Button;

		// Token: 0x04002C6B RID: 11371
		public Transform ButtonTransform;

		// Token: 0x04002C6C RID: 11372
		public Vector3 StartlocalPosition;
	}
}
