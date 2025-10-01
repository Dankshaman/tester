using System;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class UIHoverText : MonoBehaviour
{
	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06002463 RID: 9315 RVA: 0x00100C30 File Offset: 0x000FEE30
	// (set) Token: 0x06002464 RID: 9316 RVA: 0x00100C38 File Offset: 0x000FEE38
	public static string text
	{
		get
		{
			return UIHoverText.text_;
		}
		set
		{
			if (UIHoverText.text_ != value)
			{
				UIHoverText.ThisUILabel.text = value;
				UIHoverText.text_ = value;
				if (VRHMD.isVR)
				{
					if (VRTrackedController.leftVRTrackedController)
					{
						VRTrackedController.leftVRTrackedController.SetHoverText(value);
					}
					if (VRTrackedController.rightVRTrackedController)
					{
						VRTrackedController.rightVRTrackedController.SetHoverText(value);
					}
				}
				UIHoverText.ThisUILabel.ProcessText(false, true);
				UIHoverText.UpdateAnchors();
				UIHoverText.Background.SetActive(!string.IsNullOrEmpty(value));
				if (!string.IsNullOrEmpty(UIHoverText.text_))
				{
					UIHoverText.UpdatePosition();
				}
			}
		}
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x00100CD0 File Offset: 0x000FEED0
	private static void UpdateAnchors()
	{
		for (int i = 0; i < UIHoverText.Widgets.Length; i++)
		{
			UIHoverText.Widgets[i].UpdateAnchors();
		}
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x00100CFB File Offset: 0x000FEEFB
	private void Start()
	{
		UIHoverText.ThisUILabel = base.GetComponent<UILabel>();
		UIHoverText.Background = base.transform.GetChild(0).gameObject;
		UIHoverText.Widgets = UIHoverText.Background.GetComponentsInChildren<UIWidget>(true);
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x00100D2E File Offset: 0x000FEF2E
	private void Update()
	{
		if (!string.IsNullOrEmpty(UIHoverText.text_) || UIHoverIcon.icon != HoverIcons.None)
		{
			UIHoverText.UpdatePosition();
		}
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x00100D48 File Offset: 0x000FEF48
	public static void UpdatePosition()
	{
		Vector2 lastEventPosition = UICamera.lastEventPosition;
		if (UIHoverText.BottomRight)
		{
			lastEventPosition.x += 80f;
			lastEventPosition.y -= 32f;
		}
		else if (UIHoverIcon.icon == HoverIcons.Layout)
		{
			lastEventPosition.x += 38f;
			lastEventPosition.y -= 53f;
		}
		Vector3 vector = new Vector3(lastEventPosition.x / (float)Screen.width, (lastEventPosition.y + 8f) / (float)Screen.height, 0f);
		if (VRHMD.isVR && UICamera.HoveredUIObject)
		{
			vector = UICamera.mainCamera.WorldToViewportPoint(UICamera.HoveredUIObject.transform.position);
		}
		vector = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector, UIHoverText.ThisUILabel.localSize.x / 2f + 5f, UIHoverText.ThisUILabel.localSize.y + 5f, SpriteAlignment.TopCenter, true);
		Vector3 vector2 = UICamera.mainCamera.ViewportToWorldPoint(vector);
		vector2 = new Vector3(vector2.x, vector2.y, -9f);
		UIHoverText.ThisUILabel.transform.position = vector2;
		UIHoverText.ThisUILabel.transform.RoundLocalPosition();
	}

	// Token: 0x0400176A RID: 5994
	private static UILabel ThisUILabel;

	// Token: 0x0400176B RID: 5995
	private static GameObject Background;

	// Token: 0x0400176C RID: 5996
	private static UIWidget[] Widgets;

	// Token: 0x0400176D RID: 5997
	public static float UIDelayTooltipTime = 1.2f;

	// Token: 0x0400176E RID: 5998
	public static float NPODelayTooltipTime = 1.2f;

	// Token: 0x0400176F RID: 5999
	public const string DelayTooltipSpacer = "\n----------\n";

	// Token: 0x04001770 RID: 6000
	private static string text_ = "";

	// Token: 0x04001771 RID: 6001
	public static bool BottomRight = false;
}
