using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class UIInventory : MonoBehaviour
{
	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x0600248A RID: 9354 RVA: 0x001016CA File Offset: 0x000FF8CA
	// (set) Token: 0x0600248B RID: 9355 RVA: 0x001016D2 File Offset: 0x000FF8D2
	private GameObject source
	{
		get
		{
			return this._source;
		}
		set
		{
			this._source = value;
			this.sourceNPO = ((this._source == null) ? null : this._source.GetComponent<NetworkPhysicsObject>());
		}
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x00101700 File Offset: 0x000FF900
	private void Start()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.closeButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnCloseButtonClick));
		this.itemGrid.onCustomSort = new Comparison<Transform>(this.SortByIndex);
		this.dragRootPanel = this.dragAndDropRoot.GetComponent<UIPanel>();
		EventManager.OnDummyObjectFinish += this.UpdateInventoryItemBounds;
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x00101772 File Offset: 0x000FF972
	private void OnDestroy()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.closeButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnCloseButtonClick));
		EventManager.OnDummyObjectFinish -= this.UpdateInventoryItemBounds;
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x001017B1 File Offset: 0x000FF9B1
	private void OnEnable()
	{
		UIInventory.ObjectWasTaken = false;
		UIInventory.PlayersWhoHaveManipulatedDeck.Clear();
		if (UIInventory.NextSearch != "")
		{
			this.searchFilter.text = UIInventory.NextSearch;
			UIInventory.NextSearch = "";
		}
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x001017F0 File Offset: 0x000FF9F0
	private void OnDisable()
	{
		if (this.source != null)
		{
			this.search.OnSearchDelete -= this.OnDeckDeleted;
			if (Network.peerType != NetworkPeerMode.Disconnected)
			{
				this.search.RemoveSearchId(NetworkID.ID);
			}
		}
		if ((!this.sourceNPO || !this.sourceNPO.IsHandZoneStash) && this.search.playersSearching.Count > 0 && this.search.playersSearching[0].id == NetworkID.ID)
		{
			this.UpdateSource();
		}
		for (int i = 0; i < this.inventoryItems.Count; i++)
		{
			UnityEngine.Object.Destroy(this.inventoryItems[i].transform.parent.gameObject);
		}
		this.inventoryItems.Clear();
		this.sortLookup.Clear();
		UIInventory.PlayersWhoHaveManipulatedDeck.Clear();
		this.source = null;
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x001018EB File Offset: 0x000FFAEB
	public int GetIndexOfInventoryItem(UIInventoryItem item)
	{
		if (this.inventoryType == InventoryTypes.Deck)
		{
			return this.inventoryItems.IndexOf(item);
		}
		return this.inventoryItems.Count - this.inventoryItems.IndexOf(item) - 1;
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x0010191C File Offset: 0x000FFB1C
	private void Update()
	{
		bool flag = false;
		for (int i = 0; i < this.inventoryItems.Count; i++)
		{
			UIInventoryItem uiinventoryItem = this.inventoryItems[i];
			GameObject sourceInfo = uiinventoryItem.sourceInfo;
			if (sourceInfo)
			{
				if (UICamera.HoveredUIObject == uiinventoryItem.gameObject && !zInput.GetButton("Grab", ControlType.All))
				{
					sourceInfo.transform.rotation = uiinventoryItem.rotationCache;
					if (zInput.GetButtonDown("Rotate Right", ControlType.All) && Input.GetAxis("Mouse Wheel") == 0f)
					{
						sourceInfo.transform.Rotate(new Vector3(0f, (float)PlayerScript.PointerScript.RotationSnap, 0f));
					}
					if (zInput.GetButtonDown("Rotate Left", ControlType.All) && Input.GetAxis("Mouse Wheel") == 0f)
					{
						sourceInfo.transform.Rotate(new Vector3(0f, (float)(-(float)PlayerScript.PointerScript.RotationSnap), 0f));
					}
					if (zInput.GetButtonDown("Flip", ControlType.All))
					{
						sourceInfo.transform.Rotate(new Vector3(0f, 0f, 180f));
					}
					uiinventoryItem.rotationCache = sourceInfo.transform.rotation;
					if (zInput.GetButton("Shift", ControlType.All) && zInput.GetButton("Alt", ControlType.All))
					{
						sourceInfo.transform.Rotate(new Vector3(0f, 0f, 180f));
					}
					if (uiinventoryItem.gameObject != this.PrevHoverObject)
					{
						this.LerpSize = 0f;
					}
					flag = true;
					this.PrevHoverObject = uiinventoryItem.gameObject;
					this.LerpSize += Time.deltaTime;
					float num = 1f;
					float num2 = 0.15f;
					float hoverZoomMulti = uiinventoryItem.HoverZoomMulti;
					if (this.LerpSize > num2)
					{
						num = Mathf.Clamp(Mathf.Pow(this.LerpSize - num2, 3f) * 20f * hoverZoomMulti, 1f, 2.25f * hoverZoomMulti);
					}
					if (zInput.GetButton("Alt", ControlType.All))
					{
						num = 2.25f * hoverZoomMulti * Singleton<CameraController>.Instance.AltZoom;
						this.LerpSize = 99f;
					}
					float num3 = 2.25f * num;
					sourceInfo.transform.localScale = uiinventoryItem.finalLocalScale * num3;
					sourceInfo.transform.localPosition = new Vector3(uiinventoryItem.finalLocalPos.x * num3, uiinventoryItem.finalLocalPos.y * num3, -600f);
					Renderer componentInChildren = sourceInfo.GetComponentInChildren<Renderer>();
					if (componentInChildren)
					{
						Vector3 vector = UICamera.mainCamera.WorldToViewportPoint(sourceInfo.transform.position);
						Bounds bounds = componentInChildren.bounds;
						float paddingX = bounds.size.x * 0.25f * (float)Screen.height / (float)Screen.width;
						float paddingY = bounds.size.y * 0.25f;
						vector = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector, paddingX, paddingY, SpriteAlignment.Center, false);
						sourceInfo.transform.position = UICamera.mainCamera.ViewportToWorldPoint(vector);
						sourceInfo.transform.RoundLocalPosition();
					}
				}
				else if (sourceInfo.transform.localScale.x != uiinventoryItem.finalLocalScale.x)
				{
					sourceInfo.transform.localScale = uiinventoryItem.finalLocalScale;
					sourceInfo.transform.localPosition = uiinventoryItem.finalLocalPos;
				}
				bool flag2 = (!this.sourceNPO || !this.sourceNPO.IsHandZoneStash) && this.search.playersSearching.Count > 0 && this.search.playersSearching[0].id == NetworkID.ID;
				if (flag2 != uiinventoryItem.enabled)
				{
					uiinventoryItem.enabled = flag2;
					uiinventoryItem.gameObject.transform.GetChild(0).GetComponent<UISprite>().color = (flag2 ? Colour.White : Colour.Red);
				}
			}
		}
		if (!flag || zInput.GetButtonUp("Alt", ControlType.All))
		{
			this.LerpSize = 0f;
		}
		if (this.search.playersSearching[0].id != NetworkID.ID)
		{
			return;
		}
		if (zInput.GetButton("Grab", ControlType.All))
		{
			List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
			if (grabbableNPOs == null || grabbableNPOs.Count < 1)
			{
				this.dragAndDropRoot.SetActive(false);
				return;
			}
			bool flag3 = false;
			for (int j = 0; j < grabbableNPOs.Count; j++)
			{
				if (grabbableNPOs[j].HeldByPlayerID == NetworkID.ID)
				{
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				return;
			}
			if (UICamera.HoveredUIObject == null)
			{
				if (this.blankItemHoverMover.activeSelf)
				{
					this.blankItemHoverMover.SetActive(false);
					this.GridReposition();
				}
				this.dragAndDropRoot.SetActive(true);
				this.dragRootPanel.alpha = 1f;
				return;
			}
			if (UICamera.HoveredUIObject.GetComponent<UIInventoryItem>() != null)
			{
				UIInventorySort component = UICamera.HoveredUIObject.transform.parent.GetComponent<UIInventorySort>();
				Vector3 vector2 = UICamera.currentCamera.WorldToScreenPoint(component.gameObject.transform.position);
				int startPoint;
				if (Input.mousePosition.x > vector2.x)
				{
					startPoint = component.sortOrderIndex;
				}
				else
				{
					startPoint = component.sortOrderIndex - 1;
				}
				this.IncreaseOrder(startPoint);
				this.dragAndDropRoot.SetActive(false);
				return;
			}
			if (UICamera.HoveredUIObject.CompareTag("InventoryBackground"))
			{
				this.MoveOverInventoryBackground();
				this.dragAndDropRoot.SetActive(false);
				return;
			}
			if (UICamera.HoveredUIObject.CompareTag("InventoryTopBG"))
			{
				this.ScrollInventory(true);
			}
			else if (UICamera.HoveredUIObject.CompareTag("InventoryBotBG"))
			{
				this.ScrollInventory(false);
			}
			else if (!this.dragAndDropRoot.activeSelf)
			{
				this.dragAndDropRoot.SetActive(true);
				this.dragRootPanel.alpha = 1f;
			}
		}
		if (zInput.GetButtonUp("Grab", ControlType.All) && UICamera.HoveredUIObject != null && this.blankItemHoverMover.activeSelf)
		{
			List<NetworkPhysicsObject> grabbableNPOs2 = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
			if (grabbableNPOs2 == null)
			{
				return;
			}
			this.source.GetComponent<LuaGameObjectScript>();
			List<ObjectState> list = new List<ObjectState>();
			List<NetworkPhysicsObject> list2 = new List<NetworkPhysicsObject>();
			for (int k = 0; k < grabbableNPOs2.Count; k++)
			{
				NetworkPhysicsObject networkPhysicsObject = grabbableNPOs2[k];
				if (networkPhysicsObject.HeldByPlayerID == NetworkID.ID && !(networkPhysicsObject == this.sourceNPO))
				{
					if (this.inventoryType == InventoryTypes.Deck && !networkPhysicsObject.CompareTag("Card") && !networkPhysicsObject.CompareTag("Deck"))
					{
						networkPhysicsObject.ReturnToPickupPosition();
					}
					else if (this.inventoryType == InventoryTypes.Deck && networkPhysicsObject.CompareTag("Deck"))
					{
						DeckScript component2 = networkPhysicsObject.GetComponent<DeckScript>();
						if (component2 != null)
						{
							list.Add(null);
							list2.Add(networkPhysicsObject);
							List<ObjectState> cardStates = component2.GetCardStates();
							for (int l = 0; l < cardStates.Count; l++)
							{
								list.Add(cardStates[l]);
								list2.Add(null);
							}
						}
					}
					else
					{
						ObjectState item = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(networkPhysicsObject);
						list.Add(item);
						list2.Add(networkPhysicsObject);
					}
				}
			}
			this.blankItemHoverMover.SetActive(false);
			if (list.Count > 0)
			{
				this.sourceNPO.RPCMaybeInsertDroppedObjects(PlayerScript.PointerScript.ID, list, list2);
			}
			else
			{
				this.GridReposition();
			}
		}
		if (this.dragAndDropRoot.activeSelf)
		{
			this.dragAndDropRoot.SetActive(false);
		}
		this.checkScreenSize();
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x001020D0 File Offset: 0x001002D0
	public void InsertDroppedObjects(List<ObjectState> insertOSs)
	{
		int sortOrderIndex = this.blankItemHoverMover.GetComponent<UIInventorySort>().sortOrderIndex;
		this.AdjustSortOrder(sortOrderIndex, insertOSs.Count);
		for (int i = 0; i < insertOSs.Count; i++)
		{
			this.AddItem(insertOSs[i], sortOrderIndex + i);
		}
		this.GridReposition();
		this.UpdateSource();
		if (this.dragAndDropRoot.activeSelf)
		{
			this.dragAndDropRoot.SetActive(false);
		}
		this.checkScreenSize();
	}

	// Token: 0x06002493 RID: 9363 RVA: 0x00102148 File Offset: 0x00100348
	private void checkScreenSize()
	{
		if (this.lastScreenWidth != (float)Screen.width || this.lastScreenHeight != (float)Screen.height)
		{
			this.lastScreenWidth = (float)Screen.width;
			this.lastScreenHeight = (float)Screen.height;
			this.itemGrid.Reposition();
		}
	}

	// Token: 0x06002494 RID: 9364 RVA: 0x00102194 File Offset: 0x00100394
	public void Init(List<ObjectState> items, GameObject source, InventoryTypes type, int maxCards)
	{
		this.inventoryType = type;
		if (base.gameObject.activeSelf)
		{
			if (this.source != source && this.source)
			{
				this.search.RemoveSearchId(NetworkID.ID);
			}
			this.source = null;
			this.OnDisable();
		}
		this.search = source.GetComponent<PlayersSearchingInventory>();
		base.gameObject.SetActive(true);
		this.sortLookup.Add(this.blankItemHoverMover.transform, this.blankItemHoverMover.GetComponent<UIInventorySort>());
		this.source = source;
		this.search.OnSearchDelete += this.OnDeckDeleted;
		NetworkPhysicsObject npo = this.sourceNPO;
		this.title.text = TTSUtilities.CleanName(npo);
		if (type == InventoryTypes.Deck)
		{
			this.deckItemsCount = ((maxCards >= 0) ? Math.Min(maxCards, items.Count) : items.Count);
			for (int i = 0; i < this.deckItemsCount; i++)
			{
				this.AddItem(items[i], i);
			}
		}
		else
		{
			for (int j = items.Count - 1; j >= 0; j--)
			{
				this.AddItem(items[j], items.Count - j - 1);
			}
		}
		base.Invoke("GridReposition", 0.2f);
		this.scrollbar.value = 0f;
		this.Reset();
		base.Invoke("Reset", 0.3f);
		base.Invoke("Reset", 0.7f);
	}

	// Token: 0x06002495 RID: 9365 RVA: 0x00102310 File Offset: 0x00100510
	public int SortByIndex(Transform a, Transform b)
	{
		UIInventorySort uiinventorySort = this.sortLookup[a];
		UIInventorySort uiinventorySort2 = this.sortLookup[b];
		return uiinventorySort.sortOrderIndex.CompareTo(uiinventorySort2.sortOrderIndex);
	}

	// Token: 0x06002496 RID: 9366 RVA: 0x00102346 File Offset: 0x00100546
	public void OnCloseButtonClick(GameObject go)
	{
		if (this)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002497 RID: 9367 RVA: 0x00102346 File Offset: 0x00100546
	public void OnDeckDeleted()
	{
		if (this)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x0010235C File Offset: 0x0010055C
	public void DeckAddItem(ObjectState item, bool top)
	{
		int index = this.inventoryItems.Count;
		if (top)
		{
			this.IncreaseOrder(0);
			index = 0;
			this.blankItemHoverMover.SetActive(false);
		}
		this.AddItem(item, index);
		this.GridReposition();
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x0010239C File Offset: 0x0010059C
	public void AddItem(ObjectState state, int index)
	{
		GameObject gameObject = this.itemGrid.gameObject.AddChild(this.itemPrefab);
		UIInventoryItem componentInChildren = gameObject.GetComponentInChildren<UIInventoryItem>();
		componentInChildren.sort = gameObject.GetComponent<UIInventorySort>();
		componentInChildren.sort.sortOrderIndex = index;
		componentInChildren.LookAngle = state.AltLookAngle;
		componentInChildren.IncreaseSort = new UIInventoryItem.IncreaseSortHandler(this.IncreaseOrder);
		componentInChildren.RemoveItem = new UIInventoryItem.InventoryItemHandler(this.RemoveInventoryItem);
		componentInChildren.InsertItem = new UIInventoryItem.InventoryItemHandler(this.InsertItem);
		componentInChildren.ScrollInventory = new UIInventoryItem.InventoryScroll(this.ScrollInventory);
		componentInChildren.MoveOverInventoryBackground = new Action(this.MoveOverInventoryBackground);
		componentInChildren.searchFilter = this.searchFilter;
		componentInChildren.Process(state, this.source);
		this.inventoryItems.Add(componentInChildren);
		this.sortLookup.Add(gameObject.transform, componentInChildren.sort);
	}

	// Token: 0x0600249A RID: 9370 RVA: 0x00102484 File Offset: 0x00100684
	public void IncreaseOrder(int startPoint)
	{
		this.blankItemHoverMover.SetActive(true);
		if (this.sortLookup[this.blankItemHoverMover.transform].sortOrderIndex == startPoint)
		{
			return;
		}
		this.sortLookup[this.blankItemHoverMover.transform].sortOrderIndex = startPoint;
		this.AdjustSortOrder(startPoint, 1);
		this.GridReposition();
	}

	// Token: 0x0600249B RID: 9371 RVA: 0x001024E8 File Offset: 0x001006E8
	public void AdjustSortOrder(int startPoint, int increaseAmount)
	{
		for (int i = 0; i < this.inventoryItems.Count; i++)
		{
			if (i > startPoint)
			{
				this.inventoryItems[i].sort.sortOrderIndex = i + increaseAmount;
			}
			else
			{
				this.inventoryItems[i].sort.sortOrderIndex = i - increaseAmount;
			}
		}
	}

	// Token: 0x0600249C RID: 9372 RVA: 0x00102544 File Offset: 0x00100744
	public void InsertItem(UIInventoryItem item)
	{
		this.blankItemHoverMover.SetActive(false);
		this.sortLookup[this.blankItemHoverMover.transform].sortOrderIndex = this.blankItemHoverMover.GetComponent<UIInventorySort>().sortOrderIndex;
		this.GridReposition();
		item.gameObject.SetActive(false);
		item.gameObject.SetActive(true);
		this.UpdateSource();
	}

	// Token: 0x0600249D RID: 9373 RVA: 0x001025AC File Offset: 0x001007AC
	private void GridReposition()
	{
		this.itemGrid.Reposition();
		this.inventoryItems.Sort((UIInventoryItem c1, UIInventoryItem c2) => c1.sort.sortOrderIndex.CompareTo(c2.sort.sortOrderIndex));
	}

	// Token: 0x0600249E RID: 9374 RVA: 0x001025E4 File Offset: 0x001007E4
	private void Reset()
	{
		this.view.UpdateScrollbars();
		BoxCollider2D[] componentsInChildren = this.scrollbar.GetComponentsInChildren<BoxCollider2D>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
	}

	// Token: 0x0600249F RID: 9375 RVA: 0x00102620 File Offset: 0x00100820
	public void DeckShuffle(List<ObjectState> newOrder)
	{
		for (int i = 0; i < newOrder.Count; i++)
		{
			for (int j = 0; j < this.inventoryItems.Count; j++)
			{
				if (newOrder[i] == this.inventoryItems[j].objectState)
				{
					this.inventoryItems[j].sort.sortOrderIndex = i;
					break;
				}
			}
		}
		this.GridReposition();
	}

	// Token: 0x060024A0 RID: 9376 RVA: 0x00102698 File Offset: 0x00100898
	public void RemoveDeckItem(ObjectState item, bool top)
	{
		UIInventoryItem uiinventoryItem = null;
		for (int i = 0; i < this.inventoryItems.Count; i++)
		{
			if (this.inventoryItems[i].objectState == item)
			{
				uiinventoryItem = this.inventoryItems[i];
				break;
			}
		}
		if (uiinventoryItem != null)
		{
			this.RemoveItem(uiinventoryItem);
		}
		this.GridReposition();
	}

	// Token: 0x060024A1 RID: 9377 RVA: 0x001026FC File Offset: 0x001008FC
	public void RemoveItem(UIInventoryItem item)
	{
		this.sortLookup.Remove(item.transform.parent.transform);
		this.itemGrid.RemoveChild(item.transform.parent);
		this.inventoryItems.Remove(item);
		UnityEngine.Object.Destroy(item.sourceInfo);
		UnityEngine.Object.Destroy(item);
		UnityEngine.Object.Destroy(item.transform.parent.gameObject);
		this.GridReposition();
		base.Invoke("GridReposition", 0.2f);
		base.Invoke("GridReposition", 0.5f);
		if (UIInventory.CLOSE_SEARCH_AFTER_TAKE && UIInventory.ObjectWasTaken)
		{
			NetworkSingleton<NetworkUI>.Instance.EscapeMenu(NetworkUI.EscapeMenuActivation.NEVER);
		}
	}

	// Token: 0x060024A2 RID: 9378 RVA: 0x001027AE File Offset: 0x001009AE
	public void RemoveInventoryItem(UIInventoryItem item)
	{
		this.blankItemHoverMover.SetActive(false);
		this.RemoveItem(item);
		this.LastRemoved = item.sourceInfo;
		if (this.inventoryType == InventoryTypes.Deck)
		{
			this.deckItemsCount = this.inventoryItems.Count;
		}
		this.UpdateSource();
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x001027EE File Offset: 0x001009EE
	public void MoveOverInventoryBackground()
	{
		this.blankItemHoverMover.SetActive(true);
		this.blankItemHoverMover.GetComponent<UIInventorySort>().sortOrderIndex = this.inventoryItems.Count + 1;
		this.GridReposition();
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x00102820 File Offset: 0x00100A20
	public void UpdateSource()
	{
		if (!this.source)
		{
			return;
		}
		List<ObjectState> list = new List<ObjectState>();
		for (int i = 0; i < this.inventoryItems.Count; i++)
		{
			list.Add(this.inventoryItems[i].objectState);
			this.inventoryItems[i].sort.sortOrderIndex = i;
			this.inventoryItems[i].UpdateName();
		}
		if (this.inventoryType == InventoryTypes.Deck)
		{
			this.source.GetComponent<DeckScript>().SetSearch(list, this.deckItemsCount);
			this.deckItemsCount = this.inventoryItems.Count;
			return;
		}
		list.Reverse();
		this.source.GetComponent<StackObject>().SetSearch(list, -1);
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x001028E0 File Offset: 0x00100AE0
	public void UpdateInventoryItemBounds(GameObject GO)
	{
		for (int i = 0; i < this.inventoryItems.Count; i++)
		{
			if (GO == this.inventoryItems[i].sourceInfo)
			{
				this.inventoryItems[i].FitToInventoryItem();
				return;
			}
		}
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x00102930 File Offset: 0x00100B30
	public void ScrollInventory(bool up)
	{
		float num = 0.5f;
		if (up)
		{
			this.scrollbar.value -= num * Time.deltaTime;
			return;
		}
		this.scrollbar.value += num * Time.deltaTime;
	}

	// Token: 0x04001786 RID: 6022
	public static bool CLOSE_SEARCH_AFTER_TAKE = false;

	// Token: 0x04001787 RID: 6023
	public static bool ObjectWasTaken = false;

	// Token: 0x04001788 RID: 6024
	public static List<int> PlayersWhoHaveManipulatedDeck = new List<int>();

	// Token: 0x04001789 RID: 6025
	public static string NextSearch;

	// Token: 0x0400178A RID: 6026
	public GameObject itemPrefab;

	// Token: 0x0400178B RID: 6027
	public UIPopupList category;

	// Token: 0x0400178C RID: 6028
	public UIGrid itemGrid;

	// Token: 0x0400178D RID: 6029
	public UILabel title;

	// Token: 0x0400178E RID: 6030
	public UIPanel[] topPanels;

	// Token: 0x0400178F RID: 6031
	public GameObject closeButton;

	// Token: 0x04001790 RID: 6032
	public GameObject dragAndDropRoot;

	// Token: 0x04001791 RID: 6033
	public GameObject dragAndDropIcon;

	// Token: 0x04001792 RID: 6034
	public GameObject blankItemHoverMover;

	// Token: 0x04001793 RID: 6035
	public UIScrollBar scrollbar;

	// Token: 0x04001794 RID: 6036
	public UIScrollView view;

	// Token: 0x04001795 RID: 6037
	public UIInput searchFilter;

	// Token: 0x04001796 RID: 6038
	private GameObject _source;

	// Token: 0x04001797 RID: 6039
	private NetworkPhysicsObject sourceNPO;

	// Token: 0x04001798 RID: 6040
	private List<UIInventoryItem> inventoryItems = new List<UIInventoryItem>();

	// Token: 0x04001799 RID: 6041
	private int deckItemsCount;

	// Token: 0x0400179A RID: 6042
	private InventoryTypes inventoryType;

	// Token: 0x0400179B RID: 6043
	private float lastScreenWidth;

	// Token: 0x0400179C RID: 6044
	private float lastScreenHeight;

	// Token: 0x0400179D RID: 6045
	private Dictionary<Transform, UIInventorySort> sortLookup = new Dictionary<Transform, UIInventorySort>();

	// Token: 0x0400179E RID: 6046
	private PlayersSearchingInventory search;

	// Token: 0x0400179F RID: 6047
	private UIPanel dragRootPanel;

	// Token: 0x040017A0 RID: 6048
	private GameObject PrevHoverObject;

	// Token: 0x040017A1 RID: 6049
	private float LerpSize;

	// Token: 0x040017A2 RID: 6050
	public GameObject LastRemoved;
}
