using System;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class ObjectVisualizer
{
	// Token: 0x060020FE RID: 8446 RVA: 0x000EF124 File Offset: 0x000ED324
	public ObjectVisualizer(GameObject go, bool forceSpawnInPlace)
	{
		this.forceSpawnInPlace = forceSpawnInPlace;
		this.target = null;
		this.visual = go;
		this.visual.SetActive(false);
		this.Setup();
		Wait.Frames(new Action(this.Setup), 1);
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x000EF17C File Offset: 0x000ED37C
	public ObjectVisualizer(NetworkPhysicsObject npo, bool cloning = false)
	{
		if (cloning)
		{
			this.target = null;
		}
		else
		{
			this.target = npo;
		}
		ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(npo);
		this.forceSpawnInPlace = objectState.Locked;
		this.hidden = npo.IsObscured;
		this.visual = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, true, this.hidden);
		this.Setup();
		Wait.Frames(new Action(this.Setup), 1);
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000EF204 File Offset: 0x000ED404
	public void UpdateHide(bool hidden)
	{
		this.hidden = hidden;
		UnityEngine.Object.Destroy(this.visual);
		ObjectState os = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(this.target);
		this.visual = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, true, hidden);
		this.Setup();
		Wait.Frames(new Action(this.Setup), 1);
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000EF260 File Offset: 0x000ED460
	public void Setup()
	{
		if (this.target != null && this.visual.GetComponent<CustomPDF>() && this.visual.transform.childCount > 0)
		{
			BoxCollider component = this.target.GetComponent<BoxCollider>();
			BoxCollider component2 = this.visual.GetComponent<BoxCollider>();
			float x = (component2.bounds.size.x > 0f) ? (component.bounds.size.x / component2.bounds.size.x) : 1f;
			float z = (component2.bounds.size.z > 0f) ? (component.bounds.size.z / component2.bounds.size.z) : 1f;
			this.scaleMultiplier = new Vector3(x, 1f, z);
			this.visual.transform.DestroyChildren();
		}
		Collider[] componentsInChildren = this.visual.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		this.SetAlpha(ObjectPositioningVisualizer.VisualizerDropAlpha);
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x000EF3B4 File Offset: 0x000ED5B4
	public void SetAlpha(float a)
	{
		Renderer[] componentsInChildren = this.visual.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			foreach (Material material in componentsInChildren[i].materials)
			{
				material.shader = Singleton<ObjectPositioningVisualizer>.Instance.shader;
				Color color = material.color;
				color.a = a;
				material.color = color;
			}
		}
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x000EF41C File Offset: 0x000ED61C
	public void Destroy()
	{
		UnityEngine.Object.Destroy(this.visual);
		this.visual = null;
		this.target = null;
	}

	// Token: 0x04001464 RID: 5220
	public GameObject visual;

	// Token: 0x04001465 RID: 5221
	public NetworkPhysicsObject target;

	// Token: 0x04001466 RID: 5222
	public bool forceSpawnInPlace;

	// Token: 0x04001467 RID: 5223
	public bool hidden;

	// Token: 0x04001468 RID: 5224
	public Vector3 scaleMultiplier = Vector3.one;
}
