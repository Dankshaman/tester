using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x02000087 RID: 135
[AddComponentMenu("NGUI/UI/Input Field")]
public class UIInput : MonoBehaviour
{
	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000677 RID: 1655 RVA: 0x0002E33A File Offset: 0x0002C53A
	// (set) Token: 0x06000678 RID: 1656 RVA: 0x0002E350 File Offset: 0x0002C550
	public string defaultText
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mDefaultText;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			this.mDefaultText = value;
			this.UpdateLabel();
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000679 RID: 1657 RVA: 0x0002E36D File Offset: 0x0002C56D
	// (set) Token: 0x0600067A RID: 1658 RVA: 0x0002E383 File Offset: 0x0002C583
	public Color defaultColor
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mDefaultColor;
		}
		set
		{
			this.mDefaultColor = value;
			if (!this.isSelected)
			{
				this.label.color = value;
			}
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x0600067B RID: 1659 RVA: 0x0002E3A0 File Offset: 0x0002C5A0
	public bool inputShouldBeHidden
	{
		get
		{
			return this.hideInput && this.label != null && !this.label.multiLine && this.inputType != UIInput.InputType.Password;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x0600067C RID: 1660 RVA: 0x0002E3D3 File Offset: 0x0002C5D3
	// (set) Token: 0x0600067D RID: 1661 RVA: 0x0002E3DB File Offset: 0x0002C5DB
	[Obsolete("Use UIInput.value instead")]
	public string text
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600067E RID: 1662 RVA: 0x0002E3E4 File Offset: 0x0002C5E4
	// (set) Token: 0x0600067F RID: 1663 RVA: 0x0002E3FA File Offset: 0x0002C5FA
	public string value
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mValue;
		}
		set
		{
			this.Set(value, true);
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0002E404 File Offset: 0x0002C604
	public void Set(string value, bool notify = true)
	{
		if (this.readOnly)
		{
			return;
		}
		if (this.mDoInit)
		{
			this.Init();
		}
		if (value == this.value)
		{
			return;
		}
		UIInput.mDrawStart = 0;
		value = this.Validate(value);
		if (this.mValue != value)
		{
			this.mValue = value;
			this.mLoadSavedValue = false;
			if (this.isSelected)
			{
				if (string.IsNullOrEmpty(value))
				{
					this.mSelectionStart = 0;
					this.mSelectionEnd = 0;
				}
				else
				{
					this.mSelectionStart = value.Length;
					this.mSelectionEnd = this.mSelectionStart;
				}
			}
			else if (this.mStarted)
			{
				this.SaveToPlayerPrefs(value);
			}
			this.UpdateLabel();
			if (notify)
			{
				this.ExecuteOnChange();
			}
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000681 RID: 1665 RVA: 0x0002E4BA File Offset: 0x0002C6BA
	// (set) Token: 0x06000682 RID: 1666 RVA: 0x0002E4C2 File Offset: 0x0002C6C2
	[Obsolete("Use UIInput.isSelected instead")]
	public bool selected
	{
		get
		{
			return this.isSelected;
		}
		set
		{
			this.isSelected = value;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000683 RID: 1667 RVA: 0x0002E4CB File Offset: 0x0002C6CB
	// (set) Token: 0x06000684 RID: 1668 RVA: 0x0002E4D8 File Offset: 0x0002C6D8
	public bool isSelected
	{
		get
		{
			return UIInput.selection == this;
		}
		set
		{
			if (!value)
			{
				if (this.isSelected)
				{
					UICamera.selectedObject = null;
					return;
				}
			}
			else
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000685 RID: 1669 RVA: 0x0002E4F7 File Offset: 0x0002C6F7
	// (set) Token: 0x06000686 RID: 1670 RVA: 0x0002E513 File Offset: 0x0002C713
	public int cursorPosition
	{
		get
		{
			if (!this.isSelected)
			{
				return this.value.Length;
			}
			return this.mSelectionEnd;
		}
		set
		{
			if (this.isSelected)
			{
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000687 RID: 1671 RVA: 0x0002E52A File Offset: 0x0002C72A
	// (set) Token: 0x06000688 RID: 1672 RVA: 0x0002E546 File Offset: 0x0002C746
	public int selectionStart
	{
		get
		{
			if (!this.isSelected)
			{
				return this.value.Length;
			}
			return this.mSelectionStart;
		}
		set
		{
			if (this.isSelected)
			{
				this.mSelectionStart = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x0002E4F7 File Offset: 0x0002C6F7
	// (set) Token: 0x0600068A RID: 1674 RVA: 0x0002E513 File Offset: 0x0002C713
	public int selectionEnd
	{
		get
		{
			if (!this.isSelected)
			{
				return this.value.Length;
			}
			return this.mSelectionEnd;
		}
		set
		{
			if (this.isSelected)
			{
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x0002E55D File Offset: 0x0002C75D
	public UITexture caret
	{
		get
		{
			return this.mCaret;
		}
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0002E568 File Offset: 0x0002C768
	public string Validate(string val)
	{
		if (string.IsNullOrEmpty(val))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder(val.Length);
		foreach (char c in val)
		{
			if (this.onValidate != null)
			{
				c = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, c);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				c = this.Validate(stringBuilder.ToString(), stringBuilder.Length, c);
			}
			if (c != '\0')
			{
				stringBuilder.Append(c);
			}
		}
		if (this.characterLimit > 0 && stringBuilder.Length > this.characterLimit)
		{
			return stringBuilder.ToString(0, this.characterLimit);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0002E620 File Offset: 0x0002C820
	public void Start()
	{
		if (this.mStarted)
		{
			return;
		}
		if (this.mLoadSavedValue && !string.IsNullOrEmpty(this.savedAs))
		{
			this.LoadValue();
		}
		else
		{
			this.value = this.mValue.Replace("\\n", "\n");
		}
		this.selectionColor = new Color(Colour.Grey.r, Colour.Grey.g, Colour.Grey.b, 0.6f);
		if (!base.GetComponent<UIButton>())
		{
			UIButton uibutton = base.gameObject.AddComponent<UIButton>();
			uibutton.tweenTarget = base.gameObject;
			UISprite component = base.gameObject.GetComponent<UISprite>();
			if (component && component.ThemeAsSetting != UIPalette.UI.Auto)
			{
				uibutton.ThemeNormalAsSetting = component.ThemeAsSetting;
			}
			else
			{
				uibutton.ThemeNormalAsSetting = UIPalette.UI.ButtonNormal;
			}
			uibutton.ThemeHoverAsSetting = UIPalette.UI.ButtonHover;
			uibutton.ThemePressedAsSetting = UIPalette.UI.ButtonPressed;
			uibutton.ThemeDisabledAsSetting = UIPalette.UI.ButtonDisabled;
		}
		this.mStarted = true;
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0002E710 File Offset: 0x0002C910
	protected void Init()
	{
		if (!this.BeenInit)
		{
			Singleton<UIPalette>.Instance.InitTheme(this, null);
		}
		if (this.mDoInit && this.label != null)
		{
			this.mDoInit = false;
			this.mDefaultText = this.label.text;
			this.mDefaultColor = this.label.color;
			this.mEllipsis = this.label.overflowEllipsis;
			if (this.label.alignment == NGUIText.Alignment.Justified)
			{
				this.label.alignment = NGUIText.Alignment.Left;
				Debug.LogWarning("Input fields using labels with justified alignment are not supported at this time", this);
			}
			this.mAlignment = this.label.alignment;
			this.mPosition = this.label.cachedTransform.localPosition.x;
			this.UpdateLabel();
		}
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0002E7DE File Offset: 0x0002C9DE
	protected void SaveToPlayerPrefs(string val)
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			if (string.IsNullOrEmpty(val))
			{
				PlayerPrefs.DeleteKey(this.savedAs);
				return;
			}
			PlayerPrefs.SetString(this.savedAs, val);
		}
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002E810 File Offset: 0x0002CA10
	protected virtual void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			if (this.label != null)
			{
				this.label.supportEncoding = false;
			}
			if (this.mOnGUI == null)
			{
				this.mOnGUI = base.gameObject.AddComponent<UIInputOnGUI>();
			}
			this.OnSelectEvent();
			return;
		}
		if (this.mOnGUI != null)
		{
			UnityEngine.Object.Destroy(this.mOnGUI);
			this.mOnGUI = null;
		}
		this.OnDeselectEvent();
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0002E888 File Offset: 0x0002CA88
	protected void OnSelectEvent()
	{
		this.mSelectTime = Time.frameCount;
		UIInput.selection = this;
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null)
		{
			this.mEllipsis = this.label.overflowEllipsis;
			this.label.overflowEllipsis = false;
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mSelectMe = Time.frameCount;
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0002E900 File Offset: 0x0002CB00
	protected void OnDeselectEvent()
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null)
		{
			this.label.overflowEllipsis = this.mEllipsis;
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			if (!this.readOnly)
			{
				this.mValue = this.value;
			}
			if (string.IsNullOrEmpty(this.mValue))
			{
				this.label.text = this.mDefaultText;
				this.label.color = this.mDefaultColor;
			}
			else
			{
				this.label.text = this.mValue;
			}
			Input.imeCompositionMode = IMECompositionMode.Auto;
			this.label.alignment = this.mAlignment;
		}
		UIInput.selection = null;
		this.UpdateLabel();
		if (this.submitOnUnselect)
		{
			this.Submit();
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0002E9D8 File Offset: 0x0002CBD8
	protected virtual void Update()
	{
		if (!this.isSelected || this.mSelectTime == Time.frameCount)
		{
			return;
		}
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.mSelectMe != -1 && this.mSelectMe != Time.frameCount)
		{
			this.mSelectMe = -1;
			this.mSelectionStart = 0;
			this.mSelectionEnd = (string.IsNullOrEmpty(this.mValue) ? 0 : this.mValue.Length);
			if (!this.SelectAllTextOnClick)
			{
				Vector3[] worldCorners = this.label.worldCorners;
				Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);
				Plane plane = new Plane(worldCorners[0], worldCorners[1], worldCorners[2]);
				float distance;
				this.mSelectionStart = (plane.Raycast(ray, out distance) ? (UIInput.mDrawStart + this.label.GetCharacterIndexAtPosition(ray.GetPoint(distance), false)) : this.mSelectionEnd);
				this.mSelectionEnd = this.mSelectionStart;
			}
			UIInput.mDrawStart = 0;
			this.label.color = this.activeTextColor;
			if (Application.platform == RuntimePlatform.WindowsPlayer && zInput.bTouching)
			{
				Debug.Log("Showing onscreen keyboard.");
				VirtualKeyboard.ShowOnScreenKeyboard();
			}
			if (VRHMD.isVR && !UIOnScreenKeyboard.ON_SCREEN_KEYBOARD)
			{
				Debug.Log("Showing VR Keyboard");
				SteamVR.instance.overlay.ShowKeyboard(0, 0, this.defaultText, 256U, this.value, false, 0UL);
				if (this.KeyboardCharInput == null)
				{
					this.KeyboardCharInput = SteamVR_Events.SystemAction(EVREventType.VREvent_KeyboardCharInput, new UnityAction<VREvent_t>(this.OnVRKeyboard));
				}
				if (this.KeyboardClosed == null)
				{
					this.KeyboardClosed = SteamVR_Events.SystemAction(EVREventType.VREvent_KeyboardClosed, new UnityAction<VREvent_t>(this.OnVRKeyboardClose));
				}
				this.KeyboardCharInput.enabled = true;
				this.KeyboardClosed.enabled = true;
			}
			Vector2 vector = (UICamera.current != null && UICamera.current.cachedCamera != null) ? UICamera.current.cachedCamera.WorldToScreenPoint(this.label.worldCorners[0]) : this.label.worldCorners[0];
			vector.y = (float)Screen.height - vector.y;
			Input.imeCompositionMode = IMECompositionMode.On;
			Input.compositionCursorPos = vector;
			this.UpdateLabel();
			if (string.IsNullOrEmpty(Input.inputString))
			{
				return;
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (this.selectOnTab != null)
			{
				UICamera.selectedObject = this.selectOnTab;
				return;
			}
			if (this.TabIsFourSpaces)
			{
				this.Insert("    ");
			}
		}
		if (cInput.GetKeyDown("System Console") && Singleton<SystemConsole>.Instance.lockHotkey && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
		{
			return;
		}
		string compositionString = Input.compositionString;
		bool flag;
		if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
		{
			flag = (string.IsNullOrEmpty(compositionString) && !string.IsNullOrEmpty(Input.inputString));
		}
		else
		{
			flag = !string.IsNullOrEmpty(Input.inputString);
		}
		if (flag)
		{
			foreach (char c in Input.inputString)
			{
				if (c >= ' ' && c != '' && c != '' && c != '' && c != '' && c != '')
				{
					this.Insert(c.ToString());
				}
			}
		}
		if (UIInput.mLastIME != compositionString)
		{
			this.mSelectionEnd = (string.IsNullOrEmpty(compositionString) ? this.mSelectionStart : (this.mValue.Length + compositionString.Length));
			UIInput.mLastIME = compositionString;
			this.UpdateLabel();
			this.ExecuteOnChange();
		}
		if (this.mCaret != null && this.mNextBlink < RealTime.time)
		{
			this.mNextBlink = RealTime.time + 0.5f;
			this.mCaret.enabled = !this.mCaret.enabled;
		}
		if (this.isSelected && this.mLastAlpha != this.label.finalAlpha)
		{
			this.UpdateLabel();
		}
		if (this.mCam == null)
		{
			this.mCam = UICamera.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.mCam != null)
		{
			bool flag2 = false;
			if (this.label.multiLine)
			{
				bool flag3 = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
				if (this.onReturnKey == UIInput.OnReturnKey.Submit)
				{
					flag2 = flag3;
				}
				else
				{
					flag2 = !flag3;
				}
			}
			if (UICamera.GetKeyDown(this.mCam.submitKey0) || (this.mCam.submitKey0 == KeyCode.Return && UICamera.GetKeyDown(KeyCode.KeypadEnter)))
			{
				if (flag2)
				{
					this.Insert("\n");
				}
				else
				{
					if (UICamera.controller.current != null)
					{
						UICamera.controller.clickNotification = UICamera.ClickNotification.None;
					}
					UICamera.currentKey = this.mCam.submitKey0;
					this.Submit();
				}
			}
			if (UICamera.GetKeyDown(this.mCam.submitKey1) || (this.mCam.submitKey1 == KeyCode.Return && UICamera.GetKeyDown(KeyCode.KeypadEnter)))
			{
				if (flag2)
				{
					this.Insert("\n");
				}
				else
				{
					if (UICamera.controller.current != null)
					{
						UICamera.controller.clickNotification = UICamera.ClickNotification.None;
					}
					UICamera.currentKey = this.mCam.submitKey1;
					this.Submit();
				}
			}
			if (!this.mCam.useKeyboard && UICamera.GetKeyUp(KeyCode.Tab))
			{
				this.OnKey(KeyCode.Tab);
			}
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0002EF94 File Offset: 0x0002D194
	private void OnVRKeyboard(VREvent_t vrEvent)
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		SteamVR.instance.overlay.GetKeyboardText(stringBuilder, 256U);
		this.value = stringBuilder.ToString();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0002EFCE File Offset: 0x0002D1CE
	private void OnVRKeyboardClose(VREvent_t vrEvent)
	{
		this.KeyboardCharInput.enabled = false;
		this.KeyboardClosed.enabled = false;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0002EFE8 File Offset: 0x0002D1E8
	private void OnKey(KeyCode key)
	{
		int frameCount = Time.frameCount;
		if (UIInput.mIgnoreKey == frameCount)
		{
			return;
		}
		if (this.mCam != null && (key == this.mCam.cancelKey0 || key == this.mCam.cancelKey1))
		{
			UIInput.mIgnoreKey = frameCount;
			this.isSelected = false;
		}
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0002F03B File Offset: 0x0002D23B
	protected void DoBackspace()
	{
		if (!string.IsNullOrEmpty(this.mValue))
		{
			if (this.mSelectionStart == this.mSelectionEnd)
			{
				if (this.mSelectionStart < 1)
				{
					return;
				}
				this.mSelectionEnd--;
			}
			this.Insert("");
		}
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0002F07C File Offset: 0x0002D27C
	public virtual bool ProcessEvent(Event ev, bool fromOnScreenKeyboard = false, bool forceShift = false, bool forceCtrl = false)
	{
		if (this.label == null)
		{
			return false;
		}
		RuntimePlatform platform = Application.platform;
		bool flag = (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer) ? ((ev.modifiers & EventModifiers.Command) > EventModifiers.None) : ((ev.modifiers & EventModifiers.Control) > EventModifiers.None);
		if ((ev.modifiers & EventModifiers.Alt) != EventModifiers.None)
		{
			flag = false;
		}
		bool flag2 = (ev.modifiers & EventModifiers.Shift) > EventModifiers.None;
		if (fromOnScreenKeyboard)
		{
			flag2 = forceShift;
			flag = forceCtrl;
		}
		KeyCode keyCode = ev.keyCode;
		if (keyCode <= KeyCode.C)
		{
			if (keyCode <= KeyCode.Return)
			{
				if (keyCode == KeyCode.Backspace)
				{
					ev.Use();
					string leftText = this.GetLeftText();
					if (this.TabIsFourSpaces && leftText.EndsWith("    "))
					{
						for (int i = 0; i < "    ".Length; i++)
						{
							this.DoBackspace();
						}
					}
					else
					{
						this.DoBackspace();
					}
					return true;
				}
				if (keyCode == KeyCode.Return)
				{
					if (fromOnScreenKeyboard)
					{
						ev.Use();
						if (this.onReturnKey == UIInput.OnReturnKey.Submit)
						{
							this.Submit();
						}
						else
						{
							this.Insert("\n");
						}
						return true;
					}
					return false;
				}
			}
			else
			{
				if (keyCode == KeyCode.A)
				{
					if (flag)
					{
						ev.Use();
						this.mSelectionStart = 0;
						this.mSelectionEnd = this.mValue.Length;
						this.UpdateLabel();
					}
					else if (fromOnScreenKeyboard)
					{
						ev.Use();
						this.Insert(ev.character.ToString());
					}
					return true;
				}
				if (keyCode == KeyCode.C)
				{
					if (flag)
					{
						ev.Use();
						NGUITools.clipboard = this.GetSelection();
					}
					else if (fromOnScreenKeyboard)
					{
						ev.Use();
						this.Insert(ev.character.ToString());
					}
					return true;
				}
			}
		}
		else if (keyCode <= KeyCode.X)
		{
			if (keyCode == KeyCode.V)
			{
				if (flag)
				{
					ev.Use();
					this.Insert(NGUITools.clipboard);
				}
				else if (fromOnScreenKeyboard)
				{
					ev.Use();
					this.Insert(ev.character.ToString());
				}
				return true;
			}
			if (keyCode == KeyCode.X)
			{
				if (flag)
				{
					ev.Use();
					NGUITools.clipboard = this.GetSelection();
					this.Insert("");
				}
				else if (fromOnScreenKeyboard)
				{
					ev.Use();
					this.Insert(ev.character.ToString());
				}
				return true;
			}
		}
		else
		{
			if (keyCode == KeyCode.Delete)
			{
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					if (this.mSelectionStart == this.mSelectionEnd)
					{
						if (this.mSelectionStart >= this.mValue.Length)
						{
							return true;
						}
						this.mSelectionEnd++;
					}
					this.Insert("");
				}
				return true;
			}
			switch (keyCode)
			{
			case KeyCode.UpArrow:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.UpArrow);
					if (this.mSelectionEnd != 0)
					{
						this.mSelectionEnd += UIInput.mDrawStart;
					}
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			case KeyCode.DownArrow:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.DownArrow);
					if (this.mSelectionEnd != this.label.processedText.Length)
					{
						this.mSelectionEnd += UIInput.mDrawStart;
					}
					else
					{
						this.mSelectionEnd = this.mValue.Length;
					}
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			case KeyCode.RightArrow:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					this.mSelectionEnd = Mathf.Min(this.mSelectionEnd + 1, this.mValue.Length);
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			case KeyCode.LeftArrow:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					this.mSelectionEnd = Mathf.Max(this.mSelectionEnd - 1, 0);
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			case KeyCode.Home:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					if (this.label.multiLine)
					{
						this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.Home);
					}
					else
					{
						this.mSelectionEnd = 0;
					}
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			case KeyCode.End:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					if (this.label.multiLine)
					{
						this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.End);
					}
					else
					{
						this.mSelectionEnd = this.mValue.Length;
					}
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			case KeyCode.PageUp:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					this.mSelectionEnd = 0;
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			case KeyCode.PageDown:
				ev.Use();
				if (!string.IsNullOrEmpty(this.mValue))
				{
					this.mSelectionEnd = this.mValue.Length;
					if (!flag2)
					{
						this.mSelectionStart = this.mSelectionEnd;
					}
					this.UpdateLabel();
				}
				return true;
			}
		}
		if (fromOnScreenKeyboard && !flag)
		{
			ev.Use();
			this.Insert(ev.character.ToString());
			return true;
		}
		return false;
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0002F5EC File Offset: 0x0002D7EC
	protected virtual void Insert(string text)
	{
		if (this.readOnly)
		{
			return;
		}
		string leftText = this.GetLeftText();
		string rightText = this.GetRightText();
		int length = rightText.Length;
		StringBuilder stringBuilder = new StringBuilder(leftText.Length + rightText.Length + text.Length);
		stringBuilder.Append(leftText);
		int i = 0;
		int length2 = text.Length;
		while (i < length2)
		{
			char c = text[i];
			if (c == '\b')
			{
				this.DoBackspace();
			}
			else
			{
				if (this.characterLimit > 0 && stringBuilder.Length + length >= this.characterLimit)
				{
					break;
				}
				if (this.onValidate != null)
				{
					c = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, c);
				}
				else if (this.validation != UIInput.Validation.None)
				{
					c = this.Validate(stringBuilder.ToString(), stringBuilder.Length, c);
				}
				if (c != '\0')
				{
					stringBuilder.Append(c);
				}
			}
			i++;
		}
		this.mSelectionStart = stringBuilder.Length;
		this.mSelectionEnd = this.mSelectionStart;
		int j = 0;
		int length3 = rightText.Length;
		while (j < length3)
		{
			char c2 = rightText[j];
			if (this.onValidate != null)
			{
				c2 = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, c2);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				c2 = this.Validate(stringBuilder.ToString(), stringBuilder.Length, c2);
			}
			if (c2 != '\0')
			{
				stringBuilder.Append(c2);
			}
			j++;
		}
		this.mValue = stringBuilder.ToString();
		this.UpdateLabel();
		this.ExecuteOnChange();
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0002F77C File Offset: 0x0002D97C
	public string GetLeftText()
	{
		int num = Mathf.Min(new int[]
		{
			this.mSelectionStart,
			this.mSelectionEnd,
			this.mValue.Length
		});
		if (!string.IsNullOrEmpty(this.mValue) && num >= 0)
		{
			return this.mValue.Substring(0, num);
		}
		return "";
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0002F7DC File Offset: 0x0002D9DC
	public string GetRightText()
	{
		int num = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		if (!string.IsNullOrEmpty(this.mValue) && num < this.mValue.Length)
		{
			return this.mValue.Substring(num);
		}
		return "";
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0002F828 File Offset: 0x0002DA28
	protected string GetSelection()
	{
		if (string.IsNullOrEmpty(this.mValue) || this.mSelectionStart == this.mSelectionEnd)
		{
			return "";
		}
		int num = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
		int num2 = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		return this.mValue.Substring(num, num2 - num);
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0002F88C File Offset: 0x0002DA8C
	protected int GetCharUnderMouse()
	{
		Vector3[] worldCorners = this.label.worldCorners;
		Ray currentRay = UICamera.currentRay;
		Plane plane = new Plane(worldCorners[0], worldCorners[1], worldCorners[2]);
		float distance;
		if (!plane.Raycast(currentRay, out distance))
		{
			return 0;
		}
		return UIInput.mDrawStart + this.label.GetCharacterIndexAtPosition(currentRay.GetPoint(distance), false);
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0002F8F0 File Offset: 0x0002DAF0
	protected virtual void OnPress(bool isPressed)
	{
		if (isPressed && this.isSelected && this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				this.selectionStart = this.mSelectionEnd;
			}
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x0002F955 File Offset: 0x0002DB55
	protected virtual void OnDrag(Vector2 delta)
	{
		if (this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x0002F980 File Offset: 0x0002DB80
	private void OnDisable()
	{
		this.Cleanup();
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0002F988 File Offset: 0x0002DB88
	protected virtual void Cleanup()
	{
		if (this.mHighlight)
		{
			this.mHighlight.enabled = false;
		}
		if (this.mCaret)
		{
			this.mCaret.enabled = false;
		}
		if (this.mBlankTex)
		{
			NGUITools.Destroy(this.mBlankTex);
			this.mBlankTex = null;
		}
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0002F9E8 File Offset: 0x0002DBE8
	public void Submit()
	{
		if (NGUITools.GetActive(this))
		{
			if (!this.readOnly)
			{
				this.mValue = this.value;
			}
			if (UIInput.current == null)
			{
				UIInput.current = this;
				EventDelegate.Execute(this.onSubmit);
				UIInput.current = null;
			}
			this.SaveToPlayerPrefs(this.mValue);
		}
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0002FA44 File Offset: 0x0002DC44
	public void UpdateLabel()
	{
		if (this.label != null)
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			bool isSelected = this.isSelected;
			string value = this.value;
			bool flag = string.IsNullOrEmpty(value) && string.IsNullOrEmpty(Input.compositionString);
			this.label.color = ((flag && !isSelected) ? this.mDefaultColor : this.activeTextColor);
			string text;
			if (flag)
			{
				text = (isSelected ? "" : this.mDefaultText);
				this.label.alignment = this.mAlignment;
			}
			else
			{
				if (this.inputType == UIInput.InputType.Password)
				{
					text = "";
					string str = "*";
					if (this.label.bitmapFont != null && this.label.bitmapFont.bmFont != null && this.label.bitmapFont.bmFont.GetGlyph(42) == null)
					{
						str = "x";
					}
					int i = 0;
					int length = value.Length;
					while (i < length)
					{
						text += str;
						i++;
					}
				}
				else
				{
					text = value;
				}
				int num = isSelected ? Mathf.Min(text.Length, this.cursorPosition) : 0;
				string str2 = text.Substring(0, num);
				if (isSelected)
				{
					str2 += Input.compositionString;
				}
				text = str2 + text.Substring(num, text.Length - num);
				if (isSelected && this.label.overflowMethod == UILabel.Overflow.ClampContent && this.label.maxLineCount == 1)
				{
					int num2 = this.label.CalculateOffsetToFit(text);
					if (num2 == 0)
					{
						UIInput.mDrawStart = 0;
						this.label.alignment = this.mAlignment;
					}
					else if (num < UIInput.mDrawStart)
					{
						UIInput.mDrawStart = num;
						this.label.alignment = NGUIText.Alignment.Left;
					}
					else if (num2 < UIInput.mDrawStart)
					{
						UIInput.mDrawStart = num2;
						this.label.alignment = NGUIText.Alignment.Left;
					}
					else
					{
						num2 = this.label.CalculateOffsetToFit(text.Substring(0, num));
						if (num2 > UIInput.mDrawStart)
						{
							UIInput.mDrawStart = num2;
							this.label.alignment = NGUIText.Alignment.Right;
						}
					}
					if (UIInput.mDrawStart != 0)
					{
						text = text.Substring(UIInput.mDrawStart, text.Length - UIInput.mDrawStart);
					}
				}
				else
				{
					UIInput.mDrawStart = 0;
					this.label.alignment = this.mAlignment;
				}
			}
			if (!this.readOnly)
			{
				this.label.text = text;
			}
			if (isSelected)
			{
				int num3 = this.mSelectionStart - UIInput.mDrawStart;
				int num4 = this.mSelectionEnd - UIInput.mDrawStart;
				if (this.mBlankTex == null)
				{
					this.mBlankTex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
					for (int j = 0; j < 2; j++)
					{
						for (int k = 0; k < 2; k++)
						{
							this.mBlankTex.SetPixel(k, j, Color.white);
						}
					}
					this.mBlankTex.Apply();
				}
				if (num3 != num4)
				{
					if (this.mHighlight == null)
					{
						this.mHighlight = this.label.cachedGameObject.AddWidget(int.MaxValue);
						this.mHighlight.name = "Input Highlight";
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.fillGeometry = false;
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.SetAnchor(this.label.cachedTransform);
					}
					else
					{
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.MarkAsChanged();
						this.mHighlight.enabled = true;
					}
				}
				if (this.mCaret == null)
				{
					this.mCaret = this.label.cachedGameObject.AddWidget(int.MaxValue);
					this.mCaret.name = "Input Caret";
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.fillGeometry = false;
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.SetAnchor(this.label.cachedTransform);
				}
				else
				{
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.MarkAsChanged();
					this.mCaret.enabled = true;
				}
				if (num3 != num4)
				{
					this.label.PrintOverlay(num3, num4, this.mCaret.geometry, this.mHighlight.geometry, this.caretColor, this.selectionColor);
					this.mHighlight.enabled = this.mHighlight.geometry.hasVertices;
				}
				else
				{
					this.label.PrintOverlay(num3, num4, this.mCaret.geometry, null, this.caretColor, this.selectionColor);
					if (this.mHighlight != null)
					{
						this.mHighlight.enabled = false;
					}
				}
				this.mNextBlink = RealTime.time + 0.5f;
				this.mLastAlpha = this.label.finalAlpha;
				return;
			}
			this.Cleanup();
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0002FF84 File Offset: 0x0002E184
	protected char Validate(string text, int pos, char ch)
	{
		if (this.validation == UIInput.Validation.None || !base.enabled)
		{
			return ch;
		}
		if (this.validation == UIInput.Validation.Integer)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Float)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
			if (ch == '.' && !text.Contains("."))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Alphanumeric)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch;
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Username)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch - 'A' + 'a';
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Filename)
		{
			if (ch == ':')
			{
				return '\0';
			}
			if (ch == '/')
			{
				return '\0';
			}
			if (ch == '\\')
			{
				return '\0';
			}
			if (ch == '<')
			{
				return '\0';
			}
			if (ch == '>')
			{
				return '\0';
			}
			if (ch == '|')
			{
				return '\0';
			}
			if (ch == '^')
			{
				return '\0';
			}
			if (ch == '*')
			{
				return '\0';
			}
			if (ch == ';')
			{
				return '\0';
			}
			if (ch == '"')
			{
				return '\0';
			}
			if (ch == '`')
			{
				return '\0';
			}
			if (ch == '\t')
			{
				return '\0';
			}
			if (ch == '\n')
			{
				return '\0';
			}
			return ch;
		}
		else if (this.validation == UIInput.Validation.Name)
		{
			char c = (text.Length > 0) ? text[Mathf.Clamp(pos, 0, text.Length - 1)] : ' ';
			char c2 = (text.Length > 0) ? text[Mathf.Clamp(pos + 1, 0, text.Length - 1)] : '\n';
			if (ch >= 'a' && ch <= 'z')
			{
				if (c == ' ')
				{
					return ch - 'a' + 'A';
				}
				return ch;
			}
			else if (ch >= 'A' && ch <= 'Z')
			{
				if (c != ' ' && c != '\'')
				{
					return ch - 'A' + 'a';
				}
				return ch;
			}
			else if (ch == '\'')
			{
				if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
				{
					return ch;
				}
			}
			else if (ch == ' ' && c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Hex)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch >= 'A' && ch <= 'F')
			{
				return ch;
			}
			if (ch >= 'a' && ch <= 'f')
			{
				return ch + 'A' - 'a';
			}
		}
		else if (this.validation == UIInput.Validation.Variable)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_' || (ch >= '0' && ch <= '9' && pos != 0))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Tag && ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_' || ch == ' ' || (ch >= '0' && ch <= '9' && pos != 0)))
		{
			return ch;
		}
		return '\0';
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00030274 File Offset: 0x0002E474
	protected void ExecuteOnChange()
	{
		if (UIInput.current == null && EventDelegate.IsValid(this.onChange))
		{
			UIInput.current = this;
			EventDelegate.Execute(this.onChange);
			UIInput.current = null;
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x000302A7 File Offset: 0x0002E4A7
	public void RemoveFocus()
	{
		this.isSelected = false;
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000302B0 File Offset: 0x0002E4B0
	public void SaveValue()
	{
		this.SaveToPlayerPrefs(this.mValue);
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x000302C0 File Offset: 0x0002E4C0
	public void LoadValue()
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			string text = this.mValue.Replace("\\n", "\n");
			if (!this.readOnly)
			{
				this.mValue = "";
			}
			this.value = (PlayerPrefs.HasKey(this.savedAs) ? PlayerPrefs.GetString(this.savedAs) : text);
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00030324 File Offset: 0x0002E524
	public void SetReadOnlyValue(string newValue)
	{
		this.readOnly = false;
		this.value = newValue;
		this.readOnly = true;
	}

	// Token: 0x04000493 RID: 1171
	public UIPalette.UI ThemeActiveAsSetting;

	// Token: 0x04000494 RID: 1172
	public UIPalette.UI ThemeInactiveAsSetting;

	// Token: 0x04000495 RID: 1173
	public UIPalette.UI ThemeCaretAsSetting;

	// Token: 0x04000496 RID: 1174
	public UIPalette.UI ThemeSelectionAsSetting;

	// Token: 0x04000497 RID: 1175
	public UIPalette.UI ThemeActiveAs = UIPalette.UI.DoNotTheme;

	// Token: 0x04000498 RID: 1176
	public UIPalette.UI ThemeInactiveAs = UIPalette.UI.DoNotTheme;

	// Token: 0x04000499 RID: 1177
	public UIPalette.UI ThemeCaretAs = UIPalette.UI.DoNotTheme;

	// Token: 0x0400049A RID: 1178
	public UIPalette.UI ThemeSelectionAs = UIPalette.UI.DoNotTheme;

	// Token: 0x0400049B RID: 1179
	[NonSerialized]
	public bool BeenInit;

	// Token: 0x0400049C RID: 1180
	public static UIInput current;

	// Token: 0x0400049D RID: 1181
	public static UIInput selection;

	// Token: 0x0400049E RID: 1182
	public UILabel label;

	// Token: 0x0400049F RID: 1183
	public UIInput.InputType inputType;

	// Token: 0x040004A0 RID: 1184
	public UIInput.OnReturnKey onReturnKey;

	// Token: 0x040004A1 RID: 1185
	public UIInput.KeyboardType keyboardType;

	// Token: 0x040004A2 RID: 1186
	public bool readOnly;

	// Token: 0x040004A3 RID: 1187
	public bool hideInput;

	// Token: 0x040004A4 RID: 1188
	[NonSerialized]
	public bool selectAllTextOnFocus = true;

	// Token: 0x040004A5 RID: 1189
	public bool submitOnUnselect;

	// Token: 0x040004A6 RID: 1190
	public UIInput.Validation validation;

	// Token: 0x040004A7 RID: 1191
	public int characterLimit;

	// Token: 0x040004A8 RID: 1192
	public string savedAs;

	// Token: 0x040004A9 RID: 1193
	[HideInInspector]
	[SerializeField]
	public GameObject selectOnTab;

	// Token: 0x040004AA RID: 1194
	public Color activeTextColor = Color.white;

	// Token: 0x040004AB RID: 1195
	public Color caretColor = new Color(1f, 1f, 1f, 0.8f);

	// Token: 0x040004AC RID: 1196
	public Color selectionColor = new Color(1f, 0.8745098f, 0.5529412f, 0.5f);

	// Token: 0x040004AD RID: 1197
	public List<EventDelegate> onSubmit = new List<EventDelegate>();

	// Token: 0x040004AE RID: 1198
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x040004AF RID: 1199
	public UIInput.OnValidate onValidate;

	// Token: 0x040004B0 RID: 1200
	[SerializeField]
	[HideInInspector]
	protected string mValue;

	// Token: 0x040004B1 RID: 1201
	[NonSerialized]
	protected string mDefaultText = "";

	// Token: 0x040004B2 RID: 1202
	[NonSerialized]
	protected Color mDefaultColor = Color.white;

	// Token: 0x040004B3 RID: 1203
	[NonSerialized]
	protected float mPosition;

	// Token: 0x040004B4 RID: 1204
	[NonSerialized]
	protected bool mDoInit = true;

	// Token: 0x040004B5 RID: 1205
	[NonSerialized]
	protected NGUIText.Alignment mAlignment = NGUIText.Alignment.Left;

	// Token: 0x040004B6 RID: 1206
	[NonSerialized]
	protected bool mLoadSavedValue = true;

	// Token: 0x040004B7 RID: 1207
	protected static int mDrawStart = 0;

	// Token: 0x040004B8 RID: 1208
	protected static string mLastIME = "";

	// Token: 0x040004B9 RID: 1209
	[NonSerialized]
	protected int mSelectionStart;

	// Token: 0x040004BA RID: 1210
	[NonSerialized]
	protected int mSelectionEnd;

	// Token: 0x040004BB RID: 1211
	[NonSerialized]
	protected UITexture mHighlight;

	// Token: 0x040004BC RID: 1212
	[NonSerialized]
	protected UITexture mCaret;

	// Token: 0x040004BD RID: 1213
	[NonSerialized]
	protected Texture2D mBlankTex;

	// Token: 0x040004BE RID: 1214
	[NonSerialized]
	protected float mNextBlink;

	// Token: 0x040004BF RID: 1215
	[NonSerialized]
	protected float mLastAlpha;

	// Token: 0x040004C0 RID: 1216
	[NonSerialized]
	protected string mCached = "";

	// Token: 0x040004C1 RID: 1217
	[NonSerialized]
	protected int mSelectMe = -1;

	// Token: 0x040004C2 RID: 1218
	[NonSerialized]
	protected int mSelectTime = -1;

	// Token: 0x040004C3 RID: 1219
	[NonSerialized]
	protected bool mStarted;

	// Token: 0x040004C4 RID: 1220
	[NonSerialized]
	private UIInputOnGUI mOnGUI;

	// Token: 0x040004C5 RID: 1221
	[NonSerialized]
	private UICamera mCam;

	// Token: 0x040004C6 RID: 1222
	[NonSerialized]
	private bool mEllipsis;

	// Token: 0x040004C7 RID: 1223
	public bool SelectAllTextOnClick;

	// Token: 0x040004C8 RID: 1224
	public bool TabIsFourSpaces;

	// Token: 0x040004C9 RID: 1225
	private const string FakeTab = "    ";

	// Token: 0x040004CA RID: 1226
	private SteamVR_Events.Action KeyboardCharInput;

	// Token: 0x040004CB RID: 1227
	private SteamVR_Events.Action KeyboardClosed;

	// Token: 0x040004CC RID: 1228
	private static int mIgnoreKey = 0;

	// Token: 0x0200056A RID: 1386
	public enum InputType
	{
		// Token: 0x040024B7 RID: 9399
		Standard,
		// Token: 0x040024B8 RID: 9400
		AutoCorrect,
		// Token: 0x040024B9 RID: 9401
		Password
	}

	// Token: 0x0200056B RID: 1387
	public enum Validation
	{
		// Token: 0x040024BB RID: 9403
		None,
		// Token: 0x040024BC RID: 9404
		Integer,
		// Token: 0x040024BD RID: 9405
		Float,
		// Token: 0x040024BE RID: 9406
		Alphanumeric,
		// Token: 0x040024BF RID: 9407
		Username,
		// Token: 0x040024C0 RID: 9408
		Name,
		// Token: 0x040024C1 RID: 9409
		Filename,
		// Token: 0x040024C2 RID: 9410
		Hex,
		// Token: 0x040024C3 RID: 9411
		Variable,
		// Token: 0x040024C4 RID: 9412
		Tag
	}

	// Token: 0x0200056C RID: 1388
	public enum KeyboardType
	{
		// Token: 0x040024C6 RID: 9414
		Default,
		// Token: 0x040024C7 RID: 9415
		ASCIICapable,
		// Token: 0x040024C8 RID: 9416
		NumbersAndPunctuation,
		// Token: 0x040024C9 RID: 9417
		URL,
		// Token: 0x040024CA RID: 9418
		NumberPad,
		// Token: 0x040024CB RID: 9419
		PhonePad,
		// Token: 0x040024CC RID: 9420
		NamePhonePad,
		// Token: 0x040024CD RID: 9421
		EmailAddress
	}

	// Token: 0x0200056D RID: 1389
	public enum OnReturnKey
	{
		// Token: 0x040024CF RID: 9423
		Default,
		// Token: 0x040024D0 RID: 9424
		Submit,
		// Token: 0x040024D1 RID: 9425
		NewLine
	}

	// Token: 0x0200056E RID: 1390
	// (Invoke) Token: 0x06003837 RID: 14391
	public delegate char OnValidate(string text, int charIndex, char addedChar);
}
