using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	// Token: 0x060000BC RID: 188 RVA: 0x000054CB File Offset: 0x000036CB
	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.Refresh();
	}

	// Token: 0x060000BD RID: 189 RVA: 0x000054DF File Offset: 0x000036DF
	private void Start()
	{
		EventDelegate.Add(this.mList.onChange, delegate()
		{
			Localization.language = UIPopupList.current.value;
		});
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00005514 File Offset: 0x00003714
	public void Refresh()
	{
		if (this.mList != null && Localization.knownLanguages != null)
		{
			this.mList.Clear();
			int i = 0;
			int num = Localization.knownLanguages.Length;
			while (i < num)
			{
				this.mList.items.Add(Localization.knownLanguages[i]);
				i++;
			}
			this.mList.value = Localization.language;
		}
	}

	// Token: 0x0400008D RID: 141
	private UIPopupList mList;
}
