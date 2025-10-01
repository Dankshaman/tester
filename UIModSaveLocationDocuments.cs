using System;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class UIModSaveLocationDocuments : MonoBehaviour
{
	// Token: 0x06002540 RID: 9536 RVA: 0x0010703F File Offset: 0x0010523F
	private void Start()
	{
		base.gameObject.AddComponent<UITooltipScript>().Tooltip = SerializationScript.GetCleanPath(DirectoryScript.modsFilePath);
	}
}
