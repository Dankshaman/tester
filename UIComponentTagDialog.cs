using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class UIComponentTagDialog : Singleton<UIComponentTagDialog>
{
	// Token: 0x060021AA RID: 8618 RVA: 0x000F2CF3 File Offset: 0x000F0EF3
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnAvailableTagsChange += this.OnAvailableTagsChange;
	}

	// Token: 0x060021AB RID: 8619 RVA: 0x000F2D0C File Offset: 0x000F0F0C
	private void OnDestroy()
	{
		EventManager.OnAvailableTagsChange -= this.OnAvailableTagsChange;
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x000F2D20 File Offset: 0x000F0F20
	public void OnEnable()
	{
		if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || ((this.TargetNPOs == null || this.TargetNPOs.Count == 0) && this.TargetSnapPoint == null && this.TargetFlags == null))
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.AddTagsContainer.SetActive(Network.isServer);
		this.AllTagsButton.SetActive(Network.isServer);
		this.RefreshTagLabels(true);
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x000F2D93 File Offset: 0x000F0F93
	private void OnAvailableTagsChange()
	{
		this.RefreshTagLabels(false);
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x000F2D9C File Offset: 0x000F0F9C
	public void OnDisable()
	{
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		for (int i = 0; i < this.TagContainer.RemovedTags.Count; i++)
		{
			NetworkSingleton<ComponentTags>.Instance.CheckRemoveTag(grabbableNPOs, this.TagContainer.RemovedTags[i]);
		}
		this.TagContainer.RemovedTags.Clear();
	}

	// Token: 0x060021AF RID: 8623 RVA: 0x000F2DFC File Offset: 0x000F0FFC
	public void EditFlags(ref List<ulong> flags)
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		this.TargetFlags = flags;
		this.TargetSnapPoint = null;
		this.TargetNPOs = null;
		Language.UpdateUILabel(this.Title, "Snap Point Creation Tags");
		Language.UpdateUITooltip(this.HelpTooltip, "New snap points will have these tags.", null);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x000F2E68 File Offset: 0x000F1068
	public void EditSnapPoint(SnapPointManager.SnapPointObject snapPointObject)
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		this.TargetFlags = null;
		this.TargetSnapPoint = snapPointObject;
		this.TargetNPOs = null;
		Language.UpdateUILabel(this.Title, "Snap Point Tags");
		Language.UpdateUITooltip(this.HelpTooltip, "Edit tags on this snap point.", null);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x000F2ED0 File Offset: 0x000F10D0
	public void EditNPOs(List<NetworkPhysicsObject> npos, string titleText, string tooltipText)
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		this.TargetFlags = null;
		this.TargetSnapPoint = null;
		this.TargetNPOs = npos;
		Language.UpdateUILabel(this.Title, titleText);
		Language.UpdateUITooltip(this.HelpTooltip, tooltipText, null);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x000F2F30 File Offset: 0x000F1130
	public void EditHiddenZoneTags()
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		this.EditZone(PlayerScript.PointerScript.InfoHiddenZoneGO);
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x000F2F4F File Offset: 0x000F114F
	public void EditRandomizeZoneTags()
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		this.EditZone(PlayerScript.PointerScript.InfoRandomizeZoneGO);
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x000F2F70 File Offset: 0x000F1170
	public void EditZone(GameObject zoneObject)
	{
		if (zoneObject)
		{
			if (this.TargetNPOs == null)
			{
				this.TargetNPOs = new List<NetworkPhysicsObject>();
			}
			else
			{
				this.TargetNPOs.Clear();
			}
			this.TargetNPOs.Add(zoneObject.GetComponent<NetworkPhysicsObject>());
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060021B5 RID: 8629 RVA: 0x000F2FC4 File Offset: 0x000F11C4
	public void RefreshTagLabels(bool updateCurrentTags = false)
	{
		List<ComponentTag> list = new List<ComponentTag>();
		for (int i = 0; i <= NetworkSingleton<ComponentTags>.Instance.LastTag; i++)
		{
			if (NetworkSingleton<ComponentTags>.Instance.IsTagActive(i))
			{
				list.Add(new ComponentTag(i, NetworkSingleton<ComponentTags>.Instance.TagLabelFromIndex(i)));
			}
		}
		if (updateCurrentTags)
		{
			int num = 0;
			List<ulong> list2 = null;
			if (this.TargetFlags != null)
			{
				list2 = this.TargetFlags;
			}
			else if (this.TargetSnapPoint != null)
			{
				list2 = this.TargetSnapPoint.snapPoint.tags;
			}
			else
			{
				for (int j = 0; j < this.TargetNPOs.Count; j++)
				{
					NetworkPhysicsObject networkPhysicsObject = this.TargetNPOs[j];
					int flagCount = ComponentTags.GetFlagCount(networkPhysicsObject.tags);
					if (flagCount > num)
					{
						list2 = networkPhysicsObject.tags;
						num = flagCount;
					}
				}
			}
			if (list2 != null)
			{
				ComponentTags.CopyFlags(ref this.currentTags, list2);
			}
			else
			{
				this.currentTags.Clear();
			}
		}
		else
		{
			ComponentTags.CopyFlags(ref this.currentTags, this.TagContainer.CurrentTags);
		}
		if (this.TargetFlags != null)
		{
			this.TagContainer.EditTags(ref this.TargetFlags, list, this.currentTags);
		}
		else if (this.TargetSnapPoint != null)
		{
			this.TagContainer.EditTags(this.TargetSnapPoint, list, this.currentTags);
		}
		else
		{
			this.TagContainer.EditTags(this.TargetNPOs, list, this.currentTags);
		}
		this.ScrollView.UpdateScrollbars(true);
		this.ScrollView.ResetPosition();
		this.ScrollView.UpdatePosition();
	}

	// Token: 0x060021B6 RID: 8630 RVA: 0x000F3140 File Offset: 0x000F1340
	public void AddTag()
	{
		string value = this.NewTag.value;
		int num = NetworkSingleton<ComponentTags>.Instance.AddTag(value);
		if (num >= 0)
		{
			if (this.TargetFlags != null)
			{
				ComponentTags.SetFlag(ref this.TargetFlags, num, true);
			}
			else if (this.TargetSnapPoint != null)
			{
				this.TargetSnapPoint.snapPoint.SetTag(num, true);
			}
			else if (this.TargetNPOs != null)
			{
				for (int i = 0; i < this.TargetNPOs.Count; i++)
				{
					this.TargetNPOs[i].SetTag(num, true);
				}
			}
			ComponentTags.SetFlag(ref this.TagContainer.CurrentTags, num, true);
			this.RefreshTagLabels(false);
			EventManager.TriggerAvailableTagsChange();
		}
		this.NewTag.value = "";
	}

	// Token: 0x0400150A RID: 5386
	public UIComponentTagContainer TagContainer;

	// Token: 0x0400150B RID: 5387
	public UILabel Title;

	// Token: 0x0400150C RID: 5388
	public UITooltipScript HelpTooltip;

	// Token: 0x0400150D RID: 5389
	[NonSerialized]
	public List<ulong> TargetFlags;

	// Token: 0x0400150E RID: 5390
	[NonSerialized]
	public SnapPointManager.SnapPointObject TargetSnapPoint;

	// Token: 0x0400150F RID: 5391
	[NonSerialized]
	public List<NetworkPhysicsObject> TargetNPOs;

	// Token: 0x04001510 RID: 5392
	public UIScrollView ScrollView;

	// Token: 0x04001511 RID: 5393
	public UIInput NewTag;

	// Token: 0x04001512 RID: 5394
	public GameObject AddTagsContainer;

	// Token: 0x04001513 RID: 5395
	public GameObject AllTagsButton;

	// Token: 0x04001514 RID: 5396
	private List<ulong> currentTags = new List<ulong>();
}
