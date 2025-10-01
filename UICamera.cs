using System;
using System.Collections.Generic;
using RTEditor;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000084 RID: 132
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)")]
[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00014D66 File Offset: 0x00012F66
	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060005F3 RID: 1523 RVA: 0x000290BB File Offset: 0x000272BB
	// (set) Token: 0x060005F4 RID: 1524 RVA: 0x000290CE File Offset: 0x000272CE
	public static bool disableController
	{
		get
		{
			return UICamera.mDisableController && !UIPopupList.isOpen;
		}
		set
		{
			UICamera.mDisableController = value;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060005F5 RID: 1525 RVA: 0x000290D6 File Offset: 0x000272D6
	// (set) Token: 0x060005F6 RID: 1526 RVA: 0x000290DD File Offset: 0x000272DD
	[Obsolete("Use lastEventPosition instead. It handles controller input properly.")]
	public static Vector2 lastTouchPosition
	{
		get
		{
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060005F7 RID: 1527 RVA: 0x000290E8 File Offset: 0x000272E8
	// (set) Token: 0x060005F8 RID: 1528 RVA: 0x000290DD File Offset: 0x000272DD
	public static Vector2 lastEventPosition
	{
		get
		{
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				GameObject hoveredObject = UICamera.hoveredObject;
				if (hoveredObject != null)
				{
					Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(hoveredObject.transform);
					return NGUITools.FindCameraForLayer(hoveredObject.layer).WorldToScreenPoint(bounds.center);
				}
			}
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0002913A File Offset: 0x0002733A
	public static UICamera first
	{
		get
		{
			if (UICamera.list == null || UICamera.list.size == 0)
			{
				return null;
			}
			return UICamera.list[0];
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060005FA RID: 1530 RVA: 0x0002915C File Offset: 0x0002735C
	// (set) Token: 0x060005FB RID: 1531 RVA: 0x000291D8 File Offset: 0x000273D8
	public static UICamera.ControlScheme currentScheme
	{
		get
		{
			if (UICamera.mCurrentKey == KeyCode.None)
			{
				return UICamera.ControlScheme.Touch;
			}
			if (UICamera.mCurrentKey >= KeyCode.JoystickButton0)
			{
				return UICamera.ControlScheme.Controller;
			}
			if (!(UICamera.current != null))
			{
				return UICamera.ControlScheme.Mouse;
			}
			if (UICamera.mLastScheme == UICamera.ControlScheme.Controller && (UICamera.mCurrentKey == UICamera.current.submitKey0 || UICamera.mCurrentKey == UICamera.current.submitKey1))
			{
				return UICamera.ControlScheme.Controller;
			}
			if (UICamera.current.useMouse)
			{
				return UICamera.ControlScheme.Mouse;
			}
			if (UICamera.current.useTouch)
			{
				return UICamera.ControlScheme.Touch;
			}
			return UICamera.ControlScheme.Controller;
		}
		set
		{
			if (UICamera.mLastScheme != value)
			{
				if (value == UICamera.ControlScheme.Mouse)
				{
					UICamera.currentKey = KeyCode.Mouse0;
				}
				else if (value == UICamera.ControlScheme.Controller)
				{
					UICamera.currentKey = KeyCode.JoystickButton0;
				}
				else if (value == UICamera.ControlScheme.Touch)
				{
					UICamera.currentKey = KeyCode.None;
				}
				else
				{
					UICamera.currentKey = KeyCode.Alpha0;
				}
				UICamera.mLastScheme = value;
			}
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060005FC RID: 1532 RVA: 0x00029225 File Offset: 0x00027425
	// (set) Token: 0x060005FD RID: 1533 RVA: 0x0002922C File Offset: 0x0002742C
	public static KeyCode currentKey
	{
		get
		{
			return UICamera.mCurrentKey;
		}
		set
		{
			if (UICamera.mCurrentKey != value)
			{
				UICamera.ControlScheme controlScheme = UICamera.mLastScheme;
				UICamera.mCurrentKey = value;
				UICamera.mLastScheme = UICamera.currentScheme;
				if (controlScheme != UICamera.mLastScheme)
				{
					UICamera.HideTooltip();
					if (UICamera.mLastScheme == UICamera.ControlScheme.Mouse)
					{
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
					}
					else if (UICamera.current != null && UICamera.current.autoHideCursor)
					{
						Cursor.visible = false;
						Cursor.lockState = CursorLockMode.Locked;
						UICamera.mMouse[0].ignoreDelta = 2;
					}
					if (UICamera.onSchemeChange != null)
					{
						UICamera.onSchemeChange();
					}
				}
			}
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060005FE RID: 1534 RVA: 0x000292BC File Offset: 0x000274BC
	public static Ray currentRay
	{
		get
		{
			if (!(UICamera.currentCamera != null) || UICamera.currentTouch == null)
			{
				return default(Ray);
			}
			return UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x00029300 File Offset: 0x00027500
	public static bool inputHasFocus
	{
		get
		{
			return UICamera.mInputFocus && UICamera.mSelected && UICamera.mSelected.activeInHierarchy;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000600 RID: 1536 RVA: 0x00029324 File Offset: 0x00027524
	// (set) Token: 0x06000601 RID: 1537 RVA: 0x0002932B File Offset: 0x0002752B
	[Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
	public static GameObject genericEventHandler
	{
		get
		{
			return UICamera.mGenericHandler;
		}
		set
		{
			UICamera.mGenericHandler = value;
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000602 RID: 1538 RVA: 0x00029333 File Offset: 0x00027533
	public static UICamera.MouseOrTouch mouse0
	{
		get
		{
			return UICamera.mMouse[0];
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000603 RID: 1539 RVA: 0x0002933C File Offset: 0x0002753C
	public static UICamera.MouseOrTouch mouse1
	{
		get
		{
			return UICamera.mMouse[1];
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000604 RID: 1540 RVA: 0x00029345 File Offset: 0x00027545
	public static UICamera.MouseOrTouch mouse2
	{
		get
		{
			return UICamera.mMouse[2];
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000605 RID: 1541 RVA: 0x0002934E File Offset: 0x0002754E
	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000606 RID: 1542 RVA: 0x0002935B File Offset: 0x0002755B
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x0002937D File Offset: 0x0002757D
	public static GameObject tooltipObject
	{
		get
		{
			return UICamera.mTooltip;
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00029384 File Offset: 0x00027584
	public static bool IsPartOfUI(GameObject go)
	{
		return !(go == null) && !(go == UICamera.fallThrough) && NGUITools.FindInParents<UIRoot>(go) != null;
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x000293AC File Offset: 0x000275AC
	public static bool isOverUI
	{
		get
		{
			int frameCount = Time.frameCount;
			if (UICamera.mLastOverCheck != frameCount)
			{
				UICamera.mLastOverCheck = frameCount;
				if (UICamera.currentTouch != null)
				{
					UICamera.mLastOverResult = UICamera.currentTouch.isOverUI;
					return UICamera.mLastOverResult;
				}
				int i = 0;
				int count = UICamera.activeTouches.Count;
				while (i < count)
				{
					if (UICamera.IsPartOfUI(UICamera.activeTouches[i].pressed))
					{
						UICamera.mLastOverResult = true;
						return UICamera.mLastOverResult;
					}
					i++;
				}
				for (int j = 0; j < 3; j++)
				{
					if (UICamera.IsPartOfUI(UICamera.mMouse[j].current))
					{
						UICamera.mLastOverResult = true;
						return UICamera.mLastOverResult;
					}
				}
				UICamera.mLastOverResult = UICamera.IsPartOfUI(UICamera.controller.pressed);
			}
			return UICamera.mLastOverResult;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600060A RID: 1546 RVA: 0x0002946C File Offset: 0x0002766C
	public static bool uiHasFocus
	{
		get
		{
			int frameCount = Time.frameCount;
			if (UICamera.mLastFocusCheck != frameCount)
			{
				UICamera.mLastFocusCheck = frameCount;
				if (UICamera.inputHasFocus)
				{
					UICamera.mLastFocusResult = true;
					return UICamera.mLastFocusResult;
				}
				if (UICamera.currentTouch != null)
				{
					UICamera.mLastFocusResult = UICamera.currentTouch.isOverUI;
					return UICamera.mLastFocusResult;
				}
				int i = 0;
				int count = UICamera.activeTouches.Count;
				while (i < count)
				{
					if (UICamera.IsPartOfUI(UICamera.activeTouches[i].pressed))
					{
						UICamera.mLastFocusResult = true;
						return UICamera.mLastFocusResult;
					}
					i++;
				}
				for (int j = 0; j < 3; j++)
				{
					UICamera.MouseOrTouch mouseOrTouch = UICamera.mMouse[j];
					if (UICamera.IsPartOfUI(mouseOrTouch.pressed) || UICamera.IsPartOfUI(mouseOrTouch.current))
					{
						UICamera.mLastFocusResult = true;
						return UICamera.mLastFocusResult;
					}
				}
				UICamera.mLastFocusResult = UICamera.IsPartOfUI(UICamera.controller.pressed);
			}
			return UICamera.mLastFocusResult;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x00029550 File Offset: 0x00027750
	public static bool interactingWithUI
	{
		get
		{
			int frameCount = Time.frameCount;
			if (UICamera.mLastInteractionCheck != frameCount)
			{
				UICamera.mLastInteractionCheck = frameCount;
				if (UICamera.inputHasFocus)
				{
					UICamera.mLastInteractionResult = true;
					return UICamera.mLastInteractionResult;
				}
				int i = 0;
				int count = UICamera.activeTouches.Count;
				while (i < count)
				{
					if (UICamera.IsPartOfUI(UICamera.activeTouches[i].pressed))
					{
						UICamera.mLastInteractionResult = true;
						return UICamera.mLastInteractionResult;
					}
					i++;
				}
				for (int j = 0; j < 3; j++)
				{
					if (UICamera.IsPartOfUI(UICamera.mMouse[j].pressed))
					{
						UICamera.mLastInteractionResult = true;
						return UICamera.mLastInteractionResult;
					}
				}
				UICamera.mLastInteractionResult = UICamera.IsPartOfUI(UICamera.controller.pressed);
			}
			return UICamera.mLastInteractionResult;
		}
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00029605 File Offset: 0x00027805
	public static void CalculateHoveredUIObject()
	{
		UICamera.HoveredUIObject = UICamera.GetHoveredUIObject();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00029614 File Offset: 0x00027814
	private static GameObject GetHoveredUIObject()
	{
		bool flag = StandaloneInputModuleV2.Instance.IsPointerOver2DXmlObject();
		bool flag2 = !flag && StandaloneInputModuleV2.Instance.IsPointerOverGameObject();
		if (flag)
		{
			return StandaloneInputModuleV2.Instance.GameObjectUnderPointer();
		}
		if (MonoSingletonBase<EditorGizmoSystem>.Instance.IsActiveGizmoReadyForObjectManipulation())
		{
			return MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmo.gameObject;
		}
		if (UICamera.last2DRaycastHit != null)
		{
			return UICamera.last2DRaycastHit;
		}
		if (flag2)
		{
			return StandaloneInputModuleV2.Instance.GameObjectUnderPointer();
		}
		if (UICamera.last3DRaycastHit != null)
		{
			return UICamera.last3DRaycastHit;
		}
		return null;
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600060E RID: 1550 RVA: 0x0002969A File Offset: 0x0002789A
	public static GameObject RayHitObject
	{
		get
		{
			return UICamera.mRayHitObject;
		}
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x000296A1 File Offset: 0x000278A1
	public static bool HoverOverUI()
	{
		return UICamera.HoveredUIObject != null;
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000610 RID: 1552 RVA: 0x000296B0 File Offset: 0x000278B0
	// (set) Token: 0x06000611 RID: 1553 RVA: 0x00029708 File Offset: 0x00027908
	public static GameObject hoveredObject
	{
		get
		{
			if (UICamera.currentTouch != null && (UICamera.currentScheme != UICamera.ControlScheme.Mouse || UICamera.currentTouch.dragStarted))
			{
				return UICamera.currentTouch.current;
			}
			if (UICamera.mHover && UICamera.mHover.activeInHierarchy)
			{
				return UICamera.mHover;
			}
			UICamera.mHover = null;
			return null;
		}
		set
		{
			if (UICamera.mHover == value)
			{
				return;
			}
			bool flag = false;
			UICamera uicamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.ShowTooltip(null);
			if (UICamera.mSelected && UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
				UICamera.mSelected = null;
			}
			if (UICamera.mHover)
			{
				UICamera.Notify(UICamera.mHover, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, false);
				}
			}
			UICamera.mHover = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (UICamera.mHover)
			{
				if (UICamera.mHover != UICamera.controller.current && UICamera.mHover.GetComponent<UIKeyNavigation>() != null)
				{
					UICamera.controller.current = UICamera.mHover;
				}
				if (flag)
				{
					UICamera uicamera2 = (UICamera.mHover != null) ? UICamera.FindCameraForLayer(UICamera.mHover.layer) : UICamera.list[0];
					if (uicamera2 != null)
					{
						UICamera.current = uicamera2;
						UICamera.currentCamera = uicamera2.cachedCamera;
					}
				}
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, true);
				}
				UICamera.Notify(UICamera.mHover, "OnHover", true);
			}
			if (flag)
			{
				UICamera.current = uicamera;
				UICamera.currentCamera = ((uicamera != null) ? uicamera.cachedCamera : null);
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000612 RID: 1554 RVA: 0x000298C4 File Offset: 0x00027AC4
	// (set) Token: 0x06000613 RID: 1555 RVA: 0x00029A08 File Offset: 0x00027C08
	public static GameObject controllerNavigationObject
	{
		get
		{
			if (UICamera.controller.current && UICamera.controller.current.activeInHierarchy)
			{
				return UICamera.controller.current;
			}
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.current != null && UICamera.current.useController && !UICamera.ignoreControllerInput && UIKeyNavigation.list.size > 0)
			{
				for (int i = 0; i < UIKeyNavigation.list.size; i++)
				{
					UIKeyNavigation uikeyNavigation = UIKeyNavigation.list[i];
					if (uikeyNavigation && uikeyNavigation.constraint != UIKeyNavigation.Constraint.Explicit && uikeyNavigation.startsSelected)
					{
						UICamera.hoveredObject = uikeyNavigation.gameObject;
						UICamera.controller.current = UICamera.mHover;
						return UICamera.mHover;
					}
				}
				if (UICamera.mHover == null)
				{
					for (int j = 0; j < UIKeyNavigation.list.size; j++)
					{
						UIKeyNavigation uikeyNavigation2 = UIKeyNavigation.list[j];
						if (uikeyNavigation2 && uikeyNavigation2.constraint != UIKeyNavigation.Constraint.Explicit)
						{
							UICamera.hoveredObject = uikeyNavigation2.gameObject;
							UICamera.controller.current = UICamera.mHover;
							return UICamera.mHover;
						}
					}
				}
			}
			UICamera.controller.current = null;
			return null;
		}
		set
		{
			if (UICamera.controller.current != value && UICamera.controller.current)
			{
				UICamera.Notify(UICamera.controller.current, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.controller.current, false);
				}
				UICamera.controller.current = null;
			}
			UICamera.hoveredObject = value;
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000614 RID: 1556 RVA: 0x00029A7F File Offset: 0x00027C7F
	// (set) Token: 0x06000615 RID: 1557 RVA: 0x00029AA8 File Offset: 0x00027CA8
	public static GameObject selectedObject
	{
		get
		{
			if (UICamera.mSelected && UICamera.mSelected.activeInHierarchy)
			{
				return UICamera.mSelected;
			}
			UICamera.mSelected = null;
			return null;
		}
		set
		{
			if (UICamera.mSelected == value || !UICamera.changeSelection)
			{
				UICamera.hoveredObject = value;
				UICamera.controller.current = value;
				return;
			}
			UICamera.ShowTooltip(null);
			bool flag = false;
			UICamera uicamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.mInputFocus = false;
			if (UICamera.mSelected)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
			}
			UICamera.mSelected = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (value != null && value.GetComponent<UIKeyNavigation>() != null)
			{
				UICamera.controller.current = value;
			}
			if (UICamera.mSelected && flag)
			{
				UICamera uicamera2 = (UICamera.mSelected != null) ? UICamera.FindCameraForLayer(UICamera.mSelected.layer) : UICamera.list[0];
				if (uicamera2 != null)
				{
					UICamera.current = uicamera2;
					UICamera.currentCamera = uicamera2.cachedCamera;
				}
			}
			if (UICamera.mSelected)
			{
				UICamera.mInputFocus = (UICamera.mSelected.activeInHierarchy && UICamera.mSelected.GetComponent<UIInput>() != null);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, true);
				}
				UICamera.Notify(UICamera.mSelected, "OnSelect", true);
			}
			if (flag)
			{
				UICamera.current = uicamera;
				UICamera.currentCamera = ((uicamera != null) ? uicamera.cachedCamera : null);
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00029C50 File Offset: 0x00027E50
	public static bool SelectIsInput()
	{
		return UICamera.inputHasFocus || TabletScript.TabletHasFocus || EventSystem.current.currentSelectedGameObject != null;
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00029C74 File Offset: 0x00027E74
	public static bool HoverIsScroll()
	{
		return EventSystem.current.IsPointerOverGameObject() || (UICamera.HoveredUIObject && (UICamera.HoveredUIObject.GetComponent<ScrollViewMouseWheel>() || UICamera.HoveredUIObject.GetComponent<UIGridMenuButton>() || UICamera.HoveredUIObject.GetComponent<UIEventListener>() || (UICamera.HoveredUIObject.GetComponent<UITextList>() && UICamera.HoveredUIObject.GetComponent<UITextList>().scrollBar.gameObject.activeSelf)));
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00029D00 File Offset: 0x00027F00
	public static bool IsPressed(GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UICamera.mMouse[i].pressed == go)
			{
				return true;
			}
		}
		int j = 0;
		int count = UICamera.activeTouches.Count;
		while (j < count)
		{
			if (UICamera.activeTouches[j].pressed == go)
			{
				return true;
			}
			j++;
		}
		return UICamera.controller.pressed == go;
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000619 RID: 1561 RVA: 0x00029D75 File Offset: 0x00027F75
	[Obsolete("Use either 'CountInputSources()' or 'activeTouches.Count'")]
	public static int touchCount
	{
		get
		{
			return UICamera.CountInputSources();
		}
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00029D7C File Offset: 0x00027F7C
	public static int CountInputSources()
	{
		int num = 0;
		int i = 0;
		int count = UICamera.activeTouches.Count;
		while (i < count)
		{
			if (UICamera.activeTouches[i].pressed != null)
			{
				num++;
			}
			i++;
		}
		for (int j = 0; j < UICamera.mMouse.Length; j++)
		{
			if (UICamera.mMouse[j].pressed != null)
			{
				num++;
			}
		}
		if (UICamera.controller.pressed != null)
		{
			num++;
		}
		return num;
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x00029E00 File Offset: 0x00028000
	public static int dragCount
	{
		get
		{
			int num = 0;
			int i = 0;
			int count = UICamera.activeTouches.Count;
			while (i < count)
			{
				if (UICamera.activeTouches[i].dragged != null)
				{
					num++;
				}
				i++;
			}
			for (int j = 0; j < UICamera.mMouse.Length; j++)
			{
				if (UICamera.mMouse[j].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.controller.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x0600061C RID: 1564 RVA: 0x00029E84 File Offset: 0x00028084
	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			if (!(eventHandler != null))
			{
				return null;
			}
			return eventHandler.cachedCamera;
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600061D RID: 1565 RVA: 0x00029EA8 File Offset: 0x000280A8
	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uicamera = UICamera.list.buffer[i];
				if (!(uicamera == null) && uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
				{
					return uicamera;
				}
			}
			return null;
		}
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00029EF8 File Offset: 0x000280F8
	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00029F30 File Offset: 0x00028130
	private static Rigidbody FindRootRigidbody(Transform trans)
	{
		while (trans != null && !(trans.GetComponent<UIPanel>() != null))
		{
			Rigidbody component = trans.GetComponent<Rigidbody>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00029F74 File Offset: 0x00028174
	private static Rigidbody2D FindRootRigidbody2D(Transform trans)
	{
		while (trans != null && !(trans.GetComponent<UIPanel>() != null))
		{
			Rigidbody2D component = trans.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00029FB8 File Offset: 0x000281B8
	public static void Raycast(UICamera.MouseOrTouch touch)
	{
		if (!UICamera.Raycast(touch.pos))
		{
			UICamera.mRayHitObject = UICamera.fallThrough;
		}
		if (UICamera.mRayHitObject == null)
		{
			UICamera.mRayHitObject = UICamera.mGenericHandler;
		}
		touch.last = touch.current;
		touch.current = UICamera.mRayHitObject;
		UICamera.mLastPos = touch.pos;
		UICamera.CalculateHoveredUIObject();
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0002A020 File Offset: 0x00028220
	public static bool Raycast(Vector3 inPos)
	{
		if (StandaloneInputModuleV2.Instance.IsPointerOver2DXmlObject())
		{
			return false;
		}
		UICamera.last3DRaycastHit = null;
		UICamera.last2DRaycastHit = null;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			if (uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
			{
				UICamera.currentCamera = uicamera.cachedCamera;
				if (UICamera.currentCamera.targetDisplay == 0)
				{
					Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
					if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y) && vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int layerMask = UICamera.currentCamera.cullingMask & uicamera.eventReceiverMask;
						float num = (uicamera.rangeDistance > 0f) ? uicamera.rangeDistance : (UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane);
						if (uicamera.eventType == UICamera.EventType.World_3D)
						{
							UICamera.lastWorldRay = ray;
							if (Physics.Raycast(ray, out UICamera.lastHit, num, layerMask, QueryTriggerInteraction.Ignore))
							{
								UICamera.lastWorldPosition = UICamera.lastHit.point;
								UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
								UICamera.last3DRaycastHit = UICamera.mRayHitObject;
								if (!uicamera.eventsGoToColliders)
								{
									Rigidbody componentInParent = UICamera.mRayHitObject.gameObject.GetComponentInParent<Rigidbody>();
									if (componentInParent != null)
									{
										UICamera.mRayHitObject = componentInParent.gameObject;
									}
								}
								return true;
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_3D)
						{
							if (UICamera.mRayHits == null)
							{
								UICamera.mRayHits = new RaycastHit[50];
							}
							int num2 = Physics.RaycastNonAlloc(ray, UICamera.mRayHits, num, layerMask, QueryTriggerInteraction.Collide);
							if (num2 > 1)
							{
								int j = 0;
								while (j < num2)
								{
									GameObject gameObject = UICamera.mRayHits[j].collider.gameObject;
									UIWidget component = gameObject.GetComponent<UIWidget>();
									if (component != null)
									{
										if (component.isVisible)
										{
											if (component.hitCheck == null || component.hitCheck(UICamera.mRayHits[j].point))
											{
												goto IL_269;
											}
										}
									}
									else
									{
										UIRect uirect = NGUITools.FindInParents<UIRect>(gameObject);
										if (!(uirect != null) || uirect.finalAlpha >= 0.001f)
										{
											goto IL_269;
										}
									}
									IL_383:
									j++;
									continue;
									IL_269:
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									if (UICamera.mHit.depth == 2147483647)
									{
										goto IL_383;
									}
									UICamera.mHit.hit = UICamera.mRayHits[j];
									UICamera.mHit.point = UICamera.mRayHits[j].point;
									UICamera.mHit.go = UICamera.mRayHits[j].collider.gameObject;
									RaycastHit raycastHit = UICamera.mRayHits[j];
									Transform transform = raycastHit.collider.transform;
									bool flag = Vector3.Dot((transform.position - uicamera.transform.position).normalized, transform.forward) > 0.05f;
									bool flag2 = HoverScript.HoverLockObject && HoverScript.HoverLockObject != transform.root.gameObject && raycastHit.distance > HoverScript.ObjectHit.distance;
									if (flag && !flag2)
									{
										UICamera.mHits.Add(UICamera.mHit);
										goto IL_383;
									}
									goto IL_383;
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										LuaButton component2 = UICamera.mRayHits[k].collider.gameObject.GetComponent<LuaButton>();
										if (!component2 || !component2.luaObject.GetComponent<NetworkPhysicsObject>().IsHidden)
										{
											UICamera.lastHit = UICamera.mHits[k].hit;
											UICamera.mRayHitObject = UICamera.mHits[k].go;
											UICamera.last3DRaycastHit = UICamera.mRayHitObject;
											UICamera.lastWorldRay = ray;
											UICamera.lastWorldPosition = UICamera.mHits[k].point;
											UICamera.mHits.Clear();
											return true;
										}
									}
								}
								UICamera.mHits.Clear();
							}
							else if (num2 == 1)
							{
								GameObject gameObject2 = UICamera.mRayHits[0].collider.gameObject;
								UIWidget component3 = gameObject2.GetComponent<UIWidget>();
								if (component3 != null)
								{
									if (!component3.isVisible)
									{
										goto IL_968;
									}
									if (component3.hitCheck != null && !component3.hitCheck(UICamera.mRayHits[0].point))
									{
										goto IL_968;
									}
								}
								else
								{
									UIRect uirect2 = NGUITools.FindInParents<UIRect>(gameObject2);
									if (uirect2 != null && uirect2.finalAlpha < 0.001f)
									{
										goto IL_968;
									}
								}
								if (UICamera.IsVisible(UICamera.mRayHits[0].point, UICamera.mRayHits[0].collider.gameObject))
								{
									LuaButton component4 = UICamera.mRayHits[0].collider.gameObject.GetComponent<LuaButton>();
									if (!component4 || !component4.luaObject.GetComponent<NetworkPhysicsObject>().IsHidden)
									{
										RaycastHit raycastHit2 = UICamera.mRayHits[0];
										Transform transform2 = raycastHit2.collider.transform;
										bool flag3 = Vector3.Dot((transform2.position - uicamera.transform.position).normalized, transform2.forward) > 0.05f;
										bool flag4 = HoverScript.HoverLockObject && HoverScript.HoverLockObject != transform2.root.gameObject && raycastHit2.distance > HoverScript.ObjectHit.distance;
										if (flag3 && !flag4)
										{
											UICamera.lastHit = UICamera.mRayHits[0];
											UICamera.lastWorldRay = ray;
											UICamera.lastWorldPosition = UICamera.mRayHits[0].point;
											UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
											UICamera.last3DRaycastHit = UICamera.mRayHitObject;
											return true;
										}
									}
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.World_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, out num))
							{
								Vector3 point = ray.GetPoint(num);
								Collider2D collider2D = Physics2D.OverlapPoint(point, layerMask);
								if (collider2D)
								{
									UICamera.lastWorldPosition = point;
									UICamera.mRayHitObject = collider2D.gameObject;
									UICamera.last2DRaycastHit = UICamera.mRayHitObject;
									if (!uicamera.eventsGoToColliders)
									{
										Rigidbody2D rigidbody2D = UICamera.FindRootRigidbody2D(UICamera.mRayHitObject.transform);
										if (rigidbody2D != null)
										{
											UICamera.mRayHitObject = rigidbody2D.gameObject;
										}
									}
									return true;
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_2D && UICamera.m2DPlane.Raycast(ray, out num))
						{
							UICamera.lastWorldPosition = ray.GetPoint(num);
							if (UICamera.mOverlap == null)
							{
								UICamera.mOverlap = new Collider2D[50];
							}
							int num3 = Physics2D.OverlapPointNonAlloc(UICamera.lastWorldPosition, UICamera.mOverlap, layerMask);
							if (num3 > 1)
							{
								int l = 0;
								while (l < num3)
								{
									GameObject gameObject3 = UICamera.mOverlap[l].gameObject;
									UIWidget component5 = gameObject3.GetComponent<UIWidget>();
									if (component5 != null)
									{
										if (component5.isVisible)
										{
											if (component5.hitCheck == null || component5.hitCheck(UICamera.lastWorldPosition))
											{
												goto IL_7DF;
											}
										}
									}
									else
									{
										UIRect uirect3 = NGUITools.FindInParents<UIRect>(gameObject3);
										if (!(uirect3 != null) || uirect3.finalAlpha >= 0.001f)
										{
											goto IL_7DF;
										}
									}
									IL_832:
									l++;
									continue;
									IL_7DF:
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject3);
									if (UICamera.mHit.depth != 2147483647)
									{
										UICamera.mHit.go = gameObject3;
										UICamera.last2DRaycastHit = gameObject3;
										UICamera.mHit.point = UICamera.lastWorldPosition;
										UICamera.mHits.Add(UICamera.mHit);
										goto IL_832;
									}
									goto IL_832;
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int m = 0; m < UICamera.mHits.size; m++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[m]))
									{
										UICamera.mRayHitObject = UICamera.mHits[m].go;
										UICamera.last2DRaycastHit = UICamera.mRayHitObject;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if (num3 == 1)
							{
								GameObject gameObject4 = UICamera.mOverlap[0].gameObject;
								UIWidget component6 = gameObject4.GetComponent<UIWidget>();
								if (component6 != null)
								{
									if (!component6.isVisible)
									{
										goto IL_968;
									}
									if (component6.hitCheck != null && !component6.hitCheck(UICamera.lastWorldPosition))
									{
										goto IL_968;
									}
								}
								else
								{
									UIRect uirect4 = NGUITools.FindInParents<UIRect>(gameObject4);
									if (uirect4 != null && uirect4.finalAlpha < 0.001f)
									{
										goto IL_968;
									}
								}
								if (UICamera.IsVisible(UICamera.lastWorldPosition, gameObject4))
								{
									UICamera.mRayHitObject = gameObject4;
									UICamera.last2DRaycastHit = UICamera.mRayHitObject;
									return true;
								}
							}
						}
					}
				}
			}
			IL_968:;
		}
		return false;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002A9AC File Offset: 0x00028BAC
	private static bool IsVisible(Vector3 worldPoint, GameObject go)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(go);
		if (uipanel == null)
		{
			return false;
		}
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(worldPoint))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002A9E8 File Offset: 0x00028BE8
	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(de.go);
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(de.point))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002AA24 File Offset: 0x00028C24
	public static bool IsHighlighted(GameObject go)
	{
		return UICamera.hoveredObject == go;
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0002AA34 File Offset: 0x00028C34
	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			Camera cachedCamera = uicamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return uicamera;
			}
		}
		return null;
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002AA87 File Offset: 0x00028C87
	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UICamera.GetKeyDown(up))
		{
			UICamera.currentKey = up;
			return 1;
		}
		if (UICamera.GetKeyDown(down))
		{
			UICamera.currentKey = down;
			return -1;
		}
		return 0;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0002AAB4 File Offset: 0x00028CB4
	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UICamera.GetKeyDown(up0))
		{
			UICamera.currentKey = up0;
			return 1;
		}
		if (UICamera.GetKeyDown(up1))
		{
			UICamera.currentKey = up1;
			return 1;
		}
		if (UICamera.GetKeyDown(down0))
		{
			UICamera.currentKey = down0;
			return -1;
		}
		if (UICamera.GetKeyDown(down1))
		{
			UICamera.currentKey = down1;
			return -1;
		}
		return 0;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0002AB18 File Offset: 0x00028D18
	private static int GetDirection(string axis)
	{
		float time = RealTime.time;
		if (UICamera.mNextEvent < time && !string.IsNullOrEmpty(axis))
		{
			float num = UICamera.GetAxis(axis);
			if (num > 0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = time + 0.25f;
				return 1;
			}
			if (num < -0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = time + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0002AB88 File Offset: 0x00028D88
	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (UICamera.mNotifying > 10)
		{
			return;
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UIPopupList.isOpen && UIPopupList.current.source == go && UIPopupList.isOpen)
		{
			go = UIPopupList.current.gameObject;
		}
		if (go && go.activeInHierarchy)
		{
			UICamera.mNotifying++;
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			if (UICamera.mGenericHandler != null && UICamera.mGenericHandler != go)
			{
				UICamera.mGenericHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			}
			UICamera.mNotifying--;
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0002AC2C File Offset: 0x00028E2C
	private void Awake()
	{
		UICamera.mWidth = Screen.width;
		UICamera.mHeight = Screen.height;
		if (Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
		}
		UICamera.mMouse[0].pos = Input.mousePosition;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.mLastPos = UICamera.mMouse[0].pos;
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		if (commandLineArgs != null)
		{
			foreach (string a in commandLineArgs)
			{
				if (a == "-noMouse")
				{
					this.useMouse = false;
				}
				else if (a == "-noTouch")
				{
					this.useTouch = false;
				}
				else if (a == "-noController")
				{
					this.useController = false;
					UICamera.ignoreControllerInput = true;
				}
				else if (a == "-noJoystick")
				{
					this.useController = false;
					UICamera.ignoreControllerInput = true;
				}
				else if (a == "-useMouse")
				{
					this.useMouse = true;
				}
				else if (a == "-useTouch")
				{
					this.useTouch = true;
				}
				else if (a == "-useController")
				{
					this.useController = true;
				}
				else if (a == "-useJoystick")
				{
					this.useController = true;
				}
			}
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0002ADAC File Offset: 0x00028FAC
	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0002ADCF File Offset: 0x00028FCF
	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0002ADE0 File Offset: 0x00028FE0
	private void Start()
	{
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
		if (this.eventType != UICamera.EventType.World_3D && this.cachedCamera.transparencySortMode != TransparencySortMode.Orthographic)
		{
			this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
		}
		if (Application.isPlaying)
		{
			if (UICamera.fallThrough == null)
			{
				UICamera.NGUIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
				UICamera.fallThrough = ((UICamera.NGUIRoot != null) ? UICamera.NGUIRoot.gameObject : base.gameObject);
			}
			this.cachedCamera.eventMask = 0;
			if (!UICamera.ignoreControllerInput && UICamera.disableControllerCheck && this.useController && this.handlesEvents)
			{
				UICamera.disableControllerCheck = false;
				if (!string.IsNullOrEmpty(this.horizontalAxisName) && Mathf.Abs(UICamera.GetAxis(this.horizontalAxisName)) > 0.1f)
				{
					UICamera.ignoreControllerInput = true;
					return;
				}
				if (!string.IsNullOrEmpty(this.verticalAxisName) && Mathf.Abs(UICamera.GetAxis(this.verticalAxisName)) > 0.1f)
				{
					UICamera.ignoreControllerInput = true;
					return;
				}
				if (!string.IsNullOrEmpty(this.horizontalPanAxisName) && Mathf.Abs(UICamera.GetAxis(this.horizontalPanAxisName)) > 0.1f)
				{
					UICamera.ignoreControllerInput = true;
					return;
				}
				if (!string.IsNullOrEmpty(this.verticalPanAxisName) && Mathf.Abs(UICamera.GetAxis(this.verticalPanAxisName)) > 0.1f)
				{
					UICamera.ignoreControllerInput = true;
				}
			}
		}
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0002AF6B File Offset: 0x0002916B
	private void Update()
	{
		if (UICamera.ignoreAllEvents)
		{
			return;
		}
		if (!this.handlesEvents)
		{
			return;
		}
		if (this.processEventsIn == UICamera.ProcessEventsIn.Update)
		{
			this.ProcessEvents();
		}
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0002AF8C File Offset: 0x0002918C
	private void LateUpdate()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		if (this.processEventsIn == UICamera.ProcessEventsIn.LateUpdate)
		{
			this.ProcessEvents();
		}
		int width = Screen.width;
		int height = Screen.height;
		if (width != UICamera.mWidth || height != UICamera.mHeight)
		{
			UICamera.mWidth = width;
			UICamera.mHeight = height;
			UIRoot.Broadcast("UpdateAnchors");
			if (UICamera.onScreenResize != null)
			{
				UICamera.onScreenResize();
			}
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0002AFF4 File Offset: 0x000291F4
	private void ProcessEvents()
	{
		UICamera.current = this;
		NGUIDebug.debugRaycast = this.debug;
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		else if (this.useMouse)
		{
			this.ProcessMouse();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if ((this.useKeyboard || this.useController) && !UICamera.disableController && !UICamera.ignoreControllerInput)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float num = (!string.IsNullOrEmpty(this.scrollAxisName)) ? UICamera.GetAxis(this.scrollAxisName) : 0f;
			if (num != 0f)
			{
				if (UICamera.onScroll != null)
				{
					UICamera.onScroll(UICamera.mHover, num);
				}
				UICamera.Notify(UICamera.mHover, "OnScroll", num);
			}
			if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.showTooltips && UICamera.mTooltipTime != 0f && !UIPopupList.isOpen && UICamera.mMouse[0].dragged == null && (UICamera.mTooltipTime < RealTime.time || UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift)))
			{
				UICamera.currentTouch = UICamera.mMouse[0];
				UICamera.currentTouchID = -1;
				UICamera.ShowTooltip(UICamera.mHover);
			}
		}
		if (UICamera.mTooltip != null && !NGUITools.GetActive(UICamera.mTooltip))
		{
			UICamera.ShowTooltip(null);
		}
		UICamera.current = null;
		UICamera.currentTouchID = -100;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0002B188 File Offset: 0x00029388
	public void ProcessMouse()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 2; i++)
		{
			if (Input.GetMouseButtonDown(i))
			{
				UICamera.currentKey = KeyCode.Mouse0 + i;
				flag2 = true;
				flag = true;
			}
			else if (Input.GetMouseButton(i))
			{
				UICamera.currentKey = KeyCode.Mouse0 + i;
				flag = true;
			}
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Touch && UICamera.activeTouches.Count > 0)
		{
			return;
		}
		UICamera.currentTouch = UICamera.mMouse[0];
		Vector2 vector = Input.mousePosition;
		if (UICamera.currentTouch.ignoreDelta == 0)
		{
			UICamera.currentTouch.delta = vector - UICamera.currentTouch.pos;
		}
		else
		{
			UICamera.currentTouch.ignoreDelta--;
			UICamera.currentTouch.delta.x = 0f;
			UICamera.currentTouch.delta.y = 0f;
		}
		float sqrMagnitude = UICamera.currentTouch.delta.sqrMagnitude;
		UICamera.currentTouch.pos = vector;
		UICamera.mLastPos = vector;
		bool flag3 = false;
		if (UICamera.currentScheme != UICamera.ControlScheme.Mouse)
		{
			if (sqrMagnitude < 0.001f)
			{
				return;
			}
			UICamera.currentKey = KeyCode.Mouse0;
			flag3 = true;
		}
		else if (sqrMagnitude > 0.001f)
		{
			flag3 = true;
		}
		for (int j = 1; j < 3; j++)
		{
			UICamera.mMouse[j].pos = UICamera.currentTouch.pos;
			UICamera.mMouse[j].delta = UICamera.currentTouch.delta;
		}
		if (Input.GetKeyDown(KeyCode.JoystickButton0) && zInput.bController)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			flag2 = true;
			flag = true;
		}
		if (Input.GetKey(KeyCode.JoystickButton0) && zInput.bController)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			flag = true;
		}
		if (flag || flag3 || this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			UICamera.Raycast(UICamera.currentTouch);
			for (int k = 0; k < 3; k++)
			{
				UICamera.mMouse[k].current = UICamera.currentTouch.current;
			}
		}
		bool flag4 = UICamera.currentTouch.last != UICamera.currentTouch.current;
		bool flag5 = UICamera.currentTouch.pressed != null;
		if (!flag5)
		{
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouchID = -1;
		if (flag4)
		{
			UICamera.currentKey = KeyCode.Mouse0;
		}
		if (!flag && flag3 && (!this.stickyTooltip || flag4))
		{
			if (UICamera.mTooltipTime != 0f)
			{
				UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
			}
			else if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
		}
		if (flag3 && UICamera.onMouseMove != null)
		{
			UICamera.onMouseMove(UICamera.currentTouch.delta);
			UICamera.currentTouch = null;
		}
		if (flag4 && (flag2 || (flag5 && !flag)))
		{
			UICamera.hoveredObject = null;
		}
		for (int l = 0; l < 2; l++)
		{
			bool flag6 = Input.GetMouseButtonDown(l);
			bool flag7 = Input.GetMouseButtonUp(l);
			if (l == 0 && zInput.bController)
			{
				flag6 = (flag6 || Input.GetKeyDown(KeyCode.JoystickButton0));
				flag7 = (flag7 || Input.GetKeyUp(KeyCode.JoystickButton0));
			}
			if (flag6 || flag7)
			{
				UICamera.currentKey = KeyCode.Mouse0 + l;
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = KeyCode.Mouse0 + l;
			if (flag6)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(flag6, flag7, l);
		}
		if (!flag && flag4)
		{
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
			UICamera.currentTouchID = -1;
			UICamera.currentKey = KeyCode.Mouse0;
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouch = null;
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0002B5CC File Offset: 0x000297CC
	public void ProcessTouches()
	{
		int num = (UICamera.GetInputTouchCount == null) ? Input.touchCount : UICamera.GetInputTouchCount();
		for (int i = 0; i < num; i++)
		{
			TouchPhase phase;
			int fingerId;
			Vector2 position;
			int tapCount;
			if (UICamera.GetInputTouch == null)
			{
				UnityEngine.Touch touch = Input.GetTouch(i);
				phase = touch.phase;
				fingerId = touch.fingerId;
				position = touch.position;
				tapCount = touch.tapCount;
			}
			else
			{
				UICamera.Touch touch2 = UICamera.GetInputTouch(i);
				phase = touch2.phase;
				fingerId = touch2.fingerId;
				position = touch2.position;
				tapCount = touch2.tapCount;
			}
			UICamera.currentTouchID = (this.allowMultiTouch ? fingerId : 1);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID, true);
			bool flag = phase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
			bool flag2 = phase == TouchPhase.Canceled || phase == TouchPhase.Ended;
			UICamera.currentTouch.delta = position - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = position;
			UICamera.currentKey = KeyCode.None;
			UICamera.Raycast(UICamera.currentTouch);
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (tapCount > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag2, 0);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
		if (num == 0)
		{
			if (UICamera.mUsingTouchEvents)
			{
				UICamera.mUsingTouchEvents = false;
				return;
			}
			if (this.useMouse)
			{
				this.ProcessMouse();
				return;
			}
		}
		else
		{
			UICamera.mUsingTouchEvents = true;
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0002B790 File Offset: 0x00029990
	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown || mouseButtonUp || mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressTime = RealTime.time;
				UICamera.activeTouches.Add(UICamera.currentTouch);
			}
			Vector2 vector = Input.mousePosition;
			UICamera.currentTouch.delta = vector - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = vector;
			UICamera.Raycast(UICamera.currentTouch);
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			UICamera.currentKey = KeyCode.None;
			this.ProcessTouch(mouseButtonDown, mouseButtonUp, 0);
			if (mouseButtonUp)
			{
				UICamera.activeTouches.Remove(UICamera.currentTouch);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0002B89C File Offset: 0x00029A9C
	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.controller;
		bool flag = false;
		bool flag2 = false;
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyDown(KeyCode.KeypadEnter))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag2 = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyUp(KeyCode.KeypadEnter))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		if (flag)
		{
			UICamera.currentTouch.pressTime = RealTime.time;
		}
		if ((flag || flag2) && UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			this.ProcessTouch(flag, flag2, 0);
			UICamera.currentTouch.last = UICamera.currentTouch.current;
		}
		KeyCode keyCode = KeyCode.None;
		if (this.useController && !UICamera.ignoreControllerInput)
		{
			if (!UICamera.disableController && UICamera.currentScheme == UICamera.ControlScheme.Controller && (UICamera.currentTouch.current == null || !UICamera.currentTouch.current.activeInHierarchy))
			{
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			}
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				int direction = UICamera.GetDirection(this.verticalAxisName);
				if (direction != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = ((direction > 0) ? KeyCode.UpArrow : KeyCode.DownArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				int direction2 = UICamera.GetDirection(this.horizontalAxisName);
				if (direction2 != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = ((direction2 > 0) ? KeyCode.RightArrow : KeyCode.LeftArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			float num = (!string.IsNullOrEmpty(this.horizontalPanAxisName)) ? UICamera.GetAxis(this.horizontalPanAxisName) : 0f;
			float num2 = (!string.IsNullOrEmpty(this.verticalPanAxisName)) ? UICamera.GetAxis(this.verticalPanAxisName) : 0f;
			if (num != 0f || num2 != 0f)
			{
				UICamera.ShowTooltip(null);
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
				if (UICamera.currentTouch.current != null)
				{
					Vector2 vector = new Vector2(num, num2);
					vector *= Time.unscaledDeltaTime;
					if (UICamera.onPan != null)
					{
						UICamera.onPan(UICamera.currentTouch.current, vector);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnPan", vector);
				}
			}
		}
		if ((UICamera.GetAnyKeyDown != null) ? UICamera.GetAnyKeyDown() : Input.anyKeyDown)
		{
			int i = 0;
			int num3 = NGUITools.keys.Length;
			while (i < num3)
			{
				KeyCode keyCode2 = NGUITools.keys[i];
				if (keyCode != keyCode2 && UICamera.GetKeyDown(keyCode2) && (this.useKeyboard || keyCode2 >= KeyCode.Mouse0) && ((this.useController && !UICamera.ignoreControllerInput) || keyCode2 < KeyCode.JoystickButton0) && (this.useMouse || keyCode2 < KeyCode.Mouse0 || keyCode2 > KeyCode.Mouse6))
				{
					UICamera.currentKey = keyCode2;
					if (UICamera.onKey != null)
					{
						UICamera.onKey(UICamera.currentTouch.current, keyCode2);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnKey", keyCode2);
				}
				i++;
			}
		}
		UICamera.currentTouch = null;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0002BD5C File Offset: 0x00029F5C
	private void ProcessPress(bool pressed, float click, float drag, int deviceIndex)
	{
		if (pressed)
		{
			if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
			UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
			UICamera.currentTouch.pressStarted = true;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == null && UICamera.currentTouch.current != null)
			{
				UICamera.hoveredObject = UICamera.currentTouch.current;
			}
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, true);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			bool flag = true;
			UIWidget component = UICamera.currentTouch.pressed.GetComponent<UIWidget>();
			if (component && component.RefuseFocus)
			{
				flag = false;
			}
			if (UICamera.mSelected != UICamera.currentTouch.pressed && flag)
			{
				UICamera.mInputFocus = false;
				if (UICamera.mSelected)
				{
					UICamera.Notify(UICamera.mSelected, "OnSelect", false);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, false);
					}
				}
				UICamera.mSelected = UICamera.currentTouch.pressed;
				if (UICamera.currentTouch.pressed != null && UICamera.currentTouch.pressed.GetComponent<UIKeyNavigation>() != null)
				{
					UICamera.controller.current = UICamera.currentTouch.pressed;
				}
				if (UICamera.mSelected)
				{
					UICamera.mInputFocus = (UICamera.mSelected.activeInHierarchy && UICamera.mSelected.GetComponent<UIInput>() != null);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, true);
					}
					UICamera.Notify(UICamera.mSelected, "OnSelect", true);
					return;
				}
			}
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.sqrMagnitude != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float sqrMagnitude = UICamera.currentTouch.totalDelta.sqrMagnitude;
			bool flag2 = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current && !this.useTouch)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.isDragging = true;
				if (UICamera.onDragStart != null)
				{
					UICamera.onDragStart(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				if (UICamera.onDragOver != null)
				{
					UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
				UICamera.isDragging = false;
			}
			else if (!UICamera.currentTouch.dragStarted && drag < sqrMagnitude)
			{
				flag2 = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.mTooltip != null)
				{
					UICamera.ShowTooltip(null);
				}
				UICamera.isDragging = true;
				bool flag3 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag2)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					if (UICamera.onDragOut != null)
					{
						UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				if (UICamera.onDrag != null)
				{
					UICamera.onDrag(UICamera.currentTouch.dragged, UICamera.currentTouch.delta);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag3)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
					return;
				}
				if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && click < sqrMagnitude)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0002C33C File Offset: 0x0002A53C
	private void ProcessRelease(bool isMouse, float drag, bool altClick)
	{
		if (UICamera.currentTouch == null)
		{
			return;
		}
		UICamera.currentTouch.pressStarted = false;
		if (UICamera.currentTouch.pressed != null)
		{
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDragOut != null)
				{
					UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
				if (UICamera.onDragEnd != null)
				{
					UICamera.onDragEnd(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
			}
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (isMouse && this.HasCollider(UICamera.currentTouch.pressed))
			{
				if (UICamera.mHover == UICamera.currentTouch.current)
				{
					if (UICamera.onHover != null)
					{
						UICamera.onHover(UICamera.currentTouch.current, true);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
				}
				else
				{
					UICamera.hoveredObject = UICamera.currentTouch.current;
				}
			}
			if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentScheme != UICamera.ControlScheme.Controller && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.sqrMagnitude < drag))
			{
				if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.pressed == UICamera.currentTouch.current)
				{
					UICamera.ShowTooltip(null);
					float time = RealTime.time;
					if (!altClick)
					{
						if (UICamera.onClick != null)
						{
							UICamera.onClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
					}
					else
					{
						if (UICamera.onAltClick != null)
						{
							UICamera.onAltClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnAltClick", null);
					}
					if (UICamera.currentTouch.pressed.GetComponent<UIButton>() || UICamera.currentTouch.pressed.GetComponent<UIToggle>())
					{
						NetworkSingleton<NetworkUI>.Instance.GetComponent<SoundScript>().PlayGUISound(NetworkSingleton<NetworkUI>.Instance.ButtonSound, 0.3f, 1f);
					}
					if (UICamera.currentTouch.clickTime + 0.35f > time)
					{
						if (UICamera.onDoubleClick != null)
						{
							UICamera.onDoubleClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
					}
					UICamera.currentTouch.clickTime = time;
				}
			}
			else if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDrop != null)
				{
					UICamera.onDrop(UICamera.currentTouch.current, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
			}
		}
		UICamera.currentTouch.dragStarted = false;
		UICamera.currentTouch.pressed = null;
		UICamera.currentTouch.dragged = null;
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0002C690 File Offset: 0x0002A890
	private bool HasCollider(GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		Collider component = go.GetComponent<Collider>();
		if (component != null)
		{
			return component.enabled;
		}
		Collider2D component2 = go.GetComponent<Collider2D>();
		return component2 != null && component2.enabled;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0002C6D8 File Offset: 0x0002A8D8
	public void ProcessTouch(bool pressed, bool released, int deviceIndex = 0)
	{
		if (released)
		{
			UICamera.mTooltipTime = 0f;
		}
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float num = flag ? this.mouseDragThreshold : this.touchDragThreshold;
		float num2 = flag ? this.mouseClickThreshold : this.touchClickThreshold;
		num *= num;
		num2 *= num2;
		if (UICamera.currentTouch.pressed != null)
		{
			if (released)
			{
				this.ProcessRelease(flag, num, deviceIndex > 0 || UICamera.currentTouch.deltaTime > this.tooltipDelay);
			}
			this.ProcessPress(pressed, num2, num, deviceIndex);
			if (this.tooltipDelay != 0f && UICamera.currentTouch.deltaTime > this.tooltipDelay && UICamera.currentTouch.pressed == UICamera.currentTouch.current && UICamera.mTooltipTime != 0f && !UICamera.currentTouch.dragStarted)
			{
				UICamera.mTooltipTime = 0f;
				UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				if (this.longPressTooltip)
				{
					UICamera.ShowTooltip(UICamera.currentTouch.pressed);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnLongPress", null);
				return;
			}
		}
		else if (flag || pressed || released)
		{
			this.ProcessPress(pressed, num2, num, deviceIndex);
			if (released)
			{
				this.ProcessRelease(flag, num, deviceIndex > 0 || UICamera.currentTouch.deltaTime > this.tooltipDelay);
			}
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0002C840 File Offset: 0x0002AA40
	public static void CancelNextTooltip()
	{
		UICamera.mTooltipTime = 0f;
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0002C84C File Offset: 0x0002AA4C
	public static bool ShowTooltip(GameObject go)
	{
		if (UICamera.mTooltip != go)
		{
			if (UICamera.mTooltip != null)
			{
				if (UICamera.onTooltip != null)
				{
					UICamera.onTooltip(UICamera.mTooltip, false);
				}
				UICamera.Notify(UICamera.mTooltip, "OnTooltip", false);
			}
			UICamera.mTooltip = go;
			UICamera.mTooltipTime = 0f;
			if (UICamera.mTooltip != null)
			{
				if (UICamera.onTooltip != null)
				{
					UICamera.onTooltip(UICamera.mTooltip, true);
				}
				UICamera.Notify(UICamera.mTooltip, "OnTooltip", true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0002C8EE File Offset: 0x0002AAEE
	public static bool HideTooltip()
	{
		return UICamera.ShowTooltip(null);
	}

	// Token: 0x04000409 RID: 1033
	public static BetterList<UICamera> list = new BetterList<UICamera>();

	// Token: 0x0400040A RID: 1034
	public static UICamera.GetKeyStateFunc GetKeyDown = (KeyCode key) => (key < KeyCode.JoystickButton0 || !UICamera.ignoreControllerInput) && Input.GetKeyDown(key);

	// Token: 0x0400040B RID: 1035
	public static UICamera.GetKeyStateFunc GetKeyUp = (KeyCode key) => (key < KeyCode.JoystickButton0 || !UICamera.ignoreControllerInput) && Input.GetKeyUp(key);

	// Token: 0x0400040C RID: 1036
	public static UICamera.GetKeyStateFunc GetKey = (KeyCode key) => (key < KeyCode.JoystickButton0 || !UICamera.ignoreControllerInput) && Input.GetKey(key);

	// Token: 0x0400040D RID: 1037
	public static UICamera.GetAxisFunc GetAxis = delegate(string axis)
	{
		if (UICamera.ignoreControllerInput)
		{
			return 0f;
		}
		return Input.GetAxis(axis);
	};

	// Token: 0x0400040E RID: 1038
	public static UICamera.GetAnyKeyFunc GetAnyKeyDown;

	// Token: 0x0400040F RID: 1039
	public static UICamera.GetMouseDelegate GetMouse = (int button) => UICamera.mMouse[button];

	// Token: 0x04000410 RID: 1040
	public static UICamera.GetTouchDelegate GetTouch = delegate(int id, bool createIfMissing)
	{
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		int i = 0;
		int count = UICamera.mTouchIDs.Count;
		while (i < count)
		{
			if (UICamera.mTouchIDs[i] == id)
			{
				return UICamera.activeTouches[i];
			}
			i++;
		}
		if (createIfMissing)
		{
			UICamera.MouseOrTouch mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.pressTime = RealTime.time;
			mouseOrTouch.touchBegan = true;
			UICamera.activeTouches.Add(mouseOrTouch);
			UICamera.mTouchIDs.Add(id);
			return mouseOrTouch;
		}
		return null;
	};

	// Token: 0x04000411 RID: 1041
	public static UICamera.RemoveTouchDelegate RemoveTouch = delegate(int id)
	{
		int i = 0;
		int count = UICamera.mTouchIDs.Count;
		while (i < count)
		{
			if (UICamera.mTouchIDs[i] == id)
			{
				UICamera.mTouchIDs.RemoveAt(i);
				UICamera.activeTouches.RemoveAt(i);
				return;
			}
			i++;
		}
	};

	// Token: 0x04000412 RID: 1042
	public static UICamera.OnScreenResize onScreenResize;

	// Token: 0x04000413 RID: 1043
	public UICamera.EventType eventType = UICamera.EventType.UI_3D;

	// Token: 0x04000414 RID: 1044
	public bool eventsGoToColliders;

	// Token: 0x04000415 RID: 1045
	public LayerMask eventReceiverMask = -1;

	// Token: 0x04000416 RID: 1046
	public UICamera.ProcessEventsIn processEventsIn;

	// Token: 0x04000417 RID: 1047
	public bool debug;

	// Token: 0x04000418 RID: 1048
	public bool useMouse = true;

	// Token: 0x04000419 RID: 1049
	public bool useTouch = true;

	// Token: 0x0400041A RID: 1050
	public bool allowMultiTouch = true;

	// Token: 0x0400041B RID: 1051
	public bool useKeyboard = true;

	// Token: 0x0400041C RID: 1052
	public bool useController = true;

	// Token: 0x0400041D RID: 1053
	public bool stickyTooltip = true;

	// Token: 0x0400041E RID: 1054
	public float tooltipDelay = 1f;

	// Token: 0x0400041F RID: 1055
	public bool longPressTooltip;

	// Token: 0x04000420 RID: 1056
	public float mouseDragThreshold = 4f;

	// Token: 0x04000421 RID: 1057
	public float mouseClickThreshold = 10f;

	// Token: 0x04000422 RID: 1058
	public float touchDragThreshold = 40f;

	// Token: 0x04000423 RID: 1059
	public float touchClickThreshold = 40f;

	// Token: 0x04000424 RID: 1060
	public float rangeDistance = -1f;

	// Token: 0x04000425 RID: 1061
	public string horizontalAxisName = "Horizontal";

	// Token: 0x04000426 RID: 1062
	public string verticalAxisName = "Vertical";

	// Token: 0x04000427 RID: 1063
	public string horizontalPanAxisName;

	// Token: 0x04000428 RID: 1064
	public string verticalPanAxisName;

	// Token: 0x04000429 RID: 1065
	public string scrollAxisName = "Mouse ScrollWheel";

	// Token: 0x0400042A RID: 1066
	[Tooltip("If enabled, command-click will result in a right-click event on OSX")]
	public bool commandClick = true;

	// Token: 0x0400042B RID: 1067
	public KeyCode submitKey0 = KeyCode.Return;

	// Token: 0x0400042C RID: 1068
	public KeyCode submitKey1 = KeyCode.JoystickButton0;

	// Token: 0x0400042D RID: 1069
	public KeyCode cancelKey0 = KeyCode.Escape;

	// Token: 0x0400042E RID: 1070
	public KeyCode cancelKey1 = KeyCode.JoystickButton1;

	// Token: 0x0400042F RID: 1071
	public bool autoHideCursor = true;

	// Token: 0x04000430 RID: 1072
	public static UICamera.OnCustomInput onCustomInput;

	// Token: 0x04000431 RID: 1073
	public static bool showTooltips = true;

	// Token: 0x04000432 RID: 1074
	private static UIRoot NGUIRoot;

	// Token: 0x04000433 RID: 1075
	public static bool ignoreAllEvents = false;

	// Token: 0x04000434 RID: 1076
	public static bool ignoreControllerInput = false;

	// Token: 0x04000435 RID: 1077
	private static bool mDisableController = false;

	// Token: 0x04000436 RID: 1078
	private static Vector2 mLastPos = Vector2.zero;

	// Token: 0x04000437 RID: 1079
	public static Vector3 lastWorldPosition = Vector3.zero;

	// Token: 0x04000438 RID: 1080
	public static Ray lastWorldRay = default(Ray);

	// Token: 0x04000439 RID: 1081
	public static RaycastHit lastHit;

	// Token: 0x0400043A RID: 1082
	public static UICamera current = null;

	// Token: 0x0400043B RID: 1083
	public static Camera currentCamera = null;

	// Token: 0x0400043C RID: 1084
	public static UICamera.OnSchemeChange onSchemeChange;

	// Token: 0x0400043D RID: 1085
	private static UICamera.ControlScheme mLastScheme = UICamera.ControlScheme.Mouse;

	// Token: 0x0400043E RID: 1086
	public static int currentTouchID = -100;

	// Token: 0x0400043F RID: 1087
	private static KeyCode mCurrentKey = KeyCode.Alpha0;

	// Token: 0x04000440 RID: 1088
	public static UICamera.MouseOrTouch currentTouch = null;

	// Token: 0x04000441 RID: 1089
	private static bool mInputFocus = false;

	// Token: 0x04000442 RID: 1090
	private static GameObject mGenericHandler;

	// Token: 0x04000443 RID: 1091
	public static GameObject fallThrough;

	// Token: 0x04000444 RID: 1092
	public static UICamera.VoidDelegate onClick;

	// Token: 0x04000445 RID: 1093
	public static UICamera.VoidDelegate onAltClick;

	// Token: 0x04000446 RID: 1094
	public static UICamera.VoidDelegate onDoubleClick;

	// Token: 0x04000447 RID: 1095
	public static UICamera.BoolDelegate onHover;

	// Token: 0x04000448 RID: 1096
	public static UICamera.BoolDelegate onPress;

	// Token: 0x04000449 RID: 1097
	public static UICamera.BoolDelegate onSelect;

	// Token: 0x0400044A RID: 1098
	public static UICamera.FloatDelegate onScroll;

	// Token: 0x0400044B RID: 1099
	public static UICamera.VectorDelegate onDrag;

	// Token: 0x0400044C RID: 1100
	public static UICamera.VoidDelegate onDragStart;

	// Token: 0x0400044D RID: 1101
	public static UICamera.ObjectDelegate onDragOver;

	// Token: 0x0400044E RID: 1102
	public static UICamera.ObjectDelegate onDragOut;

	// Token: 0x0400044F RID: 1103
	public static UICamera.VoidDelegate onDragEnd;

	// Token: 0x04000450 RID: 1104
	public static UICamera.ObjectDelegate onDrop;

	// Token: 0x04000451 RID: 1105
	public static UICamera.KeyCodeDelegate onKey;

	// Token: 0x04000452 RID: 1106
	public static UICamera.KeyCodeDelegate onNavigate;

	// Token: 0x04000453 RID: 1107
	public static UICamera.VectorDelegate onPan;

	// Token: 0x04000454 RID: 1108
	public static UICamera.BoolDelegate onTooltip;

	// Token: 0x04000455 RID: 1109
	public static UICamera.MoveDelegate onMouseMove;

	// Token: 0x04000456 RID: 1110
	private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[]
	{
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch()
	};

	// Token: 0x04000457 RID: 1111
	public static UICamera.MouseOrTouch controller = new UICamera.MouseOrTouch();

	// Token: 0x04000458 RID: 1112
	public static List<UICamera.MouseOrTouch> activeTouches = new List<UICamera.MouseOrTouch>();

	// Token: 0x04000459 RID: 1113
	private static List<int> mTouchIDs = new List<int>();

	// Token: 0x0400045A RID: 1114
	private static int mWidth = 0;

	// Token: 0x0400045B RID: 1115
	private static int mHeight = 0;

	// Token: 0x0400045C RID: 1116
	private static GameObject mTooltip = null;

	// Token: 0x0400045D RID: 1117
	private Camera mCam;

	// Token: 0x0400045E RID: 1118
	private static float mTooltipTime = 0f;

	// Token: 0x0400045F RID: 1119
	private float mNextRaycast;

	// Token: 0x04000460 RID: 1120
	public static bool isDragging = false;

	// Token: 0x04000461 RID: 1121
	private static int mLastInteractionCheck = -1;

	// Token: 0x04000462 RID: 1122
	private static bool mLastInteractionResult = false;

	// Token: 0x04000463 RID: 1123
	private static int mLastFocusCheck = -1;

	// Token: 0x04000464 RID: 1124
	private static bool mLastFocusResult = false;

	// Token: 0x04000465 RID: 1125
	private static int mLastOverCheck = -1;

	// Token: 0x04000466 RID: 1126
	private static bool mLastOverResult = false;

	// Token: 0x04000467 RID: 1127
	private static GameObject mRayHitObject;

	// Token: 0x04000468 RID: 1128
	private static GameObject mHover;

	// Token: 0x04000469 RID: 1129
	private static GameObject mSelected;

	// Token: 0x0400046A RID: 1130
	private static GameObject last2DRaycastHit;

	// Token: 0x0400046B RID: 1131
	private static GameObject last3DRaycastHit;

	// Token: 0x0400046C RID: 1132
	public static GameObject HoveredUIObject;

	// Token: 0x0400046D RID: 1133
	public static bool changeSelection = true;

	// Token: 0x0400046E RID: 1134
	private static UICamera.DepthEntry mHit = default(UICamera.DepthEntry);

	// Token: 0x0400046F RID: 1135
	private static BetterList<UICamera.DepthEntry> mHits = new BetterList<UICamera.DepthEntry>();

	// Token: 0x04000470 RID: 1136
	private static RaycastHit[] mRayHits;

	// Token: 0x04000471 RID: 1137
	private static Collider2D[] mOverlap;

	// Token: 0x04000472 RID: 1138
	private static Plane m2DPlane = new Plane(Vector3.back, 0f);

	// Token: 0x04000473 RID: 1139
	private static float mNextEvent = 0f;

	// Token: 0x04000474 RID: 1140
	private static int mNotifying = 0;

	// Token: 0x04000475 RID: 1141
	private static bool disableControllerCheck = true;

	// Token: 0x04000476 RID: 1142
	private static bool mUsingTouchEvents = true;

	// Token: 0x04000477 RID: 1143
	public static UICamera.GetTouchCountCallback GetInputTouchCount;

	// Token: 0x04000478 RID: 1144
	public static UICamera.GetTouchCallback GetInputTouch;

	// Token: 0x02000550 RID: 1360
	public enum ControlScheme
	{
		// Token: 0x0400248B RID: 9355
		Mouse,
		// Token: 0x0400248C RID: 9356
		Touch,
		// Token: 0x0400248D RID: 9357
		Controller
	}

	// Token: 0x02000551 RID: 1361
	public enum ClickNotification
	{
		// Token: 0x0400248F RID: 9359
		None,
		// Token: 0x04002490 RID: 9360
		Always,
		// Token: 0x04002491 RID: 9361
		BasedOnDelta
	}

	// Token: 0x02000552 RID: 1362
	public class MouseOrTouch
	{
		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x0016B57F File Offset: 0x0016977F
		public float deltaTime
		{
			get
			{
				return RealTime.time - this.pressTime;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x0016B58D File Offset: 0x0016978D
		public bool isOverUI
		{
			get
			{
				return this.current != null && this.current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(this.current) != null;
			}
		}

		// Token: 0x04002492 RID: 9362
		public KeyCode key;

		// Token: 0x04002493 RID: 9363
		public Vector2 pos;

		// Token: 0x04002494 RID: 9364
		public Vector2 lastPos;

		// Token: 0x04002495 RID: 9365
		public Vector2 delta;

		// Token: 0x04002496 RID: 9366
		public Vector2 totalDelta;

		// Token: 0x04002497 RID: 9367
		public Camera pressedCam;

		// Token: 0x04002498 RID: 9368
		public GameObject last;

		// Token: 0x04002499 RID: 9369
		public GameObject current;

		// Token: 0x0400249A RID: 9370
		public GameObject pressed;

		// Token: 0x0400249B RID: 9371
		public GameObject dragged;

		// Token: 0x0400249C RID: 9372
		public float pressTime;

		// Token: 0x0400249D RID: 9373
		public float clickTime;

		// Token: 0x0400249E RID: 9374
		public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;

		// Token: 0x0400249F RID: 9375
		public bool touchBegan = true;

		// Token: 0x040024A0 RID: 9376
		public bool pressStarted;

		// Token: 0x040024A1 RID: 9377
		public bool dragStarted;

		// Token: 0x040024A2 RID: 9378
		public int ignoreDelta;
	}

	// Token: 0x02000553 RID: 1363
	public enum EventType
	{
		// Token: 0x040024A4 RID: 9380
		World_3D,
		// Token: 0x040024A5 RID: 9381
		UI_3D,
		// Token: 0x040024A6 RID: 9382
		World_2D,
		// Token: 0x040024A7 RID: 9383
		UI_2D
	}

	// Token: 0x02000554 RID: 1364
	// (Invoke) Token: 0x060037E3 RID: 14307
	public delegate bool GetKeyStateFunc(KeyCode key);

	// Token: 0x02000555 RID: 1365
	// (Invoke) Token: 0x060037E7 RID: 14311
	public delegate float GetAxisFunc(string name);

	// Token: 0x02000556 RID: 1366
	// (Invoke) Token: 0x060037EB RID: 14315
	public delegate bool GetAnyKeyFunc();

	// Token: 0x02000557 RID: 1367
	// (Invoke) Token: 0x060037EF RID: 14319
	public delegate UICamera.MouseOrTouch GetMouseDelegate(int button);

	// Token: 0x02000558 RID: 1368
	// (Invoke) Token: 0x060037F3 RID: 14323
	public delegate UICamera.MouseOrTouch GetTouchDelegate(int id, bool createIfMissing);

	// Token: 0x02000559 RID: 1369
	// (Invoke) Token: 0x060037F7 RID: 14327
	public delegate void RemoveTouchDelegate(int id);

	// Token: 0x0200055A RID: 1370
	// (Invoke) Token: 0x060037FB RID: 14331
	public delegate void OnScreenResize();

	// Token: 0x0200055B RID: 1371
	public enum ProcessEventsIn
	{
		// Token: 0x040024A9 RID: 9385
		Update,
		// Token: 0x040024AA RID: 9386
		LateUpdate
	}

	// Token: 0x0200055C RID: 1372
	// (Invoke) Token: 0x060037FF RID: 14335
	public delegate void OnCustomInput();

	// Token: 0x0200055D RID: 1373
	// (Invoke) Token: 0x06003803 RID: 14339
	public delegate void OnSchemeChange();

	// Token: 0x0200055E RID: 1374
	// (Invoke) Token: 0x06003807 RID: 14343
	public delegate void MoveDelegate(Vector2 delta);

	// Token: 0x0200055F RID: 1375
	// (Invoke) Token: 0x0600380B RID: 14347
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x02000560 RID: 1376
	// (Invoke) Token: 0x0600380F RID: 14351
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x02000561 RID: 1377
	// (Invoke) Token: 0x06003813 RID: 14355
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x02000562 RID: 1378
	// (Invoke) Token: 0x06003817 RID: 14359
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x02000563 RID: 1379
	// (Invoke) Token: 0x0600381B RID: 14363
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x02000564 RID: 1380
	// (Invoke) Token: 0x0600381F RID: 14367
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	// Token: 0x02000565 RID: 1381
	private struct DepthEntry
	{
		// Token: 0x040024AB RID: 9387
		public int depth;

		// Token: 0x040024AC RID: 9388
		public RaycastHit hit;

		// Token: 0x040024AD RID: 9389
		public Vector3 point;

		// Token: 0x040024AE RID: 9390
		public GameObject go;
	}

	// Token: 0x02000566 RID: 1382
	public class Touch
	{
		// Token: 0x040024AF RID: 9391
		public int fingerId;

		// Token: 0x040024B0 RID: 9392
		public TouchPhase phase;

		// Token: 0x040024B1 RID: 9393
		public Vector2 position;

		// Token: 0x040024B2 RID: 9394
		public int tapCount;
	}

	// Token: 0x02000567 RID: 1383
	// (Invoke) Token: 0x06003824 RID: 14372
	public delegate int GetTouchCountCallback();

	// Token: 0x02000568 RID: 1384
	// (Invoke) Token: 0x06003828 RID: 14376
	public delegate UICamera.Touch GetTouchCallback(int index);
}
