using System;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using NewNet;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class StackObject : NetworkBehavior
{
	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06001C2D RID: 7213 RVA: 0x000C1F76 File Offset: 0x000C0176
	// (set) Token: 0x06001C2E RID: 7214 RVA: 0x000C1F7E File Offset: 0x000C017E
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public OrderType Order
	{
		get
		{
			return this._Order;
		}
		set
		{
			if (this._Order != value)
			{
				this._Order = value;
				base.DirtySync("Order");
			}
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06001C2F RID: 7215 RVA: 0x000C1F9B File Offset: 0x000C019B
	// (set) Token: 0x06001C30 RID: 7216 RVA: 0x000C1FA3 File Offset: 0x000C01A3
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int num_objects_
	{
		get
		{
			return this._num_objects_;
		}
		set
		{
			if (this.IsInfiniteStack)
			{
				return;
			}
			if (value != this._num_objects_)
			{
				this._num_objects_ = value;
				base.DirtySync("num_objects_");
				base.StartCoroutine(this.UpdateNumberObjects());
			}
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06001C31 RID: 7217 RVA: 0x000C1FD6 File Offset: 0x000C01D6
	private float ScaledSpacing
	{
		get
		{
			return this.Spacing * this.StackNPO.Scale.y;
		}
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000C1FF0 File Offset: 0x000C01F0
	private void Awake()
	{
		this.StackNPO = base.GetComponent<NetworkPhysicsObject>();
		this.StackNPO.SetTypedNumberHandlers(new NetworkPhysicsObject.MaxTypedNumberMethodDelegate(StackObject.MaxTypedNumber), new NetworkPhysicsObject.HandleTypedNumberMethodDelegate(StackObject.HandleTypedNumber), false);
		this.ResetColliderOffset();
		this.customObject = this.StackNPO.customObject;
		if (this.customObject)
		{
			this.Spacing = -1f;
		}
		if (this.bagAnimation != null)
		{
			this.bagAnimation["Closing"].speed = 4f;
			this.bagAnimation["Opening"].speed = 4f;
			this.bagAnimation.CrossFade("Closed", 0f, PlayMode.StopAll);
		}
		this.playersSearchingInventory = base.gameObject.AddComponent<PlayersSearchingInventory>();
		base.networkView.RegisterNetworkBehavior(this.playersSearchingInventory);
		if (this.IsInfiniteStack)
		{
			EventManager.OnDummyObjectFinish += this.EventManager_OnDummyObjectFinish;
		}
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		if (this.IsInfiniteStack)
		{
			this.num_objects_ = 99;
		}
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000C2110 File Offset: 0x000C0310
	private void Start()
	{
		this.StackBox = base.GetComponent<BoxCollider>();
		if (base.GetComponent<SoundScript>() && this.num_objects_ == 2)
		{
			base.GetComponent<SoundScript>().PickUpSound();
		}
		if (Network.isServer && !base.GetComponent<Rigidbody>().isKinematic)
		{
			base.GetComponent<Rigidbody>().velocity /= 3f;
		}
		this.playersSearchingInventory.Init();
		base.StartCoroutine(this.UpdateNumberObjects());
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x000C2191 File Offset: 0x000C0391
	private void OnDestroy()
	{
		if (this.IsInfiniteStack)
		{
			EventManager.OnDummyObjectFinish -= this.EventManager_OnDummyObjectFinish;
		}
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x000C21C0 File Offset: 0x000C03C0
	public void ResetColliderOffset()
	{
		BoxCollider componentInChildren = base.GetComponentInChildren<BoxCollider>();
		if (componentInChildren)
		{
			this.ColliderOffset = componentInChildren.center.y;
		}
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000C21F0 File Offset: 0x000C03F0
	private void AddObject()
	{
		Vector3 position = new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.05f, 0.05f) + this.ScaledSpacing * ((float)this.StackFacades.Count + 1f) * base.transform.up.normalized.x, base.transform.position.y + this.ScaledSpacing * ((float)this.StackFacades.Count + 1f) * base.transform.up.normalized.y, base.transform.position.z + UnityEngine.Random.Range(-0.05f, 0.05f) + this.ScaledSpacing * ((float)this.StackFacades.Count + 1f) * base.transform.up.normalized.z);
		Quaternion rotation = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z));
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.StackObject_prefab, position, rotation);
		this.StackFacades.Add(gameObject);
		gameObject.transform.parent = base.transform;
		gameObject.GetComponent<MeshFilter>().mesh = base.GetComponent<MeshFilter>().sharedMesh;
		if (base.GetComponent<Highlighter>())
		{
			base.GetComponent<Highlighter>().SetDirty();
		}
		if (base.GetComponent<SoundScript>())
		{
			base.GetComponent<SoundScript>().PickUpSound();
		}
		if (Network.isServer && !base.GetComponent<Rigidbody>().isKinematic)
		{
			base.GetComponent<Rigidbody>().velocity /= 3f;
		}
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000C23E4 File Offset: 0x000C05E4
	private void RemoveObject()
	{
		GameObject gameObject = this.StackFacades[this.StackFacades.Count - 1];
		this.StackFacades.Remove(gameObject);
		UnityEngine.Object.Destroy(gameObject);
		if (base.GetComponent<Highlighter>())
		{
			base.GetComponent<Highlighter>().SetDirty();
		}
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x000C2438 File Offset: 0x000C0638
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public GameObject RemoveItemRPC(int index, Vector3 pos)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, Vector3, GameObject>(RPCTarget.Server, new Func<int, Vector3, GameObject>(this.RemoveItemRPC), index, pos);
			return null;
		}
		if (this.num_objects_ <= 0)
		{
			return null;
		}
		base.GetComponent<SoundScript>().PickUpSound();
		if (index < 0 || index > this.ObjectsHolder.Count - 1)
		{
			Debug.LogError("Index not found in Bag: " + index);
			return null;
		}
		ObjectState objectState = this.ObjectsHolder[index];
		this.ObjectsHolder.RemoveAt(index);
		int num_objects_ = this.num_objects_ - 1;
		this.num_objects_ = num_objects_;
		objectState.Transform.posX = pos.x;
		objectState.Transform.posY = pos.y;
		objectState.Transform.posZ = pos.z;
		objectState.Locked = false;
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
		NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
		Vector3 zero = Vector3.zero;
		Bounds boundsNotNormalized = component.GetBoundsNotNormalized(out zero);
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + boundsNotNormalized.extents.y + zero.y, gameObject.transform.position.z);
		if (pos != base.transform.position)
		{
			base.StartCoroutine(NetworkSingleton<ManagerPhysicsObject>.Instance.DelaySnapToGrid(gameObject));
		}
		if (Network.isServer)
		{
			base.GetComponent<Rigidbody>().AddForce(Vector3.down);
		}
		EventManager.TriggerObjectLeaveContainer(this.StackNPO, component);
		return gameObject;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x000C25D4 File Offset: 0x000C07D4
	public GameObject RemoveItemByGUID(string GUID, Vector3 pos)
	{
		for (int i = 0; i < this.ObjectsHolder.Count; i++)
		{
			if (this.ObjectsHolder[i].GUID == GUID)
			{
				return this.RemoveItemRPC(i, pos);
			}
		}
		return null;
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x000C261C File Offset: 0x000C081C
	public GameObject TakeObject(bool bSpawnTop = true)
	{
		int num = 0;
		if (base.transform.up.y >= 0f && bSpawnTop)
		{
			num = this.StackFacades.Count;
		}
		if (base.transform.up.y <= 0f && !bSpawnTop)
		{
			num = this.StackFacades.Count;
		}
		Vector3 vector;
		if (bSpawnTop)
		{
			vector = new Vector3(base.transform.position.x, base.transform.position.y + this.Spacing * (float)(num + 1) * base.GetComponent<NetworkPhysicsObject>().Scale.y + 0.1f, base.transform.position.z);
		}
		else
		{
			vector = new Vector3(base.transform.position.x, base.transform.position.y - this.Spacing * (float)(num + 1) * base.GetComponent<NetworkPhysicsObject>().Scale.y - 0.1f, base.transform.position.z);
		}
		int num_objects_;
		if (!this.bBag)
		{
			num_objects_ = this.num_objects_ - 1;
			this.num_objects_ = num_objects_;
			GameObject gameObject = null;
			if (!this.IsInfiniteStack)
			{
				if (this.num_objects_ <= 0)
				{
					return this.LastObject;
				}
				if (this.num_objects_ <= 1)
				{
					ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
					objectState.Name = this.GameObjectHolder.name;
					if (base.transform.up.y <= 0f)
					{
						objectState.Transform.posY -= this.Spacing;
					}
					this.LastObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
				}
				ObjectState objectState2 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				objectState2.Name = this.GameObjectHolder.name;
				objectState2.Transform.posX = vector.x;
				objectState2.Transform.posY = vector.y;
				objectState2.Transform.posZ = vector.z;
				gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState2, false, false);
				gameObject.GetComponent<NetworkPhysicsObject>().IsLocked = false;
			}
			else if (this.GameObjectHolder)
			{
				Quaternion rotation = base.transform.rotation;
				gameObject = Network.Instantiate(this.GameObjectHolder, vector, rotation, default(NetworkPlayer));
				gameObject.GetComponent<NetworkPhysicsObject>().SetScale(base.GetComponent<NetworkPhysicsObject>().Scale, false);
				gameObject.GetComponent<NetworkPhysicsObject>().IgnoresGrid = base.GetComponent<NetworkPhysicsObject>().IgnoresGrid;
			}
			else if (this.ObjectsHolder.Count > 0)
			{
				ObjectState objectState3 = this.ObjectsHolder[0];
				Vector3 vector2 = objectState3.Transform.Position();
				Vector3 vector3 = objectState3.Transform.Rotation();
				objectState3.Transform.posX = base.transform.position.x;
				objectState3.Transform.posY = base.transform.position.y + 1f;
				objectState3.Transform.posZ = base.transform.position.z;
				if (this.InfiniteObject)
				{
					objectState3.Transform.rotX = this.InfiniteObject.transform.eulerAngles.x;
					objectState3.Transform.rotY = this.InfiniteObject.transform.eulerAngles.y;
					objectState3.Transform.rotZ = this.InfiniteObject.transform.eulerAngles.z;
				}
				else
				{
					Quaternion quaternion = Quaternion.Euler(objectState3.Transform.Rotation()) * base.transform.rotation;
					objectState3.Transform.rotX = quaternion.eulerAngles.x;
					objectState3.Transform.rotY = quaternion.eulerAngles.y;
					objectState3.Transform.rotZ = quaternion.eulerAngles.z;
				}
				gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState3, false, false);
				objectState3.Transform.posX = vector2.x;
				objectState3.Transform.posY = vector2.y;
				objectState3.Transform.posZ = vector2.z;
				objectState3.Transform.rotX = vector3.x;
				objectState3.Transform.rotY = vector3.y;
				objectState3.Transform.rotZ = vector3.z;
			}
			if (gameObject && gameObject.GetComponent<NetworkPhysicsObject>())
			{
				gameObject.GetComponent<NetworkPhysicsObject>().IsLocked = false;
				EventManager.TriggerObjectLeaveContainer(this.StackNPO, gameObject.GetComponent<NetworkPhysicsObject>());
			}
			return gameObject;
		}
		if (this.ObjectsHolder.Count < 1)
		{
			return null;
		}
		int index;
		switch (this.Order)
		{
		case OrderType.LIFO:
			index = this.ObjectsHolder.Count - 1;
			break;
		case OrderType.FILO:
			index = 0;
			break;
		case OrderType.Random:
			index = UnityEngine.Random.Range(0, this.ObjectsHolder.Count);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		ObjectState objectState4 = this.ObjectsHolder[index];
		objectState4.Transform.posX = vector.x;
		if (bSpawnTop)
		{
			objectState4.Transform.posY = vector.y + 1f;
		}
		else
		{
			objectState4.Transform.posY = vector.y - 0.5f;
		}
		objectState4.Transform.posZ = vector.z;
		this.ObjectsHolder.RemoveAt(index);
		num_objects_ = this.num_objects_ - 1;
		this.num_objects_ = num_objects_;
		GameObject gameObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState4, false, false);
		NetworkPhysicsObject component = gameObject2.GetComponent<NetworkPhysicsObject>();
		EventManager.TriggerObjectLeaveContainer(this.StackNPO, component);
		return gameObject2;
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x000C2BE8 File Offset: 0x000C0DE8
	private IEnumerator UpdateNumberObjects()
	{
		if (this.IsInfiniteStack || this.bBag)
		{
			yield break;
		}
		if (this.customObject)
		{
			while (this.customObject.CurrentlyLoading() || this.Spacing == -1f)
			{
				yield return null;
			}
		}
		BoxCollider componentInChildren = base.GetComponentInChildren<BoxCollider>();
		float num = this.Spacing * (float)this.num_objects_;
		if (Network.isServer && base.transform.up.y <= 0f)
		{
			float num2 = num - componentInChildren.size.y;
			this.StackNPO.rigidbody.position = new Vector3(base.transform.position.x, base.transform.position.y + num2, base.transform.position.z);
		}
		componentInChildren.size = new Vector3(componentInChildren.size.x, num, componentInChildren.size.z);
		componentInChildren.center = new Vector3(componentInChildren.center.x, this.Spacing * ((float)this.num_objects_ - 1f) / 2f + this.ColliderOffset, componentInChildren.center.z);
		this.StackNPO.SetMass(1f + (float)this.num_objects_ * this.mass_per_object);
		this.StackNPO.ResetBounds();
		if (this.num_objects_ - 1 > this.StackFacades.Count)
		{
			int num3 = this.num_objects_ - 1 - this.StackFacades.Count;
			for (int i = 0; i < num3; i++)
			{
				this.AddObject();
			}
		}
		if (this.num_objects_ - 1 < this.StackFacades.Count)
		{
			int num4 = this.num_objects_ - 1 - this.StackFacades.Count;
			num4 *= -1;
			for (int j = 0; j < num4; j++)
			{
				this.RemoveObject();
			}
		}
		if (this.num_objects_ <= 1)
		{
			if (this.StackNPO)
			{
				this.StackNPO.SetInvisible("stackObject", true, 2147483647U, false, false);
				this.StackNPO.SetCollision(false);
			}
			if (Network.isServer)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
			}
		}
		yield break;
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x000C2BF8 File Offset: 0x000C0DF8
	public void SetChecker(int material)
	{
		this.MaterialInt = material;
		if (material == -1)
		{
			return;
		}
		if (material == 0)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.CheckerRed;
		}
		else if (material == 1)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.CheckerBlack;
		}
		else if (material == 2)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.CheckerWhite;
		}
		base.GetComponent<MaterialSyncScript>().SetMaterial(material);
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x000C2C5C File Offset: 0x000C0E5C
	public void SetPokerChip(int mesh)
	{
		this.MeshInt = mesh;
		if (mesh == -1)
		{
			return;
		}
		if (mesh == 10)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.Poker10;
			base.GetComponent<MeshSyncScript>().SetMesh(4);
			return;
		}
		if (mesh == 50)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.Poker50;
			base.GetComponent<MeshSyncScript>().SetMesh(0);
			return;
		}
		if (mesh == 100)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.Poker100;
			base.GetComponent<MeshSyncScript>().SetMesh(1);
			return;
		}
		if (mesh == 500)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.Poker500;
			base.GetComponent<MeshSyncScript>().SetMesh(2);
			return;
		}
		if (mesh == 1000)
		{
			this.GameObjectHolder = NetworkSingleton<GameMode>.Instance.Poker1000;
			base.GetComponent<MeshSyncScript>().SetMesh(3);
		}
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x000C2D24 File Offset: 0x000C0F24
	public void RefreshChildren()
	{
		for (int i = 0; i < this.StackFacades.Count; i++)
		{
			this.StackFacades[i].GetComponent<Renderer>().materials = base.GetComponent<Renderer>().materials;
			this.StackFacades[i].GetComponent<MeshFilter>().mesh = base.GetComponent<MeshFilter>().sharedMesh;
		}
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x000C2D89 File Offset: 0x000C0F89
	public bool AnyoneSearchingBag()
	{
		return this.playersSearchingInventory.AnyoneSearching();
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x000C2D96 File Offset: 0x000C0F96
	public bool ShuffleBag()
	{
		if (this.AnyoneSearchingBag())
		{
			return false;
		}
		if (this.bBag)
		{
			this.ObjectsHolder.Randomize<ObjectState>();
		}
		if (base.GetComponent<SoundScript>())
		{
			base.GetComponent<SoundScript>().ShakeSound();
		}
		return true;
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x000C2DCE File Offset: 0x000C0FCE
	public bool CheckStackable(GameObject Hit_Obj)
	{
		return this.CheckStackable(Hit_Obj.GetComponent<NetworkPhysicsObject>());
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x000C2DDC File Offset: 0x000C0FDC
	public bool CheckStackable(NetworkPhysicsObject NPO)
	{
		return StackObject.CheckStackable(NPO, this.StackNPO);
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x000C2DEC File Offset: 0x000C0FEC
	public static bool CheckStackable(NetworkPhysicsObject NPO, NetworkPhysicsObject NetworkPO)
	{
		return (!NetworkPO.stackObject || (!NetworkPO.stackObject.IsInfiniteStack && !NetworkPO.stackObject.bBag)) && NetworkPO.GetComponent<MeshFilter>() && NPO.GetComponent<MeshFilter>() && (!(NetworkPO.GetComponent<MeshFilter>().sharedMesh != null) || !(NPO.GetComponent<MeshFilter>().sharedMesh != null) || !(NetworkPO.GetComponent<MeshFilter>().sharedMesh.ToString() != NPO.GetComponent<MeshFilter>().sharedMesh.ToString())) && !(NetworkPO.Name != NPO.Name) && (!NetworkPO.customObject || !NPO.customObject || !(NetworkPO.customObject.GetType() != NPO.customObject.GetType())) && (!NetworkPO.customObject || NPO.customObject) && (NetworkPO.customObject || !NPO.customObject) && !(NetworkPO.DiffuseColor != NPO.DiffuseColor) && (!NetworkPO.customMesh || !NPO.customMesh || (!(NetworkPO.customMesh.customMeshState.MeshURL != NPO.customMesh.customMeshState.MeshURL) && !(NetworkPO.customMesh.customMeshState.DiffuseURL != NPO.customMesh.customMeshState.DiffuseURL) && NetworkPO.customMesh.customMeshState.TypeIndex == NPO.customMesh.customMeshState.TypeIndex)) && (!NetworkPO.customImage || !NPO.customImage || (!(NetworkPO.customImage.CustomImageURL != NPO.customImage.CustomImageURL) && !(NetworkPO.customImage.CustomImageSecondaryURL != NPO.customImage.CustomImageSecondaryURL))) && (!NetworkPO.customAssetbundle || !NPO.customAssetbundle || (!(NetworkPO.customAssetbundle.CustomAssetbundleURL != NPO.customAssetbundle.CustomAssetbundleURL) && !(NetworkPO.customAssetbundle.CustomAssetbundleSecondaryURL != NPO.customAssetbundle.CustomAssetbundleSecondaryURL))) && NetworkPO.ValueFlags == NPO.ValueFlags && NetworkPO.Value == NPO.Value && ComponentTags.FlagsAreIdentical(NetworkPO.tags, NPO.tags);
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x000C3094 File Offset: 0x000C1294
	private void OnCollisionEnter(Collision info)
	{
		if (Network.isServer)
		{
			if (this.AnyoneSearchingBag())
			{
				return;
			}
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(info.collider);
			NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
			if (component.CurrentPlayerHand || this.StackNPO.CurrentPlayerHand)
			{
				return;
			}
			if (component && this.CheckStackable(component) && this.StackNPO.IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID && component.IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.StackHitNPO(base.gameObject.GetComponent<NetworkPhysicsObject>(), gameObject.GetComponent<NetworkPhysicsObject>(), true);
				return;
			}
			if (this.IsInfiniteStack && component && !component.IsLocked)
			{
				if (!ManagerPhysicsObject.CloseEnoughForContainerMerge(this.StackNPO, component) || this.StackNPO.InsideALayoutZone || component.InsideALayoutZone)
				{
					return;
				}
				if (((this.GameObjectHolder && gameObject.CompareTag(base.gameObject.tag)) || !this.GameObjectHolder) && (!component.stackObject || !this.GameObjectHolder) && component.IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID && this.StackNPO.IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID && gameObject.transform.position.y > base.transform.position.y + this.BagYCheck)
				{
					if (!this.GameObjectHolder)
					{
						LuaGameObjectScript component2 = base.GetComponent<LuaGameObjectScript>();
						if (component2 != null && !component2.CheckObjectEnter(component))
						{
							return;
						}
						if (!this.AddToInfiniteBag(component))
						{
							return;
						}
					}
					else
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(gameObject);
						this.StackNPO.soundScript.ShakeSound();
					}
					if (Network.isServer && !base.GetComponent<Rigidbody>().isKinematic)
					{
						base.GetComponent<Rigidbody>().velocity /= 3f;
					}
					int prevHeldByPlayerID = gameObject.GetComponent<NetworkPhysicsObject>().PrevHeldByPlayerID;
					Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(prevHeldByPlayerID);
					if (pointer)
					{
						List<NetworkPhysicsObject> recentlyDropped = pointer.RecentlyDropped;
						for (int i = 0; i < recentlyDropped.Count; i++)
						{
							NetworkPhysicsObject networkPhysicsObject = recentlyDropped[i];
							if (networkPhysicsObject && !(networkPhysicsObject.gameObject == gameObject) && !(networkPhysicsObject.gameObject == base.gameObject) && ((networkPhysicsObject && this.GameObjectHolder && gameObject.CompareTag(base.gameObject.tag)) || (!this.GameObjectHolder && (!gameObject.GetComponent<StackObject>() || !this.GameObjectHolder))))
							{
								if (!this.GameObjectHolder)
								{
									ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(networkPhysicsObject);
									if (this.ObjectsHolder.Count == 0 || !objectState.EqualsObject(this.ObjectsHolder[0]))
									{
										goto IL_32B;
									}
								}
								networkPhysicsObject.SetSmoothDestroy(new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z));
							}
							IL_32B:;
						}
						return;
					}
				}
			}
			else if (this.bBag && component && component.IsHeldByNobodyAndIsNotMarkedDestroyedInHeldID && gameObject.transform.position.y > base.transform.position.y + this.BagYCheck && !component.tableScript && !component.IsLocked && component.IsGrabbable && !component.IsDestroyed)
			{
				LuaGameObjectScript component3 = base.GetComponent<LuaGameObjectScript>();
				if (!ManagerPhysicsObject.CloseEnoughForContainerMerge(this.StackNPO, component) || this.StackNPO.InsideALayoutZone || component.InsideALayoutZone)
				{
					return;
				}
				if (component3 == null || component3.CheckObjectEnter(component))
				{
					this.AddToBag(component);
					if (Network.isServer && !base.GetComponent<Rigidbody>().isKinematic)
					{
						base.GetComponent<Rigidbody>().velocity /= 3f;
					}
				}
				int prevHeldByPlayerID2 = gameObject.GetComponent<NetworkPhysicsObject>().PrevHeldByPlayerID;
				Pointer pointer2 = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(prevHeldByPlayerID2);
				if (pointer2)
				{
					foreach (NetworkPhysicsObject networkPhysicsObject2 in pointer2.RecentlyDropped)
					{
						if (networkPhysicsObject2 && !networkPhysicsObject2.IsDestroyed && !(networkPhysicsObject2.gameObject == gameObject) && !(networkPhysicsObject2.gameObject == base.gameObject) && (!(component3 != null) || component3.CheckObjectEnter(networkPhysicsObject2)))
						{
							this.AddToBag(networkPhysicsObject2);
						}
					}
				}
			}
		}
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x000C3590 File Offset: 0x000C1790
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, serializationMethod = SerializationMethod.Json)]
	public void SetSearch(List<ObjectState> objectStates, int maxCards)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<List<ObjectState>, int>(RPCTarget.Server, new Action<List<ObjectState>, int>(this.SetSearch), objectStates, maxCards);
			return;
		}
		this.ObjectsHolder.Clear();
		foreach (ObjectState item in objectStates)
		{
			this.ObjectsHolder.Add(item);
		}
		this.num_objects_ = this.ObjectsHolder.Count;
		this.playersSearchingInventory.SetSearch((int)Network.sender.id, this.StackNPO.ID, objectStates, maxCards);
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000C3648 File Offset: 0x000C1848
	private void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isServer && this.InfiniteObject)
		{
			base.networkView.RPC<byte[]>(player, new Action<byte[]>(this.RPCAddToInfiniteBag), Json.GetBson(this.ObjectsHolder[0]));
		}
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x000C3688 File Offset: 0x000C1888
	public bool AddToInfiniteBag(NetworkPhysicsObject NPO)
	{
		ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(NPO);
		NPO.transform.parent = base.transform;
		Vector3 localEulerAngles = NPO.transform.localEulerAngles;
		NPO.transform.parent = null;
		objectState.Transform.rotX = localEulerAngles.x;
		objectState.Transform.rotY = localEulerAngles.y;
		objectState.Transform.rotZ = localEulerAngles.z;
		bool flag = this.AddToInfiniteBag(objectState);
		if (flag)
		{
			EventManager.TriggerObjectEnterContainer(this.StackNPO, NPO);
			NPO.SetSmoothDestroy(new Vector3(base.transform.position.x, base.transform.position.y + this.BagYCheck, base.transform.position.z));
			this.StackNPO.soundScript.ShakeSound();
		}
		return flag;
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x000C3768 File Offset: 0x000C1968
	public bool AddToInfiniteBag(ObjectState OS)
	{
		if (this.StackNPO.customObject && this.StackNPO.customObject.DummyObject)
		{
			return false;
		}
		if (this.ObjectsHolder.Count == 0)
		{
			base.networkView.RPC<byte[]>(RPCTarget.All, new Action<byte[]>(this.RPCAddToInfiniteBag), Json.GetBson(OS));
			return true;
		}
		return OS.EqualsObject(this.ObjectsHolder[0]);
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x000C37E0 File Offset: 0x000C19E0
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCAddToInfiniteBag(byte[] objectStateBytes)
	{
		ObjectState objectState = Json.Load<ObjectState>(objectStateBytes, false);
		this.ObjectsHolder.Add(objectState);
		Transform transform = base.transform.Find("ObjectPoint");
		if (transform)
		{
			this.InfiniteObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, true, false);
			this.DisableCollidersInInfiniteObject();
			this.InfiniteObject.transform.rotation = Quaternion.Euler(objectState.Transform.Rotation()) * base.transform.rotation;
			this.InfiniteObject.transform.position = transform.position;
			this.InfiniteObject.transform.parent = base.transform;
			this.StackNPO.UpdateVisiblity(false);
			this.ResizeInfiniteObject();
		}
		if (this.bagAnimation != null)
		{
			this.bagAnimation.CrossFade("Opening", 0.25f, PlayMode.StopAll);
		}
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000C38C8 File Offset: 0x000C1AC8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCResetInfiniteBag()
	{
		this.ObjectsHolder.Clear();
		UnityEngine.Object.Destroy(this.InfiniteObject);
		if (this.bagAnimation != null)
		{
			this.bagAnimation.CrossFade("Closing", 0.25f, PlayMode.StopAll);
		}
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x000C3904 File Offset: 0x000C1B04
	private void EventManager_OnDummyObjectFinish(GameObject dummyGameObject)
	{
		if (this.InfiniteObject && this.InfiniteObject.GetComponent<ChildSpawner>().IsChildOrMe(dummyGameObject))
		{
			this.DisableCollidersInInfiniteObject();
			this.ResizeInfiniteObject();
		}
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x000C3934 File Offset: 0x000C1B34
	private void DisableCollidersInInfiniteObject()
	{
		Collider[] componentsInChildren = this.InfiniteObject.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x000C3964 File Offset: 0x000C1B64
	public void ResizeInfiniteObject()
	{
		if (this.InfiniteObject)
		{
			Bounds lhs = default(Bounds);
			foreach (Renderer renderer in this.InfiniteObject.GetComponentsInChildren<Renderer>(true))
			{
				if (lhs == default(Bounds))
				{
					lhs = renderer.bounds;
				}
				else
				{
					lhs.Encapsulate(renderer.bounds);
				}
			}
			float num = this.StackNPO.GetBounds().size.x * 0.44f;
			float num2 = num / lhs.size.x;
			if (num / lhs.size.z < num2)
			{
				num2 = num / lhs.size.z;
			}
			if (num2 < 1f)
			{
				this.InfiniteObject.transform.localScale = this.InfiniteObject.transform.localScale * num2;
			}
		}
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x000C3A58 File Offset: 0x000C1C58
	public void ResetObjectsContained()
	{
		if (this.ObjectsHolder.Count > 0)
		{
			this.ObjectsHolder.Clear();
			if (this.InfiniteObject)
			{
				base.networkView.RPC(RPCTarget.All, new Action(this.RPCResetInfiniteBag));
			}
			this.num_objects_ = 0;
		}
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x000C3AAC File Offset: 0x000C1CAC
	public void AddToBag(NetworkPhysicsObject NPO)
	{
		this.AddToBag(NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(NPO));
		NPO.SetSmoothDestroy(new Vector3(base.transform.position.x, base.transform.position.y + this.BagYCheck, base.transform.position.z));
		EventManager.TriggerObjectEnterContainer(this.StackNPO, NPO);
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x000C3B18 File Offset: 0x000C1D18
	public void AddToBag(ObjectState OS)
	{
		int num_objects_ = this.num_objects_;
		this.num_objects_ = num_objects_ + 1;
		base.GetComponent<SoundScript>().ShakeSound();
		this.ObjectsHolder.Add(OS);
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x000C3B4C File Offset: 0x000C1D4C
	private static int MaxTypedNumber(NetworkPhysicsObject npo)
	{
		if (Pointer.DEFAULT_TYPED_NUMBER_IS_SINGLE_DIGIT)
		{
			return 9;
		}
		return npo.stackObject.num_objects_;
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x000C3B64 File Offset: 0x000C1D64
	private static void HandleTypedNumber(NetworkPhysicsObject npo, int playerID, int number)
	{
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, PlayerScript.PointerScript.PointerColorLabel, number, 0);
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, NetworkSingleton<PlayerManager>.Instance.ColourLabelFromID(playerID), number, 0);
	}

	// Token: 0x040011E3 RID: 4579
	public bool IsInfiniteStack;

	// Token: 0x040011E4 RID: 4580
	public bool bBag;

	// Token: 0x040011E5 RID: 4581
	private OrderType _Order;

	// Token: 0x040011E6 RID: 4582
	public float BagYCheck = 1f;

	// Token: 0x040011E7 RID: 4583
	[SerializeField]
	private int _num_objects_ = 2;

	// Token: 0x040011E8 RID: 4584
	public float Spacing = 0.135f;

	// Token: 0x040011E9 RID: 4585
	public PlayersSearchingInventory playersSearchingInventory;

	// Token: 0x040011EA RID: 4586
	private float mass_per_object = 0.025f;

	// Token: 0x040011EB RID: 4587
	public List<ObjectState> ObjectsHolder = new List<ObjectState>();

	// Token: 0x040011EC RID: 4588
	public int MeshInt = -1;

	// Token: 0x040011ED RID: 4589
	public int MaterialInt = -1;

	// Token: 0x040011EE RID: 4590
	public GameObject StackObject_prefab;

	// Token: 0x040011EF RID: 4591
	public GameObject GameObjectHolder;

	// Token: 0x040011F0 RID: 4592
	private BoxCollider StackBox;

	// Token: 0x040011F1 RID: 4593
	private NetworkPhysicsObject StackNPO;

	// Token: 0x040011F2 RID: 4594
	private CustomObject customObject;

	// Token: 0x040011F3 RID: 4595
	private float ColliderOffset;

	// Token: 0x040011F4 RID: 4596
	public GameObject LastObject;

	// Token: 0x040011F5 RID: 4597
	public GameObject InfiniteObject;

	// Token: 0x040011F6 RID: 4598
	private List<GameObject> StackFacades = new List<GameObject>();

	// Token: 0x040011F7 RID: 4599
	public Animation bagAnimation;
}
