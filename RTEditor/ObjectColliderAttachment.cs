using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200043F RID: 1087
	public class ObjectColliderAttachment : SingletonBase<ObjectColliderAttachment>
	{
		// Token: 0x060031D4 RID: 12756 RVA: 0x0015113C File Offset: 0x0014F33C
		public void AttachCollidersToAllSceneObjects(ObjectColliderAttachmentSettings colliderAttachmentSettings)
		{
			foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType<GameObject>())
			{
				this.AttachColliderToGameObject(gameObject, colliderAttachmentSettings);
			}
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x0015116C File Offset: 0x0014F36C
		public void AttachCollidersToObjectHierarchy(GameObject hierarchyRoot, ObjectColliderAttachmentSettings colliderAttachmentSettings)
		{
			Transform[] componentsInChildren = hierarchyRoot.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				GameObject gameObject = componentsInChildren[i].gameObject;
				this.AttachColliderToGameObject(gameObject, colliderAttachmentSettings);
			}
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x001511A0 File Offset: 0x0014F3A0
		private void AttachColliderToGameObject(GameObject gameObject, ObjectColliderAttachmentSettings colliderAttachmentSettings)
		{
			if (gameObject.GetComponent<SpriteRenderer>() != null && !colliderAttachmentSettings.IgnoreSpriteObjects)
			{
				gameObject.RemoveAllColliders();
				this.AttachColliderToSpriteObject(gameObject, colliderAttachmentSettings);
				return;
			}
			if (gameObject.GetMesh() != null && !colliderAttachmentSettings.IgnoreMeshObjects)
			{
				gameObject.RemoveAllColliders();
				this.AttachColliderToMeshObject(gameObject, colliderAttachmentSettings);
				return;
			}
			if (gameObject.GetFirstLightComponent() != null && !colliderAttachmentSettings.IgnoreLightObjects)
			{
				gameObject.RemoveAllColliders();
				this.AttachColliderToLightObject(gameObject, colliderAttachmentSettings);
				return;
			}
			if (gameObject.GetFirstParticleSystemComponent() != null && !colliderAttachmentSettings.IgnoreParticleSystemObjects)
			{
				gameObject.RemoveAllColliders();
				this.AttachColliderToParticleSystemObject(gameObject, colliderAttachmentSettings);
			}
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x00151240 File Offset: 0x0014F440
		private void AttachColliderToMeshObject(GameObject gameObject, ObjectColliderAttachmentSettings colliderAttachmentSettings)
		{
			if (colliderAttachmentSettings.ColliderTypeForMeshObjects == ObjectCollider3DType.MeshCollider)
			{
				gameObject.AddComponent<MeshCollider>();
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForMeshObjects == ObjectCollider3DType.Box)
			{
				gameObject.AddComponent<BoxCollider>();
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForMeshObjects == ObjectCollider3DType.Sphere)
			{
				gameObject.AddComponent<SphereCollider>();
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForMeshObjects == ObjectCollider3DType.Capsule)
			{
				gameObject.AddComponent<CapsuleCollider>();
			}
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x0015128F File Offset: 0x0014F48F
		private void AttachColliderToSpriteObject(GameObject gameObject, ObjectColliderAttachmentSettings colliderAttachmentSettings)
		{
			if (colliderAttachmentSettings.ColliderTypeForSpriteObjects == ObjectCollider2DType.Box)
			{
				gameObject.AddComponent<BoxCollider2D>();
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForSpriteObjects == ObjectCollider2DType.Circle)
			{
				gameObject.AddComponent<CircleCollider2D>();
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForSpriteObjects == ObjectCollider2DType.Polygon)
			{
				gameObject.AddComponent<PolygonCollider2D>();
			}
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x001512C4 File Offset: 0x0014F4C4
		private void AttachColliderToLightObject(GameObject gameObject, ObjectColliderAttachmentSettings colliderAttachmentSettings)
		{
			if (colliderAttachmentSettings.ColliderTypeForLightObjects == ObjectCollider3DType.Box || colliderAttachmentSettings.ColliderTypeForLightObjects == ObjectCollider3DType.MeshCollider)
			{
				gameObject.AddComponent<BoxCollider>().size = colliderAttachmentSettings.BoxColliderSizeForNonMeshObjects;
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForLightObjects == ObjectCollider3DType.Sphere)
			{
				gameObject.AddComponent<SphereCollider>().radius = colliderAttachmentSettings.SphereColliderRadiusForNonMeshObjects;
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForLightObjects == ObjectCollider3DType.Capsule)
			{
				CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
				capsuleCollider.radius = colliderAttachmentSettings.CapsuleColliderRadiusForNonMeshObjects;
				capsuleCollider.height = colliderAttachmentSettings.CapsuleColliderHeightForNonMeshObjects;
			}
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x00151338 File Offset: 0x0014F538
		private void AttachColliderToParticleSystemObject(GameObject gameObject, ObjectColliderAttachmentSettings colliderAttachmentSettings)
		{
			if (colliderAttachmentSettings.ColliderTypeForParticleSystemObjects == ObjectCollider3DType.Box || colliderAttachmentSettings.ColliderTypeForParticleSystemObjects == ObjectCollider3DType.MeshCollider)
			{
				gameObject.AddComponent<BoxCollider>().size = colliderAttachmentSettings.BoxColliderSizeForNonMeshObjects;
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForParticleSystemObjects == ObjectCollider3DType.Sphere)
			{
				gameObject.AddComponent<SphereCollider>().radius = colliderAttachmentSettings.SphereColliderRadiusForNonMeshObjects;
				return;
			}
			if (colliderAttachmentSettings.ColliderTypeForParticleSystemObjects == ObjectCollider3DType.Capsule)
			{
				CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
				capsuleCollider.radius = colliderAttachmentSettings.CapsuleColliderRadiusForNonMeshObjects;
				capsuleCollider.height = colliderAttachmentSettings.CapsuleColliderHeightForNonMeshObjects;
			}
		}
	}
}
