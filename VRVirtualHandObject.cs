using System;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

// Token: 0x0200037E RID: 894
public class VRVirtualHandObject : Singleton<VRVirtualHandObject>
{
	// Token: 0x060029EF RID: 10735 RVA: 0x0012B782 File Offset: 0x00129982
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnZoneAdd += this.ObjectAdded;
		EventManager.OnZoneRemove += this.ObjectRemoved;
		EventManager.OnChangePlayerColor += this.OnPlayerChangedColor;
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x0012B7BD File Offset: 0x001299BD
	private void OnDestroy()
	{
		EventManager.OnZoneAdd -= this.ObjectAdded;
		EventManager.OnZoneRemove -= this.ObjectRemoved;
		EventManager.OnChangePlayerColor -= this.OnPlayerChangedColor;
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x0012B7F4 File Offset: 0x001299F4
	private void LateUpdate()
	{
		bool flag = false;
		foreach (KeyValuePair<NetworkPhysicsObject, VRVirtualHandObject.VirtualHandObject> keyValuePair in this.HandObjects)
		{
			if (keyValuePair.Key == null)
			{
				flag = true;
			}
			else
			{
				this.UpdateVHO(keyValuePair.Key, keyValuePair.Value);
			}
		}
		if (flag)
		{
			this.clean = new Dictionary<NetworkPhysicsObject, VRVirtualHandObject.VirtualHandObject>();
			foreach (KeyValuePair<NetworkPhysicsObject, VRVirtualHandObject.VirtualHandObject> keyValuePair2 in this.HandObjects)
			{
				if (keyValuePair2.Key == null)
				{
					UnityEngine.Object.Destroy(keyValuePair2.Value.copy);
				}
				else
				{
					this.clean[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			this.HandObjects.Clear();
			this.HandObjects = this.clean;
		}
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x0012B908 File Offset: 0x00129B08
	private void UpdateVHO(NetworkPhysicsObject npo, VRVirtualHandObject.VirtualHandObject vho)
	{
		GameObject copy = vho.copy;
		HandZone hand = vho.hand;
		copy.transform.localPosition = LibVector.NormalizedPosition(npo.transform.position, hand.transform.position, hand.transform.rotation) * VRVirtualHandObject.VirtualHandScale;
		copy.transform.localRotation = LibVector.NormalizedRotation(npo.transform.rotation, hand.transform.rotation);
		copy.transform.localScale = npo.transform.localScale * VRVirtualHandObject.VirtualHandScale;
		Color? color = new Color?(npo.highlighter.color);
		if (color != null)
		{
			Highlighter highlighter = copy.GetComponent<Highlighter>();
			if (!highlighter)
			{
				highlighter = copy.AddComponent<Highlighter>();
			}
			highlighter.On(color.Value);
		}
		copy.GetComponent<MeshRenderer>().enabled = npo.GetComponent<MeshRenderer>().enabled;
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x0012B9F8 File Offset: 0x00129BF8
	private void OnEnable()
	{
		HandZone handZone = HandZone.GetHandZone(Colour.MyColorLabel(), 0, false);
		if (handZone)
		{
			foreach (NetworkPhysicsObject npo in handZone.GetHandObjects(false))
			{
				this.Add(npo, handZone);
			}
		}
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x0012BA64 File Offset: 0x00129C64
	private void OnDisable()
	{
		foreach (KeyValuePair<NetworkPhysicsObject, VRVirtualHandObject.VirtualHandObject> keyValuePair in this.HandObjects)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.copy);
		}
		this.HandObjects.Clear();
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x0012BACC File Offset: 0x00129CCC
	private void OnPlayerChangedColor(Color newColor, int id)
	{
		if (id != NetworkID.ID)
		{
			return;
		}
		this.OnDisable();
		this.OnEnable();
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x0012BAE4 File Offset: 0x00129CE4
	private void Add(NetworkPhysicsObject npo, HandZone hand)
	{
		this.VirtualHandCollider.transform.localScale = hand.transform.lossyScale * VRVirtualHandObject.VirtualHandScale;
		VRVirtualHandObject.VirtualHandObject value;
		if (!this.HandObjects.TryGetValue(npo, out value))
		{
			ObjectState os = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(npo);
			GameObject gameObject = new GameObject();
			gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, true, false);
			gameObject.transform.parent = base.transform;
			gameObject.GetComponent<Collider>().enabled = false;
			if (gameObject.GetComponent<CardScript>())
			{
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject, gameObject.GetComponent<CardScript>().card_id_, npo.MostRecentCardSetup.backID, npo.MostRecentCardSetup.isHidden);
			}
			value = new VRVirtualHandObject.VirtualHandObject(hand, npo, gameObject);
			this.HandObjects[npo] = value;
		}
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x0012BBB8 File Offset: 0x00129DB8
	private void ObjectAdded(NetworkPhysicsObject zone, NetworkPhysicsObject npo)
	{
		HandZone handZone = zone.handZone;
		if (!handZone)
		{
			return;
		}
		string text = Colour.MyColorLabel();
		if (handZone.TriggerLabel != text)
		{
			return;
		}
		HandZone handZone2 = HandZone.GetHandZone(text, 0, false);
		if (handZone == handZone2)
		{
			this.Add(npo, handZone);
		}
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x0012BC04 File Offset: 0x00129E04
	private void ObjectRemoved(NetworkPhysicsObject zone, NetworkPhysicsObject npo)
	{
		HandZone handZone = zone.handZone;
		if (!handZone)
		{
			return;
		}
		string b = Colour.MyColorLabel();
		if (handZone.TriggerLabel != b)
		{
			return;
		}
		VRVirtualHandObject.VirtualHandObject virtualHandObject;
		if (this.HandObjects.TryGetValue(npo, out virtualHandObject))
		{
			UnityEngine.Object.Destroy(virtualHandObject.copy);
			this.HandObjects.Remove(npo);
		}
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x0012BC60 File Offset: 0x00129E60
	public void ReplaceNPOWithVirtualObject(NetworkPhysicsObject npo)
	{
		VRVirtualHandObject.VirtualHandObject vho;
		if (!this.HandObjects.TryGetValue(npo, out vho))
		{
			npo.DisableFastDragWhileAnimating();
			HandZone handZone = HandZone.GetHandZone(Colour.MyColorLabel(), 0, false);
			this.Add(npo, handZone);
			vho = this.HandObjects[npo];
		}
		this.UpdateVHO(npo, vho);
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x0012BCB0 File Offset: 0x00129EB0
	public void ReplaceVirtualObjectWithNPO(NetworkPhysicsObject npo)
	{
		VRVirtualHandObject.VirtualHandObject virtualHandObject;
		if (this.HandObjects.TryGetValue(npo, out virtualHandObject))
		{
			npo.DisableFastDragWhileAnimating();
			npo.SetPositionHeld(virtualHandObject.copy.transform.position);
			npo.SetRotationHeld(virtualHandObject.copy.transform.rotation.eulerAngles);
			UnityEngine.Object.Destroy(this.HandObjects[npo].copy);
			this.HandObjects.Remove(npo);
		}
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x0012BD2C File Offset: 0x00129F2C
	public Vector3 ActualPositionFromVirtualHandPosition(Vector3 position)
	{
		HandZone handZone = HandZone.GetHandZone(Colour.MyColorLabel(), 0, false);
		return LibVector.TransformedPosition(LibVector.NormalizedPosition(position, base.transform.position, base.transform.rotation) / VRVirtualHandObject.VirtualHandScale, handZone.transform.position, handZone.transform.rotation);
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x0012BD88 File Offset: 0x00129F88
	public Quaternion ActualRotationFromVirtualHandRotation(Quaternion rotation)
	{
		HandZone handZone = HandZone.GetHandZone(Colour.MyColorLabel(), 0, false);
		return LibVector.RecontainerRotation(rotation, base.transform.rotation, handZone.transform.rotation);
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x0012BDC0 File Offset: 0x00129FC0
	public void Refresh()
	{
		HandZone handZone = HandZone.GetHandZone(Colour.MyColorLabel(), 0, false);
		if (handZone)
		{
			this.VirtualHandCollider.transform.localScale = handZone.transform.lossyScale * VRVirtualHandObject.VirtualHandScale;
		}
	}

	// Token: 0x04001CA7 RID: 7335
	public Collider VirtualHandCollider;

	// Token: 0x04001CA8 RID: 7336
	public static float VirtualHandScale = 1f;

	// Token: 0x04001CA9 RID: 7337
	public Dictionary<NetworkPhysicsObject, VRVirtualHandObject.VirtualHandObject> HandObjects = new Dictionary<NetworkPhysicsObject, VRVirtualHandObject.VirtualHandObject>();

	// Token: 0x04001CAA RID: 7338
	private Dictionary<NetworkPhysicsObject, VRVirtualHandObject.VirtualHandObject> clean;

	// Token: 0x020007B6 RID: 1974
	public class VirtualHandObject
	{
		// Token: 0x06003FA5 RID: 16293 RVA: 0x00182067 File Offset: 0x00180267
		public VirtualHandObject(HandZone hand, NetworkPhysicsObject npo, GameObject copy)
		{
			this.hand = hand;
			this.copy = copy;
		}

		// Token: 0x04002D2E RID: 11566
		public HandZone hand;

		// Token: 0x04002D2F RID: 11567
		public GameObject copy;
	}
}
