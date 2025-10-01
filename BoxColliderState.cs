using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class BoxColliderState
{
	// Token: 0x06000855 RID: 2133 RVA: 0x00002594 File Offset: 0x00000794
	public BoxColliderState()
	{
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0003AC8C File Offset: 0x00038E8C
	public BoxColliderState(BoxCollider boxCollider, string Name)
	{
		this.LocalPosition = new VectorState(boxCollider.transform.localPosition);
		this.LocalEulerRotation = new VectorState(boxCollider.transform.localEulerAngles);
		this.Center = new VectorState(boxCollider.center);
		this.Size = new VectorState(boxCollider.size);
		this.Name = Name;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0003ACF4 File Offset: 0x00038EF4
	public static BoxColliderState[] GetBoxColliders(GameObject ColliderRoot, string Name)
	{
		BoxCollider[] componentsInChildren = ColliderRoot.GetComponentsInChildren<BoxCollider>();
		BoxColliderState[] array = new BoxColliderState[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i] = new BoxColliderState(componentsInChildren[i], Name);
		}
		return array;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0003AD2C File Offset: 0x00038F2C
	public static void CreateBoxColliders(GameObject ColliderRoot, BoxColliderState[] BoxColliderStates)
	{
		for (int i = 0; i < BoxColliderStates.Length; i++)
		{
			GameObject gameObject = new GameObject("Generated CompoundColliders" + i);
			gameObject.transform.parent = ColliderRoot.transform;
			gameObject.transform.localPosition = BoxColliderStates[i].LocalPosition.ToVector();
			gameObject.transform.localEulerAngles = BoxColliderStates[i].LocalEulerRotation.ToVector();
			gameObject.transform.localScale = Vector3.one;
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.center = BoxColliderStates[i].Center.ToVector();
			Vector3 size = new Vector3(Mathf.Abs(BoxColliderStates[i].Size.x), Mathf.Abs(BoxColliderStates[i].Size.y), Mathf.Abs(BoxColliderStates[i].Size.z));
			boxCollider.size = size;
			gameObject.layer = ColliderRoot.transform.root.gameObject.layer;
		}
	}

	// Token: 0x040005E0 RID: 1504
	public string Name;

	// Token: 0x040005E1 RID: 1505
	public VectorState LocalPosition;

	// Token: 0x040005E2 RID: 1506
	public VectorState LocalEulerRotation;

	// Token: 0x040005E3 RID: 1507
	public VectorState Center;

	// Token: 0x040005E4 RID: 1508
	public VectorState Size;
}
