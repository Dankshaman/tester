using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class Finder : Singleton<Finder>
{
	// Token: 0x06000DE6 RID: 3558 RVA: 0x00059870 File Offset: 0x00057A70
	public void AddFinderEntry(FinderCategory category, string keyText, string additionalText, Transform targetItem, Transform windowItem)
	{
		FinderEntry finderEntry = new FinderEntry(targetItem, windowItem, category, keyText);
		finderEntry.additionalText = additionalText;
		string key = LibString.NormalizedString(keyText, true);
		if (!this.Entries.ContainsKey(key))
		{
			this.Entries[key] = new List<FinderEntry>();
		}
		this.Entries[key].Add(finderEntry);
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x000598CC File Offset: 0x00057ACC
	public void AddFinderEntry(FinderCategory category, string keyText, string additionalText, Transform targetItem)
	{
		Transform windowItem = this.FindParentWindow(targetItem);
		this.AddFinderEntry(category, keyText, additionalText, targetItem, windowItem);
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x000598EE File Offset: 0x00057AEE
	public IEnumerable<FinderEntry> Search(string searchText)
	{
		Stopwatch stopWatch = Stopwatch.StartNew();
		string a = LibString.NormalizedString(searchText, true);
		if (a == "")
		{
			yield break;
		}
		if (a != this.lastSearch)
		{
			this.lastSearch = a;
			this.searchWordLists.Clear();
			for (string text = LibString.bite(ref a, ','); text != null; text = LibString.bite(ref a, ','))
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
		foreach (KeyValuePair<string, List<FinderEntry>> entry in this.Entries)
		{
			bool flag = false;
			for (int j = 0; j < this.searchWordLists.Count; j++)
			{
				int num = 0;
				bool flag2 = true;
				bool flag3 = false;
				for (int k = 0; k < this.searchWordLists[j].Count; k++)
				{
					string text2 = this.searchWordLists[j][k];
					if (text2.StartsWith("-"))
					{
						num++;
						text2 = text2.Substring(1);
						if (text2 != "" && entry.Key.Contains(text2))
						{
							flag3 = true;
							flag2 = false;
						}
					}
					else if (!entry.Key.Contains(text2))
					{
						flag2 = false;
					}
				}
				if (num == this.searchWordLists[j].Count)
				{
					if (flag3)
					{
						flag = false;
						break;
					}
				}
				else if (flag2)
				{
					flag = true;
				}
			}
			if (flag)
			{
				int num2;
				for (int i = 0; i < entry.Value.Count; i = num2 + 1)
				{
					yield return entry.Value[i];
					num2 = i;
				}
			}
			entry = default(KeyValuePair<string, List<FinderEntry>>);
		}
		Dictionary<string, List<FinderEntry>>.Enumerator enumerator = default(Dictionary<string, List<FinderEntry>>.Enumerator);
		stopWatch.Log("Finder Search");
		yield break;
		yield break;
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x00059905 File Offset: 0x00057B05
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		List<GameObject> sceneRootGameObjects = Utilities.GetSceneRootGameObjects();
		foreach (GameObject gameObject in sceneRootGameObjects)
		{
			this.FindWindows(gameObject.transform);
		}
		sceneRootGameObjects.Clear();
		this.ScrapeWindows();
		yield break;
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x00059914 File Offset: 0x00057B14
	private void FindWindows(Transform t)
	{
		if (Singleton<UIPalette>.Instance.IsWindow(t))
		{
			this.Windows.Add(t);
		}
		for (int i = 0; i < t.childCount; i++)
		{
			this.FindWindows(t.GetChild(i));
		}
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x00059958 File Offset: 0x00057B58
	private Transform FindParentWindow(Transform t)
	{
		if (this.Windows.Contains(t))
		{
			return t;
		}
		if (t.parent == null)
		{
			return null;
		}
		return this.FindParentWindow(t.parent);
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x00059988 File Offset: 0x00057B88
	private void ScrapeWindows()
	{
		for (int i = 0; i < this.Windows.Count; i++)
		{
			this.ScrapeChildren(this.Windows[i]);
		}
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x000599C0 File Offset: 0x00057BC0
	private void ScrapeChildren(Transform window)
	{
		for (int i = 0; i < window.childCount; i++)
		{
			Transform child = window.GetChild(i);
			if (!this.Windows.Contains(child))
			{
				this.CheckAddFinderEntry(child, window);
				this.ScrapeChildren(child);
			}
		}
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x00059A04 File Offset: 0x00057C04
	public void CheckAddFinderEntry(Transform targetItem, Transform windowItem)
	{
		UILabel component = targetItem.GetComponent<UILabel>();
		UITooltipScript component2 = targetItem.GetComponent<UITooltipScript>();
		if (component == null && component2 == null)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (component)
		{
			stringBuilder.AppendLine(component.text);
		}
		if (component2)
		{
			stringBuilder.AppendLine(component2.Tooltip);
		}
		string keyText = stringBuilder.ToString();
		string additionalText = (component2 == null) ? "" : component2.DelayTooltip;
		this.AddFinderEntry(FinderCategory.Setting, keyText, additionalText, targetItem, windowItem);
	}

	// Token: 0x04000910 RID: 2320
	private List<Transform> Windows = new List<Transform>();

	// Token: 0x04000911 RID: 2321
	private Dictionary<string, List<FinderEntry>> Entries = new Dictionary<string, List<FinderEntry>>();

	// Token: 0x04000912 RID: 2322
	private List<List<string>> searchWordLists = new List<List<string>>();

	// Token: 0x04000913 RID: 2323
	private string lastSearch = "";
}
