using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000386 RID: 902
public class StandaloneInputModuleV2 : StandaloneInputModule
{
	// Token: 0x06002A73 RID: 10867 RVA: 0x0012E6F2 File Offset: 0x0012C8F2
	protected override void Awake()
	{
		base.Awake();
		StandaloneInputModuleV2.Instance = this;
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x0012E700 File Offset: 0x0012C900
	public GameObject GameObjectUnderPointer(int pointerId)
	{
		PointerEventData lastPointerEventData = base.GetLastPointerEventData(pointerId);
		if (lastPointerEventData != null)
		{
			return lastPointerEventData.pointerCurrentRaycast.gameObject;
		}
		return null;
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x0012E728 File Offset: 0x0012C928
	public GameObject GameObjectUnderPointer()
	{
		return this.GameObjectUnderPointer(-1);
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x0012E731 File Offset: 0x0012C931
	public bool IsPointerOverGameObject()
	{
		return this.IsPointerOverGameObject(-1);
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x0012E73C File Offset: 0x0012C93C
	public override bool IsPointerOverGameObject(int id)
	{
		if (base.IsPointerOverGameObject(id))
		{
			PointerEventData pointerEventData;
			if (this.m_PointerData.TryGetValue(id, out pointerEventData))
			{
				if (pointerEventData.pointerCurrentRaycast.gameObject.layer == 5)
				{
					return true;
				}
				if (HoverScript.GrabbedStackObject || (HoverScript.HoverObject && pointerEventData.pointerCurrentRaycast.distance > HoverScript.HoverObjectDistance))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x0012E7A9 File Offset: 0x0012C9A9
	public bool IsPointerOver2DXmlObject()
	{
		return this.IsPointerOver2DXmlObject(-1);
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x0012E7B4 File Offset: 0x0012C9B4
	public bool IsPointerOver2DXmlObject(int id)
	{
		PointerEventData pointerEventData;
		return base.IsPointerOverGameObject(id) && this.m_PointerData.TryGetValue(id, out pointerEventData) && pointerEventData.pointerCurrentRaycast.gameObject.layer == 5;
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x0012E7F4 File Offset: 0x0012C9F4
	public override void Process()
	{
		PointerEventData pointerEventData;
		if (this.m_PointerData.TryGetValue(-1, out pointerEventData))
		{
			if (HoverScript.HoverObject && pointerEventData.pointerCurrentRaycast.distance > HoverScript.HoverObjectDistance && pointerEventData.pointerCurrentRaycast.gameObject.layer != 5)
			{
				return;
			}
			if (UICamera.HoveredUIObject && pointerEventData.pointerCurrentRaycast.gameObject && UICamera.HoveredUIObject.layer != pointerEventData.pointerCurrentRaycast.gameObject.layer)
			{
				return;
			}
		}
		bool flag = base.SendUpdateEventToSelectedObject();
		if (base.eventSystem.sendNavigationEvents)
		{
			if (!flag)
			{
				flag |= base.SendMoveEventToSelectedObject();
			}
			if (!flag)
			{
				base.SendSubmitEventToSelectedObject();
			}
		}
		if (!this.ProcessTouchEvents() && base.input.mousePresent)
		{
			base.ProcessMouseEvent();
		}
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x0012E8CC File Offset: 0x0012CACC
	public virtual bool ProcessVirtualTouchEvent(StandaloneInputModuleV2.VirtualTouch virtualTouch)
	{
		Touch touch = virtualTouch.touch;
		if (touch.type == TouchType.Indirect)
		{
			return false;
		}
		bool pressed;
		bool flag;
		PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, out pressed, out flag);
		if (virtualTouch.forceNoPress)
		{
			pressed = false;
			flag = false;
		}
		base.ProcessTouchPress(touchPointerEventData, pressed, flag);
		if (!flag)
		{
			this.ProcessMove(touchPointerEventData);
			this.ProcessDrag(touchPointerEventData);
		}
		else
		{
			base.RemovePointerData(touchPointerEventData);
		}
		return this.IsPointerOverGameObject(touch.fingerId);
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x0012E934 File Offset: 0x0012CB34
	protected virtual bool ProcessTouchEvents()
	{
		for (int i = 0; i < base.input.touchCount; i++)
		{
			Touch touch = base.input.GetTouch(i);
			if (touch.type != TouchType.Indirect)
			{
				bool pressed;
				bool flag;
				PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, out pressed, out flag);
				base.ProcessTouchPress(touchPointerEventData, pressed, flag);
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

	// Token: 0x04001CD8 RID: 7384
	public static StandaloneInputModuleV2 Instance;

	// Token: 0x020007C1 RID: 1985
	public class VirtualTouch
	{
		// Token: 0x04002D50 RID: 11600
		public bool forceNoPress;

		// Token: 0x04002D51 RID: 11601
		public Touch touch;
	}
}
