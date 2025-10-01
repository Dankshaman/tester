using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class ChildSpawner : NetworkBehavior
{
	// Token: 0x06000912 RID: 2322 RVA: 0x0004173B File Offset: 0x0003F93B
	private void Awake()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		NetworkEvents.OnPlayerConnected += this.NetworkEvents_OnPlayerConnected;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0004175A File Offset: 0x0003F95A
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.NetworkEvents_OnPlayerConnected;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0004176D File Offset: 0x0003F96D
	private void NetworkEvents_OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<List<ObjectState>>(player, new Action<List<ObjectState>>(this.RPCSetChildren), this.GetChildren());
		}
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00041794 File Offset: 0x0003F994
	private void _SetChildren(List<ObjectState> newChildren)
	{
		this._DestroyChildren();
		for (int i = 0; i < newChildren.Count; i++)
		{
			this._AddChild(newChildren[i]);
		}
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x000417C8 File Offset: 0x0003F9C8
	private GameObject _AddChild(ObjectState objectState)
	{
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, true, false);
		UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = objectState.Transform.Position();
		gameObject.transform.localEulerAngles = objectState.Transform.Rotation();
		gameObject.transform.localScale = objectState.Transform.Scale();
		this.children.Add(new ChildSpawner.ChildInfo(gameObject, objectState));
		return gameObject;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00041854 File Offset: 0x0003FA54
	private ObjectState _RemoveChild(int index)
	{
		ChildSpawner.ChildInfo childInfo = this.children[index];
		GameObject child = childInfo.child;
		ObjectState objectState = childInfo.objectState;
		child.transform.parent = null;
		objectState.Transform = new TransformState(child.transform);
		UnityEngine.Object.Destroy(child);
		this.children.Remove(childInfo);
		return objectState;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x000418AC File Offset: 0x0003FAAC
	private List<ObjectState> zRemoveChildren()
	{
		List<ObjectState> list = new List<ObjectState>();
		while (this.children.Count > 0)
		{
			list.Add(this._RemoveChild(0));
		}
		return list;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000418E0 File Offset: 0x0003FAE0
	private void _DestroyChildren()
	{
		foreach (ChildSpawner.ChildInfo childInfo in this.children)
		{
			UnityEngine.Object.Destroy(childInfo.child);
		}
		this.children.Clear();
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00041940 File Offset: 0x0003FB40
	[Remote("Permissions/Combining")]
	public void AddChild(NetworkPhysicsObject targetNPO)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<NetworkPhysicsObject>(RPCTarget.Server, new Action<NetworkPhysicsObject>(this.AddChild), targetNPO);
			return;
		}
		ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(targetNPO.gameObject);
		targetNPO.transform.parent = base.transform;
		objectState.Transform = new TransformState(targetNPO.transform.localPosition, targetNPO.transform.localEulerAngles, targetNPO.transform.localScale);
		targetNPO.transform.parent = null;
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(targetNPO.gameObject);
		this.AddChild(objectState);
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x000419DF File Offset: 0x0003FBDF
	public void AddChild(ObjectState objectState)
	{
		base.networkView.RPC<ObjectState>(RPCTarget.All, new Action<ObjectState>(this.RPCAddChild), objectState);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x000419FA File Offset: 0x0003FBFA
	[Remote("Permissions/Combining", serializationMethod = SerializationMethod.Json)]
	private void RPCAddChild(ObjectState objectState)
	{
		this._AddChild(objectState);
		this.UpdateChildren();
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00041A0A File Offset: 0x0003FC0A
	public void SetChildren(List<ObjectState> newChildren)
	{
		base.networkView.RPC<List<ObjectState>>(RPCTarget.All, new Action<List<ObjectState>>(this.RPCSetChildren), newChildren);
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00041A25 File Offset: 0x0003FC25
	[Remote("Permissions/Combining", serializationMethod = SerializationMethod.Json)]
	private void RPCSetChildren(List<ObjectState> newChildren)
	{
		this._SetChildren(newChildren);
		this.UpdateChildren();
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00041A34 File Offset: 0x0003FC34
	[Remote("Permissions/Combining")]
	public NetworkPhysicsObject RemoveChild(int index)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, NetworkPhysicsObject>(RPCTarget.Server, new Func<int, NetworkPhysicsObject>(this.RemoveChild), index);
			return null;
		}
		base.networkView.RPC<int, ObjectState>(RPCTarget.Others, new Func<int, ObjectState>(this.RPCRemoveChild), index);
		ObjectState objectState = this.RPCRemoveChild(index);
		if (objectState != null)
		{
			return this.SpawnChildNPO(objectState);
		}
		return null;
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00041A98 File Offset: 0x0003FC98
	private NetworkPhysicsObject SpawnChildNPO(ObjectState objectState)
	{
		Vector3 vector = objectState.Transform.Scale();
		objectState.Transform.scaleX = 1f;
		objectState.Transform.scaleY = 1f;
		objectState.Transform.scaleZ = 1f;
		NetworkPhysicsObject component = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false).GetComponent<NetworkPhysicsObject>();
		Vector3 scale = new Vector3(vector.x / component.transform.localScale.x, vector.y / component.transform.localScale.y, vector.z / component.transform.localScale.z);
		component.SetScale(scale, false);
		return component;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00041B49 File Offset: 0x0003FD49
	[Remote("Permissions/Combining")]
	private ObjectState RPCRemoveChild(int index)
	{
		ObjectState result = this._RemoveChild(index);
		this.UpdateChildren();
		return result;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00041B58 File Offset: 0x0003FD58
	public void DestroyChild(int index)
	{
		base.networkView.RPC<int, ObjectState>(RPCTarget.All, new Func<int, ObjectState>(this.RPCRemoveChild), index);
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00041B74 File Offset: 0x0003FD74
	public List<NetworkPhysicsObject> RemoveChildren()
	{
		List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
		base.networkView.RPC<List<ObjectState>>(RPCTarget.Others, new Func<List<ObjectState>>(this.RPCRemoveChildren));
		List<ObjectState> list2 = this.RPCRemoveChildren();
		for (int i = 0; i < list2.Count; i++)
		{
			list.Add(this.SpawnChildNPO(list2[i]));
		}
		return list;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00041BCB File Offset: 0x0003FDCB
	[Remote("Permissions/Combining")]
	private List<ObjectState> RPCRemoveChildren()
	{
		List<ObjectState> result = this.zRemoveChildren();
		this.UpdateChildren();
		return result;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00041BD9 File Offset: 0x0003FDD9
	public void DestroyChildren()
	{
		base.networkView.RPC(RPCTarget.All, new Action(this.RPCDestroyChildren));
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00041BF3 File Offset: 0x0003FDF3
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, permission = Permission.Client, validationFunction = "Permissions/Combining")]
	private void RPCDestroyChildren()
	{
		this._DestroyChildren();
		this.UpdateChildren();
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00041C04 File Offset: 0x0003FE04
	public List<ObjectState> GetChildren()
	{
		List<ObjectState> list = new List<ObjectState>();
		for (int i = 0; i < this.children.Count; i++)
		{
			list.Add(this.children[i].objectState);
		}
		return list;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00041C48 File Offset: 0x0003FE48
	public bool IsChildOrMe(GameObject go)
	{
		if (base.gameObject == go)
		{
			return true;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].child == go)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00041C98 File Offset: 0x0003FE98
	private void UpdateChildren()
	{
		if (this.NPO)
		{
			foreach (ChildSpawner.ChildInfo childInfo in this.children)
			{
				foreach (Renderer renderer in childInfo.child.GetComponentsInChildren<Renderer>())
				{
					if (renderer.enabled)
					{
						this.NPO.AddRenderer(renderer);
					}
				}
			}
			this.NPO.ResetBounds();
			this.NPO.ResetPhysicsMaterial();
			if (this.NPO.highlighter)
			{
				this.NPO.highlighter.SetDirty();
			}
			this.NPO.rigidbody.ResetCenterOfMass();
			this.NPO.rigidbody.ResetInertiaTensor();
			this.NPO.UpdateVisiblity(false);
		}
	}

	// Token: 0x04000682 RID: 1666
	private List<ChildSpawner.ChildInfo> children = new List<ChildSpawner.ChildInfo>();

	// Token: 0x04000683 RID: 1667
	private NetworkPhysicsObject NPO;

	// Token: 0x0200058A RID: 1418
	private class ChildInfo
	{
		// Token: 0x06003861 RID: 14433 RVA: 0x0016C876 File Offset: 0x0016AA76
		public ChildInfo(GameObject child, ObjectState objectState)
		{
			this.child = child;
			this.objectState = objectState;
		}

		// Token: 0x04002540 RID: 9536
		public GameObject child;

		// Token: 0x04002541 RID: 9537
		public ObjectState objectState;
	}
}
