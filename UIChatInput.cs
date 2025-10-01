using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class UIChatInput : Singleton<UIChatInput>
{
	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06002162 RID: 8546 RVA: 0x000F0A45 File Offset: 0x000EEC45
	public UIInput InputText
	{
		get
		{
			return this.mInput;
		}
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000F0A50 File Offset: 0x000EEC50
	private void Start()
	{
		this.ThisUISprite = base.GetComponent<UISprite>();
		this.VignetteSprite = base.transform.Find("Vignette").GetComponent<UISprite>();
		this.ThisLabel = NGUITools.GetChildLabel(base.gameObject);
		this.ToggleVisibility(false);
		this.mInput = base.GetComponent<UIInput>();
		this.mInput.label.maxLineCount = 1;
		this.mInput.onReturnKey = UIInput.OnReturnKey.Submit;
		EventDelegate.Add(this.mInput.onChange, new EventDelegate.Callback(this.OnChange));
		if (this.fillWithDummyData && this.textList != null)
		{
			for (int i = 0; i < 30; i++)
			{
				this.textList.Add(string.Concat(new object[]
				{
					(i % 2 == 0) ? "[FFFFFF]" : "[AAAAAA]",
					"This is an example paragraph for the text list, testing line ",
					i,
					"[-]"
				}));
			}
		}
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x000F0B48 File Offset: 0x000EED48
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.ChatButton.onClick, new EventDelegate.Callback(this.ChatButtonOnClick));
		EventDelegate.Add(this.ChatDeveloper.onClick, new EventDelegate.Callback(this.ChatButtonOnClick));
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x000F0B95 File Offset: 0x000EED95
	private void OnDestroy()
	{
		EventDelegate.Remove(this.ChatButton.onClick, new EventDelegate.Callback(this.ChatButtonOnClick));
		EventDelegate.Remove(this.ChatDeveloper.onClick, new EventDelegate.Callback(this.ChatButtonOnClick));
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000F0BD1 File Offset: 0x000EEDD1
	private void ChatButtonOnClick()
	{
		if (SystemConsole.dontFocusNextSwitch)
		{
			SystemConsole.dontFocusNextSwitch = false;
			return;
		}
		if (this.mInput)
		{
			this.mInput.isSelected = true;
		}
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x000F0BFA File Offset: 0x000EEDFA
	private void OnChange()
	{
		this.CheckType();
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000F0C04 File Offset: 0x000EEE04
	private void CheckType()
	{
		if (!this.typing && !this.mInput.isSelected)
		{
			return;
		}
		this.typing = this.mInput.isSelected;
		EventManager.TriggerChatTyping(NetworkSingleton<Chat>.Instance.selectedTab.chatType, this.mInput.isSelected);
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x000F0C58 File Offset: 0x000EEE58
	public void OnSubmit()
	{
		this.remainOpen = false;
		if (this.textList != null)
		{
			string text = NGUIText.StripSymbols(this.mInput.value);
			if (!string.IsNullOrEmpty(text))
			{
				this.PreviousChatMessages.Add(text);
				if (this.PreviousChatMessages.Count > 50)
				{
					this.PreviousChatMessages.RemoveAt(0);
					this.PreviousChatIndex--;
				}
				ChatMessageType chatType = NetworkSingleton<Chat>.Instance.selectedTab.chatType;
				Chat.InputChat(text, chatType == ChatMessageType.Team, "", ChatMessageType.None);
				if (chatType == ChatMessageType.System)
				{
					this.LastEnteredCommand = text;
					this.remainOpen = true;
				}
			}
			this.mInput.value = "";
			this.InChatHistory = false;
			if (!this.remainOpen)
			{
				this.mInput.isSelected = false;
			}
		}
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x000F0D28 File Offset: 0x000EEF28
	private void ToggleVisibility(bool Visible)
	{
		this.ThisUISprite.enabled = Visible;
		this.VignetteSprite.enabled = Visible;
		this.ThisLabel.enabled = Visible;
		this.ChatButton.gameObject.SetActive(!Visible);
		this.ChatEnter.gameObject.SetActive(Visible);
		if (this.mInput && UIChatInput.ClearOnDismiss)
		{
			this.mInput.value = "";
		}
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x000F0DA4 File Offset: 0x000EEFA4
	public void OnSelect(bool Selected)
	{
		this.remainOpen = false;
		if (!Selected && UICamera.hoveredObject == this.ChatEnter.gameObject)
		{
			this.OnSubmit();
		}
		if (!Selected)
		{
			this.CheckType();
		}
		if (!this.remainOpen)
		{
			this.ToggleVisibility(Selected);
		}
		this.remainOpen = false;
		this.InChatHistory = false;
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000F0DFE File Offset: 0x000EEFFE
	public void Activate()
	{
		this.ChatButton.gameObject.SendMessage("OnClick");
		this.mInput.isSelected = true;
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x000F0E24 File Offset: 0x000EF024
	private void Update()
	{
		if (this.ThisUISprite.enabled)
		{
			NetworkSingleton<Chat>.Instance.RefreshChatLog();
			if ((TTSInput.GetKeyDown(KeyCode.UpArrow) || UIOnScreenKeyboard.UpPressed) && this.PreviousChatMessages.Count > 0)
			{
				if (!this.InChatHistory || this.PreviousChatIndex <= 0)
				{
					this.PreviousChatIndex = this.PreviousChatMessages.Count - 1;
					this.InChatHistory = true;
				}
				else
				{
					this.PreviousChatIndex--;
				}
				this.mInput.value = this.PreviousChatMessages[this.PreviousChatIndex];
			}
			if ((TTSInput.GetKeyDown(KeyCode.DownArrow) || UIOnScreenKeyboard.DownPressed) && this.PreviousChatMessages.Count > 0)
			{
				if (this.PreviousChatIndex >= this.PreviousChatMessages.Count - 1)
				{
					this.PreviousChatIndex = 0;
				}
				else
				{
					this.PreviousChatIndex++;
				}
				this.InChatHistory = true;
				this.mInput.value = this.PreviousChatMessages[this.PreviousChatIndex];
			}
			if (NetworkSingleton<Chat>.Instance.selectedTab.chatType == ChatMessageType.System && (TTSInput.GetKeyDown(KeyCode.Tab) || UIOnScreenKeyboard.TabPressed) && this.mInput.value != "")
			{
				string value = this.mInput.value;
				int i = this.mInput.cursorPosition - 1;
				if (i < 0)
				{
					i = 0;
				}
				if (i >= value.Length)
				{
					i = value.Length - 1;
				}
				int num = i;
				while (i > 0)
				{
					if (!SystemConsole.IsValidCommandCharacter(value[i - 1]))
					{
						break;
					}
					i--;
				}
				while (num < value.Length && SystemConsole.IsValidCommandCharacter(value[num]))
				{
					num++;
				}
				string text = "";
				if (num > i)
				{
					text = value.Substring(i, num - i).ToLower();
				}
				if (text != "")
				{
					string text2 = "";
					List<string> list = new List<string>();
					bool flag = false;
					for (int j = 0; j < Singleton<SystemConsole>.Instance.ConsoleCommandList.Count; j++)
					{
						string text3 = Singleton<SystemConsole>.Instance.ConsoleCommandList[j];
						SystemConsole.ConsoleCommand command = Singleton<SystemConsole>.Instance.ConsoleCommands[text3];
						if (Singleton<SystemConsole>.Instance.CommandAvailable(command))
						{
							if (text3.Length >= text.Length && text3.Substring(0, text.Length) == text)
							{
								if (text2 == "")
								{
									text2 = text3;
								}
								else
								{
									int num2 = text.Length;
									while (num2 < text3.Length && num2 < text2.Length)
									{
										if (text3[num2] != text2[num2])
										{
											text2 = text2.Substring(0, num2);
											flag = true;
											break;
										}
										num2++;
									}
								}
								list.Add(text3);
							}
							else if (text2 != "")
							{
								break;
							}
						}
					}
					if (text2 != "" && text2 != text)
					{
						string text4 = text2.Substring(text.Length);
						if (!flag && list.Count == 1)
						{
							text4 += " ";
						}
						this.mInput.value = value.Substring(0, num) + text4 + value.Substring(num);
						this.mInput.cursorPosition = num + text4.Length;
						this.lastAutocompleteTime = 0f;
						this.autocompleteDisplayed = false;
					}
					if (this.lastAutocompleteTime + 3f > Time.time)
					{
						if (!this.autocompleteDisplayed)
						{
							list.Sort();
							Chat.LogSystem(string.Join(" ", list.ToArray()), Colour.Grey, false);
							this.autocompleteDisplayed = true;
							return;
						}
					}
					else
					{
						this.lastAutocompleteTime = Time.time;
						this.autocompleteDisplayed = false;
					}
				}
			}
		}
	}

	// Token: 0x040014AE RID: 5294
	public static bool ClearOnDismiss = true;

	// Token: 0x040014AF RID: 5295
	public UITextList textList;

	// Token: 0x040014B0 RID: 5296
	public bool fillWithDummyData;

	// Token: 0x040014B1 RID: 5297
	private UISprite ThisUISprite;

	// Token: 0x040014B2 RID: 5298
	private UISprite VignetteSprite;

	// Token: 0x040014B3 RID: 5299
	private List<string> PreviousChatMessages = new List<string>();

	// Token: 0x040014B4 RID: 5300
	public string LastEnteredCommand = "";

	// Token: 0x040014B5 RID: 5301
	private int PreviousChatIndex;

	// Token: 0x040014B6 RID: 5302
	private bool InChatHistory;

	// Token: 0x040014B7 RID: 5303
	public UIButton ChatButton;

	// Token: 0x040014B8 RID: 5304
	public UIButton ChatEnter;

	// Token: 0x040014B9 RID: 5305
	public UIButton ChatDeveloper;

	// Token: 0x040014BA RID: 5306
	private UILabel ThisLabel;

	// Token: 0x040014BB RID: 5307
	private UIInput mInput;

	// Token: 0x040014BC RID: 5308
	private bool remainOpen;

	// Token: 0x040014BD RID: 5309
	private const float AUTOCOMPLETE_DISPLAY_WINDOW = 3f;

	// Token: 0x040014BE RID: 5310
	private float lastAutocompleteTime;

	// Token: 0x040014BF RID: 5311
	private bool autocompleteDisplayed;

	// Token: 0x040014C0 RID: 5312
	private bool typing;
}
