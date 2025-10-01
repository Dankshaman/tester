using System;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000394 RID: 916
	[AddComponentMenu("Event/Standalone Input Module Improved")]
	public class StandaloneInputModuleImproved : PointerInputModule
	{
		// Token: 0x06002AE2 RID: 10978 RVA: 0x0013003C File Offset: 0x0012E23C
		protected StandaloneInputModuleImproved()
		{
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public StandaloneInputModuleImproved.InputMode inputMode
		{
			get
			{
				return StandaloneInputModuleImproved.InputMode.Mouse;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x00130091 File Offset: 0x0012E291
		// (set) Token: 0x06002AE5 RID: 10981 RVA: 0x00130099 File Offset: 0x0012E299
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x00130091 File Offset: 0x0012E291
		// (set) Token: 0x06002AE7 RID: 10983 RVA: 0x00130099 File Offset: 0x0012E299
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x001300A2 File Offset: 0x0012E2A2
		// (set) Token: 0x06002AE9 RID: 10985 RVA: 0x001300AA File Offset: 0x0012E2AA
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06002AEA RID: 10986 RVA: 0x001300B3 File Offset: 0x0012E2B3
		// (set) Token: 0x06002AEB RID: 10987 RVA: 0x001300BB File Offset: 0x0012E2BB
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06002AEC RID: 10988 RVA: 0x001300C4 File Offset: 0x0012E2C4
		// (set) Token: 0x06002AED RID: 10989 RVA: 0x001300CC File Offset: 0x0012E2CC
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06002AEE RID: 10990 RVA: 0x001300D5 File Offset: 0x0012E2D5
		// (set) Token: 0x06002AEF RID: 10991 RVA: 0x001300DD File Offset: 0x0012E2DD
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x001300E6 File Offset: 0x0012E2E6
		// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x001300EE File Offset: 0x0012E2EE
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x001300F7 File Offset: 0x0012E2F7
		// (set) Token: 0x06002AF3 RID: 10995 RVA: 0x001300FF File Offset: 0x0012E2FF
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x00130108 File Offset: 0x0012E308
		public override void UpdateModule()
		{
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_MousePosition = base.input.mousePosition;
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x00130127 File Offset: 0x0012E327
		public override bool IsModuleSupported()
		{
			return this.m_ForceModuleActive || base.input.mousePresent || base.input.touchSupported;
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x0013014C File Offset: 0x0012E34C
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			bool flag = this.m_ForceModuleActive;
			flag |= base.input.GetButtonDown(this.m_SubmitButton);
			flag |= base.input.GetButtonDown(this.m_CancelButton);
			flag |= !Mathf.Approximately(base.input.GetAxisRaw(this.m_HorizontalAxis), 0f);
			flag |= !Mathf.Approximately(base.input.GetAxisRaw(this.m_VerticalAxis), 0f);
			flag |= ((this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f);
			flag |= base.input.GetMouseButtonDown(0);
			if (base.input.touchCount > 0)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x00130218 File Offset: 0x0012E418
		public override void ActivateModule()
		{
			base.ActivateModule();
			this.m_MousePosition = base.input.mousePosition;
			this.m_LastMousePosition = base.input.mousePosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x00130280 File Offset: 0x0012E480
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x00130290 File Offset: 0x0012E490
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			if (!this.ProcessTouchEvents() && base.input.mousePresent)
			{
				this.ProcessMouseEvent();
			}
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x001302E4 File Offset: 0x0012E4E4
		private bool ProcessTouchEvents()
		{
			for (int i = 0; i < base.input.touchCount; i++)
			{
				Touch touch = base.input.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool pressed;
					bool flag;
					PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, out pressed, out flag);
					this.ProcessTouchPress(touchPointerEventData, pressed, flag);
					if (!flag)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return base.input.touchCount > 0;
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x00130360 File Offset: 0x0012E560
		protected void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					if (unscaledTime - pointerEvent.clickTime < 0.3f)
					{
						int clickCount = pointerEvent.clickCount + 1;
						pointerEvent.clickCount = clickCount;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x00130584 File Offset: 0x0012E784
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (base.input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (base.input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x00130608 File Offset: 0x0012E808
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = base.input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = base.input.GetAxisRaw(this.m_VerticalAxis);
			if (base.input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (base.input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x001306D8 File Offset: 0x0012E8D8
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = base.input.GetButtonDown(this.m_HorizontalAxis) || base.input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			if (axisEventData.moveDir != MoveDirection.None)
			{
				ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				if (!flag2)
				{
					this.m_ConsecutiveMoveCount = 0;
				}
				this.m_ConsecutiveMoveCount++;
				this.m_PrevActionTime = unscaledTime;
				this.m_LastMoveVector = rawMoveVector;
			}
			else
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			return axisEventData.used;
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x0013080A File Offset: 0x0012EA0A
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		[Obsolete("This method is no longer checked, overriding it with return true does nothing!")]
		protected virtual bool ForceAutoSelect()
		{
			return false;
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x00130814 File Offset: 0x0012EA14
		protected void ProcessMouseEvent(int id)
		{
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(id);
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.m_CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x00130908 File Offset: 0x0012EB08
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x00130950 File Offset: 0x0012EB50
		protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						PointerEventData pointerEventData = buttonData;
						int clickCount = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = clickCount;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x00130B4A File Offset: 0x0012ED4A
		protected GameObject GetCurrentFocusedGameObject()
		{
			return this.m_CurrentFocusedGameObject;
		}

		// Token: 0x04001D1D RID: 7453
		private float m_PrevActionTime;

		// Token: 0x04001D1E RID: 7454
		private Vector2 m_LastMoveVector;

		// Token: 0x04001D1F RID: 7455
		private int m_ConsecutiveMoveCount;

		// Token: 0x04001D20 RID: 7456
		private Vector2 m_LastMousePosition;

		// Token: 0x04001D21 RID: 7457
		private Vector2 m_MousePosition;

		// Token: 0x04001D22 RID: 7458
		private GameObject m_CurrentFocusedGameObject;

		// Token: 0x04001D23 RID: 7459
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04001D24 RID: 7460
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04001D25 RID: 7461
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04001D26 RID: 7462
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04001D27 RID: 7463
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04001D28 RID: 7464
		[SerializeField]
		private float m_RepeatDelay = 0.5f;

		// Token: 0x04001D29 RID: 7465
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		private bool m_ForceModuleActive;

		// Token: 0x020007C5 RID: 1989
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public enum InputMode
		{
			// Token: 0x04002D59 RID: 11609
			Mouse,
			// Token: 0x04002D5A RID: 11610
			Buttons
		}
	}
}
