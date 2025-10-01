using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class CardScript : NetworkBehavior
{
	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0003F432 File Offset: 0x0003D632
	// (set) Token: 0x060008C1 RID: 2241 RVA: 0x0003F43A File Offset: 0x0003D63A
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int card_id_
	{
		get
		{
			return this._card_id_;
		}
		set
		{
			if (value != this._card_id_)
			{
				this._card_id_ = value;
				base.DirtySync("card_id_");
			}
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0003F457 File Offset: 0x0003D657
	// (set) Token: 0x060008C3 RID: 2243 RVA: 0x0003F45F File Offset: 0x0003D65F
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool bSideways
	{
		get
		{
			return this._bSideways;
		}
		set
		{
			if (value != this._bSideways)
			{
				this._bSideways = value;
				base.DirtySync("bSideways");
			}
		}
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0003F47C File Offset: 0x0003D67C
	public override void OnSync()
	{
		this.cardManagerInstance.SetupCard(base.gameObject, this.card_id_, -1, false);
		if (this.ThisHideObject.bHidden)
		{
			this.ThisHideObject.bHidden = false;
			this.ThisHideObject.Hide(true, false);
		}
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0003F4C8 File Offset: 0x0003D6C8
	private void Awake()
	{
		this.cardNPO = base.GetComponent<NetworkPhysicsObject>();
		if (this.cardNPO)
		{
			this.cardNPO.SetTypedNumberHandlers(new NetworkPhysicsObject.MaxTypedNumberMethodDelegate(CardScript.MaxTypedNumber), new NetworkPhysicsObject.HandleTypedNumberMethodDelegate(CardScript.HandleTypedNumber), false);
		}
		this.cardManagerInstance = NetworkSingleton<CardManagerScript>.Instance;
		this.ThisHideObject = base.GetComponent<HideObject>();
		base.gameObject.AddComponent<PhysicsReduceY>();
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0003F535 File Offset: 0x0003D735
	private void Start()
	{
		this.DummyObject = base.GetComponent<DummyObject>();
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0003F543 File Offset: 0x0003D743
	private void OnDestroy()
	{
		if ((Network.isServer || this.DummyObject) && NetworkSingleton<CardManagerScript>.Instance)
		{
			NetworkSingleton<CardManagerScript>.Instance.CleanupIndex(this.GetCustomIndexes(), this.DummyObject);
		}
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0003F580 File Offset: 0x0003D780
	public List<int> GetCustomIndexes()
	{
		List<int> list = new List<int>();
		int num = 0;
		int i = this.card_id_;
		while (i > 99)
		{
			i -= 100;
			num++;
		}
		if (num > 0 && !list.Contains(num))
		{
			list.Add(num);
		}
		return list;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0003F5C4 File Offset: 0x0003D7C4
	public List<CustomDeckData> GetCustomDeckDatas()
	{
		List<CustomDeckData> list = new List<CustomDeckData>();
		List<int> customIndexes = this.GetCustomIndexes();
		for (int i = 0; i < customIndexes.Count; i++)
		{
			int key = customIndexes[i];
			CustomDeckData item;
			if (NetworkSingleton<CardManagerScript>.Instance.GetCustomDecks(this.DummyObject).TryGetValue(key, out item))
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0003F61F File Offset: 0x0003D81F
	public int card_id()
	{
		return this.card_id_;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0003F628 File Offset: 0x0003D828
	[Remote(Permission.Server)]
	public void SetCardID(int id)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.SetCardID), id);
		}
		this.card_id_ = id;
		this.cardManagerInstance.SetupCard(base.gameObject, this.card_id_, -1, false);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003F678 File Offset: 0x0003D878
	public void ResetCard()
	{
		FixedJoint component = base.GetComponent<FixedJoint>();
		if (component)
		{
			if (component.connectedBody && component.connectedBody.GetComponent<CardScript>())
			{
				component.connectedBody.GetComponent<CardScript>().CardAttachToThis = null;
			}
			UnityEngine.Object.Destroy(component);
		}
		this.cardNPO.rigidbody.mass = 0.5f;
		Collider component2 = base.GetComponent<Collider>();
		for (int i = 0; i < this.ignoreColliders.Count; i++)
		{
			if (this.ignoreColliders[i] != null)
			{
				Physics.IgnoreCollision(component2, this.ignoreColliders[i], false);
			}
		}
		this.ignoreColliders.Clear();
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0003F730 File Offset: 0x0003D930
	public List<GameObject> CardsAttachedToThis()
	{
		if (!this.CardAttachToThis)
		{
			return new List<GameObject>();
		}
		List<GameObject> list = new List<GameObject>();
		GameObject gameObject = base.gameObject;
		int num = 0;
		while (gameObject)
		{
			num++;
			if (num > 52)
			{
				Debug.Log("Stopped CardsAttachedToThis while loop crash.");
				break;
			}
			gameObject = gameObject.GetComponent<CardScript>().CardAttachToThis;
			if (!gameObject)
			{
				break;
			}
			list.Add(gameObject);
		}
		return list;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0003F79C File Offset: 0x0003D99C
	public GameObject RootCard()
	{
		if (!base.GetComponent<FixedJoint>() || !base.gameObject.GetComponent<FixedJoint>().connectedBody)
		{
			return null;
		}
		GameObject gameObject = base.gameObject.GetComponent<FixedJoint>().connectedBody.gameObject;
		int num = 0;
		FixedJoint component = gameObject.GetComponent<FixedJoint>();
		while (component && component.connectedBody && component.connectedBody.gameObject)
		{
			num++;
			if (num > 52)
			{
				Debug.Log("Stopped RootCard while loop crash.");
				break;
			}
			component = gameObject.GetComponent<FixedJoint>();
			if (!component || !component.connectedBody.gameObject)
			{
				break;
			}
			gameObject = component.connectedBody.gameObject;
		}
		return gameObject;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0003F85C File Offset: 0x0003DA5C
	public bool RootHeldByMe()
	{
		GameObject gameObject = this.RootCard();
		return gameObject && gameObject.GetComponent<NetworkPhysicsObject>().HeldByPlayerID == NetworkID.ID;
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0003F88C File Offset: 0x0003DA8C
	public bool RootHeldByAnyone()
	{
		GameObject gameObject = this.RootCard();
		return gameObject && gameObject.GetComponent<NetworkPhysicsObject>().IsHeldBySomebody;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003F8B8 File Offset: 0x0003DAB8
	private void OnCollisionEnter(Collision info)
	{
		if (Network.isServer)
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(info.collider);
			if (!networkPhysicsObject || networkPhysicsObject.CurrentPlayerHand || this.cardNPO.CurrentPlayerHand)
			{
				return;
			}
			if (networkPhysicsObject.deckScript)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.NotifyCardHitDeck(this.cardNPO, networkPhysicsObject, true);
				return;
			}
			if (networkPhysicsObject.cardScript)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.NotifyCardHitCard(this.cardNPO, networkPhysicsObject, true);
			}
		}
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003F946 File Offset: 0x0003DB46
	private static int MaxTypedNumber(NetworkPhysicsObject npo)
	{
		if (CardScript.CARD_IS_A_DECK_FOR_HOTKEYS && npo.CanBeHeldInHand && NetworkPhysicsObject.DrawByTypingNumbers)
		{
			return 1;
		}
		return NetworkPhysicsObject.DefaultMaxTypedNumber(npo);
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0003F968 File Offset: 0x0003DB68
	private static void HandleTypedNumber(NetworkPhysicsObject npo, int playerID, int number)
	{
		if (!CardScript.CARD_IS_A_DECK_FOR_HOTKEYS || !npo.CanBeHeldInHand || !NetworkPhysicsObject.DrawByTypingNumbers)
		{
			NetworkPhysicsObject.DefaultHandleTypedNumber(npo, playerID, number);
			return;
		}
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, PlayerScript.PointerScript.PointerColorLabel, number, 0);
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, NetworkSingleton<PlayerManager>.Instance.ColourLabelFromID(playerID), number, 0);
	}

	// Token: 0x04000657 RID: 1623
	public static bool CARD_IS_A_DECK_FOR_HOTKEYS = true;

	// Token: 0x04000658 RID: 1624
	private int _card_id_ = -1;

	// Token: 0x04000659 RID: 1625
	private bool _bSideways;

	// Token: 0x0400065A RID: 1626
	public int deck_id_ = -1;

	// Token: 0x0400065B RID: 1627
	public GameObject CardAttachToThis;

	// Token: 0x0400065C RID: 1628
	private NetworkPhysicsObject cardNPO;

	// Token: 0x0400065D RID: 1629
	private CardManagerScript cardManagerInstance;

	// Token: 0x0400065E RID: 1630
	public DummyObject DummyObject;

	// Token: 0x0400065F RID: 1631
	private HideObject ThisHideObject;

	// Token: 0x04000660 RID: 1632
	public List<Collider> ignoreColliders = new List<Collider>();
}
