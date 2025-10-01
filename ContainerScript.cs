using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public abstract class ContainerScript : NetworkBehavior
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00045A0F File Offset: 0x00043C0F
	// (set) Token: 0x060009D6 RID: 2518 RVA: 0x00045A17 File Offset: 0x00043C17
	public NetworkPhysicsObject NPO { get; private set; }

	// Token: 0x060009D7 RID: 2519 RVA: 0x00045A20 File Offset: 0x00043C20
	protected virtual void Awake()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00045A30 File Offset: 0x00043C30
	public virtual bool Add(NetworkPhysicsObject npo, bool top = true)
	{
		if (!this.ValidateAdd(npo))
		{
			return false;
		}
		ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(npo);
		npo.SetSmoothDestroy(this.GetPosition());
		this.Add(objectState, top);
		EventManager.TriggerObjectEnterContainer(this.NPO, npo);
		return true;
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00045A78 File Offset: 0x00043C78
	public void Add(List<NetworkPhysicsObject> npos)
	{
		for (int i = 0; i < npos.Count; i++)
		{
			this.Add(npos[i], true);
		}
	}

	// Token: 0x060009DA RID: 2522
	protected abstract bool ValidateAdd(NetworkPhysicsObject npo);

	// Token: 0x060009DB RID: 2523 RVA: 0x00045AA5 File Offset: 0x00043CA5
	public virtual void Add(ObjectState objectState, bool top = true)
	{
		if (top)
		{
			this.ObjectsInside.Insert(0, objectState);
			return;
		}
		this.ObjectsInside.Add(objectState);
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x00045AC4 File Offset: 0x00043CC4
	public void Add(List<ObjectState> objectStates)
	{
		for (int i = 0; i < objectStates.Count; i++)
		{
			this.Add(objectStates[i], true);
		}
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00045AF0 File Offset: 0x00043CF0
	public NetworkPhysicsObject Remove(int index, Vector3 position = default(Vector3))
	{
		if (this.ObjectsInside.Count <= index)
		{
			return null;
		}
		return this.Remove(this.ObjectsInside[index], position);
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00045B18 File Offset: 0x00043D18
	public NetworkPhysicsObject Remove(string guid, Vector3 position = default(Vector3))
	{
		for (int i = 0; i < this.ObjectsInside.Count; i++)
		{
			if (this.ObjectsInside[i].GUID == guid)
			{
				return this.Remove(this.ObjectsInside[i], position);
			}
		}
		return null;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00045B6C File Offset: 0x00043D6C
	public NetworkPhysicsObject Remove(bool top = true, Vector3 position = default(Vector3))
	{
		if (this.ObjectsInside.Count <= 0)
		{
			return null;
		}
		if (top)
		{
			return this.Remove(this.ObjectsInside[0], position);
		}
		return this.Remove(this.ObjectsInside[this.ObjectsInside.Count - 1], position);
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00045BC0 File Offset: 0x00043DC0
	public virtual NetworkPhysicsObject Remove(ObjectState objectState, Vector3 position = default(Vector3))
	{
		if (!this.ObjectsInside.Remove(objectState))
		{
			return null;
		}
		if (position == default(Vector3))
		{
			position = this.GetPosition();
		}
		objectState.Transform.posX = position.x;
		objectState.Transform.posY = position.y;
		objectState.Transform.posZ = position.z;
		NetworkPhysicsObject component = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false).GetComponent<NetworkPhysicsObject>();
		EventManager.TriggerObjectLeaveContainer(this.NPO, component);
		return component;
	}

	// Token: 0x060009E1 RID: 2529
	protected abstract Vector3 GetPosition();

	// Token: 0x060009E2 RID: 2530
	public abstract void SetObjectsInside(List<ObjectState> objectStates);

	// Token: 0x060009E3 RID: 2531 RVA: 0x00045C49 File Offset: 0x00043E49
	public List<ObjectState> GetObjectsInside()
	{
		return this.ObjectsInside;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x00045C51 File Offset: 0x00043E51
	public int GetNumberInside()
	{
		return this.ObjectsInside.Count;
	}

	// Token: 0x0400070A RID: 1802
	protected List<ObjectState> ObjectsInside = new List<ObjectState>();

	// Token: 0x0400070B RID: 1803
	[NonSerialized]
	public NetworkPhysicsObject LastNPOInside;
}
