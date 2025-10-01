using System;
using System.Collections.Generic;
using NewNet;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Gestures.Base;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class TouchHandlerScript : MonoBehaviour
{
	// Token: 0x06002001 RID: 8193 RVA: 0x000E4C8C File Offset: 0x000E2E8C
	private void Init()
	{
		if (this.inited)
		{
			return;
		}
		this.inited = true;
		this.tapGesture = base.gameObject.AddComponent<TapGesture>();
		this.doubleTapGesture = base.gameObject.AddComponent<TapGesture>();
		this.pressGesture = base.gameObject.AddComponent<PressGesture>();
		this.releaseGesture = base.gameObject.AddComponent<ReleaseGesture>();
		this.longPressGesture = base.gameObject.AddComponent<LongPressGesture>();
		this.panGesture = base.gameObject.AddComponent<TransformGesture>();
		this.panGesture.Type = TransformGestureBase.TransformType.Translation;
		this.panTwoFingerGesture = base.gameObject.AddComponent<TransformGesture>();
		this.panTwoFingerGesture.Type = TransformGestureBase.TransformType.Translation;
		this.scaleGesture = base.gameObject.AddComponent<TransformGesture>();
		this.scaleGesture.Type = TransformGestureBase.TransformType.Scaling;
		this.tapGesture.RequireGestureToFail = this.doubleTapGesture;
		this.doubleTapGesture.NumberOfTapsRequired = 2;
		this.doubleTapGesture.TimeLimit = 0.4f;
		this.longPressGesture.TimeToPress = 0.4f;
		this.longPressGesture.DistanceLimit = 0.5f;
		this.panGesture.AddFriendlyGesture(this.longPressGesture);
		this.panTwoFingerGesture.MinTouches = 2;
		this.panTwoFingerGesture.AddFriendlyGesture(this.panGesture);
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		if (TouchHandlerScript.PixelMovementThreshold == 0f)
		{
			TouchHandlerScript.PixelMovementThreshold = TouchManager.Instance.DotsPerCentimeter / 7f;
		}
		if (!TouchHandlerScript.cameraController)
		{
			TouchHandlerScript.cameraController = Singleton<CameraController>.Instance;
		}
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000E4E14 File Offset: 0x000E3014
	private void OnEnable()
	{
		this.Init();
		this.tapGesture.Tapped += this.Tapped;
		this.doubleTapGesture.Tapped += this.DoubleTapped;
		this.pressGesture.Pressed += this.Pressed;
		this.releaseGesture.Released += this.Released;
		this.longPressGesture.LongPressed += this.LongPressed;
		this.panGesture.TransformStarted += this.PanStarted;
		this.panGesture.Transformed += this.Panned;
		this.panGesture.TransformCompleted += this.PanCompleted;
		this.panTwoFingerGesture.TransformStarted += this.PanStartedTwoFinger;
		this.panTwoFingerGesture.Transformed += this.PannedTwoFinger;
		this.panTwoFingerGesture.TransformCompleted += this.PanCompletedTwoFinger;
		this.scaleGesture.TransformStarted += this.ScaleStarted;
		this.scaleGesture.Transformed += this.Scaled;
		this.scaleGesture.TransformCompleted += this.ScaleCompleted;
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x000E4F6C File Offset: 0x000E316C
	private void OnDisable()
	{
		this.tapGesture.Tapped -= this.Tapped;
		this.doubleTapGesture.Tapped -= this.DoubleTapped;
		this.pressGesture.Pressed -= this.Pressed;
		this.releaseGesture.Released -= this.Released;
		this.longPressGesture.LongPressed -= this.LongPressed;
		this.panGesture.TransformStarted -= this.PanStarted;
		this.panGesture.Transformed -= this.Panned;
		this.panGesture.TransformCompleted -= this.PanCompleted;
		this.panTwoFingerGesture.TransformStarted -= this.PanStartedTwoFinger;
		this.panTwoFingerGesture.Transformed -= this.PannedTwoFinger;
		this.panTwoFingerGesture.TransformCompleted -= this.PanCompletedTwoFinger;
		this.scaleGesture.TransformStarted -= this.ScaleStarted;
		this.scaleGesture.Transformed -= this.Scaled;
		this.scaleGesture.TransformCompleted -= this.ScaleCompleted;
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000E50BC File Offset: 0x000E32BC
	private void Tapped(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("Tapped " + base.gameObject.name);
		PointerMode currentPointerMode = PlayerScript.PointerScript.CurrentPointerMode;
		if (currentPointerMode != PointerMode.Hidden)
		{
			if (currentPointerMode != PointerMode.Line)
			{
			}
		}
		else
		{
			PlayerScript.PointerScript.CheckDeleteTag(PlayerScript.PointerScript.pointerSyncs.PositionFromID(TouchScriptUpdater.BeginTouchId), "Fog");
		}
		if (this.NPO && this.NPO.IsGrabbable && !this.NPO.IsLocked)
		{
			if (PlayerScript.PointerScript.CurrentPointerMode != PointerMode.Snap && PlayerScript.PointerScript.CurrentPointerMode != PointerMode.SnapRotate && this.bPressed)
			{
				TouchHandlerScript.bAltZoom = true;
				return;
			}
		}
		else
		{
			PlayerScript.PointerScript.ResetAllObjects();
			PlayerScript.PointerScript.ResetHighlight();
		}
	}

	// Token: 0x06002005 RID: 8197 RVA: 0x000E51A0 File Offset: 0x000E33A0
	private void DoubleTapped(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("DoubleTapped " + base.gameObject.name);
		if (this.NPO && this.NPO.IsGrabbable && !this.NPO.IsLocked)
		{
			PlayerScript.PointerScript.Rotation(this.NPO.ID, 0, 12, false);
		}
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x000E5220 File Offset: 0x000E3420
	private void Pressed(object sender, EventArgs e)
	{
		this.bPressed = false;
		this.PressedScreenPosition = this.pressGesture.ScreenPosition;
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("Pressed " + base.gameObject.name);
		this.bLongPressed = false;
		TouchHandlerScript.bAltZoom = false;
		this.bPressed = true;
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x000E528C File Offset: 0x000E348C
	private void Released(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("Released " + base.gameObject.name);
		if (!this.bLongPressed && (PlayerScript.PointerScript.CurrentPointerMode == PointerMode.Snap || PlayerScript.PointerScript.CurrentPointerMode == PointerMode.SnapRotate) && Mathf.Abs(this.releaseGesture.ScreenPosition.x - this.PressedScreenPosition.x) / TouchManager.Instance.DotsPerCentimeter < 0.5f && Mathf.Abs(this.releaseGesture.ScreenPosition.y - this.PressedScreenPosition.y) / TouchManager.Instance.DotsPerCentimeter < 0.5f)
		{
			NetworkSingleton<SnapPointManager>.Instance.CreateSnapPoint(HoverScript.GetWorldPositionFromScreenPos(this.releaseGesture.ScreenPosition));
		}
		if (this.bLongPressed && PlayerScript.PointerScript.InfoObject == base.gameObject && this.NPO && this.NPO.IsGrabbable && this.NPO.IsHeldByNobody && !PlayerScript.PointerScript.HighLightedObjects.Contains(this.NPO))
		{
			PlayerScript.PointerScript.AddHighlight(this.NPO, false);
		}
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x000E53E4 File Offset: 0x000E35E4
	private void LongPressed(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("LongPressed " + base.gameObject.name);
		this.bLongPressed = true;
		if (this.NPO)
		{
			if (this.NPO.IsGrabbable && this.NPO.IsHeldByNobody && PlayerScript.PointerScript.CurrentPointerMode != PointerMode.Hidden && PlayerScript.PointerScript.CurrentPointerMode != PointerMode.Randomize)
			{
				if (base.GetComponent<SoundScript>())
				{
					base.GetComponent<SoundScript>().PickUpSound();
				}
				PlayerScript.PointerScript.StartContextual(base.gameObject, false);
			}
			else if (PermissionsOptions.CheckIfAllowedInMode(PointerMode.Hidden, -1))
			{
				PlayerScript.PointerScript.CheckStartZones(this.longPressGesture.ScreenPosition);
			}
		}
		if (!PlayerScript.PointerScript.InfoObject && !PlayerScript.PointerScript.InfoHiddenZoneGO && !PlayerScript.PointerScript.InfoRandomizeZoneGO && !PlayerScript.PointerScript.InfoLayoutZoneGO)
		{
			PlayerScript.PointerScript.StartGlobalContextual(this.longPressGesture.ScreenPosition);
		}
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x000E551B File Offset: 0x000E371B
	private void PanStartedTwoFinger(object sender, EventArgs e)
	{
		Debug.Log("PanStartedTwoFinger");
		this.bTwoFingerPanning = true;
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x000E5530 File Offset: 0x000E3730
	private void PannedTwoFinger(object sender, EventArgs e)
	{
		if (UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("PannedTwoFinger: " + (this.panTwoFingerGesture.ScreenPosition - this.panTwoFingerGesture.PreviousScreenPosition));
		TouchHandlerScript.cameraController.x += (this.panTwoFingerGesture.ScreenPosition.x - this.panTwoFingerGesture.PreviousScreenPosition.x) / TouchManager.Instance.DotsPerCentimeter * 15f;
		TouchHandlerScript.cameraController.y -= (this.panTwoFingerGesture.ScreenPosition.y - this.panTwoFingerGesture.PreviousScreenPosition.y) / TouchManager.Instance.DotsPerCentimeter * 15f;
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x000E55FF File Offset: 0x000E37FF
	private void PanCompletedTwoFinger(object sender, EventArgs e)
	{
		Debug.Log("PanCompletedTwoFinger");
		this.bTwoFingerPanning = false;
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x000E5612 File Offset: 0x000E3812
	private void PanStarted(object sender, EventArgs e)
	{
		PlayerScript.PointerScript.ResetAllObjects();
		Debug.Log("PanStarted " + base.gameObject.name);
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x000E5638 File Offset: 0x000E3838
	private void Panned(object sender, EventArgs e)
	{
		if (this.bTwoFingerPanning)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < this.panGesture.ActiveTouches.Count; i++)
		{
			int id = this.panGesture.ActiveTouches[i].Id;
			if (!this.PanIds.Contains(id))
			{
				num = id;
				this.PanIds.Add(id);
				break;
			}
		}
		if (UICamera.HoveredUIObject)
		{
			return;
		}
		if (PlayerScript.Pointer)
		{
			PointerMode currentPointerMode = PlayerScript.PointerScript.CurrentPointerMode;
			switch (currentPointerMode)
			{
			case PointerMode.Grab:
			{
				if (!this.NPO || !this.NPO.IsGrabbable || this.NPO.IsLocked)
				{
					this.PanCamera();
					goto IL_3FF;
				}
				int num2 = num;
				if (num2 == -1)
				{
					return;
				}
				if (this.bLongPressed || PlayerScript.PointerScript.HighLightedObjects.Contains(this.NPO) || (!base.gameObject.GetComponent<StackObject>() && !base.gameObject.GetComponent<DeckScript>() && !base.gameObject.GetComponent<ClockScript>()))
				{
					TouchScriptNGUI.DropIds.Add(num2);
					PlayerScript.PointerScript.Grab(this.NPO.ID, new Vector3?(PlayerScript.PointerScript.pointerSyncs.PositionFromID(num2)), num2, true, null);
					goto IL_3FF;
				}
				if (base.gameObject.GetComponent<StackObject>())
				{
					TouchScriptNGUI.DropIds.Add(num2);
					if (!Network.isServer)
					{
						PlayerScript.PointerScript.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(PlayerScript.PointerScript.TellObjectManagerAboutObjectTake), this.NPO.ID, PlayerScript.PointerScript.ID, num2);
						goto IL_3FF;
					}
					PlayerScript.PointerScript.TellObjectManagerAboutObjectTake(this.NPO.ID, PlayerScript.PointerScript.ID, num2);
					goto IL_3FF;
				}
				else if (base.gameObject.GetComponent<DeckScript>())
				{
					TouchScriptNGUI.DropIds.Add(num2);
					if (!Network.isServer)
					{
						PlayerScript.PointerScript.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(PlayerScript.PointerScript.TellObjectManagerAboutCardPeel), this.NPO.ID, PlayerScript.PointerScript.ID, num2);
						goto IL_3FF;
					}
					PlayerScript.PointerScript.TellObjectManagerAboutCardPeel(this.NPO.ID, PlayerScript.PointerScript.ID, num2);
					goto IL_3FF;
				}
				else
				{
					if (base.gameObject.GetComponent<ClockScript>())
					{
						TouchScriptNGUI.DropIds.Add(num2);
						PlayerScript.PointerScript.Grab(this.NPO.ID, new Vector3?(PlayerScript.PointerScript.pointerSyncs.PositionFromID(num2)), num2, true, null);
						goto IL_3FF;
					}
					goto IL_3FF;
				}
				break;
			}
			case PointerMode.Paint:
			case PointerMode.Erase:
				for (int j = 0; j < this.panGesture.ActiveTouches.Count; j++)
				{
					int id2 = this.panGesture.ActiveTouches[j].Id;
					PlayerScript.PointerScript.pointerSyncs.PositionFromID(id2);
				}
				goto IL_3FF;
			case PointerMode.Hidden:
			case (PointerMode)6:
				goto IL_3FF;
			case PointerMode.Line:
				if (num == -1)
				{
					PlayerScript.PointerScript.UpdateLine(HoverScript.PointerPosition);
					goto IL_3FF;
				}
				PlayerScript.PointerScript.StartLine(HoverScript.PointerPosition, null);
				goto IL_3FF;
			case PointerMode.Flick:
				if (!this.NPO || !this.NPO.IsGrabbable || this.NPO.IsLocked)
				{
					this.PanCamera();
					goto IL_3FF;
				}
				if (num == -1)
				{
					PlayerScript.PointerScript.UpdateLine(HoverScript.PointerPosition);
					goto IL_3FF;
				}
				PlayerScript.PointerScript.StartLine(HoverScript.PointerPosition, null);
				goto IL_3FF;
			case PointerMode.Snap:
				break;
			default:
				if (currentPointerMode != PointerMode.SnapRotate)
				{
					goto IL_3FF;
				}
				break;
			}
			if (!this.NPO || !this.NPO.IsGrabbable || this.NPO.IsLocked)
			{
				this.PanCamera();
			}
			IL_3FF:
			if (Pointer.IsZoneTool(PlayerScript.PointerScript.CurrentPointerMode))
			{
				if (num == -1)
				{
					PlayerScript.PointerScript.UpdateZone(PlayerScript.PointerScript.CurrentPointerMode, HoverScript.PointerPosition, Input.mousePosition, false);
				}
				else
				{
					PlayerScript.PointerScript.StartZone(PlayerScript.PointerScript.CurrentPointerMode, HoverScript.PointerPosition, Input.mousePosition, null);
				}
			}
			if (Pointer.IsCombineTool(PlayerScript.PointerScript.CurrentPointerMode))
			{
				if (!this.NPO || !this.NPO.IsGrabbable)
				{
					this.PanCamera();
					return;
				}
				if (num == -1)
				{
					PlayerScript.PointerScript.UpdateLine(HoverScript.PointerPosition);
					return;
				}
				PlayerScript.PointerScript.StartLine(HoverScript.PointerPosition, null);
				return;
			}
		}
		else
		{
			this.PanCamera();
		}
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x000E5B00 File Offset: 0x000E3D00
	private void PanCamera()
	{
		float x = this.panGesture.LocalDeltaPosition.x;
		float z = this.panGesture.LocalDeltaPosition.z;
		TouchHandlerScript.cameraController.target.position = new Vector3(TouchHandlerScript.cameraController.target.position.x + -x, TouchHandlerScript.cameraController.target.position.y, TouchHandlerScript.cameraController.target.position.z + -z);
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x000E5B88 File Offset: 0x000E3D88
	private void PanCompleted(object sender, EventArgs e)
	{
		this.PanIds.Clear();
		if (!PlayerScript.Pointer)
		{
			return;
		}
		Debug.Log("PanCompleted " + base.gameObject.name);
		PointerMode currentPointerMode = PlayerScript.PointerScript.CurrentPointerMode;
		if (currentPointerMode != PointerMode.Grab)
		{
			if (currentPointerMode != PointerMode.Line)
			{
				if (currentPointerMode == PointerMode.Flick)
				{
					if (this.NPO && this.NPO.IsGrabbable && !this.NPO.IsLocked)
					{
						PlayerScript.PointerScript.EndLine(HoverScript.PointerPosition, null);
					}
				}
			}
			else
			{
				PlayerScript.PointerScript.EndLine(HoverScript.PointerPosition, null);
			}
		}
		else if (this.NPO && this.NPO.IsGrabbable)
		{
		}
		if (Pointer.IsZoneTool(PlayerScript.PointerScript.CurrentPointerMode))
		{
			PlayerScript.PointerScript.EndZone(PlayerScript.PointerScript.CurrentPointerMode);
		}
		if (Pointer.IsCombineTool(PlayerScript.PointerScript.CurrentPointerMode) && this.NPO && this.NPO.IsGrabbable)
		{
			PlayerScript.PointerScript.EndLine(HoverScript.PointerPosition, null);
		}
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x000E5CA4 File Offset: 0x000E3EA4
	private void ScaleStarted(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("ScaleStarted " + base.gameObject.name);
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x000E5CD9 File Offset: 0x000E3ED9
	private void Scaled(object sender, EventArgs e)
	{
		TouchHandlerScript.cameraController.distance = TouchHandlerScript.cameraController.distance / this.scaleGesture.DeltaScale;
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x000E5CFB File Offset: 0x000E3EFB
	private void ScaleCompleted(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("ScaleCompleted " + base.gameObject.name);
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000E5D30 File Offset: 0x000E3F30
	private void RotateStarted(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("RotateStarted " + base.gameObject.name);
		this.RotateDegrees = 0f;
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x000E5D70 File Offset: 0x000E3F70
	private void Rotated(object sender, EventArgs e)
	{
		if (UICamera.HoveredUIObject)
		{
			return;
		}
		if (PlayerScript.Pointer && this.NPO && this.NPO.IsGrabbable && !this.NPO.IsLocked && Mathf.Abs(this.RotateDegrees) > 5f)
		{
			this.RotateDegrees = 0f;
			int spinRotationIndex = PlayerScript.PointerScript.RotationSnap / 15;
			PlayerScript.PointerScript.Rotation(this.NPO.ID, spinRotationIndex, 0, false);
		}
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000E5E00 File Offset: 0x000E4000
	private void RotateCompleted(object sender, EventArgs e)
	{
		if (!PlayerScript.Pointer || UICamera.HoveredUIObject)
		{
			return;
		}
		Debug.Log("RotateCompleted " + base.gameObject.name);
	}

	// Token: 0x04001391 RID: 5009
	private TapGesture tapGesture;

	// Token: 0x04001392 RID: 5010
	private TapGesture doubleTapGesture;

	// Token: 0x04001393 RID: 5011
	private PressGesture pressGesture;

	// Token: 0x04001394 RID: 5012
	private ReleaseGesture releaseGesture;

	// Token: 0x04001395 RID: 5013
	private LongPressGesture longPressGesture;

	// Token: 0x04001396 RID: 5014
	private TransformGesture panGesture;

	// Token: 0x04001397 RID: 5015
	private TransformGesture scaleGesture;

	// Token: 0x04001398 RID: 5016
	private TransformGesture panTwoFingerGesture;

	// Token: 0x04001399 RID: 5017
	private NetworkPhysicsObject NPO;

	// Token: 0x0400139A RID: 5018
	private static float PixelMovementThreshold;

	// Token: 0x0400139B RID: 5019
	private static CameraController cameraController;

	// Token: 0x0400139C RID: 5020
	public static bool bAltZoom;

	// Token: 0x0400139D RID: 5021
	private bool inited;

	// Token: 0x0400139E RID: 5022
	private bool bPressed;

	// Token: 0x0400139F RID: 5023
	private Vector2 PressedScreenPosition;

	// Token: 0x040013A0 RID: 5024
	private bool bLongPressed;

	// Token: 0x040013A1 RID: 5025
	private bool bTwoFingerPanning;

	// Token: 0x040013A2 RID: 5026
	private List<int> PanIds = new List<int>();

	// Token: 0x040013A3 RID: 5027
	private float RotateDegrees;
}
