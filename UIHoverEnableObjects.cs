using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class UIHoverEnableObjects : MonoBehaviour
{
	// Token: 0x06002449 RID: 9289 RVA: 0x001006A7 File Offset: 0x000FE8A7
	private void OnEnable()
	{
		this.OnHover(UICamera.HoveredUIObject == base.gameObject);
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x001006BF File Offset: 0x000FE8BF
	private void OnHover(bool bHover)
	{
		if (!base.enabled)
		{
			return;
		}
		if (bHover)
		{
			base.CancelInvoke("CheckHover");
			this.SetHoverEnabled(true);
			return;
		}
		base.InvokeRepeating("CheckHover", 0f, 0.05f);
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x001006F8 File Offset: 0x000FE8F8
	private void CheckHover()
	{
		GameObject hoveredUIObject = UICamera.HoveredUIObject;
		for (int i = 0; i < this.HoverEnableObjects.Count; i++)
		{
			GameObject gameObject = this.HoverEnableObjects[i];
			if ((gameObject && hoveredUIObject == gameObject) || (!this.DisableIfNoHover && !hoveredUIObject) || (this.EnableIfSelected && gameObject && UICamera.selectedObject == gameObject))
			{
				return;
			}
		}
		base.CancelInvoke("CheckHover");
		this.SetHoverEnabled(false);
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x00100780 File Offset: 0x000FE980
	public void SetHoverEnabled(bool bActive)
	{
		foreach (GameObject gameObject in this.HoverEnableObjects)
		{
			if (gameObject)
			{
				gameObject.SetActive(bActive);
				if (this.OnlyEnableDisableFirstObject)
				{
					break;
				}
			}
		}
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x001007E8 File Offset: 0x000FE9E8
	private void OnDragStart()
	{
		this.SetHoverEnabled(false);
	}

	// Token: 0x0600244E RID: 9294 RVA: 0x001007E8 File Offset: 0x000FE9E8
	private void OnDisable()
	{
		this.SetHoverEnabled(false);
	}

	// Token: 0x0400174A RID: 5962
	public List<GameObject> HoverEnableObjects = new List<GameObject>();

	// Token: 0x0400174B RID: 5963
	public bool OnlyEnableDisableFirstObject;

	// Token: 0x0400174C RID: 5964
	public bool DisableIfNoHover;

	// Token: 0x0400174D RID: 5965
	public bool EnableIfSelected;
}
