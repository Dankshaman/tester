using System;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class UIFogColor : MonoBehaviour
{
	// Token: 0x0600231F RID: 8991 RVA: 0x000F9E50 File Offset: 0x000F8050
	private void Start()
	{
		this.label = base.gameObject.name.Substring(0, base.gameObject.name.Length - 4);
		this.colour = Colour.ColourFromLabel(this.label);
		base.GetComponent<UISprite>().color = this.colour;
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000F9EB0 File Offset: 0x000F80B0
	private void OnClick()
	{
		if (!PlayerScript.PointerScript)
		{
			return;
		}
		switch (this.colorWheelMode)
		{
		case ColorWheelMode.HiddenZone:
		{
			GameObject infoHiddenZoneGO = PlayerScript.PointerScript.InfoHiddenZoneGO;
			if (infoHiddenZoneGO)
			{
				infoHiddenZoneGO.GetComponent<HiddenZone>().SyncSetZoneColor(this.label);
				PlayerScript.PointerScript.ResetHiddenZoneObject();
				return;
			}
			break;
		}
		case ColorWheelMode.Hand:
		{
			GameObject infoHandObject = PlayerScript.PointerScript.InfoHandObject;
			if (infoHandObject)
			{
				infoHandObject.GetComponent<HandZone>().TriggerLabel = this.label;
				PlayerScript.PointerScript.ResetHandObject();
				return;
			}
			break;
		}
		case ColorWheelMode.Card:
			break;
		case ColorWheelMode.Notepad:
			this.Notebook.OnColorFilterSelected(this.colour);
			break;
		default:
			return;
		}
	}

	// Token: 0x04001646 RID: 5702
	private string label = "";

	// Token: 0x04001647 RID: 5703
	private Colour colour;

	// Token: 0x04001648 RID: 5704
	public UINotebook Notebook;

	// Token: 0x04001649 RID: 5705
	public ColorWheelMode colorWheelMode;
}
