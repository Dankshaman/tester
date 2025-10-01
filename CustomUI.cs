using System;
using UnityEngine;

// Token: 0x02000269 RID: 617
public class CustomUI : MonoBehaviour
{
	// Token: 0x06002091 RID: 8337 RVA: 0x000EAFDF File Offset: 0x000E91DF
	public void Start()
	{
		CustomUI.Instance = this;
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x000EAFE8 File Offset: 0x000E91E8
	public void AddButton(string label, string command, Vector2 position, Vector2 size, int fontSize = 20, int volume = 1)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ButtonTemplate);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = position;
		UILabel component = gameObject.transform.GetChild(0).GetComponent<UILabel>();
		component.text = label;
		component.fontSize = fontSize;
		UISprite component2 = gameObject.GetComponent<UISprite>();
		component2.width = (int)size.x;
		component2.height = (int)size.y;
		UIButton component3 = gameObject.GetComponent<UIButton>();
		UIEventListener uieventListener = UIEventListener.Get(gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.PerformAction));
		UICommandAction component4 = gameObject.GetComponent<UICommandAction>();
		component4.command = command;
		component4.button = component3;
		component4.buttonID = volume;
		gameObject.SetActive(true);
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x000EB0D0 File Offset: 0x000E92D0
	public void AddCheckbox(string label, string command, Vector2 position, int size, Colour colour, Colour? dropShadow, Colour? outline, int fontSize = 20, int volume = 1, bool labelOnRight = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CheckboxTemplate);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = position;
		UISprite component = gameObject.transform.GetChild(1).GetComponent<UISprite>();
		component.width = size;
		component.height = size;
		UILabel component2 = gameObject.transform.GetChild(0).GetComponent<UILabel>();
		component2.fontSize = fontSize;
		component2.overflowMethod = UILabel.Overflow.ResizeFreely;
		component2.text = label;
		int num;
		if (labelOnRight)
		{
			num = size / 2 + 20 + fontSize / 3;
		}
		else
		{
			num = -fontSize / 3 - component2.width;
		}
		component2.transform.localPosition = new Vector3((float)num, (float)((int)((double)(-(double)fontSize) / 12.5)), 0f);
		component2.color = colour;
		if (dropShadow != null)
		{
			component2.effectColor = dropShadow.Value;
			component2.effectStyle = UILabel.Effect.Shadow;
		}
		else if (outline != null)
		{
			component2.effectColor = outline.Value;
			component2.effectStyle = UILabel.Effect.Outline;
		}
		UIToggle component3 = gameObject.GetComponent<UIToggle>();
		component3.startsActive = Singleton<SystemConsole>.Instance.GetToggleValue(command);
		Singleton<SystemConsole>.Instance.uiToggleToUpdate[command] = component3;
		UIEventListener uieventListener = UIEventListener.Get(gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.PerformAction));
		UICommandAction component4 = gameObject.GetComponent<UICommandAction>();
		component4.command = command;
		component4.checkbox = component3;
		component4.buttonID = volume;
		gameObject.SetActive(true);
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x000EB27C File Offset: 0x000E947C
	public void AddLabel(string text, Vector2 position, Colour colour, Colour? dropShadow, Colour? outline, int fontSize = 20)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.LabelTemplate);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = position;
		UILabel component = gameObject.GetComponent<UILabel>();
		component.fontSize = fontSize;
		component.overflowMethod = UILabel.Overflow.ResizeFreely;
		component.text = text;
		component.color = colour;
		if (dropShadow != null)
		{
			component.effectColor = dropShadow.Value;
			component.effectStyle = UILabel.Effect.Shadow;
		}
		else if (outline != null)
		{
			component.effectColor = outline.Value;
			component.effectStyle = UILabel.Effect.Outline;
		}
		gameObject.SetActive(true);
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x000EB34C File Offset: 0x000E954C
	public void Clear()
	{
		for (int i = base.transform.childCount - 1; i >= 3; i--)
		{
			UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x000EB388 File Offset: 0x000E9588
	private void PerformAction(GameObject source)
	{
		UICommandAction component = source.GetComponent<UICommandAction>();
		if (component.checkbox)
		{
			string str = component.checkbox.value.ToString();
			Singleton<SystemConsole>.Instance.ProcessCommand(component.command + " " + str, true, (SystemConsole.CommandEcho)component.buttonID);
			return;
		}
		if (component.button)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand(component.command, true, (SystemConsole.CommandEcho)component.buttonID);
		}
	}

	// Token: 0x04001407 RID: 5127
	public static CustomUI Instance;

	// Token: 0x04001408 RID: 5128
	public GameObject ButtonTemplate;

	// Token: 0x04001409 RID: 5129
	public GameObject CheckboxTemplate;

	// Token: 0x0400140A RID: 5130
	public GameObject LabelTemplate;

	// Token: 0x0400140B RID: 5131
	private const int templateCount = 3;
}
