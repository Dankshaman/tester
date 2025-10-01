using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000440 RID: 1088
	[Serializable]
	public class ObjectColliderAttachmentSettings
	{
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060031DC RID: 12764 RVA: 0x001513B1 File Offset: 0x0014F5B1
		public static Vector3 MinBoxColliderSizeForNonMeshObjects
		{
			get
			{
				return Vector3.one * 0.1f;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinSphereColliderRadiusForNonMeshObjects
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060031DE RID: 12766 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinCapsuleColliderRadiusForNonMeshObjects
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinCapsuleColliderHeightForNonMeshObjects
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x001513C2 File Offset: 0x0014F5C2
		// (set) Token: 0x060031E1 RID: 12769 RVA: 0x001513CA File Offset: 0x0014F5CA
		public ObjectCollider3DType ColliderTypeForMeshObjects
		{
			get
			{
				return this._colliderTypeForMeshObjects;
			}
			set
			{
				this._colliderTypeForMeshObjects = value;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060031E2 RID: 12770 RVA: 0x001513D3 File Offset: 0x0014F5D3
		// (set) Token: 0x060031E3 RID: 12771 RVA: 0x001513DB File Offset: 0x0014F5DB
		public ObjectCollider3DType ColliderTypeForLightObjects
		{
			get
			{
				return this._colliderTypeForLightObjects;
			}
			set
			{
				this._colliderTypeForLightObjects = value;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x001513E4 File Offset: 0x0014F5E4
		// (set) Token: 0x060031E5 RID: 12773 RVA: 0x001513EC File Offset: 0x0014F5EC
		public ObjectCollider3DType ColliderTypeForParticleSystemObjects
		{
			get
			{
				return this._colliderTypeForParticleSystemObjects;
			}
			set
			{
				this._colliderTypeForParticleSystemObjects = value;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060031E6 RID: 12774 RVA: 0x001513F5 File Offset: 0x0014F5F5
		// (set) Token: 0x060031E7 RID: 12775 RVA: 0x001513FD File Offset: 0x0014F5FD
		public ObjectCollider2DType ColliderTypeForSpriteObjects
		{
			get
			{
				return this._colliderTypeForSpriteObjects;
			}
			set
			{
				this._colliderTypeForSpriteObjects = value;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x00151406 File Offset: 0x0014F606
		// (set) Token: 0x060031E9 RID: 12777 RVA: 0x0015140E File Offset: 0x0014F60E
		public Vector3 BoxColliderSizeForNonMeshObjects
		{
			get
			{
				return this._boxColliderSizeForNonMeshObjects;
			}
			set
			{
				this._boxColliderSizeForNonMeshObjects = Vector3.Max(value, ObjectColliderAttachmentSettings.MinBoxColliderSizeForNonMeshObjects);
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x00151421 File Offset: 0x0014F621
		// (set) Token: 0x060031EB RID: 12779 RVA: 0x00151429 File Offset: 0x0014F629
		public float SphereColliderRadiusForNonMeshObjects
		{
			get
			{
				return this._sphereColliderRadiusForNonMeshObjects;
			}
			set
			{
				this._sphereColliderRadiusForNonMeshObjects = Mathf.Max(value, ObjectColliderAttachmentSettings.MinSphereColliderRadiusForNonMeshObjects);
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060031EC RID: 12780 RVA: 0x0015143C File Offset: 0x0014F63C
		// (set) Token: 0x060031ED RID: 12781 RVA: 0x00151444 File Offset: 0x0014F644
		public float CapsuleColliderRadiusForNonMeshObjects
		{
			get
			{
				return this._capsuleColliderRadiusForNonMeshObjects;
			}
			set
			{
				this._capsuleColliderRadiusForNonMeshObjects = Mathf.Max(value, ObjectColliderAttachmentSettings.MinCapsuleColliderRadiusForNonMeshObjects);
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x00151457 File Offset: 0x0014F657
		// (set) Token: 0x060031EF RID: 12783 RVA: 0x0015145F File Offset: 0x0014F65F
		public float CapsuleColliderHeightForNonMeshObjects
		{
			get
			{
				return this._capsuleColliderHeightForNonMeshObjects;
			}
			set
			{
				this._capsuleColliderHeightForNonMeshObjects = Mathf.Max(value, ObjectColliderAttachmentSettings.MinCapsuleColliderHeightForNonMeshObjects);
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060031F0 RID: 12784 RVA: 0x00151472 File Offset: 0x0014F672
		// (set) Token: 0x060031F1 RID: 12785 RVA: 0x0015147A File Offset: 0x0014F67A
		public bool IgnoreMeshObjects
		{
			get
			{
				return this._ignoreMeshObjects;
			}
			set
			{
				this._ignoreMeshObjects = value;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060031F2 RID: 12786 RVA: 0x00151483 File Offset: 0x0014F683
		// (set) Token: 0x060031F3 RID: 12787 RVA: 0x0015148B File Offset: 0x0014F68B
		public bool IgnoreLightObjects
		{
			get
			{
				return this._ignoreLightObjects;
			}
			set
			{
				this._ignoreLightObjects = value;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060031F4 RID: 12788 RVA: 0x00151494 File Offset: 0x0014F694
		// (set) Token: 0x060031F5 RID: 12789 RVA: 0x0015149C File Offset: 0x0014F69C
		public bool IgnoreParticleSystemObjects
		{
			get
			{
				return this._ignoreParticleSystemObjects;
			}
			set
			{
				this._ignoreParticleSystemObjects = value;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060031F6 RID: 12790 RVA: 0x001514A5 File Offset: 0x0014F6A5
		// (set) Token: 0x060031F7 RID: 12791 RVA: 0x001514AD File Offset: 0x0014F6AD
		public bool IgnoreSpriteObjects
		{
			get
			{
				return this._ignoreSpriteObjects;
			}
			set
			{
				this._ignoreSpriteObjects = value;
			}
		}

		// Token: 0x04002028 RID: 8232
		[SerializeField]
		private ObjectCollider3DType _colliderTypeForMeshObjects;

		// Token: 0x04002029 RID: 8233
		[SerializeField]
		private ObjectCollider3DType _colliderTypeForLightObjects;

		// Token: 0x0400202A RID: 8234
		[SerializeField]
		private ObjectCollider3DType _colliderTypeForParticleSystemObjects;

		// Token: 0x0400202B RID: 8235
		[SerializeField]
		private ObjectCollider2DType _colliderTypeForSpriteObjects;

		// Token: 0x0400202C RID: 8236
		[SerializeField]
		private Vector3 _boxColliderSizeForNonMeshObjects = Vector3.one;

		// Token: 0x0400202D RID: 8237
		[SerializeField]
		private float _sphereColliderRadiusForNonMeshObjects = 1f;

		// Token: 0x0400202E RID: 8238
		[SerializeField]
		private float _capsuleColliderRadiusForNonMeshObjects = 1f;

		// Token: 0x0400202F RID: 8239
		[SerializeField]
		private float _capsuleColliderHeightForNonMeshObjects = 1f;

		// Token: 0x04002030 RID: 8240
		[SerializeField]
		private bool _ignoreMeshObjects;

		// Token: 0x04002031 RID: 8241
		[SerializeField]
		private bool _ignoreLightObjects = true;

		// Token: 0x04002032 RID: 8242
		[SerializeField]
		private bool _ignoreParticleSystemObjects = true;

		// Token: 0x04002033 RID: 8243
		[SerializeField]
		private bool _ignoreSpriteObjects;
	}
}
