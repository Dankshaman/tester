using System;
using UnityEngine;

// Token: 0x0200033B RID: 827
public class UISettingsTemplate : MonoBehaviour
{
	// Token: 0x0600274A RID: 10058 RVA: 0x001179EC File Offset: 0x00115BEC
	public void Start()
	{
		Transform transform = GameObject.Find("/UI Root/### UNIVERSAL/Settings Template").transform;
		this.PanelTemplate = transform.Find("Panel");
		this.UI = UnityEngine.Object.Instantiate<GameObject>(this.PanelTemplate.gameObject);
		this.UI.SetActive(true);
		Transform transform2 = this.UI.transform;
		transform2.parent = base.transform;
		transform2 = transform2.GetChild(0);
		this.UI = transform2.gameObject;
		this.ScrollView = transform2.Find("Controls/View").GetComponent<UIScrollView>();
		this.Grid = transform2.Find("Controls/View/Grid");
		this.Scrollbar = transform2.Find("Controls/Scroll Bar").gameObject;
		this.TitleLabel = transform2.Find("Title").GetComponent<UILabel>();
		this.ResetButton = transform2.Find("Reset Button").gameObject;
		Transform transform3 = transform.Find("Row Templates");
		this.ButtonTemplate = transform3.Find("Button Row Template").gameObject;
		this.CheckboxTemplate = transform3.Find("Checkbox Row Template").gameObject;
		this.SliderTemplate = transform3.Find("Slider Row Template").gameObject;
		this.StringTemplate = transform3.Find("String Row Template").gameObject;
		this.Line = transform3.Find("Line Template").gameObject;
		this.DummyLine = transform3.Find("Dummy Line Template").gameObject;
		if (this.ShowResetButton)
		{
			this.ResetButton.SetActive(true);
			EventDelegate.Add(this.ResetButton.GetComponent<UIButton>().onClick, delegate()
			{
				this.ResetSettings();
			});
		}
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x00117B98 File Offset: 0x00115D98
	public void Display()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.UI.SetActive(true);
		this.Scrollbar.GetComponent<UIScrollBar>().value = 0f;
		Wait.Frames(delegate
		{
			this.UpdateDisplayedValues();
			this.ScrollView.UpdateScrollbars(true);
			this.ScrollView.ResetPosition();
			this.ScrollView.UpdatePosition();
		}, 1);
		Wait.Frames(delegate
		{
			this.Scrollbar.GetComponent<UIScrollBar>().value = 0f;
			this.ScrollView.ResetPosition();
			this.ScrollView.UpdatePosition();
			this.ScrollView.UpdateScrollbars();
		}, 2);
		Wait.Frames(delegate
		{
			this.Scrollbar.GetComponent<UIScrollBar>().value = 0.5f;
			this.ScrollView.ResetPosition();
			this.ScrollView.UpdatePosition();
			this.ScrollView.UpdateScrollbars();
		}, 3);
		Wait.Frames(delegate
		{
			this.Scrollbar.GetComponent<UIScrollBar>().value = 0f;
			this.ScrollView.ResetPosition();
			this.ScrollView.UpdatePosition();
			this.ScrollView.UpdateScrollbars();
		}, 4);
	}

	// Token: 0x0600274C RID: 10060 RVA: 0x00117C20 File Offset: 0x00115E20
	private void Initialize()
	{
		this.initialized = true;
		this.TitleLabel.text = this.Title;
		this.AddSettings();
		this.Grid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x000025B8 File Offset: 0x000007B8
	public virtual void AddSettings()
	{
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x00117C54 File Offset: 0x00115E54
	private void UpdateDisplayedValues()
	{
		foreach (UICommandAction uicommandAction in this.Grid.GetComponentsInChildren<UICommandAction>())
		{
			if (uicommandAction.button == null || uicommandAction.buttonID == 0)
			{
				uicommandAction.UpdateDisplayedValue(null, true);
			}
		}
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x00117CA0 File Offset: 0x00115EA0
	public void ResetSettings()
	{
		foreach (UICommandAction uicommandAction in this.Grid.GetComponentsInChildren<UICommandAction>())
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("reset " + uicommandAction.command, true, SystemConsole.CommandEcho.Silent);
		}
		this.UpdateDisplayedValues();
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x00117CED File Offset: 0x00115EED
	private static string labelFromCommand(string command)
	{
		if (command.StartsWith("vr_"))
		{
			command = command.Substring(3);
		}
		return LibString.CamelCaseFromUnderscore(command, true, true);
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x00117D0D File Offset: 0x00115F0D
	public void NextGroup()
	{
		this.nextLineVisible = true;
		this.inGroup = true;
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x00117D1D File Offset: 0x00115F1D
	public void IndividualEntries()
	{
		this.nextLineVisible = true;
		this.inGroup = false;
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x00117D30 File Offset: 0x00115F30
	private void AddLine()
	{
		GameObject gameObject;
		if (this.nextLineVisible)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Line);
			if (this.inGroup)
			{
				this.nextLineVisible = false;
			}
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.DummyLine);
		}
		gameObject.transform.parent = this.Grid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UIWidget component = gameObject.GetComponent<UIWidget>();
		component.leftAnchor.target = this.Grid.parent;
		component.rightAnchor.target = this.Grid.parent;
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x00117DD0 File Offset: 0x00115FD0
	public void AddCheckbox(string command, string label = "")
	{
		this.AddLine();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CheckboxTemplate);
		gameObject.transform.parent = this.Grid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (label == "")
		{
			label = UISettingsTemplate.labelFromCommand(command);
		}
		UILabel component = gameObject.GetComponent<UILabel>();
		Language.UpdateUILabel(component, label);
		component.leftAnchor.target = this.Grid.parent;
		component.rightAnchor.target = this.Grid.parent;
		gameObject.GetComponent<UITooltipScript>().Tooltip = Singleton<SystemConsole>.Instance.ConsoleCommands[command].help;
		GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
		UIEventListener uieventListener = UIEventListener.Get(gameObject2);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.PerformAction));
		UICommandAction component2 = gameObject2.GetComponent<UICommandAction>();
		component2.command = command;
		component2.checkbox = gameObject2.GetComponent<UIToggle>();
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x00117EDB File Offset: 0x001160DB
	public void AddSlider(string command, float minValue, float maxValue, int decimals = 0, bool steps = false)
	{
		this.AddSlider(command, "", minValue, maxValue, decimals, steps);
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x00117EF0 File Offset: 0x001160F0
	public void AddSlider(string command, string label, float minValue, float maxValue, int decimals = 0, bool steps = false)
	{
		this.AddLine();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SliderTemplate);
		gameObject.transform.parent = this.Grid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (label == "")
		{
			label = UISettingsTemplate.labelFromCommand(command);
		}
		UILabel component = gameObject.GetComponent<UILabel>();
		Language.UpdateUILabel(component, label);
		component.leftAnchor.target = this.Grid.parent;
		component.rightAnchor.target = this.Grid.parent;
		gameObject.GetComponent<UITooltipScript>().Tooltip = Singleton<SystemConsole>.Instance.ConsoleCommands[command].help;
		GameObject slider = gameObject.transform.GetChild(0).gameObject;
		UIScrollBar component2 = slider.GetComponent<UIScrollBar>();
		EventDelegate.Add(component2.onChange, delegate()
		{
			this.PerformAction(slider);
		});
		UISliderRange component3 = component2.transform.Find("Thumb/Label").GetComponent<UISliderRange>();
		component3.Min = minValue;
		component3.Max = maxValue;
		component3.NumDecimals = decimals;
		UIProgressBar progressBar = component3.GetProgressBar();
		if (progressBar)
		{
			progressBar.numberOfSteps = (steps ? ((int)maxValue - (int)minValue + 1) : 0);
		}
		UICommandAction component4 = slider.GetComponent<UICommandAction>();
		component4.command = command;
		component4.scrollbar = component2;
		component4.minValue = minValue;
		component4.maxValue = maxValue;
		component4.UpdateDisplayedValue(null, true);
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x00118074 File Offset: 0x00116274
	public void AddTextInput(string command, string label = "")
	{
		this.AddLine();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.StringTemplate);
		gameObject.transform.parent = this.Grid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (label == "")
		{
			label = UISettingsTemplate.labelFromCommand(command);
		}
		UILabel component = gameObject.GetComponent<UILabel>();
		Language.UpdateUILabel(component, label);
		component.leftAnchor.target = this.Grid.parent;
		component.rightAnchor.target = this.Grid.parent;
		gameObject.GetComponent<UITooltipScript>().Tooltip = Singleton<SystemConsole>.Instance.ConsoleCommands[command].help;
		UIInput textInput = gameObject.transform.GetChild(0).GetComponent<UIInput>();
		EventDelegate.Add(textInput.onChange, delegate()
		{
			this.PerformAction(textInput.gameObject);
		});
		UICommandAction component2 = textInput.GetComponent<UICommandAction>();
		component2.command = command;
		component2.textInput = textInput;
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x00118190 File Offset: 0x00116390
	public void AddButtons(string command, string[] buttonLabels, string label = "", bool[] enabled = null)
	{
		this.AddLine();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ButtonTemplate);
		gameObject.transform.parent = this.Grid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (label == "")
		{
			label = UISettingsTemplate.labelFromCommand(command);
		}
		UILabel component = gameObject.GetComponent<UILabel>();
		Language.UpdateUILabel(component, label);
		component.leftAnchor.target = this.Grid.parent;
		component.rightAnchor.target = this.Grid.parent;
		gameObject.GetComponent<UITooltipScript>().Tooltip = Singleton<SystemConsole>.Instance.ConsoleCommands[command].help;
		UICommandAction[] array = new UICommandAction[3];
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			UIButton component2 = gameObject2.GetComponent<UIButton>();
			if (enabled != null && !enabled[i])
			{
				component2.isEnabled = false;
			}
			Language.UpdateUILabel(gameObject2.transform.GetChild(0).GetComponent<UILabel>(), buttonLabels[i]);
			UIEventListener uieventListener = UIEventListener.Get(gameObject2);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.PerformAction));
			array[i] = gameObject2.GetComponent<UICommandAction>();
			array[i].command = command;
			array[i].button = component2;
			array[i].buttonID = i;
		}
		array[0].linkedActions[0] = array[1];
		array[0].linkedActions[1] = array[2];
		array[1].linkedActions[0] = array[0];
		array[1].linkedActions[1] = array[2];
		array[2].linkedActions[0] = array[0];
		array[2].linkedActions[1] = array[1];
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x0011834D File Offset: 0x0011654D
	private void PerformAction(GameObject source)
	{
		source.GetComponent<UICommandAction>().UpdateLinkedSetting();
	}

	// Token: 0x040019C7 RID: 6599
	public string Title;

	// Token: 0x040019C8 RID: 6600
	public bool ShowResetButton = true;

	// Token: 0x040019C9 RID: 6601
	[NonSerialized]
	public Transform PanelTemplate;

	// Token: 0x040019CA RID: 6602
	[NonSerialized]
	public GameObject UI;

	// Token: 0x040019CB RID: 6603
	[NonSerialized]
	public Transform Grid;

	// Token: 0x040019CC RID: 6604
	[NonSerialized]
	public UIScrollView ScrollView;

	// Token: 0x040019CD RID: 6605
	[NonSerialized]
	public GameObject Scrollbar;

	// Token: 0x040019CE RID: 6606
	[NonSerialized]
	public GameObject ButtonTemplate;

	// Token: 0x040019CF RID: 6607
	[NonSerialized]
	public GameObject CheckboxTemplate;

	// Token: 0x040019D0 RID: 6608
	[NonSerialized]
	public GameObject SliderTemplate;

	// Token: 0x040019D1 RID: 6609
	[NonSerialized]
	public GameObject StringTemplate;

	// Token: 0x040019D2 RID: 6610
	[NonSerialized]
	public GameObject Line;

	// Token: 0x040019D3 RID: 6611
	[NonSerialized]
	public GameObject DummyLine;

	// Token: 0x040019D4 RID: 6612
	[NonSerialized]
	public UILabel TitleLabel;

	// Token: 0x040019D5 RID: 6613
	[NonSerialized]
	public GameObject ResetButton;

	// Token: 0x040019D6 RID: 6614
	private bool initialized;

	// Token: 0x040019D7 RID: 6615
	private bool nextLineVisible;

	// Token: 0x040019D8 RID: 6616
	private bool inGroup;
}
