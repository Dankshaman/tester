using System;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class DeckImageScript : MonoBehaviour
{
	// Token: 0x06000C32 RID: 3122 RVA: 0x00052EC9 File Offset: 0x000510C9
	public int card_id()
	{
		return this.card_id_;
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x00052ED1 File Offset: 0x000510D1
	public void SetCardID(int id)
	{
		this.card_id_ = id;
	}

	// Token: 0x0400084B RID: 2123
	public int card_id_ = -1;
}
