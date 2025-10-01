using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RTEditor
{
	// Token: 0x020003D0 RID: 976
	[Serializable]
	public class EditorObjectSelection : MonoSingletonBase<EditorObjectSelection>
	{
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06002DB2 RID: 11698 RVA: 0x0013C7E0 File Offset: 0x0013A9E0
		// (remove) Token: 0x06002DB3 RID: 11699 RVA: 0x0013C818 File Offset: 0x0013AA18
		public event EditorObjectSelection.SelectionChangedHandler SelectionChanged;

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x0013C84D File Offset: 0x0013AA4D
		public int NumberOfSelectedObjects
		{
			get
			{
				return this._selectedObjects.Count;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x0013C85A File Offset: 0x0013AA5A
		public GameObject LastSelectedGameObject
		{
			get
			{
				return this._lastSelectedGameObject;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x0013C862 File Offset: 0x0013AA62
		public HashSet<GameObject> SelectedGameObjects
		{
			get
			{
				return new HashSet<GameObject>(this._selectedObjects);
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x0013C86F File Offset: 0x0013AA6F
		public HashSet<GameObject> MaskedObjects
		{
			get
			{
				return new HashSet<GameObject>(this._maskedObjects);
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x0013C87C File Offset: 0x0013AA7C
		public ShortcutKeys AppendToSelectionShortcut
		{
			get
			{
				return this._appendToSelectionShortcut;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x0013C884 File Offset: 0x0013AA84
		public ShortcutKeys MultiDeselectShortcut
		{
			get
			{
				return this._multiDeselectShortcut;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06002DBA RID: 11706 RVA: 0x0013C88C File Offset: 0x0013AA8C
		public ShortcutKeys DuplicateSelectionShortcut
		{
			get
			{
				return this._duplicateSelectionShortcut;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x0013C894 File Offset: 0x0013AA94
		public ObjectSelectionSettings ObjectSelectionSettings
		{
			get
			{
				return this._objectSelectionSettings;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002DBC RID: 11708 RVA: 0x0013C89C File Offset: 0x0013AA9C
		public ObjectSelectionRectangleRenderSettings ObjectSelectionRectangleRenderSettings
		{
			get
			{
				return this._objectSelectionRectangle.RenderSettings;
			}
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x0013C8AC File Offset: 0x0013AAAC
		public bool AddObjectToSelection(GameObject gameObj, bool allowUndoRedo)
		{
			if (gameObj == null)
			{
				return false;
			}
			if (this.IsSelectionExactMatch(new List<GameObject>
			{
				gameObj
			}))
			{
				return false;
			}
			if (allowUndoRedo)
			{
				ObjectSelectionSnapshot objectSelectionSnapshot = new ObjectSelectionSnapshot();
				objectSelectionSnapshot.TakeSnapshot();
				if (this.AddObjectToSelection(gameObj, ObjectSelectActionType.AddObjectToSelectionCall))
				{
					ObjectSelectionSnapshot objectSelectionSnapshot2 = new ObjectSelectionSnapshot();
					objectSelectionSnapshot2.TakeSnapshot();
					new PostObjectSelectionChangedAction(objectSelectionSnapshot, objectSelectionSnapshot2).Execute();
					ObjectSelectionChangedEventArgs selectionChangedEventArgs = new ObjectSelectionChangedEventArgs(ObjectSelectActionType.AddObjectToSelectionCall, new List<GameObject>
					{
						gameObj
					}, ObjectDeselectActionType.None, new List<GameObject>());
					this.OnSelectionChanged(selectionChangedEventArgs);
					return true;
				}
			}
			else if (this.AddObjectToSelection(gameObj, ObjectSelectActionType.AddObjectToSelectionCall))
			{
				ObjectSelectionChangedEventArgs selectionChangedEventArgs2 = new ObjectSelectionChangedEventArgs(ObjectSelectActionType.AddObjectToSelectionCall, new List<GameObject>
				{
					gameObj
				}, ObjectDeselectActionType.None, new List<GameObject>());
				this.OnSelectionChanged(selectionChangedEventArgs2);
				return true;
			}
			return false;
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x0013C95C File Offset: 0x0013AB5C
		public bool RemoveObjectFromSelection(GameObject gameObj, bool allowUndoRedo)
		{
			if (gameObj == null)
			{
				return false;
			}
			if (!this.IsObjectSelected(gameObj))
			{
				return false;
			}
			if (allowUndoRedo)
			{
				ObjectSelectionSnapshot objectSelectionSnapshot = new ObjectSelectionSnapshot();
				objectSelectionSnapshot.TakeSnapshot();
				if (this.RemoveObjectFromSelection(gameObj, ObjectDeselectActionType.RemoveObjectFromSelectionCall))
				{
					ObjectSelectionSnapshot objectSelectionSnapshot2 = new ObjectSelectionSnapshot();
					objectSelectionSnapshot2.TakeSnapshot();
					new PostObjectSelectionChangedAction(objectSelectionSnapshot, objectSelectionSnapshot2).Execute();
					ObjectSelectionChangedEventArgs selectionChangedEventArgs = new ObjectSelectionChangedEventArgs(ObjectSelectActionType.None, new List<GameObject>(), ObjectDeselectActionType.RemoveObjectFromSelectionCall, new List<GameObject>
					{
						gameObj
					});
					this.OnSelectionChanged(selectionChangedEventArgs);
					return true;
				}
			}
			else if (this.RemoveObjectFromSelection(gameObj, ObjectDeselectActionType.RemoveObjectFromSelectionCall))
			{
				ObjectSelectionChangedEventArgs selectionChangedEventArgs2 = new ObjectSelectionChangedEventArgs(ObjectSelectActionType.None, new List<GameObject>(), ObjectDeselectActionType.RemoveObjectFromSelectionCall, new List<GameObject>
				{
					gameObj
				});
				this.OnSelectionChanged(selectionChangedEventArgs2);
				return true;
			}
			return false;
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x0013CA00 File Offset: 0x0013AC00
		public void SetSelectedObjects(List<GameObject> selectedObjects, bool allowUndoRedo)
		{
			if (this.IsSelectionExactMatch(selectedObjects))
			{
				return;
			}
			List<GameObject> deselectedObjects = new List<GameObject>(this.SelectedGameObjects);
			if (selectedObjects == null || selectedObjects.Count == 0)
			{
				this.ClearSelection(allowUndoRedo, ObjectDeselectActionType.SetSelectedObjectsCall);
			}
			else if (allowUndoRedo)
			{
				ObjectSelectionSnapshot objectSelectionSnapshot = new ObjectSelectionSnapshot();
				objectSelectionSnapshot.TakeSnapshot();
				this.ClearSelection(false, ObjectDeselectActionType.SetSelectedObjectsCall);
				this.AddObjectCollectionToSelection(selectedObjects, ObjectSelectActionType.SetSelectedObjectsCall);
				ObjectSelectionSnapshot objectSelectionSnapshot2 = new ObjectSelectionSnapshot();
				objectSelectionSnapshot2.TakeSnapshot();
				new PostObjectSelectionChangedAction(objectSelectionSnapshot, objectSelectionSnapshot2).Execute();
			}
			else
			{
				this.AddObjectCollectionToSelection(selectedObjects, ObjectSelectActionType.SetSelectedObjectsCall);
			}
			ObjectSelectionChangedEventArgs selectionChangedEventArgs = new ObjectSelectionChangedEventArgs(ObjectSelectActionType.SetSelectedObjectsCall, new List<GameObject>(this._selectedObjects), ObjectDeselectActionType.SetSelectedObjectsCall, deselectedObjects);
			this.OnSelectionChanged(selectionChangedEventArgs);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x0013CA96 File Offset: 0x0013AC96
		public bool ClearSelection(bool allowUndoRedo)
		{
			return this.ClearSelection(allowUndoRedo, ObjectDeselectActionType.ClearSelectionCall);
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x0013CAA0 File Offset: 0x0013ACA0
		public void AddGameObjectCollectionToSelectionMask(List<GameObject> gameObjects)
		{
			foreach (GameObject item in gameObjects)
			{
				this._maskedObjects.Add(item);
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x0013CAF4 File Offset: 0x0013ACF4
		public void RemoveGameObjectCollectionFromSelectionMask(List<GameObject> gameObjects)
		{
			foreach (GameObject item in gameObjects)
			{
				this._maskedObjects.Remove(item);
			}
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x0013CB48 File Offset: 0x0013AD48
		public bool IsSelectionExactMatch(List<GameObject> gameObjectsToMatch)
		{
			if (gameObjectsToMatch == null)
			{
				return this.NumberOfSelectedObjects == 0;
			}
			if (this.NumberOfSelectedObjects != gameObjectsToMatch.Count)
			{
				return false;
			}
			foreach (GameObject gameObj in gameObjectsToMatch)
			{
				if (!this.IsObjectSelected(gameObj))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x0013CBC0 File Offset: 0x0013ADC0
		public void UndoRedoSelection(ObjectSelectionSnapshot objectSelectionSnapshot, UndoRedoActionType undoRedoActionType)
		{
			List<GameObject> list = new List<GameObject>(this.SelectedGameObjects);
			this._selectedObjects = new HashSet<GameObject>(objectSelectionSnapshot.SelectedGameObjects);
			this._lastSelectedGameObject = objectSelectionSnapshot.LastSelectedGameObject;
			ObjectDeselectActionType deselectActionType = (undoRedoActionType == UndoRedoActionType.Undo) ? ObjectDeselectActionType.Undo : ObjectDeselectActionType.Redo;
			ObjectDeselectEventArgs deselectEventArgs = new ObjectDeselectEventArgs(deselectActionType);
			foreach (GameObject gameObject in list)
			{
				IRTEditorEventListener component = gameObject.GetComponent<IRTEditorEventListener>();
				if (component != null)
				{
					component.OnDeselected(deselectEventArgs);
				}
			}
			ObjectSelectActionType selectActionType = (undoRedoActionType == UndoRedoActionType.Undo) ? ObjectSelectActionType.Undo : ObjectSelectActionType.Redo;
			ObjectSelectEventArgs selectEventArgs = new ObjectSelectEventArgs(selectActionType);
			foreach (GameObject gameObject2 in this._selectedObjects)
			{
				IRTEditorEventListener component2 = gameObject2.GetComponent<IRTEditorEventListener>();
				if (component2 != null)
				{
					component2.OnSelected(selectEventArgs);
				}
			}
			EditorGizmoSystem instance = MonoSingletonBase<EditorGizmoSystem>.Instance;
			this.ConnectObjectSelectionToGizmo(instance.TranslationGizmo);
			this.ConnectObjectSelectionToGizmo(instance.RotationGizmo);
			this.ConnectObjectSelectionToGizmo(instance.ScaleGizmo);
			this.ConnectObjectSelectionToGizmo(instance.VolumeScaleGizmo);
			ObjectSelectionChangedEventArgs selectionChangedEventArgs = new ObjectSelectionChangedEventArgs(selectActionType, new List<GameObject>(this._selectedObjects), deselectActionType, list);
			this.OnSelectionChanged(selectionChangedEventArgs);
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x0013CD0C File Offset: 0x0013AF0C
		public bool IsObjectSelected(GameObject gameObj)
		{
			return this._selectedObjects.Contains(gameObj);
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x0013CD1A File Offset: 0x0013AF1A
		public void ConnectObjectSelectionToGizmo(Gizmo gizmo)
		{
			gizmo.ControlledObjects = this._selectedObjects;
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x0013CD28 File Offset: 0x0013AF28
		public Box GetWorldBox()
		{
			if (this.NumberOfSelectedObjects == 0)
			{
				return Box.GetInvalid();
			}
			Box result = Box.GetInvalid();
			foreach (GameObject gameObject in this._selectedObjects)
			{
				Box worldBox = gameObject.GetWorldBox();
				if (worldBox.IsValid())
				{
					if (result.IsValid())
					{
						result.Encapsulate(worldBox);
					}
					else
					{
						result = worldBox;
					}
				}
			}
			return result;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x0013CDAC File Offset: 0x0013AFAC
		public Vector3 GetSelectionWorldCenter()
		{
			return this.CalculateSelectionWorldCenter();
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x0013CDB4 File Offset: 0x0013AFB4
		private bool CanOperate()
		{
			return base.gameObject.activeSelf && base.enabled && !MonoSingletonBase<EditorGizmoSystem>.Instance.IsActiveGizmoReadyForObjectManipulation() && !MonoSingletonBase<SceneGizmo>.Instance.IsHovered();
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x0013CDE6 File Offset: 0x0013AFE6
		private bool CanPerformMultiSelect()
		{
			return this.ObjectSelectionSettings.CanMultiSelect && (this.CanOperate() & MonoSingletonBase<InputDevice>.Instance.IsPressed(0)) && this._objectSelectionRectangle.IsVisible;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x0013CE16 File Offset: 0x0013B016
		private bool IsObjectMasked(GameObject gameObj)
		{
			return this._maskedObjects.Contains(gameObj) || !LayerHelper.IsLayerBitSet(this._objectSelectionSettings.SelectableLayers, gameObj.layer);
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x0013CE44 File Offset: 0x0013B044
		private bool CanSelectObject(GameObject gameObj, ObjectSelectActionType selectActionType)
		{
			if (gameObj == null || !gameObj.activeSelf || !this.CanOperate())
			{
				return false;
			}
			if (gameObj.IsRTEditorSystemObject())
			{
				return false;
			}
			bool flag = gameObj.HasMesh();
			if (!this._objectSelectionSettings.CanSelectTerrainObjects && gameObj.HasTerrain())
			{
				return false;
			}
			if (!flag && !this._objectSelectionSettings.CanSelectLightObjects && gameObj.HasLight())
			{
				return false;
			}
			if (!flag && !this._objectSelectionSettings.CanSelectParticleSystemObjects && gameObj.HasParticleSystem())
			{
				return false;
			}
			if (!this._objectSelectionSettings.CanSelectEmptyObjects && gameObj.IsEmpty())
			{
				return false;
			}
			if (!this._objectSelectionSettings.CanSelectSpriteObjects && gameObj.IsSprite())
			{
				return false;
			}
			if (this.IsObjectSelected(gameObj) || this.IsObjectMasked(gameObj))
			{
				return false;
			}
			IRTEditorEventListener component = gameObj.GetComponent<IRTEditorEventListener>();
			if (component != null)
			{
				ObjectSelectEventArgs selectEventArgs = new ObjectSelectEventArgs(selectActionType);
				return component.OnCanBeSelected(selectEventArgs);
			}
			return true;
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x0013CF24 File Offset: 0x0013B124
		private void Update()
		{
			if (!PlayerScript.Pointer || !Pointer.IsGizmoTool(PlayerScript.PointerScript.CurrentPointerMode))
			{
				return;
			}
			this.RemoveNullAndInactiveObjectRefs();
			if (this._duplicateSelectionShortcut.IsActiveInCurrentFrame())
			{
				new ObjectDuplicationAction(new List<GameObject>(MonoSingletonBase<EditorObjectSelection>.Instance.SelectedGameObjects)).Execute();
			}
			if (!this.WereAnyUIElementsHovered())
			{
				if (MonoSingletonBase<InputDevice>.Instance.WasPressedInCurrentFrame(0))
				{
					this.OnInputDeviceFirstButtonDown();
				}
				if (MonoSingletonBase<InputDevice>.Instance.WasMoved())
				{
					this.OnInputDeviceMoved();
				}
			}
			if (MonoSingletonBase<InputDevice>.Instance.WasReleasedInCurrentFrame(0))
			{
				this.OnInputDeviceFirstButtonUp();
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x0013CFBC File Offset: 0x0013B1BC
		private void RemoveNullAndInactiveObjectRefs()
		{
			List<GameObject> list = new List<GameObject>(this._selectedObjects).FindAll((GameObject item) => item != null && !item.activeSelf);
			this._selectedObjects.RemoveWhere((GameObject item) => item == null || !item.activeSelf);
			this._maskedObjects.RemoveWhere((GameObject item) => item == null);
			if (list.Count != 0)
			{
				ObjectSelectionChangedEventArgs selectionChangedEventArgs = new ObjectSelectionChangedEventArgs(ObjectSelectActionType.None, new List<GameObject>(), ObjectDeselectActionType.DeselectInactive, list);
				this.OnSelectionChanged(selectionChangedEventArgs);
			}
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x0013D06C File Offset: 0x0013B26C
		private void OnInputDeviceFirstButtonDown()
		{
			this._multiSelectPreChangeSnapshot = new ObjectSelectionSnapshot();
			this._multiSelectPreChangeSnapshot.TakeSnapshot();
			if (this.CanOperate() && this.ObjectSelectionSettings.CanClickSelect)
			{
				GameObject gameObject = this.PickObjectInScene();
				if (gameObject != null)
				{
					ObjectSelectionSnapshot objectSelectionSnapshot = new ObjectSelectionSnapshot();
					objectSelectionSnapshot.TakeSnapshot();
					if (this.HandleObjectClick(gameObject))
					{
						ObjectSelectionSnapshot objectSelectionSnapshot2 = new ObjectSelectionSnapshot();
						objectSelectionSnapshot2.TakeSnapshot();
						new PostObjectSelectionChangedAction(objectSelectionSnapshot, objectSelectionSnapshot2).Execute();
						ObjectSelectionChangedEventArgs selectionChangedEventArgs = ObjectSelectionChangedEventArgs.FromSnapshots(this._lastSelectActionType, this._lastDeselectActionType, objectSelectionSnapshot, objectSelectionSnapshot2);
						this.OnSelectionChanged(selectionChangedEventArgs);
					}
				}
				else if (!this._multiDeselectShortcut.IsActive())
				{
					this.ClearSelection(true, ObjectDeselectActionType.ClearClickAir);
				}
			}
			Vector2 vector;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out vector))
			{
				return;
			}
			ObjectSelectionShape objectSelectionShape = this.GetObjectSelectionShape();
			objectSelectionShape.SetEnclosingRectBottomRightPoint(vector);
			objectSelectionShape.SetEnclosingRectTopLeftPoint(vector);
			if (this.CanOperate())
			{
				objectSelectionShape.IsVisible = true;
				return;
			}
			objectSelectionShape.IsVisible = false;
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x0013D158 File Offset: 0x0013B358
		private bool ClearSelection(bool allowUndoRedo, ObjectDeselectActionType deselectActionType)
		{
			if (this.NumberOfSelectedObjects != 0)
			{
				List<GameObject> list = new List<GameObject>(this.SelectedGameObjects);
				if (allowUndoRedo)
				{
					ObjectSelectionSnapshot objectSelectionSnapshot = new ObjectSelectionSnapshot();
					objectSelectionSnapshot.TakeSnapshot();
					this._selectedObjects.Clear();
					this._lastSelectedGameObject = null;
					ObjectSelectionSnapshot objectSelectionSnapshot2 = new ObjectSelectionSnapshot();
					objectSelectionSnapshot2.TakeSnapshot();
					new PostObjectSelectionChangedAction(objectSelectionSnapshot, objectSelectionSnapshot2).Execute();
				}
				else
				{
					this._selectedObjects.Clear();
					this._lastSelectedGameObject = null;
				}
				ObjectDeselectEventArgs deselectEventArgs = new ObjectDeselectEventArgs(deselectActionType);
				foreach (GameObject gameObject in list)
				{
					if (!(gameObject == null))
					{
						IRTEditorEventListener component = gameObject.GetComponent<IRTEditorEventListener>();
						if (component != null)
						{
							component.OnDeselected(deselectEventArgs);
						}
					}
				}
				ObjectSelectionChangedEventArgs selectionChangedEventArgs = new ObjectSelectionChangedEventArgs(ObjectSelectActionType.None, null, deselectActionType, list);
				this.OnSelectionChanged(selectionChangedEventArgs);
				return true;
			}
			return false;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x0013D240 File Offset: 0x0013B440
		private void OnInputDeviceFirstButtonUp()
		{
			this.GetObjectSelectionShape().IsVisible = false;
			if (this._selectionChangedWithShape)
			{
				this._multiSelectPostChangeSnapshot = new ObjectSelectionSnapshot();
				this._multiSelectPostChangeSnapshot.TakeSnapshot();
				new PostObjectSelectionChangedAction(this._multiSelectPreChangeSnapshot, this._multiSelectPostChangeSnapshot).Execute();
				this._selectionChangedWithShape = false;
				ObjectSelectionChangedEventArgs selectionChangedEventArgs = ObjectSelectionChangedEventArgs.FromSnapshots(this._lastSelectActionType, this._lastDeselectActionType, this._multiSelectPreChangeSnapshot, this._multiSelectPostChangeSnapshot);
				this.OnSelectionChanged(selectionChangedEventArgs);
			}
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x0013D2BC File Offset: 0x0013B4BC
		private void OnInputDeviceMoved()
		{
			if (this.CanPerformMultiSelect())
			{
				Vector2 enclosingRectTopLeftPoint;
				if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out enclosingRectTopLeftPoint))
				{
					return;
				}
				this.GetObjectSelectionShape().SetEnclosingRectTopLeftPoint(enclosingRectTopLeftPoint);
				List<GameObject> visibleGameObjects = MonoSingletonBase<EditorCamera>.Instance.GetVisibleGameObjects();
				List<GameObject> intersectingGameObjects = this.GetObjectSelectionShape().GetIntersectingGameObjects(visibleGameObjects, MonoSingletonBase<EditorCamera>.Instance.Camera);
				if (this.HandleObjectsEnteredSelectionShape(intersectingGameObjects))
				{
					this._selectionChangedWithShape = true;
				}
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x0013D31E File Offset: 0x0013B51E
		private bool HandleObjectsEnteredSelectionShape(List<GameObject> objectCollection)
		{
			if (this._multiDeselectShortcut.IsActive())
			{
				return this.RemoveObjectCollectionFromSelection(objectCollection, ObjectDeselectActionType.MultiDeselect);
			}
			if (this._appendToSelectionShortcut.IsActive())
			{
				return this.AddObjectCollectionToSelection(objectCollection, ObjectSelectActionType.MultiSelect);
			}
			return this.ClearAndAddObjectCollectionToSelection(objectCollection, ObjectSelectActionType.MultiSelect);
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x0013D354 File Offset: 0x0013B554
		private GameObject PickObjectInScene()
		{
			GameObject result = null;
			if (!this.ObjectSelectionSettings.CanSelectTerrainObjects)
			{
				SingletonBase<MouseCursor>.Instance.PushObjectPickMaskFlags(MouseCursorObjectPickFlags.ObjectTerrain);
			}
			MouseCursorRayHit rayHit = SingletonBase<MouseCursor>.Instance.GetRayHit(this.ObjectSelectionSettings.SelectableLayers);
			if (rayHit.WasAnObjectHit)
			{
				List<GameObjectRayHit> sortedObjectRayHits = rayHit.SortedObjectRayHits;
				if (!this.ObjectSelectionSettings.CanSelectLightObjects)
				{
					sortedObjectRayHits.RemoveAll((GameObjectRayHit item) => !item.HitObject.HasMesh() && item.HitObject.HasLight());
				}
				if (!this.ObjectSelectionSettings.CanSelectParticleSystemObjects)
				{
					sortedObjectRayHits.RemoveAll((GameObjectRayHit item) => !item.HitObject.HasMesh() && item.HitObject.HasParticleSystem());
				}
				if (!this.ObjectSelectionSettings.CanSelectSpriteObjects)
				{
					sortedObjectRayHits.RemoveAll((GameObjectRayHit item) => item.HitObject.IsSprite());
				}
				if (!this.ObjectSelectionSettings.CanSelectEmptyObjects)
				{
					sortedObjectRayHits.RemoveAll((GameObjectRayHit item) => item.HitObject.IsEmpty());
				}
				if (sortedObjectRayHits.Count != 0)
				{
					result = sortedObjectRayHits[0].HitObject;
				}
			}
			return result;
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x0013D488 File Offset: 0x0013B688
		private ObjectSelectionShape GetObjectSelectionShape()
		{
			return this._objectSelectionRectangle;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0013D490 File Offset: 0x0013B690
		private Vector3 CalculateSelectionWorldCenter()
		{
			if (this.NumberOfSelectedObjects == 0)
			{
				return Vector3.zero;
			}
			int num = 0;
			Vector3 a = Vector3.zero;
			foreach (GameObject gameObject in this._selectedObjects)
			{
				Box worldBox = gameObject.GetWorldBox();
				if (worldBox.IsValid())
				{
					a += worldBox.Center;
					num++;
				}
			}
			if (num == 0)
			{
				return Vector3.zero;
			}
			return a / (float)num;
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0013D524 File Offset: 0x0013B724
		private GameObject RetrieveAGameObjectFromObjectSelectionCollection()
		{
			using (HashSet<GameObject>.Enumerator enumerator = this._selectedObjects.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0013D578 File Offset: 0x0013B778
		private void OnRenderObject()
		{
			if (!PlayerScript.Pointer || !Pointer.IsGizmoTool(PlayerScript.PointerScript.CurrentPointerMode))
			{
				return;
			}
			if (Camera.current != MonoSingletonBase<EditorCamera>.Instance.Camera)
			{
				return;
			}
			if (this.ObjectSelectionSettings.ObjectSelectionBoxRenderSettings.DrawBoxes)
			{
				ObjectSelectionRendererFactory.Create(this._objectSelectionSettings.ObjectSelectionRenderMode).RenderObjectSelection(this._selectedObjects, this._objectSelectionSettings);
			}
			if (this.ObjectSelectionSettings.CanMultiSelect)
			{
				this.GetObjectSelectionShape().Render();
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0013D608 File Offset: 0x0013B808
		private bool WereAnyUIElementsHovered()
		{
			if (EventSystem.current == null)
			{
				return false;
			}
			Vector2 vector;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out vector))
			{
				return false;
			}
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = new Vector2(vector.x, vector.y);
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			return list.Count != 0;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x0013D671 File Offset: 0x0013B871
		private bool HandleObjectClick(GameObject clickedObject)
		{
			if (!this._appendToSelectionShortcut.IsActive())
			{
				return this.ClearAndAddObjectToSelection(clickedObject, ObjectSelectActionType.Click);
			}
			if (this.IsObjectSelected(clickedObject))
			{
				return this.RemoveObjectFromSelection(clickedObject, ObjectDeselectActionType.ClickAlreadySelected);
			}
			return this.AddObjectToSelection(clickedObject, ObjectSelectActionType.ClickAppend);
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x0013D6A4 File Offset: 0x0013B8A4
		private bool AddObjectToSelection(GameObject gameObj, ObjectSelectActionType selectActionType)
		{
			if (this.CanSelectObject(gameObj, selectActionType))
			{
				this._selectedObjects.Add(gameObj);
				this._lastSelectedGameObject = gameObj;
				this._lastSelectActionType = selectActionType;
				this._lastDeselectActionType = ObjectDeselectActionType.None;
				IRTEditorEventListener component = gameObj.GetComponent<IRTEditorEventListener>();
				if (component != null)
				{
					ObjectSelectEventArgs selectEventArgs = new ObjectSelectEventArgs(selectActionType);
					component.OnSelected(selectEventArgs);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0013D6FC File Offset: 0x0013B8FC
		private bool AddObjectCollectionToSelection(List<GameObject> objectCollection, ObjectSelectActionType selectActionType)
		{
			bool result = false;
			foreach (GameObject gameObj in objectCollection)
			{
				result = this.AddObjectToSelection(gameObj, selectActionType);
			}
			return result;
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x0013D750 File Offset: 0x0013B950
		private bool RemoveObjectFromSelection(GameObject gameObj, ObjectDeselectActionType deselectActionType)
		{
			if (this.IsObjectSelected(gameObj))
			{
				this._selectedObjects.Remove(gameObj);
				this._lastSelectedGameObject = this.RetrieveAGameObjectFromObjectSelectionCollection();
				this._lastDeselectActionType = deselectActionType;
				this._lastSelectActionType = ObjectSelectActionType.None;
				IRTEditorEventListener component = gameObj.GetComponent<IRTEditorEventListener>();
				if (component != null)
				{
					ObjectDeselectEventArgs deselectEventArgs = new ObjectDeselectEventArgs(deselectActionType);
					component.OnDeselected(deselectEventArgs);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0013D7A8 File Offset: 0x0013B9A8
		private bool RemoveObjectCollectionFromSelection(List<GameObject> objectCollection, ObjectDeselectActionType deselectActionType)
		{
			bool result = false;
			foreach (GameObject gameObj in objectCollection)
			{
				result = this.RemoveObjectFromSelection(gameObj, deselectActionType);
			}
			return result;
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x0013D7FC File Offset: 0x0013B9FC
		private bool ClearAndAddObjectToSelection(GameObject gameObj, ObjectSelectActionType selectActionType)
		{
			if (!this.IsSelectionExactMatch(new List<GameObject>
			{
				gameObj
			}))
			{
				this._selectedObjects.Clear();
				this.AddObjectToSelection(gameObj, selectActionType);
				return true;
			}
			return false;
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0013D829 File Offset: 0x0013BA29
		private bool ClearAndAddObjectCollectionToSelection(List<GameObject> objectCollection, ObjectSelectActionType selectActionType)
		{
			if (!this.IsSelectionExactMatch(objectCollection))
			{
				this._selectedObjects.Clear();
				this.AddObjectCollectionToSelection(objectCollection, selectActionType);
				return true;
			}
			return false;
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x0013D84B File Offset: 0x0013BA4B
		private void OnSelectionChanged(ObjectSelectionChangedEventArgs selectionChangedEventArgs)
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(selectionChangedEventArgs);
			}
		}

		// Token: 0x04001E8D RID: 7821
		[SerializeField]
		private ObjectSelectionSettings _objectSelectionSettings = new ObjectSelectionSettings();

		// Token: 0x04001E8E RID: 7822
		[SerializeField]
		private ShortcutKeys _appendToSelectionShortcut = new ShortcutKeys("Append to selection", 0)
		{
			LCtrl = true,
			UseMouseButtons = false
		};

		// Token: 0x04001E8F RID: 7823
		[SerializeField]
		private ShortcutKeys _multiDeselectShortcut = new ShortcutKeys("Multi deselect", 0)
		{
			LShift = true,
			UseMouseButtons = false
		};

		// Token: 0x04001E90 RID: 7824
		[SerializeField]
		private ShortcutKeys _duplicateSelectionShortcut = new ShortcutKeys("Duplicate selection", 1)
		{
			Key0 = KeyCode.D,
			LCtrl = true,
			UseMouseButtons = false
		};

		// Token: 0x04001E91 RID: 7825
		private HashSet<GameObject> _selectedObjects = new HashSet<GameObject>();

		// Token: 0x04001E92 RID: 7826
		private HashSet<GameObject> _maskedObjects = new HashSet<GameObject>();

		// Token: 0x04001E93 RID: 7827
		private GameObject _lastSelectedGameObject;

		// Token: 0x04001E94 RID: 7828
		[SerializeField]
		private ObjectSelectionRectangle _objectSelectionRectangle = new ObjectSelectionRectangle();

		// Token: 0x04001E95 RID: 7829
		private ObjectSelectionSnapshot _multiSelectPreChangeSnapshot;

		// Token: 0x04001E96 RID: 7830
		private ObjectSelectionSnapshot _multiSelectPostChangeSnapshot;

		// Token: 0x04001E97 RID: 7831
		private bool _selectionChangedWithShape;

		// Token: 0x04001E98 RID: 7832
		private ObjectSelectActionType _lastSelectActionType = ObjectSelectActionType.None;

		// Token: 0x04001E99 RID: 7833
		private ObjectDeselectActionType _lastDeselectActionType = ObjectDeselectActionType.None;

		// Token: 0x020007F7 RID: 2039
		// (Invoke) Token: 0x0600408D RID: 16525
		public delegate void SelectionChangedHandler(ObjectSelectionChangedEventArgs selectionChangedEventArgs);
	}
}
