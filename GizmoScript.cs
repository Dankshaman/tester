using System;
using System.Collections.Generic;
using System.Linq;
using NewNet;
using RTEditor;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class GizmoScript : MonoBehaviour
{
	// Token: 0x06000F01 RID: 3841 RVA: 0x000662C8 File Offset: 0x000644C8
	private void Start()
	{
		EventManager.OnChangePointerMode += this.ChangePointerMode;
		EventManager.OnNetworkObjectDestroy += this.ObjectDestroy;
		MonoSingletonBase<EditorGizmoSystem>.Instance.TranslationGizmo.GizmoDragStart += this.DragStart;
		MonoSingletonBase<EditorGizmoSystem>.Instance.RotationGizmo.GizmoDragStart += this.DragStart;
		MonoSingletonBase<EditorGizmoSystem>.Instance.ScaleGizmo.GizmoDragStart += this.DragStart;
		MonoSingletonBase<EditorGizmoSystem>.Instance.VolumeScaleGizmo.GizmoDragStart += this.DragStart;
		MonoSingletonBase<EditorGizmoSystem>.Instance.TranslationGizmo.GizmoDragUpdate += this.DragUpdate;
		MonoSingletonBase<EditorGizmoSystem>.Instance.RotationGizmo.GizmoDragUpdate += this.DragUpdate;
		MonoSingletonBase<EditorGizmoSystem>.Instance.ScaleGizmo.GizmoDragUpdate += this.DragUpdate;
		MonoSingletonBase<EditorGizmoSystem>.Instance.VolumeScaleGizmo.GizmoDragUpdate += this.DragUpdate;
		MonoSingletonBase<EditorGizmoSystem>.Instance.TranslationGizmo.GizmoDragEnd += this.DragEnd;
		MonoSingletonBase<EditorGizmoSystem>.Instance.RotationGizmo.GizmoDragEnd += this.DragEnd;
		MonoSingletonBase<EditorGizmoSystem>.Instance.ScaleGizmo.GizmoDragEnd += this.DragEnd;
		MonoSingletonBase<EditorGizmoSystem>.Instance.VolumeScaleGizmo.GizmoDragEnd += this.DragEnd;
		EventDelegate.Add(this.TransformSpaceButton.onClick, new EventDelegate.Callback(this.OnClickTransformSpace));
		EventDelegate.Add(this.PivotPointButton.onClick, new EventDelegate.Callback(this.OnClickPivotPoint));
		MonoSingletonBase<EditorGizmoSystem>.Instance.TransformSpace = TransformSpace.Local;
		MonoSingletonBase<EditorGizmoSystem>.Instance.TransformPivotPoint = TransformPivotPoint.Center;
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x0006648C File Offset: 0x0006468C
	private void OnDestroy()
	{
		EventManager.OnChangePointerMode -= this.ChangePointerMode;
		EventManager.OnNetworkObjectDestroy -= this.ObjectDestroy;
		MonoSingletonBase<EditorGizmoSystem>.Instance.TranslationGizmo.GizmoDragUpdate -= this.DragUpdate;
		MonoSingletonBase<EditorGizmoSystem>.Instance.RotationGizmo.GizmoDragUpdate -= this.DragUpdate;
		MonoSingletonBase<EditorGizmoSystem>.Instance.ScaleGizmo.GizmoDragUpdate -= this.DragUpdate;
		MonoSingletonBase<EditorGizmoSystem>.Instance.VolumeScaleGizmo.GizmoDragUpdate -= this.DragUpdate;
		EventDelegate.Remove(this.TransformSpaceButton.onClick, new EventDelegate.Callback(this.OnClickTransformSpace));
		EventDelegate.Remove(this.PivotPointButton.onClick, new EventDelegate.Callback(this.OnClickPivotPoint));
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x00066561 File Offset: 0x00064761
	private bool inGizmoMode()
	{
		return PlayerScript.Pointer && Pointer.IsGizmoTool(PlayerScript.PointerScript.CurrentPointerMode);
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x00066580 File Offset: 0x00064780
	private void ChangePointerMode(PointerMode newMode)
	{
		if (!this.inGizmoMode())
		{
			MonoSingletonBase<EditorObjectSelection>.Instance.ClearSelection(false);
			return;
		}
		PointerMode currentPointerMode = PlayerScript.PointerScript.CurrentPointerMode;
		switch (currentPointerMode)
		{
		case PointerMode.Move:
			MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType = GizmoType.Translation;
			NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(false);
			return;
		case PointerMode.Rotate:
			MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType = GizmoType.Rotation;
			NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(false);
			return;
		case PointerMode.Scale:
			MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType = GizmoType.Scale;
			NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(false);
			return;
		default:
			if (currentPointerMode == PointerMode.VolumeScale)
			{
				MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType = GizmoType.VolumeScale;
				NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(false);
				return;
			}
			if (currentPointerMode != PointerMode.RotationValue)
			{
				return;
			}
			MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType = GizmoType.Rotation;
			if (MonoSingletonBase<EditorObjectSelection>.Instance.LastSelectedGameObject)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(true);
			}
			return;
		}
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x00066685 File Offset: 0x00064885
	private void ObjectDestroy(NetworkPhysicsObject NPO)
	{
		if (MonoSingletonBase<EditorObjectSelection>.Instance.IsObjectSelected(NPO.gameObject))
		{
			MonoSingletonBase<EditorObjectSelection>.Instance.RemoveObjectFromSelection(NPO.gameObject, false);
		}
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x000666AC File Offset: 0x000648AC
	private void DragStart(Gizmo gizmo)
	{
		GizmoType gizmoType = gizmo.GetGizmoType();
		if (gizmoType - GizmoType.Scale <= 1)
		{
			this.ScaleDictionary.Clear();
			foreach (GameObject gameObject in MonoSingletonBase<EditorObjectSelection>.Instance.SelectedGameObjects)
			{
				this.ScaleDictionary.Add(gameObject, gameObject.transform.localScale);
			}
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0006672C File Offset: 0x0006492C
	private void DragUpdate(Gizmo gizmo)
	{
		GizmoScript.ClientSyncTransform(gizmo.GetGizmoType() == GizmoType.Translation || gizmo.GetGizmoType() == GizmoType.VolumeScale, gizmo.GetGizmoType() == GizmoType.Rotation, false);
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x00066754 File Offset: 0x00064954
	private void DragEnd(Gizmo gizmo)
	{
		GizmoType gizmoType = gizmo.GetGizmoType();
		if (gizmoType - GizmoType.Scale <= 1)
		{
			foreach (KeyValuePair<GameObject, Vector3> keyValuePair in this.ScaleDictionary)
			{
				if (keyValuePair.Key != null)
				{
					NetworkPhysicsObject component = keyValuePair.Key.GetComponent<NetworkPhysicsObject>();
					if (component)
					{
						Vector3 vector = new Vector3(component.transform.localScale.x / keyValuePair.Value.x, component.transform.localScale.y / keyValuePair.Value.y, component.transform.localScale.z / keyValuePair.Value.z);
						if (vector != Vector3.one)
						{
							vector = new Vector3(vector.x * component.Scale.x, vector.y * component.Scale.y, vector.z * component.Scale.z);
							component.SetScale(vector, true);
							GizmoScript.ClientSyncTransform(gizmo.GetGizmoType() == GizmoType.VolumeScale, false, true);
						}
					}
				}
			}
			this.ScaleDictionary.Clear();
		}
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x000668BC File Offset: 0x00064ABC
	public static void ClientSyncTransform(bool syncPosition, bool syncRotation, bool syncScale)
	{
		if (Network.isClient)
		{
			foreach (GameObject gameObject in MonoSingletonBase<EditorObjectSelection>.Instance.SelectedGameObjects)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component)
				{
					if (syncPosition)
					{
						component.SetPosition(gameObject.transform.position);
					}
					if (syncRotation)
					{
						component.SetRotation(gameObject.transform.eulerAngles);
					}
					if (syncScale)
					{
						component.SetScale(component.Scale, false);
					}
				}
			}
		}
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0006695C File Offset: 0x00064B5C
	private void OnClickTransformSpace()
	{
		UILabel componentInChildren = this.TransformSpaceButton.GetComponentInChildren<UILabel>();
		if (MonoSingletonBase<EditorGizmoSystem>.Instance.TransformSpace == TransformSpace.Local)
		{
			MonoSingletonBase<EditorGizmoSystem>.Instance.TransformSpace = TransformSpace.Global;
			Language.UpdateUILabel(componentInChildren, "Global");
			return;
		}
		MonoSingletonBase<EditorGizmoSystem>.Instance.TransformSpace = TransformSpace.Local;
		Language.UpdateUILabel(componentInChildren, "Local");
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x000669B0 File Offset: 0x00064BB0
	private void OnClickPivotPoint()
	{
		UILabel componentInChildren = this.PivotPointButton.GetComponentInChildren<UILabel>();
		if (MonoSingletonBase<EditorGizmoSystem>.Instance.TransformPivotPoint == TransformPivotPoint.Center)
		{
			MonoSingletonBase<EditorGizmoSystem>.Instance.TransformPivotPoint = TransformPivotPoint.MeshPivot;
			Language.UpdateUILabel(componentInChildren, "Pivot");
			return;
		}
		MonoSingletonBase<EditorGizmoSystem>.Instance.TransformPivotPoint = TransformPivotPoint.Center;
		Language.UpdateUILabel(componentInChildren, "Center");
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x00066A03 File Offset: 0x00064C03
	private bool GizmoAction()
	{
		return zInput.GetButton("Grab", ControlType.All) || (VRHMD.isVR && VRTrackedController.ToolAction);
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x00066A22 File Offset: 0x00064C22
	private bool GizmoActionDown()
	{
		return zInput.GetButtonDown("Grab", ControlType.All) || (VRHMD.isVR && VRTrackedController.ToolActionDown);
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x00066A41 File Offset: 0x00064C41
	private bool GizmoActionUp()
	{
		return zInput.GetButtonUp("Grab", ControlType.All) || (VRHMD.isVR && VRTrackedController.ToolActionUp);
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x00066A60 File Offset: 0x00064C60
	private void LateUpdate()
	{
		if (!PlayerScript.Pointer)
		{
			MonoSingletonBase<EditorObjectSelection>.Instance.ClearSelection(false);
			return;
		}
		if (PlayerScript.Pointer && !VRHMD.isVR && this.inGizmoMode() && !UICamera.HoveredUIObject)
		{
			if (this.GizmoActionDown())
			{
				this.boxSelect = false;
				this.startCheck = true;
				this.buttonDownStart = HoverScript.PointerPosition;
			}
			if (this.startCheck && !this.boxSelect && this.GizmoAction() && Vector3.Distance(this.buttonDownStart, HoverScript.PointerPosition) > HoverScript.DISTANCE_CHECK)
			{
				if (!zInput.GetButton("Ctrl", ControlType.All))
				{
					MonoSingletonBase<EditorObjectSelection>.Instance.ClearSelection(false);
				}
				this.boxSelect = true;
				Singleton<RectangleSelection>.Instance.StartSelection(MonoSingletonBase<EditorObjectSelection>.Instance.SelectedGameObjects.ToList<GameObject>(), new Action<GameObject>(this.AddSelection), new Action<GameObject>(this.RemoveSelection), 256);
				this.startCheck = false;
			}
			if (this.startCheck && !this.boxSelect && this.GizmoActionUp())
			{
				this.startCheck = false;
				if (!zInput.GetButton("Ctrl", ControlType.All))
				{
					MonoSingletonBase<EditorObjectSelection>.Instance.ClearSelection(false);
				}
				GameObject hoverGizmoObject = HoverScript.HoverGizmoObject;
				if (hoverGizmoObject)
				{
					hoverGizmoObject.GetComponent<NetworkPhysicsObject>();
					if (!MonoSingletonBase<EditorObjectSelection>.Instance.IsObjectSelected(hoverGizmoObject))
					{
						if (!MonoSingletonBase<EditorObjectSelection>.Instance.AddObjectToSelection(hoverGizmoObject, false))
						{
							Debug.Log(string.Concat(new object[]
							{
								"Can't add: ",
								hoverGizmoObject,
								" : ",
								LayerMask.LayerToName(hoverGizmoObject.layer)
							}));
						}
						NetworkSingleton<NetworkUI>.Instance.GUITransform.gameObject.SetActive(true);
						if (PlayerScript.PointerScript.CurrentPointerMode == PointerMode.RotationValue)
						{
							NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(false);
							NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(true);
						}
					}
					else if (zInput.GetButton("Ctrl", ControlType.All))
					{
						MonoSingletonBase<EditorObjectSelection>.Instance.RemoveObjectFromSelection(hoverGizmoObject, false);
					}
				}
			}
		}
		bool flag = false;
		if (this.boxSelect)
		{
			if (this.GizmoActionUp())
			{
				this.boxSelect = false;
				Singleton<RectangleSelection>.Instance.EndSelection();
				flag = true;
			}
		}
		else if (VRHMD.isVR && this.GizmoActionUp())
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in PlayerScript.PointerScript.HighLightedObjects)
			{
				this.AddSelection(networkPhysicsObject.gameObject);
			}
			flag = true;
		}
		if (flag)
		{
			NetworkSingleton<NetworkUI>.Instance.GUITransform.gameObject.SetActive(MonoSingletonBase<EditorObjectSelection>.Instance.NumberOfSelectedObjects > 0);
			if (PlayerScript.PointerScript.CurrentPointerMode == PointerMode.RotationValue)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIRotationValue.gameObject.SetActive(MonoSingletonBase<EditorObjectSelection>.Instance.NumberOfSelectedObjects > 0);
			}
		}
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x00066D58 File Offset: 0x00064F58
	public void AddSelection(GameObject GO)
	{
		MonoSingletonBase<EditorObjectSelection>.Instance.AddObjectToSelection(GO, false);
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x00066D67 File Offset: 0x00064F67
	public void RemoveSelection(GameObject GO)
	{
		MonoSingletonBase<EditorObjectSelection>.Instance.RemoveObjectFromSelection(GO, false);
	}

	// Token: 0x04000952 RID: 2386
	public UIButton TransformSpaceButton;

	// Token: 0x04000953 RID: 2387
	public UIButton PivotPointButton;

	// Token: 0x04000954 RID: 2388
	private Dictionary<GameObject, Vector3> ScaleDictionary = new Dictionary<GameObject, Vector3>();

	// Token: 0x04000955 RID: 2389
	private bool boxSelect;

	// Token: 0x04000956 RID: 2390
	private bool startCheck;

	// Token: 0x04000957 RID: 2391
	private Vector3 buttonDownStart = Vector3.zero;
}
