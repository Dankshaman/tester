using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003FC RID: 1020
	public static class TransformExtensions
	{
		// Token: 0x06002EDB RID: 11995 RVA: 0x00141490 File Offset: 0x0013F690
		public static Matrix4x4 GetRelativeTransform(this Transform transform, Transform referenceTransform)
		{
			return referenceTransform.localToWorldMatrix.inverse * transform.localToWorldMatrix;
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x001414B6 File Offset: 0x0013F6B6
		public static Matrix4x4 GetWorldMatrix(this Transform transform)
		{
			return Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		}
	}
}
