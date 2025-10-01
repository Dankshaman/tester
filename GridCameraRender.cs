using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class GridCameraRender : MonoBehaviour
{
	// Token: 0x06000F17 RID: 3863 RVA: 0x00066EA8 File Offset: 0x000650A8
	private void OnPreCull()
	{
		if (!NetworkSingleton<GridOptions>.Instance.gridState.Lines)
		{
			return;
		}
		if (!this.PreCull)
		{
			return;
		}
		if (!base.enabled)
		{
			return;
		}
		this.PreCull = false;
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		bool activeSelf = NetworkSingleton<NetworkUI>.Instance.GUIGrid.activeSelf;
		for (int i = 0; i < grabbableNPOs.Count; i++)
		{
			this.CheckAddLayerNPO(grabbableNPOs[i], activeSelf);
		}
		this.CheckAddLayerNPO(NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO, activeSelf);
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x00066F2C File Offset: 0x0006512C
	private void CheckAddLayerNPO(NetworkPhysicsObject npo, bool force)
	{
		if (npo.ShowGridProjection || force)
		{
			List<Renderer> renderers = npo.Renderers;
			for (int i = 0; i < renderers.Count; i++)
			{
				if (renderers[i])
				{
					this.AddLayerObject(renderers[i].gameObject);
				}
			}
		}
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x00066F7B File Offset: 0x0006517B
	private void AddLayerObject(GameObject layerObject)
	{
		this.LayerObjects.Add(new GridCameraRender.CameraLayerObject(layerObject));
		layerObject.layer = 21;
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x00066F98 File Offset: 0x00065198
	private void OnPostRender()
	{
		if (!NetworkSingleton<GridOptions>.Instance.gridState.Lines)
		{
			return;
		}
		if (this.PreCull)
		{
			return;
		}
		if (!base.enabled)
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
		}
		this.LayerObjects.Clear();
	}

	// Token: 0x0400095B RID: 2395
	private List<GridCameraRender.CameraLayerObject> LayerObjects = new List<GridCameraRender.CameraLayerObject>();

	// Token: 0x0400095C RID: 2396
	private bool PreCull = true;

	// Token: 0x02000635 RID: 1589
	public struct CameraLayerObject
	{
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06003AE8 RID: 15080 RVA: 0x00175CAE File Offset: 0x00173EAE
		// (set) Token: 0x06003AE9 RID: 15081 RVA: 0x00175CB6 File Offset: 0x00173EB6
		public GameObject go { get; private set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x00175CBF File Offset: 0x00173EBF
		// (set) Token: 0x06003AEB RID: 15083 RVA: 0x00175CC7 File Offset: 0x00173EC7
		public int layer { get; private set; }

		// Token: 0x06003AEC RID: 15084 RVA: 0x00175CD0 File Offset: 0x00173ED0
		public CameraLayerObject(GameObject go)
		{
			this.go = go;
			this.layer = go.layer;
		}
	}
}
