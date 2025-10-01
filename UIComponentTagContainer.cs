using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class UIComponentTagContainer : MonoBehaviour
{
	// Token: 0x1400005F RID: 95
	// (add) Token: 0x0600219C RID: 8604 RVA: 0x000F2604 File Offset: 0x000F0804
	// (remove) Token: 0x0600219D RID: 8605 RVA: 0x000F263C File Offset: 0x000F083C
	public event EventHandler TagsRepositioned;

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x0600219E RID: 8606 RVA: 0x000F2671 File Offset: 0x000F0871
	public int Width
	{
		get
		{
			return base.gameObject.GetComponent<UISprite>().width;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x0600219F RID: 8607 RVA: 0x000F2683 File Offset: 0x000F0883
	public int Height
	{
		get
		{
			return base.gameObject.GetComponent<UISprite>().height;
		}
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000F2695 File Offset: 0x000F0895
	public void EditTags(ref List<ulong> flagsToEdit, List<ComponentTag> tags, List<ulong> selectedTags)
	{
		this.EditTags(flagsToEdit, null, null, tags, selectedTags);
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000F26A3 File Offset: 0x000F08A3
	public void EditTags(SnapPointManager.SnapPointObject snapPoint, List<ComponentTag> tags, List<ulong> selectedTags)
	{
		this.EditTags(null, snapPoint, null, tags, selectedTags);
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000F26B0 File Offset: 0x000F08B0
	public void EditTags(List<NetworkPhysicsObject> nposToEdit, List<ComponentTag> tags, List<ulong> selectedTags)
	{
		this.EditTags(null, null, nposToEdit, tags, selectedTags);
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000F26C0 File Offset: 0x000F08C0
	public void EditTags(List<ulong> flagsToEdit, SnapPointManager.SnapPointObject snapPointObjectToEdit, List<NetworkPhysicsObject> nposToEdit, List<ComponentTag> tags, List<ulong> selectedTags)
	{
		this.Tags.Clear();
		for (int i = 0; i < tags.Count; i++)
		{
			this.Tags.Add(tags[i]);
		}
		this.Tags.Sort((ComponentTag a, ComponentTag b) => a.label.normalized.CompareTo(b.label.normalized));
		this.TargetFlags = flagsToEdit;
		this.TargetSnapPointObject = snapPointObjectToEdit;
		this.TargetNPOs = nposToEdit;
		ComponentTags.CopyFlags(ref this.CurrentTags, selectedTags);
		if (this._initialized)
		{
			this.RemoveAllTags();
			if (this.Tags.Count > 0)
			{
				for (int j = 0; j < this.Tags.Count; j++)
				{
					this.AddTag(this.Tags[j].label, this.Tags[j].index, ComponentTags.GetFlag(selectedTags, this.Tags[j].index));
				}
				this.PositionTag();
				return;
			}
		}
		else
		{
			this.Initialize();
		}
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x000F27C8 File Offset: 0x000F09C8
	public void AddTag(TagLabel label, int index, bool active = false)
	{
		UITag component = UnityEngine.Object.Instantiate<GameObject>(this.TagPrefab).GetComponent<UITag>();
		component.Toggle.value = active;
		component.transform.parent = base.gameObject.transform;
		component.transform.localScale = Vector3.one;
		component.name = label.normalized;
		component.Text = label.displayed;
		component.ComponentTagIndex = index;
		component.Resize();
		if (active)
		{
			component.OnAddClicked(null);
		}
		component.PropertyChanged += this.TagObjectOnPropertyChanged;
		this.TagButtons.Add(component);
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000F2868 File Offset: 0x000F0A68
	public void RemoveAllTags()
	{
		for (int i = this.TagButtons.Count - 1; i >= 0; i--)
		{
			UITag uitag = this.TagButtons[i];
			if (uitag != null)
			{
				uitag.PropertyChanged -= this.TagObjectOnPropertyChanged;
				this.TagButtons.Remove(uitag);
				UnityEngine.Object.Destroy(uitag.gameObject);
			}
		}
		base.gameObject.GetComponent<UISprite>().height = this.MinHeight;
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000F28E4 File Offset: 0x000F0AE4
	private void Initialize()
	{
		if (this._initialized)
		{
			return;
		}
		this._initialized = true;
		this._containerWidth = base.gameObject.GetComponent<UISprite>().width;
		if (this.Tags.Count > 0)
		{
			if (this.TargetFlags != null)
			{
				for (int i = 0; i < this.Tags.Count; i++)
				{
					this.AddTag(this.Tags[i].label, this.Tags[i].index, ComponentTags.GetFlag(this.TargetFlags, this.Tags[i].index));
				}
			}
			else if (this.TargetSnapPointObject != null)
			{
				for (int j = 0; j < this.Tags.Count; j++)
				{
					this.AddTag(this.Tags[j].label, this.Tags[j].index, ComponentTags.GetFlag(this.TargetSnapPointObject.snapPoint.tags, this.Tags[j].index));
				}
			}
			else
			{
				for (int k = 0; k < this.Tags.Count; k++)
				{
					this.AddTag(this.Tags[k].label, this.Tags[k].index, this.TargetNPOs[0].TagIsSet(this.Tags[k].index));
				}
			}
			this.PositionTag();
		}
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x000F2A64 File Offset: 0x000F0C64
	private void PositionTag()
	{
		List<Vector3> list = new List<Vector3>();
		int width = base.gameObject.GetComponent<UISprite>().width;
		int num = -this.OffsetY;
		float num2 = 7.5f;
		for (int i = 0; i < this.TagButtons.Count; i++)
		{
			int width2 = this.TagButtons[i].gameObject.GetComponent<UISprite>().width;
			if (num2 + 15f + (float)width2 > (float)width - 7.5f)
			{
				num2 = 7.5f;
				num -= 38;
			}
			float x = (float)(-(float)width) / 2f + num2 + (float)width2 / 2f + 5f;
			list.Add(new Vector3(x, (float)num, 0f));
			num2 += 5f + (float)width2;
		}
		num -= 25;
		int num3 = num * -1;
		if (this.MinHeight > 0 && num3 < this.MinHeight)
		{
			num3 = this.MinHeight;
		}
		base.gameObject.GetComponent<UISprite>().height = num3;
		float num4 = (float)num3 * 0.5f - 25f - list[0].y;
		for (int j = 0; j < this.TagButtons.Count; j++)
		{
			UnityEngine.Component component = this.TagButtons[j];
			Vector3 localPosition = list[j];
			localPosition.y += num4;
			component.gameObject.transform.localPosition = localPosition;
		}
		EventHandler tagsRepositioned = this.TagsRepositioned;
		if (tagsRepositioned == null)
		{
			return;
		}
		tagsRepositioned(this, EventArgs.Empty);
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000F2BEC File Offset: 0x000F0DEC
	private void TagObjectOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		UITag uitag;
		if ((uitag = (sender as UITag)) == null)
		{
			return;
		}
		if (this.TargetFlags != null)
		{
			ComponentTags.SetFlag(ref this.TargetFlags, uitag.ComponentTagIndex, uitag.Active);
		}
		else if (this.TargetSnapPointObject != null)
		{
			this.TargetSnapPointObject.snapPoint.SetTag(uitag.ComponentTagIndex, uitag.Active);
		}
		else
		{
			for (int i = 0; i < this.TargetNPOs.Count; i++)
			{
				this.TargetNPOs[i].SetTag(uitag.ComponentTagIndex, uitag.Active);
			}
		}
		ComponentTags.SetFlag(ref this.CurrentTags, uitag.ComponentTagIndex, uitag.Active);
		if (this.TargetFlags == null)
		{
			if (uitag.Active)
			{
				this.RemovedTags.Remove(uitag.ComponentTagIndex);
				return;
			}
			this.RemovedTags.TryAddUnique(uitag.ComponentTagIndex);
		}
	}

	// Token: 0x040014FC RID: 5372
	public GameObject TagPrefab;

	// Token: 0x040014FD RID: 5373
	public readonly List<UITag> TagButtons = new List<UITag>();

	// Token: 0x040014FF RID: 5375
	public bool RemoveIsDelete;

	// Token: 0x04001500 RID: 5376
	public int OffsetY;

	// Token: 0x04001501 RID: 5377
	public int MinHeight;

	// Token: 0x04001502 RID: 5378
	private int _containerWidth;

	// Token: 0x04001503 RID: 5379
	private bool _initialized;

	// Token: 0x04001504 RID: 5380
	public List<ComponentTag> Tags = new List<ComponentTag>();

	// Token: 0x04001505 RID: 5381
	private List<ulong> TargetFlags;

	// Token: 0x04001506 RID: 5382
	private SnapPointManager.SnapPointObject TargetSnapPointObject;

	// Token: 0x04001507 RID: 5383
	private List<NetworkPhysicsObject> TargetNPOs;

	// Token: 0x04001508 RID: 5384
	public List<ulong> CurrentTags;

	// Token: 0x04001509 RID: 5385
	public List<int> RemovedTags = new List<int>();
}
