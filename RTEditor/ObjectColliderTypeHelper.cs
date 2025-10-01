using System;
using System.Collections.Generic;
using System.Linq;

namespace RTEditor
{
	// Token: 0x02000419 RID: 1049
	public static class ObjectColliderTypeHelper
	{
		// Token: 0x06003096 RID: 12438 RVA: 0x0014CD14 File Offset: 0x0014AF14
		public static List<ObjectCollider3DType> GetPossibleObjectCollderTypes()
		{
			Array values = Enum.GetValues(typeof(ObjectCollider3DType));
			List<ObjectCollider3DType> list = new List<ObjectCollider3DType>();
			foreach (object obj in values)
			{
				ObjectCollider3DType item = (ObjectCollider3DType)obj;
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x0014CD80 File Offset: 0x0014AF80
		public static string[] GetPossibleObjectColliderTypeNames(ObjectCollider3DType[] ignoreTypes = null)
		{
			List<ObjectCollider3DType> possibleObjectCollderTypes = ObjectColliderTypeHelper.GetPossibleObjectCollderTypes();
			if (ignoreTypes != null && ignoreTypes.Length != 0)
			{
				return (from colliderType in possibleObjectCollderTypes
				where !ignoreTypes.Contains(colliderType)
				select colliderType).Select(delegate(ObjectCollider3DType colliderType)
				{
					ObjectCollider3DType objectCollider3DType = colliderType;
					return objectCollider3DType.ToString();
				}).ToArray<string>();
			}
			return possibleObjectCollderTypes.Select(delegate(ObjectCollider3DType colliderType)
			{
				ObjectCollider3DType objectCollider3DType = colliderType;
				return objectCollider3DType.ToString();
			}).ToArray<string>();
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x0014CE18 File Offset: 0x0014B018
		public static bool GetObjectColliderTypeByName(string name, out ObjectCollider3DType outputColliderType)
		{
			outputColliderType = ObjectCollider3DType.Box;
			foreach (ObjectCollider3DType objectCollider3DType in ObjectColliderTypeHelper.GetPossibleObjectCollderTypes())
			{
				if (objectCollider3DType.ToString() == name)
				{
					outputColliderType = objectCollider3DType;
					return true;
				}
			}
			return false;
		}
	}
}
