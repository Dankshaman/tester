using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000043 RID: 67
[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060001AC RID: 428 RVA: 0x0000AB68 File Offset: 0x00008D68
	public string captionText
	{
		get
		{
			string text = NGUITools.KeyToCaption(this.keyCode);
			if (this.modifier == UIKeyBinding.Modifier.Alt)
			{
				return "Alt+" + text;
			}
			if (this.modifier == UIKeyBinding.Modifier.Control)
			{
				return "Control+" + text;
			}
			if (this.modifier == UIKeyBinding.Modifier.Shift)
			{
				return "Shift+" + text;
			}
			return text;
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000ABC4 File Offset: 0x00008DC4
	public static bool IsBound(KeyCode key)
	{
		int i = 0;
		int count = UIKeyBinding.mList.Count;
		while (i < count)
		{
			UIKeyBinding uikeyBinding = UIKeyBinding.mList[i];
			if (uikeyBinding != null && uikeyBinding.keyCode == key)
			{
				return true;
			}
			i++;
		}
		return false;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000AC09 File Offset: 0x00008E09
	protected virtual void OnEnable()
	{
		UIKeyBinding.mList.Add(this);
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000AC16 File Offset: 0x00008E16
	protected virtual void OnDisable()
	{
		UIKeyBinding.mList.Remove(this);
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x0000AC24 File Offset: 0x00008E24
	protected virtual void Start()
	{
		UIOnScreenKeyboard.KeyBindingObject = base.gameObject;
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = (component != null);
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000AC74 File Offset: 0x00008E74
	protected virtual void OnSubmit()
	{
		if ((UICamera.currentKey == this.keyCode || UICamera.currentKey.ToString() == cInput.GetText(this.cInputName, 1) || UICamera.currentKey.ToString() == cInput.GetText(this.cInputName, 2)) && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000ACE9 File Offset: 0x00008EE9
	protected virtual bool IsModifierActive()
	{
		return UIKeyBinding.IsModifierActive(this.modifier);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000ACF8 File Offset: 0x00008EF8
	public static bool IsModifierActive(UIKeyBinding.Modifier modifier)
	{
		if (modifier == UIKeyBinding.Modifier.Any)
		{
			return true;
		}
		if (modifier == UIKeyBinding.Modifier.Alt)
		{
			if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
			{
				return true;
			}
		}
		else if (modifier == UIKeyBinding.Modifier.Control)
		{
			if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
			{
				return true;
			}
		}
		else if (modifier == UIKeyBinding.Modifier.Shift)
		{
			if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
			{
				return true;
			}
		}
		else if (modifier == UIKeyBinding.Modifier.None)
		{
			return !UICamera.GetKey(KeyCode.LeftAlt) && !UICamera.GetKey(KeyCode.RightAlt) && !UICamera.GetKey(KeyCode.LeftControl) && !UICamera.GetKey(KeyCode.RightControl) && !UICamera.GetKey(KeyCode.LeftShift) && !UICamera.GetKey(KeyCode.RightShift);
		}
		return false;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000ADF8 File Offset: 0x00008FF8
	protected virtual void Update()
	{
		if (UICamera.inputHasFocus)
		{
			return;
		}
		if ((this.keyCode == KeyCode.None && this.cInputName == "") || !this.IsModifierActive())
		{
			return;
		}
		bool flag = UICamera.GetKeyDown(this.keyCode);
		bool flag2 = UICamera.GetKeyUp(this.keyCode);
		if (flag)
		{
			this.mPress = true;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
		{
			if (UICamera.SelectIsInput())
			{
				return;
			}
			if (flag || (this.cInputName != "" && cInput.GetKeyDown(this.cInputName)))
			{
				UICamera.currentTouchID = -1;
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(true);
			}
			if ((this.mPress && flag2) || (this.cInputName != "" && cInput.GetKeyUp(this.cInputName)))
			{
				UICamera.currentTouchID = -1;
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(false);
				this.OnBindingClick();
			}
		}
		if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && (flag2 || (this.cInputName != "" && cInput.GetKeyUp(this.cInputName))))
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus && !UICamera.SelectIsInput())
				{
					UICamera.selectedObject = base.gameObject;
				}
				this.mIgnoreUp = false;
			}
			else
			{
				UICamera.hoveredObject = base.gameObject;
			}
		}
		if (flag2)
		{
			this.mPress = false;
		}
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000AF74 File Offset: 0x00009174
	protected virtual void OnBindingPress(bool pressed)
	{
		UICamera.Notify(base.gameObject, "OnPress", pressed);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000AF8C File Offset: 0x0000918C
	protected virtual void OnBindingClick()
	{
		UICamera.Notify(base.gameObject, "OnClick", null);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000AF9F File Offset: 0x0000919F
	public override string ToString()
	{
		return UIKeyBinding.GetString(this.keyCode, this.modifier);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000AFB2 File Offset: 0x000091B2
	public static string GetString(KeyCode keyCode, UIKeyBinding.Modifier modifier)
	{
		if (modifier == UIKeyBinding.Modifier.None)
		{
			return keyCode.ToString();
		}
		return modifier + "+" + keyCode;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000AFDC File Offset: 0x000091DC
	public static bool GetKeyCode(string text, out KeyCode key, out UIKeyBinding.Modifier modifier)
	{
		key = KeyCode.None;
		modifier = UIKeyBinding.Modifier.None;
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		if (text.Contains("+"))
		{
			string[] array = text.Split(new char[]
			{
				'+'
			});
			try
			{
				modifier = (UIKeyBinding.Modifier)Enum.Parse(typeof(UIKeyBinding.Modifier), array[0]);
				key = (KeyCode)Enum.Parse(typeof(KeyCode), array[1]);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		modifier = UIKeyBinding.Modifier.None;
		try
		{
			key = (KeyCode)Enum.Parse(typeof(KeyCode), text);
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000B094 File Offset: 0x00009294
	public static UIKeyBinding.Modifier GetActiveModifier()
	{
		UIKeyBinding.Modifier result = UIKeyBinding.Modifier.None;
		if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
		{
			result = UIKeyBinding.Modifier.Alt;
		}
		else if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
		{
			result = UIKeyBinding.Modifier.Shift;
		}
		else if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
		{
			result = UIKeyBinding.Modifier.Control;
		}
		return result;
	}

	// Token: 0x04000177 RID: 375
	private static List<UIKeyBinding> mList = new List<UIKeyBinding>();

	// Token: 0x04000178 RID: 376
	public string cInputName = "";

	// Token: 0x04000179 RID: 377
	public KeyCode keyCode;

	// Token: 0x0400017A RID: 378
	public UIKeyBinding.Modifier modifier;

	// Token: 0x0400017B RID: 379
	public UIKeyBinding.Action action;

	// Token: 0x0400017C RID: 380
	[NonSerialized]
	private bool mIgnoreUp;

	// Token: 0x0400017D RID: 381
	[NonSerialized]
	private bool mIsInput;

	// Token: 0x0400017E RID: 382
	[NonSerialized]
	private bool mPress;

	// Token: 0x0200050C RID: 1292
	public enum Action
	{
		// Token: 0x040023B3 RID: 9139
		PressAndClick,
		// Token: 0x040023B4 RID: 9140
		Select,
		// Token: 0x040023B5 RID: 9141
		All
	}

	// Token: 0x0200050D RID: 1293
	public enum Modifier
	{
		// Token: 0x040023B7 RID: 9143
		Any,
		// Token: 0x040023B8 RID: 9144
		Shift,
		// Token: 0x040023B9 RID: 9145
		Control,
		// Token: 0x040023BA RID: 9146
		Alt,
		// Token: 0x040023BB RID: 9147
		None
	}
}
