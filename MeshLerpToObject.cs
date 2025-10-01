using System;
using NewNet;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class MeshLerpToObject : MonoBehaviour
{
	// Token: 0x060015EC RID: 5612 RVA: 0x00098E0C File Offset: 0x0009700C
	private void Awake()
	{
		this.ThisMeshFilter = base.GetComponent<MeshFilter>();
		this.ThisRender = base.GetComponent<Renderer>();
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x00098E26 File Offset: 0x00097026
	private void Start()
	{
		this.isMine = this.FollowObject.GetComponent<NetworkView>().isMine;
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x00098E40 File Offset: 0x00097040
	private void Update()
	{
		if (!this.FollowObject)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!this.isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.FollowObject.transform.position, 30f * Time.deltaTime);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.FollowObject.transform.rotation, 30f * Time.deltaTime);
			this.ThisRender.enabled = (!NetworkSingleton<PlayerManager>.Instance.IsBlinded() && !this.IsInvisible);
		}
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x00098F00 File Offset: 0x00097100
	public void SetPointerMesh(int MeshInt, int TypeInt, bool forSpectatorCamera = false)
	{
		if (TypeInt < 0)
		{
			this.ThisMeshFilter.mesh = null;
			return;
		}
		if (forSpectatorCamera)
		{
			base.gameObject.layer = 29;
		}
		else
		{
			base.gameObject.layer = 8;
		}
		if (MeshInt != this.CurrentMeshInt || TypeInt != this.CurrentTypeInt)
		{
			if (MeshInt == 0 || MeshInt == 1)
			{
				this.FollowObject.GetComponent<Pointer>().FirstGrabbedObject = null;
			}
			this.CurrentMeshInt = MeshInt;
			this.CurrentTypeInt = TypeInt;
			if (this.CurrentMeshInt != -1)
			{
				if (MeshInt < 4)
				{
					if (this.CurrentTypeInt == 0)
					{
						this.ThisMeshFilter.mesh = this.PointerDefaultMeshes[this.CurrentMeshInt];
						base.GetComponent<Renderer>().materials = this.PointerDefaultMaterials;
					}
					else if (this.CurrentTypeInt == 1)
					{
						this.ThisMeshFilter.mesh = this.PointerRobottMeshes[this.CurrentMeshInt];
						base.GetComponent<Renderer>().materials = this.PointerRobotMaterials;
					}
				}
				else if (this.CurrentMeshInt == 4)
				{
					this.ThisMeshFilter.mesh = this.PointerPencil;
					base.GetComponent<Renderer>().materials = this.PointerPencilMaterials;
					base.GetComponent<Renderer>().materials[2].color = this.PointerColor;
				}
				else if (this.CurrentMeshInt == 5)
				{
					this.ThisMeshFilter.mesh = this.PointerEraser;
					base.GetComponent<Renderer>().materials = this.PointerEraserMaterials;
				}
				else if (this.CurrentMeshInt == 6)
				{
					this.ThisMeshFilter.mesh = this.PointerLine;
					base.GetComponent<Renderer>().materials = this.PointerLineMaterials;
				}
				else if (this.CurrentMeshInt == 7)
				{
					this.ThisMeshFilter.mesh = this.PointerJoint;
					base.GetComponent<Renderer>().materials = this.PointerJointMaterials;
					base.GetComponent<Renderer>().material.SetColor("_SpecColor", this.PointerColor);
				}
				else if (this.CurrentMeshInt == 8)
				{
					this.ThisMeshFilter.mesh = this.PointerText;
					base.GetComponent<Renderer>().materials = this.PointerTextMaterials;
				}
				base.GetComponent<Renderer>().material.color = this.PointerColor;
				return;
			}
			this.ThisMeshFilter.mesh = null;
		}
	}

	// Token: 0x04000C4C RID: 3148
	public GameObject FollowObject;

	// Token: 0x04000C4D RID: 3149
	public Mesh[] PointerDefaultMeshes;

	// Token: 0x04000C4E RID: 3150
	public Mesh[] PointerRobottMeshes;

	// Token: 0x04000C4F RID: 3151
	public Mesh PointerPencil;

	// Token: 0x04000C50 RID: 3152
	public Mesh PointerEraser;

	// Token: 0x04000C51 RID: 3153
	public Mesh PointerLine;

	// Token: 0x04000C52 RID: 3154
	public Mesh PointerJoint;

	// Token: 0x04000C53 RID: 3155
	public Mesh PointerText;

	// Token: 0x04000C54 RID: 3156
	public Material[] PointerDefaultMaterials;

	// Token: 0x04000C55 RID: 3157
	public Material[] PointerTextMaterials;

	// Token: 0x04000C56 RID: 3158
	public Material[] PointerRobotMaterials;

	// Token: 0x04000C57 RID: 3159
	public Material[] PointerPencilMaterials;

	// Token: 0x04000C58 RID: 3160
	public Material[] PointerEraserMaterials;

	// Token: 0x04000C59 RID: 3161
	public Material[] PointerLineMaterials;

	// Token: 0x04000C5A RID: 3162
	public Material[] PointerJointMaterials;

	// Token: 0x04000C5B RID: 3163
	private int CurrentMeshInt = -1;

	// Token: 0x04000C5C RID: 3164
	private int CurrentTypeInt = -1;

	// Token: 0x04000C5D RID: 3165
	public Color PointerColor;

	// Token: 0x04000C5E RID: 3166
	private MeshFilter ThisMeshFilter;

	// Token: 0x04000C5F RID: 3167
	private Renderer ThisRender;

	// Token: 0x04000C60 RID: 3168
	private bool isMine;

	// Token: 0x04000C61 RID: 3169
	public bool IsInvisible;
}
