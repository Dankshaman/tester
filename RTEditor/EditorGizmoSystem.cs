using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003CF RID: 975
	public class EditorGizmoSystem : MonoSingletonBase<EditorGizmoSystem>, IMessageListener
	{
		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06002D84 RID: 11652 RVA: 0x0013BEC8 File Offset: 0x0013A0C8
		// (remove) Token: 0x06002D85 RID: 11653 RVA: 0x0013BF00 File Offset: 0x0013A100
		public event EditorGizmoSystem.ActiveGizmoTypeChangedHandler ActiveGizmoTypeChanged;

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002D86 RID: 11654 RVA: 0x0013BF35 File Offset: 0x0013A135
		// (set) Token: 0x06002D87 RID: 11655 RVA: 0x0013BF3D File Offset: 0x0013A13D
		public TranslationGizmo TranslationGizmo
		{
			get
			{
				return this._translationGizmo;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._translationGizmo = value;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06002D88 RID: 11656 RVA: 0x0013BF50 File Offset: 0x0013A150
		// (set) Token: 0x06002D89 RID: 11657 RVA: 0x0013BF58 File Offset: 0x0013A158
		public RotationGizmo RotationGizmo
		{
			get
			{
				return this._rotationGizmo;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._rotationGizmo = value;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002D8A RID: 11658 RVA: 0x0013BF6B File Offset: 0x0013A16B
		// (set) Token: 0x06002D8B RID: 11659 RVA: 0x0013BF73 File Offset: 0x0013A173
		public ScaleGizmo ScaleGizmo
		{
			get
			{
				return this._scaleGizmo;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._scaleGizmo = value;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002D8C RID: 11660 RVA: 0x0013BF86 File Offset: 0x0013A186
		// (set) Token: 0x06002D8D RID: 11661 RVA: 0x0013BF8E File Offset: 0x0013A18E
		public VolumeScaleGizmo VolumeScaleGizmo
		{
			get
			{
				return this._volumeScaleGizmo;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._volumeScaleGizmo = value;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002D8E RID: 11662 RVA: 0x0013BFA1 File Offset: 0x0013A1A1
		// (set) Token: 0x06002D8F RID: 11663 RVA: 0x0013BFA9 File Offset: 0x0013A1A9
		public TransformSpace TransformSpace
		{
			get
			{
				return this._transformSpace;
			}
			set
			{
				this.ChangeTransformSpace(value);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002D90 RID: 11664 RVA: 0x0013BFB2 File Offset: 0x0013A1B2
		// (set) Token: 0x06002D91 RID: 11665 RVA: 0x0013BFBA File Offset: 0x0013A1BA
		public GizmoType ActiveGizmoType
		{
			get
			{
				return this._activeGizmoType;
			}
			set
			{
				this.ChangeActiveGizmo(value);
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002D92 RID: 11666 RVA: 0x0013BFC3 File Offset: 0x0013A1C3
		public Gizmo ActiveGizmo
		{
			get
			{
				return this._activeGizmo;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002D93 RID: 11667 RVA: 0x0013BFCB File Offset: 0x0013A1CB
		// (set) Token: 0x06002D94 RID: 11668 RVA: 0x0013BFD3 File Offset: 0x0013A1D3
		public TransformPivotPoint TransformPivotPoint
		{
			get
			{
				return this._transformPivotPoint;
			}
			set
			{
				this.ChangeTransformPivotPoint(value);
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002D95 RID: 11669 RVA: 0x0013BFDC File Offset: 0x0013A1DC
		public bool AreGizmosTurnedOff
		{
			get
			{
				return this._areGizmosTurnedOff;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x0013BFE4 File Offset: 0x0013A1E4
		public ShortcutKeys ActivateTranslationGizmoShortcut
		{
			get
			{
				return this._activateTranslationGizmoShortcut;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002D97 RID: 11671 RVA: 0x0013BFEC File Offset: 0x0013A1EC
		public ShortcutKeys ActivateRotationGizmoShortcut
		{
			get
			{
				return this._activateRotationGizmoShortcut;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002D98 RID: 11672 RVA: 0x0013BFF4 File Offset: 0x0013A1F4
		public ShortcutKeys ActivateScaleGizmoShortcut
		{
			get
			{
				return this._activateScaleGizmoShortcut;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06002D99 RID: 11673 RVA: 0x0013BFFC File Offset: 0x0013A1FC
		public ShortcutKeys ActivateVolumeScaleGizmoShortcut
		{
			get
			{
				return this._activateVolumeScaleGizmoShortcut;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x0013C004 File Offset: 0x0013A204
		public ShortcutKeys ActivateGlobalTransformShortcut
		{
			get
			{
				return this._activateGlobalTransformShortcut;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06002D9B RID: 11675 RVA: 0x0013C00C File Offset: 0x0013A20C
		public ShortcutKeys ActivateLocalTransformShortcut
		{
			get
			{
				return this._activateLocalTransformShortcut;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002D9C RID: 11676 RVA: 0x0013C014 File Offset: 0x0013A214
		public ShortcutKeys TurnOffGizmosShortcut
		{
			get
			{
				return this._turnOffGizmosShortcut;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06002D9D RID: 11677 RVA: 0x0013C01C File Offset: 0x0013A21C
		public ShortcutKeys TogglePivotShortcut
		{
			get
			{
				return this._togglePivotShortcut;
			}
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x0013C024 File Offset: 0x0013A224
		public bool IsActiveGizmoReadyForObjectManipulation()
		{
			return !(this._activeGizmo == null) && this._activeGizmo.gameObject.activeSelf && this._activeGizmo.IsReadyForObjectManipulation();
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x0013C053 File Offset: 0x0013A253
		public void TurnOffGizmos()
		{
			this._areGizmosTurnedOff = true;
			this.DeactivateAllGizmoObjects();
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x0013C064 File Offset: 0x0013A264
		private void Start()
		{
			this.ValidatePropertiesForRuntime();
			this.DeactivateAllGizmoObjects();
			this.ConnectObjectSelectionToGizmos();
			this.ChangeActiveGizmo(this._activeGizmoType);
			this.ChangeTransformPivotPoint(this._transformPivotPoint);
			MessageListenerDatabase instance = MonoSingletonBase<MessageListenerDatabase>.Instance;
			instance.RegisterListenerForMessage(MessageType.GizmoTransformedObjects, this);
			instance.RegisterListenerForMessage(MessageType.GizmoTransformOperationWasUndone, this);
			instance.RegisterListenerForMessage(MessageType.GizmoTransformOperationWasRedone, this);
			instance.RegisterListenerForMessage(MessageType.VertexSnappingDisabled, this);
			MonoSingletonBase<EditorObjectSelection>.Instance.SelectionChanged += this.OnSelectionChanged;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x0013C0D8 File Offset: 0x0013A2D8
		private void Update()
		{
			if (this._activateTranslationGizmoShortcut.IsActiveInCurrentFrame())
			{
				this.ChangeActiveGizmo(GizmoType.Translation);
			}
			else if (this._activateRotationGizmoShortcut.IsActiveInCurrentFrame())
			{
				this.ChangeActiveGizmo(GizmoType.Rotation);
			}
			else if (this._activateScaleGizmoShortcut.IsActiveInCurrentFrame())
			{
				this.ChangeActiveGizmo(GizmoType.Scale);
			}
			else if (this._activateVolumeScaleGizmoShortcut.IsActiveInCurrentFrame())
			{
				this.ChangeActiveGizmo(GizmoType.VolumeScale);
			}
			if (this._activateGlobalTransformShortcut.IsActiveInCurrentFrame())
			{
				this.ChangeTransformSpace(TransformSpace.Global);
			}
			else if (this._activateLocalTransformShortcut.IsActiveInCurrentFrame())
			{
				this.ChangeTransformSpace(TransformSpace.Local);
			}
			if (this._turnOffGizmosShortcut.IsActiveInCurrentFrame())
			{
				this.TurnOffGizmos();
				return;
			}
			if (this._togglePivotShortcut.IsActiveInCurrentFrame())
			{
				TransformPivotPoint transformPivotPoint = (this._transformPivotPoint == TransformPivotPoint.Center) ? TransformPivotPoint.MeshPivot : TransformPivotPoint.Center;
				this.ChangeTransformPivotPoint(transformPivotPoint);
			}
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x0013C19C File Offset: 0x0013A39C
		private void ValidatePropertiesForRuntime()
		{
			bool flag = true;
			if (this._translationGizmo == null)
			{
				Debug.LogError("EditorGizmoSystem.Start: Missing translation gizmo. Please assign a game object with the 'TranslationGizmo' script attached to it.");
				flag = false;
			}
			if (this._rotationGizmo == null)
			{
				Debug.LogError("EditorGizmoSystem.Start: Missing rotation gizmo. Please assign a game object with the 'RotationGizmo' script attached to it.");
				flag = false;
			}
			if (this._scaleGizmo == null)
			{
				Debug.LogError("EditorGizmoSystem.Start: Missing scale gizmo. Please assign a game object with the 'ScaleGizmo' script attached to it.");
				flag = false;
			}
			if (this._volumeScaleGizmo == null)
			{
				Debug.LogError("EditorGizmoSystem.Start: Missing volume scale gizmo. Please assign a game object with the 'VolumeScaleGizmo' script attached to it.");
				flag = false;
			}
			if (!flag)
			{
				ApplicationHelper.Quit();
			}
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x0013C21B File Offset: 0x0013A41B
		private void ConnectObjectSelectionToGizmos()
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			instance.ConnectObjectSelectionToGizmo(this._translationGizmo);
			instance.ConnectObjectSelectionToGizmo(this._rotationGizmo);
			instance.ConnectObjectSelectionToGizmo(this._scaleGizmo);
			instance.ConnectObjectSelectionToGizmo(this._volumeScaleGizmo);
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x0013C254 File Offset: 0x0013A454
		private void DeactivateAllGizmoObjects()
		{
			this._translationGizmo.gameObject.SetActive(false);
			this._rotationGizmo.gameObject.SetActive(false);
			this._scaleGizmo.gameObject.SetActive(false);
			this._volumeScaleGizmo.gameObject.SetActive(false);
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x0013C2A8 File Offset: 0x0013A4A8
		private void ChangeActiveGizmo(GizmoType gizmoType)
		{
			this._areGizmosTurnedOff = false;
			Gizmo activeGizmo = this._activeGizmo;
			bool flag = gizmoType == this._activeGizmoType;
			this._activeGizmoType = gizmoType;
			this._activeGizmo = this.GetGizmoByType(gizmoType);
			if (activeGizmo != null)
			{
				activeGizmo.gameObject.SetActive(false);
				this.EstablishActiveGizmoPosition();
				this.UpdateActiveGizmoRotation();
			}
			if (MonoSingletonBase<EditorObjectSelection>.Instance.NumberOfSelectedObjects != 0)
			{
				this._activeGizmo.gameObject.SetActive(true);
			}
			else
			{
				this._activeGizmo.gameObject.SetActive(false);
			}
			if (!flag && this.ActiveGizmoTypeChanged != null)
			{
				this.ActiveGizmoTypeChanged(this._activeGizmoType);
			}
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x0013C34C File Offset: 0x0013A54C
		private void ChangeTransformSpace(TransformSpace transformSpace)
		{
			if (transformSpace == this._transformSpace)
			{
				return;
			}
			this._transformSpace = transformSpace;
			this.UpdateActiveGizmoRotation();
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x0013C368 File Offset: 0x0013A568
		private void ChangeTransformPivotPoint(TransformPivotPoint transformPivotPoint)
		{
			if (this._transformPivotPoint == transformPivotPoint)
			{
				return;
			}
			this._transformPivotPoint = transformPivotPoint;
			this._translationGizmo.TransformPivotPoint = this._transformPivotPoint;
			this._rotationGizmo.TransformPivotPoint = this._transformPivotPoint;
			this._scaleGizmo.TransformPivotPoint = this._transformPivotPoint;
			this._volumeScaleGizmo.TransformPivotPoint = this._transformPivotPoint;
			this.EstablishActiveGizmoPosition();
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0013C3D0 File Offset: 0x0013A5D0
		private Gizmo GetGizmoByType(GizmoType gizmoType)
		{
			if (gizmoType == GizmoType.Translation)
			{
				return this._translationGizmo;
			}
			if (gizmoType == GizmoType.Rotation)
			{
				return this._rotationGizmo;
			}
			if (gizmoType == GizmoType.Scale)
			{
				return this._scaleGizmo;
			}
			return this._volumeScaleGizmo;
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x0013C3F8 File Offset: 0x0013A5F8
		private void EstablishActiveGizmoPosition()
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			if (this._activeGizmo.GetGizmoType() != GizmoType.VolumeScale && this._activeGizmo != null)
			{
				if (this._transformPivotPoint == TransformPivotPoint.MeshPivot && instance.LastSelectedGameObject != null)
				{
					this._activeGizmo.transform.position = instance.LastSelectedGameObject.transform.position;
				}
				else
				{
					this._activeGizmo.transform.position = instance.GetSelectionWorldCenter();
				}
			}
			if (this._volumeScaleGizmo != null && instance.NumberOfSelectedObjects == 1)
			{
				this._volumeScaleGizmo.transform.position = instance.LastSelectedGameObject.transform.position;
				this._volumeScaleGizmo.RefreshTargets();
			}
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x0013C4B8 File Offset: 0x0013A6B8
		private void UpdateActiveGizmoRotation()
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			if (this._activeGizmoType == GizmoType.VolumeScale)
			{
				if (instance.NumberOfSelectedObjects == 1)
				{
					this._activeGizmo.transform.rotation = instance.LastSelectedGameObject.transform.rotation;
				}
				return;
			}
			if ((this._transformSpace == TransformSpace.Global && this._activeGizmoType != GizmoType.Scale) || instance.LastSelectedGameObject == null)
			{
				this._activeGizmo.transform.rotation = Quaternion.identity;
				return;
			}
			this._activeGizmo.transform.rotation = instance.LastSelectedGameObject.transform.rotation;
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x0013C554 File Offset: 0x0013A754
		private void OnSelectionChanged(ObjectSelectionChangedEventArgs selChangedEventArgs)
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			if (instance.NumberOfSelectedObjects == 0)
			{
				this._activeGizmo.gameObject.SetActive(false);
			}
			else if (!this._areGizmosTurnedOff && instance.NumberOfSelectedObjects != 0 && !this._activeGizmo.gameObject.activeSelf)
			{
				this._activeGizmo.gameObject.SetActive(true);
			}
			this.EstablishActiveGizmoPosition();
			this.UpdateActiveGizmoRotation();
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x0013C5C4 File Offset: 0x0013A7C4
		public void RespondToMessage(Message message)
		{
			switch (message.Type)
			{
			case MessageType.GizmoTransformedObjects:
				this.RespondToMessage(message as GizmoTransformedObjectsMessage);
				return;
			case MessageType.GizmoTransformOperationWasUndone:
				this.RespondToMessage(message as GizmoTransformOperationWasUndoneMessage);
				return;
			case MessageType.GizmoTransformOperationWasRedone:
				this.RespondToMessage(message as GizmoTransformOperationWasRedoneMessage);
				return;
			case MessageType.VertexSnappingEnabled:
				break;
			case MessageType.VertexSnappingDisabled:
				this.RespondToMessage(message as VertexSnappingDisabledMessage);
				break;
			default:
				return;
			}
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x0013C626 File Offset: 0x0013A826
		private void RespondToMessage(GizmoTransformedObjectsMessage message)
		{
			this.UpdateActiveGizmoRotation();
			this.EstablishActiveGizmoPosition();
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x0013C634 File Offset: 0x0013A834
		private void RespondToMessage(GizmoTransformOperationWasUndoneMessage message)
		{
			this.EstablishActiveGizmoPosition();
			this.UpdateActiveGizmoRotation();
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x0013C634 File Offset: 0x0013A834
		private void RespondToMessage(GizmoTransformOperationWasRedoneMessage message)
		{
			this.EstablishActiveGizmoPosition();
			this.UpdateActiveGizmoRotation();
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x0013C642 File Offset: 0x0013A842
		private void RespondToMessage(VertexSnappingDisabledMessage message)
		{
			this.EstablishActiveGizmoPosition();
		}

		// Token: 0x04001E7B RID: 7803
		[SerializeField]
		private ShortcutKeys _activateTranslationGizmoShortcut = new ShortcutKeys("Activate move gizmo", 1)
		{
			Key0 = KeyCode.W,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E7C RID: 7804
		[SerializeField]
		private ShortcutKeys _activateRotationGizmoShortcut = new ShortcutKeys("Activate rotation gizmo", 1)
		{
			Key0 = KeyCode.E,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E7D RID: 7805
		[SerializeField]
		private ShortcutKeys _activateScaleGizmoShortcut = new ShortcutKeys("Activate scale gizmo", 1)
		{
			Key0 = KeyCode.R,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E7E RID: 7806
		[SerializeField]
		private ShortcutKeys _activateVolumeScaleGizmoShortcut = new ShortcutKeys("Activate volume scale gizmo", 1)
		{
			Key0 = KeyCode.U,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E7F RID: 7807
		[SerializeField]
		private ShortcutKeys _activateGlobalTransformShortcut = new ShortcutKeys("Activate global transform", 1)
		{
			Key0 = KeyCode.G,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E80 RID: 7808
		[SerializeField]
		private ShortcutKeys _activateLocalTransformShortcut = new ShortcutKeys("Activate local transform", 1)
		{
			Key0 = KeyCode.L,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E81 RID: 7809
		[SerializeField]
		private ShortcutKeys _turnOffGizmosShortcut = new ShortcutKeys("Turn off gizmos", 1)
		{
			Key0 = KeyCode.Q,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E82 RID: 7810
		[SerializeField]
		private ShortcutKeys _togglePivotShortcut = new ShortcutKeys("Toggle pivot", 1)
		{
			Key0 = KeyCode.P,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictMouseCheck = true
		};

		// Token: 0x04001E83 RID: 7811
		[SerializeField]
		private TranslationGizmo _translationGizmo;

		// Token: 0x04001E84 RID: 7812
		[SerializeField]
		private RotationGizmo _rotationGizmo;

		// Token: 0x04001E85 RID: 7813
		[SerializeField]
		private ScaleGizmo _scaleGizmo;

		// Token: 0x04001E86 RID: 7814
		[SerializeField]
		private VolumeScaleGizmo _volumeScaleGizmo;

		// Token: 0x04001E87 RID: 7815
		private Gizmo _activeGizmo;

		// Token: 0x04001E88 RID: 7816
		[SerializeField]
		private TransformSpace _transformSpace = TransformSpace.Global;

		// Token: 0x04001E89 RID: 7817
		[SerializeField]
		private GizmoType _activeGizmoType;

		// Token: 0x04001E8A RID: 7818
		[SerializeField]
		private TransformPivotPoint _transformPivotPoint = TransformPivotPoint.Center;

		// Token: 0x04001E8B RID: 7819
		private bool _areGizmosTurnedOff;

		// Token: 0x020007F6 RID: 2038
		// (Invoke) Token: 0x06004089 RID: 16521
		public delegate void ActiveGizmoTypeChangedHandler(GizmoType newGizmoType);
	}
}
