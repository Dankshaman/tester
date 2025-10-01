using System;
using System.Collections.Generic;
using mset;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200026F RID: 623
public class NGUIHelper : MonoBehaviour
{
	// Token: 0x060020CF RID: 8399 RVA: 0x000ED81C File Offset: 0x000EBA1C
	public static void RemoveClickEventListener(GameObject gameObject, UIEventListener.VoidDelegate listener)
	{
		if (gameObject == null)
		{
			return;
		}
		UIEventListener listener2 = NGUIHelper.GetListener(gameObject);
		if (listener2 != null)
		{
			UIEventListener uieventListener = listener2;
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, listener);
		}
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x000ED85C File Offset: 0x000EBA5C
	public static void RemoveDragEventListener(GameObject gameObject, UIEventListener.VectorDelegate listener)
	{
		if (gameObject == null)
		{
			return;
		}
		UIEventListener listener2 = NGUIHelper.GetListener(gameObject);
		if (listener2 != null)
		{
			UIEventListener uieventListener = listener2;
			uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener.onDrag, listener);
		}
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x000ED89C File Offset: 0x000EBA9C
	public static void RemovePressEventListener(GameObject gameObject, UIEventListener.BoolDelegate listener)
	{
		if (gameObject == null)
		{
			return;
		}
		UIEventListener listener2 = NGUIHelper.GetListener(gameObject);
		if (listener2 != null)
		{
			UIEventListener uieventListener = listener2;
			uieventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onPress, listener);
		}
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x000ED8DC File Offset: 0x000EBADC
	public static void RemoveSubmitEventListener(GameObject gameObject, UIEventListener.VoidDelegate listener)
	{
		if (gameObject == null)
		{
			return;
		}
		UIEventListener listener2 = NGUIHelper.GetListener(gameObject);
		if (listener2 != null)
		{
			UIEventListener uieventListener = listener2;
			uieventListener.onSubmit = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onSubmit, listener);
		}
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x000ED91A File Offset: 0x000EBB1A
	public static void EnableColorTween(GameObject go, bool enable)
	{
		go.GetComponent<TweenColor>().enabled = enable;
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x000ED928 File Offset: 0x000EBB28
	public static void DisableColorTweens(GameObject go)
	{
		TweenColor[] componentsInChildren = go.GetComponentsInChildren<TweenColor>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x000ED954 File Offset: 0x000EBB54
	public static bool ClampAndAddDots(UILabel Label)
	{
		Label.overflowMethod = UILabel.Overflow.ClampContent;
		Label.overflowEllipsis = false;
		Label.overflowEllipsis = true;
		Label.ResetAndUpdateAnchors();
		Label.ProcessText(false, true);
		return Label.processedText.EndsWith("...");
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x000ED98C File Offset: 0x000EBB8C
	public static bool ClampAndAddDots(UILabel Label, GameObject TargetAddTooltip, bool DestroyTooltipIfNotNeeded = true)
	{
		string text = Label.text;
		UITooltipScript uitooltipScript = TargetAddTooltip.GetComponent<UITooltipScript>();
		if (NGUIHelper.ClampAndAddDots(Label))
		{
			if (!uitooltipScript)
			{
				uitooltipScript = TargetAddTooltip.AddComponent<UITooltipScript>();
			}
			uitooltipScript.Tooltip = text;
			uitooltipScript.QuestionMark = false;
			return true;
		}
		if (DestroyTooltipIfNotNeeded)
		{
			UnityEngine.Object.Destroy(uitooltipScript);
		}
		else if (uitooltipScript)
		{
			uitooltipScript.Tooltip = "";
		}
		return false;
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x000ED9F0 File Offset: 0x000EBBF0
	public static void FitGameObjectToUI(GameObject GO, Transform parent, Vector3 offset, Vector3 initialScale, float Size, Vector3? rotationTarget = null, Vector3? altLookAngle = null)
	{
		GO.transform.parent = null;
		GO.transform.localScale = initialScale;
		GO.transform.rotation = Quaternion.identity;
		Transform[] componentsInChildren = GO.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = parent.gameObject.layer;
		}
		Collider[] componentsInChildren2 = GO.GetComponentsInChildren<Collider>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].enabled = false;
		}
		bool flag = GO.GetComponentInChildren<Animation>();
		flag = false;
		Bounds bounds = default(Bounds);
		foreach (Renderer renderer in GO.GetComponentsInChildren<Renderer>(true))
		{
			if (renderer.material != null)
			{
				Sky.SetUniformOcclusion(renderer, 0f, 0.25f);
			}
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			if (bounds == default(Bounds))
			{
				bounds = renderer.bounds;
			}
			else
			{
				bounds.Encapsulate(renderer.bounds);
			}
		}
		if (flag)
		{
			bounds = default(Bounds);
			foreach (Collider collider in componentsInChildren2)
			{
				if (!collider.isTrigger)
				{
					if (bounds == default(Bounds))
					{
						bounds = collider.bounds;
					}
					else
					{
						bounds.Encapsulate(collider.bounds);
					}
				}
			}
		}
		Light[] componentsInChildren4 = GO.GetComponentsInChildren<Light>(true);
		for (int m = 0; m < componentsInChildren4.Length; m++)
		{
			componentsInChildren4[m].cullingMask = 0;
		}
		ParticleSystem[] componentsInChildren5 = GO.GetComponentsInChildren<ParticleSystem>(true);
		for (int n = 0; n < componentsInChildren5.Length; n++)
		{
			componentsInChildren5[n].emission.enabled = false;
		}
		GO.transform.parent = parent;
		GO.transform.rotation = Quaternion.identity;
		bool flag2 = GO.GetComponent<CustomImage>() && GO.GetComponent<CustomImage>().ObjectName == "Figurine";
		bool flag3 = (GO.GetComponent<DeckScript>() && GO.GetComponent<DeckScript>().bSideways) || (GO.GetComponent<CardScript>() && GO.GetComponent<CardScript>().bSideways);
		bool flag4 = GO.CompareTag("Notecard") || GO.CompareTag("Counter");
		bool flag5 = flag2 || GO.CompareTag("Clock");
		float num = bounds.size.z;
		Utilities.Face face = Utilities.GetLargestFace(bounds);
		if (GO.GetComponent<StackObject>() && (!GO.GetComponent<StackObject>().bBag & !GO.GetComponent<StackObject>().IsInfiniteStack))
		{
			face = Utilities.Face.Top;
		}
		switch (face)
		{
		case Utilities.Face.Top:
			GO.transform.Rotate(90f, 180f, 0f);
			if (bounds.size.z > bounds.size.x)
			{
				num = bounds.size.z;
			}
			else
			{
				num = bounds.size.x;
			}
			break;
		case Utilities.Face.Front:
			GO.transform.Rotate(0f, 180f, 0f);
			if (bounds.size.x > bounds.size.y)
			{
				num = bounds.size.x;
			}
			else
			{
				num = bounds.size.y;
			}
			break;
		case Utilities.Face.Side:
			GO.transform.Rotate(0f, 90f, 0f);
			if (bounds.size.z > bounds.size.y)
			{
				num = bounds.size.z;
			}
			else
			{
				num = bounds.size.y;
			}
			break;
		}
		if (altLookAngle != null)
		{
			Vector3? vector = altLookAngle;
			Vector3 zero = Vector3.zero;
			if (vector == null || (vector != null && vector.GetValueOrDefault() != zero))
			{
				GO.transform.rotation *= Quaternion.Euler(altLookAngle.Value);
				goto IL_45A;
			}
		}
		if (flag4)
		{
			GO.transform.Rotate(0f, 180f, 0f);
		}
		if (flag5)
		{
			GO.transform.Rotate(0f, 180f, 0f);
		}
		IL_45A:
		if (flag3)
		{
			GO.transform.Rotate(0f, -90f, 0f);
		}
		if (rotationTarget != null)
		{
			GO.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f)) * Quaternion.Euler(rotationTarget.Value);
		}
		float num2 = Size / num;
		if (!float.IsInfinity(num2))
		{
			GO.transform.localScale = initialScale * num2;
		}
		bounds = default(Bounds);
		Renderer[] componentsInChildren3;
		foreach (Renderer renderer2 in componentsInChildren3)
		{
			if (bounds == default(Bounds))
			{
				bounds = renderer2.bounds;
			}
			else
			{
				bounds.Encapsulate(renderer2.bounds);
			}
		}
		if (flag)
		{
			bounds = default(Bounds);
			foreach (Collider collider2 in componentsInChildren2)
			{
				if (!collider2.isTrigger)
				{
					if (bounds == default(Bounds))
					{
						bounds = collider2.bounds;
					}
					else
					{
						bounds.Encapsulate(collider2.bounds);
					}
				}
			}
		}
		Vector3 vector2 = new Vector3(GO.transform.position.x - bounds.center.x, GO.transform.position.y - bounds.center.y, GO.transform.position.z - bounds.center.z);
		GO.transform.localPosition = new Vector3(offset.x, offset.y, -250f);
		GO.transform.position = new Vector3(GO.transform.position.x + vector2.x, GO.transform.position.y + vector2.y, GO.transform.position.z);
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x000EE047 File Offset: 0x000EC247
	private static UIEventListener GetListener(GameObject go)
	{
		return go.GetComponent<UIEventListener>();
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x000EE050 File Offset: 0x000EC250
	public static TType GetComponentInChildren<TType>(GameObject objRoot) where TType : Component
	{
		TType ttype = objRoot.GetComponent<TType>();
		if (null == ttype)
		{
			Transform transform = objRoot.transform;
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				ttype = NGUIHelper.GetComponentInChildren<TType>(transform.GetChild(i).gameObject);
				if (null != ttype)
				{
					break;
				}
			}
		}
		return ttype;
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x000EE0B0 File Offset: 0x000EC2B0
	public static TType GetTopMostParent<TType>(GameObject objRoot, bool excludeUIRoot) where TType : Component
	{
		TType result = objRoot.GetComponent<TType>();
		Transform parent = objRoot.transform.parent;
		TType ttype = default(!!0);
		while (parent != null)
		{
			TType component = parent.gameObject.GetComponent<TType>();
			if (component != null)
			{
				if (excludeUIRoot)
				{
					if (parent.transform.parent == null)
					{
						result = ttype;
						break;
					}
					ttype = component;
				}
				else
				{
					result = component;
				}
			}
			parent = parent.transform.parent;
		}
		return result;
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000EE12C File Offset: 0x000EC32C
	public static void ButtonDisable(UIButton button, bool disable, Color? defaultColor = null, Color? disableColor = null)
	{
		if (button == null)
		{
			return;
		}
		if (defaultColor == null)
		{
			defaultColor = new Color?(Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal]);
		}
		if (disableColor == null)
		{
			disableColor = new Color?(Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral]);
		}
		button.defaultColor = (disable ? disableColor.Value : defaultColor.Value);
		button.GetComponent<Collider2D>().enabled = !disable;
		button.enabled = !disable;
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000EE1BF File Offset: 0x000EC3BF
	public static void Toggle(GameObject go)
	{
		go.SetActive(!go.activeSelf);
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000EE1D0 File Offset: 0x000EC3D0
	public static int GetIndexRadioButtons(List<UIToggle> radioToggles)
	{
		for (int i = 0; i < radioToggles.Count; i++)
		{
			if (radioToggles[i].enabled)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000EE200 File Offset: 0x000EC400
	public static void SetIndexRadioButtons(List<UIToggle> radioToggles, int index)
	{
		int group = radioToggles[0].group;
		for (int i = 0; i < radioToggles.Count; i++)
		{
			radioToggles[i].group = 0;
			radioToggles[i].value = (i == index);
			radioToggles[i].group = group;
		}
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000EE258 File Offset: 0x000EC458
	public static void BringToFront(UIPanel panel)
	{
		int num = 0;
		foreach (UIPanel uipanel in NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.GetComponentsInChildren<UIPanel>())
		{
			if (uipanel != panel && uipanel.depth > num)
			{
				num = uipanel.depth;
			}
		}
		panel.depth = num + 1;
	}
}
