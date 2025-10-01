using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class ParticleHighlight : MonoBehaviour
{
	// Token: 0x0600185F RID: 6239 RVA: 0x000A5E60 File Offset: 0x000A4060
	public void SetTarget(Transform target, bool box = false)
	{
		this.target = target;
		base.transform.rotation = target.rotation;
		if (box)
		{
			this.hasMesh = false;
			Vector3 vector = target.GetComponentInChildren<Collider>().bounds.size * ParticleHighlight.HIGHLIGHT_3D_SCALAR;
			if (vector.x < ParticleHighlight.HIGHLIGHT_3D_MIN_SIZE)
			{
				vector.x = ParticleHighlight.HIGHLIGHT_3D_MIN_SIZE;
			}
			if (vector.y < ParticleHighlight.HIGHLIGHT_3D_MIN_SIZE)
			{
				vector.y = ParticleHighlight.HIGHLIGHT_3D_MIN_SIZE;
			}
			if (vector.z < ParticleHighlight.HIGHLIGHT_3D_MIN_SIZE)
			{
				vector.z = ParticleHighlight.HIGHLIGHT_3D_MIN_SIZE;
			}
			base.transform.localScale = vector;
			base.GetComponent<Renderer>().enabled = true;
			return;
		}
		base.transform.localScale = Vector3.one * ParticleHighlight.HIGHLIGHT_3D_SCALAR;
		if (target.GetComponent<MeshFilter>())
		{
			this.hasMesh = true;
			base.GetComponent<MeshFilter>().sharedMesh = target.GetComponent<MeshFilter>().sharedMesh;
		}
		else
		{
			this.hasMesh = false;
			Renderer componentInChildren = target.gameObject.GetComponentInChildren<Renderer>();
			if (componentInChildren)
			{
				MeshFilter component = componentInChildren.GetComponent<MeshFilter>();
				if (component)
				{
					base.GetComponent<MeshFilter>().sharedMesh = component.sharedMesh;
					base.transform.rotation = component.transform.rotation;
				}
				else
				{
					base.GetComponent<MeshFilter>().sharedMesh = componentInChildren.GetComponent<SkinnedMeshRenderer>().sharedMesh;
					base.transform.rotation = componentInChildren.transform.rotation;
				}
			}
			else
			{
				base.GetComponent<MeshFilter>().sharedMesh = target.gameObject.GetComponentInChildren<Collider>().GetComponent<MeshFilter>().sharedMesh;
			}
		}
		base.GetComponent<Renderer>().enabled = true;
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x000A600A File Offset: 0x000A420A
	private void Update()
	{
		if (this.hasMesh)
		{
			base.GetComponent<MeshFilter>().sharedMesh = this.target.GetComponent<MeshFilter>().sharedMesh;
		}
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x000A6030 File Offset: 0x000A4230
	public void SetColor(Colour colour)
	{
		Colour colour2 = colour;
		colour2.a = ParticleHighlight.HIGHLIGHT_3D_OPACITY;
		base.GetComponent<Renderer>().material.SetColor("_TintColor", colour2);
	}

	// Token: 0x04000E9B RID: 3739
	public static float HIGHLIGHT_3D_OPACITY = 0.1f;

	// Token: 0x04000E9C RID: 3740
	public static float HIGHLIGHT_3D_MIN_SIZE = 0.5f;

	// Token: 0x04000E9D RID: 3741
	public static float HIGHLIGHT_3D_SCALAR = 1.01f;

	// Token: 0x04000E9E RID: 3742
	private Transform target;

	// Token: 0x04000E9F RID: 3743
	private bool hasMesh;
}
