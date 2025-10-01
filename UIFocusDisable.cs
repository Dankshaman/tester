using System;
using UnityEngine;

// Token: 0x020002CB RID: 715
public class UIFocusDisable : MonoBehaviour
{
	// Token: 0x0600231A RID: 8986 RVA: 0x000F9DF3 File Offset: 0x000F7FF3
	private void Awake()
	{
		EventManager.OnFocusUI += this.CheckFocus;
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000F9E06 File Offset: 0x000F8006
	private void OnEnable()
	{
		this.ParentDragObject = base.GetComponentInParent<UIDragObject>();
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000F9E14 File Offset: 0x000F8014
	private void OnDestroy()
	{
		EventManager.OnFocusUI -= this.CheckFocus;
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000F9E27 File Offset: 0x000F8027
	private void CheckFocus(UIDragObject FocusObject)
	{
		if (!this.ParentDragObject)
		{
			return;
		}
		base.gameObject.SetActive(this.ParentDragObject == FocusObject);
	}

	// Token: 0x04001645 RID: 5701
	private UIDragObject ParentDragObject;
}
