using System;
using UnityEngine;

// Token: 0x02000292 RID: 658
public class UICommandAction : MonoBehaviour
{
	// Token: 0x06002199 RID: 8601 RVA: 0x000F238C File Offset: 0x000F058C
	public void UpdateDisplayedValue(object valueOverride = null, bool propagate = true)
	{
		object obj;
		if (valueOverride != null)
		{
			obj = valueOverride;
		}
		else
		{
			obj = Singleton<SystemConsole>.Instance.GetVariableValue(this.command);
		}
		if (this.checkbox)
		{
			this.checkbox.value = (bool)obj;
			return;
		}
		if (this.textInput)
		{
			this.textInput.value = (string)obj;
			return;
		}
		if (!this.scrollbar)
		{
			if (this.button)
			{
				int num = (int)obj;
				if (this.buttonID == num)
				{
					this.button.defaultColor = this.button.hover;
				}
				else
				{
					this.button.ResetDefaultColor();
				}
				if (propagate)
				{
					this.linkedActions[0].UpdateDisplayedValue(num, false);
					this.linkedActions[1].UpdateDisplayedValue(num, false);
				}
			}
			return;
		}
		if (Singleton<SystemConsole>.Instance.ConsoleCommands[this.command].variableType == typeof(int))
		{
			this.scrollbar.value = Mathf.InverseLerp(this.minValue, this.maxValue, (float)((int)obj));
			return;
		}
		this.scrollbar.value = Mathf.InverseLerp(this.minValue, this.maxValue, (float)obj);
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x000F24DC File Offset: 0x000F06DC
	public void UpdateLinkedSetting()
	{
		string text = null;
		bool flag = false;
		if (this.checkbox)
		{
			text = this.checkbox.value.ToString();
		}
		else if (this.textInput)
		{
			text = this.textInput.value;
		}
		else if (this.scrollbar)
		{
			float num = Mathf.Lerp(this.minValue, this.maxValue, this.scrollbar.value);
			if (Singleton<SystemConsole>.Instance.ConsoleCommands[this.command].variableType == typeof(int))
			{
				text = ((int)num).ToString();
			}
			else
			{
				text = num.ToString();
			}
		}
		else if (this.button)
		{
			text = this.buttonID.ToString();
			flag = true;
		}
		if (text != null)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand(this.command + " " + text, true, SystemConsole.CommandEcho.Silent);
		}
		if (flag)
		{
			this.UpdateDisplayedValue(this.buttonID, true);
		}
	}

	// Token: 0x040014F3 RID: 5363
	public string command;

	// Token: 0x040014F4 RID: 5364
	public UIToggle checkbox;

	// Token: 0x040014F5 RID: 5365
	public UIInput textInput;

	// Token: 0x040014F6 RID: 5366
	public UIScrollBar scrollbar;

	// Token: 0x040014F7 RID: 5367
	public float minValue;

	// Token: 0x040014F8 RID: 5368
	public float maxValue;

	// Token: 0x040014F9 RID: 5369
	public UIButton button;

	// Token: 0x040014FA RID: 5370
	public int buttonID;

	// Token: 0x040014FB RID: 5371
	public UICommandAction[] linkedActions = new UICommandAction[2];
}
