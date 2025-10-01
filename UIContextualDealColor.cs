using System;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public class UIContextualDealColor : MonoBehaviour
{
	// Token: 0x06002214 RID: 8724 RVA: 0x000F567C File Offset: 0x000F387C
	private void Start()
	{
		this.label = base.gameObject.name.Substring(0, base.gameObject.name.Length - 5);
		this.colour = Colour.ColourFromLabel(this.label);
		base.GetComponent<UISprite>().color = this.colour;
		UIButton component = base.GetComponent<UIButton>();
		component.defaultColor = this.colour;
		component.hover = this.colour;
		component.pressed = this.colour;
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000F5714 File Offset: 0x000F3914
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(gameObject.GetComponent<NetworkPhysicsObject>().ID, this.label, 1, 0);
		}
	}

	// Token: 0x0400158B RID: 5515
	private string label = "";

	// Token: 0x0400158C RID: 5516
	private Colour colour;
}
