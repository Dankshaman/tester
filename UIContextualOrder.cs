using System;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class UIContextualOrder : MonoBehaviour
{
	// Token: 0x06002217 RID: 8727 RVA: 0x000F57B8 File Offset: 0x000F39B8
	private void OnClick()
	{
		if (PlayerScript.PointerScript && PlayerScript.PointerScript.InfoObject)
		{
			PlayerScript.PointerScript.Order(PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().ID, this.Order);
		}
	}

	// Token: 0x0400158D RID: 5517
	public OrderType Order;
}
