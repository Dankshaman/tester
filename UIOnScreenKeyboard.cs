using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class UIOnScreenKeyboard : Singleton<UIOnScreenKeyboard>
{
	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x060025D3 RID: 9683 RVA: 0x0010A734 File Offset: 0x00108934
	// (set) Token: 0x060025D4 RID: 9684 RVA: 0x0010A73B File Offset: 0x0010893B
	public static float SCALE
	{
		get
		{
			return UIOnScreenKeyboard._SCALE;
		}
		set
		{
			if (UIOnScreenKeyboard._SCALE != value)
			{
				UIOnScreenKeyboard._SCALE = value;
				Singleton<UIOnScreenKeyboard>.Instance.transform.parent.localScale = new Vector3(UIOnScreenKeyboard._SCALE, UIOnScreenKeyboard._SCALE, UIOnScreenKeyboard._SCALE);
			}
		}
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x060025D5 RID: 9685 RVA: 0x0010A773 File Offset: 0x00108973
	// (set) Token: 0x060025D6 RID: 9686 RVA: 0x0010A77C File Offset: 0x0010897C
	public static UIOnScreenKeyboard.KeyboardDefaultState DEFAULT_KEYBOARD_STATE
	{
		get
		{
			return UIOnScreenKeyboard._DefaultKeyboardState;
		}
		set
		{
			Singleton<UISettings>.Instance.keyboardDisabled.value = (value == UIOnScreenKeyboard.KeyboardDefaultState.Disabled);
			Singleton<UISettings>.Instance.keyboardInVR.value = (value == UIOnScreenKeyboard.KeyboardDefaultState.EnabledInVR);
			Singleton<UISettings>.Instance.keyboardEnabled.value = (value == UIOnScreenKeyboard.KeyboardDefaultState.Enabled);
			UIOnScreenKeyboard._DefaultKeyboardState = value;
		}
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x060025D7 RID: 9687 RVA: 0x0010A7C8 File Offset: 0x001089C8
	// (set) Token: 0x060025D8 RID: 9688 RVA: 0x0010A7CF File Offset: 0x001089CF
	public static bool ON_SCREEN_KEYBOARD
	{
		get
		{
			return UIOnScreenKeyboard._OnScreenKeyboard;
		}
		set
		{
			if (value != UIOnScreenKeyboard._OnScreenKeyboard)
			{
				UIOnScreenKeyboard._OnScreenKeyboard = value;
				UIOnScreenKeyboard.SetActive(value);
				if (value)
				{
					Singleton<UIOnScreenKeyboard>.Instance.OnActivate();
				}
			}
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x060025D9 RID: 9689 RVA: 0x0010A7F2 File Offset: 0x001089F2
	// (set) Token: 0x060025DA RID: 9690 RVA: 0x0010A801 File Offset: 0x00108A01
	public static bool EnterPressed
	{
		get
		{
			return Singleton<UIOnScreenKeyboard>.Instance._EnterPressed == 1;
		}
		set
		{
			if (value)
			{
				Singleton<UIOnScreenKeyboard>.Instance._EnterPressed = 2;
				return;
			}
			Singleton<UIOnScreenKeyboard>.Instance._EnterPressed = 0;
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x060025DB RID: 9691 RVA: 0x0010A81D File Offset: 0x00108A1D
	// (set) Token: 0x060025DC RID: 9692 RVA: 0x0010A82C File Offset: 0x00108A2C
	public static bool TabPressed
	{
		get
		{
			return Singleton<UIOnScreenKeyboard>.Instance._TabPressed == 1;
		}
		set
		{
			if (value)
			{
				Singleton<UIOnScreenKeyboard>.Instance._TabPressed = 2;
				return;
			}
			Singleton<UIOnScreenKeyboard>.Instance._TabPressed = 0;
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x060025DD RID: 9693 RVA: 0x0010A848 File Offset: 0x00108A48
	// (set) Token: 0x060025DE RID: 9694 RVA: 0x0010A857 File Offset: 0x00108A57
	public static bool UpPressed
	{
		get
		{
			return Singleton<UIOnScreenKeyboard>.Instance._UpPressed == 1;
		}
		set
		{
			if (value)
			{
				Singleton<UIOnScreenKeyboard>.Instance._UpPressed = 2;
				return;
			}
			Singleton<UIOnScreenKeyboard>.Instance._UpPressed = 0;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x060025DF RID: 9695 RVA: 0x0010A873 File Offset: 0x00108A73
	// (set) Token: 0x060025E0 RID: 9696 RVA: 0x0010A882 File Offset: 0x00108A82
	public static bool DownPressed
	{
		get
		{
			return Singleton<UIOnScreenKeyboard>.Instance._DownPressed == 1;
		}
		set
		{
			if (value)
			{
				Singleton<UIOnScreenKeyboard>.Instance._DownPressed = 2;
				return;
			}
			Singleton<UIOnScreenKeyboard>.Instance._DownPressed = 0;
		}
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x0010A8A0 File Offset: 0x00108AA0
	private void Update()
	{
		base.transform.parent.GetComponent<UIPanel>().depth = -1;
		if (UIOnScreenKeyboard.READOUT_DISPLAY_DURATION > 0f)
		{
			if (this.Readout.activeInHierarchy)
			{
				if (UICamera.selectedObject && UICamera.selectedObject.activeInHierarchy)
				{
					UIInput component = UICamera.selectedObject.GetComponent<UIInput>();
					if (component && component.value != "" && !component.readOnly)
					{
						string text = component.GetLeftText();
						string text2 = component.GetRightText();
						while (text.Length + text2.Length > this.MaxReadoutLength)
						{
							if (text.Length > text2.Length)
							{
								text = text.Substring(1);
							}
							else
							{
								text2 = text2.Substring(0, text2.Length - 1);
							}
						}
						if (Time.time * 1.75f % 2f < 1f)
						{
							this.Caret = '|';
						}
						else
						{
							this.Caret = ' ';
						}
						this.Readout.GetComponent<UILabel>().text = text + this.Caret.ToString() + text2;
					}
					else
					{
						this.Readout.GetComponent<UILabel>().text = "";
					}
				}
				else
				{
					this.Readout.GetComponent<UILabel>().text = "";
				}
				if (this.readOutDisplayFadeFrom == 0f)
				{
					if (Time.time - this.lastButtonHit >= UIOnScreenKeyboard.READOUT_DISPLAY_DURATION)
					{
						this.readOutDisplayFadeFrom = Time.time;
						return;
					}
				}
				else
				{
					if (Time.time > this.readOutDisplayFadeFrom + 1f)
					{
						this.Readout.SetActive(false);
						return;
					}
					Color color = this.Readout.GetComponent<UILabel>().color;
					color.a = Mathf.Lerp(1f, 0f, (Time.time - this.readOutDisplayFadeFrom) / 1f);
					this.Readout.GetComponent<UILabel>().color = color;
					return;
				}
			}
			else if (Time.time - this.lastButtonHit < UIOnScreenKeyboard.READOUT_DISPLAY_DURATION)
			{
				this.DisplayReadout();
			}
		}
	}

	// Token: 0x060025E2 RID: 9698 RVA: 0x0010AAB0 File Offset: 0x00108CB0
	public void DisplayReadout()
	{
		this.Readout.SetActive(true);
		Color color = this.Readout.GetComponent<UILabel>().color;
		color.a = 1f;
		this.Readout.GetComponent<UILabel>().color = color;
		this.readOutDisplayFadeFrom = 0f;
		this.Readout.GetComponent<UILabel>().text = "";
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x0010AB18 File Offset: 0x00108D18
	private void LateUpdate()
	{
		if (this._EnterPressed > 0)
		{
			this._EnterPressed--;
		}
		if (this._TabPressed > 0)
		{
			this._TabPressed--;
		}
		if (this._UpPressed > 0)
		{
			this._UpPressed--;
		}
		if (this._DownPressed > 0)
		{
			this._DownPressed--;
		}
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x0010AB84 File Offset: 0x00108D84
	private void Start()
	{
		base.GetComponent<UIDragObject>().TweenInDuration = 0f;
		Vector3 vector = base.transform.position;
		vector.x = PlayerPrefs.GetFloat("OnScreenKeyboardX", vector.x);
		vector.y = PlayerPrefs.GetFloat("OnScreenKeyboardY", vector.y);
		base.transform.position = vector;
		base.GetComponent<UIWidget>().RefuseFocus = true;
		base.GetComponent<UIWidget>().width = (int)(14f * Mathf.Abs(41f));
		base.GetComponent<UIWidget>().height = (int)(6f * Mathf.Abs(-41f));
		this.fontSize = this.ButtonTemplate.transform.Find("Label").GetComponent<UILabel>().fontSize;
		int num = 500;
		for (int i = 0; i < this.rows.Length; i++)
		{
			float num2 = 0f;
			for (int j = 0; j < this.rows[i].Length; j++)
			{
				char c = this.rows[i][j];
				int num3 = 1;
				while (j + num3 < this.rows[i].Length && this.rows[i][j + num3] == c)
				{
					num3++;
				}
				float num4 = (float)num3;
				float num5 = 0f;
				if (c == 'S' || c == 'Z' || c == 'T' || c == 'B')
				{
					num4 += 0.5f;
					if (j < 6)
					{
						num5 = 0.5f;
					}
				}
				else if (c == 'C')
				{
					num2 += 0.5f;
				}
				if (c != ' ')
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ButtonTemplate);
					gameObject.transform.parent = this.ButtonTemplate.transform.parent;
					gameObject.transform.position = this.ButtonTemplate.transform.parent.position;
					gameObject.transform.rotation = this.ButtonTemplate.transform.parent.rotation;
					gameObject.transform.localScale = this.ButtonTemplate.transform.localScale;
					num = (gameObject.GetComponent<UIWidget>().depth = num + 1);
					gameObject.GetComponent<UIWidget>().RefuseFocus = true;
					Vector2 offset = gameObject.GetComponent<BoxCollider2D>().offset;
					offset.x -= 1f;
					offset.y -= 1f;
					gameObject.GetComponent<BoxCollider2D>().offset = offset;
					Vector2 size = gameObject.GetComponent<BoxCollider2D>().size;
					size.x += 2f;
					size.y += 2f;
					gameObject.GetComponent<BoxCollider2D>().size = size;
					vector = Vector3.zero;
					float num6 = -8f + (float)i * 0.5f;
					num6 += 0.5f * (num4 - 1f) + num5 + num2;
					vector.x = (float)j * 41f + num6 * 41f;
					vector.y = (float)i * -41f + 82f;
					gameObject.transform.localPosition = vector;
					int num7 = (num3 > 2) ? 3 : 1;
					gameObject.GetComponent<UISprite>().width = (int)((float)(gameObject.GetComponent<UISprite>().width - num7) * num4 + (float)(num7 * 2));
					UILabel component = gameObject.transform.Find("Label").GetComponent<UILabel>();
					component.text = UIOnScreenKeyboard.getLabel(this.rows[i][j]);
					if (c >= 'A' && c <= 'Z')
					{
						component.effectStyle = UILabel.Effect.Shadow;
					}
					num = (component.GetComponent<UILabel>().depth = num + 1);
					gameObject.name = c.ToString();
					UIEventListener uieventListener = UIEventListener.Get(gameObject);
					uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.PerformKeyAction));
					this.keyButton[c] = gameObject;
					gameObject.SetActive(true);
				}
				j += num3 - 1;
			}
		}
		this.Readout = UnityEngine.Object.Instantiate<GameObject>(this.ButtonTemplate.transform.Find("Label").gameObject);
		this.Readout.SetActive(false);
		this.Readout.transform.parent = this.ButtonTemplate.transform.parent;
		vector = base.transform.position;
		vector.y += 0.007f;
		this.Readout.transform.position = vector;
		this.Readout.transform.rotation = this.ButtonTemplate.transform.parent.rotation;
		this.Readout.transform.localScale = this.ButtonTemplate.transform.localScale;
		this.Readout.name = "Read-Out";
		this.Readout.GetComponent<UIWidget>().width = base.GetComponent<UIWidget>().width;
		this.Readout.GetComponent<UIWidget>().depth = num + 1;
		this.Readout.GetComponent<UIWidget>().RefuseFocus = true;
		this.Readout.GetComponent<UILabel>().color = Color.white;
		this.Readout.GetComponent<UILabel>().effectColor = Color.black;
		this.Readout.GetComponent<UILabel>().effectStyle = UILabel.Effect.Outline;
		this.Readout.GetComponent<UILabel>().effectDistance = new Vector2(1f, 1f);
		this.OnActivate();
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x0010B109 File Offset: 0x00109309
	private void OnActivate()
	{
		if (UIOnScreenKeyboard.READOUT_DISPLAY_DURATION == 0f)
		{
			this.DisplayReadout();
		}
		this.UpdateButtons();
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x0010B124 File Offset: 0x00109324
	private void OnDestroy()
	{
		Vector3 position = base.transform.position;
		PlayerPrefs.SetFloat("OnScreenKeyboardX", position.x);
		PlayerPrefs.SetFloat("OnScreenKeyboardY", position.y);
		foreach (KeyValuePair<char, GameObject> keyValuePair in this.keyButton)
		{
			UIEventListener uieventListener = UIEventListener.Get(keyValuePair.Value);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.PerformKeyAction));
		}
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x0010B1CC File Offset: 0x001093CC
	private void PerformKeyAction(GameObject button)
	{
		this.lastButtonHit = Time.time;
		char c = button.name[0];
		Event @event = new Event();
		bool flag = true;
		UIInput uiinput = null;
		if (UICamera.selectedObject)
		{
			uiinput = UICamera.selectedObject.GetComponent<UIInput>();
		}
		switch (c)
		{
		case 'B':
			@event.keyCode = KeyCode.Backspace;
			break;
		case 'C':
			UIOnScreenKeyboard.Ctrl = !UIOnScreenKeyboard.Ctrl;
			flag = false;
			break;
		case 'D':
			if (!UIOnScreenKeyboard.Ctrl)
			{
				@event.keyCode = KeyCode.DownArrow;
				UIOnScreenKeyboard.DownPressed = true;
			}
			else
			{
				@event.keyCode = KeyCode.PageDown;
			}
			break;
		default:
			switch (c)
			{
			case 'L':
				if (!UIOnScreenKeyboard.Ctrl)
				{
					@event.keyCode = KeyCode.LeftArrow;
					goto IL_26D;
				}
				@event.keyCode = KeyCode.Home;
				goto IL_26D;
			case 'M':
			case 'O':
			case 'P':
			case 'Q':
			case 'V':
			case 'W':
			case 'Y':
				break;
			case 'N':
				@event.keyCode = KeyCode.Return;
				if (uiinput)
				{
					@event.character = '\n';
				}
				else
				{
					if (!UICamera.inputHasFocus && !UICamera.SelectIsInput())
					{
						UICamera.selectedObject = UIOnScreenKeyboard.KeyBindingObject;
					}
					flag = false;
				}
				UIOnScreenKeyboard.EnterPressed = true;
				goto IL_26D;
			case 'R':
				if (!UIOnScreenKeyboard.Ctrl)
				{
					@event.keyCode = KeyCode.RightArrow;
					goto IL_26D;
				}
				@event.keyCode = KeyCode.End;
				goto IL_26D;
			case 'S':
			case 'Z':
				UIOnScreenKeyboard.Shift = !UIOnScreenKeyboard.Shift;
				flag = false;
				goto IL_26D;
			case 'T':
				@event.keyCode = KeyCode.Tab;
				UIOnScreenKeyboard.TabPressed = true;
				goto IL_26D;
			case 'U':
				if (!UIOnScreenKeyboard.Ctrl)
				{
					@event.keyCode = KeyCode.UpArrow;
					UIOnScreenKeyboard.UpPressed = true;
					goto IL_26D;
				}
				@event.keyCode = KeyCode.PageUp;
				goto IL_26D;
			case 'X':
				@event.keyCode = KeyCode.Space;
				@event.character = ' ';
				goto IL_26D;
			default:
				if (c == '~')
				{
					if (UIOnScreenKeyboard.Shift)
					{
						@event.keyCode = (KeyCode)c;
						@event.character = c;
						goto IL_26D;
					}
					if (NetworkSingleton<Chat>.Instance.isConsoleSwitching)
					{
						goto IL_26D;
					}
					NetworkSingleton<Chat>.Instance.ChatWindow.SetActive(true);
					if (NetworkSingleton<Chat>.Instance.selectedTab.chatType == ChatMessageType.System)
					{
						NetworkSingleton<Chat>.Instance.previousTab.SendMessage("OnClick");
						goto IL_26D;
					}
					Singleton<SystemConsole>.Instance.SystemTab.SendMessage("OnClick");
					goto IL_26D;
				}
				break;
			}
			@event.keyCode = (KeyCode)c;
			@event.character = UIOnScreenKeyboard.getLabel(c)[0];
			break;
		}
		IL_26D:
		if (flag)
		{
			if (uiinput)
			{
				uiinput.ProcessEvent(@event, true, UIOnScreenKeyboard.Shift, UIOnScreenKeyboard.Ctrl);
			}
			UIOnScreenKeyboard.Shift = false;
			UIOnScreenKeyboard.Ctrl = false;
		}
		this.UpdateButtons();
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x0010B478 File Offset: 0x00109678
	private static string getLabel(char key)
	{
		if (key != ' ')
		{
			switch (key)
			{
			case 'B':
				return "◁";
			case 'C':
				return "CTRL";
			case 'D':
				if (!UIOnScreenKeyboard.Ctrl)
				{
					return "⇩";
				}
				return "PGDN";
			case 'E':
				return "⎋";
			default:
				switch (key)
				{
				case 'L':
					if (!UIOnScreenKeyboard.Ctrl)
					{
						return "⇦";
					}
					return "HOME";
				case 'N':
					return "⏎";
				case 'R':
					if (!UIOnScreenKeyboard.Ctrl)
					{
						return "⇨";
					}
					return "END";
				case 'S':
					return "SHIFT";
				case 'T':
					return "TAB";
				case 'U':
					goto IL_98;
				case 'X':
					return "";
				case 'Z':
					return "SHIFT";
				}
				if (key >= 'a' && key <= 'z')
				{
					if (UIOnScreenKeyboard.Shift)
					{
						return key.ToString().ToUpper();
					}
					return key.ToString();
				}
				else
				{
					if (UIOnScreenKeyboard.Shift)
					{
						switch (key)
						{
						case '!':
							return ":";
						case '"':
							return "'";
						case '#':
						case '$':
						case '%':
						case '&':
						case '\'':
						case '(':
						case ')':
						case '*':
						case ':':
						case ';':
						case '<':
						case '>':
							break;
						case '+':
							return "]";
						case ',':
							return "<";
						case '-':
							return "[";
						case '.':
							return ">";
						case '/':
							return "\\";
						case '0':
							return ")";
						case '1':
							return "!";
						case '2':
							return "\"";
						case '3':
							return "£";
						case '4':
							return "$";
						case '5':
							return "%";
						case '6':
							return "^";
						case '7':
							return "&";
						case '8':
							return "*";
						case '9':
							return "(";
						case '=':
							return "}";
						case '?':
							return ";";
						default:
							if (key == '_')
							{
								return "{";
							}
							break;
						}
						return key.ToString();
					}
					return key.ToString();
				}
				break;
			}
		}
		IL_98:
		if (!UIOnScreenKeyboard.Ctrl)
		{
			return "⇧";
		}
		return "PGUP";
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x0010B6AC File Offset: 0x001098AC
	public static void SetActive(bool active)
	{
		Singleton<UIOnScreenKeyboard>.Instance.gameObject.SetActive(active);
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x0010B6C0 File Offset: 0x001098C0
	public void UpdateButtons()
	{
		bool flag = false;
		foreach (KeyValuePair<char, GameObject> keyValuePair in this.keyButton)
		{
			char key = keyValuePair.Key;
			GameObject value = keyValuePair.Value;
			if (key != 'C')
			{
				if ((key == 'S' || key == 'Z') && UIOnScreenKeyboard.Shift != UIOnScreenKeyboard.displayedShift)
				{
					flag = true;
					if (UIOnScreenKeyboard.Shift)
					{
						value.GetComponent<UIButtonColor>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonPressed];
					}
					else
					{
						value.GetComponent<UIButtonColor>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
					}
					value.GetComponent<UIButtonColor>().UpdateColor(false);
				}
			}
			else if (UIOnScreenKeyboard.Ctrl != UIOnScreenKeyboard.displayedCtrl)
			{
				flag = true;
				if (UIOnScreenKeyboard.Ctrl)
				{
					value.GetComponent<UIButtonColor>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonPressed];
				}
				else
				{
					value.GetComponent<UIButtonColor>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
				}
				value.GetComponent<UIButtonColor>().UpdateColor(false);
			}
			UILabel component = value.transform.Find("Label").GetComponent<UILabel>();
			component.text = UIOnScreenKeyboard.getLabel(key);
			if (component.text.Length > 1)
			{
				component.fontSize = this.fontSize / 2;
			}
			else
			{
				component.fontSize = this.fontSize;
			}
		}
		if (flag)
		{
			UIOnScreenKeyboard.displayedShift = UIOnScreenKeyboard.Shift;
			UIOnScreenKeyboard.displayedCtrl = UIOnScreenKeyboard.Ctrl;
		}
	}

	// Token: 0x0400187C RID: 6268
	private static float _SCALE = 1f;

	// Token: 0x0400187D RID: 6269
	public static float READOUT_DISPLAY_DURATION = 10f;

	// Token: 0x0400187E RID: 6270
	private const float OFFSET_X = -8f;

	// Token: 0x0400187F RID: 6271
	private const float OFFSET_Y = -2f;

	// Token: 0x04001880 RID: 6272
	private const float STEP_X = 41f;

	// Token: 0x04001881 RID: 6273
	private const float STEP_Y = -41f;

	// Token: 0x04001882 RID: 6274
	private const float READOUT_FADE_DURATION = 1f;

	// Token: 0x04001883 RID: 6275
	private const float BLINK_RATE = 1.75f;

	// Token: 0x04001884 RID: 6276
	private const char CARET = '|';

	// Token: 0x04001885 RID: 6277
	private int fontSize;

	// Token: 0x04001886 RID: 6278
	private int MaxReadoutLength = 50;

	// Token: 0x04001887 RID: 6279
	public static GameObject KeyBindingObject;

	// Token: 0x04001888 RID: 6280
	private static UIOnScreenKeyboard.KeyboardDefaultState _DefaultKeyboardState = UIOnScreenKeyboard.KeyboardDefaultState.EnabledInVR;

	// Token: 0x04001889 RID: 6281
	private static bool _OnScreenKeyboard = false;

	// Token: 0x0400188A RID: 6282
	private int _EnterPressed;

	// Token: 0x0400188B RID: 6283
	private int _TabPressed;

	// Token: 0x0400188C RID: 6284
	private int _UpPressed;

	// Token: 0x0400188D RID: 6285
	private int _DownPressed;

	// Token: 0x0400188E RID: 6286
	private GameObject Readout;

	// Token: 0x0400188F RID: 6287
	public GameObject ButtonTemplate;

	// Token: 0x04001890 RID: 6288
	public UISprite background;

	// Token: 0x04001891 RID: 6289
	private static bool Shift = false;

	// Token: 0x04001892 RID: 6290
	private static bool Ctrl = false;

	// Token: 0x04001893 RID: 6291
	private static bool displayedShift = false;

	// Token: 0x04001894 RID: 6292
	private static bool displayedCtrl = false;

	// Token: 0x04001895 RID: 6293
	private float lastButtonHit = -999f;

	// Token: 0x04001896 RID: 6294
	private float readOutDisplayFadeFrom;

	// Token: 0x04001897 RID: 6295
	private char Caret = '|';

	// Token: 0x04001898 RID: 6296
	private Dictionary<char, GameObject> keyButton = new Dictionary<char, GameObject>();

	// Token: 0x04001899 RID: 6297
	private readonly string[] rows = new string[]
	{
		"  ~1234567890-+",
		" T qwertyuiopB",
		" !\"asdfghjklNN",
		"S ?zxcvbnm/UZ",
		"C_=XXXX,.LDR"
	};

	// Token: 0x02000772 RID: 1906
	public enum KeyboardDefaultState
	{
		// Token: 0x04002BF5 RID: 11253
		Disabled,
		// Token: 0x04002BF6 RID: 11254
		EnabledInVR,
		// Token: 0x04002BF7 RID: 11255
		Enabled
	}
}
