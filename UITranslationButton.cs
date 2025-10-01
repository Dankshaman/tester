using System;
using UnityEngine;

// Token: 0x02000355 RID: 853
public class UITranslationButton : MonoBehaviour
{
	// Token: 0x0600287E RID: 10366 RVA: 0x0011E1B1 File Offset: 0x0011C3B1
	public void LoadThisTranslation()
	{
		Singleton<UILanguageSettings>.Instance.LoadTranslation(this.Title, this.Author, this.Filename);
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x0011E1CF File Offset: 0x0011C3CF
	public void DeleteThisTranslation()
	{
		Singleton<UILanguageSettings>.Instance.AskDeleteTranslation(this.Title, this.Author, this.Filename, this.WorkshopID);
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x0011E1F3 File Offset: 0x0011C3F3
	public void DisableDelete()
	{
		this.DeleteButton.SetActive(false);
	}

	// Token: 0x04001AA3 RID: 6819
	public string Title;

	// Token: 0x04001AA4 RID: 6820
	public string Author;

	// Token: 0x04001AA5 RID: 6821
	public string Filename;

	// Token: 0x04001AA6 RID: 6822
	public ulong WorkshopID;

	// Token: 0x04001AA7 RID: 6823
	public GameObject DeleteButton;
}
