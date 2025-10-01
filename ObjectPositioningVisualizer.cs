using System;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class ObjectPositioningVisualizer : Singleton<ObjectPositioningVisualizer>
{
	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x060020EC RID: 8428 RVA: 0x000EE762 File Offset: 0x000EC962
	public bool VisualizeDrop
	{
		get
		{
			return ObjectPositioningVisualizer.VisualizeGrabbedObjects || this.SpawningObjects;
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x060020ED RID: 8429 RVA: 0x000EE773 File Offset: 0x000EC973
	public bool SpawnOK
	{
		get
		{
			return this.visualizerActive && this.objectVisualizer != null && this.objectVisualizer.visual != null;
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x060020EE RID: 8430 RVA: 0x000EE798 File Offset: 0x000EC998
	public Vector3 SpawnLocation
	{
		get
		{
			if (this.objectVisualizer == null || this.objectVisualizer.visual == null)
			{
				return Vector3.zero;
			}
			return this.objectVisualizer.visual.transform.position;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x060020EF RID: 8431 RVA: 0x000EE7D0 File Offset: 0x000EC9D0
	public Vector3 SpawnRotation
	{
		get
		{
			if (this.objectVisualizer == null || this.objectVisualizer.visual == null)
			{
				return Vector3.zero;
			}
			return this.objectVisualizer.visual.transform.eulerAngles;
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x060020F0 RID: 8432 RVA: 0x000EE808 File Offset: 0x000ECA08
	public bool ForceSpawnInPlace
	{
		get
		{
			return this.objectVisualizer != null && this.objectVisualizer.forceSpawnInPlace;
		}
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x000EE81F File Offset: 0x000ECA1F
	protected override void Awake()
	{
		base.Awake();
		this.shader = Shader.Find("Transparent/Diffuse");
		EventManager.OnLateFixedUpdate += this.LateFixedUpdate;
		EventManager.OnDummyObjectFinish += this.EventManager_OnDummyObjectFinish;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000EE859 File Offset: 0x000ECA59
	private void OnDestroy()
	{
		EventManager.OnLateFixedUpdate -= this.LateFixedUpdate;
		EventManager.OnDummyObjectFinish -= this.EventManager_OnDummyObjectFinish;
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000EE880 File Offset: 0x000ECA80
	private void EventManager_OnDummyObjectFinish(GameObject dummyGameObject)
	{
		if (this.objectVisualizer == null || this.objectVisualizer.visual == null)
		{
			return;
		}
		ChildSpawner[] componentsInChildren = this.objectVisualizer.visual.GetComponentsInChildren<ChildSpawner>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].IsChildOrMe(dummyGameObject))
			{
				this.objectVisualizer.Setup();
				return;
			}
		}
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x000EE8E0 File Offset: 0x000ECAE0
	private void LateFixedUpdate()
	{
		this.EnableComponentWindow(!this.SpawningObjects || !ObjectPositioningVisualizer.VisualizerSpawnHideWindow);
		this.visualizerActive = false;
		if (this.SpawningObjects)
		{
			if (this.objectVisualizer == null)
			{
				return;
			}
			this.visualizerActive = true;
			if (this.objectVisualizer != null)
			{
				this.objectVisualizer.visual.SetActive(true);
			}
			Vector3 vector = PlayerScript.PointerScript.GetSpawnPosition();
			Vector3 eulerAngles = this.objectVisualizer.visual.transform.eulerAngles;
			this.objectVisualizer.visual.transform.position = vector;
			Vector3 vector2;
			Vector3 vector3;
			Vector3 vector4;
			if (ObjectPositioningVisualizer.VisualizerSpawnObjectSnap && HoverScript.HoverObject != null)
			{
				vector = HoverScript.HoverObject.transform.position;
			}
			else if (NetworkSingleton<ManagerPhysicsObject>.Instance.CheckPointSnap(this.objectVisualizer.visual, out vector2, out vector3, 99999f))
			{
				vector = vector2;
			}
			else if (NetworkSingleton<ManagerPhysicsObject>.Instance.CheckGridSnap(this.objectVisualizer.visual.transform.position, out vector4))
			{
				vector = vector4;
			}
			this.SpawnBounds = default(Bounds);
			foreach (Collider collider in this.objectVisualizer.visual.GetComponentsInChildren<Collider>())
			{
				bool enabled = collider.enabled;
				collider.enabled = true;
				if (this.SpawnBounds == default(Bounds))
				{
					this.SpawnBounds = collider.bounds;
				}
				else
				{
					this.SpawnBounds.Encapsulate(collider.bounds);
				}
				collider.enabled = enabled;
			}
			Vector3 zero = Vector3.zero;
			Rigidbody[] componentsInChildren2 = this.objectVisualizer.visual.GetComponentsInChildren<Rigidbody>();
			int i = 0;
			if (i < componentsInChildren2.Length)
			{
				Rigidbody rigidbody = componentsInChildren2[i];
				if (this.SpawnBounds == default(Bounds))
				{
					this.SpawnBounds = new Bounds(rigidbody.position, Vector3.zero);
				}
				zero = new Vector3(rigidbody.position.x - this.SpawnBounds.center.x, rigidbody.position.y - this.SpawnBounds.center.y, rigidbody.position.z - this.SpawnBounds.center.z);
			}
			Vector3 vector5 = new Vector3(vector.x - zero.x, 100f, vector.z - zero.z);
			float maxDistance = vector5.y + this.SpawnBounds.size.y + zero.y + PlayerScript.PointerScript.GetLiftHeightOffset();
			RaycastHit raycastHit;
			if (Physics.BoxCast(vector5, this.SpawnBounds.extents, Vector3.down, out raycastHit, Quaternion.identity, maxDistance, HoverScript.NonHeldLayerMask))
			{
				vector.y = raycastHit.point.y;
			}
			vector.y += this.SpawnBounds.extents.y + zero.y;
			this.objectVisualizer.visual.transform.position = vector;
			this.objectVisualizer.visual.transform.eulerAngles = eulerAngles;
		}
		else
		{
			if (!this.VisualizeDrop || this.npoToVisualize == null)
			{
				this.removeAt = 0f;
			}
			if (this.removeAt >= 0f)
			{
				if (Time.time >= this.removeAt)
				{
					this.ActuallyRemoveVisualizer();
					return;
				}
				if (this.objectVisualizer != null)
				{
					float num = (this.removeAt - Time.time) / 0.5f;
					this.objectVisualizer.SetAlpha(Mathf.Lerp(0f, ObjectPositioningVisualizer.VisualizerDropAlpha, num * num));
				}
			}
			if (this.npoToVisualize == null)
			{
				return;
			}
			Vector3 position = this.npoToVisualize.transform.position;
			Vector3 eulerAngles2 = this.npoToVisualize.transform.eulerAngles;
			if (PlayerScript.PointerScript.tapping() || this.npoToVisualize.CachedIsInvisible || this.npoToVisualize.InsideALayoutZone || this.npoToVisualize.IsObscuredAndQuestionMark)
			{
				this.visualizerActive = false;
			}
			else if (NetworkSingleton<ManagerPhysicsObject>.Instance.CheckPointSnap(this.npoToVisualize, out position, out eulerAngles2, 99999f))
			{
				this.visualizerActive = ObjectPositioningVisualizer.VisualizeDropSnap;
			}
			else if (NetworkSingleton<ManagerPhysicsObject>.Instance.CheckGridSnap(this.npoToVisualize.gameObject, out position, 99999f))
			{
				this.visualizerActive = ObjectPositioningVisualizer.VisualizeDropGrid;
			}
			else
			{
				this.visualizerActive = ObjectPositioningVisualizer.VisualizeDropFree;
			}
			if (this.visualizerActive)
			{
				if (this.objectVisualizer == null)
				{
					this.objectVisualizer = new ObjectVisualizer(this.npoToVisualize, false);
				}
				if (this.npoToVisualize.CachedIsObscured != this.objectVisualizer.hidden)
				{
					this.objectVisualizer.UpdateHide(this.npoToVisualize.CachedIsObscured);
				}
				Vector3 vector6;
				Bounds boundsNotNormalized = this.npoToVisualize.GetBoundsNotNormalized(out vector6);
				Vector3 vector7 = new Vector3(position.x - vector6.x, this.objectVisualizer.target.transform.position.y - vector6.y, position.z - vector6.z);
				float maxDistance2 = vector7.y + boundsNotNormalized.size.y + vector6.y;
				RaycastHit raycastHit2;
				if (Physics.BoxCast(vector7, boundsNotNormalized.extents, Vector3.down, out raycastHit2, this.objectVisualizer.target.transform.rotation, maxDistance2, HoverScript.NonHeldLayerMask))
				{
					position.y = raycastHit2.point.y;
				}
				position.y += boundsNotNormalized.extents.y + vector6.y;
				this.objectVisualizer.visual.transform.position = position;
				this.objectVisualizer.visual.transform.eulerAngles = eulerAngles2;
				this.objectVisualizer.visual.transform.localScale = LibVector.Mul(this.npoToVisualize.transform.localScale, this.objectVisualizer.scaleMultiplier);
			}
		}
		if (this.objectVisualizer != null)
		{
			this.objectVisualizer.visual.SetActive(this.visualizerActive);
		}
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x000EEF00 File Offset: 0x000ED100
	private NetworkPhysicsObject npoFromTransform(Transform transform)
	{
		NetworkPhysicsObject component = transform.GetComponent<NetworkPhysicsObject>();
		if (component)
		{
			return component;
		}
		if (transform.parent)
		{
			return this.npoFromTransform(transform.parent);
		}
		return null;
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000EEF3C File Offset: 0x000ED13C
	public void AddVisualizer(NetworkPhysicsObject npo)
	{
		if (!this.VisualizeDrop)
		{
			return;
		}
		if (this.removeAt >= 0f)
		{
			this.ActuallyRemoveVisualizer();
		}
		this.npoToVisualize = npo;
		if (this.objectVisualizer != null)
		{
			this.objectVisualizer.Destroy();
			this.objectVisualizer = null;
		}
		this.removeAt = -1f;
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000EEF91 File Offset: 0x000ED191
	public void ActuallyRemoveVisualizer()
	{
		if (this.objectVisualizer != null)
		{
			this.objectVisualizer.Destroy();
			this.objectVisualizer = null;
		}
		this.npoToVisualize = null;
		this.removeAt = -1f;
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x000EEFBF File Offset: 0x000ED1BF
	public void RemoveVisualizer()
	{
		this.removeAt = Time.time + 0.5f;
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x000EEFD2 File Offset: 0x000ED1D2
	public void SetSpawnVisualizer(ObjectVisualizer ov)
	{
		if (this.objectVisualizer != null)
		{
			this.objectVisualizer.Destroy();
		}
		this.objectVisualizer = ov;
		this.SpawningObjects = true;
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x000EEFF5 File Offset: 0x000ED1F5
	public void EndSpawnVisualizer()
	{
		if (this.objectVisualizer != null)
		{
			this.objectVisualizer.Destroy();
		}
		this.SpawningObjects = false;
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000EF014 File Offset: 0x000ED214
	private void EnableComponentWindow(bool enabled)
	{
		if (this.CustomObjectContainer.activeSelf == enabled)
		{
			return;
		}
		this.CustomObjectContainer.SetActive(enabled);
		if (enabled)
		{
			this.ComponentWindow.position = Vector3.zero;
			this.GamesWindow.position = Vector3.zero;
			if (SystemConsole.AutofocusSearch)
			{
				this.ComponentWindow.GetComponent<UIGridMenuObjects>().SearchInput.isSelected = true;
				return;
			}
		}
		else
		{
			this.ComponentWindow.position = new Vector3(-100000f, 0f, 0f);
			this.GamesWindow.position = new Vector3(-100000f, 0f, 0f);
		}
	}

	// Token: 0x0400144F RID: 5199
	public static bool VisualizeGrabbedObjects = true;

	// Token: 0x04001450 RID: 5200
	public static bool VisualizeDropSnap = true;

	// Token: 0x04001451 RID: 5201
	public static bool VisualizeDropGrid = true;

	// Token: 0x04001452 RID: 5202
	public static bool VisualizeDropFree = false;

	// Token: 0x04001453 RID: 5203
	public static float VisualizerDropAlpha = 0.5f;

	// Token: 0x04001454 RID: 5204
	public static float VisualizerSpawnAlpha = 0.75f;

	// Token: 0x04001455 RID: 5205
	public static bool VisualizerSpawnObjectSnap = true;

	// Token: 0x04001456 RID: 5206
	public static bool VisualizerSpawnHideWindow = true;

	// Token: 0x04001457 RID: 5207
	public static bool VisualizerSpawnEndOnRightClick = true;

	// Token: 0x04001458 RID: 5208
	public static bool VisualizerSpawnAboveTable = true;

	// Token: 0x04001459 RID: 5209
	public const float MaximumLifeAfterRelease = 0.5f;

	// Token: 0x0400145A RID: 5210
	private bool SpawningObjects;

	// Token: 0x0400145B RID: 5211
	[NonSerialized]
	public NetworkPhysicsObject npoToVisualize;

	// Token: 0x0400145C RID: 5212
	private ObjectVisualizer objectVisualizer;

	// Token: 0x0400145D RID: 5213
	private bool visualizerActive;

	// Token: 0x0400145E RID: 5214
	public Bounds SpawnBounds;

	// Token: 0x0400145F RID: 5215
	private float removeAt = -1f;

	// Token: 0x04001460 RID: 5216
	public Shader shader;

	// Token: 0x04001461 RID: 5217
	public Transform ComponentWindow;

	// Token: 0x04001462 RID: 5218
	public Transform GamesWindow;

	// Token: 0x04001463 RID: 5219
	public GameObject CustomObjectContainer;
}
