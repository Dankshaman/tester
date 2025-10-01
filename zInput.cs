using System;
using System.Collections.Generic;
using TouchScript;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class zInput
{
	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06002A7E RID: 10878 RVA: 0x0012E9B5 File Offset: 0x0012CBB5
	// (set) Token: 0x06002A7F RID: 10879 RVA: 0x0012E9BC File Offset: 0x0012CBBC
	public static bool bTouching
	{
		get
		{
			return zInput._bTouching;
		}
		set
		{
			if (zInput._bTouching != value)
			{
				zInput._bTouching = value;
				if (TouchScriptNGUI.uiCamera)
				{
					TouchScriptNGUI.uiCamera.useMouse = !value;
					TouchScriptNGUI.uiCamera.useTouch = !value;
				}
				if (zInput.bTouching && !zInput.bTouchEventFired)
				{
					zInput.bTouchEventFired = true;
					Chat.Log(string.Concat(new object[]
					{
						"Touch Detected! Dots per cm: ",
						TouchManager.Instance.DotsPerCentimeter,
						" DPI: ",
						TouchManager.Instance.DPI
					}), ChatMessageType.All);
				}
			}
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06002A80 RID: 10880 RVA: 0x0012EA5B File Offset: 0x0012CC5B
	// (set) Token: 0x06002A81 RID: 10881 RVA: 0x0012EA62 File Offset: 0x0012CC62
	public static bool bTouch
	{
		get
		{
			return zInput._btouch;
		}
		set
		{
			if (zInput._btouch != value)
			{
				zInput._btouch = value;
				Singleton<TouchScriptNGUI>.Instance.EnableTouch(value);
			}
		}
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x0012EA80 File Offset: 0x0012CC80
	private static string GetControllerButtonName(string ButtonName)
	{
		if (zInput.ControllerDictionary.ContainsKey(ButtonName))
		{
			return zInput.ControllerDictionary[ButtonName];
		}
		string text = ButtonName + " (Controller)";
		zInput.ControllerDictionary.Add(ButtonName, text);
		return text;
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x0012EAC0 File Offset: 0x0012CCC0
	public static bool GetButton(string ButtonName, ControlType CT = ControlType.All)
	{
		if (zInput.bTouching)
		{
			return false;
		}
		if (CT == ControlType.All)
		{
			return cInput.GetKey(ButtonName) || (zInput.bController && zInput.TryGetButton(zInput.GetControllerButtonName(ButtonName)));
		}
		if (CT == ControlType.Keyboard)
		{
			return cInput.GetKey(ButtonName);
		}
		return zInput.bController && zInput.TryGetButton(zInput.GetControllerButtonName(ButtonName));
	}

	// Token: 0x06002A84 RID: 10884 RVA: 0x0012EB17 File Offset: 0x0012CD17
	private static bool TryGetButton(string ButtonName)
	{
		return cInput.IsKeyDefined(ButtonName) && cInput.GetKey(ButtonName);
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x0012EB2C File Offset: 0x0012CD2C
	public static bool GetButtonDown(string ButtonName, ControlType CT = ControlType.All)
	{
		if (zInput.bTouching)
		{
			return false;
		}
		if (CT == ControlType.All)
		{
			return cInput.GetKeyDown(ButtonName) || (zInput.bController && zInput.TryGetButtonDown(zInput.GetControllerButtonName(ButtonName)));
		}
		if (CT == ControlType.Keyboard)
		{
			return cInput.GetKeyDown(ButtonName);
		}
		return zInput.bController && zInput.TryGetButtonDown(zInput.GetControllerButtonName(ButtonName));
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x0012EB83 File Offset: 0x0012CD83
	private static bool TryGetButtonDown(string ButtonName)
	{
		return cInput.IsKeyDefined(ButtonName) && cInput.GetKeyDown(ButtonName);
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x0012EB98 File Offset: 0x0012CD98
	public static bool GetButtonUp(string ButtonName, ControlType CT = ControlType.All)
	{
		if (zInput.bTouching)
		{
			return false;
		}
		if (CT == ControlType.All)
		{
			return cInput.GetKeyUp(ButtonName) || (zInput.bController && zInput.TryGetButtonUp(zInput.GetControllerButtonName(ButtonName)));
		}
		if (CT == ControlType.Keyboard)
		{
			return cInput.GetKeyUp(ButtonName);
		}
		return zInput.bController && zInput.TryGetButtonUp(zInput.GetControllerButtonName(ButtonName));
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x0012EBEF File Offset: 0x0012CDEF
	private static bool TryGetButtonUp(string ButtonName)
	{
		return cInput.IsKeyDefined(ButtonName) && cInput.GetKeyUp(ButtonName);
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x0012EC04 File Offset: 0x0012CE04
	public static float GetAxis(string ButtonName, ControlType CT = ControlType.All, bool bOverrideControllerCheck = false)
	{
		if (zInput.bTouching)
		{
			return 0f;
		}
		float num = 0f;
		if (CT == ControlType.All || CT == ControlType.Keyboard)
		{
			num += cInput.GetAxis(ButtonName);
		}
		if ((zInput.bController || bOverrideControllerCheck) && (CT == ControlType.All || CT == ControlType.Controller))
		{
			num += zInput.TryGetAxis(zInput.GetControllerButtonName(ButtonName));
		}
		return num;
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x0012EC53 File Offset: 0x0012CE53
	private static float TryGetAxis(string ButtonName)
	{
		if (cInput.IsAxisDefined(ButtonName))
		{
			return cInput.GetAxis(ButtonName);
		}
		return 0f;
	}

	// Token: 0x06002A8B RID: 10891 RVA: 0x0012EC69 File Offset: 0x0012CE69
	public static bool GetRecursiveButton(string ButtonName, ref float LastTime, float Interval = 0.1f, ControlType CT = ControlType.All)
	{
		if (zInput.GetButton(ButtonName, CT) && Time.time > LastTime + Interval)
		{
			LastTime = Time.time;
			return true;
		}
		return false;
	}

	// Token: 0x06002A8C RID: 10892 RVA: 0x0012EC8C File Offset: 0x0012CE8C
	public static bool GetScriptingButtonDown(int index)
	{
		return zInput.GetButtonDown(zInput.ScriptingButtons[index].Name, ControlType.All);
	}

	// Token: 0x06002A8D RID: 10893 RVA: 0x0012ECB4 File Offset: 0x0012CEB4
	public static bool GetScriptingButtonUp(int index)
	{
		return zInput.GetButtonUp(zInput.ScriptingButtons[index].Name, ControlType.All);
	}

	// Token: 0x04001CDF RID: 7391
	private static bool _bTouching = false;

	// Token: 0x04001CE0 RID: 7392
	public static bool bTouchEventFired = false;

	// Token: 0x04001CE1 RID: 7393
	private static bool _btouch = false;

	// Token: 0x04001CE2 RID: 7394
	public static bool bController = false;

	// Token: 0x04001CE3 RID: 7395
	public const string CONTROLLER_BINDING = " (Controller)";

	// Token: 0x04001CE4 RID: 7396
	private static Dictionary<string, string> ControllerDictionary = new Dictionary<string, string>();

	// Token: 0x04001CE5 RID: 7397
	public static ControlType CurrentControlType = ControlType.Keyboard;

	// Token: 0x04001CE6 RID: 7398
	public static readonly Dictionary<int, zInput.KeyInfo> ScriptingButtons = new Dictionary<int, zInput.KeyInfo>
	{
		{
			1,
			new zInput.KeyInfo("Scripting 1", "Keypad1")
		},
		{
			2,
			new zInput.KeyInfo("Scripting 2", "Keypad2")
		},
		{
			3,
			new zInput.KeyInfo("Scripting 3", "Keypad3")
		},
		{
			4,
			new zInput.KeyInfo("Scripting 4", "Keypad4")
		},
		{
			5,
			new zInput.KeyInfo("Scripting 5", "Keypad5")
		},
		{
			6,
			new zInput.KeyInfo("Scripting 6", "Keypad6")
		},
		{
			7,
			new zInput.KeyInfo("Scripting 7", "Keypad7")
		},
		{
			8,
			new zInput.KeyInfo("Scripting 8", "Keypad8")
		},
		{
			9,
			new zInput.KeyInfo("Scripting 9", "Keypad9")
		},
		{
			10,
			new zInput.KeyInfo("Scripting 10", "Keypad0")
		}
	};

	// Token: 0x020007C2 RID: 1986
	public struct KeyInfo
	{
		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06003FBF RID: 16319 RVA: 0x0018247E File Offset: 0x0018067E
		// (set) Token: 0x06003FC0 RID: 16320 RVA: 0x00182486 File Offset: 0x00180686
		public string Name { get; private set; }

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06003FC1 RID: 16321 RVA: 0x0018248F File Offset: 0x0018068F
		// (set) Token: 0x06003FC2 RID: 16322 RVA: 0x00182497 File Offset: 0x00180697
		public string Key { get; private set; }

		// Token: 0x06003FC3 RID: 16323 RVA: 0x001824A0 File Offset: 0x001806A0
		public KeyInfo(string Name, string Key)
		{
			this.Name = Name;
			this.Key = Key;
		}
	}
}
