using System;
using UnityEngine;

// Token: 0x02000338 RID: 824
public class UISetSplitNumber : MonoBehaviour
{
	// Token: 0x0600273B RID: 10043 RVA: 0x00117046 File Offset: 0x00115246
	private void OnEnable()
	{
		this.UpdateSelection(1);
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x00117050 File Offset: 0x00115250
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		ManagerPhysicsObject instance = NetworkSingleton<ManagerPhysicsObject>.Instance;
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				if (gameObject && gameObject.CompareTag("Deck"))
				{
					instance.SplitDeck(gameObject.GetComponent<NetworkPhysicsObject>().ID, this.Number);
				}
				if (gameObject && gameObject.GetComponent<StackObject>() && !gameObject.GetComponent<StackObject>().bBag && !gameObject.GetComponent<StackObject>().IsInfiniteStack)
				{
					instance.SplitStack(gameObject.GetComponent<NetworkPhysicsObject>().ID, this.Number);
				}
			}
		}
		Transform parent = base.transform.parent;
		for (int i = 0; i < parent.childCount; i++)
		{
			parent.GetChild(i).GetComponent<UISetSplitNumber>().UpdateSelection(this.Number);
		}
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x00117184 File Offset: 0x00115384
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

	// Token: 0x040019AF RID: 6575
	private UILabel Label;

	// Token: 0x040019B0 RID: 6576
	private int Number;
}
