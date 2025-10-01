using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class UIDialog : MonoBehaviour
{
	// Token: 0x060022D0 RID: 8912 RVA: 0x000F8660 File Offset: 0x000F6860
	private void Start()
	{
		base.GetComponent<UIDragObject>().ResetDepth = false;
		if (this.closeButton != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.closeButton);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.CloseButton_OnClick));
		}
		UIEventListener uieventListener2 = UIEventListener.Get(this.leftButton);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.LeftButton_OnClick));
		UIEventListener uieventListener3 = UIEventListener.Get(this.middleButton);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.MiddleButton_OnClick));
		UIEventListener uieventListener4 = UIEventListener.Get(this.rightButton);
		uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.RightButton_OnClick));
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x000F8738 File Offset: 0x000F6938
	private void OnDestroy()
	{
		if (this.closeButton != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.closeButton);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.CloseButton_OnClick));
		}
		UIEventListener uieventListener2 = UIEventListener.Get(this.leftButton);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.LeftButton_OnClick));
		UIEventListener uieventListener3 = UIEventListener.Get(this.middleButton);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.MiddleButton_OnClick));
		UIEventListener uieventListener4 = UIEventListener.Get(this.rightButton);
		uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.RightButton_OnClick));
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000F8804 File Offset: 0x000F6A04
	private static void SetMode(UIDialog uiDialog, UIDialog.Mode mode)
	{
		uiDialog.ResetFunctions();
		Vector3 localPosition = uiDialog.dropDown.transform.localPosition;
		Vector3 localPosition2 = uiDialog.input.transform.localPosition;
		localPosition.y = 2f;
		localPosition2.y = 14f;
		float num = -32f;
		float num2 = 25f;
		int num3 = 136;
		int num4 = 400;
		switch (mode)
		{
		case UIDialog.Mode.Input:
		case UIDialog.Mode.DropDown:
			num -= 10f;
			num2 += 15f;
			num3 += 20;
			break;
		case UIDialog.Mode.DrowDownInput:
			num -= 25f;
			num2 += 30f;
			num3 += 50;
			localPosition.y += 10f;
			localPosition2.y -= 20f;
			break;
		case UIDialog.Mode.MemoInput:
		case UIDialog.Mode.CodeInput:
			num -= 175f;
			num2 += 180f;
			num4 += 200;
			num3 += 350;
			localPosition2.y -= 20f;
			break;
		}
		uiDialog.dropDown.transform.localPosition = localPosition;
		uiDialog.input.transform.localPosition = localPosition2;
		NGUIHelper.BringToFront(uiDialog.transform.parent.GetComponent<UIPanel>());
		Vector3 localPosition3 = uiDialog.leftButton.transform.localPosition;
		Vector3 localPosition4 = uiDialog.rightButton.transform.localPosition;
		Vector3 localPosition5 = uiDialog.middleButton.transform.localPosition;
		Vector3 localPosition6 = uiDialog.desc.transform.localPosition;
		localPosition3.y = num;
		localPosition4.y = num;
		localPosition5.y = num;
		localPosition6.y = num2;
		uiDialog.leftButton.transform.localPosition = localPosition3;
		uiDialog.rightButton.transform.localPosition = localPosition4;
		uiDialog.middleButton.transform.localPosition = localPosition5;
		uiDialog.desc.transform.localPosition = localPosition6;
		uiDialog.background.width = num4;
		uiDialog.background.height = num3;
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000F8A14 File Offset: 0x000F6C14
	private void ResetFunctions()
	{
		this.middleButtonFunction = null;
		this.leftButtonFunction = null;
		this.rightButtonFunction = null;
		this.middleButtonInputFunction = null;
		this.leftButtonInputFunction = null;
		this.rightButtonInputFunction = null;
		this.middleButtonInputMemoFunction = null;
		this.leftButtonInputMemoFunction = null;
		this.rightButtonInputMemoFunction = null;
		this.middleButtonInputCodeFunction = null;
		this.leftButtonInputCodeFunction = null;
		this.rightButtonInputCodeFunction = null;
		this.middleButtonDropDownFunction = null;
		this.leftButtonDropDownFunction = null;
		this.rightButtonDropDownFunction = null;
		this.middleButtonDropDownInputFunction = null;
		this.leftButtonDropDownInputFunction = null;
		this.rightButtonDropDownInputFunction = null;
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000F8A9F File Offset: 0x000F6C9F
	public static void Show(string description, string buttonText, Action buttonFunc)
	{
		UIDialog.Show(UIDialog.Instance, description, buttonText, buttonFunc);
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000F8AB0 File Offset: 0x000F6CB0
	public static void Show(UIDialog uiDialog, string description, string buttonText, Action buttonFunc)
	{
		UIDialog.SetMode(uiDialog, UIDialog.Mode.Default);
		uiDialog.gameObject.SetActive(true);
		NGUITools.SetActive(uiDialog.leftButton, false);
		NGUITools.SetActive(uiDialog.middleButton, true);
		NGUITools.SetActive(uiDialog.rightButton, false);
		NGUITools.SetActive(uiDialog.input.gameObject, false);
		NGUITools.SetActive(uiDialog.memo.gameObject, false);
		NGUITools.SetActive(uiDialog.code.gameObject, false);
		NGUITools.SetActive(uiDialog.dropDown.gameObject, false);
		uiDialog.desc.text = Language.Translate(description);
		uiDialog.middleButtonText.text = Language.Translate(buttonText);
		uiDialog.middleButtonFunction = buttonFunc;
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000F8B61 File Offset: 0x000F6D61
	public static void Show(string description, string leftButtonText, string rightButtonText, Action leftButtonFunc, Action rightButtonFunc = null)
	{
		UIDialog.Show(UIDialog.Instance, description, leftButtonText, rightButtonText, leftButtonFunc, rightButtonFunc);
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000F8B74 File Offset: 0x000F6D74
	public static void Show(UIDialog uiDialog, string description, string leftButtonText, string rightButtonText, Action leftButtonFunc, Action rightButtonFunc = null)
	{
		UIDialog.SetMode(uiDialog, UIDialog.Mode.Default);
		uiDialog.gameObject.SetActive(true);
		NGUITools.SetActive(uiDialog.leftButton, true);
		NGUITools.SetActive(uiDialog.middleButton, false);
		NGUITools.SetActive(uiDialog.rightButton, true);
		NGUITools.SetActive(uiDialog.input.gameObject, false);
		NGUITools.SetActive(uiDialog.memo.gameObject, false);
		NGUITools.SetActive(uiDialog.code.gameObject, false);
		NGUITools.SetActive(uiDialog.dropDown.gameObject, false);
		uiDialog.desc.text = Language.Translate(description);
		uiDialog.leftButtonText.text = Language.Translate(leftButtonText);
		uiDialog.rightButtonText.text = Language.Translate(rightButtonText);
		uiDialog.leftButtonFunction = leftButtonFunc;
		uiDialog.rightButtonFunction = rightButtonFunc;
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000F8C3F File Offset: 0x000F6E3F
	public static void ShowInput(string description, string leftButtonText, string rightButtonText, Action<string> leftButtonFunc, Action<string> rightButtonFunc = null, string inputValue = "", string inputName = "")
	{
		UIDialog.ShowInput(UIDialog.Instance, description, leftButtonText, rightButtonText, leftButtonFunc, rightButtonFunc, inputValue, inputName);
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000F8C58 File Offset: 0x000F6E58
	public static void ShowInput(UIDialog uiDialog, string description, string leftButtonText, string rightButtonText, Action<string> leftButtonFunc, Action<string> rightButtonFunc = null, string inputValue = "", string inputName = "")
	{
		UIDialog.Show(uiDialog, description, leftButtonText, rightButtonText, null, null);
		UIDialog.SetMode(uiDialog, UIDialog.Mode.Input);
		NGUITools.SetActive(uiDialog.input.gameObject, true);
		uiDialog.input.defaultText = Language.Translate(inputName);
		uiDialog.input.value = inputValue;
		uiDialog.input.isSelected = true;
		uiDialog.leftButtonInputFunction = leftButtonFunc;
		uiDialog.rightButtonInputFunction = rightButtonFunc;
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000F8CC3 File Offset: 0x000F6EC3
	public static void ShowMemoInput(string description, string leftButtonText, string rightButtonText, Action<string, string> leftButtonFunc, Action<string, string> rightButtonFunc = null, string inputValue = "", string inputName = "")
	{
		UIDialog.ShowMemoInput(UIDialog.Instance, description, leftButtonText, rightButtonText, leftButtonFunc, rightButtonFunc, inputValue, inputName);
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000F8CDC File Offset: 0x000F6EDC
	public static void ShowMemoInput(UIDialog uiDialog, string description, string leftButtonText, string rightButtonText, Action<string, string> leftButtonFunc, Action<string, string> rightButtonFunc = null, string inputValue = "", string inputName = "")
	{
		UIDialog.Show(uiDialog, description, leftButtonText, rightButtonText, null, null);
		UIDialog.SetMode(uiDialog, UIDialog.Mode.MemoInput);
		NGUITools.SetActive(uiDialog.memo, true);
		uiDialog.memoInput.defaultText = Language.Translate(inputName);
		uiDialog.memoInput.value = inputValue;
		uiDialog.leftButtonInputMemoFunction = leftButtonFunc;
		uiDialog.rightButtonInputMemoFunction = rightButtonFunc;
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000F8D38 File Offset: 0x000F6F38
	public static void ShowCodeInput(string description, string leftButtonText, string rightButtonText, Action<string, string> leftButtonFunc, Action<string, string> rightButtonFunc = null, string inputValue = "", string inputName = "", bool selectAll = false, string middleButtonText = "", Action<string, string> middleButtonFunc = null)
	{
		UIDialog.ShowCodeInput(UIDialog.Instance, description, leftButtonText, rightButtonText, leftButtonFunc, rightButtonFunc, inputValue, inputName, selectAll, middleButtonText, middleButtonFunc);
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x000F8D60 File Offset: 0x000F6F60
	private static void ShowCodeInput(UIDialog uiDialog, string description, string leftButtonText, string rightButtonText, Action<string, string> leftButtonFunc, Action<string, string> rightButtonFunc = null, string inputValue = "", string inputName = "", bool selectAll = false, string middleButtonText = "", Action<string, string> middleButtonFunc = null)
	{
		UIDialog.Show(uiDialog, description, leftButtonText, rightButtonText, null, null);
		UIDialog.SetMode(uiDialog, UIDialog.Mode.CodeInput);
		NGUITools.SetActive(uiDialog.code, true);
		uiDialog.codeLabel.autoResizeBoxCollider = true;
		uiDialog.codeInput.defaultText = Language.Translate(inputName);
		uiDialog.codeInput.value = inputValue;
		uiDialog.codeInput.SelectAllTextOnClick = selectAll;
		uiDialog.leftButtonInputCodeFunction = leftButtonFunc;
		uiDialog.rightButtonInputCodeFunction = rightButtonFunc;
		uiDialog.leftButtonText.text = Language.Translate(leftButtonText);
		uiDialog.rightButtonText.text = Language.Translate(rightButtonText);
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x000F8DF5 File Offset: 0x000F6FF5
	public static void ShowDropDown(string description, string leftButtonText, string rightButtonText, List<string> dropDownOptions, Action<string> leftButtonFunc, Action<string> rightButtonFunc = null, string drowDownValue = "")
	{
		UIDialog.ShowDropDown(UIDialog.Instance, description, leftButtonText, rightButtonText, dropDownOptions, leftButtonFunc, rightButtonFunc, drowDownValue);
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x000F8E0C File Offset: 0x000F700C
	public static void ShowDropDown(UIDialog uiDialog, string description, string leftButtonText, string rightButtonText, List<string> dropDownOptions, Action<string> leftButtonFunc, Action<string> rightButtonFunc = null, string drowDownValue = "")
	{
		UIDialog.Show(uiDialog, description, leftButtonText, rightButtonText, null, null);
		UIDialog.SetMode(uiDialog, UIDialog.Mode.DropDown);
		NGUITools.SetActive(uiDialog.dropDown.gameObject, true);
		uiDialog.dropDown.items = dropDownOptions;
		if (!string.IsNullOrEmpty(drowDownValue))
		{
			uiDialog.dropDown.value = drowDownValue;
		}
		if (!uiDialog.dropDown.items.Contains(uiDialog.dropDown.value) && uiDialog.dropDown.items.Count > 0)
		{
			uiDialog.dropDown.value = uiDialog.dropDown.items[0];
		}
		uiDialog.leftButtonDropDownFunction = leftButtonFunc;
		uiDialog.rightButtonDropDownFunction = rightButtonFunc;
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000F8EBC File Offset: 0x000F70BC
	public static void ShowDropDownInput(string description, string leftButtonText, string rightButtonText, List<string> dropDownOptions, Action<string, string> leftButtonFunc, Action<string, string> rightButtonFunc = null, string inputValue = "", string inputName = "", string drowDownValue = "")
	{
		UIDialog.ShowDropDownInput(UIDialog.Instance, description, leftButtonText, rightButtonText, dropDownOptions, leftButtonFunc, rightButtonFunc, inputValue, inputName, drowDownValue);
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000F8EE4 File Offset: 0x000F70E4
	private static void ShowDropDownInput(UIDialog uiDialog, string description, string leftButtonText, string rightButtonText, List<string> dropDownOptions, Action<string, string> leftButtonFunc, Action<string, string> rightButtonFunc = null, string inputValue = "", string inputName = "", string drowDownValue = "")
	{
		UIDialog.Show(uiDialog, description, leftButtonText, rightButtonText, null, null);
		UIDialog.SetMode(uiDialog, UIDialog.Mode.DrowDownInput);
		NGUITools.SetActive(uiDialog.dropDown.gameObject, true);
		uiDialog.dropDown.items = dropDownOptions;
		if (!string.IsNullOrEmpty(drowDownValue))
		{
			uiDialog.dropDown.value = drowDownValue;
		}
		if (!uiDialog.dropDown.items.Contains(uiDialog.dropDown.value) && uiDialog.dropDown.items.Count > 0)
		{
			uiDialog.dropDown.value = uiDialog.dropDown.items[0];
		}
		NGUITools.SetActive(uiDialog.input.gameObject, true);
		uiDialog.input.defaultText = Language.Translate(inputName);
		uiDialog.input.value = inputValue;
		uiDialog.leftButtonDropDownInputFunction = leftButtonFunc;
		uiDialog.rightButtonDropDownInputFunction = rightButtonFunc;
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000F8FC4 File Offset: 0x000F71C4
	private void LeftButton_OnClick(GameObject go)
	{
		base.gameObject.SetActive(false);
		Action action = this.leftButtonFunction;
		if (action != null)
		{
			action();
		}
		Action<string> action2 = this.leftButtonInputFunction;
		if (action2 != null)
		{
			action2(this.input.value);
		}
		Action<string, string> action3 = this.leftButtonInputMemoFunction;
		if (action3 != null)
		{
			action3(this.memoInput.value, this.desc.text);
		}
		Action<string, string> action4 = this.leftButtonInputCodeFunction;
		if (action4 != null)
		{
			action4(this.codeInput.value, this.desc.text);
		}
		Action<string> action5 = this.leftButtonDropDownFunction;
		if (action5 != null)
		{
			action5(this.dropDown.value);
		}
		Action<string, string> action6 = this.leftButtonDropDownInputFunction;
		if (action6 == null)
		{
			return;
		}
		action6(this.dropDown.value, this.input.value);
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000F909C File Offset: 0x000F729C
	private void MiddleButton_OnClick(GameObject go)
	{
		base.gameObject.SetActive(false);
		Action action = this.middleButtonFunction;
		if (action != null)
		{
			action();
		}
		Action<string> action2 = this.middleButtonInputFunction;
		if (action2 != null)
		{
			action2(this.input.value);
		}
		Action<string, string> action3 = this.middleButtonInputMemoFunction;
		if (action3 != null)
		{
			action3(this.memoInput.value, this.desc.text);
		}
		Action<string, string> action4 = this.middleButtonInputCodeFunction;
		if (action4 != null)
		{
			action4(this.codeInput.value, this.desc.text);
		}
		Action<string> action5 = this.middleButtonDropDownFunction;
		if (action5 != null)
		{
			action5(this.dropDown.value);
		}
		Action<string, string> action6 = this.middleButtonDropDownInputFunction;
		if (action6 == null)
		{
			return;
		}
		action6(this.dropDown.value, this.input.value);
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000F9174 File Offset: 0x000F7374
	private void RightButton_OnClick(GameObject go)
	{
		base.gameObject.SetActive(false);
		Action action = this.rightButtonFunction;
		if (action != null)
		{
			action();
		}
		Action<string> action2 = this.rightButtonInputFunction;
		if (action2 != null)
		{
			action2(this.input.value);
		}
		Action<string, string> action3 = this.rightButtonInputMemoFunction;
		if (action3 != null)
		{
			action3(this.memoInput.value, this.desc.text);
		}
		Action<string, string> action4 = this.rightButtonInputCodeFunction;
		if (action4 != null)
		{
			action4(this.codeInput.value, this.desc.text);
		}
		Action<string> action5 = this.rightButtonDropDownFunction;
		if (action5 != null)
		{
			action5(this.dropDown.value);
		}
		Action<string, string> action6 = this.rightButtonDropDownInputFunction;
		if (action6 == null)
		{
			return;
		}
		action6(this.dropDown.value, this.input.value);
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000BCD9D File Offset: 0x000BAF9D
	private void CloseButton_OnClick(GameObject go)
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000F924C File Offset: 0x000F744C
	public static void ShowInfoForLua(NetworkPlayer targetPlayer, string info)
	{
		if (!Network.isServer)
		{
			return;
		}
		if (targetPlayer.isServer)
		{
			UIDialog.Show(UIDialog.LuaDialog, info, "OK", null);
			return;
		}
		LuaGlobalScriptManager.Instance.networkView.RPC<string>(targetPlayer, new Action<string>(LuaGlobalScriptManager.Instance.RPCInfoShow), info);
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000F92A0 File Offset: 0x000F74A0
	public static void ShowConfirmForLua(NetworkPlayer targetPlayer, string info, Action callback)
	{
		if (!Network.isServer)
		{
			return;
		}
		if (targetPlayer.isServer)
		{
			UIDialog.Show(UIDialog.LuaDialog, info, "OK", "Cancel", callback, null);
			return;
		}
		LuaGlobalScriptManager.Instance.ActionCallbackFromPlayerID[(int)targetPlayer.id] = callback;
		LuaGlobalScriptManager.Instance.networkView.RPC<string, int>(targetPlayer, new Action<string, int>(LuaGlobalScriptManager.Instance.RPCShowConfirm), info, callback.GetHashCode());
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000F9314 File Offset: 0x000F7514
	public static void ShowInputForLua(NetworkPlayer targetPlayer, string description, string defaultString, Action<string> callback)
	{
		if (!Network.isServer)
		{
			return;
		}
		if (targetPlayer.isServer)
		{
			UIDialog.ShowInput(UIDialog.LuaDialog, description, "OK", "Cancel", callback, null, defaultString, "");
			return;
		}
		LuaGlobalScriptManager.Instance.ActionStringCallbackFromPlayerID[(int)targetPlayer.id] = callback;
		LuaGlobalScriptManager.Instance.networkView.RPC<int, string, string>(targetPlayer, new Action<int, string, string>(LuaGlobalScriptManager.Instance.RPCShowInput), callback.GetHashCode(), description, defaultString);
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000F9390 File Offset: 0x000F7590
	public static void ShowMemoInputForLua(NetworkPlayer targetPlayer, string description, string defaultString, Action<string> callback)
	{
		UIDialog.<>c__DisplayClass63_0 CS$<>8__locals1 = new UIDialog.<>c__DisplayClass63_0();
		CS$<>8__locals1.callback = callback;
		if (!Network.isServer)
		{
			return;
		}
		if (targetPlayer.isServer)
		{
			UIDialog.ShowMemoInput(UIDialog.LuaDialog, description, "OK", "Cancel", new Action<string, string>(CS$<>8__locals1.<ShowMemoInputForLua>g__wrapper|0), null, defaultString, "");
			return;
		}
		LuaGlobalScriptManager.Instance.ActionStringCallbackFromPlayerID[(int)targetPlayer.id] = CS$<>8__locals1.callback;
		LuaGlobalScriptManager.Instance.networkView.RPC<int, string, string>(targetPlayer, new Action<int, string, string>(LuaGlobalScriptManager.Instance.RPCShowMemoInput), CS$<>8__locals1.callback.GetHashCode(), description, defaultString);
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000F9430 File Offset: 0x000F7630
	public static void ShowDropDownForLua(NetworkPlayer targetPlayer, string description, List<string> dropDownOptions, string dropDownValue, Action<string> callback)
	{
		if (!Network.isServer)
		{
			return;
		}
		if (targetPlayer.isServer)
		{
			UIDialog.ShowDropDown(UIDialog.LuaDialog, description, "OK", "Cancel", dropDownOptions, callback, null, dropDownValue);
			return;
		}
		LuaGlobalScriptManager.Instance.ActionStringCallbackFromPlayerID[(int)targetPlayer.id] = callback;
		LuaGlobalScriptManager.Instance.networkView.RPC<int, string, List<string>, string>(targetPlayer, new Action<int, string, List<string>, string>(LuaGlobalScriptManager.Instance.RPCShowDropDown), callback.GetHashCode(), description, dropDownOptions, dropDownValue);
	}

	// Token: 0x04001602 RID: 5634
	public static UIDialog Instance;

	// Token: 0x04001603 RID: 5635
	public static UIDialog LuaDialog;

	// Token: 0x04001604 RID: 5636
	private Action middleButtonFunction;

	// Token: 0x04001605 RID: 5637
	private Action leftButtonFunction;

	// Token: 0x04001606 RID: 5638
	private Action rightButtonFunction;

	// Token: 0x04001607 RID: 5639
	private Action<string> middleButtonInputFunction;

	// Token: 0x04001608 RID: 5640
	private Action<string> leftButtonInputFunction;

	// Token: 0x04001609 RID: 5641
	private Action<string> rightButtonInputFunction;

	// Token: 0x0400160A RID: 5642
	private Action<string, string> middleButtonInputMemoFunction;

	// Token: 0x0400160B RID: 5643
	private Action<string, string> leftButtonInputMemoFunction;

	// Token: 0x0400160C RID: 5644
	private Action<string, string> rightButtonInputMemoFunction;

	// Token: 0x0400160D RID: 5645
	private Action<string, string> middleButtonInputCodeFunction;

	// Token: 0x0400160E RID: 5646
	private Action<string, string> leftButtonInputCodeFunction;

	// Token: 0x0400160F RID: 5647
	private Action<string, string> rightButtonInputCodeFunction;

	// Token: 0x04001610 RID: 5648
	private Action<string> middleButtonDropDownFunction;

	// Token: 0x04001611 RID: 5649
	private Action<string> leftButtonDropDownFunction;

	// Token: 0x04001612 RID: 5650
	private Action<string> rightButtonDropDownFunction;

	// Token: 0x04001613 RID: 5651
	private Action<string, string> middleButtonDropDownInputFunction;

	// Token: 0x04001614 RID: 5652
	private Action<string, string> leftButtonDropDownInputFunction;

	// Token: 0x04001615 RID: 5653
	private Action<string, string> rightButtonDropDownInputFunction;

	// Token: 0x04001616 RID: 5654
	public GameObject closeButton;

	// Token: 0x04001617 RID: 5655
	public GameObject leftButton;

	// Token: 0x04001618 RID: 5656
	public UILabel leftButtonText;

	// Token: 0x04001619 RID: 5657
	public GameObject middleButton;

	// Token: 0x0400161A RID: 5658
	public UILabel middleButtonText;

	// Token: 0x0400161B RID: 5659
	public GameObject rightButton;

	// Token: 0x0400161C RID: 5660
	public UILabel rightButtonText;

	// Token: 0x0400161D RID: 5661
	public UILabel desc;

	// Token: 0x0400161E RID: 5662
	public UIInput input;

	// Token: 0x0400161F RID: 5663
	public GameObject memo;

	// Token: 0x04001620 RID: 5664
	public UIInput memoInput;

	// Token: 0x04001621 RID: 5665
	public UILabel memoLabel;

	// Token: 0x04001622 RID: 5666
	public GameObject code;

	// Token: 0x04001623 RID: 5667
	public UIInput codeInput;

	// Token: 0x04001624 RID: 5668
	public UILabel codeLabel;

	// Token: 0x04001625 RID: 5669
	public UIPopupList dropDown;

	// Token: 0x04001626 RID: 5670
	public UISprite background;

	// Token: 0x02000717 RID: 1815
	private enum Mode
	{
		// Token: 0x04002A9F RID: 10911
		Default,
		// Token: 0x04002AA0 RID: 10912
		Input,
		// Token: 0x04002AA1 RID: 10913
		DropDown,
		// Token: 0x04002AA2 RID: 10914
		DrowDownInput,
		// Token: 0x04002AA3 RID: 10915
		MemoInput,
		// Token: 0x04002AA4 RID: 10916
		CodeInput
	}
}
