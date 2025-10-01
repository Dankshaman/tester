using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class HandCameraRender : MonoBehaviour
{
	// Token: 0x06000F33 RID: 3891 RVA: 0x00067FE8 File Offset: 0x000661E8
	private void OnPreCull()
	{
		if (!this.PreCull)
		{
			return;
		}
		this.PreCull = false;
		HandZone hand = this.handCamera.GetHand();
		if (hand == null)
		{
			return;
		}
		bool handInInteraction = this.handCamera.handInInteraction;
		Bounds bounds = hand.renderer.bounds;
		List<NetworkPhysicsObject> handObjects = hand.GetHandObjects(false);
		for (int i = 0; i < handObjects.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = handObjects[i];
			if (!(networkPhysicsObject == null) && bounds.Contains(networkPhysicsObject.transform.position))
			{
				this.CheckAddLayerNPO(networkPhysicsObject);
			}
		}
		UIPanel.ForceUpdate(this.PanelObjects);
		if (HoverScript.HoverCamera == this.handCamera.camera && PlayerScript.PointerScript != null && PlayerScript.PointerScript.ReferenceFollower != null)
		{
			GameObject referenceFollower = PlayerScript.PointerScript.ReferenceFollower;
			this.pointerPosition = referenceFollower.transform.position;
			this.pointerRotation = referenceFollower.transform.eulerAngles;
			referenceFollower.transform.position = new Vector3(this.pointerPosition.x, hand.transform.position.y + 24f, this.pointerPosition.z);
			referenceFollower.transform.eulerAngles = new Vector3(0f, hand.transform.eulerAngles.y - 180f, 0f);
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x00068164 File Offset: 0x00066364
	private void OnPostRender()
	{
		if (this.PreCull)
		{
			return;
		}
		this.PreCull = true;
		for (int i = 0; i < this.LayerObjects.Count; i++)
		{
			GridCameraRender.CameraLayerObject cameraLayerObject = this.LayerObjects[i];
			GameObject go = cameraLayerObject.go;
			int layer = cameraLayerObject.layer;
			go.layer = layer;
			if (this.RaiseObjects.ContainsKey(go))
			{
				this.RaiseObjects[go].Reset();
			}
		}
		UIPanel.ForceUpdate(this.PanelObjects);
		if (HoverScript.HoverCamera == this.handCamera.camera && PlayerScript.PointerScript != null && PlayerScript.PointerScript.ReferenceFollower != null)
		{
			GameObject referenceFollower = PlayerScript.PointerScript.ReferenceFollower;
			referenceFollower.transform.position = this.pointerPosition;
			referenceFollower.transform.eulerAngles = this.pointerRotation;
		}
		this.RaiseObjects.Clear();
		this.LayerObjects.Clear();
		this.PanelObjects.Clear();
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x0006826C File Offset: 0x0006646C
	private void CheckAddLayerNPO(NetworkPhysicsObject npo)
	{
		List<Renderer> renderers = npo.Renderers;
		for (int i = 0; i < renderers.Count; i++)
		{
			if (renderers[i])
			{
				this.AddLayerObject(renderers[i].gameObject);
			}
		}
		foreach (UIPanel uipanel in npo.UIPanels)
		{
			if (uipanel)
			{
				this.AddLayerObject(uipanel.gameObject);
				this.PanelObjects.Add(uipanel);
			}
		}
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00068310 File Offset: 0x00066510
	private void AddLayerObject(GameObject layerObject)
	{
		this.LayerObjects.Add(new GridCameraRender.CameraLayerObject(layerObject));
		layerObject.layer = 19;
	}

	// Token: 0x04000975 RID: 2421
	public HandCamera handCamera;

	// Token: 0x04000976 RID: 2422
	private List<GridCameraRender.CameraLayerObject> LayerObjects = new List<GridCameraRender.CameraLayerObject>(100);

	// Token: 0x04000977 RID: 2423
	private bool PreCull = true;

	// Token: 0x04000978 RID: 2424
	private Vector3 pointerPosition;

	// Token: 0x04000979 RID: 2425
	private Vector3 pointerRotation;

	// Token: 0x0400097A RID: 2426
	private Dictionary<GameObject, HandCameraRender.HoverState> RaiseObjects = new Dictionary<GameObject, HandCameraRender.HoverState>();

	// Token: 0x0400097B RID: 2427
	private List<UIPanel> PanelObjects = new List<UIPanel>();

	// Token: 0x02000636 RID: 1590
	private struct HoverState
	{
		// Token: 0x06003AED RID: 15085 RVA: 0x00175CE8 File Offset: 0x00173EE8
		public HoverState(NetworkPhysicsObject npo)
		{
			this.npo = npo;
			this.position = npo.transform.position;
			this.rotation = npo.transform.rotation;
			this.scale = npo.transform.localScale;
			this.sleeping = (!npo.IsMoving && !npo.IsSmoothMoving);
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x00175D49 File Offset: 0x00173F49
		public void Reset()
		{
			this.npo.transform.position = this.position;
			this.npo.transform.rotation = this.rotation;
		}

		// Token: 0x04002743 RID: 10051
		public NetworkPhysicsObject npo;

		// Token: 0x04002744 RID: 10052
		public Vector3 position;

		// Token: 0x04002745 RID: 10053
		public Quaternion rotation;

		// Token: 0x04002746 RID: 10054
		public Vector3 scale;

		// Token: 0x04002747 RID: 10055
		public bool sleeping;
	}
}
