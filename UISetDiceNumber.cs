using System;
using UnityEngine;

// Token: 0x02000334 RID: 820
public class UISetDiceNumber : MonoBehaviour
{
	// Token: 0x0600272B RID: 10027 RVA: 0x00116894 File Offset: 0x00114A94
	private void OnEnable()
	{
		int rotationIndex = PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().GetRotationIndex(false);
		this.UpdateSelection(rotationIndex + 1);
		string value = PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().RotationValues[this.Number - 1].value;
		UITooltipScript uitooltipScript = base.gameObject.AddMissingComponent<UITooltipScript>();
		uitooltipScript.Tooltip = value;
		uitooltipScript.enabled = (value != this.Label.text);
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x00116910 File Offset: 0x00114B10
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject && gameObject.GetComponent<NetworkPhysicsObject>().HasRotationsValues())
			{
				gameObject.GetComponent<NetworkPhysicsObject>().SetRotationNumber(this.Number, PlayerScript.PointerScript.ID);
			}
		}
		Transform parent = base.transform.parent;
		for (int i = 0; i < parent.childCount; i++)
		{
			parent.GetChild(i).GetComponent<UISetDiceNumber>().UpdateSelection(this.Number);
		}
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x001169E8 File Offset: 0x00114BE8
	private void UpdateSelection(int RotationIndex)
	{
		this.Label = base.GetComponentsInChildren<UILabel>(true)[0];
		this.Number = int.Parse(base.gameObject.name);
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		if (RotationIndex == this.Number)
		{
			this.Label.ThemeAs = UIPalette.UI.ContextMenuHighlight;
			this.Label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuHighlight];
			return;
		}
		this.Label.ThemeAs = UIPalette.UI.ContextMenuText;
		this.Label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuText];
	}

	// Token: 0x040019A6 RID: 6566
	private UILabel Label;

	// Token: 0x040019A7 RID: 6567
	private int Number;
}
