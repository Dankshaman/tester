using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000062 RID: 98
public static class NGUITools
{
	// Token: 0x060003A0 RID: 928 RVA: 0x0001ADC4 File Offset: 0x00018FC4
	public static UILabel GetChildLabel(GameObject Button)
	{
		return Button.transform.Find("Label").GetComponent<UILabel>();
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060003A1 RID: 929 RVA: 0x0001ADDB File Offset: 0x00018FDB
	// (set) Token: 0x060003A2 RID: 930 RVA: 0x0001AE03 File Offset: 0x00019003
	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001AE24 File Offset: 0x00019024
	public static bool fileAccess
	{
		get
		{
			return Application.platform != RuntimePlatform.WebGLPlayer;
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001AE32 File Offset: 0x00019032
	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001AE44 File Offset: 0x00019044
	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001AE54 File Offset: 0x00019054
	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		float time = RealTime.time;
		if (NGUITools.mLastClip == clip && NGUITools.mLastTimestamp + 0.1f > time)
		{
			return null;
		}
		NGUITools.mLastClip = clip;
		NGUITools.mLastTimestamp = time;
		volume *= NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null || !NGUITools.GetActive(NGUITools.mListener))
			{
				AudioListener[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (NGUITools.GetActive(array[i]))
						{
							NGUITools.mListener = array[i];
							break;
						}
					}
				}
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = (UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera);
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null && NGUITools.mListener.enabled && NGUITools.GetActive(NGUITools.mListener.gameObject))
			{
				if (!NGUITools.audioSource)
				{
					NGUITools.audioSource = NGUITools.mListener.GetComponent<AudioSource>();
					if (NGUITools.audioSource == null)
					{
						NGUITools.audioSource = NGUITools.mListener.gameObject.AddComponent<AudioSource>();
					}
				}
				NGUITools.audioSource.priority = 50;
				NGUITools.audioSource.pitch = pitch;
				NGUITools.audioSource.PlayOneShot(clip, volume);
				return NGUITools.audioSource;
			}
		}
		return null;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001AFE3 File Offset: 0x000191E3
	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return UnityEngine.Random.Range(min, max + 1);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001AFF4 File Offset: 0x000191F4
	public static string GetHierarchy(GameObject obj)
	{
		if (obj == null)
		{
			return "";
		}
		string text = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = obj.name + "\\" + text;
		}
		return text;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001B051 File Offset: 0x00019251
	public static T[] FindActive<T>() where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType(typeof(!!0)) as !!0[];
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001B068 File Offset: 0x00019268
	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		Camera camera;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			camera = UICamera.list.buffer[i].cachedCamera;
			if (camera && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		camera = Camera.main;
		if (camera && (camera.cullingMask & num) != 0)
		{
			return camera;
		}
		Camera[] array = new Camera[Camera.allCamerasCount];
		int allCameras = Camera.GetAllCameras(array);
		for (int j = 0; j < allCameras; j++)
		{
			camera = array[j];
			if (camera && camera.enabled && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		return null;
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001B11A File Offset: 0x0001931A
	public static void AddWidgetCollider(GameObject go)
	{
		NGUITools.AddWidgetCollider(go, false);
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001B124 File Offset: 0x00019324
	public static void AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			Collider component = go.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			if (boxCollider != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
				return;
			}
			if (component != null)
			{
				return;
			}
			BoxCollider2D boxCollider2D = go.GetComponent<BoxCollider2D>();
			if (boxCollider2D != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			UICamera uicamera = UICamera.FindCameraForLayer(go.layer);
			if (uicamera != null && (uicamera.eventType == UICamera.EventType.World_2D || uicamera.eventType == UICamera.EventType.UI_2D))
			{
				boxCollider2D = go.AddComponent<BoxCollider2D>();
				boxCollider2D.isTrigger = true;
				UIWidget component2 = go.GetComponent<UIWidget>();
				if (component2 != null)
				{
					component2.autoResizeBoxCollider = true;
				}
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			boxCollider = go.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			UIWidget component3 = go.GetComponent<UIWidget>();
			if (component3 != null)
			{
				component3.autoResizeBoxCollider = true;
			}
			NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
		}
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001B204 File Offset: 0x00019404
	public static void UpdateWidgetCollider(GameObject go)
	{
		NGUITools.UpdateWidgetCollider(go, false);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001B210 File Offset: 0x00019410
	public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			BoxCollider component = go.GetComponent<BoxCollider>();
			if (component != null)
			{
				NGUITools.UpdateWidgetCollider(component, considerInactive);
				return;
			}
			BoxCollider2D component2 = go.GetComponent<BoxCollider2D>();
			if (component2 != null)
			{
				NGUITools.UpdateWidgetCollider(component2, considerInactive);
			}
		}
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001B258 File Offset: 0x00019458
	public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector4 drawRegion = component.drawRegion;
				if (drawRegion.x != 0f || drawRegion.y != 0f || drawRegion.z != 1f || drawRegion.w != 1f)
				{
					Vector4 drawingDimensions = component.drawingDimensions;
					box.center = new Vector3((drawingDimensions.x + drawingDimensions.z) * 0.5f, (drawingDimensions.y + drawingDimensions.w) * 0.5f);
					box.size = new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y);
					return;
				}
				Vector3[] localCorners = component.localCorners;
				box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
				box.size = localCorners[2] - localCorners[0];
				return;
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bounds.center;
				box.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
			}
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0001B3AC File Offset: 0x000195AC
	public static void UpdateWidgetCollider(BoxCollider2D box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector4 drawRegion = component.drawRegion;
				if (drawRegion.x != 0f || drawRegion.y != 0f || drawRegion.z != 1f || drawRegion.w != 1f)
				{
					Vector4 drawingDimensions = component.drawingDimensions;
					box.offset = new Vector3((drawingDimensions.x + drawingDimensions.z) * 0.5f, (drawingDimensions.y + drawingDimensions.w) * 0.5f);
					box.size = new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y);
					return;
				}
				Vector3[] localCorners = component.localCorners;
				box.offset = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
				box.size = localCorners[2] - localCorners[0];
				return;
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.offset = bounds.center;
				box.size = new Vector2(bounds.size.x, bounds.size.y);
			}
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0001B514 File Offset: 0x00019714
	public static string GetTypeName<T>()
	{
		string text = typeof(!!0).ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0001B560 File Offset: 0x00019760
	public static string GetTypeName(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string text = obj.GetType().ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x000025B8 File Offset: 0x000007B8
	public static void RegisterUndo(UnityEngine.Object obj, string name)
	{
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x000025B8 File Offset: 0x000007B8
	public static void SetDirty(UnityEngine.Object obj)
	{
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001B5B6 File Offset: 0x000197B6
	public static GameObject AddChild(GameObject parent)
	{
		return parent.AddChild(true, -1);
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0001B5C0 File Offset: 0x000197C0
	public static GameObject AddChild(this GameObject parent, int layer)
	{
		return parent.AddChild(true, layer);
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0001B5CA File Offset: 0x000197CA
	public static GameObject AddChild(this GameObject parent, bool undo)
	{
		return parent.AddChild(undo, -1);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001B5D4 File Offset: 0x000197D4
	public static GameObject AddChild(this GameObject parent, bool undo, int layer)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			if (layer == -1)
			{
				gameObject.layer = parent.layer;
			}
			else if (layer > -1 && layer < 32)
			{
				gameObject.layer = layer;
			}
		}
		return gameObject;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0001B645 File Offset: 0x00019845
	public static GameObject AddChild(this GameObject parent, GameObject prefab)
	{
		return parent.AddChild(prefab, -1);
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0001B650 File Offset: 0x00019850
	public static GameObject AddChild(this GameObject parent, GameObject prefab, int layer)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		if (gameObject != null)
		{
			gameObject.name = prefab.name;
			if (parent != null)
			{
				Transform transform = gameObject.transform;
				transform.parent = parent.transform;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				if (layer == -1)
				{
					gameObject.layer = parent.layer;
				}
				else if (layer > -1 && layer < 32)
				{
					gameObject.layer = layer;
				}
			}
			gameObject.SetActive(true);
		}
		return gameObject;
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0001B6E0 File Offset: 0x000198E0
	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = int.MaxValue;
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			if (componentsInChildren[i].enabled)
			{
				num = Mathf.Min(num, componentsInChildren[i].raycastDepth);
			}
			i++;
		}
		return num;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001B744 File Offset: 0x00019944
	public static int CalculateNextDepth(GameObject go)
	{
		if (go)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				num = Mathf.Max(num, componentsInChildren[i].depth);
				i++;
			}
			return num + 1;
		}
		return 0;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0001B788 File Offset: 0x00019988
	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (go && ignoreChildrenWithColliders)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				UIWidget uiwidget = componentsInChildren[i];
				if (!(uiwidget.cachedGameObject != go) || (!(uiwidget.GetComponent<Collider>() != null) && !(uiwidget.GetComponent<Collider2D>() != null)))
				{
					num = Mathf.Max(num, uiwidget.depth);
				}
				i++;
			}
			return num + 1;
		}
		return NGUITools.CalculateNextDepth(go);
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0001B804 File Offset: 0x00019A04
	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (!(go != null))
		{
			return 0;
		}
		UIPanel uipanel = go.GetComponent<UIPanel>();
		if (uipanel != null)
		{
			UIPanel[] componentsInChildren = go.GetComponentsInChildren<UIPanel>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].depth += adjustment;
			}
			return 1;
		}
		uipanel = NGUITools.FindInParents<UIPanel>(go);
		if (uipanel == null)
		{
			return 0;
		}
		UIWidget[] componentsInChildren2 = go.GetComponentsInChildren<UIWidget>(true);
		int j = 0;
		int num = componentsInChildren2.Length;
		while (j < num)
		{
			UIWidget uiwidget = componentsInChildren2[j];
			if (!(uiwidget.panel != uipanel))
			{
				uiwidget.depth += adjustment;
			}
			j++;
		}
		return 2;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001B8B0 File Offset: 0x00019AB0
	public static void BringForward(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, 1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
			return;
		}
		if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001B8DC File Offset: 0x00019ADC
	public static void PushBack(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, -1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
			return;
		}
		if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0001B908 File Offset: 0x00019B08
	public static void NormalizeDepths()
	{
		NGUITools.NormalizeWidgetDepths();
		NGUITools.NormalizePanelDepths();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0001B914 File Offset: 0x00019B14
	public static void NormalizeWidgetDepths()
	{
		NGUITools.NormalizeWidgetDepths(NGUITools.FindActive<UIWidget>());
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0001B920 File Offset: 0x00019B20
	public static void NormalizeWidgetDepths(GameObject go)
	{
		NGUITools.NormalizeWidgetDepths(go.GetComponentsInChildren<UIWidget>());
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001B930 File Offset: 0x00019B30
	public static void NormalizeWidgetDepths(UIWidget[] list)
	{
		int num = list.Length;
		if (num > 0)
		{
			Array.Sort<UIWidget>(list, new Comparison<UIWidget>(UIWidget.FullCompareFunc));
			int num2 = 0;
			int depth = list[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIWidget uiwidget = list[i];
				if (uiwidget.depth == depth)
				{
					uiwidget.depth = num2;
				}
				else
				{
					depth = uiwidget.depth;
					num2 = (uiwidget.depth = num2 + 1);
				}
			}
		}
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0001B99C File Offset: 0x00019B9C
	public static void NormalizePanelDepths()
	{
		UIPanel[] array = NGUITools.FindActive<UIPanel>();
		int num = array.Length;
		if (num > 0)
		{
			Array.Sort<UIPanel>(array, new Comparison<UIPanel>(UIPanel.CompareFunc));
			int num2 = 0;
			int depth = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIPanel uipanel = array[i];
				if (uipanel.depth == depth)
				{
					uipanel.depth = num2;
				}
				else
				{
					depth = uipanel.depth;
					num2 = (uipanel.depth = num2 + 1);
				}
			}
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0001BA12 File Offset: 0x00019C12
	public static UIPanel CreateUI(bool advanced3D)
	{
		return NGUITools.CreateUI(null, advanced3D, -1);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0001BA1C File Offset: 0x00019C1C
	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return NGUITools.CreateUI(null, advanced3D, layer);
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0001BA28 File Offset: 0x00019C28
	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uiroot = (trans != null) ? NGUITools.FindInParents<UIRoot>(trans.gameObject) : null;
		if (uiroot == null && UIRoot.list.Count > 0)
		{
			foreach (UIRoot uiroot2 in UIRoot.list)
			{
				if (uiroot2.gameObject.layer == layer)
				{
					uiroot = uiroot2;
					break;
				}
			}
		}
		if (uiroot == null)
		{
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				UIPanel uipanel = UIPanel.list[i];
				GameObject gameObject = uipanel.gameObject;
				if (gameObject.hideFlags == HideFlags.None && gameObject.layer == layer)
				{
					trans.parent = uipanel.transform;
					trans.localScale = Vector3.one;
					return uipanel;
				}
				i++;
			}
		}
		if (uiroot != null)
		{
			UICamera componentInChildren = uiroot.GetComponentInChildren<UICamera>();
			if (componentInChildren != null && componentInChildren.GetComponent<Camera>().orthographic == advanced3D)
			{
				trans = null;
				uiroot = null;
			}
		}
		if (uiroot == null)
		{
			GameObject gameObject2 = NGUITools.AddChild((GameObject)null, false);
			uiroot = gameObject2.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject2.layer = layer;
			if (advanced3D)
			{
				gameObject2.name = "UI Root (3D)";
				uiroot.scalingStyle = UIRoot.Scaling.Constrained;
			}
			else
			{
				gameObject2.name = "UI Root";
				uiroot.scalingStyle = UIRoot.Scaling.Flexible;
			}
			uiroot.UpdateScale(true);
		}
		UIPanel uipanel2 = uiroot.GetComponentInChildren<UIPanel>();
		if (uipanel2 == null)
		{
			Camera[] array = NGUITools.FindActive<Camera>();
			float num = -1f;
			bool flag = false;
			int num2 = 1 << uiroot.gameObject.layer;
			foreach (Camera camera in array)
			{
				if (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox)
				{
					flag = true;
				}
				num = Mathf.Max(num, camera.depth);
				camera.cullingMask &= ~num2;
			}
			Camera camera2 = uiroot.gameObject.AddChild(false);
			camera2.gameObject.AddComponent<UICamera>();
			camera2.clearFlags = (flag ? CameraClearFlags.Depth : CameraClearFlags.Color);
			camera2.backgroundColor = Color.grey;
			camera2.cullingMask = num2;
			camera2.depth = num + 1f;
			if (advanced3D)
			{
				camera2.nearClipPlane = 0.1f;
				camera2.farClipPlane = 4f;
				camera2.transform.localPosition = new Vector3(0f, 0f, -700f);
			}
			else
			{
				camera2.orthographic = true;
				camera2.orthographicSize = 1f;
				camera2.nearClipPlane = -10f;
				camera2.farClipPlane = 10f;
			}
			AudioListener[] array2 = NGUITools.FindActive<AudioListener>();
			if (array2 == null || array2.Length == 0)
			{
				camera2.gameObject.AddComponent<AudioListener>();
			}
			uipanel2 = uiroot.gameObject.AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.parent != null)
			{
				trans = trans.parent;
			}
			if (NGUITools.IsChild(trans, uipanel2.transform))
			{
				uipanel2 = trans.gameObject.AddComponent<UIPanel>();
			}
			else
			{
				trans.parent = uipanel2.transform;
				trans.localScale = Vector3.one;
				trans.localPosition = Vector3.zero;
				uipanel2.cachedTransform.SetChildLayer(uipanel2.cachedGameObject.layer);
			}
		}
		return uipanel2;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0001BDA0 File Offset: 0x00019FA0
	public static void SetChildLayer(this Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			child.SetChildLayer(layer);
		}
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001BDD8 File Offset: 0x00019FD8
	public static T AddChild<T>(this GameObject parent) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent);
		string typeName;
		if (!NGUITools.mTypeNames.TryGetValue(typeof(!!0), out typeName) || typeName == null)
		{
			typeName = NGUITools.GetTypeName<T>();
			NGUITools.mTypeNames[typeof(!!0)] = typeName;
		}
		gameObject.name = typeName;
		return gameObject.AddComponent<T>();
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001BE30 File Offset: 0x0001A030
	public static T AddChild<T>(this GameObject parent, bool undo) where T : Component
	{
		GameObject gameObject = parent.AddChild(undo);
		string typeName;
		if (!NGUITools.mTypeNames.TryGetValue(typeof(!!0), out typeName) || typeName == null)
		{
			typeName = NGUITools.GetTypeName<T>();
			NGUITools.mTypeNames[typeof(!!0)] = typeName;
		}
		gameObject.name = typeName;
		return gameObject.AddComponent<T>();
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001BE86 File Offset: 0x0001A086
	public static T AddWidget<T>(this GameObject go, int depth = 2147483647) where T : UIWidget
	{
		if (depth == 2147483647)
		{
			depth = NGUITools.CalculateNextDepth(go);
		}
		T t = go.AddChild<T>();
		t.width = 100;
		t.height = 100;
		t.depth = depth;
		return t;
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001BEC4 File Offset: 0x0001A0C4
	public static UISprite AddSprite(this GameObject go, UIAtlas atlas, string spriteName, int depth = 2147483647)
	{
		UISpriteData uispriteData = (atlas != null) ? atlas.GetSprite(spriteName) : null;
		UISprite uisprite = go.AddWidget(depth);
		uisprite.type = ((uispriteData == null || !uispriteData.hasBorder) ? UIBasicSprite.Type.Simple : UIBasicSprite.Type.Sliced);
		uisprite.atlas = atlas;
		uisprite.spriteName = spriteName;
		return uisprite;
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0001BF10 File Offset: 0x0001A110
	public static GameObject GetRoot(GameObject go)
	{
		Transform transform = go.transform;
		for (;;)
		{
			Transform parent = transform.parent;
			if (parent == null)
			{
				break;
			}
			transform = parent;
		}
		return transform.gameObject;
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001BF40 File Offset: 0x0001A140
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return default(!!0);
		}
		return go.GetComponentInParent<T>();
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0001BF68 File Offset: 0x0001A168
	public static T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null)
		{
			return default(!!0);
		}
		return trans.GetComponentInParent<T>();
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001BF90 File Offset: 0x0001A190
	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj)
		{
			if (obj is Transform)
			{
				Transform transform = obj as Transform;
				GameObject gameObject = transform.gameObject;
				if (Application.isPlaying)
				{
					transform.parent = null;
					UnityEngine.Object.Destroy(gameObject);
					return;
				}
				UnityEngine.Object.DestroyImmediate(gameObject);
				return;
			}
			else if (obj is GameObject)
			{
				GameObject gameObject2 = obj as GameObject;
				Transform transform2 = gameObject2.transform;
				if (Application.isPlaying)
				{
					transform2.parent = null;
					UnityEngine.Object.Destroy(gameObject2);
					return;
				}
				UnityEngine.Object.DestroyImmediate(gameObject2);
				return;
			}
			else
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(obj);
					return;
				}
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001C020 File Offset: 0x0001A220
	public static void DestroyChildren(this Transform t)
	{
		bool isPlaying = Application.isPlaying;
		while (t.childCount != 0)
		{
			Transform child = t.GetChild(0);
			if (isPlaying)
			{
				child.parent = null;
				UnityEngine.Object.Destroy(child.gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0001C067 File Offset: 0x0001A267
	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
				return;
			}
			UnityEngine.Object.Destroy(obj);
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001C088 File Offset: 0x0001A288
	public static void Broadcast(string funcName)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001C0C4 File Offset: 0x0001A2C4
	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001C101 File Offset: 0x0001A301
	public static bool IsChild(Transform parent, Transform child)
	{
		return child.IsChildOf(parent);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001C10A File Offset: 0x0001A30A
	private static void Activate(Transform t)
	{
		NGUITools.Activate(t, false);
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001C114 File Offset: 0x0001A314
	private static void Activate(Transform t, bool compatibilityMode)
	{
		NGUITools.SetActiveSelf(t.gameObject, true);
		if (compatibilityMode)
		{
			int i = 0;
			int childCount = t.childCount;
			while (i < childCount)
			{
				if (t.GetChild(i).gameObject.activeSelf)
				{
					return;
				}
				i++;
			}
			int j = 0;
			int childCount2 = t.childCount;
			while (j < childCount2)
			{
				NGUITools.Activate(t.GetChild(j), true);
				j++;
			}
		}
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001C177 File Offset: 0x0001A377
	private static void Deactivate(Transform t)
	{
		NGUITools.SetActiveSelf(t.gameObject, false);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0001C185 File Offset: 0x0001A385
	public static void SetActive(GameObject go, bool state)
	{
		NGUITools.SetActive(go, state, true);
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0001C18F File Offset: 0x0001A38F
	public static void SetActive(GameObject go, bool state, bool compatibilityMode)
	{
		if (go)
		{
			if (state)
			{
				NGUITools.Activate(go.transform, compatibilityMode);
				NGUITools.CallCreatePanel(go.transform);
				return;
			}
			NGUITools.Deactivate(go.transform);
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001C1C0 File Offset: 0x0001A3C0
	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.CallCreatePanel(t.GetChild(i));
			i++;
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001C204 File Offset: 0x0001A404
	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transform = go.transform;
		if (state)
		{
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				NGUITools.Activate(transform.GetChild(i));
				i++;
			}
			return;
		}
		int j = 0;
		int childCount2 = transform.childCount;
		while (j < childCount2)
		{
			NGUITools.Deactivate(transform.GetChild(j));
			j++;
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001C25C File Offset: 0x0001A45C
	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001C27C File Offset: 0x0001A47C
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(Behaviour mb)
	{
		return mb && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0001C29B File Offset: 0x0001A49B
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(GameObject go)
	{
		return go && go.activeInHierarchy;
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0001C2AD File Offset: 0x0001A4AD
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0001C2B8 File Offset: 0x0001A4B8
	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		Transform transform = go.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			NGUITools.SetLayer(transform.GetChild(i).gameObject, layer);
			i++;
		}
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0001C2F8 File Offset: 0x0001A4F8
	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0001C334 File Offset: 0x0001A534
	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.localPosition = NGUITools.Round(t.localPosition);
			t.localScale = NGUITools.Round(t.localScale);
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.MakePixelPerfect(t.GetChild(i));
			i++;
		}
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0001C3B4 File Offset: 0x0001A5B4
	public static void FitOnScreen(this Camera cam, Transform t, bool considerInactive = false, bool considerChildren = true)
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(t, t, considerInactive, considerChildren);
		Vector3 a = cam.WorldToScreenPoint(t.position);
		Vector3 vector = a + bounds.min;
		Vector3 vector2 = a + bounds.max;
		int width = Screen.width;
		int height = Screen.height;
		Vector2 zero = Vector2.zero;
		if (vector.x < 0f)
		{
			zero.x = -vector.x;
		}
		else if (vector2.x > (float)width)
		{
			zero.x = (float)width - vector2.x;
		}
		if (vector.y < 0f)
		{
			zero.y = -vector.y;
		}
		else if (vector2.y > (float)height)
		{
			zero.y = (float)height - vector2.y;
		}
		if (zero.sqrMagnitude > 0f)
		{
			t.localPosition += new Vector3(zero.x, zero.y, 0f);
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0001C4AD File Offset: 0x0001A6AD
	public static void FitOnScreen(this Camera cam, Transform transform, Vector3 pos)
	{
		cam.FitOnScreen(transform, transform, pos, false);
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0001C4BC File Offset: 0x0001A6BC
	public static void FitOnScreen(this Camera cam, Transform transform, Transform content, Vector3 pos, bool considerInactive = false)
	{
		Bounds bounds;
		cam.FitOnScreen(transform, content, pos, out bounds, considerInactive);
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0001C4D8 File Offset: 0x0001A6D8
	public static void FitOnScreen(this Camera cam, Transform transform, Transform content, Vector3 pos, out Bounds bounds, bool considerInactive = false)
	{
		bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, content, considerInactive, true);
		Vector3 min = bounds.min;
		Vector3 vector = bounds.max;
		Vector3 size = bounds.size;
		size.x += min.x;
		size.y -= vector.y;
		if (cam != null)
		{
			pos.x = Mathf.Clamp01(pos.x / (float)Screen.width);
			pos.y = Mathf.Clamp01(pos.y / (float)Screen.height);
			float num = cam.orthographicSize / transform.parent.lossyScale.y;
			float num2 = (float)Screen.height * 0.5f / num;
			vector = new Vector2(num2 * size.x / (float)Screen.width, num2 * size.y / (float)Screen.height);
			pos.x = Mathf.Min(pos.x, 1f - vector.x);
			pos.y = Mathf.Max(pos.y, vector.y);
			transform.position = cam.ViewportToWorldPoint(pos);
			pos = transform.localPosition;
			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round(pos.y);
		}
		else
		{
			if (pos.x + size.x > (float)Screen.width)
			{
				pos.x = (float)Screen.width - size.x;
			}
			if (pos.y - size.y < 0f)
			{
				pos.y = size.y;
			}
			pos.x -= (float)Screen.width * 0.5f;
			pos.y -= (float)Screen.height * 0.5f;
		}
		transform.localPosition = pos;
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
	public static bool Save(string fileName, byte[] bytes)
	{
		if (!NGUITools.fileAccess)
		{
			return false;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (bytes == null)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(path);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.Message);
			return false;
		}
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Close();
		return true;
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0001C72C File Offset: 0x0001A92C
	public static byte[] Load(string fileName)
	{
		if (!NGUITools.fileAccess)
		{
			return null;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (File.Exists(path))
		{
			return File.ReadAllBytes(path);
		}
		return null;
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0001C764 File Offset: 0x0001A964
	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001C7B4 File Offset: 0x0001A9B4
	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			componentsInChildren[i].ParentHasChanged();
			i++;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001C7E0 File Offset: 0x0001A9E0
	// (set) Token: 0x060003EE RID: 1006 RVA: 0x0001C7E7 File Offset: 0x0001A9E7
	public static string clipboard
	{
		get
		{
			return GUIUtility.systemCopyBuffer;
		}
		set
		{
			GUIUtility.systemCopyBuffer = value;
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x000178CB File Offset: 0x00015ACB
	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x00017791 File Offset: 0x00015991
	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001C7EF File Offset: 0x0001A9EF
	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001C7F8 File Offset: 0x0001A9F8
	public static T AddMissingComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001C822 File Offset: 0x0001AA22
	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001C841 File Offset: 0x0001AA41
	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0001C84B File Offset: 0x0001AA4B
	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001C86C File Offset: 0x0001AA6C
	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.orthographic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float y = -orthographicSize;
			float y2 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 screenSize = NGUITools.screenSize;
			float num3 = screenSize.x / screenSize.y;
			num3 *= rect.width / rect.height;
			num *= num3;
			num2 *= num3;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			int num4 = Mathf.RoundToInt(screenSize.x);
			int num5 = Mathf.RoundToInt(screenSize.y);
			if ((num4 & 1) == 1)
			{
				position.x -= 1f / screenSize.x;
			}
			if ((num5 & 1) == 1)
			{
				position.y += 1f / screenSize.y;
			}
			NGUITools.mSides[0] = rotation * new Vector3(num, 0f, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(0f, y2, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, 0f, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(0f, y, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0001CA98 File Offset: 0x0001AC98
	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		float depth = Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f);
		return cam.GetWorldCorners(depth, null);
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0001CAC4 File Offset: 0x0001ACC4
	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0001CACE File Offset: 0x0001ACCE
	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0001CAF0 File Offset: 0x0001ACF0
	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.orthographic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float y = -orthographicSize;
			float y2 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 screenSize = NGUITools.screenSize;
			float num3 = screenSize.x / screenSize.y;
			num3 *= rect.width / rect.height;
			num *= num3;
			num2 *= num3;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			NGUITools.mSides[0] = rotation * new Vector3(num, y, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(num, y2, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, y2, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(num2, y, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0001CCB4 File Offset: 0x0001AEB4
	public static string GetFuncName(object obj, string method)
	{
		if (obj == null)
		{
			return "<null>";
		}
		string text = obj.GetType().ToString();
		int num = text.LastIndexOf('/');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(method))
		{
			return text + "/" + method;
		}
		return text;
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0001CD04 File Offset: 0x0001AF04
	public static void Execute<T>(GameObject go, string funcName) where T : Component
	{
		foreach (T t in go.GetComponents<T>())
		{
			MethodInfo method = t.GetType().GetMethod(funcName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null)
			{
				method.Invoke(t, null);
			}
		}
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0001CD5C File Offset: 0x0001AF5C
	public static void ExecuteAll<T>(GameObject root, string funcName) where T : Component
	{
		NGUITools.Execute<T>(root, funcName);
		Transform transform = root.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			NGUITools.ExecuteAll<T>(transform.GetChild(i).gameObject, funcName);
			i++;
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0001CD9C File Offset: 0x0001AF9C
	public static void ImmediatelyCreateDrawCalls(GameObject root)
	{
		NGUITools.ExecuteAll<UIWidget>(root, "Start");
		NGUITools.ExecuteAll<UIPanel>(root, "Start");
		NGUITools.ExecuteAll<UIWidget>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "LateUpdate");
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001CDD5 File Offset: 0x0001AFD5
	public static Vector2 screenSize
	{
		get
		{
			return new Vector2((float)Screen.width, (float)Screen.height);
		}
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0001CDE8 File Offset: 0x0001AFE8
	public static string KeyToCaption(KeyCode key)
	{
		switch (key)
		{
		case KeyCode.None:
			return null;
		case (KeyCode)1:
		case (KeyCode)2:
		case (KeyCode)3:
		case (KeyCode)4:
		case (KeyCode)5:
		case (KeyCode)6:
		case (KeyCode)7:
		case (KeyCode)10:
		case (KeyCode)11:
		case (KeyCode)14:
		case (KeyCode)15:
		case (KeyCode)16:
		case (KeyCode)17:
		case (KeyCode)18:
		case (KeyCode)20:
		case (KeyCode)21:
		case (KeyCode)22:
		case (KeyCode)23:
		case (KeyCode)24:
		case (KeyCode)25:
		case (KeyCode)26:
		case (KeyCode)28:
		case (KeyCode)29:
		case (KeyCode)30:
		case (KeyCode)31:
		case KeyCode.Percent:
		case (KeyCode)65:
		case (KeyCode)66:
		case (KeyCode)67:
		case (KeyCode)68:
		case (KeyCode)69:
		case (KeyCode)70:
		case (KeyCode)71:
		case (KeyCode)72:
		case (KeyCode)73:
		case (KeyCode)74:
		case (KeyCode)75:
		case (KeyCode)76:
		case (KeyCode)77:
		case (KeyCode)78:
		case (KeyCode)79:
		case (KeyCode)80:
		case (KeyCode)81:
		case (KeyCode)82:
		case (KeyCode)83:
		case (KeyCode)84:
		case (KeyCode)85:
		case (KeyCode)86:
		case (KeyCode)87:
		case (KeyCode)88:
		case (KeyCode)89:
		case (KeyCode)90:
		case KeyCode.LeftCurlyBracket:
		case KeyCode.Pipe:
		case KeyCode.RightCurlyBracket:
		case KeyCode.Tilde:
			break;
		case KeyCode.Backspace:
			return "BS";
		case KeyCode.Tab:
			return "Tab";
		case KeyCode.Clear:
			return "Clr";
		case KeyCode.Return:
			return "NT";
		case KeyCode.Pause:
			return "PS";
		case KeyCode.Escape:
			return "Esc";
		case KeyCode.Space:
			return "SP";
		case KeyCode.Exclaim:
			return "!";
		case KeyCode.DoubleQuote:
			return "\"";
		case KeyCode.Hash:
			return "#";
		case KeyCode.Dollar:
			return "$";
		case KeyCode.Ampersand:
			return "&";
		case KeyCode.Quote:
			return "'";
		case KeyCode.LeftParen:
			return "(";
		case KeyCode.RightParen:
			return ")";
		case KeyCode.Asterisk:
			return "*";
		case KeyCode.Plus:
			return "+";
		case KeyCode.Comma:
			return ",";
		case KeyCode.Minus:
			return "-";
		case KeyCode.Period:
			return ".";
		case KeyCode.Slash:
			return "/";
		case KeyCode.Alpha0:
			return "0";
		case KeyCode.Alpha1:
			return "1";
		case KeyCode.Alpha2:
			return "2";
		case KeyCode.Alpha3:
			return "3";
		case KeyCode.Alpha4:
			return "4";
		case KeyCode.Alpha5:
			return "5";
		case KeyCode.Alpha6:
			return "6";
		case KeyCode.Alpha7:
			return "7";
		case KeyCode.Alpha8:
			return "8";
		case KeyCode.Alpha9:
			return "9";
		case KeyCode.Colon:
			return ":";
		case KeyCode.Semicolon:
			return ";";
		case KeyCode.Less:
			return "<";
		case KeyCode.Equals:
			return "=";
		case KeyCode.Greater:
			return ">";
		case KeyCode.Question:
			return "?";
		case KeyCode.At:
			return "@";
		case KeyCode.LeftBracket:
			return "[";
		case KeyCode.Backslash:
			return "\\";
		case KeyCode.RightBracket:
			return "]";
		case KeyCode.Caret:
			return "^";
		case KeyCode.Underscore:
			return "_";
		case KeyCode.BackQuote:
			return "`";
		case KeyCode.A:
			return "A";
		case KeyCode.B:
			return "B";
		case KeyCode.C:
			return "C";
		case KeyCode.D:
			return "D";
		case KeyCode.E:
			return "E";
		case KeyCode.F:
			return "F";
		case KeyCode.G:
			return "G";
		case KeyCode.H:
			return "H";
		case KeyCode.I:
			return "I";
		case KeyCode.J:
			return "J";
		case KeyCode.K:
			return "K";
		case KeyCode.L:
			return "L";
		case KeyCode.M:
			return "M";
		case KeyCode.N:
			return "N0";
		case KeyCode.O:
			return "O";
		case KeyCode.P:
			return "P";
		case KeyCode.Q:
			return "Q";
		case KeyCode.R:
			return "R";
		case KeyCode.S:
			return "S";
		case KeyCode.T:
			return "T";
		case KeyCode.U:
			return "U";
		case KeyCode.V:
			return "V";
		case KeyCode.W:
			return "W";
		case KeyCode.X:
			return "X";
		case KeyCode.Y:
			return "Y";
		case KeyCode.Z:
			return "Z";
		case KeyCode.Delete:
			return "Del";
		default:
			switch (key)
			{
			case KeyCode.Keypad0:
				return "K0";
			case KeyCode.Keypad1:
				return "K1";
			case KeyCode.Keypad2:
				return "K2";
			case KeyCode.Keypad3:
				return "K3";
			case KeyCode.Keypad4:
				return "K4";
			case KeyCode.Keypad5:
				return "K5";
			case KeyCode.Keypad6:
				return "K6";
			case KeyCode.Keypad7:
				return "K7";
			case KeyCode.Keypad8:
				return "K8";
			case KeyCode.Keypad9:
				return "K9";
			case KeyCode.KeypadPeriod:
				return ".";
			case KeyCode.KeypadDivide:
				return "/";
			case KeyCode.KeypadMultiply:
				return "*";
			case KeyCode.KeypadMinus:
				return "-";
			case KeyCode.KeypadPlus:
				return "+";
			case KeyCode.KeypadEnter:
				return "NT";
			case KeyCode.KeypadEquals:
				return "=";
			case KeyCode.UpArrow:
				return "UP";
			case KeyCode.DownArrow:
				return "DN";
			case KeyCode.RightArrow:
				return "LT";
			case KeyCode.LeftArrow:
				return "RT";
			case KeyCode.Insert:
				return "Ins";
			case KeyCode.Home:
				return "Home";
			case KeyCode.End:
				return "End";
			case KeyCode.PageUp:
				return "PU";
			case KeyCode.PageDown:
				return "PD";
			case KeyCode.F1:
				return "F1";
			case KeyCode.F2:
				return "F2";
			case KeyCode.F3:
				return "F3";
			case KeyCode.F4:
				return "F4";
			case KeyCode.F5:
				return "F5";
			case KeyCode.F6:
				return "F6";
			case KeyCode.F7:
				return "F7";
			case KeyCode.F8:
				return "F8";
			case KeyCode.F9:
				return "F9";
			case KeyCode.F10:
				return "F10";
			case KeyCode.F11:
				return "F11";
			case KeyCode.F12:
				return "F12";
			case KeyCode.F13:
				return "F13";
			case KeyCode.F14:
				return "F14";
			case KeyCode.F15:
				return "F15";
			case KeyCode.Numlock:
				return "Num";
			case KeyCode.CapsLock:
				return "Cap";
			case KeyCode.ScrollLock:
				return "Scr";
			case KeyCode.RightShift:
				return "RS";
			case KeyCode.LeftShift:
				return "LS";
			case KeyCode.RightControl:
				return "RC";
			case KeyCode.LeftControl:
				return "LC";
			case KeyCode.RightAlt:
				return "RA";
			case KeyCode.LeftAlt:
				return "LA";
			case KeyCode.Mouse0:
				return "M0";
			case KeyCode.Mouse1:
				return "M1";
			case KeyCode.Mouse2:
				return "M2";
			case KeyCode.Mouse3:
				return "M3";
			case KeyCode.Mouse4:
				return "M4";
			case KeyCode.Mouse5:
				return "M5";
			case KeyCode.Mouse6:
				return "M6";
			case KeyCode.JoystickButton0:
				return "(A)";
			case KeyCode.JoystickButton1:
				return "(B)";
			case KeyCode.JoystickButton2:
				return "(X)";
			case KeyCode.JoystickButton3:
				return "(Y)";
			case KeyCode.JoystickButton4:
				return "(RB)";
			case KeyCode.JoystickButton5:
				return "(LB)";
			case KeyCode.JoystickButton6:
				return "(Back)";
			case KeyCode.JoystickButton7:
				return "(Start)";
			case KeyCode.JoystickButton8:
				return "(LS)";
			case KeyCode.JoystickButton9:
				return "(RS)";
			case KeyCode.JoystickButton10:
				return "J10";
			case KeyCode.JoystickButton11:
				return "J11";
			case KeyCode.JoystickButton12:
				return "J12";
			case KeyCode.JoystickButton13:
				return "J13";
			case KeyCode.JoystickButton14:
				return "J14";
			case KeyCode.JoystickButton15:
				return "J15";
			case KeyCode.JoystickButton16:
				return "J16";
			case KeyCode.JoystickButton17:
				return "J17";
			case KeyCode.JoystickButton18:
				return "J18";
			case KeyCode.JoystickButton19:
				return "J19";
			}
			break;
		}
		return null;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001D500 File Offset: 0x0001B700
	public static T Draw<T>(string id, NGUITools.OnInitFunc<T> onInit = null) where T : UIWidget
	{
		UIWidget uiwidget;
		if (NGUITools.mWidgets.TryGetValue(id, out uiwidget) && uiwidget)
		{
			return (!!0)((object)uiwidget);
		}
		if (NGUITools.mRoot == null)
		{
			UICamera x = null;
			UIRoot uiroot = null;
			for (int i = 0; i < UIRoot.list.Count; i++)
			{
				UIRoot uiroot2 = UIRoot.list[i];
				if (uiroot2)
				{
					UICamera uicamera = UICamera.FindCameraForLayer(uiroot2.gameObject.layer);
					if (uicamera && uicamera.cachedCamera.orthographic)
					{
						x = uicamera;
						uiroot = uiroot2;
						break;
					}
				}
			}
			if (x == null)
			{
				NGUITools.mRoot = NGUITools.CreateUI(false, LayerMask.NameToLayer("UI"));
			}
			else
			{
				NGUITools.mRoot = uiroot.gameObject.AddChild<UIPanel>();
			}
			NGUITools.mRoot.depth = 100000;
			NGUITools.mGo = NGUITools.mRoot.gameObject;
			NGUITools.mGo.name = "Immediate Mode GUI";
		}
		uiwidget = NGUITools.mGo.AddWidget(int.MaxValue);
		uiwidget.name = id;
		NGUITools.mWidgets[id] = uiwidget;
		if (onInit != null)
		{
			onInit((!!0)((object)uiwidget));
		}
		return (!!0)((object)uiwidget);
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0001D638 File Offset: 0x0001B838
	public static Color GammaToLinearSpace(this Color c)
	{
		if (NGUITools.mColorSpace == ColorSpace.Uninitialized)
		{
			NGUITools.mColorSpace = QualitySettings.activeColorSpace;
		}
		if (NGUITools.mColorSpace == ColorSpace.Linear)
		{
			return new Color(Mathf.GammaToLinearSpace(c.r), Mathf.GammaToLinearSpace(c.g), Mathf.GammaToLinearSpace(c.b), Mathf.GammaToLinearSpace(c.a));
		}
		return c;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0001D694 File Offset: 0x0001B894
	// Note: this type is marked as 'beforefieldinit'.
	static NGUITools()
	{
		KeyCode[] array = new KeyCode[145];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.7FB9790B49277F6151D3EB5D555CCF105904DB43).FieldHandle);
		NGUITools.keys = array;
		NGUITools.mWidgets = new Dictionary<string, UIWidget>();
		NGUITools.mColorSpace = ColorSpace.Uninitialized;
	}

	// Token: 0x040002CF RID: 719
	[NonSerialized]
	private static AudioListener mListener;

	// Token: 0x040002D0 RID: 720
	[NonSerialized]
	public static AudioSource audioSource;

	// Token: 0x040002D1 RID: 721
	private static bool mLoaded = false;

	// Token: 0x040002D2 RID: 722
	private static float mGlobalVolume = 1f;

	// Token: 0x040002D3 RID: 723
	private static float mLastTimestamp = 0f;

	// Token: 0x040002D4 RID: 724
	private static AudioClip mLastClip;

	// Token: 0x040002D5 RID: 725
	private static Dictionary<Type, string> mTypeNames = new Dictionary<Type, string>();

	// Token: 0x040002D6 RID: 726
	private static Vector3[] mSides = new Vector3[4];

	// Token: 0x040002D7 RID: 727
	public static KeyCode[] keys;

	// Token: 0x040002D8 RID: 728
	private static Dictionary<string, UIWidget> mWidgets;

	// Token: 0x040002D9 RID: 729
	private static UIPanel mRoot;

	// Token: 0x040002DA RID: 730
	private static GameObject mGo;

	// Token: 0x040002DB RID: 731
	private static ColorSpace mColorSpace;

	// Token: 0x0200052C RID: 1324
	// (Invoke) Token: 0x06003794 RID: 14228
	public delegate void OnInitFunc<T>(T w) where T : UIWidget;
}
