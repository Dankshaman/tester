using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000349 RID: 841
public class UITagEditor : Singleton<UITagEditor>
{
	// Token: 0x060027DF RID: 10207 RVA: 0x0011A3CA File Offset: 0x001185CA
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnLoadingComplete += this.LoadTags;
		EventManager.OnAvailableTagsChange += this.OnAvailableTagsChange;
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x0011A3F4 File Offset: 0x001185F4
	private void OnDestroy()
	{
		EventManager.OnLoadingComplete -= this.LoadTags;
		EventManager.OnAvailableTagsChange -= this.OnAvailableTagsChange;
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x0011A418 File Offset: 0x00118618
	private void OnEnable()
	{
		if (!Network.isServer)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.RecordSelection();
		this.LoadTags(false);
	}

	// Token: 0x060027E2 RID: 10210 RVA: 0x0011A43B File Offset: 0x0011863B
	private void LoadTags()
	{
		this.LoadTags(true);
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x0011A444 File Offset: 0x00118644
	private void OnAvailableTagsChange()
	{
		this.LoadTags(false);
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x0011A450 File Offset: 0x00118650
	private void LoadTags(bool retainSelection)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (!retainSelection)
		{
			this.SelectedTagIndex = -1;
		}
		this.TagContainer.DestroyChildren();
		List<ComponentTag> list = new List<ComponentTag>();
		foreach (KeyValuePair<int, TagLabel> keyValuePair in NetworkSingleton<ComponentTags>.Instance.activeLabels)
		{
			list.Add(new ComponentTag(keyValuePair.Key, keyValuePair.Value));
		}
		list.Sort((ComponentTag a, ComponentTag b) => a.label.normalized.CompareTo(b.label.normalized));
		for (int i = 0; i < list.Count; i++)
		{
			this.AddRow(list[i].label, list[i].index, list[i].index == this.SelectedTagIndex);
		}
		this.UpdateRows();
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x0011A550 File Offset: 0x00118750
	private void AddRow(TagLabel tagLabel, int tagIndex, bool isSelected = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TagRowPrefab, this.TagContainer);
		UITagRow component = gameObject.GetComponent<UITagRow>();
		component.tagEditor = this;
		component.tagLabel = tagLabel;
		component.tagIndex = tagIndex;
		component.isSelected = isSelected;
		gameObject.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = tagLabel.displayed;
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x0011A5B0 File Offset: 0x001187B0
	private void UpdateRows()
	{
		this.TagContainer.GetComponent<UIGrid>().enabled = true;
		this.ScrollView.UpdateScrollbars(true);
		this.ScrollView.ResetPosition();
		this.ScrollView.UpdatePosition();
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x0011A5E8 File Offset: 0x001187E8
	private void RecordSelection()
	{
		this.globalSelection.Clear();
		for (int i = 0; i < PlayerScript.PointerScript.HighLightedObjects.Count; i++)
		{
			this.globalSelection.Add(PlayerScript.PointerScript.HighLightedObjects[i]);
		}
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x0011A635 File Offset: 0x00118835
	public void SelectionChanged()
	{
		if (base.gameObject.activeSelf)
		{
			this.RecordSelection();
			this.LoadTags(false);
		}
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x0011A654 File Offset: 0x00118854
	public void Select(int tagIndex)
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		for (int i = 0; i < this.TagContainer.childCount; i++)
		{
			UITagRow component = this.TagContainer.GetChild(i).GetComponent<UITagRow>();
			component.isSelected = (component.tagIndex == tagIndex);
		}
		this.SelectedTagIndex = tagIndex;
		this.HighlightTagIndex(tagIndex);
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x0011A6B4 File Offset: 0x001188B4
	private void HighlightTagIndex(int tagIndex)
	{
		PlayerScript.PointerScript.ResetHighlight();
		List<NetworkPhysicsObject> list = (this.globalSelection.Count == 0) ? NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs : this.globalSelection;
		for (int i = 0; i < list.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = list[i];
			GameObject gameObject = networkPhysicsObject.gameObject;
			if ((!networkPhysicsObject.IsHidden || networkPhysicsObject.IsObscured) && gameObject.layer != 2 && networkPhysicsObject.IsGrabbable)
			{
				if (tagIndex == -1)
				{
					if (list == this.globalSelection)
					{
						PlayerScript.PointerScript.AddHighlight(networkPhysicsObject, zInput.GetButton("Ctrl", ControlType.All));
					}
				}
				else if (networkPhysicsObject.TagIsSet(tagIndex))
				{
					PlayerScript.PointerScript.AddHighlight(networkPhysicsObject, zInput.GetButton("Ctrl", ControlType.All));
				}
			}
		}
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x0011A773 File Offset: 0x00118973
	public void Deselect()
	{
		this.Select(-1);
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x0011A77C File Offset: 0x0011897C
	public void HoverTag(int tagIndex)
	{
		this.hoveredTagIndex = tagIndex;
		this.HighlightTagIndex(this.hoveredTagIndex);
	}

	// Token: 0x060027ED RID: 10221 RVA: 0x0011A791 File Offset: 0x00118991
	public void UnHoverTag(int tagIndex)
	{
		if (this.hoveredTagIndex == tagIndex)
		{
			this.hoveredTagIndex = -1;
			this.HighlightTagIndex(this.SelectedTagIndex);
		}
	}

	// Token: 0x060027EE RID: 10222 RVA: 0x0011A7B0 File Offset: 0x001189B0
	public void DeleteTag()
	{
		if (!Network.isServer)
		{
			return;
		}
		if (this.SelectedTagIndex == -1)
		{
			return;
		}
		UIDialog.Show(Language.Translate("Delete tag '{0}'? (It will be removed from all components and objects.)", NetworkSingleton<ComponentTags>.Instance.TagLabelFromIndex(this.SelectedTagIndex).displayed), "Yes", "No", delegate()
		{
			this.ConfirmDeleteTag();
		}, null);
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x0011A80C File Offset: 0x00118A0C
	private void ConfirmDeleteTag()
	{
		if (!Network.isServer)
		{
			return;
		}
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		for (int i = 0; i < grabbableNPOs.Count; i++)
		{
			if (grabbableNPOs[i].TagIsSet(this.SelectedTagIndex))
			{
				grabbableNPOs[i].SetTag(this.SelectedTagIndex, false);
			}
		}
		NetworkSingleton<SnapPointManager>.Instance.RemoveTagFromAllSnapPoints(this.SelectedTagIndex);
		if (NetworkSingleton<ComponentTags>.Instance.CheckRemoveTag(null, this.SelectedTagIndex))
		{
			EventManager.TriggerAvailableTagsChange();
		}
	}

	// Token: 0x04001A2F RID: 6703
	private const int COMPACT_COUNT = 16;

	// Token: 0x04001A30 RID: 6704
	public UISprite Window;

	// Token: 0x04001A31 RID: 6705
	public Transform TagContainer;

	// Token: 0x04001A32 RID: 6706
	public GameObject TagRowPrefab;

	// Token: 0x04001A33 RID: 6707
	public UILabel ExpandButtonLabel;

	// Token: 0x04001A34 RID: 6708
	public UIScrollView ScrollView;

	// Token: 0x04001A35 RID: 6709
	public int SelectedTagIndex = -1;

	// Token: 0x04001A36 RID: 6710
	private int hoveredTagIndex = -1;

	// Token: 0x04001A37 RID: 6711
	private List<NetworkPhysicsObject> globalSelection = new List<NetworkPhysicsObject>();
}
