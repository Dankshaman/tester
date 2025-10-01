using System;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class UISetSearchNumber : MonoBehaviour
{
	// Token: 0x06002737 RID: 10039 RVA: 0x00116EA0 File Offset: 0x001150A0
	private void OnEnable()
	{
		this.UpdateSelection(1);
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x00116EAC File Offset: 0x001150AC
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		ManagerPhysicsObject instance = NetworkSingleton<ManagerPhysicsObject>.Instance;
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject && gameObject && gameObject.CompareTag("Deck"))
			{
				instance.SearchInventory(gameObject.GetComponent<NetworkPhysicsObject>().ID, NetworkID.ID, this.Number);
				break;
			}
		}
		NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x00116F70 File Offset: 0x00115170
	private void UpdateSelection(int number)
	{
		if (number > 20)
		{
			number = 20;
		}
		this.Label = base.GetComponentsInChildren<UILabel>(true)[0];
		this.Number = int.Parse(base.gameObject.name);
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject || PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().GetSelectedStateId() == -1)
		{
			return;
		}
		if (number == this.Number)
		{
			this.Label.ThemeAs = UIPalette.UI.ContextMenuHighlight;
			this.Label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuHighlight];
			return;
		}
		this.Label.ThemeAs = UIPalette.UI.ContextMenuText;
		this.Label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuText];
	}

	// Token: 0x040019AD RID: 6573
	private UILabel Label;

	// Token: 0x040019AE RID: 6574
	private int Number;
}
