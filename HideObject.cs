using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class HideObject : MonoBehaviour
{
	// Token: 0x06000FA5 RID: 4005 RVA: 0x0006AAE1 File Offset: 0x00068CE1
	private void Awake()
	{
		this.NPO = base.gameObject.GetComponent<NetworkPhysicsObject>();
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x0006AAF4 File Offset: 0x00068CF4
	public void Hide(bool bTrue, bool forceRefresh = false)
	{
		if (this.bHidden == bTrue && !forceRefresh)
		{
			return;
		}
		this.bHidden = bTrue;
		if (bTrue)
		{
			if (this.HideMesh != null)
			{
				base.GetComponent<MeshFilter>().mesh = this.HideMesh;
				return;
			}
			if (this.NPO.cardScript)
			{
				if (NetworkSingleton<CardManagerScript>.Instance)
				{
					NetworkSingleton<CardManagerScript>.Instance.SetupCard(base.gameObject, this.NPO.cardScript.card_id_, -1, true);
					return;
				}
			}
			else if (this.NPO.deckScript)
			{
				if (this.NPO.customObject && this.NPO.deckScript.GetDeck().Count == 0)
				{
					return;
				}
				if (NetworkSingleton<CardManagerScript>.Instance)
				{
					NetworkSingleton<CardManagerScript>.Instance.SetupCard(base.gameObject, this.NPO.deckScript.get_bottom_card_id(), this.NPO.deckScript.get_top_card_id(), true);
					return;
				}
			}
		}
		else
		{
			if (this.HideMesh != null)
			{
				base.GetComponent<MeshFilter>().mesh = base.GetComponent<MeshSyncScript>().Meshes[base.GetComponent<MeshSyncScript>().GetMesh()];
				return;
			}
			if (this.NPO.cardScript)
			{
				if (NetworkSingleton<CardManagerScript>.Instance)
				{
					NetworkSingleton<CardManagerScript>.Instance.SetupCard(base.gameObject, this.NPO.cardScript.card_id_, -1, false);
					return;
				}
			}
			else if (this.NPO.deckScript)
			{
				if (this.NPO.customObject && this.NPO.deckScript.GetDeck().Count == 0)
				{
					return;
				}
				if (NetworkSingleton<CardManagerScript>.Instance)
				{
					NetworkSingleton<CardManagerScript>.Instance.SetupCard(base.gameObject, this.NPO.deckScript.get_bottom_card_id(), this.NPO.deckScript.get_top_card_id(), false);
				}
			}
		}
	}

	// Token: 0x040009B2 RID: 2482
	public Mesh HideMesh;

	// Token: 0x040009B3 RID: 2483
	public bool bHidden;

	// Token: 0x040009B4 RID: 2484
	private NetworkPhysicsObject NPO;
}
