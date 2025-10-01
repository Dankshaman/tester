using System;
using UnityEngine;

// Token: 0x020002EF RID: 751
public class UIInit : MonoBehaviour
{
	// Token: 0x06002482 RID: 9346 RVA: 0x001015E0 File Offset: 0x000FF7E0
	private void Awake()
	{
		UIDialog.Instance = this.UIDialogGO.transform.GetChild(0).GetComponent<UIDialog>();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.UIDialogGO, this.UIDialogGO.transform.parent);
		gameObject.name = "Lua UIDialog";
		gameObject.transform.Find("Background/Blackout Background").gameObject.SetActive(false);
		UIDialog.LuaDialog = gameObject.transform.GetChild(0).GetComponent<UIDialog>();
	}

	// Token: 0x0400177E RID: 6014
	public GameObject UIDialogGO;
}
