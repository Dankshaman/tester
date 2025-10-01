using System;
using System.Collections.Generic;
using TouchScript;
using TouchScript.InputSources;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class TouchScriptNGUI : Singleton<TouchScriptNGUI>
{
	// Token: 0x06002018 RID: 8216 RVA: 0x000E5E48 File Offset: 0x000E4048
	public void EnableTouch(bool value)
	{
		base.GetComponent<TTSStandardInput>().enabled = value;
		TouchHandlerScript[] array = UnityEngine.Object.FindObjectsOfType<TouchHandlerScript>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = value;
		}
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000E5E80 File Offset: 0x000E4080
	private void Start()
	{
		TouchScriptNGUI.DropIds.Clear();
		TouchScriptNGUI.uiCamera = UICamera.mainCamera.GetComponent<UICamera>();
		TouchManager.Instance.TouchesBegan += this.TouchManagerBegan;
		TouchManager.Instance.TouchesMoved += this.TouchManagerMoved;
		TouchManager.Instance.TouchesEnded += this.TouchManagerEnded;
		TouchManager.Instance.TouchesCancelled += this.TouchManagerCancelled;
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000E5F00 File Offset: 0x000E4100
	private void OnDestroy()
	{
		if (TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan -= this.TouchManagerBegan;
			TouchManager.Instance.TouchesMoved -= this.TouchManagerMoved;
			TouchManager.Instance.TouchesEnded -= this.TouchManagerEnded;
			TouchManager.Instance.TouchesCancelled -= this.TouchManagerCancelled;
		}
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000E5F6C File Offset: 0x000E416C
	private void TouchManagerBegan(object sender, TouchEventArgs eventArgs)
	{
		zInput.bTouching = true;
		for (int i = 0; i < eventArgs.Touches.Count; i++)
		{
			TouchScriptUpdater.BeginTouchId = eventArgs.Touches[i].Id;
		}
		this.TouchManagerChanged(sender, eventArgs, TouchPhase.Began);
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000E5FB4 File Offset: 0x000E41B4
	private void TouchManagerMoved(object sender, TouchEventArgs eventArgs)
	{
		this.TouchManagerChanged(sender, eventArgs, TouchPhase.Moved);
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000E5FC0 File Offset: 0x000E41C0
	private void TouchManagerEnded(object sender, TouchEventArgs eventArgs)
	{
		for (int i = 0; i < eventArgs.Touches.Count; i++)
		{
			TouchScriptUpdater.EndTouchId = eventArgs.Touches[i].Id;
			if (TouchScriptNGUI.DropIds.Contains(TouchScriptUpdater.EndTouchId))
			{
				TouchScriptNGUI.DropIds.Remove(TouchScriptUpdater.EndTouchId);
				if (PlayerScript.Pointer)
				{
					PlayerScript.PointerScript.Release(PlayerScript.PointerScript.ID, -1, TouchScriptUpdater.EndTouchId, -1);
				}
			}
		}
		this.TouchManagerChanged(sender, eventArgs, TouchPhase.Ended);
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000E604A File Offset: 0x000E424A
	private void TouchManagerCancelled(object sender, TouchEventArgs eventArgs)
	{
		this.TouchManagerChanged(sender, eventArgs, TouchPhase.Canceled);
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000E6058 File Offset: 0x000E4258
	private void TouchManagerChanged(object sender, TouchEventArgs eventArgs, TouchPhase touchPhase)
	{
		foreach (TouchPoint touchPoint in eventArgs.Touches)
		{
			UICamera.currentTouchID = (TouchScriptNGUI.uiCamera.allowMultiTouch ? touchPoint.Id : 1);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID, true);
			bool flag = touchPhase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
			bool flag2 = touchPhase == TouchPhase.Canceled || touchPhase == TouchPhase.Ended;
			UICamera.currentTouch.touchBegan = false;
			if (flag)
			{
				UICamera.currentTouch.delta = Vector2.zero;
			}
			else
			{
				UICamera.currentTouch.delta = touchPoint.Position - touchPoint.PreviousPosition;
			}
			UICamera.currentTouch.pos = touchPoint.Position;
			bool flag3 = UICamera.Raycast(UICamera.currentTouch.pos);
			UICamera.currentTouch.current = (flag3 ? UICamera.HoveredUIObject : UICamera.fallThrough);
			UICamera.hoveredObject = (flag3 ? UICamera.HoveredUIObject : UICamera.fallThrough);
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.genericEventHandler;
			}
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastEventPosition = UICamera.currentTouch.pos;
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			TouchScriptNGUI.uiCamera.ProcessTouch(flag, flag2, 0);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch = null;
			if (!TouchScriptNGUI.uiCamera.allowMultiTouch)
			{
				break;
			}
		}
	}

	// Token: 0x040013A4 RID: 5028
	public static UICamera uiCamera;

	// Token: 0x040013A5 RID: 5029
	public static List<int> DropIds = new List<int>();
}
