using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003FB RID: 1019
	public static class SpriteRendererExtensions
	{
		// Token: 0x06002ED5 RID: 11989 RVA: 0x00141188 File Offset: 0x0013F388
		public static List<Vector3> GetWorldCenterAndCornerPoints(this SpriteRenderer spriteRenderer)
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(spriteRenderer.GetWorldCenterPoint());
			List<Vector3> worldCornerPoints = spriteRenderer.GetWorldCornerPoints();
			list.AddRange(worldCornerPoints);
			return list;
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x001411B4 File Offset: 0x0013F3B4
		public static List<Vector3> GetWorldCornerPoints(this SpriteRenderer spriteRenderer)
		{
			Vector3 vector = Vector3.Scale(spriteRenderer.GetModelSpaceSize(), spriteRenderer.transform.lossyScale) * 0.5f;
			Transform transform = spriteRenderer.transform;
			Vector3 worldCenterPoint = spriteRenderer.GetWorldCenterPoint();
			Vector3 right = transform.right;
			Vector3 up = transform.up;
			return new List<Vector3>
			{
				worldCenterPoint - right * vector.x + up * vector.y,
				worldCenterPoint + right * vector.x + up * vector.y,
				worldCenterPoint + right * vector.x - up * vector.y,
				worldCenterPoint - right * vector.x - up * vector.y
			};
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x001412A8 File Offset: 0x0013F4A8
		public static Vector3 GetWorldCenterPoint(this SpriteRenderer spriteRenderer)
		{
			return spriteRenderer.transform.TransformPoint(spriteRenderer.GetModelSpaceBounds().center);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x001412D0 File Offset: 0x0013F4D0
		public static Vector3 GetModelSpaceSize(this SpriteRenderer spriteRenderer)
		{
			return spriteRenderer.GetModelSpaceBounds().size;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x001412EC File Offset: 0x0013F4EC
		public static Bounds GetModelSpaceBounds(this SpriteRenderer spriteRenderer)
		{
			Sprite sprite = spriteRenderer.sprite;
			if (sprite == null)
			{
				return BoundsExtensions.GetInvalidBoundsInstance();
			}
			Vector3 center = spriteRenderer.transform.InverseTransformPoint(spriteRenderer.bounds.center);
			center.z = 0f;
			return new Bounds(center, sprite.rect.size / sprite.pixelsPerUnit);
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x0014135C File Offset: 0x0013F55C
		public static bool IsPixelFullyTransparent(this SpriteRenderer spriteRenderer, Vector3 worldPos)
		{
			Sprite sprite = spriteRenderer.sprite;
			if (sprite == null)
			{
				return true;
			}
			Texture2D texture = sprite.texture;
			if (texture == null)
			{
				return true;
			}
			Vector3 point = spriteRenderer.transform.InverseTransformPoint(worldPos);
			Plane plane = new Plane(Vector3.forward, 0f);
			Vector3 vector = plane.ProjectPoint(point);
			Bounds modelSpaceBounds = spriteRenderer.GetModelSpaceBounds();
			modelSpaceBounds.size = new Vector3(modelSpaceBounds.size.x, modelSpaceBounds.size.y, 1f);
			if (!modelSpaceBounds.Contains(vector))
			{
				return true;
			}
			Vector3 b = plane.ProjectPoint(modelSpaceBounds.min);
			Vector3 vector2 = vector - b;
			Vector2 vector3 = new Vector2(vector2.x * sprite.pixelsPerUnit, vector2.y * sprite.pixelsPerUnit);
			vector3 += sprite.textureRectOffset;
			bool result;
			try
			{
				result = (texture.GetPixel((int)(vector3.x + 0.5f), (int)(vector3.y + 0.5f)).a <= 0.001f);
			}
			catch (UnityException ex)
			{
				result = (ex == null && false);
			}
			return result;
		}
	}
}
