using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HighlightingSystem;
using MoonSharp.Interpreter;
using NewNet;
using RTEditor;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class NetworkPhysicsObject : NetworkBehavior, IRTEditorEventListener, INetworkLifeCycle
{
	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06001649 RID: 5705 RVA: 0x0009AC67 File Offset: 0x00098E67
	// (set) Token: 0x0600164A RID: 5706 RVA: 0x0009AC6F File Offset: 0x00098E6F
	public int ID { get; private set; } = -1;

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x0600164B RID: 5707 RVA: 0x0009AC78 File Offset: 0x00098E78
	// (set) Token: 0x0600164C RID: 5708 RVA: 0x0009AC80 File Offset: 0x00098E80
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public string GUID
	{
		get
		{
			return this._GUID;
		}
		set
		{
			if (value == this._GUID)
			{
				return;
			}
			if (Network.isServer)
			{
				List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
				for (;;)
				{
					bool flag = false;
					for (int i = 0; i < grabbableNPOs.Count; i++)
					{
						NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
						if (!networkPhysicsObject.IsDestroyed && networkPhysicsObject.GUID == value && this != networkPhysicsObject)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					value = LibString.GetRandomGUID();
				}
			}
			this._GUID = value;
			base.DirtySync("GUID");
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x0600164D RID: 5709 RVA: 0x0009AD0A File Offset: 0x00098F0A
	// (set) Token: 0x0600164E RID: 5710 RVA: 0x0009AD12 File Offset: 0x00098F12
	public bool isMine { get; private set; }

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x0600164F RID: 5711 RVA: 0x0009AD1B File Offset: 0x00098F1B
	// (set) Token: 0x06001650 RID: 5712 RVA: 0x0009AD23 File Offset: 0x00098F23
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IsGrabbable
	{
		get
		{
			return this._IsGrabbable;
		}
		set
		{
			if (value != this._IsGrabbable)
			{
				this._IsGrabbable = value;
				base.DirtySync("IsGrabbable");
			}
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06001651 RID: 5713 RVA: 0x0009AD40 File Offset: 0x00098F40
	// (set) Token: 0x06001652 RID: 5714 RVA: 0x0009AD48 File Offset: 0x00098F48
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IsLocked
	{
		get
		{
			return this._IsLocked;
		}
		set
		{
			if (value != this._IsLocked)
			{
				this._IsLocked = value;
				base.DirtySync("IsLocked");
			}
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06001653 RID: 5715 RVA: 0x0009AD65 File Offset: 0x00098F65
	// (set) Token: 0x06001654 RID: 5716 RVA: 0x0009AD6D File Offset: 0x00098F6D
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IgnoresGrid
	{
		get
		{
			return this._IgnoresGrid;
		}
		set
		{
			if (value != this._IgnoresGrid)
			{
				this._IgnoresGrid = value;
				base.DirtySync("IgnoresGrid");
			}
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06001655 RID: 5717 RVA: 0x0009AD8A File Offset: 0x00098F8A
	// (set) Token: 0x06001656 RID: 5718 RVA: 0x0009AD92 File Offset: 0x00098F92
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool DoesNotPersist
	{
		get
		{
			return this._DoesNotPersist;
		}
		set
		{
			if (value != this._DoesNotPersist)
			{
				this._DoesNotPersist = value;
				base.DirtySync("DoesNotPersist");
			}
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06001657 RID: 5719 RVA: 0x0009ADAF File Offset: 0x00098FAF
	// (set) Token: 0x06001658 RID: 5720 RVA: 0x0009ADB7 File Offset: 0x00098FB7
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool DoAutoRaise
	{
		get
		{
			return this._DoAutoRaise;
		}
		set
		{
			if (value != this._DoAutoRaise)
			{
				this._DoAutoRaise = value;
				base.DirtySync("DoAutoRaise");
			}
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06001659 RID: 5721 RVA: 0x0009ADD4 File Offset: 0x00098FD4
	// (set) Token: 0x0600165A RID: 5722 RVA: 0x0009ADDC File Offset: 0x00098FDC
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IsSticky
	{
		get
		{
			return this._IsSticky;
		}
		set
		{
			if (value != this._IsSticky)
			{
				this._IsSticky = value;
				base.DirtySync("IsSticky");
			}
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x0600165B RID: 5723 RVA: 0x0009ADF9 File Offset: 0x00098FF9
	// (set) Token: 0x0600165C RID: 5724 RVA: 0x0009AE01 File Offset: 0x00099001
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IgnoresSnap
	{
		get
		{
			return this._IgnoresSnap;
		}
		set
		{
			if (value != this._IgnoresSnap)
			{
				this._IgnoresSnap = value;
				base.DirtySync("IgnoresSnap");
			}
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x0600165D RID: 5725 RVA: 0x0009AE1E File Offset: 0x0009901E
	// (set) Token: 0x0600165E RID: 5726 RVA: 0x0009AE26 File Offset: 0x00099026
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool ShowTooltip
	{
		get
		{
			return this._ShowTooltip;
		}
		set
		{
			if (value != this._ShowTooltip)
			{
				this._ShowTooltip = value;
				base.DirtySync("ShowTooltip");
			}
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x0600165F RID: 5727 RVA: 0x0009AE43 File Offset: 0x00099043
	// (set) Token: 0x06001660 RID: 5728 RVA: 0x0009AE4B File Offset: 0x0009904B
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool RotatesThroughRotationValues
	{
		get
		{
			return this._RotatesThroughRotationValues;
		}
		set
		{
			if (value != this._RotatesThroughRotationValues)
			{
				this._RotatesThroughRotationValues = value;
				base.DirtySync("RotatesThroughRotationValues");
			}
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001661 RID: 5729 RVA: 0x0009AE68 File Offset: 0x00099068
	// (set) Token: 0x06001662 RID: 5730 RVA: 0x0009AE70 File Offset: 0x00099070
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IgnoresFogOfWar
	{
		get
		{
			return this._IgnoresFogOfWar;
		}
		set
		{
			if (value != this._IgnoresFogOfWar)
			{
				this._IgnoresFogOfWar = value;
				base.DirtySync("IgnoresFogOfWar");
				FogOfWarZone[] array = UnityEngine.Object.FindObjectsOfType<FogOfWarZone>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RemoveObjectHiding(this);
				}
			}
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06001663 RID: 5731 RVA: 0x0009AEB5 File Offset: 0x000990B5
	// (set) Token: 0x06001664 RID: 5732 RVA: 0x0009AEBD File Offset: 0x000990BD
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IsDragSelectable
	{
		get
		{
			return this._IsDragSelectable;
		}
		set
		{
			if (value != this._IsDragSelectable)
			{
				this._IsDragSelectable = value;
				base.DirtySync("IsDragSelectable");
			}
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06001665 RID: 5733 RVA: 0x0009AEDA File Offset: 0x000990DA
	// (set) Token: 0x06001666 RID: 5734 RVA: 0x0009AEE2 File Offset: 0x000990E2
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IsGizmoSelectable
	{
		get
		{
			return this._IsGizmoSelectable;
		}
		set
		{
			if (value != this._IsGizmoSelectable)
			{
				this._IsGizmoSelectable = value;
				base.DirtySync("IsGizmoSelectable");
			}
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06001667 RID: 5735 RVA: 0x0009AEFF File Offset: 0x000990FF
	// (set) Token: 0x06001668 RID: 5736 RVA: 0x0009AF07 File Offset: 0x00099107
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool ShowRulerWhenHeld
	{
		get
		{
			return this._ShowRulerWhenHeld;
		}
		set
		{
			if (value != this._ShowRulerWhenHeld)
			{
				this._ShowRulerWhenHeld = value;
				base.DirtySync("ShowRulerWhenHeld");
			}
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06001669 RID: 5737 RVA: 0x0009AF24 File Offset: 0x00099124
	// (set) Token: 0x0600166A RID: 5738 RVA: 0x0009AF2C File Offset: 0x0009912C
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool ShowGridProjection
	{
		get
		{
			return this._ShowGridProjection;
		}
		set
		{
			if (value != this._ShowGridProjection)
			{
				this._ShowGridProjection = value;
				base.DirtySync("ShowGridProjection");
			}
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x0600166B RID: 5739 RVA: 0x0009AF49 File Offset: 0x00099149
	// (set) Token: 0x0600166C RID: 5740 RVA: 0x0009AF51 File Offset: 0x00099151
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool CanBeHeldInHand
	{
		get
		{
			return this._IsHeldInHand;
		}
		set
		{
			if (value != this._IsHeldInHand)
			{
				this._IsHeldInHand = value;
				base.DirtySync("CanBeHeldInHand");
			}
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x0600166D RID: 5741 RVA: 0x0009AF6E File Offset: 0x0009916E
	// (set) Token: 0x0600166E RID: 5742 RVA: 0x0009AF76 File Offset: 0x00099176
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IsSaved
	{
		get
		{
			return this._IsSaved;
		}
		set
		{
			if (value != this._IsSaved)
			{
				this._IsSaved = value;
				base.DirtySync("IsSaved");
			}
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x0600166F RID: 5743 RVA: 0x0009AF93 File Offset: 0x00099193
	// (set) Token: 0x06001670 RID: 5744 RVA: 0x0009AF9B File Offset: 0x0009919B
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool UseAltSounds
	{
		get
		{
			return this._UseAltSounds;
		}
		set
		{
			if (value != this._UseAltSounds)
			{
				this._UseAltSounds = value;
				if (this.soundScript)
				{
					this.soundScript.UseAltSounds = this.UseAltSounds;
				}
				base.DirtySync("UseAltSounds");
			}
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06001671 RID: 5745 RVA: 0x0009AFD6 File Offset: 0x000991D6
	// (set) Token: 0x06001672 RID: 5746 RVA: 0x0009AFDE File Offset: 0x000991DE
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int ValueFlags
	{
		get
		{
			return this._valueFlags;
		}
		set
		{
			if (value != this._valueFlags)
			{
				this._valueFlags = value;
				base.DirtySync("ValueFlags");
			}
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06001673 RID: 5747 RVA: 0x0009AFFB File Offset: 0x000991FB
	// (set) Token: 0x06001674 RID: 5748 RVA: 0x0009B003 File Offset: 0x00099203
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int Value
	{
		get
		{
			return this._Value;
		}
		set
		{
			if (value != this._Value)
			{
				this._Value = value;
				base.DirtySync("Value");
			}
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06001675 RID: 5749 RVA: 0x0009B020 File Offset: 0x00099220
	// (set) Token: 0x06001676 RID: 5750 RVA: 0x0009B028 File Offset: 0x00099228
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public string Name
	{
		get
		{
			return this._Name;
		}
		set
		{
			if (value != this._Name)
			{
				this._Name = value;
				this.CheckUpdateNotecard();
				base.DirtySync("Name");
			}
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06001677 RID: 5751 RVA: 0x0009B050 File Offset: 0x00099250
	// (set) Token: 0x06001678 RID: 5752 RVA: 0x0009B058 File Offset: 0x00099258
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public string Description
	{
		get
		{
			return this._Description;
		}
		set
		{
			if (value != this._Description)
			{
				this._Description = value;
				this.CheckUpdateNotecard();
				base.DirtySync("Description");
			}
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06001679 RID: 5753 RVA: 0x0009B080 File Offset: 0x00099280
	// (set) Token: 0x0600167A RID: 5754 RVA: 0x0009B088 File Offset: 0x00099288
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public string GMNotes
	{
		get
		{
			return this._GMNotes;
		}
		set
		{
			if (value != this._GMNotes)
			{
				this._GMNotes = value;
				this.CheckUpdateNotecard();
				base.DirtySync("GMNotes");
			}
		}
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x0009B0B0 File Offset: 0x000992B0
	private void CheckUpdateNotecard()
	{
		if (base.gameObject.CompareTag("Notecard"))
		{
			UILabel[] componentsInChildren = this.transform.Find("Panel").GetComponentsInChildren<UILabel>();
			componentsInChildren[1].text = this.Name;
			componentsInChildren[0].text = this.Description;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x0600167C RID: 5756 RVA: 0x0009B0FF File Offset: 0x000992FF
	// (set) Token: 0x0600167D RID: 5757 RVA: 0x0009B107 File Offset: 0x00099307
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public string Memo
	{
		get
		{
			return this._Memo;
		}
		set
		{
			if (value != this._Memo)
			{
				this._Memo = value;
				base.DirtySync("Memo");
			}
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x0600167E RID: 5758 RVA: 0x0009B129 File Offset: 0x00099329
	// (set) Token: 0x0600167F RID: 5759 RVA: 0x0009B131 File Offset: 0x00099331
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public Vector3 AltLookAngle
	{
		get
		{
			return this._AltLookAngle;
		}
		set
		{
			if (value == this._AltLookAngle)
			{
				return;
			}
			this._AltLookAngle = value;
			base.DirtySync("AltLookAngle");
		}
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x0009B154 File Offset: 0x00099354
	private void Awake()
	{
		this.isMine = base.networkView.isMine;
		this.ID = (int)base.networkView.id;
		this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		this.rigidbody.maxAngularVelocity = 20f;
		this.mass = this.rigidbody.mass;
		this.StartMass = this.mass;
		this.BaseScale = this.transform.localScale;
		if (this.MaxTypedNumberFunction == null)
		{
			this.MaxTypedNumberFunction = new NetworkPhysicsObject.MaxTypedNumberMethodDelegate(NetworkPhysicsObject.DefaultMaxTypedNumber);
		}
		if (this.HandleTypedNumber == null)
		{
			this.HandleTypedNumber = new NetworkPhysicsObject.HandleTypedNumberMethodDelegate(NetworkPhysicsObject.DefaultHandleTypedNumber);
		}
		base.GetComponentsInChildren<Renderer>(this.Renderers);
		for (int i = this.Renderers.Count - 1; i >= 0; i--)
		{
			if (!this.Renderers[i].enabled)
			{
				this.Renderers.RemoveAt(i);
			}
		}
		if (this.tableScript)
		{
			for (int j = 0; j < this.tableScript.ObjectsToShowForAbstract.Length; j++)
			{
				foreach (Renderer item in this.tableScript.ObjectsToShowForAbstract[j].GetComponentsInChildren<Renderer>(true))
				{
					this.Renderers.Add(item);
				}
			}
			for (int l = 0; l < this.tableScript.CustomObjectsToShowForAbstract.Length; l++)
			{
				foreach (Renderer item2 in this.tableScript.CustomObjectsToShowForAbstract[l].GetComponentsInChildren<Renderer>(true))
				{
					this.Renderers.Add(item2);
				}
			}
		}
		base.GetComponentsInChildren<UIPanel>(this.UIPanels);
		for (int m = this.UIPanels.Count - 1; m >= 0; m--)
		{
			if (!this.UIPanels[m].enabled)
			{
				this.UIPanels.RemoveAt(m);
			}
		}
		base.GetComponentsInChildren<Collider>(this.Colliders);
		for (int n = 0; n < this.Colliders.Count; n++)
		{
			Collider collider = this.Colliders[n];
			if (collider.enabled && collider.isTrigger)
			{
				this.TriggerOnlyColliders.Add(collider);
			}
		}
		this.SetupGrabbableLayer(true);
		if (Network.isServer)
		{
			this.SpawnFreeze();
		}
		else
		{
			this.isKinematic = true;
		}
		if (!this.customObject)
		{
			this.SetupDefaultRotationValues();
		}
		if (FastDrag.Enabled && this.IsRegistered)
		{
			this.fastDrag = base.gameObject.AddComponent<FastDrag>();
		}
		if (zInput.bTouchEventFired)
		{
			base.gameObject.AddComponent<TouchHandlerScript>();
		}
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x0009B3F4 File Offset: 0x000995F4
	private void Start()
	{
		this.Spawn();
		Wait.Frames(delegate
		{
			EventManager.TriggerLuaObjectSpawn(this.luaGameObjectScript);
		}, 1);
		EventManager.TriggerNetworkObjectSpawn(this);
		if (this.spawnedByUI)
		{
			EventManager.TriggerNetworkObjectSpawnFromUI(this);
		}
		this.collisionEvents.OnCollisionEnterEvent += this.ManagedOnCollisionEnter;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected += this.OnPlayerDisconnect;
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x0009B468 File Offset: 0x00099668
	private void OnDestroy()
	{
		if (!base.GetComponent<DummyObject>())
		{
			EventManager.TriggerLuaObjectDestroy(this.luaGameObjectScript);
		}
		EventManager.TriggerNetworkObjectDestroy(this);
		this.collisionEvents.OnCollisionEnterEvent -= this.ManagedOnCollisionEnter;
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected -= this.OnPlayerDisconnect;
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0009B4CC File Offset: 0x000996CC
	public void Creation()
	{
		if (this.IsRegistered)
		{
			if (Network.isServer)
			{
				this.GUID = LibString.GetRandomGUID();
			}
			if (NetworkSingleton<ManagerPhysicsObject>.Instance)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.RegisterGrabbableObject(this);
			}
		}
		else if (NetworkSingleton<ManagerPhysicsObject>.Instance)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.RegisterManagedObject(this);
		}
		if (this.Renderers.Count > 0 && this.Renderers[0].sharedMaterial && this.Renderers[0].sharedMaterial.mainTexture)
		{
			if (!base.CompareTag("Deck") && !base.CompareTag("Card"))
			{
				this.Renderers[0].sharedMaterial.mainTexture.mipMapBias = MipmapBiasTextures.CurrentMipMapBias;
				return;
			}
			this.Renderers[0].sharedMaterial.mainTexture.mipMapBias = 0f;
		}
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x0009B5C4 File Offset: 0x000997C4
	private void Spawn()
	{
		this.ResetRigidbody();
		this.ResetPhysicsMaterial();
		this.ResetBounds();
		this.FixConcave();
		if (this.isMine)
		{
			this.isKinematic = this.IsLocked;
		}
		if (Network.isServer)
		{
			this.GetSelectedStateId();
			this.GetStatesCount();
			this.SyncStatesToOthers();
		}
		if (this.spawnedByUI && NetworkPhysicsObject.OverrideDefaultsWhenSpawning)
		{
			this.IgnoresGrid = !NetworkPhysicsObject.GridOverride;
			this.IgnoresSnap = !NetworkPhysicsObject.SnapOverride;
			this.CanBeHeldInHand = NetworkPhysicsObject.UseHandsOverride;
			this.DoAutoRaise = NetworkPhysicsObject.AutoRaiseOverride;
			this.IsSticky = NetworkPhysicsObject.StickyOverride;
			this.ShowTooltip = NetworkPhysicsObject.TooltipOverride;
			this.IgnoresFogOfWar = NetworkPhysicsObject.IgnoreFOWOverride;
			this.fogOfWarRevealer.Active = NetworkPhysicsObject.RevealFOROverride;
		}
		if (Network.isServer && !base.GetComponent<DummyObject>())
		{
			base.GetComponentsInChildren<SnapPoint>(NetworkPhysicsObject.snaps);
			if (NetworkPhysicsObject.snaps.Count > 0)
			{
				List<SnapPointInfo> snapPointStates = NetworkSingleton<SnapPointManager>.Instance.GetSnapPointStates(base.networkView);
				if (snapPointStates == null || snapPointStates.Count != NetworkPhysicsObject.snaps.Count)
				{
					List<LuaGameObjectScript.LuaSnapPointParameters> list = new List<LuaGameObjectScript.LuaSnapPointParameters>();
					foreach (SnapPoint snapPoint in NetworkPhysicsObject.snaps)
					{
						Transform transform = snapPoint.transform;
						transform.parent = this.transform;
						list.Add(new LuaGameObjectScript.LuaSnapPointParameters
						{
							position = transform.localPosition,
							rotation = transform.localEulerAngles,
							rotation_snap = snapPoint.bRotate,
							tags = null
						});
						UnityEngine.Object.Destroy(snapPoint.gameObject);
					}
					NetworkSingleton<SnapPointManager>.Instance.SetSnapPoints(list, base.networkView);
				}
			}
		}
	}

	// Token: 0x06001685 RID: 5765 RVA: 0x0009B798 File Offset: 0x00099998
	public void Destruction()
	{
		this.ID = -1;
		if (NetworkSingleton<ManagerPhysicsObject>.Instance)
		{
			if (this.IsRegistered)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.UnRegisterGrabbableObject(this);
				return;
			}
			NetworkSingleton<ManagerPhysicsObject>.Instance.UnRegisterManagedObject(this);
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06001686 RID: 5766 RVA: 0x0009B7CC File Offset: 0x000999CC
	public bool IsDestroyed
	{
		get
		{
			return this.ID == -1 || this.HeldIDIndicatesDestruction || this.currentSmoothPosition.DestroyWhenFinish || this.currentSmoothRotation.DestroyWhenFinish;
		}
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06001687 RID: 5767 RVA: 0x0009B7F9 File Offset: 0x000999F9
	public bool IsTryingToMove
	{
		get
		{
			return this.IsHeldBySomebody || this.IsSmoothMoving;
		}
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x0009B80C File Offset: 0x00099A0C
	public bool IsLoadingCustom()
	{
		if (this.cardScript || this.deckScript)
		{
			Material[] sharedMaterials = this.Renderers[0].sharedMaterials;
			return !sharedMaterials[1].mainTexture || !sharedMaterials[2].mainTexture;
		}
		return this.customObject && !this.customObject.bFinished;
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x0009B884 File Offset: 0x00099A84
	public void ManagedLateUpdate()
	{
		bool flag = this.rigidbody.IsSleeping();
		if (!this.sleeping && flag)
		{
			this.JustFellAsleep = true;
		}
		this.sleeping = flag;
		if (Debugging.bDebug && !this.sleeping && this.highlighter)
		{
			this.highlighter.Hover(Colour.UnityRed);
		}
		if (Vector3.Dot(this.transform.up, Vector3.down) > 0.8f)
		{
			if (!this.IsFaceDown)
			{
				this.IsFaceDown = true;
				if (this.IsHiddenWhenFaceDown)
				{
					this.SetObscured("facedown", true, 2147483647U, false);
				}
			}
		}
		else if (this.IsFaceDown)
		{
			this.IsFaceDown = false;
			this.SetObscured("facedown", false, 2147483647U, false);
		}
		if (this.IsHiddenWhenFaceDown != this.prevIsHiddenWhenFaceDown)
		{
			this.prevIsHiddenWhenFaceDown = this.IsHiddenWhenFaceDown;
			if (this.IsFaceDown && this.IsHiddenWhenFaceDown)
			{
				this.SetObscured("facedown", true, 2147483647U, false);
			}
			else
			{
				this.SetObscured("facedown", false, 2147483647U, false);
			}
		}
		if (this.IsHeldBySomebody)
		{
			if (this.LastFrameHeldID != this.HeldByPlayerID)
			{
				this.HighlightColor = NetworkPhysicsObject.DarkColourFromPlayerID(this.HeldByPlayerID);
				if (this.HeldByPlayerID == NetworkID.ID)
				{
					NetworkPhysicsObject.LastNPOHeldByMe = this;
				}
				this.PickedUpFromLayoutZone = -1;
				this.PickedUpFromLayoutZoneGroup = -1;
				LayoutZone layoutZone;
				int num;
				if (Network.isServer && this.InsideALayoutZone && LayoutZone.TryNPOInLayoutZone(this, out layoutZone, out num, LayoutZone.PotentialZoneCheck.None))
				{
					this.PickedUpFromLayoutZone = layoutZone.NPO.ID;
					if (layoutZone.GroupsInZone[num].Count > 1)
					{
						this.PickedUpFromLayoutZoneGroup = num;
					}
					else
					{
						this.PickedUpFromLayoutZoneGroup = -(num + 1);
					}
					LayoutZone.RemoveObjectFromGroups(this, layoutZone, num, true);
				}
				PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(this.HeldByPlayerID);
				if (playerState != null)
				{
					Wait.Frames(delegate
					{
						EventManager.TriggerObjectPickUp(this, playerState);
					}, 1);
				}
			}
			this.LastFrameHeldID = this.HeldByPlayerID;
			if (this.highlighter)
			{
				this.highlighter.Hover(this.HighlightColor);
			}
			if (Network.isServer)
			{
				this.rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
			}
		}
		else
		{
			if (this.LastFrameHeldID > -1)
			{
				PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(this.LastFrameHeldID);
				if (playerState != null)
				{
					Wait.Frames(delegate
					{
						EventManager.TriggerObjectDrop(this, playerState);
					}, 1);
				}
				if (this.PickedUpFromLayoutZone != -1)
				{
					LayoutZone layoutZone2 = LayoutZone.ZoneFromID(this.PickedUpFromLayoutZone);
					LayoutZone x;
					int num2;
					if (LayoutZone.TryNPOInLayoutZone(this, out x, out num2, LayoutZone.PotentialZoneCheck.None))
					{
						if (x != layoutZone2 && layoutZone2.Options.InstantRefill)
						{
							layoutZone2.QueueUpdate(-1, 0.2f, false);
						}
					}
					else if (layoutZone2.Options.InstantRefill)
					{
						layoutZone2.QueueUpdate(-1, 0.2f, false);
					}
				}
			}
			this.LastFrameHeldID = -1;
			if (this.highlighter)
			{
				if (this.PlayersSelectingWithGizmo.Count > 0)
				{
					this.highlighter.Hover(NetworkPhysicsObject.DarkColourFromPlayerID(this.PlayersSelectingWithGizmo[0]));
				}
				else if (this.TemporaryHighlightColor != null)
				{
					if (this.TemporaryHighlightColorPulse == 0f)
					{
						this.highlighter.Hover(this.TemporaryHighlightColor.Value);
					}
					else
					{
						float t = Mathf.Abs(Mathf.Sin(3.1415927f * (Time.time - this.TemporaryHighlightColorStart) / this.TemporaryHighlightColorPulse));
						this.highlighter.Hover(Colour.AlphaFade(this.TemporaryHighlightColor.Value, t));
					}
				}
				else if (this.OverrideHighlightColor != null)
				{
					this.highlighter.Hover(this.OverrideHighlightColor.Value);
				}
				else if (this.LuaHighlightColor != null)
				{
					this.highlighter.Hover(this.LuaHighlightColor.Value);
				}
			}
			if (Network.isServer)
			{
				this.rigidbody.interpolation = RigidbodyInterpolation.None;
			}
		}
		if (this.isMine)
		{
			Transform transform = this.transform;
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			this.justMoved = (this.prevPosition != position || this.prevRotation != rotation);
			if (this.justMoved)
			{
				this.prevPosition = position;
				this.prevRotation = rotation;
			}
			if (this.IsHeldByNobody)
			{
				this.SetLayerToHeld(false);
			}
			if (this.IsGrabbable && this.NPOBeingRotated)
			{
				if (!this.NPOBeingRotated.IsCollidable && this.NPOBeingRotated.IsHeldByNobody && this.IsHeldByNobody && this.IsCollidable)
				{
					this.isKinematic = true;
					return;
				}
				this.NPOBeingRotated = null;
			}
			if (this.isSpawnFreeze && !this.IsTryingToMove)
			{
				this.isKinematic = true;
				return;
			}
			if (this.IsLoadingCustom() && !this.IsTryingToMove)
			{
				this.isKinematic = true;
				return;
			}
			if (this.checkLoadFreeze)
			{
				if (UILoading.IsLoading)
				{
					if (!this.IsTryingToMove)
					{
						this.isKinematic = true;
						return;
					}
					if (!this.customObject || !this.customObject.CurrentlyLoading() || !this.customObject.FreezeDuringLoad)
					{
						this.checkLoadFreeze = false;
					}
				}
				else
				{
					this.checkLoadFreeze = false;
				}
			}
			if (this.PlayersSelectingWithGizmo.Count > 0)
			{
				this.isKinematic = true;
				return;
			}
			if (this.IsHeldByNobody && (ServerOptions.isPhysicsSemi || ServerOptions.isPhysicsLock) && !this.IsMoving && !this.IsSmoothMoving)
			{
				if (this.SleepTimeHolder > 0.5f)
				{
					this.isKinematic = true;
					return;
				}
				this.SleepTimeHolder += Time.deltaTime;
			}
			else
			{
				this.ResetIdleFreeze();
			}
			if (this.IsSmoothMoving)
			{
				this.isKinematic = false;
			}
			else if (this.isKinematic != this.IsLocked)
			{
				this.isKinematic = this.IsLocked;
				if (!this.IsLocked)
				{
					this.rigidbody.WakeUp();
				}
				this.FixConcave();
			}
			if (!this.IsLocked && position.magnitude > 10000f && !this.tableScript)
			{
				Chat.LogError("Destroying " + TTSUtilities.CleanName(this) + " because it fell through the world.", true);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
			}
			return;
		}
		else
		{
			this.isKinematic = true;
			if (this.isGizmoSelectedByMe)
			{
				this.interpolate.enabled = false;
				return;
			}
			return;
		}
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x0009BF12 File Offset: 0x0009A112
	private void ManagedOnCollisionEnter(Collision collisionInfo)
	{
		if (Network.isServer)
		{
			if (this.bColliding)
			{
				this.StopSmoothMove(true);
			}
			this.bReduceForce = true;
		}
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x0009BF34 File Offset: 0x0009A134
	private void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			if (this.States != null)
			{
				base.networkView.RPC<Dictionary<int, ObjectState>>(player, new Action<Dictionary<int, ObjectState>>(this.RPCStates), this.States);
			}
			if (this.RotationValues != null && this.RotationValues.Count > 0)
			{
				base.networkView.RPC<List<RotationValue>>(player, new Action<List<RotationValue>>(this.RPCSetRotationValues), this.RotationValues);
			}
			this.UpdateNPOHiders(player);
			this.UpdateNPOTags(player);
			this.GiveAllCustomContextMenus(player);
		}
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x0009BFB8 File Offset: 0x0009A1B8
	private void OnPlayerDisconnect(NetworkPlayer player, DisconnectInfo info)
	{
		if (Network.isServer)
		{
			int num = NetworkID.IDFromNetworkPlayer(player);
			if (this.PlayersSelectingWithGizmo.Contains(num))
			{
				base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCRemoveGizmoSelected), num);
			}
		}
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x0009BFFA File Offset: 0x0009A1FA
	public bool isSidewaysCard()
	{
		return (this.deckScript && this.deckScript.bSideways) || (this.cardScript && this.cardScript.bSideways);
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x0009C032 File Offset: 0x0009A232
	public HandZone GetContainingHandZone()
	{
		return HandZone.GetHandZoneWhichContains(this);
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x0600168F RID: 5775 RVA: 0x0009C03A File Offset: 0x0009A23A
	public bool IsHandZoneStash
	{
		get
		{
			return this.CurrentPlayerHand != null && this.CurrentPlayerHand.Stash == this;
		}
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x0009C060 File Offset: 0x0009A260
	public void ContextualMenu(int playerID, int menuID)
	{
		if (Network.isServer)
		{
			this.ContextualMenuRPC(playerID, menuID, this.transform.position, this.luaGameObjectScript);
			return;
		}
		base.networkView.RPC<int, int, Vector3, LuaGameObjectScript>(RPCTarget.Server, new Action<int, int, Vector3, LuaGameObjectScript>(this.ContextualMenuRPC), playerID, menuID, this.transform.position, this.luaGameObjectScript);
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x0009C0BC File Offset: 0x0009A2BC
	[Remote(SendType.ReliableNoDelay)]
	public void ContextualMenuRPC(int playerID, int menuID, Vector3 position, LuaGameObjectScript npo)
	{
		if (!Network.isServer)
		{
			return;
		}
		MoonSharp.Interpreter.Closure function;
		if (NetworkSingleton<UserDefinedContextualManager>.Instance.ObjectMethods.TryGetValue(menuID, out function))
		{
			string text = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(playerID));
			LuaScript.TryCall(function, new object[]
			{
				text,
				position,
				npo
			});
		}
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0009C11A File Offset: 0x0009A31A
	public void AddCustomContextMenu(int id)
	{
		if (!Network.isServer || this.CustomContextMenus.Contains(id))
		{
			return;
		}
		this.CustomContextMenus.Add(id);
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.RPCAddCustomContextMenu), id);
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0009C157 File Offset: 0x0009A357
	[Remote(Permission.Server)]
	public void RPCAddCustomContextMenu(int id)
	{
		if (Network.isServer)
		{
			return;
		}
		this.CustomContextMenus.Add(id);
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0009C170 File Offset: 0x0009A370
	public void GiveAllCustomContextMenus(NetworkPlayer player)
	{
		if (!Network.isServer)
		{
			return;
		}
		for (int i = 0; i < this.CustomContextMenus.Count; i++)
		{
			base.networkView.RPC<int>(player, new Action<int>(this.RPCAddCustomContextMenu), this.CustomContextMenus[i]);
		}
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x0009C1BF File Offset: 0x0009A3BF
	public void ClearCustomContextMenu()
	{
		if (!Network.isServer)
		{
			return;
		}
		this.CustomContextMenus.Clear();
		base.networkView.RPC(RPCTarget.Others, new Action(this.RPCClearCustomContextMenu));
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x0009C1EC File Offset: 0x0009A3EC
	[Remote(Permission.Server)]
	public void RPCClearCustomContextMenu()
	{
		if (Network.isServer)
		{
			return;
		}
		this.CustomContextMenus.Clear();
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06001697 RID: 5783 RVA: 0x0009C201 File Offset: 0x0009A401
	public bool IsGizmoSelectedBySomebody
	{
		get
		{
			return this.PlayersSelectingWithGizmo.Count > 0;
		}
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x0009C211 File Offset: 0x0009A411
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCAddGizmoSelected(int id)
	{
		if (!this.PlayersSelectingWithGizmo.Contains(id))
		{
			this.PlayersSelectingWithGizmo.Add(id);
		}
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x0009C22D File Offset: 0x0009A42D
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCRemoveGizmoSelected(int id)
	{
		if (this.PlayersSelectingWithGizmo.Contains(id))
		{
			this.PlayersSelectingWithGizmo.Remove(id);
		}
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x0009C24A File Offset: 0x0009A44A
	public bool OnCanBeSelected(ObjectSelectEventArgs selectEventArgs)
	{
		return !this.tableScript && this.IsGizmoSelectable;
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x0009C261 File Offset: 0x0009A461
	public void OnSelected(ObjectSelectEventArgs selectEventArgs)
	{
		this.isGizmoSelectedByMe = true;
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCAddGizmoSelected), NetworkID.ID);
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x0009C287 File Offset: 0x0009A487
	public void OnDeselected(ObjectDeselectEventArgs deselectEventArgs)
	{
		this.isGizmoSelectedByMe = false;
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCRemoveGizmoSelected), NetworkID.ID);
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x000025B8 File Offset: 0x000007B8
	public void OnAlteredByTransformGizmo(Gizmo gizmo)
	{
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x0600169E RID: 5790 RVA: 0x0009C2AD File Offset: 0x0009A4AD
	// (set) Token: 0x0600169F RID: 5791 RVA: 0x0009C2B8 File Offset: 0x0009A4B8
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int HeldByPlayerID
	{
		get
		{
			return this._HeldByPlayerID;
		}
		set
		{
			if (value != this._HeldByPlayerID)
			{
				if (this._HeldByPlayerID >= 0)
				{
					Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(this.HeldByPlayerID);
					if (pointer)
					{
						pointer.HeldObjects.Remove(this);
					}
				}
				this._HeldByPlayerID = value;
				base.DirtySync("HeldByPlayerID");
				if (value >= 0)
				{
					Pointer pointer2 = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(this._HeldByPlayerID);
					if (pointer2)
					{
						pointer2.HeldObjects.TryAddUnique(this);
					}
					this.PickedUpPosition = this.transform.position;
					this.PickedUpRotation = this.transform.eulerAngles;
				}
			}
		}
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0009C35E File Offset: 0x0009A55E
	public bool IsHeldBySomebody
	{
		get
		{
			return this._HeldByPlayerID >= 0;
		}
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060016A1 RID: 5793 RVA: 0x0009C36C File Offset: 0x0009A56C
	public bool IsHeldByNobody
	{
		get
		{
			return this._HeldByPlayerID <= -1;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x060016A2 RID: 5794 RVA: 0x0009C37A File Offset: 0x0009A57A
	public bool HeldIDIndicatesDestruction
	{
		get
		{
			return this._HeldByPlayerID == -10;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060016A3 RID: 5795 RVA: 0x0009C386 File Offset: 0x0009A586
	public bool IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID
	{
		get
		{
			return this._HeldByPlayerID == -1;
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060016A4 RID: 5796 RVA: 0x0009C391 File Offset: 0x0009A591
	// (set) Token: 0x060016A5 RID: 5797 RVA: 0x0009C3A7 File Offset: 0x0009A5A7
	public Vector3 HeldOffset
	{
		get
		{
			if (this.HeldCloseInVR != 0)
			{
				return Vector3.zero;
			}
			return this._HeldOffset;
		}
		set
		{
			this._HeldOffset = value;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x060016A6 RID: 5798 RVA: 0x0009C3B0 File Offset: 0x0009A5B0
	// (set) Token: 0x060016A7 RID: 5799 RVA: 0x0009C3C6 File Offset: 0x0009A5C6
	public Vector3 HeldRotationOffset
	{
		get
		{
			if (this.HeldCloseInVR != 0)
			{
				return Vector3.zero;
			}
			return this._HeldRotationOffset;
		}
		set
		{
			this._HeldRotationOffset = value;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x060016A8 RID: 5800 RVA: 0x0009C3CF File Offset: 0x0009A5CF
	// (set) Token: 0x060016A9 RID: 5801 RVA: 0x0009C3F4 File Offset: 0x0009A5F4
	public Vector3 HeldByControllerPickupOffset
	{
		get
		{
			if (this.HeldCloseInVR != 0)
			{
				return new Vector3(0f, 0.8f, 0.5f);
			}
			return this._HeldByControllerPickupOffset;
		}
		set
		{
			this._HeldByControllerPickupOffset = value;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x060016AA RID: 5802 RVA: 0x0009C3FD File Offset: 0x0009A5FD
	// (set) Token: 0x060016AB RID: 5803 RVA: 0x0009C413 File Offset: 0x0009A613
	public Quaternion HeldByControllerPickupRotation
	{
		get
		{
			if (this.HeldCloseInVR != 0)
			{
				return Quaternion.identity;
			}
			return this._HeldByControllerPickupRotation;
		}
		set
		{
			this._HeldByControllerPickupRotation = value;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x060016AC RID: 5804 RVA: 0x0009C41C File Offset: 0x0009A61C
	// (set) Token: 0x060016AD RID: 5805 RVA: 0x0009C433 File Offset: 0x0009A633
	public int HeldSpinRotationIndex
	{
		get
		{
			if (this.HeldCloseInVR != 0)
			{
				return this.heldSpinRotationIndexVR;
			}
			return this.heldSpinRotationIndex;
		}
		set
		{
			if (this.HeldCloseInVR != 0)
			{
				this.heldSpinRotationIndexVR = value;
				return;
			}
			this.heldSpinRotationIndex = value;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x060016AE RID: 5806 RVA: 0x0009C44C File Offset: 0x0009A64C
	// (set) Token: 0x060016AF RID: 5807 RVA: 0x0009C463 File Offset: 0x0009A663
	public int HeldFlipRotationIndex
	{
		get
		{
			if (this.HeldCloseInVR != 0)
			{
				return this.heldFlipRotationIndexVR;
			}
			return this.heldFlipRotationIndex;
		}
		set
		{
			if (this.HeldCloseInVR != 0)
			{
				this.heldFlipRotationIndexVR = value;
				return;
			}
			this.heldFlipRotationIndex = value;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x060016B0 RID: 5808 RVA: 0x0009C47C File Offset: 0x0009A67C
	// (set) Token: 0x060016B1 RID: 5809 RVA: 0x0009C484 File Offset: 0x0009A684
	public int HeldCloseInVR
	{
		get
		{
			return this._HeldCloseInVR;
		}
		set
		{
			if (value != this._HeldCloseInVR)
			{
				this._HeldCloseInVR = value;
				this.DisableFastDragWhileAnimating();
			}
		}
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x0009C49C File Offset: 0x0009A69C
	public void ResetVRRotations()
	{
		this.heldFlipRotationIndexVR = 0;
		if (this.cardScript && this.cardScript.bSideways)
		{
			this.heldSpinRotationIndexVR = 6;
			return;
		}
		this.heldSpinRotationIndexVR = 12;
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x060016B3 RID: 5811 RVA: 0x0009C4CF File Offset: 0x0009A6CF
	// (set) Token: 0x060016B4 RID: 5812 RVA: 0x0009C4D7 File Offset: 0x0009A6D7
	public bool bheldLayer { get; private set; }

	// Token: 0x060016B5 RID: 5813 RVA: 0x0009C4E0 File Offset: 0x0009A6E0
	public void SetLayerToHeld(bool bHeldLayer)
	{
		if (bHeldLayer == this.bheldLayer)
		{
			return;
		}
		this.bheldLayer = bHeldLayer;
		base.GetComponentsInChildren<Collider>(this.Colliders);
		if (bHeldLayer)
		{
			for (int i = 0; i < this.Colliders.Count; i++)
			{
				Collider collider = this.Colliders[i];
				if (collider.enabled)
				{
					GameObject gameObject = collider.gameObject;
					if (gameObject.layer == 10)
					{
						gameObject.layer = 9;
					}
				}
			}
			return;
		}
		for (int j = 0; j < this.Colliders.Count; j++)
		{
			Collider collider2 = this.Colliders[j];
			if (collider2.enabled)
			{
				GameObject gameObject2 = collider2.gameObject;
				if (gameObject2.layer == 9)
				{
					gameObject2.layer = 10;
				}
			}
		}
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x0009C59C File Offset: 0x0009A79C
	public void Drop()
	{
		this.ResetObject();
		this.SetLayerToHeld(false);
		this.SetCollision(false);
		this.SetCollision(true);
	}

	// Token: 0x060016B7 RID: 5815 RVA: 0x0009C5B9 File Offset: 0x0009A7B9
	public void SetHeldClose(int playerID, bool immediate = false)
	{
		if (playerID == this.HeldCloseInVR)
		{
			return;
		}
		this.SetHeldCloseRPC(playerID, immediate);
		base.networkView.RPC<int, bool>(RPCTarget.Others, new Action<int, bool>(this.SetHeldCloseRPC), playerID, immediate);
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x0009C5E8 File Offset: 0x0009A7E8
	[Remote(SendType.ReliableNoDelay)]
	public void SetHeldCloseRPC(int playerID, bool immediate)
	{
		if (playerID == 0)
		{
			if (immediate)
			{
				this.SetObscured("VR", false, 2147483647U, false);
			}
			else
			{
				base.StartCoroutine(this.RemoveHideAfterDelay("VR", 0.33f));
			}
		}
		else
		{
			this.SetObscured("VR", true, Colour.InverseFlagsFromLabel(NetworkSingleton<PlayerManager>.Instance.ColourLabelFromID(playerID)), false);
		}
		this.HeldCloseInVR = playerID;
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x0009C650 File Offset: 0x0009A850
	[Remote(SendType.ReliableNoDelay)]
	public void ReturnToPickupPosition()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.ReturnToPickupPosition));
			return;
		}
		this.SetRotation(this.PickedUpRotation);
		this.SetSmoothPosition(this.PickedUpPosition, false, true, false, true, null, false, true, null);
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x0009C6A8 File Offset: 0x0009A8A8
	[Remote(SendType.ReliableNoDelay)]
	public void RPCMaybeInsertDroppedObjects(int playerID, List<ObjectState> mightInsertOSs, List<NetworkPhysicsObject> mightInsertNPOs)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, List<ObjectState>, List<NetworkPhysicsObject>>(RPCTarget.Server, new Action<int, List<ObjectState>, List<NetworkPhysicsObject>>(this.RPCMaybeInsertDroppedObjects), playerID, mightInsertOSs, mightInsertNPOs);
			return;
		}
		List<ObjectState> list = new List<ObjectState>();
		LuaGameObjectScript component = base.GetComponent<LuaGameObjectScript>();
		bool flag = false;
		for (int i = 0; i < mightInsertOSs.Count; i++)
		{
			ObjectState objectState = mightInsertOSs[i];
			NetworkPhysicsObject networkPhysicsObject = mightInsertNPOs[i];
			if (objectState != null)
			{
				if (networkPhysicsObject != null)
				{
					if (component == null || component.CheckObjectEnter(networkPhysicsObject))
					{
						list.Add(objectState);
						EventManager.TriggerObjectEnterContainer(this, networkPhysicsObject);
						NetworkSingleton<ManagerPhysicsObject>.Instance.CheckPutInContainerDestroy(this, networkPhysicsObject);
					}
					else
					{
						networkPhysicsObject.ReturnToPickupPosition();
					}
				}
				else if (flag)
				{
					list.Add(objectState);
				}
			}
			else
			{
				flag = (component == null || component.CheckObjectEnter(networkPhysicsObject));
				if (flag)
				{
					EventManager.TriggerObjectEnterContainer(this, networkPhysicsObject);
					NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(networkPhysicsObject.gameObject);
				}
				else
				{
					networkPhysicsObject.ReturnToPickupPosition();
				}
			}
		}
		base.networkView.RPC<List<ObjectState>>(Network.IdToNetworkPlayer(playerID), new Action<List<ObjectState>>(this.RPCInsertDroppedObjects), list);
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x0009C7C4 File Offset: 0x0009A9C4
	[Remote(SendType.ReliableNoDelay)]
	public void RPCInsertDroppedObjects(List<ObjectState> insertOSs)
	{
		NetworkSingleton<NetworkUI>.Instance.GUIInventory.InsertDroppedObjects(insertOSs);
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x060016BC RID: 5820 RVA: 0x0009C7D6 File Offset: 0x0009A9D6
	public bool IsHidden
	{
		get
		{
			return this.IsInvisible || this.IsObscured;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060016BD RID: 5821 RVA: 0x0009C7E8 File Offset: 0x0009A9E8
	// (set) Token: 0x060016BE RID: 5822 RVA: 0x0009C7F0 File Offset: 0x0009A9F0
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool IsHiddenWhenFaceDown
	{
		get
		{
			return this._IsHiddenWhenFaceDown;
		}
		set
		{
			if (value != this._IsHiddenWhenFaceDown)
			{
				this._IsHiddenWhenFaceDown = value;
				base.DirtySync("IsHiddenWhenFaceDown");
			}
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x060016BF RID: 5823 RVA: 0x0009C80D File Offset: 0x0009AA0D
	public bool CanBePeeked
	{
		get
		{
			return !this.IsHidden || (!this.IsInvisible && this.ObscuredHiders.Count == 1 && this.ObscuredHiders.ContainsKey("facedown"));
		}
	}

	// Token: 0x060016C0 RID: 5824 RVA: 0x0009C844 File Offset: 0x0009AA44
	private static uint RecalculateHiderFlags(Dictionary<string, uint> hiders, out uint regularFlags, out uint invertedFlags)
	{
		regularFlags = 0U;
		invertedFlags = 0U;
		uint result = 0U;
		foreach (KeyValuePair<string, uint> keyValuePair in hiders)
		{
			uint value = keyValuePair.Value;
			if ((value & 2147483648U) > 0U)
			{
				invertedFlags |= (value & 2147483647U);
			}
			else if (keyValuePair.Key == "facedown")
			{
				result = value;
			}
			else
			{
				regularFlags |= value;
			}
		}
		return result;
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x060016C1 RID: 5825 RVA: 0x0009C8D4 File Offset: 0x0009AAD4
	public bool IsInvisible
	{
		get
		{
			if (this.OverrideIsInvisible)
			{
				this.CachedIsInvisible = true;
				return true;
			}
			if (!NetworkSingleton<NetworkUI>.Instance || !Singleton<TeamScript>.Instance)
			{
				return false;
			}
			string text = NetworkSingleton<NetworkUI>.Instance.HotseatBetweenTurns ? "Grey" : Colour.MyColorLabel();
			if (text != "Black")
			{
				uint num = Colour.FlagFromLabel(text) | Singleton<TeamScript>.Instance.TeamFlagsFromPlayerID(NetworkID.ID);
				if ((this.NormalInvisibleFlags != 0U && (this.NormalInvisibleFlags & num) == num) || (this.ReverseInvisibleFlags != 0U && (this.ReverseInvisibleFlags & num) != 0U))
				{
					this.CachedIsInvisible = true;
					return true;
				}
			}
			this.CachedIsInvisible = false;
			return false;
		}
	}

	// Token: 0x060016C2 RID: 5826 RVA: 0x0009C97F File Offset: 0x0009AB7F
	public static bool IsHiddenToMe(uint hiderFlags, bool includeTeam = true)
	{
		if ((hiderFlags & 2147483648U) != 0U)
		{
			return NetworkPhysicsObject.IsHiddenToMe(0U, hiderFlags & 2147483647U, includeTeam);
		}
		return NetworkPhysicsObject.IsHiddenToMe(hiderFlags, 0U, includeTeam);
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x0009C9A4 File Offset: 0x0009ABA4
	public static bool IsHiddenToMe(uint normalHideForFlags, uint reverseHideForFlags, bool includeTeam = true)
	{
		if (NetworkSingleton<NetworkUI>.Instance.HotseatBetweenTurns)
		{
			return true;
		}
		string text = Colour.MyColorLabel();
		if (text == "Black")
		{
			return false;
		}
		uint num = Colour.FlagFromLabel(text);
		if (includeTeam)
		{
			num |= Singleton<TeamScript>.Instance.TeamFlagsFromPlayerID(NetworkID.ID);
		}
		return (normalHideForFlags != 0U && (normalHideForFlags & num) == num) || (reverseHideForFlags != 0U && (reverseHideForFlags & num) > 0U);
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x0009CA06 File Offset: 0x0009AC06
	public void ForceInvisible(bool isInvisible)
	{
		this.OverrideIsInvisible = isInvisible;
		if (this.CachedIsInvisible != isInvisible)
		{
			this.UpdateVisiblity(false);
		}
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x0009CA20 File Offset: 0x0009AC20
	public void SetInvisible(string hiderLabel, bool hiderIsActive, uint players = 2147483647U, bool doRPC = false, bool isInverted = false)
	{
		if (isInverted)
		{
			players |= 2147483648U;
		}
		bool isHidden = this.IsHidden;
		bool flag;
		if (hiderIsActive)
		{
			uint num;
			flag = (!this.InvisibleHiders.TryGetValue(hiderLabel, out num) || num != players);
			if (flag)
			{
				this.InvisibleHiders[hiderLabel] = players;
			}
		}
		else
		{
			flag = this.InvisibleHiders.Remove(hiderLabel);
		}
		if (flag)
		{
			NetworkPhysicsObject.RecalculateHiderFlags(this.InvisibleHiders, out this.NormalInvisibleFlags, out this.ReverseInvisibleFlags);
			this.UpdateVisiblity(false);
		}
		if (doRPC)
		{
			base.networkView.RPC<string, bool, uint>(RPCTarget.Others, new Action<string, bool, uint>(this.SetInvisibleRPC), hiderLabel, hiderIsActive, players);
		}
		if (isHidden != this.IsHidden)
		{
			EventManager.TriggerNetworkObjectHide(this, this.IsHidden);
		}
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x0009CAD5 File Offset: 0x0009ACD5
	[Remote(SendType.ReliableNoDelay)]
	public void SetInvisibleRPC(string hider, bool hide, uint players)
	{
		this.SetInvisible(hider, hide, players, false, false);
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x060016C7 RID: 5831 RVA: 0x0009CAE4 File Offset: 0x0009ACE4
	public bool IsObscured
	{
		get
		{
			if (this.OverrideIsObscured)
			{
				this.CachedIsObscured = true;
				return true;
			}
			if (!NetworkSingleton<NetworkUI>.Instance || !Singleton<TeamScript>.Instance)
			{
				return false;
			}
			string text = NetworkSingleton<NetworkUI>.Instance.HotseatBetweenTurns ? "Grey" : Colour.MyColorLabel();
			if (text != "Black")
			{
				uint num = Colour.FlagFromLabel(text) | Singleton<TeamScript>.Instance.TeamFlagsFromPlayerID(NetworkID.ID);
				uint num2 = this.NormalObscuredFlags & ~this.ShowThroughObscuredFlags;
				if (!this.PeekIds.Contains(text))
				{
					num2 |= this.FacedownObscuredFlags;
				}
				if ((num2 != 0U && (num2 & num) == num) || (this.ReverseObscuredFlags != 0U && (this.ReverseObscuredFlags & num) == 0U))
				{
					this.CachedIsObscured = true;
					return true;
				}
			}
			this.CachedIsObscured = false;
			return false;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x060016C8 RID: 5832 RVA: 0x0009CBAB File Offset: 0x0009ADAB
	public bool IsObscuredAndQuestionMark
	{
		get
		{
			return this.ObscuredQuestionMark && this.ObscuredQuestionMark.activeSelf;
		}
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x0009CBC7 File Offset: 0x0009ADC7
	public void ForceObscured(bool isObscured)
	{
		this.OverrideIsObscured = isObscured;
		if (this.CachedIsObscured != isObscured)
		{
			this.UpdateVisiblity(false);
		}
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x0009CBE0 File Offset: 0x0009ADE0
	public bool SetObscured(string hiderLabel, bool hiderIsActive, uint players = 2147483647U, bool doRPC = false)
	{
		bool isHidden = this.IsHidden;
		bool flag;
		if (hiderIsActive)
		{
			uint num;
			flag = (!this.ObscuredHiders.TryGetValue(hiderLabel, out num) || num != players);
			if (flag)
			{
				this.ObscuredHiders[hiderLabel] = players;
			}
		}
		else
		{
			flag = this.ObscuredHiders.Remove(hiderLabel);
		}
		if (flag)
		{
			this.FacedownObscuredFlags = NetworkPhysicsObject.RecalculateHiderFlags(this.ObscuredHiders, out this.NormalObscuredFlags, out this.ReverseObscuredFlags);
			this.UpdateVisiblity(false);
		}
		if (doRPC)
		{
			base.networkView.RPC<string, bool, uint>(RPCTarget.Others, new Action<string, bool, uint>(this.SetObscuredRPC), hiderLabel, hiderIsActive, players);
		}
		if (isHidden != this.IsHidden)
		{
			EventManager.TriggerNetworkObjectHide(this, this.IsHidden);
		}
		return flag;
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x0009CC8E File Offset: 0x0009AE8E
	[Remote(SendType.ReliableNoDelay)]
	public void SetObscuredRPC(string hider, bool hide, uint players)
	{
		this.SetObscured(hider, hide, players, false);
	}

	// Token: 0x060016CC RID: 5836 RVA: 0x0009CC9C File Offset: 0x0009AE9C
	[Remote(Permission.Server)]
	public void SetHandObscuredRPC(HandZone playerHand)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<HandZone>(RPCTarget.Others, new Action<HandZone>(this.SetHandObscuredRPC), playerHand);
		}
		if (this.CurrentPlayerHand != playerHand)
		{
			playerHand.ResetHandObject(this);
			this.CurrentPlayerHand = playerHand;
		}
		this.SetObscured(playerHand.HiderID, true, Colour.FlagFromLabel(playerHand.TriggerLabel) | 2147483648U, false);
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x0009CD08 File Offset: 0x0009AF08
	public bool SetShowThroughObscured(uint players)
	{
		bool flag = this.ShowThroughObscuredFlags != players;
		if (!flag)
		{
			return false;
		}
		this.ShowThroughObscuredFlags = players;
		base.networkView.RPC<uint>(RPCTarget.Others, new Action<uint>(this.SetShowThroughObscuredRPC), players);
		return flag;
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x0009CD48 File Offset: 0x0009AF48
	public bool ClearShowThroughObscured()
	{
		if (this.ShowThroughObscuredFlags == 0U)
		{
			return false;
		}
		this.SetShowThroughObscured(0U);
		return true;
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x0009CD5D File Offset: 0x0009AF5D
	[Remote(SendType.ReliableNoDelay)]
	private void SetShowThroughObscuredRPC(uint players)
	{
		this.ShowThroughObscuredFlags = players;
		this.UpdateVisiblity(false);
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x0009CD6D File Offset: 0x0009AF6D
	public void UpdateNPOHiders(NetworkPlayer player)
	{
		if ((int)player.id != NetworkID.ID)
		{
			base.networkView.RPC<Dictionary<string, uint>, Dictionary<string, uint>>(player, new Action<Dictionary<string, uint>, Dictionary<string, uint>>(this.SetNPOHiders), this.InvisibleHiders, this.ObscuredHiders);
		}
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x0009CDA4 File Offset: 0x0009AFA4
	[Remote(Permission.Server)]
	public void SetNPOHiders(Dictionary<string, uint> invisibleHiders, Dictionary<string, uint> obscuredHiders)
	{
		foreach (KeyValuePair<string, uint> keyValuePair in invisibleHiders)
		{
			this.SetInvisible(keyValuePair.Key, true, keyValuePair.Value, false, false);
		}
		foreach (KeyValuePair<string, uint> keyValuePair2 in obscuredHiders)
		{
			this.SetObscured(keyValuePair2.Key, true, keyValuePair2.Value, false);
		}
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x0009CE50 File Offset: 0x0009B050
	public void ResetHiders()
	{
		this.InvisibleHiders.Clear();
		this.NormalInvisibleFlags = (this.ReverseInvisibleFlags = 0U);
		this.ObscuredHiders.Clear();
		this.NormalObscuredFlags = (this.ReverseObscuredFlags = 0U);
		this.UpdateVisiblity(false);
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x0009CE9C File Offset: 0x0009B09C
	public void UpdateVisiblity(bool forceRefresh = false)
	{
		bool isHidden = this.IsHidden;
		bool isInvisible = this.IsInvisible;
		bool flag = this.IsObscured && !isInvisible;
		bool flag2 = !isHidden;
		bool flag3 = this.hideObject && flag;
		for (int i = 0; i < this.Renderers.Count; i++)
		{
			if (this.Renderers[i] != null)
			{
				this.Renderers[i].enabled = (flag2 || (flag3 && i == 0));
			}
		}
		for (int j = 0; j < this.UIPanels.Count; j++)
		{
			if (this.UIPanels[j] != null)
			{
				this.UIPanels[j].enabled = !isHidden;
			}
		}
		for (int k = 0; k < this.ShownWhenHidden.Count; k++)
		{
			if (this.ShownWhenHidden[k] != null)
			{
				this.ShownWhenHidden[k].SetActive(!isHidden);
			}
		}
		if (this.stackObject && this.stackObject.IsInfiniteStack && this.stackObject.InfiniteObject)
		{
			this.stackObject.InfiniteObject.SetActive(!isHidden);
		}
		if (this.hideObject)
		{
			this.hideObject.Hide(flag, forceRefresh);
			return;
		}
		if (flag)
		{
			if (this.ObscuredQuestionMark == null)
			{
				this.ObscuredQuestionMark = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Question_Mark"));
			}
			Transform transform = this.ObscuredQuestionMark.transform;
			transform.parent = null;
			transform.localScale = Vector3.one;
			transform.parent = this.transform;
			transform.localPosition = -this.GetBoundsCenterOffset();
			transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
			if (this.highlighter)
			{
				this.highlighter.SetDirty();
			}
			this.ObscuredQuestionMark.SetActive(true);
			return;
		}
		if (this.ObscuredQuestionMark)
		{
			this.ObscuredQuestionMark.SetActive(false);
		}
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x0009D0D2 File Offset: 0x0009B2D2
	public bool MakeInvisibleToSpectator()
	{
		return !this.CachedIsInvisible && this.InvisibleHiders.Count > 0;
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x0009D0EC File Offset: 0x0009B2EC
	public bool MakeObscuredToSpectator()
	{
		return !this.CachedIsObscured && !this.CachedIsInvisible && this.ObscuredHiders.Count > 0;
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x060016D6 RID: 5846 RVA: 0x0009D110 File Offset: 0x0009B310
	public bool CanSeeName
	{
		get
		{
			if (base.CompareTag("Deck"))
			{
				bool flag = false;
				if (this.IsObscured)
				{
					flag = (this.ObscuredHiders.Count > 1 || !this.ObscuredHiders.ContainsKey("facedown"));
				}
				return !this.IsInvisible && !flag;
			}
			if (base.CompareTag("Notecard"))
			{
				return !this.IsHidden && this.transform.up.normalized.y > 0f;
			}
			return !this.IsHidden;
		}
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x0009D1A5 File Offset: 0x0009B3A5
	private IEnumerator RemoveHideAfterDelay(string hider, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (this.HeldCloseInVR == 0)
		{
			this.SetObscured(hider, false, 2147483647U, false);
		}
		yield break;
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x0009D1C4 File Offset: 0x0009B3C4
	public void LuaHighlightOn(Color color)
	{
		if (color != this.LuaHighlightColor)
		{
			base.networkView.RPC<Color32>(RPCTarget.All, new Action<Color32>(this.RPCLuaHighlightColor), color);
		}
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x0009D214 File Offset: 0x0009B414
	public void LuaHighlightOn(Color color, float Duration)
	{
		if (color != this.LuaHighlightColor)
		{
			this.LuaHighlightOn(color);
			base.Invoke("LuaHightlightDurationEnd", Duration);
			return;
		}
		base.CancelInvoke("LuaHightlightDurationEnd");
		base.Invoke("LuaHightlightDurationEnd", Duration);
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x0009D270 File Offset: 0x0009B470
	private void LuaHightlightDurationEnd()
	{
		if (this.LuaHighlightColor != null)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearLuaHighlightColor));
		}
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x0009D270 File Offset: 0x0009B470
	public void LuaHighlightOff()
	{
		if (this.LuaHighlightColor != null)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearLuaHighlightColor));
		}
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x0009D298 File Offset: 0x0009B498
	public void HighlightOn(Color color)
	{
		if (color != this.OverrideHighlightColor)
		{
			base.networkView.RPC<Color32>(RPCTarget.All, new Action<Color32>(this.RPCOverrideHighlightColor), color);
		}
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x0009D2E7 File Offset: 0x0009B4E7
	public void HighlightNotify(Color color)
	{
		this.HighlightOn(color, 0.5f, 0);
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x0009D2F8 File Offset: 0x0009B4F8
	public void HighlightOn(Color color, float Duration, int pulses = 0)
	{
		if (color != this.TemporaryHighlightColor)
		{
			base.networkView.RPC<Color32, int, float>(RPCTarget.All, new Action<Color32, int, float>(this.RPCTemporaryHighlightColor), color, pulses, Duration);
			base.Invoke("HightlightDurationEnd", Duration);
			return;
		}
		base.CancelInvoke("HightlightDurationEnd");
		base.Invoke("HightlightDurationEnd", Duration);
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x0009D36D File Offset: 0x0009B56D
	private void HightlightDurationEnd()
	{
		if (this.TemporaryHighlightColor != null)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearTemporaryHighlightColor));
		}
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x0009D394 File Offset: 0x0009B594
	public void HighlightOff()
	{
		if (this.OverrideHighlightColor != null)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearOverrideHighlightColor));
		}
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x0009D3BB File Offset: 0x0009B5BB
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCLuaHighlightColor(Color32 color)
	{
		this.LuaHighlightColor = new Color?(color);
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x0009D3CE File Offset: 0x0009B5CE
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCClearLuaHighlightColor()
	{
		this.LuaHighlightColor = null;
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x0009D3DC File Offset: 0x0009B5DC
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCTemporaryHighlightColor(Color32 color, int pulses, float duration)
	{
		this.TemporaryHighlightColor = new Color?(color);
		if (pulses == 0)
		{
			this.TemporaryHighlightColorPulse = 0f;
			return;
		}
		this.TemporaryHighlightColorPulse = duration / (float)pulses;
		this.TemporaryHighlightColorStart = Time.time;
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x0009D413 File Offset: 0x0009B613
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCClearTemporaryHighlightColor()
	{
		this.TemporaryHighlightColor = null;
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x0009D421 File Offset: 0x0009B621
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCOverrideHighlightColor(Color32 color)
	{
		this.OverrideHighlightColor = new Color?(color);
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x0009D434 File Offset: 0x0009B634
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCClearOverrideHighlightColor()
	{
		this.OverrideHighlightColor = null;
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x0009D442 File Offset: 0x0009B642
	public void EnableParticleHighlights(bool enabled)
	{
		if (!enabled && this.ParticleHighlights.Count > 0)
		{
			this.RemoveAllParticleHighlights();
		}
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x0009D45C File Offset: 0x0009B65C
	public void UpdateParticleHighlights()
	{
		if (this.highlighter && this.highlighter.IsOn())
		{
			if (this.ParticleHighlights.Count < NetworkPhysicsObject.NumParticleHighlights)
			{
				for (int i = this.ParticleHighlights.Count; i < NetworkPhysicsObject.NumParticleHighlights; i++)
				{
					ParticleHighlight component = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<ParticleHighlightManager>.Instance.Prefab, this.transform).GetComponent<ParticleHighlight>();
					component.SetTarget(this.transform, this.cardScript || this.deckScript);
					this.ParticleHighlights.Add(component);
				}
			}
			for (int j = 0; j < this.ParticleHighlights.Count; j++)
			{
				this.ParticleHighlights[j].SetColor(this.highlighter.GetColor());
			}
			return;
		}
		if (this.ParticleHighlights.Count > 0)
		{
			this.RemoveAllParticleHighlights();
		}
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x0009D550 File Offset: 0x0009B750
	private void RemoveAllParticleHighlights()
	{
		for (int i = this.ParticleHighlights.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.ParticleHighlights[i].gameObject);
		}
		this.ParticleHighlights.Clear();
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x0009D598 File Offset: 0x0009B798
	private static Colour DarkColourFromPlayerID(int id)
	{
		Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(id);
		if (!pointer)
		{
			return Colour.Black;
		}
		return pointer.PointerDarkColour;
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x0009D5C8 File Offset: 0x0009B7C8
	public void AddPeekIndicator()
	{
		if (!PermissionsOptions.options.Peeking && !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			return;
		}
		string color = PlayerScript.Pointer ? PlayerScript.PointerScript.PointerColorLabel : null;
		this.AddPeekIndicator(color);
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x0009D610 File Offset: 0x0009B810
	public void AddPeekIndicator(string color)
	{
		if (color == null)
		{
			return;
		}
		if (this.RPCAddPeekIndicator(color))
		{
			base.networkView.RPC<string, bool>(RPCTarget.Others, new Func<string, bool>(this.RPCAddPeekIndicator), color);
		}
		this.RemovePeekIndicator(color);
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x0009D640 File Offset: 0x0009B840
	[Remote(SendType.ReliableNoDelay)]
	private bool RPCAddPeekIndicator(string color)
	{
		if (this.PeekIds.Contains(color))
		{
			return false;
		}
		this.PeekIds.Add(color);
		this.UpdateVisiblity(false);
		Singleton<UIObjectIndicatorManager>.Instance.AddIndicator(this, UIObjectIndicatorManager.IndicatorType.Peek, color, "");
		Chat.LogSystem(string.Concat(new string[]
		{
			Colour.ColourFromLabel(color).RGBHex,
			color,
			"[-] peeked at ",
			base.tag,
			Network.isAdmin ? ("〔" + this.GUID + "〕") : ""
		}), Colour.Grey, false);
		EventManager.TriggerObjectPeek(this, color);
		return true;
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x0009D6F1 File Offset: 0x0009B8F1
	private void StopRemovePeekCoroutine()
	{
		if (this.removePeekCoroutine != null)
		{
			base.StopCoroutine(this.removePeekCoroutine);
			this.removePeekCoroutine = null;
		}
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x0009D70E File Offset: 0x0009B90E
	private void RemovePeekIndicator(string color)
	{
		this.StopRemovePeekCoroutine();
		this.removePeekCoroutine = base.StartCoroutine(this.DelayRemovePeek(color));
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x0009D729 File Offset: 0x0009B929
	private IEnumerator DelayRemovePeek(string color)
	{
		yield return this.peekWait;
		if (this.RPCRemovePeekIndicator(color))
		{
			base.networkView.RPC<string, bool>(RPCTarget.Others, new Func<string, bool>(this.RPCRemovePeekIndicator), color);
		}
		yield break;
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x0009D73F File Offset: 0x0009B93F
	[Remote(SendType.ReliableNoDelay)]
	public bool RPCRemovePeekIndicator(string color)
	{
		if (!this.PeekIds.Contains(color))
		{
			return false;
		}
		this.PeekIds.Remove(color);
		this.UpdateVisiblity(false);
		Singleton<UIObjectIndicatorManager>.Instance.RemoveIndicator(this, UIObjectIndicatorManager.IndicatorType.Peek, color);
		return true;
	}

	// Token: 0x060016F2 RID: 5874 RVA: 0x0009D773 File Offset: 0x0009B973
	public void AddObjectEnteredContainerIndicator(string color)
	{
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.RPCAddObjectEnteredContainerIndicator), color);
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x0009D78E File Offset: 0x0009B98E
	[Remote(SendType.ReliableNoDelay)]
	private void RPCAddObjectEnteredContainerIndicator(string color)
	{
		Singleton<UIObjectIndicatorManager>.Instance.AddIndicator(this, UIObjectIndicatorManager.IndicatorType.ObjectEnteredContainer, color, "");
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x060016F4 RID: 5876 RVA: 0x0009D7A2 File Offset: 0x0009B9A2
	// (set) Token: 0x060016F5 RID: 5877 RVA: 0x0009D7AA File Offset: 0x0009B9AA
	public Joint joint { get; private set; }

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x060016F6 RID: 5878 RVA: 0x0009D7B3 File Offset: 0x0009B9B3
	// (set) Token: 0x060016F7 RID: 5879 RVA: 0x0009D7BB File Offset: 0x0009B9BB
	public FixedJoint fixedJoint { get; private set; }

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x060016F8 RID: 5880 RVA: 0x0009D7C4 File Offset: 0x0009B9C4
	// (set) Token: 0x060016F9 RID: 5881 RVA: 0x0009D7CC File Offset: 0x0009B9CC
	public HingeJoint hingeJoint { get; private set; }

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x060016FA RID: 5882 RVA: 0x0009D7D5 File Offset: 0x0009B9D5
	// (set) Token: 0x060016FB RID: 5883 RVA: 0x0009D7DD File Offset: 0x0009B9DD
	public SpringJoint springJoint { get; private set; }

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x060016FC RID: 5884 RVA: 0x0009D7E8 File Offset: 0x0009B9E8
	public bool IsFloatingFromTeleport
	{
		get
		{
			return !this.IsCollidable && !this.rigidbody.useGravity && this.IsHeldByNobody && this.rigidbody.velocity == Vector3.zero && this.rigidbody.angularVelocity == Vector3.zero && !this.CurrentPlayerHand;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x060016FD RID: 5885 RVA: 0x0009D850 File Offset: 0x0009BA50
	public bool InsideALayoutZone
	{
		get
		{
			return this.LayoutZonesContaining.Count > 0;
		}
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x0009D860 File Offset: 0x0009BA60
	public void SetupGrabbableLayer(bool strictness = true)
	{
		base.GetComponentsInChildren<Transform>(true, NetworkPhysicsObject.childTransforms);
		for (int i = 0; i < NetworkPhysicsObject.childTransforms.Count; i++)
		{
			Transform transform = NetworkPhysicsObject.childTransforms[i];
			string text = LayerMask.LayerToName(transform.gameObject.layer);
			if (!transform.CompareTag("Snap") && !text.Contains("UI") && ((strictness && transform.gameObject.layer == 0) || (!strictness && transform.gameObject.layer != 2)))
			{
				transform.gameObject.layer = 10;
			}
		}
		NetworkPhysicsObject.childTransforms.Clear();
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x060016FF RID: 5887 RVA: 0x0009D8FD File Offset: 0x0009BAFD
	// (set) Token: 0x06001700 RID: 5888 RVA: 0x0009D908 File Offset: 0x0009BB08
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public Vector3 Scale
	{
		get
		{
			return this._Scale;
		}
		private set
		{
			if (base.CompareTag("Card") || base.CompareTag("Deck") || base.CompareTag("Tile"))
			{
				value.y = 1f;
			}
			if (value != this._Scale)
			{
				if (this.recalculateStateScale && this.States != null)
				{
					foreach (KeyValuePair<int, ObjectState> keyValuePair in this.States)
					{
						keyValuePair.Value.Transform.scaleX *= value.x / this.Scale.x;
						keyValuePair.Value.Transform.scaleY *= value.y / this.Scale.y;
						keyValuePair.Value.Transform.scaleZ *= value.z / this.Scale.z;
					}
				}
				this._Scale = value;
				this.transform.localScale = new Vector3(this.BaseScale.x * this.Scale.x, this.BaseScale.y * this.Scale.y, this.BaseScale.z * this.Scale.z);
				if (this.OverrideRigidbody == null)
				{
					this.SetMass((this.ScaleAverage - 1f) / 4f + this.StartMass);
				}
				if (base.CompareTag("Tileset"))
				{
					this.InternalGridSize = 2f * this.ScaleAverage;
				}
				if (this.customBoardScript)
				{
					this.customBoardScript.StartCheck();
				}
				this.ResetBounds();
				base.DirtySync("Scale");
			}
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06001701 RID: 5889 RVA: 0x0009DB04 File Offset: 0x0009BD04
	public float ScaleAverage
	{
		get
		{
			return (this.Scale.x + this.Scale.y + this.Scale.z) / 3f;
		}
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x0009DB2F File Offset: 0x0009BD2F
	private void SpawnFreeze()
	{
		this.isSpawnFreeze = true;
		this.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		this.isKinematic = true;
		Wait.Frames(delegate
		{
			if (this.rigidbody)
			{
				this.rigidbody.constraints = RigidbodyConstraints.None;
				this.isSpawnFreeze = false;
			}
		}, 2);
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x0009DB5F File Offset: 0x0009BD5F
	public bool GetUseGravity()
	{
		return this.OverrideRigidbody == null || this.OverrideRigidbody.UseGravity;
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x0009DB7C File Offset: 0x0009BD7C
	public void SetUseGravity(bool bUseGravity)
	{
		if (this.OverrideRigidbody == null)
		{
			this.OverrideRigidbody = new RigidbodyState
			{
				AngularDrag = this.GetAngularDrag(),
				Drag = this.GetDrag(),
				Mass = this.GetMass(),
				UseGravity = this.GetUseGravity()
			};
		}
		this.OverrideRigidbody.UseGravity = bUseGravity;
		this.ResetRigidbody();
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x0009DBE4 File Offset: 0x0009BDE4
	public float GetMass()
	{
		if (this.OverrideRigidbody == null)
		{
			return this.mass;
		}
		return this.OverrideRigidbody.Mass;
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x0009DC06 File Offset: 0x0009BE06
	public float GetDrag()
	{
		if (!(this.OverrideRigidbody == null))
		{
			return this.OverrideRigidbody.Drag;
		}
		if (!this.sphereCollider)
		{
			return 0.1f;
		}
		return 0.2f;
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x0009DC3C File Offset: 0x0009BE3C
	public void SetDrag(float DragValue)
	{
		if (this.OverrideRigidbody == null)
		{
			this.OverrideRigidbody = new RigidbodyState
			{
				AngularDrag = this.GetAngularDrag(),
				Drag = this.GetDrag(),
				Mass = this.GetMass(),
				UseGravity = this.GetUseGravity()
			};
		}
		this.OverrideRigidbody.Drag = DragValue;
		this.ResetRigidbody();
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x0009DCA4 File Offset: 0x0009BEA4
	public float GetAngularDrag()
	{
		if (!(this.OverrideRigidbody == null))
		{
			return this.OverrideRigidbody.AngularDrag;
		}
		if (!this.sphereCollider)
		{
			return 0.1f;
		}
		return 0.2f;
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x0009DCD8 File Offset: 0x0009BED8
	public void SetAngularDrag(float AngularDragValue)
	{
		if (this.OverrideRigidbody == null)
		{
			this.OverrideRigidbody = new RigidbodyState
			{
				AngularDrag = this.GetAngularDrag(),
				Drag = this.GetDrag(),
				Mass = this.GetMass(),
				UseGravity = this.GetUseGravity()
			};
		}
		this.OverrideRigidbody.AngularDrag = AngularDragValue;
		this.ResetRigidbody();
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x0009DD40 File Offset: 0x0009BF40
	public Bounds GetBounds()
	{
		this.CheckResetBounds();
		return this.CombinedBounds;
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x0009DD50 File Offset: 0x0009BF50
	private void CheckResetBounds()
	{
		if (this.CombinedBounds == default(Bounds))
		{
			this.ResetBounds();
		}
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x0009DD79 File Offset: 0x0009BF79
	public Bounds GetRendererBounds()
	{
		this.CheckResetBounds();
		return this.CombinedRendererBounds;
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x0009DD88 File Offset: 0x0009BF88
	public Bounds GetBoundsNotNormalized()
	{
		Vector3 vector;
		return this.GetBoundsNotNormalized(out vector);
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x0009DDA0 File Offset: 0x0009BFA0
	public Bounds GetBoundsNotNormalized(out Vector3 BoundsCenterOffsetNotNormalized)
	{
		Bounds bounds = default(Bounds);
		for (int i = 0; i < this.Colliders.Count; i++)
		{
			Collider collider = this.Colliders[i];
			if (collider && collider.enabled && !this.TriggerOnlyColliders.Contains(collider))
			{
				if (bounds == default(Bounds))
				{
					bounds = collider.bounds;
				}
				else
				{
					bounds.Encapsulate(collider.bounds);
				}
			}
		}
		if (bounds == default(Bounds))
		{
			bounds = new Bounds(this.rigidbody.position, Vector3.zero);
		}
		Vector3 position = this.rigidbody.position;
		BoundsCenterOffsetNotNormalized = new Vector3(position.x - bounds.center.x, position.y - bounds.center.y, position.z - bounds.center.z);
		return bounds;
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x0009DE98 File Offset: 0x0009C098
	public Vector3 GetBoundsCenterOffset()
	{
		this.CheckResetBounds();
		return this.BoundsCenterOffset;
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x0009DEA6 File Offset: 0x0009C0A6
	public Vector3 GetRendererBoundsCenterOffset()
	{
		this.CheckResetBounds();
		return this.RendererBoundsCenterOffset;
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x0009DEB4 File Offset: 0x0009C0B4
	public float GetBoundsSphereRadius()
	{
		float result = 1f;
		Bounds bounds = this.GetBounds();
		if (bounds != default(Bounds))
		{
			if (bounds.extents.x >= bounds.extents.z)
			{
				result = bounds.extents.x;
			}
			else if (bounds.extents.z > bounds.extents.x)
			{
				result = bounds.extents.z;
			}
		}
		return result;
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06001712 RID: 5906 RVA: 0x0009DF30 File Offset: 0x0009C130
	public bool IsCollidable
	{
		get
		{
			return this.bColliding;
		}
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x0009DF38 File Offset: 0x0009C138
	public void SetObject(bool bAltSounds, int MeshInt = -1, int MatInt = -1)
	{
		this.UseAltSounds = bAltSounds;
		if (Network.isClient)
		{
			return;
		}
		if (this.meshSyncScript && MeshInt != -1)
		{
			this.meshSyncScript.SetMesh(MeshInt);
		}
		if (this.materialSyncScript && MatInt != -1)
		{
			this.materialSyncScript.SetMaterial(MatInt);
		}
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x0009DF90 File Offset: 0x0009C190
	public void SetMass(float mass)
	{
		this.rigidbody.mass = mass;
		if (this.OverrideRigidbody == null)
		{
			this.mass = mass;
		}
		else
		{
			this.OverrideRigidbody.Mass = mass;
		}
		if (base.GetComponent<LowerCenterOfMass>())
		{
			base.GetComponent<LowerCenterOfMass>().CalculateCenterOfMass();
		}
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x0009DFE4 File Offset: 0x0009C1E4
	public void SetCollision(bool bCollide)
	{
		if (bCollide != this.bColliding)
		{
			this.bColliding = bCollide;
			base.GetComponentsInChildren<Collider>(this.Colliders);
			for (int i = 0; i < this.Colliders.Count; i++)
			{
				Collider collider = this.Colliders[i];
				MeshCollider meshCollider;
				if ((!collider || collider.enabled) && !this.TriggerOnlyColliders.Contains(collider) && (bCollide || (meshCollider = (collider as MeshCollider)) == null || meshCollider.convex))
				{
					collider.isTrigger = !this.bColliding;
				}
			}
		}
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x0009E072 File Offset: 0x0009C272
	public float GetStaticFriction()
	{
		this.CheckOverridePhysicsMaterial();
		return this.OverridePhysicsMaterial.StaticFriction;
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x0009E085 File Offset: 0x0009C285
	public void SetStaticFriction(float StaticFriction)
	{
		this.CheckOverridePhysicsMaterial();
		this.OverridePhysicsMaterial.StaticFriction = Mathf.Clamp01(StaticFriction);
		this.ResetPhysicsMaterial();
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x0009E0A4 File Offset: 0x0009C2A4
	public float GetDynamicFriction()
	{
		this.CheckOverridePhysicsMaterial();
		return this.OverridePhysicsMaterial.DynamicFriction;
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x0009E0B7 File Offset: 0x0009C2B7
	public void SetDynamicFriction(float DynamicFriction)
	{
		this.CheckOverridePhysicsMaterial();
		this.OverridePhysicsMaterial.DynamicFriction = Mathf.Clamp01(DynamicFriction);
		this.ResetPhysicsMaterial();
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x0009E0D6 File Offset: 0x0009C2D6
	public float GetBounciness()
	{
		this.CheckOverridePhysicsMaterial();
		return this.OverridePhysicsMaterial.Bounciness;
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x0009E0E9 File Offset: 0x0009C2E9
	public void SetBounciness(float Bounciness)
	{
		this.CheckOverridePhysicsMaterial();
		this.OverridePhysicsMaterial.Bounciness = Mathf.Clamp01(Bounciness);
		this.ResetPhysicsMaterial();
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x0009E108 File Offset: 0x0009C308
	public void CheckOverridePhysicsMaterial()
	{
		if (this.OverridePhysicsMaterial == null)
		{
			this.OverridePhysicsMaterial = new PhysicsMaterialState();
			base.GetComponentsInChildren<Collider>(this.Colliders);
			for (int i = 0; i < this.Colliders.Count; i++)
			{
				Collider collider = this.Colliders[i];
				if (collider.enabled && !this.TriggerOnlyColliders.Contains(collider))
				{
					PhysicMaterial material = collider.material;
					this.OverridePhysicsMaterial.DynamicFriction = material.dynamicFriction;
					this.OverridePhysicsMaterial.StaticFriction = material.staticFriction;
					this.OverridePhysicsMaterial.Bounciness = material.bounciness;
					this.OverridePhysicsMaterial.FrictionCombine = material.frictionCombine;
					this.OverridePhysicsMaterial.BounceCombine = material.bounceCombine;
					return;
				}
			}
		}
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x0009E1D9 File Offset: 0x0009C3D9
	public void AddJoint(Joint joint_)
	{
		this.joint = joint_;
		this.fixedJoint = (this.joint as FixedJoint);
		this.hingeJoint = (this.joint as HingeJoint);
		this.springJoint = (this.joint as SpringJoint);
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x0009E215 File Offset: 0x0009C415
	public void ResetObject()
	{
		this.HeldByPlayerID = -1;
		this.HeldByTouchID = -1;
		this.HeldByControllerPickupRotation = Quaternion.identity;
		this.HeldByControllerPickupOffset = Vector3.zero;
		this.ResetRigidbody();
		this.FixConcave();
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x0009E248 File Offset: 0x0009C448
	public void ResetRigidbody()
	{
		this.rigidbody.useGravity = this.GetUseGravity();
		this.rigidbody.drag = this.GetDrag();
		this.rigidbody.angularDrag = this.GetAngularDrag();
		this.rigidbody.mass = this.GetMass();
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x0009E29C File Offset: 0x0009C49C
	public void ResetPhysicsMaterial()
	{
		if (this.OverridePhysicsMaterial != null)
		{
			base.GetComponentsInChildren<Collider>(this.Colliders);
			for (int i = 0; i < this.Colliders.Count; i++)
			{
				Collider collider = this.Colliders[i];
				if (collider.enabled && !this.TriggerOnlyColliders.Contains(collider))
				{
					collider.material = new PhysicMaterial
					{
						dynamicFriction = this.OverridePhysicsMaterial.DynamicFriction,
						staticFriction = this.OverridePhysicsMaterial.StaticFriction,
						bounciness = this.OverridePhysicsMaterial.Bounciness,
						frictionCombine = this.OverridePhysicsMaterial.FrictionCombine,
						bounceCombine = this.OverridePhysicsMaterial.BounceCombine
					};
				}
			}
		}
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x0009E368 File Offset: 0x0009C568
	public void ResetBounds()
	{
		Quaternion rotation = this.transform.rotation;
		this.transform.rotation = Quaternion.identity;
		base.GetComponentsInChildren<Collider>(this.Colliders);
		this.CombinedBounds = new Bounds(this.rigidbody.position, Vector3.zero);
		for (int i = 0; i < this.Colliders.Count; i++)
		{
			Collider collider = this.Colliders[i];
			if (collider.enabled && (!this.TriggerOnlyColliders.Contains(collider) || this.zone))
			{
				this.CombinedBounds.Encapsulate(collider.bounds);
				MeshCollider meshCollider = collider as MeshCollider;
				if (meshCollider != null && !this.ConvexColliders.Contains(meshCollider) && !this.ConcaveColliders.Contains(meshCollider))
				{
					if (meshCollider.convex)
					{
						this.ConvexColliders.Add(meshCollider);
					}
					else
					{
						this.ConcaveColliders.Add(meshCollider);
					}
				}
				if (this.IsSmoothMoving && !this.bColliding)
				{
					if (meshCollider != null && !meshCollider.convex)
					{
						meshCollider.convex = true;
					}
					collider.isTrigger = !this.bColliding;
				}
			}
		}
		this.CombinedRendererBounds = default(Bounds);
		foreach (Renderer renderer in this.Renderers)
		{
			if (renderer)
			{
				if (this.CombinedRendererBounds == default(Bounds))
				{
					this.CombinedRendererBounds = renderer.bounds;
				}
				else
				{
					this.CombinedRendererBounds.Encapsulate(renderer.bounds);
				}
			}
		}
		this.transform.rotation = rotation;
		Vector3 position = this.transform.position;
		this.BoundsCenterOffset = position - this.CombinedBounds.center;
		this.RendererBoundsCenterOffset = position - this.CombinedRendererBounds.center;
		this.FixConcave();
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x0009E584 File Offset: 0x0009C784
	public void ResetIdleFreeze()
	{
		this.SleepTimeHolder = 0f;
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x0009E594 File Offset: 0x0009C794
	public void ResetIdleFreezeAroundObject()
	{
		if (ServerOptions.isPhysicsSemi || ServerOptions.isPhysicsLock)
		{
			float boundsSphereRadius = this.GetBoundsSphereRadius();
			Vector3 position = this.rigidbody.position;
			ValueTuple<RaycastHit[], int> valueTuple = PhysicsNonAlloc.SphereCast(new Vector3(position.x, position.y + 50f, position.z), boundsSphereRadius, Vector3.down, 100f, HoverScript.NonHeldLayerMask, false);
			RaycastHit[] item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			for (int i = 0; i < item2; i++)
			{
				NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(item[i].collider);
				if (networkPhysicsObject)
				{
					networkPhysicsObject.ResetIdleFreeze();
				}
			}
		}
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x0009E640 File Offset: 0x0009C840
	[return: TupleElementNames(new string[]
	{
		"hits",
		"count"
	})]
	public ValueTuple<RaycastHit[], int> GetOverlapHits()
	{
		Bounds bounds = this.GetBounds();
		Bounds boundsNotNormalized = this.GetBoundsNotNormalized();
		return PhysicsNonAlloc.BoxCast(new Vector3(boundsNotNormalized.center.x, boundsNotNormalized.center.y + 50f, boundsNotNormalized.center.z), bounds.extents, Vector3.down, this.rigidbody.rotation, 100f, HoverScript.NonHeldLayerMask, false);
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x0009E6B4 File Offset: 0x0009C8B4
	public List<NetworkPhysicsObject> GetOverlapObjects()
	{
		List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
		ValueTuple<RaycastHit[], int> overlapHits = this.GetOverlapHits();
		RaycastHit[] item = overlapHits.Item1;
		int item2 = overlapHits.Item2;
		for (int i = 0; i < item2; i++)
		{
			RaycastHit raycastHit = item[i];
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(raycastHit.collider);
			if (networkPhysicsObject && networkPhysicsObject != this && !networkPhysicsObject.IsLocked && networkPhysicsObject.IsGrabbable)
			{
				list.Add(networkPhysicsObject);
			}
		}
		return list;
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x0009E730 File Offset: 0x0009C930
	public List<NetworkPhysicsObject> GetStickyObjects()
	{
		List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
		if (!this.IsSticky)
		{
			return list;
		}
		Bounds bounds = this.GetBounds();
		float num = bounds.size.x;
		if (num < bounds.size.y)
		{
			num = bounds.size.y;
		}
		if (num < bounds.size.z)
		{
			num = bounds.size.z;
		}
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		for (int i = 0; i < grabbableNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
			if (!networkPhysicsObject.IsHeldBySomebody && !networkPhysicsObject.HeldIDIndicatesDestruction && !networkPhysicsObject.IsLocked && networkPhysicsObject.IsGrabbable && networkPhysicsObject.bColliding && ((!base.gameObject.CompareTag("Card") && !base.gameObject.CompareTag("Deck")) || (!networkPhysicsObject.CompareTag("Card") && !networkPhysicsObject.CompareTag("Deck"))) && (Mathf.Abs(networkPhysicsObject.rigidbody.position.x - this.rigidbody.position.x) <= num * 2f || Mathf.Abs(networkPhysicsObject.rigidbody.position.z - this.rigidbody.position.z) <= num * 2f))
			{
				Bounds boundsNotNormalized = networkPhysicsObject.GetBoundsNotNormalized();
				float maxDistance = boundsNotNormalized.extents.y + 0.5f;
				RaycastHit raycastHit;
				if (Physics.Raycast(boundsNotNormalized.center, Vector3.down, out raycastHit, maxDistance, HoverScript.HeldLayerMask) && NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider) == base.gameObject)
				{
					list.Add(networkPhysicsObject);
				}
			}
		}
		return list;
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x0009E90C File Offset: 0x0009CB0C
	public Vector3[] GetRaycastPoints(float RaycastSpacer = 0.25f)
	{
		Bounds bounds = this.GetBounds();
		int num = 0;
		float x = bounds.size.x;
		float z = bounds.size.z;
		float num2 = (float)((int)(x / RaycastSpacer));
		int num3 = (int)(z / RaycastSpacer);
		float num4 = num2 * RaycastSpacer / 2f;
		float num5 = (float)num3 * RaycastSpacer / 2f;
		float num6 = (num != 0) ? (RaycastSpacer / 2f) : 0f;
		float num7 = (num != 0) ? (RaycastSpacer / 2f) : 0f;
		int num8 = 0;
		for (float num9 = num6; num9 < x; num9 += RaycastSpacer)
		{
			for (float num10 = num7; num10 < z; num10 += RaycastSpacer)
			{
				num8++;
			}
		}
		Vector3[] array = new Vector3[num8];
		int num11 = 0;
		bool flag = false;
		float num12 = num6;
		while (num12 < x && !flag)
		{
			for (float num13 = num7; num13 < z; num13 += RaycastSpacer)
			{
				Vector3 vector = new Vector3(num12 - num4, 0f, num13 - num5);
				vector = Utilities.RotatePointAroundPivot(vector, Vector3.zero, this.rigidbody.rotation);
				vector = new Vector3(vector.x + this.rigidbody.position.x, this.rigidbody.position.y, vector.z + this.rigidbody.position.z);
				array[num11] = vector;
				num11++;
				if (num11 >= array.Length - 1)
				{
					flag = true;
					break;
				}
			}
			num12 += RaycastSpacer;
		}
		array[num11] = Vector3.zero;
		return array;
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x06001728 RID: 5928 RVA: 0x0009EA95 File Offset: 0x0009CC95
	// (set) Token: 0x06001729 RID: 5929 RVA: 0x0009EAA2 File Offset: 0x0009CCA2
	public bool isKinematic
	{
		get
		{
			return this.rigidbody.isKinematic;
		}
		set
		{
			this.rigidbody.isKinematic = value;
		}
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x0009EAB0 File Offset: 0x0009CCB0
	public void FixConcave()
	{
		for (int i = 0; i < this.ConcaveColliders.Count; i++)
		{
			MeshCollider meshCollider = this.ConcaveColliders[i];
			if (meshCollider)
			{
				meshCollider.convex = !this.IsLocked;
			}
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x0600172B RID: 5931 RVA: 0x0009EAF8 File Offset: 0x0009CCF8
	public bool IsMoving
	{
		get
		{
			return this.rigidbody.velocity.magnitude > 0.15f || this.rigidbody.angularVelocity.magnitude > 0.14f;
		}
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x0009EB3B File Offset: 0x0009CD3B
	public bool isSleeping()
	{
		return this.sleeping && !this.justMoved;
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x0009EB50 File Offset: 0x0009CD50
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetPositionHeld(Vector3 position)
	{
		if ((int)Network.sender.id != this.HeldByPlayerID)
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3>(RPCTarget.Server, new Action<Vector3>(this.SetPositionHeld), position);
		}
		this.transform.position = position;
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x0009EBA0 File Offset: 0x0009CDA0
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void SetRotationHeld(Vector3 rotation)
	{
		if ((int)Network.sender.id != this.HeldByPlayerID)
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3>(RPCTarget.Server, new Action<Vector3>(this.SetRotationHeld), rotation);
		}
		this.transform.eulerAngles = rotation;
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x0009EBEF File Offset: 0x0009CDEF
	[Remote(Permission.Admin)]
	public void SetPosition(Vector3 position)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3>(RPCTarget.Server, new Action<Vector3>(this.SetPosition), position);
		}
		this.transform.position = position;
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x0009EC1D File Offset: 0x0009CE1D
	[Remote(Permission.Admin)]
	public void SetRotation(Vector3 rotation)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3>(RPCTarget.Server, new Action<Vector3>(this.SetRotation), rotation);
		}
		this.transform.eulerAngles = rotation;
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x0009EC4B File Offset: 0x0009CE4B
	[Remote(Permission.Admin)]
	public void SetScale(Vector3 scale, bool RecalculateStateScale = false)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, bool>(RPCTarget.Server, new Action<Vector3, bool>(this.SetScale), scale, RecalculateStateScale);
		}
		this.recalculateStateScale = RecalculateStateScale;
		this.Scale = scale;
		this.recalculateStateScale = false;
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x0009EC84 File Offset: 0x0009CE84
	public void ResetCardJoint()
	{
		if (this.cardScript)
		{
			if (this.fixedJoint)
			{
				this.cardScript.ResetCard();
			}
			if (this.cardScript.CardAttachToThis)
			{
				this.cardScript.CardAttachToThis.GetComponent<CardScript>().ResetCard();
			}
		}
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x0009ECDD File Offset: 0x0009CEDD
	public void AddRenderer(Renderer render)
	{
		if (this.Renderers.TryAddUnique(render))
		{
			this.UpdateVisiblity(false);
		}
	}

	// Token: 0x06001734 RID: 5940 RVA: 0x0009ECF4 File Offset: 0x0009CEF4
	public void TryRemoveRenderer(Renderer render)
	{
		this.Renderers.Remove(render);
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06001735 RID: 5941 RVA: 0x0009ED04 File Offset: 0x0009CF04
	// (set) Token: 0x06001736 RID: 5942 RVA: 0x0009ED78 File Offset: 0x0009CF78
	[Sync(true)]
	public Color DiffuseColor
	{
		get
		{
			if (this.Renderers.Count > 0 && this.Renderers[0].sharedMaterial && this.Renderers[0].sharedMaterial.HasProperty("_Color"))
			{
				return this.Renderers[0].sharedMaterial.color;
			}
			return Colour.UnityWhite;
		}
		set
		{
			if (this.zone)
			{
				return;
			}
			if (this.Renderers.Count == 0 || this.Renderers[0].sharedMaterial == null || !this.Renderers[0].sharedMaterial.HasProperty("_Color") || this.Renderers[0].sharedMaterial.color == value)
			{
				return;
			}
			this.Renderers[0].material.color = value;
			TextureScript.UpdateMaterialTransparency(this.Renderers[0].material);
			if (this.highlighter)
			{
				this.highlighter.SetDirty();
			}
			base.DirtySync("DiffuseColor");
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06001737 RID: 5943 RVA: 0x0009EE45 File Offset: 0x0009D045
	// (set) Token: 0x06001738 RID: 5944 RVA: 0x0009EE4D File Offset: 0x0009D04D
	public FastDrag fastDrag { get; set; }

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06001739 RID: 5945 RVA: 0x0009EE56 File Offset: 0x0009D056
	// (set) Token: 0x0600173A RID: 5946 RVA: 0x0009EE5E File Offset: 0x0009D05E
	public float DisableFastDragUntil { get; private set; }

	// Token: 0x0600173B RID: 5947 RVA: 0x0009EE67 File Offset: 0x0009D067
	public void DisableFastDragWhileAnimating()
	{
		this.DisableFastDragUntil = Time.time + 0.5f;
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x0009EE67 File Offset: 0x0009D067
	public void DisableFastDragWhilePickingUp()
	{
		this.DisableFastDragUntil = Time.time + 0.5f;
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x0009EE7A File Offset: 0x0009D07A
	public void SetRotationValues(List<RotationValue> rotationValues)
	{
		this.RotationValues = rotationValues;
		base.networkView.RPC<List<RotationValue>>(RPCTarget.Others, new Action<List<RotationValue>>(this.RPCSetRotationValues), rotationValues);
	}

	// Token: 0x0600173E RID: 5950 RVA: 0x0009EE9C File Offset: 0x0009D09C
	[Remote(SerializationMethod.Json)]
	private void RPCSetRotationValues(List<RotationValue> rotationValues)
	{
		this.RotationValues = rotationValues;
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x0009EEA5 File Offset: 0x0009D0A5
	public bool HasRotationsValues()
	{
		return this.RotationValues.Count > 0;
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x0009EEB8 File Offset: 0x0009D0B8
	public RotationValue GetRotationValue(bool invert = false)
	{
		Vector3 position = this.transform.position;
		Vector3 position2 = new Vector3(position.x, position.y + (invert ? -1f : 1f), position.z);
		Vector3 normalized = this.transform.InverseTransformPoint(position2).normalized;
		RotationValue result = null;
		float num = float.MinValue;
		for (int i = 0; i < this.RotationValues.Count; i++)
		{
			RotationValue rotationValue = this.RotationValues[i];
			float num2 = Vector3.Dot(normalized, rotationValue.direction);
			if (num2 > num)
			{
				num = num2;
				result = rotationValue;
			}
		}
		return result;
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x0009EF60 File Offset: 0x0009D160
	public void SetRotationValue(string value)
	{
		for (int i = 0; i < this.RotationValues.Count; i++)
		{
			RotationValue rotationValue = this.RotationValues[i];
			if (rotationValue.value == value)
			{
				this.SetSmoothToRotationValue(rotationValue, -1);
				return;
			}
		}
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x0009EFA8 File Offset: 0x0009D1A8
	public int GetRotationIndex(bool invert = false)
	{
		RotationValue rotationValue = this.GetRotationValue(invert);
		if (rotationValue != null)
		{
			return this.RotationValues.IndexOf(rotationValue);
		}
		return -1;
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x0009EFCE File Offset: 0x0009D1CE
	public void SetRotationIndex(int index, int playerId = -1)
	{
		if (index < this.RotationValues.Count && index >= 0)
		{
			this.SetSmoothToRotationValue(this.RotationValues[index], playerId);
		}
	}

	// Token: 0x06001744 RID: 5956 RVA: 0x0009EFF8 File Offset: 0x0009D1F8
	public int GetRotationNumber(bool invert = false)
	{
		int num = this.GetRotationIndex(invert);
		if (num > -1)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x0009F016 File Offset: 0x0009D216
	[Remote(SendType.ReliableNoDelay)]
	public void SetRotationNumber(int number, int playerId)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int>(RPCTarget.Server, new Action<int, int>(this.SetRotationNumber), number, playerId);
			return;
		}
		this.SetRotationIndex(number - 1, playerId);
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x0009F044 File Offset: 0x0009D244
	public void SetupDefaultRotationValues()
	{
		if (this.RotationValues.Count == 0)
		{
			if (base.CompareTag("Dice"))
			{
				RotationValue[] array = DiceScript.DiceToRotations(base.gameObject);
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						this.RotationValues.Add(new RotationValue(array[i].value, array[i].rotation));
					}
					return;
				}
			}
			else if (base.InternalName == "Quarter")
			{
				this.RotationValues.Add(new RotationValue("Heads", new Vector3(0f, 90f, 180f)));
				this.RotationValues.Add(new RotationValue("Tails", new Vector3(0f, -90f, 0f)));
			}
		}
	}

	// Token: 0x06001747 RID: 5959 RVA: 0x0009F114 File Offset: 0x0009D314
	public void SetSmoothDestroy(Vector3 Position)
	{
		this.IsBeingDestroyed = true;
		this.SetCollision(false);
		this.SetSmoothPosition(Position, false, true, true, true, null, false, false, null);
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x0009F148 File Offset: 0x0009D348
	public void SetSmoothDestroy(Vector3 Position, Quaternion Rotation)
	{
		this.IsBeingDestroyed = true;
		this.SetCollision(false);
		this.SetSmoothRotation(Rotation, true, false, false, true, null, false);
		this.SetSmoothPosition(Position, false, false, true, true, null, false, false, null);
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x0009F190 File Offset: 0x0009D390
	public Vector3? GetSmoothPosition()
	{
		if (this.currentSmoothPosition.Moving)
		{
			return new Vector3?(this.currentSmoothPosition.TargetPosition);
		}
		return null;
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x0009F1C4 File Offset: 0x0009D3C4
	public void SetSmoothPosition(Vector3 Position, bool Colliding = true, bool FastSpeed = false, bool DestroyWhenFinish = false, bool ToggleCollidingBackOn = true, Color? highlightColor = null, bool Freeze = false, bool IgnoreHandTriggers = false, NetworkPhysicsObject DontStopBefore = null)
	{
		if (this.currentSmoothPosition.Moving && Vector3.Distance(this.currentSmoothPosition.TargetPosition, Position) <= 0.025f)
		{
			if (DestroyWhenFinish)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
			}
			return;
		}
		if (Vector3.Distance(this.rigidbody.position, Position) > 0.025f)
		{
			if (this.currentSmoothPosition.DestroyWhenFinish)
			{
				return;
			}
			if (DestroyWhenFinish)
			{
				this.HeldByPlayerID = -10;
				this.ID = -1;
			}
			else
			{
				this.HeldByPlayerID = -1;
			}
			this.ResetCardJoint();
			this.currentSmoothPosition.Moving = true;
			this.currentSmoothPosition.Colliding = Colliding;
			this.currentSmoothPosition.FastSpeed = FastSpeed;
			this.currentSmoothPosition.TargetPosition = Position;
			this.currentSmoothPosition.DestroyWhenFinish = DestroyWhenFinish;
			this.currentSmoothPosition.ToggleCollidingBackOn = ToggleCollidingBackOn;
			this.currentSmoothPosition.IgnoreHandTriggers = IgnoreHandTriggers;
			this.currentSmoothPosition.DontStopBefore = DontStopBefore;
			this.IsLocked = Freeze;
			if (this.OverrideHighlightColor != highlightColor)
			{
				if (highlightColor != null)
				{
					base.networkView.RPC<Color32>(RPCTarget.All, new Action<Color32>(this.RPCOverrideHighlightColor), highlightColor.Value);
					return;
				}
				base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearOverrideHighlightColor));
			}
		}
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x0009F348 File Offset: 0x0009D548
	[Remote(SendType.ReliableNoDelay)]
	public void RPCSetSmoothPosition(Vector3 Position, bool Colliding = true, bool FastSpeed = false)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, bool, bool>(RPCTarget.Server, new Action<Vector3, bool, bool>(this.RPCSetSmoothPosition), Position, Colliding, FastSpeed);
			return;
		}
		if (!this.IsLocked)
		{
			this.SetSmoothPosition(Position, Colliding, FastSpeed, false, true, null, false, false, null);
		}
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x0009F398 File Offset: 0x0009D598
	public Vector3? GetSmoothRotation()
	{
		if (this.currentSmoothRotation.Moving)
		{
			return new Vector3?(this.currentSmoothRotation.TargetRotation.eulerAngles);
		}
		return null;
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x0009F3D1 File Offset: 0x0009D5D1
	public void SetSmoothRotation(Vector3 Rotation, bool Colliding = true, bool FastSpeed = false, bool DestroyWhenFinish = false, bool ToggleCollidingBackOn = true, Color? highlightColor = null, bool Freeze = false)
	{
		this.SetSmoothRotation(Quaternion.Euler(Rotation), Colliding, FastSpeed, DestroyWhenFinish, ToggleCollidingBackOn, highlightColor, Freeze);
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x0009F3EC File Offset: 0x0009D5EC
	public void SetSmoothRotation(Quaternion Rotation, bool Colliding = true, bool FastSpeed = false, bool DestroyWhenFinish = false, bool ToggleCollidingBackOn = true, Color? highlightColor = null, bool Freeze = false)
	{
		if (this.currentSmoothRotation.Moving && Quaternion.Angle(this.currentSmoothRotation.TargetRotation, Rotation) <= 1f)
		{
			return;
		}
		if (Quaternion.Angle(this.rigidbody.rotation, Rotation) > 1f)
		{
			if (this.currentSmoothRotation.DestroyWhenFinish || this.currentSmoothPosition.DestroyWhenFinish)
			{
				return;
			}
			if (DestroyWhenFinish)
			{
				this.HeldByPlayerID = -10;
				this.ID = -1;
			}
			else
			{
				this.HeldByPlayerID = -1;
			}
			this.ResetCardJoint();
			this.currentSmoothRotation.Moving = true;
			this.currentSmoothRotation.Colliding = Colliding;
			this.currentSmoothRotation.FastSpeed = FastSpeed;
			this.currentSmoothRotation.TargetRotation = Rotation;
			this.currentSmoothRotation.DestroyWhenFinish = DestroyWhenFinish;
			this.currentSmoothRotation.ToggleCollidingBackOn = ToggleCollidingBackOn;
			this.IsLocked = Freeze;
			if (this.OverrideHighlightColor == null && this.OverrideHighlightColor != highlightColor)
			{
				if (highlightColor != null)
				{
					base.networkView.RPC<Color32>(RPCTarget.All, new Action<Color32>(this.RPCOverrideHighlightColor), highlightColor.Value);
					return;
				}
				base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearOverrideHighlightColor));
			}
		}
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x0009F560 File Offset: 0x0009D760
	[Remote(SendType.ReliableNoDelay)]
	public void RPCSetSmoothRotation(Vector3 Rotation, bool Colliding = true, bool FastSpeed = false)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, bool, bool>(RPCTarget.Server, new Action<Vector3, bool, bool>(this.RPCSetSmoothRotation), Rotation, Colliding, FastSpeed);
			return;
		}
		if (!this.IsLocked)
		{
			this.SetSmoothRotation(Rotation, Colliding, FastSpeed, false, true, null, false);
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06001750 RID: 5968 RVA: 0x0009F5AD File Offset: 0x0009D7AD
	public bool IsSmoothMoving
	{
		get
		{
			return this.currentSmoothPosition.Moving || this.currentSmoothRotation.Moving;
		}
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x0009F5C9 File Offset: 0x0009D7C9
	public void StopSmoothMove(bool Teleport = true)
	{
		this.StopSmoothPosition(Teleport);
		this.StopSmoothRotation(Teleport);
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x0009F5DC File Offset: 0x0009D7DC
	public void StopSmoothPosition(bool Teleport = true)
	{
		if (this.currentSmoothPosition.Moving)
		{
			if (this.currentSmoothPosition.DestroyWhenFinish)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				return;
			}
			this.currentSmoothPosition.Moving = false;
			if (Teleport)
			{
				if (this.IsLocked)
				{
					this.transform.position = this.currentSmoothPosition.TargetPosition;
				}
				else
				{
					this.rigidbody.position = this.currentSmoothPosition.TargetPosition;
				}
			}
			this.rigidbody.velocity = Vector3.zero;
			if (!this.IsSmoothMoving)
			{
				this.resetSmooth();
			}
		}
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x0009F67C File Offset: 0x0009D87C
	public void StopSmoothRotation(bool Teleport = true)
	{
		if (this.currentSmoothRotation.Moving)
		{
			if (this.currentSmoothRotation.DestroyWhenFinish)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				return;
			}
			this.currentSmoothRotation.Moving = false;
			if (Teleport)
			{
				if (this.IsLocked)
				{
					this.transform.rotation = this.currentSmoothRotation.TargetRotation;
				}
				else
				{
					this.rigidbody.rotation = this.currentSmoothRotation.TargetRotation;
				}
			}
			this.rigidbody.angularVelocity = Vector3.zero;
			if (!this.IsSmoothMoving)
			{
				this.resetSmooth();
			}
		}
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x0009F71C File Offset: 0x0009D91C
	private void resetSmooth()
	{
		this.HeldByPlayerID = -1;
		if (!this.CurrentPlayerHand)
		{
			this.rigidbody.useGravity = this.GetUseGravity();
		}
		if (this.currentSmoothPosition.ToggleCollidingBackOn && this.currentSmoothRotation.ToggleCollidingBackOn)
		{
			this.SetCollision(true);
		}
		if (this.IsLocked)
		{
			this.isKinematic = this.IsLocked;
		}
		this.currentSmoothPosition.Reset();
		this.currentSmoothRotation.Reset();
		this.rigidbody.mass = this.GetMass();
		this.rigidbody.drag = this.GetDrag();
		this.rigidbody.angularDrag = this.GetAngularDrag();
		if (this.OverrideHighlightColor != null)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearOverrideHighlightColor));
		}
		EventManager.TriggerObjectFinishSmoothMove(this);
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x0009F7F8 File Offset: 0x0009D9F8
	private void SetSmoothToRotationValue(RotationValue rotationValue, int playerId = -1)
	{
		if (rotationValue != null)
		{
			if (this.soundScript)
			{
				this.soundScript.ShakeSound();
			}
			Quaternion quaternion = Quaternion.Euler(rotationValue.rotation);
			GameObject gameObject = base.gameObject;
			Vector3 vector = NetworkSingleton<ManagerPhysicsObject>.Instance.SurfacePointBelowObject(gameObject);
			Vector3 position = gameObject.transform.position;
			vector = new Vector3(position.x, vector.y + this.GetBounds().extents.magnitude + 0.1f, position.z);
			this.SetSmoothPosition(vector, false, true, false, true, null, false, false, null);
			Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(playerId);
			if (pointer)
			{
				quaternion = Quaternion.Euler(new Vector3(0f, pointer.transform.eulerAngles.y - 180f, 0f)) * quaternion;
				this.SetSmoothRotation(quaternion, true, false, false, true, new Color?(Colour.ColourFromLabel(pointer.PointerColorLabel)), false);
				return;
			}
			this.SetSmoothRotation(quaternion, true, false, false, true, null, false);
		}
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x0009F924 File Offset: 0x0009DB24
	public bool HasStates()
	{
		return this.GetSelectedStateId() != -1;
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x0009F934 File Offset: 0x0009DB34
	public int GetSelectedStateId()
	{
		if (this.cacheSelectedStateId != -1)
		{
			return this.cacheSelectedStateId;
		}
		if (this.States == null || this.States.Count == 0)
		{
			return -1;
		}
		int num = 1;
		while (num <= 10000 && this.States.ContainsKey(num))
		{
			num++;
		}
		this.cacheSelectedStateId = num;
		return num;
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x0009F98E File Offset: 0x0009DB8E
	public int GetStatesCount()
	{
		if (this.cacheStatesCount != -1)
		{
			return this.cacheStatesCount;
		}
		if (this.States != null)
		{
			this.cacheStatesCount = this.States.Count + 1;
		}
		return this.cacheStatesCount;
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x0009F9C4 File Offset: 0x0009DBC4
	public GameObject ShuffleStates()
	{
		if (this.GetSelectedStateId() != -1)
		{
			int id = UnityEngine.Random.Range(0, this.States.Count + 1) + 1;
			return this.ChangeState(id);
		}
		return null;
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x0009F9FC File Offset: 0x0009DBFC
	[Remote(SendType.ReliableNoDelay)]
	public GameObject ChangeState(int id)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, GameObject>(RPCTarget.Server, new Func<int, GameObject>(this.ChangeState), id);
			return null;
		}
		int selectedStateId = this.GetSelectedStateId();
		if (id == selectedStateId)
		{
			if (this.soundScript)
			{
				this.soundScript.PickUpSound();
			}
			if (PlayerScript.PointerScript.InfoObject == base.gameObject)
			{
				PlayerScript.PointerScript.ResetInfoObject();
			}
		}
		if (selectedStateId != -1 && this.States.ContainsKey(id) && this.ID != -1)
		{
			bool flag = false;
			if (PlayerScript.PointerScript != null && PlayerScript.PointerScript.HighLightedObjects.Contains(this))
			{
				flag = true;
				PlayerScript.PointerScript.RemoveHighlight(this, false);
			}
			ObjectState objectState = this.States[id];
			ObjectState objectState2 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
			objectState2.States = null;
			Vector3 position = this.transform.position;
			objectState.Transform.posX = position.x;
			objectState.Transform.posY = position.y;
			objectState.Transform.posZ = position.z;
			Vector3 eulerAngles = this.transform.eulerAngles;
			objectState.Transform.rotX = eulerAngles.x;
			objectState.Transform.rotY = eulerAngles.y;
			objectState.Transform.rotZ = eulerAngles.z;
			objectState.JointFixed = objectState2.JointFixed;
			objectState.JointHinge = objectState2.JointHinge;
			objectState.JointSpring = objectState2.JointSpring;
			objectState2.JointFixed = null;
			objectState2.JointHinge = null;
			objectState2.JointSpring = null;
			JointState jointState = null;
			if (objectState.JointFixed != null && !string.IsNullOrEmpty(objectState.JointFixed.ConnectedBodyGUID))
			{
				jointState = objectState.JointFixed;
			}
			else if (objectState.JointHinge != null && !string.IsNullOrEmpty(objectState.JointHinge.ConnectedBodyGUID))
			{
				jointState = objectState.JointHinge;
			}
			else if (objectState.JointSpring != null && !string.IsNullOrEmpty(objectState.JointSpring.ConnectedBodyGUID))
			{
				jointState = objectState.JointSpring;
			}
			if (jointState != null)
			{
				NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(jointState.ConnectedBodyGUID);
				if (networkPhysicsObject)
				{
					Debug.Log("Generate joints!");
					NetworkSingleton<ManagerPhysicsObject>.Instance.GenerateJointRelations(new List<ObjectState>
					{
						objectState,
						NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(networkPhysicsObject)
					});
					NetworkSingleton<ManagerPhysicsObject>.Instance.JointRelationFromGUID[jointState.ConnectedBodyGUID].Target = networkPhysicsObject.gameObject;
				}
			}
			this.States.Add(selectedStateId, objectState2);
			this.States.Remove(id);
			objectState.States = this.States;
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
			NetworkSingleton<ManagerPhysicsObject>.Instance.JointRelationFromGUID.Clear();
			NetworkPhysicsObject NewNPO = gameObject.GetComponent<NetworkPhysicsObject>();
			NewNPO.HeldByPlayerID = this.HeldByPlayerID;
			NewNPO.HeldByTouchID = this.HeldByTouchID;
			NewNPO.HeldSpinRotationIndex = this.HeldSpinRotationIndex;
			NewNPO.HeldFlipRotationIndex = this.HeldFlipRotationIndex;
			NewNPO.HeldByControllerPickupRotation = this.HeldByControllerPickupRotation;
			if (this.CurrentPlayerHand != null)
			{
				NewNPO.SetObscured("StateChangeInHand", true, 2147483647U, false);
				Wait.Condition(delegate
				{
					NewNPO.SetObscured("StateChangeInHand", false, 2147483647U, false);
				}, () => NewNPO.CurrentPlayerHand != null, float.PositiveInfinity, null);
			}
			if (NewNPO.soundScript)
			{
				NewNPO.soundScript.PickUpSound();
			}
			if (flag)
			{
				Wait.Frames(delegate
				{
					PlayerScript.PointerScript.AddHighlight(NewNPO, false);
				}, 1);
			}
			string oldGUID = this.GUID;
			Wait.Frames(delegate
			{
				if (NewNPO.luaGameObjectScript.CanCall("onStateChange", false))
				{
					NewNPO.luaGameObjectScript.TryCall("onStateChange", new object[]
					{
						oldGUID
					});
				}
				if (LuaGlobalScriptManager.Instance.CanCall("onObjectStateChange", false))
				{
					LuaGlobalScriptManager.Instance.TryCall("onObjectStateChange", new object[]
					{
						NewNPO.luaGameObjectScript,
						oldGUID
					});
				}
			}, 1);
			return gameObject;
		}
		return null;
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x0009FE0C File Offset: 0x0009E00C
	public void NextState()
	{
		int num = this.GetSelectedStateId() + 1;
		if (num > this.GetStatesCount())
		{
			if (!NetworkPhysicsObject.WrapStates)
			{
				return;
			}
			num = 1;
		}
		this.ChangeState(num);
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x0009FE40 File Offset: 0x0009E040
	public void PrevState()
	{
		int num = this.GetSelectedStateId() - 1;
		if (num <= 0)
		{
			if (!NetworkPhysicsObject.WrapStates)
			{
				return;
			}
			num = this.GetStatesCount();
		}
		this.ChangeState(num);
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x0009FE74 File Offset: 0x0009E074
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void CreateStates(int PlayerId)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.CreateStates), PlayerId);
			return;
		}
		this.cacheSelectedStateId = -1;
		this.cacheStatesCount = -1;
		if (this.States == null)
		{
			this.States = new Dictionary<int, ObjectState>();
		}
		else
		{
			this.States.Clear();
		}
		List<GameObject> selectedObjects = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(PlayerId).GetSelectedObjects(-1, true, false);
		int num = 2;
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject && gameObject != base.gameObject)
			{
				ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(gameObject);
				objectState.States = null;
				this.States.Add(num, objectState);
				num++;
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(gameObject);
			}
		}
		if (this.States.Count == 0)
		{
			this.States = null;
		}
		this.GetSelectedStateId();
		this.GetStatesCount();
		this.SyncStatesToOthers();
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x0009FF8C File Offset: 0x0009E18C
	private void SyncStatesToOthers()
	{
		if (Network.isServer)
		{
			base.networkView.RPC<Dictionary<int, ObjectState>>(RPCTarget.Others, new Action<Dictionary<int, ObjectState>>(this.RPCStates), this.States);
		}
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x0009FFB3 File Offset: 0x0009E1B3
	[Remote(SerializationMethod.Json)]
	private void RPCStates(Dictionary<int, ObjectState> states)
	{
		this.States = states;
		this.cacheSelectedStateId = -1;
		this.cacheStatesCount = -1;
		this.GetSelectedStateId();
		this.GetStatesCount();
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x0009FFD8 File Offset: 0x0009E1D8
	public void ClearTags()
	{
		if (this.hasTags)
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearTags));
		}
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x0009FFFC File Offset: 0x0009E1FC
	[Remote(Permission.Admin)]
	private void RPCClearTags()
	{
		List<ulong> oldTags = ComponentTags.NewCopyOfFlags(this.tags);
		this.tags.Clear();
		this.hasTags = false;
		EventManager.TriggerObjectTagsChange(this, oldTags, this.tags);
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x000A0034 File Offset: 0x0009E234
	public void SetTag(int index, bool isEnabled)
	{
		base.networkView.RPC<int, bool>(RPCTarget.All, new Action<int, bool>(this.RPCSetTag), index, isEnabled);
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x000A0050 File Offset: 0x0009E250
	[Remote(Permission.Admin)]
	private void RPCSetTag(int index, bool isEnabled)
	{
		List<ulong> oldTags = ComponentTags.NewCopyOfFlags(this.tags);
		ComponentTags.SetFlag(ref this.tags, index, isEnabled);
		this.hasTags = (isEnabled || ComponentTags.HasAnyFlag(this.tags));
		EventManager.TriggerObjectTagsChange(this, oldTags, this.tags);
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x000A009C File Offset: 0x0009E29C
	[Remote(Permission.Admin)]
	private void RPCSetTags(List<ulong> flags)
	{
		List<ulong> oldTags = ComponentTags.NewCopyOfFlags(this.tags);
		this.tags = flags;
		this.hasTags = ComponentTags.HasAnyFlag(this.tags);
		EventManager.TriggerObjectTagsChange(this, oldTags, this.tags);
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x000A00DC File Offset: 0x0009E2DC
	public void LoadTags(List<string> labels)
	{
		this.ClearTags();
		for (int i = 0; i < labels.Count; i++)
		{
			TagLabel tagLabel = new TagLabel(labels[i]);
			int num = NetworkSingleton<ComponentTags>.Instance.TagIndexFromLabel(tagLabel);
			if (num >= 0)
			{
				this.SetTag(num, true);
			}
			else if (Network.isServer)
			{
				num = NetworkSingleton<ComponentTags>.Instance.AddTag(tagLabel);
				this.SetTag(num, true);
			}
			else
			{
				base.networkView.RPC<TagLabel, NetworkPhysicsObject>(RPCTarget.Server, new Action<TagLabel, NetworkPhysicsObject>(NetworkSingleton<ComponentTags>.Instance.RPCRequestTagForNPO), tagLabel, this);
			}
		}
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x000A0163 File Offset: 0x0009E363
	public void UpdateNPOTags(NetworkPlayer player)
	{
		if ((int)player.id != NetworkID.ID)
		{
			base.networkView.RPC<List<ulong>>(player, new Action<List<ulong>>(this.RPCSetTags), this.tags);
		}
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x000A0191 File Offset: 0x0009E391
	public bool TagIsSet(int index)
	{
		return ComponentTags.GetFlag(this.tags, index);
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x000A01A0 File Offset: 0x0009E3A0
	public bool TagIsSet(string labelText)
	{
		TagLabel label = new TagLabel(labelText);
		int num = NetworkSingleton<ComponentTags>.Instance.TagIndexFromLabel(label);
		return num != -1 && this.TagIsSet(num);
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000A01CE File Offset: 0x0009E3CE
	public bool TagsAllowActingUpon(NetworkPhysicsObject otherNPO)
	{
		return otherNPO != null && (!this.hasTags || ComponentTags.HaveMatchingFlag(this.tags, otherNPO.tags));
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x0600176A RID: 5994 RVA: 0x000A01F6 File Offset: 0x0009E3F6
	// (set) Token: 0x0600176B RID: 5995 RVA: 0x000A01FE File Offset: 0x0009E3FE
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int ManualMaxTypedNumber
	{
		get
		{
			return this._ManualMaxTypedNumber;
		}
		set
		{
			if (value == this._ManualMaxTypedNumber)
			{
				return;
			}
			this._ManualMaxTypedNumber = value;
			base.DirtySync("ManualMaxTypedNumber");
		}
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x000A021C File Offset: 0x0009E41C
	public int MaxTypedNumber()
	{
		int b = (!this.IgnoreSingleDigitDefault && Pointer.DEFAULT_TYPED_NUMBER_IS_SINGLE_DIGIT) ? 9 : int.MaxValue;
		if (this.ManualMaxTypedNumber < 0)
		{
			return Mathf.Min(this.MaxTypedNumberFunction(this), b);
		}
		return Mathf.Min(this.ManualMaxTypedNumber, b);
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000A026C File Offset: 0x0009E46C
	public static int DefaultMaxTypedNumber(NetworkPhysicsObject npo)
	{
		if (npo.HasRotationsValues())
		{
			return npo.RotationValues.Count;
		}
		if (npo.GetSelectedStateId() != -1 && NetworkPhysicsObject.ChangeStateByTypingNumbers)
		{
			return npo.GetStatesCount();
		}
		if (npo.CanBeHeldInHand && NetworkPhysicsObject.DrawByTypingNumbers && !npo.cardScript)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x000A02C4 File Offset: 0x0009E4C4
	public static void DefaultHandleTypedNumber(NetworkPhysicsObject npo, int playerID, int number)
	{
		if (npo.HasRotationsValues())
		{
			npo.SetRotationNumber(number, playerID);
			return;
		}
		if (npo.GetSelectedStateId() != -1 && NetworkPhysicsObject.ChangeStateByTypingNumbers)
		{
			npo.ChangeState(number);
			return;
		}
		if (npo.CanBeHeldInHand && NetworkPhysicsObject.DrawByTypingNumbers && !npo.cardScript)
		{
			if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, PlayerScript.PointerScript.PointerColorLabel, number, 0);
				return;
			}
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, NetworkSingleton<PlayerManager>.Instance.ColourLabelFromID(playerID), number, 0);
		}
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x000A035D File Offset: 0x0009E55D
	public void SetTypedNumberHandlers(NetworkPhysicsObject.MaxTypedNumberMethodDelegate maxTypedNumberMethod, NetworkPhysicsObject.HandleTypedNumberMethodDelegate handleTypedNumberMethod, bool ignoreSingleDigitDefault = false)
	{
		this.MaxTypedNumberFunction = maxTypedNumberMethod;
		this.HandleTypedNumber = handleTypedNumberMethod;
		this.IgnoreSingleDigitDefault = ignoreSingleDigitDefault;
	}

	// Token: 0x04000CA6 RID: 3238
	private const float FACEDOWN_THRESHOLD = 0.8f;

	// Token: 0x04000CA7 RID: 3239
	public static bool OverrideDefaultsWhenSpawning = false;

	// Token: 0x04000CA8 RID: 3240
	public static bool GridOverride = true;

	// Token: 0x04000CA9 RID: 3241
	public static bool AutoRaiseOverride = true;

	// Token: 0x04000CAA RID: 3242
	public static bool StickyOverride = true;

	// Token: 0x04000CAB RID: 3243
	public static bool SnapOverride = true;

	// Token: 0x04000CAC RID: 3244
	public static bool TooltipOverride = true;

	// Token: 0x04000CAD RID: 3245
	public static bool IgnoreFOWOverride = false;

	// Token: 0x04000CAE RID: 3246
	public static bool RevealFOROverride = false;

	// Token: 0x04000CAF RID: 3247
	public static bool UseHandsOverride = false;

	// Token: 0x04000CB0 RID: 3248
	public static bool WrapStates = false;

	// Token: 0x04000CB1 RID: 3249
	public static bool ChangeStateByTypingNumbers = true;

	// Token: 0x04000CB2 RID: 3250
	public static bool DrawByTypingNumbers = true;

	// Token: 0x04000CB3 RID: 3251
	public static bool AllowTypingNegativeNumbers = false;

	// Token: 0x04000CB4 RID: 3252
	public static bool UseParticleHighlights = false;

	// Token: 0x04000CB5 RID: 3253
	public static int NumParticleHighlights = 1;

	// Token: 0x04000CB6 RID: 3254
	private readonly List<ParticleHighlight> ParticleHighlights = new List<ParticleHighlight>(NetworkPhysicsObject.NumParticleHighlights);

	// Token: 0x04000CB7 RID: 3255
	public static NetworkPhysicsObject LastNPOHeldByMe;

	// Token: 0x04000CB8 RID: 3256
	private const float DRAG = 0.1f;

	// Token: 0x04000CB9 RID: 3257
	private const float ANGULAR_DRAG = 0.1f;

	// Token: 0x04000CBA RID: 3258
	private float mass = 1f;

	// Token: 0x04000CBB RID: 3259
	[NonSerialized]
	public bool IsFaceDown;

	// Token: 0x04000CBC RID: 3260
	[NonSerialized]
	public HandZone CurrentPlayerHand;

	// Token: 0x04000CBD RID: 3261
	[NonSerialized]
	public CardSetup MostRecentCardSetup;

	// Token: 0x04000CBE RID: 3262
	[NonSerialized]
	public NetworkPhysicsObject NPOBeingRotated;

	// Token: 0x04000CBF RID: 3263
	[NonSerialized]
	public List<UIPanel> UIPanels = new List<UIPanel>();

	// Token: 0x04000CC0 RID: 3264
	public bool HasInternalGrid;

	// Token: 0x04000CC1 RID: 3265
	public bool InternalGridIsOffset;

	// Token: 0x04000CC2 RID: 3266
	public float InternalGridSize;

	// Token: 0x04000CC4 RID: 3268
	private string _GUID;

	// Token: 0x04000CC6 RID: 3270
	[NonSerialized]
	public bool spawnedByUI;

	// Token: 0x04000CC7 RID: 3271
	[SerializeField]
	private bool _IsGrabbable = true;

	// Token: 0x04000CC8 RID: 3272
	public bool IsRegistered = true;

	// Token: 0x04000CC9 RID: 3273
	[SerializeField]
	private bool _IsLocked;

	// Token: 0x04000CCA RID: 3274
	[SerializeField]
	private bool _IgnoresGrid;

	// Token: 0x04000CCB RID: 3275
	[SerializeField]
	private bool _DoesNotPersist = true;

	// Token: 0x04000CCC RID: 3276
	[SerializeField]
	private bool _DoAutoRaise = true;

	// Token: 0x04000CCD RID: 3277
	[SerializeField]
	private bool _IsSticky = true;

	// Token: 0x04000CCE RID: 3278
	[SerializeField]
	private bool _IgnoresSnap;

	// Token: 0x04000CCF RID: 3279
	[SerializeField]
	private bool _ShowTooltip = true;

	// Token: 0x04000CD0 RID: 3280
	[SerializeField]
	private bool _RotatesThroughRotationValues;

	// Token: 0x04000CD1 RID: 3281
	[SerializeField]
	private bool _IgnoresFogOfWar;

	// Token: 0x04000CD2 RID: 3282
	[SerializeField]
	private bool _IsDragSelectable = true;

	// Token: 0x04000CD3 RID: 3283
	[SerializeField]
	private bool _IsGizmoSelectable = true;

	// Token: 0x04000CD4 RID: 3284
	[SerializeField]
	private bool _ShowRulerWhenHeld;

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	private bool _ShowGridProjection;

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	private bool _IsHeldInHand;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	private bool _IsSaved = true;

	// Token: 0x04000CD8 RID: 3288
	private bool _UseAltSounds;

	// Token: 0x04000CD9 RID: 3289
	private int _valueFlags;

	// Token: 0x04000CDA RID: 3290
	private int _Value;

	// Token: 0x04000CDB RID: 3291
	private string _Name = "";

	// Token: 0x04000CDC RID: 3292
	private string _Description = "";

	// Token: 0x04000CDD RID: 3293
	private string _GMNotes = "";

	// Token: 0x04000CDE RID: 3294
	private string _Memo;

	// Token: 0x04000CDF RID: 3295
	private Vector3 _AltLookAngle = Vector3.zero;

	// Token: 0x04000CE0 RID: 3296
	[NonSerialized]
	public bool bReduceForce;

	// Token: 0x04000CE1 RID: 3297
	[HideInInspector]
	public new Transform transform;

	// Token: 0x04000CE2 RID: 3298
	[HideInInspector]
	public Rigidbody rigidbody;

	// Token: 0x04000CE3 RID: 3299
	[HideInInspector]
	public SoundScript soundScript;

	// Token: 0x04000CE4 RID: 3300
	[HideInInspector]
	public CardScript cardScript;

	// Token: 0x04000CE5 RID: 3301
	[HideInInspector]
	public DeckScript deckScript;

	// Token: 0x04000CE6 RID: 3302
	[HideInInspector]
	public StackObject stackObject;

	// Token: 0x04000CE7 RID: 3303
	[HideInInspector]
	public HideObject hideObject;

	// Token: 0x04000CE8 RID: 3304
	[HideInInspector]
	public MaterialSyncScript materialSyncScript;

	// Token: 0x04000CE9 RID: 3305
	[HideInInspector]
	public MeshSyncScript meshSyncScript;

	// Token: 0x04000CEA RID: 3306
	[HideInInspector]
	public CustomObject customObject;

	// Token: 0x04000CEB RID: 3307
	[HideInInspector]
	public CustomMesh customMesh;

	// Token: 0x04000CEC RID: 3308
	[HideInInspector]
	public CustomImage customImage;

	// Token: 0x04000CED RID: 3309
	[HideInInspector]
	public CustomAssetbundle customAssetbundle;

	// Token: 0x04000CEE RID: 3310
	[HideInInspector]
	public CustomPDF customPDF;

	// Token: 0x04000CEF RID: 3311
	[HideInInspector]
	public CustomDice customDice;

	// Token: 0x04000CF0 RID: 3312
	[HideInInspector]
	public CustomToken customToken;

	// Token: 0x04000CF1 RID: 3313
	[HideInInspector]
	public CustomJigsawPuzzle customJigsawPuzzle;

	// Token: 0x04000CF2 RID: 3314
	[HideInInspector]
	public CustomTile customTile;

	// Token: 0x04000CF3 RID: 3315
	[HideInInspector]
	public CustomBoardScript customBoardScript;

	// Token: 0x04000CF4 RID: 3316
	[HideInInspector]
	public TableScript tableScript;

	// Token: 0x04000CF5 RID: 3317
	[HideInInspector]
	public ClockScript clockScript;

	// Token: 0x04000CF6 RID: 3318
	[HideInInspector]
	public TabletScript tabletScript;

	// Token: 0x04000CF7 RID: 3319
	[HideInInspector]
	public Mp3PlayerScript mp3PlayerScript;

	// Token: 0x04000CF8 RID: 3320
	[HideInInspector]
	public RPGFigurines rpgFigurines;

	// Token: 0x04000CF9 RID: 3321
	[HideInInspector]
	public CounterScript counterScript;

	// Token: 0x04000CFA RID: 3322
	[HideInInspector]
	public CalculatorScript calculatorScript;

	// Token: 0x04000CFB RID: 3323
	[HideInInspector]
	public TextTool textTool;

	// Token: 0x04000CFC RID: 3324
	[HideInInspector]
	public Zone zone;

	// Token: 0x04000CFD RID: 3325
	[HideInInspector]
	public HiddenZone hiddenZone;

	// Token: 0x04000CFE RID: 3326
	[HideInInspector]
	public RandomizeZone randomizeZone;

	// Token: 0x04000CFF RID: 3327
	[HideInInspector]
	public ScriptingZone scriptingZone;

	// Token: 0x04000D00 RID: 3328
	[HideInInspector]
	public FogOfWarZone fogOfWarZone;

	// Token: 0x04000D01 RID: 3329
	[HideInInspector]
	public LayoutZone layoutZone;

	// Token: 0x04000D02 RID: 3330
	[HideInInspector]
	public HandZone handZone;

	// Token: 0x04000D03 RID: 3331
	[HideInInspector]
	public SphereCollider sphereCollider;

	// Token: 0x04000D04 RID: 3332
	[HideInInspector]
	public NetworkInterpolate interpolate;

	// Token: 0x04000D05 RID: 3333
	[HideInInspector]
	public Highlighter highlighter;

	// Token: 0x04000D06 RID: 3334
	[HideInInspector]
	public CollisionEvents collisionEvents;

	// Token: 0x04000D07 RID: 3335
	[HideInInspector]
	public XmlUIScript xmlUI;

	// Token: 0x04000D08 RID: 3336
	[HideInInspector]
	public LuaGameObjectScript luaGameObjectScript;

	// Token: 0x04000D09 RID: 3337
	[HideInInspector]
	public FogOfWarRevealer fogOfWarRevealer;

	// Token: 0x04000D0A RID: 3338
	[HideInInspector]
	public ChildSpawner childSpawner;

	// Token: 0x04000D0B RID: 3339
	[NonSerialized]
	public List<int> CustomContextMenus = new List<int>();

	// Token: 0x04000D0C RID: 3340
	private bool isGizmoSelectedByMe;

	// Token: 0x04000D0D RID: 3341
	private List<int> PlayersSelectingWithGizmo = new List<int>();

	// Token: 0x04000D0E RID: 3342
	public const int HELD_BY_NOBODY = -1;

	// Token: 0x04000D0F RID: 3343
	public const int HELD_BY_NOBODY_AND_BEING_DESTROYED = -10;

	// Token: 0x04000D10 RID: 3344
	[NonSerialized]
	private int _HeldByPlayerID = -1;

	// Token: 0x04000D11 RID: 3345
	private int LastFrameHeldID = -1;

	// Token: 0x04000D12 RID: 3346
	[NonSerialized]
	public Vector3 PickedUpPosition = Vector3.zero;

	// Token: 0x04000D13 RID: 3347
	[NonSerialized]
	public Vector3 PickedUpRotation = Vector3.zero;

	// Token: 0x04000D14 RID: 3348
	[NonSerialized]
	public int PickedUpFromLayoutZone = -1;

	// Token: 0x04000D15 RID: 3349
	[NonSerialized]
	public int PickedUpFromLayoutZoneGroup = -1;

	// Token: 0x04000D16 RID: 3350
	[NonSerialized]
	public int PrevHeldByPlayerID = -1;

	// Token: 0x04000D17 RID: 3351
	[NonSerialized]
	public int HeldByTouchID = -1;

	// Token: 0x04000D18 RID: 3352
	[NonSerialized]
	public int LayoutGroupSortIndex;

	// Token: 0x04000D19 RID: 3353
	[NonSerialized]
	public Vector3? DesiredPosition;

	// Token: 0x04000D1A RID: 3354
	[NonSerialized]
	public Quaternion? DesiredRotation;

	// Token: 0x04000D1B RID: 3355
	[NonSerialized]
	public Vector3? DesiredVelocity;

	// Token: 0x04000D1C RID: 3356
	[NonSerialized]
	public Vector3? DesiredAngularVelocity;

	// Token: 0x04000D1D RID: 3357
	[NonSerialized]
	public float DesiredRotationStartTime;

	// Token: 0x04000D1E RID: 3358
	[NonSerialized]
	public float HeldMinimumY;

	// Token: 0x04000D1F RID: 3359
	private Vector3 _HeldOffset = Vector3.zero;

	// Token: 0x04000D20 RID: 3360
	private Vector3 _HeldRotationOffset = Vector3.zero;

	// Token: 0x04000D21 RID: 3361
	private Vector3 _HeldByControllerPickupOffset;

	// Token: 0x04000D22 RID: 3362
	private Quaternion _HeldByControllerPickupRotation;

	// Token: 0x04000D23 RID: 3363
	private int heldSpinRotationIndex;

	// Token: 0x04000D24 RID: 3364
	private int heldSpinRotationIndexVR;

	// Token: 0x04000D25 RID: 3365
	private int heldFlipRotationIndex;

	// Token: 0x04000D26 RID: 3366
	private int heldFlipRotationIndexVR;

	// Token: 0x04000D27 RID: 3367
	private int _HeldCloseInVR;

	// Token: 0x04000D29 RID: 3369
	public const string FACEDOWN_HIDER_ID = "facedown";

	// Token: 0x04000D2A RID: 3370
	public const uint INVERSE_HIDER_FLAG = 2147483648U;

	// Token: 0x04000D2B RID: 3371
	[SerializeField]
	private bool _IsHiddenWhenFaceDown;

	// Token: 0x04000D2C RID: 3372
	private bool prevIsHiddenWhenFaceDown;

	// Token: 0x04000D2D RID: 3373
	[NonSerialized]
	public List<GameObject> ShownWhenHidden = new List<GameObject>();

	// Token: 0x04000D2E RID: 3374
	[NonSerialized]
	public Dictionary<string, uint> InvisibleHiders = new Dictionary<string, uint>();

	// Token: 0x04000D2F RID: 3375
	[NonSerialized]
	public uint NormalInvisibleFlags;

	// Token: 0x04000D30 RID: 3376
	[NonSerialized]
	public uint ReverseInvisibleFlags;

	// Token: 0x04000D31 RID: 3377
	[NonSerialized]
	public bool OverrideIsInvisible;

	// Token: 0x04000D32 RID: 3378
	[NonSerialized]
	public bool CachedIsInvisible;

	// Token: 0x04000D33 RID: 3379
	public Dictionary<string, uint> ObscuredHiders = new Dictionary<string, uint>();

	// Token: 0x04000D34 RID: 3380
	[NonSerialized]
	public uint NormalObscuredFlags;

	// Token: 0x04000D35 RID: 3381
	[NonSerialized]
	public uint FacedownObscuredFlags;

	// Token: 0x04000D36 RID: 3382
	[NonSerialized]
	public uint ReverseObscuredFlags;

	// Token: 0x04000D37 RID: 3383
	[NonSerialized]
	public uint ShowThroughObscuredFlags;

	// Token: 0x04000D38 RID: 3384
	private GameObject ObscuredQuestionMark;

	// Token: 0x04000D39 RID: 3385
	[NonSerialized]
	public bool OverrideIsObscured;

	// Token: 0x04000D3A RID: 3386
	[NonSerialized]
	public bool CachedIsObscured;

	// Token: 0x04000D3B RID: 3387
	private Color HighlightColor = Colour.UnityClear;

	// Token: 0x04000D3C RID: 3388
	public Color? LuaHighlightColor;

	// Token: 0x04000D3D RID: 3389
	private Color? TemporaryHighlightColor;

	// Token: 0x04000D3E RID: 3390
	private float TemporaryHighlightColorPulse;

	// Token: 0x04000D3F RID: 3391
	private float TemporaryHighlightColorStart;

	// Token: 0x04000D40 RID: 3392
	private Color? OverrideHighlightColor;

	// Token: 0x04000D41 RID: 3393
	private List<string> PeekIds = new List<string>();

	// Token: 0x04000D42 RID: 3394
	private UnityEngine.Coroutine removePeekCoroutine;

	// Token: 0x04000D43 RID: 3395
	private WaitForSeconds peekWait = new WaitForSeconds(3f);

	// Token: 0x04000D44 RID: 3396
	private static readonly List<SnapPoint> snaps = new List<SnapPoint>();

	// Token: 0x04000D45 RID: 3397
	private const float SleepTime = 0.5f;

	// Token: 0x04000D46 RID: 3398
	private float SleepTimeHolder;

	// Token: 0x04000D47 RID: 3399
	public RigidbodyState OverrideRigidbody;

	// Token: 0x04000D48 RID: 3400
	public PhysicsMaterialState OverridePhysicsMaterial;

	// Token: 0x04000D4D RID: 3405
	public bool IsBeingDestroyed;

	// Token: 0x04000D4E RID: 3406
	[NonSerialized]
	public List<Collider> TriggerOnlyColliders = new List<Collider>();

	// Token: 0x04000D4F RID: 3407
	[NonSerialized]
	public List<LayoutZone> LayoutZonesContaining = new List<LayoutZone>();

	// Token: 0x04000D50 RID: 3408
	private static readonly List<Transform> childTransforms = new List<Transform>();

	// Token: 0x04000D51 RID: 3409
	private Vector3 _Scale = Vector3.one;

	// Token: 0x04000D52 RID: 3410
	private bool recalculateStateScale;

	// Token: 0x04000D53 RID: 3411
	[NonSerialized]
	public Vector3 BaseScale = Vector3.one;

	// Token: 0x04000D54 RID: 3412
	private float StartMass = 1f;

	// Token: 0x04000D55 RID: 3413
	private bool isSpawnFreeze;

	// Token: 0x04000D56 RID: 3414
	private Bounds CombinedBounds;

	// Token: 0x04000D57 RID: 3415
	private Bounds CombinedRendererBounds;

	// Token: 0x04000D58 RID: 3416
	private Vector3 BoundsCenterOffset = Vector3.zero;

	// Token: 0x04000D59 RID: 3417
	private Vector3 RendererBoundsCenterOffset = Vector3.zero;

	// Token: 0x04000D5A RID: 3418
	private bool bColliding = true;

	// Token: 0x04000D5B RID: 3419
	private readonly List<Collider> Colliders = new List<Collider>();

	// Token: 0x04000D5C RID: 3420
	private readonly List<MeshCollider> ConvexColliders = new List<MeshCollider>();

	// Token: 0x04000D5D RID: 3421
	private readonly List<MeshCollider> ConcaveColliders = new List<MeshCollider>();

	// Token: 0x04000D5E RID: 3422
	private Vector3 prevPosition;

	// Token: 0x04000D5F RID: 3423
	private Quaternion prevRotation;

	// Token: 0x04000D60 RID: 3424
	private bool justMoved;

	// Token: 0x04000D61 RID: 3425
	private bool sleeping = true;

	// Token: 0x04000D62 RID: 3426
	[NonSerialized]
	public bool JustFellAsleep;

	// Token: 0x04000D63 RID: 3427
	private bool checkLoadFreeze = true;

	// Token: 0x04000D64 RID: 3428
	[NonSerialized]
	public List<Renderer> Renderers = new List<Renderer>();

	// Token: 0x04000D67 RID: 3431
	public List<RotationValue> RotationValues = new List<RotationValue>();

	// Token: 0x04000D68 RID: 3432
	public const float SmoothPositionPrecision = 0.025f;

	// Token: 0x04000D69 RID: 3433
	public const float SmoothRotationPrecision = 1f;

	// Token: 0x04000D6A RID: 3434
	public static bool DebugSmoothMove = false;

	// Token: 0x04000D6B RID: 3435
	[NonSerialized]
	public NetworkPhysicsObject.SmoothPosition currentSmoothPosition = new NetworkPhysicsObject.SmoothPosition();

	// Token: 0x04000D6C RID: 3436
	[NonSerialized]
	public NetworkPhysicsObject.SmoothRotation currentSmoothRotation = new NetworkPhysicsObject.SmoothRotation();

	// Token: 0x04000D6D RID: 3437
	public Dictionary<int, ObjectState> States;

	// Token: 0x04000D6E RID: 3438
	private int cacheSelectedStateId = -1;

	// Token: 0x04000D6F RID: 3439
	private int cacheStatesCount = -1;

	// Token: 0x04000D70 RID: 3440
	[NonSerialized]
	public List<ulong> tags = new List<ulong>();

	// Token: 0x04000D71 RID: 3441
	[NonSerialized]
	public bool hasTags;

	// Token: 0x04000D72 RID: 3442
	public NetworkPhysicsObject.MaxTypedNumberMethodDelegate MaxTypedNumberFunction;

	// Token: 0x04000D73 RID: 3443
	public NetworkPhysicsObject.HandleTypedNumberMethodDelegate HandleTypedNumber;

	// Token: 0x04000D74 RID: 3444
	public bool IgnoreSingleDigitDefault;

	// Token: 0x04000D75 RID: 3445
	private int _ManualMaxTypedNumber = -1;

	// Token: 0x020006A4 RID: 1700
	[Serializable]
	public class SmoothOrientation
	{
		// Token: 0x06003C14 RID: 15380 RVA: 0x001787B4 File Offset: 0x001769B4
		public virtual void Reset()
		{
			this.Moving = false;
			this.Colliding = true;
			this.ToggleCollidingBackOn = true;
			this.FastSpeed = false;
			this.DestroyWhenFinish = false;
			this.IgnoreHandTriggers = false;
			this.DontStopBefore = null;
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x001787E7 File Offset: 0x001769E7
		public bool CanStop()
		{
			return this.DontStopBefore == null || !this.DontStopBefore.currentSmoothPosition.Moving;
		}

		// Token: 0x040028D0 RID: 10448
		public bool Moving;

		// Token: 0x040028D1 RID: 10449
		public bool Colliding = true;

		// Token: 0x040028D2 RID: 10450
		public bool ToggleCollidingBackOn = true;

		// Token: 0x040028D3 RID: 10451
		public bool FastSpeed;

		// Token: 0x040028D4 RID: 10452
		public bool DestroyWhenFinish;

		// Token: 0x040028D5 RID: 10453
		public bool IgnoreHandTriggers;

		// Token: 0x040028D6 RID: 10454
		public NetworkPhysicsObject DontStopBefore;
	}

	// Token: 0x020006A5 RID: 1701
	[Serializable]
	public class SmoothPosition : NetworkPhysicsObject.SmoothOrientation
	{
		// Token: 0x06003C17 RID: 15383 RVA: 0x00178822 File Offset: 0x00176A22
		public override void Reset()
		{
			base.Reset();
			this.TargetPosition = Vector3.zero;
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00178835 File Offset: 0x00176A35
		public override string ToString()
		{
			return this.TargetPosition.ToString();
		}

		// Token: 0x040028D7 RID: 10455
		public Vector3 TargetPosition = Vector3.zero;
	}

	// Token: 0x020006A6 RID: 1702
	[Serializable]
	public class SmoothRotation : NetworkPhysicsObject.SmoothOrientation
	{
		// Token: 0x06003C1A RID: 15386 RVA: 0x0017885B File Offset: 0x00176A5B
		public override void Reset()
		{
			base.Reset();
			this.TargetRotation = Quaternion.identity;
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x0017886E File Offset: 0x00176A6E
		public override string ToString()
		{
			return this.TargetRotation.ToString();
		}

		// Token: 0x040028D8 RID: 10456
		public Quaternion TargetRotation = Quaternion.identity;
	}

	// Token: 0x020006A7 RID: 1703
	// (Invoke) Token: 0x06003C1E RID: 15390
	public delegate int MaxTypedNumberMethodDelegate(NetworkPhysicsObject npo);

	// Token: 0x020006A8 RID: 1704
	// (Invoke) Token: 0x06003C22 RID: 15394
	public delegate void HandleTypedNumberMethodDelegate(NetworkPhysicsObject npo, int playerID, int number);
}
