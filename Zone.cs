using System;
using System.Collections.Generic;
using HighlightingSystem;
using NewNet;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class Zone : NetworkBehavior
{
	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06002A55 RID: 10837 RVA: 0x0012DAAD File Offset: 0x0012BCAD
	// (set) Token: 0x06002A56 RID: 10838 RVA: 0x0012DAB5 File Offset: 0x0012BCB5
	public NetworkPhysicsObject NPO { get; private set; }

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06002A57 RID: 10839 RVA: 0x0012DABE File Offset: 0x0012BCBE
	// (set) Token: 0x06002A58 RID: 10840 RVA: 0x0012DAC6 File Offset: 0x0012BCC6
	public BoxCollider BoxCollider { get; private set; }

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06002A59 RID: 10841 RVA: 0x0012DACF File Offset: 0x0012BCCF
	// (set) Token: 0x06002A5A RID: 10842 RVA: 0x0012DAD7 File Offset: 0x0012BCD7
	public Highlighter Highlighter { get; private set; }

	// Token: 0x06002A5B RID: 10843 RVA: 0x0012DAE0 File Offset: 0x0012BCE0
	protected virtual void Awake()
	{
		this.render = base.GetComponentInChildren<Renderer>();
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		this.BoxCollider = base.GetComponent<BoxCollider>();
		if (this.NPO)
		{
			this.NPO.IsLocked = true;
			this.NPO.IsGrabbable = false;
			this.Highlighter = this.NPO.highlighter;
			this.Highlighter.overlay = false;
		}
		EventManager.OnLateFixedUpdate += this.CheckQueuedRemoveObjects;
		EventManager.OnNetworkObjectDestroy += this.OnNetworkObjectDestroy;
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void Start()
	{
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x0012DB75 File Offset: 0x0012BD75
	protected virtual void OnDestroy()
	{
		this.RemoveAllObjects();
		EventManager.OnLateFixedUpdate -= this.CheckQueuedRemoveObjects;
		EventManager.OnNetworkObjectDestroy -= this.OnNetworkObjectDestroy;
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x0012DBA0 File Offset: 0x0012BDA0
	protected virtual void Update()
	{
		this.CleanUpNulls();
		this.CheckValids();
		if (PlayerScript.PointerScript)
		{
			bool flag = false;
			if (this.Highlighter && !PlayerScript.PointerScript.ClickingWhileInHandSelectMode && this.highlightColor != Color.clear)
			{
				using (List<NetworkPhysicsObject>.Enumerator enumerator = this.ContainedNPOs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HeldByPlayerID == NetworkID.ID)
						{
							this.Highlighter.On(this.highlightColor);
							flag = true;
							break;
						}
					}
				}
			}
			this.render.enabled = (Pointer.IsGizmoTool(PlayerScript.PointerScript.CurrentPointerMode) || PlayerScript.PointerScript.CurrentPointerMode == this.PointerModeVisibility || this.PointerModeVisibility == PointerMode.Hidden || (flag && this.PointerModeVisibility == PointerMode.Hands));
			base.gameObject.layer = ((Pointer.IsGizmoTool(PlayerScript.PointerScript.CurrentPointerMode) || PlayerScript.PointerScript.CurrentPointerMode == this.PointerModeVisibility) ? 18 : 2);
			return;
		}
		this.render.enabled = (this.PointerModeVisibility == PointerMode.Hidden);
		base.gameObject.layer = 2;
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x0012DCF4 File Offset: 0x0012BEF4
	private void CleanUpNulls()
	{
		foreach (NetworkPhysicsObject networkPhysicsObject in this.ContainedNPOs)
		{
			if (networkPhysicsObject == null)
			{
				this.reusableNPOs.Add(networkPhysicsObject);
			}
		}
		foreach (NetworkPhysicsObject npo in this.reusableNPOs)
		{
			this.InternalRemoveObject(npo);
		}
		this.reusableNPOs.Clear();
		foreach (NetworkPhysicsObject networkPhysicsObject2 in this.InvalidContainedNPOs)
		{
			if (networkPhysicsObject2 == null)
			{
				this.reusableNPOs.Add(networkPhysicsObject2);
			}
		}
		foreach (NetworkPhysicsObject npo2 in this.reusableNPOs)
		{
			this.InternalRemoveObject(npo2);
		}
		this.reusableNPOs.Clear();
		foreach (KeyValuePair<Collider, NetworkPhysicsObject> keyValuePair in this.ContainedColliderNPOs)
		{
			if (keyValuePair.Key == null)
			{
				this.reusableColliderNPOs.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		foreach (KeyValuePair<Collider, NetworkPhysicsObject> keyValuePair2 in this.reusableColliderNPOs)
		{
			this.InternalRemoveObject(keyValuePair2.Key, keyValuePair2.Value);
		}
		this.reusableColliderNPOs.Clear();
		foreach (KeyValuePair<Collider, NetworkPhysicsObject> keyValuePair3 in this.InvalidContainedColliderNPOs)
		{
			if (keyValuePair3.Key == null)
			{
				this.reusableColliderNPOs.Add(keyValuePair3.Key, keyValuePair3.Value);
			}
		}
		foreach (KeyValuePair<Collider, NetworkPhysicsObject> keyValuePair4 in this.reusableColliderNPOs)
		{
			this.InternalRemoveObject(keyValuePair4.Key, keyValuePair4.Value);
		}
		this.reusableColliderNPOs.Clear();
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x0012DFC8 File Offset: 0x0012C1C8
	private void CheckValids()
	{
		foreach (NetworkPhysicsObject networkPhysicsObject in this.ContainedNPOs)
		{
			if (!this.ValidateAddObject(networkPhysicsObject))
			{
				this.reusableNPOs.Add(networkPhysicsObject);
			}
		}
		foreach (NetworkPhysicsObject npo in this.reusableNPOs)
		{
			this.Swap(npo, this.ContainedColliderNPOs, this.ContainedNPOs, this.InvalidContainedColliderNPOs, this.InvalidContainedNPOs);
			this.OnRemoveObject(npo);
		}
		this.reusableNPOs.Clear();
		foreach (NetworkPhysicsObject networkPhysicsObject2 in this.InvalidContainedNPOs)
		{
			if (this.ValidateAddObject(networkPhysicsObject2))
			{
				this.reusableNPOs.Add(networkPhysicsObject2);
			}
		}
		foreach (NetworkPhysicsObject npo2 in this.reusableNPOs)
		{
			this.Swap(npo2, this.InvalidContainedColliderNPOs, this.InvalidContainedNPOs, this.ContainedColliderNPOs, this.ContainedNPOs);
			this.OnAddObject(npo2);
		}
		this.reusableNPOs.Clear();
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x0012E154 File Offset: 0x0012C354
	private void CheckQueuedRemoveObjects()
	{
		foreach (KeyValuePair<Collider, NetworkPhysicsObject> keyValuePair in this.queuedRemoveObjects)
		{
			this.InternalRemoveObject(keyValuePair.Key, keyValuePair.Value);
		}
		this.queuedRemoveObjects.Clear();
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x0012E1C0 File Offset: 0x0012C3C0
	private void Swap(NetworkPhysicsObject npo, Dictionary<Collider, NetworkPhysicsObject> fromColliderNPOs, List<NetworkPhysicsObject> fromNPOs, Dictionary<Collider, NetworkPhysicsObject> toColliderNPOs, List<NetworkPhysicsObject> toNPOs)
	{
		foreach (KeyValuePair<Collider, NetworkPhysicsObject> keyValuePair in fromColliderNPOs)
		{
			if (keyValuePair.Value == npo)
			{
				this.reusableColliders.Add(keyValuePair.Key);
			}
		}
		foreach (Collider key in this.reusableColliders)
		{
			fromColliderNPOs.Remove(key);
			toColliderNPOs.Add(key, npo);
		}
		fromNPOs.Remove(npo);
		toNPOs.Add(npo);
		this.reusableColliders.Clear();
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x0012E290 File Offset: 0x0012C490
	protected void AddObject(Collider addCollider, NetworkPhysicsObject npo)
	{
		if (addCollider.CompareTag("Decal"))
		{
			return;
		}
		this.InternalAddObject(addCollider, npo);
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x0012E2AC File Offset: 0x0012C4AC
	private bool InternalAddObject(Collider addCollider, NetworkPhysicsObject npo)
	{
		if (this.queuedRemoveObjects.ContainsKey(addCollider))
		{
			this.queuedRemoveObjects.Remove(addCollider);
			return false;
		}
		bool flag = this.ValidateAddObject(npo);
		if ((flag && !this.InvalidContainedNPOs.Contains(npo)) || (!flag && this.ContainedNPOs.Contains(npo)))
		{
			if (this.ContainedColliderNPOs.TryAddUnique(addCollider, npo) && this.ContainedNPOs.TryAddUnique(npo))
			{
				this.OnAddObject(npo);
				return true;
			}
		}
		else if (this.InvalidContainedColliderNPOs.TryAddUnique(addCollider, npo))
		{
			this.InvalidContainedNPOs.TryAddUnique(npo);
		}
		return false;
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x0012E343 File Offset: 0x0012C543
	protected virtual void OnAddObject(NetworkPhysicsObject npo)
	{
		EventManager.TriggerZoneAdd(this.NPO, npo);
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x0012E351 File Offset: 0x0012C551
	protected void RemoveObject(Collider addCollider, NetworkPhysicsObject npo)
	{
		if (addCollider.CompareTag("Decal"))
		{
			return;
		}
		if (this.queuedRemoveObjects.ContainsKey(addCollider))
		{
			return;
		}
		this.queuedRemoveObjects.Add(addCollider, npo);
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x0012E380 File Offset: 0x0012C580
	private bool InternalRemoveObject(Collider addCollider, NetworkPhysicsObject npo)
	{
		if (this.ContainedColliderNPOs.Remove(addCollider))
		{
			if (!this.ContainedColliderNPOs.ContainsValue(npo) && this.ContainedNPOs.Remove(npo))
			{
				this.OnRemoveObject(npo);
				return true;
			}
		}
		else if (this.InvalidContainedColliderNPOs.Remove(addCollider) && !this.InvalidContainedColliderNPOs.ContainsValue(npo))
		{
			this.InvalidContainedNPOs.Remove(npo);
		}
		return false;
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x0012E3EC File Offset: 0x0012C5EC
	private bool InternalRemoveObject(NetworkPhysicsObject npo)
	{
		if (this.ContainedNPOs.Remove(npo))
		{
			foreach (KeyValuePair<Collider, NetworkPhysicsObject> keyValuePair in this.ContainedColliderNPOs)
			{
				if (keyValuePair.Value == npo)
				{
					this.reusableColliders.Add(keyValuePair.Key);
				}
			}
			foreach (Collider key in this.reusableColliders)
			{
				this.ContainedColliderNPOs.Remove(key);
			}
			this.reusableColliders.Clear();
			this.OnRemoveObject(npo);
			return true;
		}
		return false;
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x0012E4C8 File Offset: 0x0012C6C8
	protected virtual void OnRemoveObject(NetworkPhysicsObject npo)
	{
		EventManager.TriggerZoneRemove(this.NPO, npo);
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x0012E4D6 File Offset: 0x0012C6D6
	protected virtual bool ValidateAddObject(NetworkPhysicsObject npo)
	{
		return npo != null && !npo.IsDestroyed && npo.IsGrabbable && this.NPO.TagsAllowActingUpon(npo);
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x0012E4FF File Offset: 0x0012C6FF
	private void OnNetworkObjectDestroy(NetworkPhysicsObject npo)
	{
		this.InternalRemoveObject(npo);
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x0012E50C File Offset: 0x0012C70C
	protected void RemoveAllObjects()
	{
		foreach (NetworkPhysicsObject item in this.ContainedNPOs)
		{
			this.reusableNPOs.Add(item);
		}
		foreach (NetworkPhysicsObject npo in this.reusableNPOs)
		{
			this.InternalRemoveObject(npo);
		}
		this.reusableNPOs.Clear();
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x0012E5B4 File Offset: 0x0012C7B4
	protected void ResetZone()
	{
		this.BoxCollider.enabled = false;
		this.BoxCollider.enabled = true;
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x0012E5CE File Offset: 0x0012C7CE
	public bool IsNPOCollidersInZone(NetworkPhysicsObject npo)
	{
		return this.ContainedNPOs.Contains(npo) || this.InvalidContainedNPOs.Contains(npo);
	}

	// Token: 0x04001CC7 RID: 7367
	public PointerMode PointerModeVisibility = PointerMode.None;

	// Token: 0x04001CCB RID: 7371
	protected Renderer render;

	// Token: 0x04001CCC RID: 7372
	protected Color highlightColor;

	// Token: 0x04001CCD RID: 7373
	public readonly Dictionary<Collider, NetworkPhysicsObject> ContainedColliderNPOs = new Dictionary<Collider, NetworkPhysicsObject>();

	// Token: 0x04001CCE RID: 7374
	public readonly List<NetworkPhysicsObject> ContainedNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04001CCF RID: 7375
	public readonly Dictionary<Collider, NetworkPhysicsObject> InvalidContainedColliderNPOs = new Dictionary<Collider, NetworkPhysicsObject>();

	// Token: 0x04001CD0 RID: 7376
	public readonly List<NetworkPhysicsObject> InvalidContainedNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04001CD1 RID: 7377
	private readonly List<NetworkPhysicsObject> reusableNPOs = new List<NetworkPhysicsObject>();

	// Token: 0x04001CD2 RID: 7378
	private readonly List<Collider> reusableColliders = new List<Collider>();

	// Token: 0x04001CD3 RID: 7379
	private readonly Dictionary<Collider, NetworkPhysicsObject> reusableColliderNPOs = new Dictionary<Collider, NetworkPhysicsObject>();

	// Token: 0x04001CD4 RID: 7380
	private readonly Dictionary<Collider, NetworkPhysicsObject> queuedRemoveObjects = new Dictionary<Collider, NetworkPhysicsObject>();
}
