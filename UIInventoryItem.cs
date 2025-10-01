using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class UIInventoryItem : UIDragDropItem
{
	// Token: 0x060024A9 RID: 9385 RVA: 0x001029B0 File Offset: 0x00100BB0
	public void Process(ObjectState OS, GameObject source)
	{
		this.source = source;
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(OS, true, false);
		this.objectState = OS;
		this.nameLabel.text = OS.Nickname;
		if (!UIInventoryItem.NGUICamera)
		{
			UIInventoryItem.NGUICamera = UICamera.mainCamera;
		}
		gameObject.gameObject.transform.rotation = Quaternion.identity;
		this.sourceInfo = gameObject;
		if (source.GetComponent<DeckScript>())
		{
			this.HoverZoomMulti = Mathf.Min(2f, source.transform.localScale.x);
		}
		this.initialLocalScale = gameObject.transform.localScale;
		this.spawnName = Utilities.RemoveCloneFromName(gameObject.name);
		this.UpdateName();
		this.FitToInventoryItem();
		foreach (Renderer renderer in new List<Renderer>(gameObject.GetComponentsInChildren<Renderer>(true)))
		{
			for (int i = 0; i < renderer.materials.Length; i++)
			{
				Material material = renderer.materials[i];
				if (material.HasProperty("_SpecInt") && material.GetFloat("_SpecInt") < UIInventoryItem.SPECULAR_INTENSITY_TO_KEEP_COLOR)
				{
					material.SetColor("_SpecColor", Color.black);
					renderer.materials[i] = material;
				}
			}
		}
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x00102B18 File Offset: 0x00100D18
	public void UpdateName()
	{
		this.nameLabel.text = this.objectState.Nickname + string.Format("({0})", this.sort.sortOrderIndex + 1);
		NGUIHelper.ClampAndAddDots(this.nameLabel, base.gameObject, string.IsNullOrEmpty(this.objectState.Description));
		if (!string.IsNullOrEmpty(this.objectState.Description))
		{
			base.gameObject.AddMissingComponent<UITooltipScript>().DelayTooltip = this.objectState.Description;
		}
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x00102BAC File Offset: 0x00100DAC
	public void FitToInventoryItem()
	{
		NGUIHelper.FitGameObjectToUI(this.sourceInfo, base.gameObject.transform.parent, this.targetPosition.transform.localPosition, this.initialLocalScale, 85f, null, new Vector3?(this.LookAngle));
		this.finalLocalScale = this.sourceInfo.transform.localScale;
		this.finalLocalPos = this.sourceInfo.transform.localPosition;
		this.rotationCache = this.sourceInfo.transform.rotation;
	}

	// Token: 0x060024AC RID: 9388 RVA: 0x00102C45 File Offset: 0x00100E45
	protected override void OnDragDropStart()
	{
		base.OnDragDropStart();
		this.originSortIndex = this.sourceInventoryItem.sort.sortOrderIndex;
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x00102C64 File Offset: 0x00100E64
	protected override void OnDragDropMove(Vector2 delta)
	{
		base.OnDragDropMove(delta);
		this.scrolling = true;
		if (!string.IsNullOrEmpty(this.searchFilter.value))
		{
			return;
		}
		if (UICamera.HoveredUIObject != null)
		{
			if (UICamera.HoveredUIObject.GetComponent<UIInventoryItem>() != null && UICamera.HoveredUIObject.transform.parent.GetComponent<UIInventorySort>())
			{
				UIInventorySort component = UICamera.HoveredUIObject.transform.parent.GetComponent<UIInventorySort>();
				int sortOrderIndex;
				if (this.sort.gameObject.transform.position.x > component.gameObject.transform.position.x)
				{
					sortOrderIndex = component.sortOrderIndex;
				}
				else
				{
					sortOrderIndex = component.sortOrderIndex;
				}
				if (this.IncreaseSort != null && this.updateSortOrder)
				{
					this.updateSortOrder = false;
					this.IncreaseSort(sortOrderIndex);
					base.StartCoroutine(this.WaitToUpdateOrder());
					return;
				}
			}
			else if (UICamera.HoveredUIObject.CompareTag("InventoryBackground") && this.MoveOverInventoryBackground != null)
			{
				this.MoveOverInventoryBackground();
			}
		}
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x00102D82 File Offset: 0x00100F82
	private IEnumerator WaitToUpdateOrder()
	{
		int num;
		for (int i = 0; i < 1; i = num + 1)
		{
			yield return null;
			num = i;
		}
		this.updateSortOrder = true;
		yield break;
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x00102D94 File Offset: 0x00100F94
	protected override void OnDragDropRelease(GameObject surface)
	{
		this.scrolling = false;
		base.OnDragDropRelease(surface);
		bool flag = false;
		if (!UICamera.HoveredUIObject)
		{
			Vector3 pos = UIInventoryItem.NGUICamera.WorldToScreenPoint(new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z));
			Vector3 pos2 = Vector3.zero;
			if (VRHMD.isVR)
			{
				VRTrackedController lastTouchController = VRTrackedController.lastTouchController;
				pos2 = lastTouchController.WorldPosition();
				lastTouchController.GrabSearchInventoryObject();
			}
			else
			{
				RaycastHit[] array = Physics.RaycastAll(Camera.main.ScreenPointToRay(pos), 1000f);
				Array.Sort<RaycastHit>(array, new RaycastHitComparator());
				foreach (RaycastHit raycastHit in array)
				{
					GameObject gameObject = raycastHit.collider.gameObject;
					if (!gameObject.CompareTag("Pointer") && !gameObject.GetComponent<Zone>())
					{
						pos2 = raycastHit.point;
						break;
					}
				}
				if (PlayerScript.Pointer)
				{
					pos2 = PlayerScript.PointerScript.GetSpawnPosition(pos2, false);
				}
			}
			if (this.source.GetComponent<DeckScript>())
			{
				UIInventory.ObjectWasTaken = true;
				this.source.GetComponent<DeckScript>().RemoveCardRPC(NetworkSingleton<NetworkUI>.Instance.GUIInventory.GetIndexOfInventoryItem(this.sourceInventoryItem), pos2);
			}
			else if (this.source.GetComponent<StackObject>())
			{
				UIInventory.ObjectWasTaken = true;
				this.source.GetComponent<StackObject>().RemoveItemRPC(NetworkSingleton<NetworkUI>.Instance.GUIInventory.GetIndexOfInventoryItem(this.sourceInventoryItem), pos2);
			}
			if (this.RemoveItem != null)
			{
				this.RemoveItem(this.sourceInventoryItem);
			}
		}
		else if (UICamera.HoveredUIObject.transform.parent && UICamera.HoveredUIObject.transform.parent.GetComponent<UIInventorySort>() && string.IsNullOrEmpty(this.searchFilter.value))
		{
			int sortOrderIndex = UICamera.HoveredUIObject.transform.parent.GetComponent<UIInventorySort>().sortOrderIndex;
			this.sourceInventoryItem.sort.gameObject.SetActive(true);
			this.sourceInventoryItem.sort.sortOrderIndex = sortOrderIndex;
			if (this.InsertItem != null)
			{
				this.InsertItem(this.sourceInventoryItem);
				flag = true;
			}
		}
		else if (string.IsNullOrEmpty(this.searchFilter.value))
		{
			this.sourceInventoryItem.sort.gameObject.SetActive(true);
			if (this.InsertItem != null)
			{
				this.InsertItem(this.sourceInventoryItem);
				flag = true;
			}
		}
		if (flag && this.sourceInventoryItem.sourceInfo.CompareTag("Card"))
		{
			Transform parent = UICamera.HoveredUIObject.transform.parent;
			if (parent.name != "Grid")
			{
				parent = parent.parent;
			}
			for (int j = 0; j < parent.childCount; j++)
			{
				Transform child = parent.GetChild(j);
				if (child && child.childCount != 0)
				{
					child = child.GetChild(0);
					if (child)
					{
						UIInventoryItem component = child.GetComponent<UIInventoryItem>();
						if (component && component.sourceInfo == this.sourceInventoryItem.sourceInfo)
						{
							if (this.originSortIndex != component.sort.sortOrderIndex && !UIInventory.PlayersWhoHaveManipulatedDeck.Contains(PlayerScript.Id))
							{
								UIInventory.PlayersWhoHaveManipulatedDeck.Add(PlayerScript.Id);
								NetworkPhysicsObject npo = this.source.GetComponent<DeckScript>().NPO;
								PlayerScript.PointerScript.LogInventoryDrag((npo == null) ? "" : npo.Name);
								return;
							}
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x060024B0 RID: 9392 RVA: 0x00103174 File Offset: 0x00101374
	protected override void Update()
	{
		if (UICamera.HoveredUIObject == null || !this.scrolling)
		{
			return;
		}
		if (UICamera.HoveredUIObject.CompareTag("InventoryTopBG") && this.ScrollInventory != null)
		{
			this.ScrollInventory(true);
			return;
		}
		if (UICamera.HoveredUIObject.CompareTag("InventoryBotBG") && this.ScrollInventory != null)
		{
			this.ScrollInventory(false);
		}
	}

	// Token: 0x040017A3 RID: 6051
	public static float SPECULAR_INTENSITY_TO_KEEP_COLOR = 0.66f;

	// Token: 0x040017A4 RID: 6052
	public GameObject targetPosition;

	// Token: 0x040017A5 RID: 6053
	public ObjectState objectState;

	// Token: 0x040017A6 RID: 6054
	public GameObject source;

	// Token: 0x040017A7 RID: 6055
	public GameObject sourceInfo;

	// Token: 0x040017A8 RID: 6056
	public Vector3 sourceInfoSize;

	// Token: 0x040017A9 RID: 6057
	public Vector3 LookAngle;

	// Token: 0x040017AA RID: 6058
	public string spawnName;

	// Token: 0x040017AB RID: 6059
	public static Camera NGUICamera;

	// Token: 0x040017AC RID: 6060
	public UIInventoryItem.InventoryItemHandler RemoveItem;

	// Token: 0x040017AD RID: 6061
	public UIInventoryItem.IncreaseSortHandler IncreaseSort;

	// Token: 0x040017AE RID: 6062
	public UIInventoryItem.InventoryScroll ScrollInventory;

	// Token: 0x040017AF RID: 6063
	public Action MoveOverInventoryBackground;

	// Token: 0x040017B0 RID: 6064
	public UIInventorySort sort;

	// Token: 0x040017B1 RID: 6065
	public UIInventoryItem sourceInventoryItem;

	// Token: 0x040017B2 RID: 6066
	public UIInventoryItem.InventoryItemHandler InsertItem;

	// Token: 0x040017B3 RID: 6067
	public UIInventoryItem.InventoryItemHandler DropInventoryItem;

	// Token: 0x040017B4 RID: 6068
	public UILabel nameLabel;

	// Token: 0x040017B5 RID: 6069
	public UIInput searchFilter;

	// Token: 0x040017B6 RID: 6070
	private bool scrolling;

	// Token: 0x040017B7 RID: 6071
	public UISprite backgroundSprite;

	// Token: 0x040017B8 RID: 6072
	private Vector3 posOffset;

	// Token: 0x040017B9 RID: 6073
	public float HoverZoomMulti = 1f;

	// Token: 0x040017BA RID: 6074
	public Vector3 initialLocalScale;

	// Token: 0x040017BB RID: 6075
	public Vector3 finalLocalScale;

	// Token: 0x040017BC RID: 6076
	public Vector3 finalLocalPos;

	// Token: 0x040017BD RID: 6077
	private bool updateSortOrder = true;

	// Token: 0x040017BE RID: 6078
	public Quaternion rotationCache = Quaternion.identity;

	// Token: 0x040017BF RID: 6079
	private int originSortIndex;

	// Token: 0x02000765 RID: 1893
	// (Invoke) Token: 0x06003ED5 RID: 16085
	public delegate void InventoryItemHandler(UIInventoryItem item);

	// Token: 0x02000766 RID: 1894
	// (Invoke) Token: 0x06003ED9 RID: 16089
	public delegate void IncreaseSortHandler(int start);

	// Token: 0x02000767 RID: 1895
	// (Invoke) Token: 0x06003EDD RID: 16093
	public delegate void InventoryScroll(bool up);
}
