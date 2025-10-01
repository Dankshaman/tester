using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class DeckScript : ContainerScript
{
	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00052EE9 File Offset: 0x000510E9
	// (set) Token: 0x06000C36 RID: 3126 RVA: 0x00052EF1 File Offset: 0x000510F1
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int top_card_id_
	{
		get
		{
			return this._top_card_id_;
		}
		set
		{
			if (value != this._top_card_id_)
			{
				this._top_card_id_ = value;
				base.DirtySync("top_card_id_");
			}
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00052F0E File Offset: 0x0005110E
	// (set) Token: 0x06000C38 RID: 3128 RVA: 0x00052F16 File Offset: 0x00051116
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int bottom_card_id_
	{
		get
		{
			return this._bottom_card_id_;
		}
		set
		{
			if (value != this._bottom_card_id_)
			{
				this._bottom_card_id_ = value;
				base.DirtySync("bottom_card_id_");
			}
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00052F33 File Offset: 0x00051133
	// (set) Token: 0x06000C3A RID: 3130 RVA: 0x00052F3B File Offset: 0x0005113B
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int num_cards_
	{
		get
		{
			return this._num_cards_;
		}
		set
		{
			if (value != this._num_cards_)
			{
				this._num_cards_ = value;
				base.DirtySync("num_cards_");
			}
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00052F58 File Offset: 0x00051158
	// (set) Token: 0x06000C3C RID: 3132 RVA: 0x00052F60 File Offset: 0x00051160
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

	// Token: 0x06000C3D RID: 3133 RVA: 0x00052F7D File Offset: 0x0005117D
	public override void OnSync()
	{
		this.CardManagerInstance.SetupCard(base.gameObject, this.bottom_card_id_, this.top_card_id_, false);
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x00052FA0 File Offset: 0x000511A0
	protected override void Awake()
	{
		base.Awake();
		this.CardManagerInstance = NetworkSingleton<CardManagerScript>.Instance;
		base.NPO.SetTypedNumberHandlers(new NetworkPhysicsObject.MaxTypedNumberMethodDelegate(DeckScript.MaxTypedNumber), new NetworkPhysicsObject.HandleTypedNumberMethodDelegate(DeckScript.HandleTypedNumber), false);
		base.gameObject.AddComponent<PhysicsReduceY>();
		this.playersSearchingInventory = base.gameObject.AddComponent<PlayersSearchingInventory>();
		this.playersSearchingInventory.Init();
		base.networkView.RegisterNetworkBehavior(this.playersSearchingInventory);
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0005301C File Offset: 0x0005121C
	private void Start()
	{
		this.DummyObject = base.GetComponent<DummyObject>();
		if (Network.isServer)
		{
			if (this.bRandomSpawn)
			{
				this.GenerateDeck(true);
			}
			if (!base.GetComponent<Rigidbody>().isKinematic)
			{
				base.GetComponent<Rigidbody>().velocity /= 3f;
			}
		}
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00053073 File Offset: 0x00051273
	public bool AnyoneSearchingDeck()
	{
		return this.playersSearchingInventory.AnyoneSearching();
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00053080 File Offset: 0x00051280
	private void OnDestroy()
	{
		this.CleanupCustomDeck();
		if (this.playersSearchingInventory)
		{
			UnityEngine.Object.Destroy(this.playersSearchingInventory.gameObject);
		}
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x000530A5 File Offset: 0x000512A5
	private void CleanupCustomDeck()
	{
		if ((Network.isServer || this.DummyObject) && NetworkSingleton<CardManagerScript>.Instance)
		{
			NetworkSingleton<CardManagerScript>.Instance.CleanupIndex(this.GetCustomIndexes(), this.DummyObject);
		}
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x000530E4 File Offset: 0x000512E4
	public List<int> GetCustomIndexes()
	{
		List<int> list = new List<int>();
		foreach (int num in this.Deck)
		{
			int num2 = 0;
			int i = num;
			while (i > 99)
			{
				i -= 100;
				num2++;
			}
			if (num2 > 0 && !list.Contains(num2))
			{
				list.Add(num2);
			}
		}
		return list;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x00053160 File Offset: 0x00051360
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

	// Token: 0x06000C45 RID: 3141 RVA: 0x000531BC File Offset: 0x000513BC
	public void GenerateDeck(bool Randomize = true)
	{
		if (this.deck_name == "Standard")
		{
			for (int i = 0; i < this.num_cards_; i++)
			{
				this.Deck.Add(i);
			}
		}
		if (this.deck_name == "CardBot Main")
		{
			for (int j = -200; j > -269; j--)
			{
				this.Deck.Add(j);
			}
			for (int k = -300; k > -369; k--)
			{
				this.Deck.Add(k);
			}
			for (int l = -400; l > -414; l--)
			{
				this.Deck.Add(l);
			}
		}
		if (this.deck_name == "CardBot Head")
		{
			for (int m = -500; m > -510; m--)
			{
				this.Deck.Add(m);
			}
			this.bSideways = true;
		}
		if (Randomize)
		{
			this.RandomizeDeck();
		}
		this.num_cards_ = this.Deck.Count;
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x000532C4 File Offset: 0x000514C4
	public void GenerateDeck(int index)
	{
		for (int i = 100 * index; i < this.num_cards_ + 100 * index; i++)
		{
			this.Deck.Add(i);
		}
		this.PopulateCardStates();
		this.RegenerateEndCardIDs();
		this.num_cards_ = this.Deck.Count;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00053314 File Offset: 0x00051514
	public bool RandomizeDeck()
	{
		if (this.AnyoneSearchingDeck())
		{
			return false;
		}
		this.PopulateCardStates();
		if (!this.DummyObject)
		{
			base.GetComponent<SoundScript>().ShakeSound();
		}
		List<int> list = new List<int>();
		List<ObjectState> list2 = new List<ObjectState>();
		while (this.Deck.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, this.Deck.Count);
			list.Add(this.Deck[index]);
			list2.Add(this.CardStates[index]);
			this.Deck.RemoveAt(index);
			this.CardStates.RemoveAt(index);
		}
		this.Deck = list;
		this.CardStates = list2;
		this.RegenerateEndCardIDs();
		return true;
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x000533C8 File Offset: 0x000515C8
	public void SpawnFacadeCard()
	{
		if (base.NPO && base.NPO.CanSeeName && !this.DummyObject)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.cardfacade_prefab, new Vector3(base.transform.position.x, base.transform.position.y + UnityEngine.Random.Range(-0.2f, 0.2f) * (float)Mathf.Min(3, Mathf.Max(1, this.num_cards_) / 52), base.transform.position.z), base.transform.rotation).GetComponent<CardFacadeScript>().DeckThatSpawned = base.gameObject;
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00053488 File Offset: 0x00051688
	public void RegenerateEndCardIDs()
	{
		this.top_card_id_ = -1;
		this.bottom_card_id_ = -1;
		if (this.num_cards_ > 1)
		{
			this.top_card_id_ = this.Deck[0];
		}
		if (this.num_cards_ > 0)
		{
			this.bottom_card_id_ = this.Deck[this.num_cards_ - 1];
		}
		if (this.bottom_card_id_ != -1)
		{
			this.CardManagerInstance.SetupCard(base.gameObject, this.bottom_card_id_, this.top_card_id_, false);
		}
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00053507 File Offset: 0x00051707
	public void ResetDeck()
	{
		this.CleanupCustomDeck();
		this.Deck.Clear();
		this.CardStates.Clear();
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00053525 File Offset: 0x00051725
	public List<int> GetDeck()
	{
		return this.Deck;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0005352D File Offset: 0x0005172D
	public List<ObjectState> GetCardStates()
	{
		return this.CardStates;
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00053538 File Offset: 0x00051738
	private void PopulateCardStates()
	{
		if (this.CardStates.Count == 0)
		{
			ObjectState objectState = this.DummyObject ? new ObjectState() : NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
			objectState.Name = NetworkSingleton<GameMode>.Instance.Card.name;
			objectState.DeckIDs = null;
			objectState.CustomDeck = null;
			objectState.Nickname = "";
			objectState.Description = "";
			objectState.GMNotes = "";
			objectState.Hands = new bool?(true);
			objectState.HideWhenFaceDown = null;
			objectState.LuaScript = null;
			objectState.LuaScriptState = null;
			for (int i = 0; i < this.Deck.Count; i++)
			{
				ObjectState objectState2 = objectState.Clone();
				objectState2.GUID = LibString.GetRandomGUID();
				objectState2.CardID = new int?(this.Deck[i]);
				this.CardStates.Add(objectState2);
			}
		}
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x00053630 File Offset: 0x00051830
	public void SetDeck(List<int> deck, List<ObjectState> CardS)
	{
		this.Deck.Clear();
		this.num_cards_ = deck.Count;
		foreach (int item in deck)
		{
			this.Deck.Add(item);
		}
		this.CardStates.Clear();
		if (CardS != null && CardS.Count > 0)
		{
			if (deck.Count != CardS.Count)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Error setting deck because deck ids and card states do not match in number! deck count = ",
					deck.Count,
					" objectstates count = ",
					CardS.Count
				}));
			}
			using (List<ObjectState>.Enumerator enumerator2 = CardS.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ObjectState objectState = enumerator2.Current;
					this.CardStates.Add(objectState.Clone());
				}
				goto IL_101;
			}
		}
		this.DummyObject = base.GetComponent<DummyObject>();
		this.PopulateCardStates();
		IL_101:
		this.RegenerateEndCardIDs();
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00053760 File Offset: 0x00051960
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, serializationMethod = SerializationMethod.Json)]
	public void SetSearch(List<ObjectState> cardStates, int upToIndex)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<List<ObjectState>, int>(RPCTarget.Server, new Action<List<ObjectState>, int>(this.SetSearch), cardStates, upToIndex);
			return;
		}
		if (upToIndex >= this.Deck.Count)
		{
			this.Deck.Clear();
			this.CardStates.Clear();
			using (List<ObjectState>.Enumerator enumerator = cardStates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ObjectState objectState = enumerator.Current;
					this.CardStates.Add(objectState);
					this.Deck.Add(objectState.CardID.Value);
				}
				goto IL_ED;
			}
		}
		this.Deck.RemoveRange(0, upToIndex);
		this.CardStates.RemoveRange(0, upToIndex);
		for (int i = 0; i < cardStates.Count; i++)
		{
			this.CardStates.Insert(i, cardStates[i]);
			this.Deck.Insert(i, cardStates[i].CardID.Value);
		}
		IL_ED:
		this.num_cards_ = this.Deck.Count;
		this.RegenerateEndCardIDs();
		this.playersSearchingInventory.SetSearch((int)Network.sender.id, base.NPO.ID, cardStates, upToIndex);
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x000538A8 File Offset: 0x00051AA8
	public void AddCard(bool top, int id, ObjectState OS)
	{
		if (top)
		{
			this.Deck.Insert(0, id);
			this.CardStates.Insert(0, OS);
		}
		else
		{
			this.Deck.Add(id);
			this.CardStates.Add(OS);
		}
		int num_cards_ = this.num_cards_ + 1;
		this.num_cards_ = num_cards_;
		this.RegenerateEndCardIDs();
		if (Network.isServer && !base.GetComponent<Rigidbody>().isKinematic)
		{
			base.GetComponent<Rigidbody>().velocity /= 3f;
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00053934 File Offset: 0x00051B34
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public GameObject RemoveCardRPC(int index, Vector3 pos)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, Vector3, GameObject>(RPCTarget.Server, new Func<int, Vector3, GameObject>(this.RemoveCardRPC), index, pos);
			return null;
		}
		if (this.num_cards_ == 0)
		{
			if (this.LastCard)
			{
				return this.LastCard;
			}
			return null;
		}
		else
		{
			this.PlayDrawCardSound();
			if (index < 0 || index > this.Deck.Count - 1)
			{
				Debug.LogError("Index not found in deck: " + index);
				return null;
			}
			int cardID = this.Deck[index];
			this.Deck.RemoveAt(index);
			ObjectState objectState = this.CardStates[index];
			this.CardStates.RemoveAt(index);
			int num_cards_ = this.num_cards_ - 1;
			this.num_cards_ = num_cards_;
			this.RegenerateEndCardIDs();
			objectState.Transform = new TransformState(base.transform);
			objectState.Transform.posX = pos.x;
			objectState.Transform.posY = pos.y;
			objectState.Transform.posZ = pos.z;
			objectState.Transform.scaleX = base.NPO.Scale.x;
			objectState.Transform.scaleY = base.NPO.Scale.y;
			objectState.Transform.scaleZ = base.NPO.Scale.z;
			objectState.ColorDiffuse = new ColourState?(new ColourState(base.NPO.DiffuseColor));
			objectState.Grid = !base.NPO.IgnoresGrid;
			objectState.Autoraise = base.NPO.DoAutoRaise;
			objectState.Snap = !base.NPO.IgnoresSnap;
			objectState.Sticky = base.NPO.IsSticky;
			objectState.HideWhenFaceDown = new bool?(base.NPO.IsHiddenWhenFaceDown);
			objectState.Locked = false;
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
			gameObject.GetComponent<CardScript>().SetCardID(cardID);
			gameObject.GetComponent<CardScript>().deck_id_ = base.NPO.ID;
			NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
			component.DoesNotPersist = base.NPO.DoesNotPersist;
			Vector3 zero = Vector3.zero;
			Bounds boundsNotNormalized = gameObject.GetComponent<NetworkPhysicsObject>().GetBoundsNotNormalized(out zero);
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + boundsNotNormalized.extents.y + zero.y, gameObject.transform.position.z);
			if (pos != base.transform.position)
			{
				base.StartCoroutine(NetworkSingleton<ManagerPhysicsObject>.Instance.DelaySnapToGrid(gameObject));
			}
			if (this.num_cards_ == 1)
			{
				this.LastCard = this.TakeCard(false, true);
			}
			if (Network.isServer)
			{
				base.GetComponent<Rigidbody>().AddForce(Vector3.down);
			}
			EventManager.TriggerObjectLeaveContainer(base.NPO, component);
			return gameObject;
		}
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00053C30 File Offset: 0x00051E30
	public GameObject RemoveCardByGUID(string guid, Vector3 pos)
	{
		for (int i = 0; i < this.CardStates.Count; i++)
		{
			if (this.CardStates[i].GUID == guid)
			{
				return this.RemoveCardRPC(i, pos);
			}
		}
		return null;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00053C78 File Offset: 0x00051E78
	public GameObject TakeCard(bool top, bool bSpawnTop = true)
	{
		if (this.num_cards_ == 0)
		{
			return this.LastCard;
		}
		this.PlayDrawCardSound();
		int cardID;
		ObjectState objectState;
		if (top)
		{
			cardID = this.Deck[0];
			this.Deck.RemoveAt(0);
			objectState = this.CardStates[0];
			this.CardStates.RemoveAt(0);
		}
		else
		{
			cardID = this.Deck[this.num_cards_ - 1];
			this.Deck.RemoveAt(this.num_cards_ - 1);
			objectState = this.CardStates[this.num_cards_ - 1];
			this.CardStates.RemoveAt(this.num_cards_ - 1);
		}
		int num_cards_ = this.num_cards_ - 1;
		this.num_cards_ = num_cards_;
		this.RegenerateEndCardIDs();
		Vector3 vector;
		if (this.num_cards_ > 0)
		{
			if (bSpawnTop)
			{
				vector = new Vector3(base.transform.position.x, base.transform.position.y + base.GetComponent<Renderer>().bounds.size.y / 2f + 0.1f, base.transform.position.z);
			}
			else
			{
				vector = new Vector3(base.transform.position.x, base.transform.position.y - base.GetComponent<Renderer>().bounds.size.y / 2f - 0.2f, base.transform.position.z);
			}
		}
		else
		{
			vector = new Vector3(base.transform.position.x, base.transform.position.y - base.GetComponent<Renderer>().bounds.size.y / 2f, base.transform.position.z);
		}
		objectState.Transform = new TransformState(base.transform);
		objectState.Transform.posX = vector.x;
		objectState.Transform.posY = vector.y;
		objectState.Transform.posZ = vector.z;
		objectState.Transform.scaleX = base.NPO.Scale.x;
		objectState.Transform.scaleY = base.NPO.Scale.y;
		objectState.Transform.scaleZ = base.NPO.Scale.z;
		objectState.ColorDiffuse = new ColourState?(new ColourState(base.NPO.DiffuseColor));
		objectState.Grid = !base.NPO.IgnoresGrid;
		objectState.Autoraise = base.NPO.DoAutoRaise;
		objectState.Snap = !base.NPO.IgnoresSnap;
		objectState.Sticky = base.NPO.IsSticky;
		if (objectState.HideWhenFaceDown == null)
		{
			objectState.HideWhenFaceDown = new bool?(base.NPO.IsHiddenWhenFaceDown);
		}
		objectState.Locked = false;
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
		gameObject.GetComponent<CardScript>().SetCardID(cardID);
		gameObject.GetComponent<CardScript>().deck_id_ = base.NPO.ID;
		NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
		component.DoesNotPersist = base.NPO.DoesNotPersist;
		if (this.num_cards_ == 1)
		{
			this.LastCard = this.TakeCard(false, true);
		}
		if (Network.isServer)
		{
			base.GetComponent<Rigidbody>().AddForce(Vector3.down);
		}
		EventManager.TriggerObjectLeaveContainer(base.NPO, component);
		base.NPO.UpdateVisiblity(true);
		return gameObject;
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00054018 File Offset: 0x00052218
	private void Update()
	{
		if (this.num_cards_ != this.prev_num_cards_ || (base.NPO && base.NPO.Scale != this.PrevScaleMulti))
		{
			float num = base.NPO ? base.NPO.Scale.y : 1f;
			this.prev_num_cards_ = this.num_cards_;
			this.TargetYScale = Mathf.Min(5f, (float)Mathf.Max(1, this.num_cards_) * 0.023f + 0.1f) * num;
			float y = base.GetComponent<Renderer>().bounds.size.y;
			base.transform.localScale = new Vector3(base.transform.localScale.x, this.TargetYScale, base.transform.localScale.z);
			base.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(1f, Mathf.Max(0.04f, Mathf.Min(2f, (this.TargetYScale - 0.075f) * 0.8f)));
			if (!base.NPO)
			{
				return;
			}
			base.NPO.ResetIdleFreeze();
			this.PrevScaleMulti = base.NPO.Scale;
			if (Network.isServer && !base.NPO.IsLocked)
			{
				float num2 = base.GetComponent<Renderer>().bounds.size.y - y;
				num2 = Mathf.Max(0f, num2);
				base.GetComponent<Rigidbody>().position = new Vector3(base.GetComponent<Rigidbody>().position.x, base.GetComponent<Rigidbody>().position.y + num2 + 0.05f, base.GetComponent<Rigidbody>().position.z);
			}
			base.NPO.SetMass(0.75f + (float)Mathf.Min(100, Mathf.Max(1, this.num_cards_)) * 0.005f);
			base.NPO.ResetBounds();
			if (this.num_cards_ <= 1)
			{
				base.GetComponent<Renderer>().enabled = false;
				base.GetComponent<Collider>().enabled = false;
				if (Network.isServer)
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
					return;
				}
			}
			else
			{
				if (!base.NPO.IsHidden)
				{
					base.GetComponent<Renderer>().enabled = true;
				}
				base.GetComponent<Collider>().enabled = true;
			}
		}
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x0005428C File Offset: 0x0005248C
	private void OnCollisionEnter(Collision info)
	{
		if (Network.isServer)
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(info.collider);
			if (!networkPhysicsObject || networkPhysicsObject.CurrentPlayerHand || base.NPO.CurrentPlayerHand)
			{
				return;
			}
			if (networkPhysicsObject.deckScript)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.NotifyDeckHitDeck(base.NPO, networkPhysicsObject, true);
			}
		}
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x000542F9 File Offset: 0x000524F9
	public int get_top_card_id()
	{
		return this.top_card_id_;
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00054301 File Offset: 0x00052501
	public int get_bottom_card_id()
	{
		return this.bottom_card_id_;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0005430C File Offset: 0x0005250C
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetDeckIDs(int bottom_card_id, int top_card_id)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<int, int>(RPCTarget.Others, new Action<int, int>(this.SetDeckIDs), bottom_card_id, top_card_id);
			return;
		}
		this.bottom_card_id_ = bottom_card_id;
		this.top_card_id_ = top_card_id;
		this.CardManagerInstance.SetupCard(base.gameObject, this.bottom_card_id_, this.top_card_id_, false);
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00054367 File Offset: 0x00052567
	public void PlayDrawCardSound()
	{
		base.NPO.soundScript.PlayImpactSound(SoundMaterialType.WoodSurface, 1f);
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00054380 File Offset: 0x00052580
	public override bool Add(NetworkPhysicsObject npo, bool top)
	{
		if (!this.ValidateAdd(npo))
		{
			return false;
		}
		if (npo.cardScript)
		{
			this.Add(NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(npo), top);
		}
		else if (npo.deckScript)
		{
			List<ObjectState> objectsInside = npo.deckScript.ObjectsInside;
			for (int i = 0; i < objectsInside.Count; i++)
			{
				this.Add(objectsInside[i], top);
			}
		}
		npo.SetSmoothDestroy(this.GetPosition());
		EventManager.TriggerObjectEnterContainer(base.NPO, npo);
		return true;
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0005440A File Offset: 0x0005260A
	protected override bool ValidateAdd(NetworkPhysicsObject npo)
	{
		return (npo != null && npo.cardScript) || npo.deckScript;
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00054430 File Offset: 0x00052630
	public override void Add(ObjectState objectState, bool top)
	{
		base.Add(objectState, top);
		if (Network.isServer && !base.NPO.rigidbody.isKinematic)
		{
			base.NPO.rigidbody.velocity /= 3f;
		}
		this.UpdateDeck();
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00054484 File Offset: 0x00052684
	public override NetworkPhysicsObject Remove(ObjectState objectState, Vector3 position = default(Vector3))
	{
		if (this.ObjectsInside.Count == 0)
		{
			return this.LastNPOInside;
		}
		this.PlayDrawCardSound();
		int cardID = -1;
		this.ObjectsInside.Remove(objectState);
		this.RegenerateEndCardIDs();
		if (position == default(Vector3))
		{
			position = this.GetPosition();
		}
		objectState.Transform = new TransformState(base.transform)
		{
			posX = position.x,
			posY = position.y,
			posZ = position.z,
			scaleX = base.NPO.Scale.x,
			scaleY = base.NPO.Scale.y,
			scaleZ = base.NPO.Scale.z
		};
		objectState.ColorDiffuse = new ColourState?(new ColourState(base.NPO.DiffuseColor));
		objectState.Grid = !base.NPO.IgnoresGrid;
		objectState.Autoraise = base.NPO.DoAutoRaise;
		objectState.Snap = !base.NPO.IgnoresSnap;
		objectState.Sticky = base.NPO.IsSticky;
		objectState.Locked = false;
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
		gameObject.GetComponent<CardScript>().SetCardID(cardID);
		gameObject.GetComponent<CardScript>().deck_id_ = base.NPO.ID;
		NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
		component.DoesNotPersist = base.NPO.DoesNotPersist;
		if (this.ObjectsInside.Count == 1)
		{
			this.LastNPOInside = this.Remove(this.ObjectsInside[0], default(Vector3));
		}
		if (Network.isServer)
		{
			base.GetComponent<Rigidbody>().AddForce(Vector3.down);
		}
		EventManager.TriggerObjectLeaveContainer(base.NPO, component);
		this.UpdateDeck();
		return component;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x000025B8 File Offset: 0x000007B8
	private void UpdateDeck()
	{
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00054664 File Offset: 0x00052864
	protected override Vector3 GetPosition()
	{
		Vector3 result;
		if (this.ObjectsInside.Count > 0)
		{
			result = new Vector3(base.transform.position.x, base.transform.position.y + base.GetComponent<Renderer>().bounds.size.y / 2f + 0.1f, base.transform.position.z);
		}
		else
		{
			result = new Vector3(base.transform.position.x, base.transform.position.y + base.GetComponent<Renderer>().bounds.size.y / 2f - 0.1f, base.transform.position.z);
		}
		return result;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0005473A File Offset: 0x0005293A
	public override void SetObjectsInside(List<ObjectState> objectStates)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x00054741 File Offset: 0x00052941
	private static int MaxTypedNumber(NetworkPhysicsObject npo)
	{
		if (Pointer.DEFAULT_TYPED_NUMBER_IS_SINGLE_DIGIT)
		{
			return 9;
		}
		return npo.deckScript.num_cards_;
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00054758 File Offset: 0x00052958
	private static void HandleTypedNumber(NetworkPhysicsObject npo, int playerID, int number)
	{
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, PlayerScript.PointerScript.PointerColorLabel, number, 0);
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, NetworkSingleton<PlayerManager>.Instance.ColourLabelFromID(playerID), number, 0);
	}

	// Token: 0x0400084C RID: 2124
	private const float CARD_THICKNESS_MULT = 0.023f;

	// Token: 0x0400084D RID: 2125
	private const float ORIGINAL_SCALE = 1f;

	// Token: 0x0400084E RID: 2126
	private const float DECK_MASS_PER_CARD = 0.005f;

	// Token: 0x0400084F RID: 2127
	public GameObject card_prefab;

	// Token: 0x04000850 RID: 2128
	public GameObject cardfacade_prefab;

	// Token: 0x04000851 RID: 2129
	private List<int> Deck = new List<int>();

	// Token: 0x04000852 RID: 2130
	private List<ObjectState> CardStates = new List<ObjectState>();

	// Token: 0x04000853 RID: 2131
	private int _top_card_id_;

	// Token: 0x04000854 RID: 2132
	private int _bottom_card_id_;

	// Token: 0x04000855 RID: 2133
	[SerializeField]
	private int _num_cards_ = 52;

	// Token: 0x04000856 RID: 2134
	private bool _bSideways;

	// Token: 0x04000857 RID: 2135
	public string deck_name = "Standard";

	// Token: 0x04000858 RID: 2136
	private int prev_num_cards_ = -1;

	// Token: 0x04000859 RID: 2137
	public bool bRandomSpawn = true;

	// Token: 0x0400085A RID: 2138
	private CardManagerScript CardManagerInstance;

	// Token: 0x0400085B RID: 2139
	private Vector3 PrevScaleMulti;

	// Token: 0x0400085C RID: 2140
	private float TargetYScale;

	// Token: 0x0400085D RID: 2141
	public GameObject LastCard;

	// Token: 0x0400085E RID: 2142
	public PlayersSearchingInventory playersSearchingInventory;

	// Token: 0x0400085F RID: 2143
	public DummyObject DummyObject;
}
