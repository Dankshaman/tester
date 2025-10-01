using System;
using UnityEngine;

// Token: 0x02000286 RID: 646
public class UICameraStates : MonoBehaviour
{
	// Token: 0x0600215E RID: 8542 RVA: 0x000F0894 File Offset: 0x000EEA94
	private void Awake()
	{
		this.Slot = int.Parse(base.GetComponentInChildren<UILabel>().text);
		this.cameraController = Singleton<CameraController>.Instance;
		this.uiLabel = base.GetComponentInChildren<UILabel>();
		UITooltipScript uitooltipScript = base.gameObject.AddComponent<UITooltipScript>();
		if (this.bLoad)
		{
			uitooltipScript.Tooltip = "<Shift> + " + this.Slot;
		}
		else
		{
			uitooltipScript.Tooltip = "<Ctrl> + " + this.Slot;
		}
		uitooltipScript.QuestionMark = false;
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x000F0924 File Offset: 0x000EEB24
	private void Update()
	{
		bool flag = this.cameraController.CameraStateInUse(this.Slot);
		if (this.bLoad)
		{
			if (flag)
			{
				this.uiLabel.ThemeAs = UIPalette.UI.ContextMenuText;
				this.uiLabel.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuText];
				return;
			}
			this.uiLabel.ThemeAs = UIPalette.UI.ButtonNeutral;
			this.uiLabel.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			return;
		}
		else
		{
			if (flag)
			{
				this.uiLabel.ThemeAs = UIPalette.UI.ButtonHighlightA;
				this.uiLabel.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonHighlightA];
				return;
			}
			this.uiLabel.ThemeAs = UIPalette.UI.ContextMenuText;
			this.uiLabel.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuText];
			return;
		}
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x000F0A08 File Offset: 0x000EEC08
	private void OnClick()
	{
		if (this.bLoad)
		{
			this.cameraController.LoadCamera(this.Slot);
			return;
		}
		this.cameraController.SaveCamera(this.Slot, true);
	}

	// Token: 0x040014AA RID: 5290
	public bool bLoad = true;

	// Token: 0x040014AB RID: 5291
	[SerializeField]
	private int Slot;

	// Token: 0x040014AC RID: 5292
	private CameraController cameraController;

	// Token: 0x040014AD RID: 5293
	private UILabel uiLabel;
}
