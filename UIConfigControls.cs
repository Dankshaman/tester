using System;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class UIConfigControls : MonoBehaviour, INotifySceneAwake
{
	// Token: 0x060021BF RID: 8639 RVA: 0x000F3368 File Offset: 0x000F1568
	private void OnEnable()
	{
		this.ControllerToggle.value = zInput.bController;
		this.TouchToggle.value = zInput.bTouch;
		this.OnLanguageChange("", "");
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x000F339C File Offset: 0x000F159C
	private void Start()
	{
		this.Initialize();
		EventDelegate.Add(this.ControllerToggle.onChange, new EventDelegate.Callback(this.ControllerClick));
		EventDelegate.Add(this.TouchToggle.onChange, new EventDelegate.Callback(this.TouchClick));
		EventManager.OnLanguageChange += this.OnLanguageChange;
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x000F33FC File Offset: 0x000F15FC
	private void OnDestroy()
	{
		EventDelegate.Remove(this.ControllerToggle.onChange, new EventDelegate.Callback(this.ControllerClick));
		EventDelegate.Remove(this.TouchToggle.onChange, new EventDelegate.Callback(this.TouchClick));
		EventManager.OnLanguageChange -= this.OnLanguageChange;
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x000F3454 File Offset: 0x000F1654
	private void ControllerClick()
	{
		zInput.bController = this.ControllerToggle.value;
		PlayerPrefs.SetString("bController3", zInput.bController.ToString());
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000F347C File Offset: 0x000F167C
	private void TouchClick()
	{
		zInput.bTouch = this.TouchToggle.value;
		PlayerPrefs.SetString("bTouch4", zInput.bTouch.ToString());
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x000F34B0 File Offset: 0x000F16B0
	private void Initialize()
	{
		this.Grid = base.transform.GetChild(0).gameObject;
		TTSUtilities.DestroyChildren(this.Grid.transform);
		for (int i = 0; i < cInput.length; i++)
		{
			string text = cInput.GetText(i);
			if (!text.Contains("(Controller)") && !text.StartsWith("user:"))
			{
				GameObject gameObject = this.Grid.AddChild(this.ControlButton);
				gameObject.name = cInput.GetText(i);
				this.SetupButton(gameObject, i);
			}
		}
		this.Grid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x000F354C File Offset: 0x000F174C
	private void SetupButton(GameObject Button, int i)
	{
		UILabel[] componentsInChildren = Button.GetComponentsInChildren<UILabel>();
		componentsInChildren[0].text = Language.Translate(cInput.GetText(i));
		componentsInChildren[1].text = cInput.GetText(i, 1);
		componentsInChildren[2].text = cInput.GetText(i, 2);
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x000F3584 File Offset: 0x000F1784
	private void Update()
	{
		if (this.StartScan && !cInput.scanning)
		{
			this.StartScan = false;
			this.Refresh();
		}
		if (cInput.scanning)
		{
			this.StartScan = true;
		}
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x000F35B0 File Offset: 0x000F17B0
	public void Reset()
	{
		Chat.Log("Controls have been reset to default.", ChatMessageType.Game);
		cInput.ResetInputs();
		this.Refresh();
		zInput.bController = false;
		zInput.bTouch = false;
		this.ControllerToggle.value = zInput.bController;
		this.TouchToggle.value = zInput.bTouch;
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x000F3600 File Offset: 0x000F1800
	private void Refresh()
	{
		if (!this.Grid)
		{
			this.Initialize();
		}
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			this.SetupButton(this.Grid.transform.GetChild(i).gameObject, i);
		}
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x000F3658 File Offset: 0x000F1858
	private void OnLanguageChange(string oldCode = "", string newCode = "")
	{
		if (this.Grid == null || this.Grid.transform == null || this.Grid.transform.childCount == 0)
		{
			return;
		}
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			this.SetupButton(this.Grid.transform.GetChild(i).gameObject, i);
		}
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x000F36D4 File Offset: 0x000F18D4
	public void SceneAwake()
	{
		if (PlayerPrefs.HasKey("bController3"))
		{
			zInput.bController = bool.Parse(PlayerPrefs.GetString("bController3"));
		}
		if (PlayerPrefs.HasKey("bTouch4"))
		{
			zInput.bTouch = bool.Parse(PlayerPrefs.GetString("bTouch4"));
		}
	}

	// Token: 0x04001519 RID: 5401
	public GameObject ControlButton;

	// Token: 0x0400151A RID: 5402
	public UIToggle ControllerToggle;

	// Token: 0x0400151B RID: 5403
	public UIToggle TouchToggle;

	// Token: 0x0400151C RID: 5404
	private bool StartScan;

	// Token: 0x0400151D RID: 5405
	private GameObject Grid;

	// Token: 0x0400151E RID: 5406
	public const string CONTROLLER_PLAYER_PREFS = "bController3";

	// Token: 0x0400151F RID: 5407
	public const string TOUCH_PLAYER_PREFS = "bTouch4";
}
