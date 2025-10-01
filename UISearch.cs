using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x0200032E RID: 814
public class UISearch : MonoBehaviour
{
	// Token: 0x060026DF RID: 9951 RVA: 0x0011443A File Offset: 0x0011263A
	private void Awake()
	{
		base.gameObject.AddComponent<UIClearInput>();
	}

	// Token: 0x060026E0 RID: 9952 RVA: 0x00114448 File Offset: 0x00112648
	private void OnEnable()
	{
		this.SearchInput = base.GetComponent<UIInput>();
		if (this.ClearOnEnable)
		{
			this.SearchInput.value = "";
		}
		this.TimeHold = Time.time;
		this.ResetScroll();
		Wait.Frames(new Action(this.Focus), 3);
	}

	// Token: 0x060026E1 RID: 9953 RVA: 0x0011449D File Offset: 0x0011269D
	private void Focus()
	{
		this.SearchInput.isSelected = true;
	}

	// Token: 0x060026E2 RID: 9954 RVA: 0x001144AC File Offset: 0x001126AC
	private void Update()
	{
		if ((this.SearchInput.value != this.PrevSearch || this.ForceUpdate) && Time.time > this.TimeHold + this.TimeBetweenSearch)
		{
			this.PrevSearch = this.SearchInput.value;
			this.TimeHold = Time.time;
			int childCount = this.SearchGrid.transform.childCount;
			string normalizedSearchString = LibString.NormalizedString(this.SearchInput.value, true);
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject = this.SearchGrid.transform.GetChild(i).gameObject;
				if (!gameObject.CompareTag("InventoryItemBlank"))
				{
					if (this.PreFilter != null && !this.PreFilter(gameObject))
					{
						gameObject.SetActive(false);
					}
					else
					{
						gameObject.SetActive(this.CheckListObject(gameObject, normalizedSearchString));
					}
				}
			}
			this.ForceUpdate = false;
			this.ResetScroll();
		}
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x0011459C File Offset: 0x0011279C
	public void ResetSearch()
	{
		this.PrevSearch = "";
		this.Update();
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x001145AF File Offset: 0x001127AF
	public void ResetScroll()
	{
		this.SearchGrid.GetComponent<UIGrid>().repositionNow = true;
		Wait.Frames(new Action(this.ResetScrollView), 1);
		Wait.Frames(new Action(this.ResetScrollView), 2);
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x001145E8 File Offset: 0x001127E8
	private void ResetScrollView()
	{
		if (UICamera.selectedObject != this.ScrollBar.GetComponent<UIScrollBar>().foregroundWidget.gameObject)
		{
			this.SearchGrid.transform.parent.GetComponent<UIScrollView>().ResetPosition();
		}
		this.SearchGrid.transform.parent.GetComponent<UIScrollView>().UpdateScrollbars();
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x0011464C File Offset: 0x0011284C
	public bool CheckListObject(GameObject SearchButton, string normalizedSearchString = null)
	{
		if (this.SearchInput.value.Trim() == "")
		{
			return true;
		}
		if (normalizedSearchString == null)
		{
			normalizedSearchString = LibString.NormalizedString(this.SearchInput.value, true);
		}
		if (normalizedSearchString != this.lastSearch)
		{
			this.lastSearch = normalizedSearchString;
			this.searchWordLists.Clear();
			for (string text = LibString.bite(ref normalizedSearchString, ','); text != null; text = LibString.bite(ref normalizedSearchString, ','))
			{
				List<string> list = new List<string>();
				for (string item = LibString.bite(ref text, false, ' ', false, false, '\0'); item != null; item = LibString.bite(ref text, false, ' ', false, false, '\0'))
				{
					list.Add(item);
				}
				if (list.Count > 0)
				{
					this.searchWordLists.Add(list);
				}
			}
		}
		string text2;
		if (this.StringFromGameObject != null)
		{
			text2 = LibString.NormalizedString(this.StringFromGameObject(SearchButton), true);
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (UILabel uilabel in SearchButton.GetComponentsInChildren<UILabel>(true))
			{
				stringBuilder.AppendLine(uilabel.text);
			}
			foreach (UITooltipScript uitooltipScript in SearchButton.GetComponentsInChildren<UITooltipScript>(true))
			{
				stringBuilder.AppendLine(uitooltipScript.Tooltip);
			}
			text2 = LibString.NormalizedString(stringBuilder.ToString(), true);
		}
		bool result = false;
		for (int j = 0; j < this.searchWordLists.Count; j++)
		{
			int num = 0;
			bool flag = true;
			bool flag2 = false;
			for (int k = 0; k < this.searchWordLists[j].Count; k++)
			{
				string text3 = this.searchWordLists[j][k];
				if (text3.StartsWith("-"))
				{
					num++;
					text3 = text3.Substring(1);
					if (text3 != "" && text2.Contains(text3))
					{
						flag2 = true;
						flag = false;
					}
				}
				else if (!text2.Contains(text3))
				{
					flag = false;
				}
			}
			if (num == this.searchWordLists[j].Count)
			{
				if (flag2)
				{
					return false;
				}
			}
			else if (flag)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x04001957 RID: 6487
	public GameObject SearchGrid;

	// Token: 0x04001958 RID: 6488
	public GameObject ScrollBar;

	// Token: 0x04001959 RID: 6489
	public bool ForceUpdate;

	// Token: 0x0400195A RID: 6490
	public bool ClearOnEnable = true;

	// Token: 0x0400195B RID: 6491
	public UISearch.ExtraFilterFunction PreFilter;

	// Token: 0x0400195C RID: 6492
	public UISearch.StringFromGameObjectFunction StringFromGameObject;

	// Token: 0x0400195D RID: 6493
	public UIInput SearchInput;

	// Token: 0x0400195E RID: 6494
	private string PrevSearch = "";

	// Token: 0x0400195F RID: 6495
	private float TimeHold;

	// Token: 0x04001960 RID: 6496
	private float TimeBetweenSearch = 0.2f;

	// Token: 0x04001961 RID: 6497
	private List<List<string>> searchWordLists = new List<List<string>>();

	// Token: 0x04001962 RID: 6498
	private string lastSearch = "";

	// Token: 0x02000782 RID: 1922
	// (Invoke) Token: 0x06003F1D RID: 16157
	public delegate bool ExtraFilterFunction(GameObject searchItem);

	// Token: 0x02000783 RID: 1923
	// (Invoke) Token: 0x06003F21 RID: 16161
	public delegate string StringFromGameObjectFunction(GameObject searchItem);
}
