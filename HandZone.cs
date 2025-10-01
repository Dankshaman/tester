using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class HandZone : Zone, INetworkLifeCycle
{
	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06000F4C RID: 3916 RVA: 0x000688AF File Offset: 0x00066AAF
	// (set) Token: 0x06000F4D RID: 3917 RVA: 0x000688B8 File Offset: 0x00066AB8
	[Sync(Permission.Client, null, SerializationMethod.Default, false, validationFunction = "Permissions/Zones")]
	public string TriggerLabel
	{
		get
		{
			return this._TriggerLabel;
		}
		set
		{
			if (value != this._TriggerLabel)
			{
				if (!Colour.IsColourLabel(value))
				{
					return;
				}
				this.ResetHandZone();
				this._TriggerLabel = value;
				this.TriggerColour = Colour.ColourFromLabel(this.TriggerLabel);
				this.highlightColor = Colour.DarkenedFromColour(this.TriggerColour);
				EventManager.TriggerHandZoneChange(this);
				base.DirtySync("TriggerLabel");
			}
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06000F4E RID: 3918 RVA: 0x00068921 File Offset: 0x00066B21
	public bool HasQueuedHandSelectMode
	{
		get
		{
			return this.HandSelectModeSettingsQueue != null && this.HandSelectModeSettingsQueue.Count > 0;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06000F4F RID: 3919 RVA: 0x0006893B File Offset: 0x00066B3B
	public bool bDisabled
	{
		get
		{
			return !NetworkSingleton<Hands>.Instance.handsState.Enable || (NetworkSingleton<Hands>.Instance.handsState.DisableUnused && !NetworkSingleton<PlayerManager>.Instance.ColourInUse(this.TriggerColour));
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00068978 File Offset: 0x00066B78
	private static List<HandZone> HandZones
	{
		get
		{
			ManagerPhysicsObject instance = NetworkSingleton<ManagerPhysicsObject>.Instance;
			if (instance)
			{
				return instance.HandZones;
			}
			return new List<HandZone>();
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0006899F File Offset: 0x00066B9F
	// (set) Token: 0x06000F52 RID: 3922 RVA: 0x000689A7 File Offset: 0x00066BA7
	public string HiderID { get; private set; }

	// Token: 0x06000F53 RID: 3923 RVA: 0x000689B0 File Offset: 0x00066BB0
	protected override void Awake()
	{
		base.Awake();
		if (!base.NPO)
		{
			if (string.IsNullOrEmpty(this.TriggerLabel))
			{
				this.TriggerLabel = base.gameObject.name.Replace(" Trigger", "");
			}
			base.gameObject.SetActive(false);
			return;
		}
		this.HiderID = base.NPO.networkView.id.ToString();
		base.gameObject.tag = "Hand";
		this.handSortComparer = new HandZone.HandSortComparer(this);
		this.renderer = base.GetComponent<Renderer>();
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x00068A50 File Offset: 0x00066C50
	protected override void Start()
	{
		base.Start();
		if (!base.NPO)
		{
			return;
		}
		if (Network.isServer && string.IsNullOrEmpty(this.TriggerLabel))
		{
			this.TriggerLabel = "White";
		}
		this.ThisMat = this.renderer.material;
		this.CachedHandSettings();
		EventManager.OnLoadingComplete += this.FinishSaveLoading;
		EventManager.OnLateFixedUpdate += this.LateFixedUpdate;
		EventManager.OnObjectLeaveContainer += this.onObjectLeaveContainer;
		EventManager.OnObjectFinishSmoothMove += this.onObjectFinishSmoothMove;
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x00068AEC File Offset: 0x00066CEC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!base.NPO)
		{
			return;
		}
		EventManager.OnLoadingComplete -= this.FinishSaveLoading;
		EventManager.OnLateFixedUpdate -= this.LateFixedUpdate;
		EventManager.OnObjectLeaveContainer -= this.onObjectLeaveContainer;
		EventManager.OnObjectFinishSmoothMove -= this.onObjectFinishSmoothMove;
		EventManager.TriggerHandZoneChange(this);
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00068B57 File Offset: 0x00066D57
	private void FinishSaveLoading()
	{
		this.CachedHandSettings();
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x00068B60 File Offset: 0x00066D60
	private void CachedHandSettings()
	{
		if (!this)
		{
			Debug.LogError("This hand zone should not be null wtf happened!?");
			return;
		}
		this.RotationInt = (NetworkSingleton<ManagerPhysicsObject>.Instance.SpinRotationIndexFromGrabbable(base.gameObject, 15f) - 12) % 24;
		Vector3 vector;
		this.SurfacePoint = NetworkSingleton<ManagerPhysicsObject>.Instance.StaticSurfacePointBelowWorldPos(base.transform.position, out vector);
		Quaternion rotation = base.transform.rotation;
		base.transform.rotation = Quaternion.identity;
		this.HandBounds = this.renderer.bounds;
		base.transform.rotation = rotation;
		EventManager.TriggerHandZoneChange(this);
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x00068C00 File Offset: 0x00066E00
	protected override bool ValidateAddObject(NetworkPhysicsObject npo)
	{
		return base.ValidateAddObject(npo) && (npo.CanBeHeldInHand && !npo.IsLocked && (!npo.cardScript || !npo.fixedJoint || !npo.cardScript.RootHeldByAnyone()) && (!npo.currentSmoothPosition.Moving || !npo.currentSmoothPosition.IgnoreHandTriggers) && !this.bDisabled);
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00068C78 File Offset: 0x00066E78
	protected override void OnAddObject(NetworkPhysicsObject npo)
	{
		if (npo.IsHeldByNobody)
		{
			npo.SetCollision(false);
		}
		if (!npo.CurrentPlayerHand)
		{
			npo.CurrentPlayerHand = this;
		}
		bool flag = npo.SetShowThroughObscured(this.revealHandToPlayers);
		bool flag2 = npo.SetObscured(this.HiderID, true, this.hiddenFlags, false);
		if (flag && !flag2)
		{
			npo.UpdateVisiblity(false);
		}
		if (Network.isServer)
		{
			npo.HeldSpinRotationIndex = this.GetRotationInt(npo);
		}
		base.OnAddObject(npo);
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00068CF0 File Offset: 0x00066EF0
	protected override void OnRemoveObject(NetworkPhysicsObject npo)
	{
		this.ResetHandObject(npo);
		if (this.HasQueuedHandSelectMode && Singleton<HandSelectMode>.Instance.ActiveHandZone == this)
		{
			Singleton<HandSelectMode>.Instance.Unselect(npo);
		}
		base.OnRemoveObject(npo);
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x00068D26 File Offset: 0x00066F26
	public int GetRotationInt(NetworkPhysicsObject npo)
	{
		if (!npo.isSidewaysCard())
		{
			return this.RotationInt;
		}
		return this.RotationInt - 6;
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x00068D40 File Offset: 0x00066F40
	private void OnTriggerEnter(Collider otherCollider)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(otherCollider);
		if (!networkPhysicsObject)
		{
			return;
		}
		base.AddObject(otherCollider, networkPhysicsObject);
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x00068D6C File Offset: 0x00066F6C
	private void OnTriggerExit(Collider otherCollider)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(otherCollider);
		if (!networkPhysicsObject)
		{
			return;
		}
		base.RemoveObject(otherCollider, networkPhysicsObject);
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00068D98 File Offset: 0x00066F98
	public void RevealHandToPlayer(Colour playerColor)
	{
		this.RevealHandToPlayerRPC(playerColor);
		base.networkView.RPC<Colour>(RPCTarget.Others, new Action<Colour>(this.RevealHandToPlayerRPC), playerColor);
		for (int i = 0; i < this.ContainedNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.ContainedNPOs[i];
			if (networkPhysicsObject.SetShowThroughObscured(this.revealHandToPlayers))
			{
				networkPhysicsObject.UpdateVisiblity(false);
			}
		}
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x00068DFD File Offset: 0x00066FFD
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RevealHandToPlayerRPC(Colour playerColor)
	{
		if (playerColor == Colour.Black)
		{
			this.revealHandToPlayers = uint.MaxValue;
			return;
		}
		this.revealHandToPlayers = Colour.FlagFromColor(playerColor);
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00068E28 File Offset: 0x00067028
	public void EndRevealHand()
	{
		this.EndRevealHandToPlayerRPC();
		base.networkView.RPC(RPCTarget.Others, new Action(this.EndRevealHandToPlayerRPC));
		for (int i = 0; i < this.ContainedNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = this.ContainedNPOs[i];
			if (networkPhysicsObject.ClearShowThroughObscured())
			{
				networkPhysicsObject.UpdateVisiblity(false);
			}
		}
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00068E85 File Offset: 0x00067085
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void EndRevealHandToPlayerRPC()
	{
		this.revealHandToPlayers = 0U;
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00068E8E File Offset: 0x0006708E
	public bool RevealingToAllPlayers
	{
		get
		{
			return this.revealHandToPlayers == uint.MaxValue;
		}
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00068E99 File Offset: 0x00067099
	public void ResetHandZone()
	{
		base.RemoveAllObjects();
		base.ResetZone();
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00068EA8 File Offset: 0x000670A8
	public static HandZone GetHandZoneBeingRevealedForColour(string playerLabel)
	{
		for (int i = 0; i < HandZone.HandZones.Count; i++)
		{
			HandZone handZone = HandZone.HandZones[i];
			if (handZone.TriggerLabel == playerLabel && handZone.revealHandToPlayers != 0U)
			{
				return handZone;
			}
		}
		return null;
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00068EF0 File Offset: 0x000670F0
	public static void ResetHandZones()
	{
		for (int i = 0; i < HandZone.HandZones.Count; i++)
		{
			HandZone.HandZones[i].ResetHandZone();
		}
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00068F24 File Offset: 0x00067124
	public void ResetHandObject(NetworkPhysicsObject npo)
	{
		if (!npo)
		{
			return;
		}
		bool flag = npo.ClearShowThroughObscured();
		bool flag2 = npo.SetObscured(this.HiderID, false, 2147483647U, false);
		if (flag && !flag2)
		{
			npo.UpdateVisiblity(false);
		}
		if (npo.CurrentPlayerHand == this)
		{
			npo.CurrentPlayerHand = null;
		}
		if (Network.isClient)
		{
			return;
		}
		if (!npo.IsDestroyed)
		{
			npo.SetCollision(true);
		}
		npo.ResetRigidbody();
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00068F92 File Offset: 0x00067192
	public void MoveToStash(NetworkPhysicsObject npo)
	{
		if (!Network.isServer)
		{
			return;
		}
		base.networkView.RPC<NetworkPhysicsObject>(RPCTarget.All, new Action<NetworkPhysicsObject>(this.RPCSendToStash), npo);
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00068FB8 File Offset: 0x000671B8
	[Remote(Permission.Server)]
	private void RPCSendToStash(NetworkPhysicsObject npo)
	{
		if (this.Stash == null)
		{
			this.Stash = npo;
		}
		else if (this.Stash != npo)
		{
			this.Stash.IsGrabbable = true;
			this.Stash.CanBeHeldInHand = true;
			List<NetworkPhysicsObject> selectedNPOs = new List<NetworkPhysicsObject>
			{
				this.Stash,
				npo
			};
			List<NetworkPhysicsObject> list = NetworkSingleton<ManagerPhysicsObject>.Instance.Group(selectedNPOs);
			if (list.Count > 0)
			{
				this.Stash = list[0];
			}
		}
		if (this.Stash)
		{
			this.ContainedNPOs.Remove(this.Stash);
			this.Stash.IsGrabbable = false;
			this.Stash.CanBeHeldInHand = false;
		}
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x00069074 File Offset: 0x00067274
	public void MoveStashToHand()
	{
		if (this.Stash == null || !Network.isServer)
		{
			return;
		}
		if (this.Stash.deckScript && this.Stash.deckScript.num_cards_ > 1)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(this.Stash.ID, this.TriggerLabel, this.Stash.deckScript.num_cards_, this.GetHandIndex(true));
		}
		else
		{
			this.Stash.IsGrabbable = true;
			this.Stash.CanBeHeldInHand = true;
			this.ContainedNPOs.Add(this.Stash);
			this.DealToEnd(this.Stash, true, false);
		}
		this.clearStash();
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x0006912D File Offset: 0x0006732D
	private void clearStash()
	{
		base.networkView.RPC(RPCTarget.All, new Action(this.RPCClearStash));
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x00069147 File Offset: 0x00067347
	[Remote(Permission.Server)]
	private void RPCClearStash()
	{
		this.Stash = null;
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x00069150 File Offset: 0x00067350
	public void SetStashLocation(Vector3? position, int rotation)
	{
		if (!Network.isServer)
		{
			return;
		}
		base.networkView.RPC<Vector3?, int>(RPCTarget.All, new Action<Vector3?, int>(this.RPCSetStashLocation), position, rotation);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x00069174 File Offset: 0x00067374
	[Remote(Permission.Server)]
	private void RPCSetStashLocation(Vector3? position, int rotation)
	{
		this.StashPosition = position;
		this.StashRotation = new Vector3(0f, (float)(rotation + 90), 180f);
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x00069198 File Offset: 0x00067398
	protected override void Update()
	{
		base.Update();
		if (base.transform.hasChanged)
		{
			this.CachedHandSettings();
			base.transform.hasChanged = false;
		}
		if (NetworkSingleton<Hands>.Instance.handsState.Hiding != HidingType.Disable)
		{
			this.hiddenFlags = Colour.InverseFlagsFromLabel(this.TriggerLabel);
			if (NetworkSingleton<Hands>.Instance.handsState.Hiding == HidingType.Reverse)
			{
				this.hiddenFlags |= 2147483648U;
			}
		}
		else
		{
			this.hiddenFlags = 0U;
		}
		bool flag = PlayerScript.Pointer && (PlayerScript.PointerScript.CurrentPointerMode == PointerMode.Hands || Pointer.IsGizmoTool(PlayerScript.PointerScript.CurrentPointerMode));
		this.ThisMat.color = new Color(this.TriggerColour.r, this.TriggerColour.g, this.TriggerColour.b, flag ? 0.5f : 0f);
		if (this.revealHandToPlayers != 0U)
		{
			for (int i = 0; i < NetworkSingleton<PlayerManager>.Instance.PlayersList.Count; i++)
			{
				PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayersList[i];
				if (!(playerState.stringColor == this.TriggerLabel) && !(playerState.stringColor == "Black") && !(playerState.stringColor == "Grey") && this.revealHandToPlayers.IsSet(Colour.FlagFromColor(playerState.color)))
				{
					base.NPO.AddPeekIndicator(playerState.stringColor);
				}
			}
		}
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x00069323 File Offset: 0x00067523
	private void LateFixedUpdate()
	{
		if (this.bPositionHandObjects && !base.NPO.IsDestroyed)
		{
			this.PositionHandObjects();
		}
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x00069340 File Offset: 0x00067540
	private void PositionHandObjects()
	{
		if (this.ContainedNPOs.Count <= 0 && !this.Stash)
		{
			return;
		}
		this.ContainedNPOs.Sort(this.handSortComparer);
		this.MoveEndObjects();
		this.HandObjectsTotalXSize = 0f;
		this.TallestYSize = 0f;
		for (int i = 0; i < this.ContainedNPOs.Count + 1; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = (i < this.ContainedNPOs.Count) ? this.ContainedNPOs[i] : this.Stash;
			if (networkPhysicsObject)
			{
				if (!networkPhysicsObject.CurrentPlayerHand)
				{
					networkPhysicsObject.CurrentPlayerHand = this;
				}
				bool flag = networkPhysicsObject.SetShowThroughObscured(this.revealHandToPlayers);
				bool flag2 = networkPhysicsObject.SetObscured(this.HiderID, true, this.hiddenFlags, false);
				if (flag && !flag2)
				{
					networkPhysicsObject.UpdateVisiblity(false);
				}
				if (networkPhysicsObject.cardScript && networkPhysicsObject.fixedJoint && !networkPhysicsObject.cardScript.RootHeldByAnyone())
				{
					networkPhysicsObject.cardScript.ResetCard();
				}
				if (!(networkPhysicsObject == this.Stash))
				{
					Bounds bounds = networkPhysicsObject.GetBounds();
					Vector3 vector = networkPhysicsObject.GetBoundsCenterOffset();
					float num = Mathf.Abs(networkPhysicsObject.transform.eulerAngles.y - base.transform.eulerAngles.y) % 180f;
					if (num > 45f && num < 135f)
					{
						bounds.size = new Vector3(bounds.size.z + 0f, bounds.size.y, bounds.size.x);
					}
					else
					{
						bounds.size = new Vector3(bounds.size.x + 0f, bounds.size.y, bounds.size.z);
					}
					if (networkPhysicsObject.IsFaceDown)
					{
						vector *= -1f;
					}
					this.HandObjectsTotalXSize += bounds.size.x;
					if (bounds.size.z > this.TallestYSize)
					{
						this.TallestYSize = bounds.size.z;
					}
					HandZone.BoundsOffset boundsOffset = new HandZone.BoundsOffset(bounds, vector);
					if (i > this.BoundsOffsets.Count - 1)
					{
						this.BoundsOffsets.Add(boundsOffset);
					}
					else
					{
						this.BoundsOffsets[i] = boundsOffset;
					}
				}
			}
		}
		if (Network.isClient)
		{
			return;
		}
		if (this.Stash)
		{
			if (this.stashWasGizmoSelected)
			{
				if (!this.Stash.IsGizmoSelectedBySomebody)
				{
					this.stashWasGizmoSelected = false;
					if (!base.BoxCollider.bounds.Contains(this.Stash.transform.position))
					{
						this.Stash.IsGrabbable = true;
						this.Stash.CanBeHeldInHand = true;
						this.Stash.rigidbody.WakeUp();
						this.clearStash();
					}
				}
			}
			else if (this.Stash.IsGizmoSelectedBySomebody)
			{
				this.stashWasGizmoSelected = true;
			}
			else
			{
				if (!this.Stash.currentSmoothRotation.Moving)
				{
					Vector3 rotation = base.transform.rotation.eulerAngles + this.StashRotation;
					this.Stash.SetSmoothRotation(rotation, true, false, false, true, null, false);
				}
				Vector3 localScale = base.transform.localScale;
				Vector3 vector2;
				if (this.StashPosition != null)
				{
					vector2 = localScale.MultiplyParts(this.StashPosition.Value);
					vector2 *= 0.49f;
					vector2 = base.transform.rotation * vector2 + base.transform.position;
				}
				else
				{
					vector2 = new Vector3(0f, 0f, -base.transform.localScale.z * 0.4f);
					vector2 = base.transform.rotation * vector2 + base.transform.position;
					vector2.y = this.SurfacePoint.y + 1f;
				}
				this.Stash.SetSmoothPosition(vector2, false, true, false, false, null, false, false, null);
				this.Stash.rigidbody.useGravity = false;
				this.Stash.rigidbody.drag = 20f;
				this.Stash.rigidbody.angularDrag = 20f;
				if (!this.Stash.IsSmoothMoving)
				{
					this.Stash.rigidbody.Sleep();
				}
			}
		}
		if (this.ContainedNPOs.Count == 0)
		{
			return;
		}
		float num2 = 1f / (float)this.ContainedNPOs.Count;
		num2 *= Mathf.Clamp(this.HandBounds.size.x, 10f, 20f) / 10f;
		num2 = Mathf.Min(0.1f, num2);
		HandZone.BoundsOffset boundsOffset2 = this.BoundsOffsets[0];
		float x = boundsOffset2.bounds.extents.x;
		boundsOffset2 = this.BoundsOffsets[this.ContainedNPOs.Count - 1];
		float x2 = boundsOffset2.bounds.extents.x;
		float num3 = this.HandBounds.size.x - x - x2;
		float num4 = this.HandObjectsTotalXSize - x - x2;
		float num5;
		float num6;
		if ((num5 = num3 / num4) < 1f)
		{
			num6 = -this.HandBounds.size.x / 2f;
		}
		else
		{
			num6 = -this.HandObjectsTotalXSize / 2f;
			num5 = 1f;
		}
		num6 += x;
		bool flag3 = Singleton<HandSelectMode>.Instance.IsActive && this == HandZone.GetHandZone(PlayerScript.PointerScript.PointerColorLabel, 0, false);
		for (int j = 0; j < this.ContainedNPOs.Count; j++)
		{
			NetworkPhysicsObject networkPhysicsObject2 = this.ContainedNPOs[j];
			Quaternion setRotationFromGrabbable = NetworkSingleton<ManagerPhysicsObject>.Instance.GetSetRotationFromGrabbable(networkPhysicsObject2.gameObject);
			Vector3 centerOffset = this.BoundsOffsets[j].centerOffset;
			Bounds bounds2 = this.BoundsOffsets[j].bounds;
			float num7 = bounds2.extents.x * num5;
			float num8 = this.SurfacePoint.y + 2f + num2 * (float)j;
			if (j != 0)
			{
				num6 += num7;
			}
			float num9 = num6 + centerOffset.x;
			float num10 = num6 + centerOffset.z;
			Vector3 vector3 = new Vector3(base.transform.position.x + num9 * base.transform.right.normalized.x, num8 + centerOffset.y + bounds2.extents.y, base.transform.position.z + num10 * base.transform.right.normalized.z);
			if (flag3 && Singleton<HandSelectMode>.Instance.IsSelected(networkPhysicsObject2))
			{
				vector3 += base.transform.forward * (num7 * 0.2f);
			}
			num6 += num7;
			if (networkPhysicsObject2.IsHeldByNobody && networkPhysicsObject2.CurrentPlayerHand == this)
			{
				networkPhysicsObject2.SetCollision(false);
				if (!networkPhysicsObject2.currentSmoothRotation.Moving)
				{
					networkPhysicsObject2.SetSmoothRotation(setRotationFromGrabbable, false, false, false, true, null, false);
				}
				networkPhysicsObject2.SetSmoothPosition(vector3, false, true, false, false, null, false, false, null);
				networkPhysicsObject2.rigidbody.useGravity = false;
				networkPhysicsObject2.rigidbody.drag = 20f;
				networkPhysicsObject2.rigidbody.angularDrag = 20f;
				if (!networkPhysicsObject2.IsSmoothMoving)
				{
					networkPhysicsObject2.rigidbody.Sleep();
				}
			}
		}
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00069B48 File Offset: 0x00067D48
	public void MoveEndObjects()
	{
		for (int i = 0; i < this.EndObjects.Count; i++)
		{
			NetworkPhysicsObject npo = this.EndObjects[i].NPO;
			if (npo)
			{
				int num = this.ContainedNPOs.IndexOf(npo);
				if (num >= 0)
				{
					if (num == this.EndObjects[i].Index)
					{
						this.EndObjects.RemoveAt(i);
						i--;
					}
					else
					{
						this.ContainedNPOs.RemoveAt(num);
						this.ContainedNPOs.Add(npo);
					}
				}
			}
			else
			{
				this.EndObjects.RemoveAt(i);
				i--;
			}
		}
		for (int j = 0; j < this.EndObjects.Count; j++)
		{
			this.EndObjects[j].Index = this.ContainedNPOs.IndexOf(this.EndObjects[j].NPO);
		}
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00069C2C File Offset: 0x00067E2C
	public void PushHandSelectMode(HandSelectModeSettings settings)
	{
		if (this.HandSelectModeSettingsQueue == null)
		{
			this.HandSelectModeSettingsQueue = new List<HandSelectModeSettings>();
		}
		this.HandSelectModeSettingsQueue.Add(settings);
		this.startHandSelectMode();
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x00069C53 File Offset: 0x00067E53
	private void startHandSelectMode()
	{
		if (this.HasQueuedHandSelectMode && this.TriggerLabel == PlayerScript.PointerScript.PointerColorLabel)
		{
			Singleton<HandSelectMode>.Instance.StartHandSelectMode(this);
		}
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x00069C7F File Offset: 0x00067E7F
	public void EndHandSelectMode()
	{
		this.HandSelectModeSettingsQueue.RemoveAt(0);
		if (this.HandSelectModeSettingsQueue.Count > 0)
		{
			this.startHandSelectMode();
		}
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x00069CA1 File Offset: 0x00067EA1
	public void ClearHandSelectModes()
	{
		this.HandSelectModeSettingsQueue.Clear();
		Singleton<HandSelectMode>.Instance.Refresh();
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x00069CB8 File Offset: 0x00067EB8
	public void DealToEnd(NetworkPhysicsObject npo, bool faceUp = true, bool fast = false)
	{
		npo.CanBeHeldInHand = true;
		npo.SetHandObscuredRPC(this);
		npo.SetSmoothPosition(this.GetDealPosition(), false, fast, false, true, null, false, false, null);
		Quaternion quaternion = NetworkSingleton<ManagerPhysicsObject>.Instance.RotationFromIndex(this.GetRotationInt(npo));
		Quaternion rotation;
		if (faceUp)
		{
			rotation = quaternion;
		}
		else
		{
			Quaternion setRotationFromGrabbable = NetworkSingleton<ManagerPhysicsObject>.Instance.GetSetRotationFromGrabbable(npo.gameObject);
			rotation = Quaternion.Euler(new Vector3(setRotationFromGrabbable.eulerAngles.x, quaternion.eulerAngles.y, setRotationFromGrabbable.eulerAngles.z));
		}
		npo.SetSmoothRotation(rotation, true, false, false, true, null, false);
		base.StartCoroutine(this.AddEndObject(npo));
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x00069D6B File Offset: 0x00067F6B
	private IEnumerator AddEndObject(NetworkPhysicsObject npo)
	{
		HandZone.EndObjectIndex endObjectIndex = new HandZone.EndObjectIndex(npo);
		this.EndObjects.Add(endObjectIndex);
		yield return new WaitForSeconds(5f);
		if (this.EndObjects.Remove(endObjectIndex))
		{
			this.ResetHandObject(npo);
			npo.CurrentPlayerHand = null;
		}
		yield break;
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x00069D84 File Offset: 0x00067F84
	public float GetYHeight()
	{
		float num = this.SurfacePoint.y + 2f;
		for (int i = this.ContainedNPOs.Count - 1; i >= 0; i--)
		{
			NetworkPhysicsObject networkPhysicsObject = this.ContainedNPOs[i];
			if (!networkPhysicsObject.IsHeldBySomebody)
			{
				Vector3 vector = networkPhysicsObject.transform.position;
				if (networkPhysicsObject.currentSmoothPosition.Moving)
				{
					vector = networkPhysicsObject.currentSmoothPosition.TargetPosition;
				}
				float num2 = vector.y + 0.1f;
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x00069E0C File Offset: 0x0006800C
	public Vector3 GetDealPosition()
	{
		if (this.HasHandObjects)
		{
			for (int i = this.ContainedNPOs.Count - 1; i >= 0; i--)
			{
				NetworkPhysicsObject networkPhysicsObject = this.ContainedNPOs[i];
				Vector3 vector = networkPhysicsObject.currentSmoothPosition.Moving ? networkPhysicsObject.currentSmoothPosition.TargetPosition : networkPhysicsObject.transform.position;
				if (base.BoxCollider.bounds.Contains(vector))
				{
					return new Vector3(vector.x + 0.5f * base.transform.right.normalized.x, vector.y + 0.5f, vector.z + 0.5f * base.transform.right.normalized.z);
				}
			}
		}
		return new Vector3(base.transform.position.x, this.SurfacePoint.y + 2f, base.transform.position.z);
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x00069F20 File Offset: 0x00068120
	public Vector3 getRelativePosition(Transform hand, Transform card)
	{
		Vector3 lhs = card.position - hand.position;
		Vector3 zero = Vector3.zero;
		zero.x = Vector3.Dot(lhs, hand.right.normalized);
		zero.y = Vector3.Dot(lhs, hand.up.normalized);
		zero.z = Vector3.Dot(lhs, hand.forward.normalized);
		return zero;
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x00069F98 File Offset: 0x00068198
	public float getRelativePositionX(Transform hand, Transform card)
	{
		return Vector3.Dot(card.position - hand.position, hand.right.normalized);
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x00069FC9 File Offset: 0x000681C9
	public List<NetworkPhysicsObject> GetHandObjects(bool includeStash = false)
	{
		if (!includeStash || this.Stash == null)
		{
			return this.ContainedNPOs;
		}
		List<NetworkPhysicsObject> list = this.ContainedNPOs.ShallowCopy<NetworkPhysicsObject>();
		list.Add(this.Stash);
		return list;
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00069FFA File Offset: 0x000681FA
	public bool HasHandObjects
	{
		get
		{
			return this.ContainedNPOs.Count > 0;
		}
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0006A00C File Offset: 0x0006820C
	private void onObjectLeaveContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object)
	{
		if (Container == this.Stash && Container.deckScript.num_cards_ <= 1)
		{
			if (Container.deckScript.LastNPOInside)
			{
				this.Stash = Container.deckScript.LastNPOInside;
				return;
			}
			if (Container.deckScript.LastCard)
			{
				this.Stash = Container.deckScript.LastCard.GetComponent<NetworkPhysicsObject>();
			}
		}
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0006A081 File Offset: 0x00068281
	private void onObjectFinishSmoothMove(NetworkPhysicsObject NPO)
	{
		if (!Network.isServer)
		{
			return;
		}
		NPO == this.Stash;
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x0006A098 File Offset: 0x00068298
	public static List<HandZone> GetHandZones()
	{
		return HandZone.HandZones;
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x0006A0A0 File Offset: 0x000682A0
	public static int GetHandCount(string color)
	{
		int num = 0;
		for (int i = 0; i < HandZone.HandZones.Count; i++)
		{
			if (HandZone.HandZones[i].TriggerLabel == color)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x0006A0E4 File Offset: 0x000682E4
	public static HandZone GetHandZone(string color, int index = 0, bool skipSmall = false)
	{
		int num = 0;
		for (int i = 0; i < HandZone.HandZones.Count; i++)
		{
			HandZone handZone = HandZone.HandZones[i];
			if (handZone.TriggerLabel == color && (!skipSmall || handZone.HandBounds.size.magnitude > 1f))
			{
				if (num == index)
				{
					return handZone;
				}
				num++;
			}
		}
		return null;
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x0006A14C File Offset: 0x0006834C
	public int GetHandIndex(bool skipSmall = false)
	{
		int num = 0;
		for (int i = 0; i < HandZone.HandZones.Count; i++)
		{
			HandZone handZone = HandZone.HandZones[i];
			if (handZone.TriggerLabel == this.TriggerLabel && (!skipSmall || handZone.HandBounds.size.magnitude > 1f))
			{
				if (handZone == this)
				{
					return num;
				}
				num++;
			}
		}
		return -1;
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0006A1BC File Offset: 0x000683BC
	public static GameObject GetHand(string color, int index = 0)
	{
		HandZone handZone = HandZone.GetHandZone(color, index, false);
		if (!handZone)
		{
			return null;
		}
		return handZone.gameObject;
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x0006A1E4 File Offset: 0x000683E4
	public static HandZone GetStartHandZone()
	{
		float num = float.MaxValue;
		HandZone result = null;
		for (int i = 0; i < HandZone.HandZones.Count; i++)
		{
			HandZone handZone = HandZone.HandZones[i];
			float num2 = HandZone.HandAngle(new Vector3(0.95f, 0f, -1f), handZone.transform.position);
			if (num2 < num)
			{
				num = num2;
				result = handZone;
			}
		}
		return result;
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x0006A24C File Offset: 0x0006844C
	public static GameObject GetStartHand()
	{
		HandZone startHandZone = HandZone.GetStartHandZone();
		if (!startHandZone)
		{
			return null;
		}
		return startHandZone.gameObject;
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x0006A270 File Offset: 0x00068470
	public static HandZone GetHandZoneWhichContains(NetworkPhysicsObject npo)
	{
		for (int i = 0; i < HandZone.HandZones.Count; i++)
		{
			HandZone handZone = HandZone.HandZones[i];
			if (handZone.ContainedNPOs.Contains(npo))
			{
				return handZone;
			}
		}
		return null;
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x0006A2B0 File Offset: 0x000684B0
	public static float HandAngle(Vector3 a, Vector3 b)
	{
		float num = Vector3.Angle(a, b);
		if (Mathf.Sign(Vector3.Cross(a, b).y) < 0f)
		{
			num = 360f - num;
		}
		return num;
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x0006A2E8 File Offset: 0x000684E8
	public static int GetUniqueHandCount()
	{
		int num = 0;
		List<string> list = new List<string>();
		foreach (HandZone handZone in HandZone.HandZones)
		{
			if (!list.Contains(handZone.TriggerLabel))
			{
				list.Add(handZone.TriggerLabel);
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x0006A35C File Offset: 0x0006855C
	public void Creation()
	{
		if (!base.NPO)
		{
			return;
		}
		HandZone.HandZones.Add(this);
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x0006A377 File Offset: 0x00068577
	public void Destruction()
	{
		if (!base.NPO)
		{
			return;
		}
		HandZone.HandZones.Remove(this);
	}

	// Token: 0x0400098F RID: 2447
	private string _TriggerLabel;

	// Token: 0x04000990 RID: 2448
	[NonSerialized]
	public List<HandSelectModeSettings> HandSelectModeSettingsQueue;

	// Token: 0x04000991 RID: 2449
	[NonSerialized]
	public NetworkPhysicsObject Stash;

	// Token: 0x04000992 RID: 2450
	[NonSerialized]
	public Vector3? StashPosition;

	// Token: 0x04000993 RID: 2451
	[NonSerialized]
	public Vector3 StashRotation = new Vector3(0f, 90f, 180f);

	// Token: 0x04000994 RID: 2452
	private Colour TriggerColour;

	// Token: 0x04000995 RID: 2453
	public Colour TriggerHighlightColour;

	// Token: 0x04000996 RID: 2454
	private uint hiddenFlags;

	// Token: 0x04000997 RID: 2455
	public bool bPositionHandObjects = true;

	// Token: 0x04000998 RID: 2456
	private Material ThisMat;

	// Token: 0x04000999 RID: 2457
	private HandZone.HandSortComparer handSortComparer;

	// Token: 0x0400099A RID: 2458
	private uint revealHandToPlayers;

	// Token: 0x0400099B RID: 2459
	public Vector3 SurfacePoint;

	// Token: 0x0400099C RID: 2460
	public int RotationInt;

	// Token: 0x0400099D RID: 2461
	public Bounds HandBounds;

	// Token: 0x0400099E RID: 2462
	public Renderer renderer;

	// Token: 0x040009A0 RID: 2464
	private List<HandZone.BoundsOffset> BoundsOffsets = new List<HandZone.BoundsOffset>();

	// Token: 0x040009A1 RID: 2465
	public float HandObjectsTotalXSize;

	// Token: 0x040009A2 RID: 2466
	public float TallestYSize;

	// Token: 0x040009A3 RID: 2467
	private bool stashWasGizmoSelected;

	// Token: 0x040009A4 RID: 2468
	private List<HandZone.EndObjectIndex> EndObjects = new List<HandZone.EndObjectIndex>();

	// Token: 0x02000637 RID: 1591
	public struct BoundsOffset
	{
		// Token: 0x06003AEF RID: 15087 RVA: 0x00175D77 File Offset: 0x00173F77
		public BoundsOffset(Bounds bounds, Vector3 centerOffset)
		{
			this.bounds = bounds;
			this.centerOffset = centerOffset;
		}

		// Token: 0x04002748 RID: 10056
		public Bounds bounds;

		// Token: 0x04002749 RID: 10057
		public Vector3 centerOffset;
	}

	// Token: 0x02000638 RID: 1592
	private class EndObjectIndex
	{
		// Token: 0x06003AF0 RID: 15088 RVA: 0x00175D87 File Offset: 0x00173F87
		public EndObjectIndex(NetworkPhysicsObject NPO)
		{
			this.NPO = NPO;
			this.Index = -1;
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x00175D9D File Offset: 0x00173F9D
		public EndObjectIndex(NetworkPhysicsObject NPO, int Index)
		{
			this.NPO = NPO;
			this.Index = Index;
		}

		// Token: 0x0400274A RID: 10058
		public NetworkPhysicsObject NPO;

		// Token: 0x0400274B RID: 10059
		public int Index;
	}

	// Token: 0x02000639 RID: 1593
	public class HandSortComparer : IComparer<NetworkPhysicsObject>
	{
		// Token: 0x06003AF2 RID: 15090 RVA: 0x00175DB3 File Offset: 0x00173FB3
		public HandSortComparer(HandZone handZone)
		{
			this.handZone = handZone;
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x00175DC4 File Offset: 0x00173FC4
		public int Compare(NetworkPhysicsObject x, NetworkPhysicsObject y)
		{
			Transform transform = x.transform;
			Transform transform2 = y.transform;
			float relativePositionX = this.handZone.getRelativePositionX(this.handZone.transform, transform);
			float relativePositionX2 = this.handZone.getRelativePositionX(this.handZone.transform, transform2);
			if (relativePositionX < relativePositionX2)
			{
				return -1;
			}
			if (relativePositionX2 < relativePositionX)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0400274C RID: 10060
		private HandZone handZone;
	}
}
