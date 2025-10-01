using System;
using NewNet;
using UnityEngine;

// Token: 0x02000256 RID: 598
public class TextTool : NetworkBehavior
{
	// Token: 0x06001F81 RID: 8065 RVA: 0x000E0FF8 File Offset: 0x000DF1F8
	private void Start()
	{
		this.creationPosition = base.transform.position;
		this.createRotation = base.transform.eulerAngles;
		this.inputTextCache = string.Empty;
		base.transform.parent = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot3D.transform;
		base.transform.position = this.creationPosition;
		base.transform.eulerAngles = this.createRotation;
		base.transform.localScale = Vector3.one;
		base.gameObject.layer = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot3D.layer;
		base.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
		if (Network.isServer)
		{
			base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.SetPlayerID), this.playerId);
			base.networkView.RPC<string>(RPCTarget.Others, new Action<string>(this.SendLabelTextRPC), this.input.value);
			base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.RPCSetFontSize), this.input.label.fontSize);
			base.networkView.RPC<float, float, float>(RPCTarget.All, new Action<float, float, float>(this.SetTextToolColor), this.input.label.color.r, this.input.label.color.g, this.input.label.color.b);
		}
		UIEventListener uieventListener = UIEventListener.Get(this.decreaseFontSize);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnTextSizeDecrease));
		UIEventListener uieventListener2 = UIEventListener.Get(this.increaseFontSize);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnTextSizeIncrease));
		UIEventListener uieventListener3 = UIEventListener.Get(this.deleteText);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnDeleteButtonClicked));
		EventDelegate.Add(this.input.onChange, new EventDelegate.Callback(this.OnInputChanged));
		EventManager.OnChangePointerMode += this.OnPointerChange;
		EventDelegate.Add(this.colorButton.onClick, new EventDelegate.Callback(this.ColorOnClick));
		if (PlayerScript.PointerScript != null)
		{
			this.OnPointerChange(PlayerScript.PointerScript.CurrentPointerMode);
		}
		else
		{
			this.OnPointerChange(PointerMode.Grab);
		}
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x000E1280 File Offset: 0x000DF480
	private void OnDestroy()
	{
		EventDelegate.Remove(this.input.onChange, new EventDelegate.Callback(this.OnInputChanged));
		EventManager.OnChangePointerMode -= this.OnPointerChange;
		if (this.decreaseFontSize != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.decreaseFontSize);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnTextSizeDecrease));
		}
		if (this.increaseFontSize != null)
		{
			UIEventListener uieventListener2 = UIEventListener.Get(this.increaseFontSize);
			uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnTextSizeIncrease));
		}
		if (this.deleteText != null)
		{
			UIEventListener uieventListener3 = UIEventListener.Get(this.deleteText);
			uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnDeleteButtonClicked));
		}
		EventDelegate.Remove(this.colorButton.onClick, new EventDelegate.Callback(this.ColorOnClick));
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x000E1397 File Offset: 0x000DF597
	private void ColorOnClick()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIColorPickerScript.Show(this.input.label.color, new Action<Color>(this.SetColorFromColorPicker));
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x000E13C4 File Offset: 0x000DF5C4
	private void SelectText()
	{
		this.input.isSelected = true;
		UICamera.hoveredObject = base.gameObject;
		base.GetComponent<UIHoverEnableObjects>().SetHoverEnabled(true);
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000E13E9 File Offset: 0x000DF5E9
	public void SetText(string Text)
	{
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.SendLabelTextRPC), Text);
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x000E1404 File Offset: 0x000DF604
	[Remote("Permissions/Notes")]
	private void SendLabelTextRPC(string text)
	{
		this.inputTextCache = text;
		if (this.input.value != text)
		{
			this.input.value = text;
		}
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000E142C File Offset: 0x000DF62C
	[Remote("Permissions/Notes")]
	private void SetTextToolColor(float r, float g, float b)
	{
		this.SetColor(new Color(r, g, b));
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x000E143C File Offset: 0x000DF63C
	[Remote("Permissions/Notes")]
	private void SetPlayerID(int Id)
	{
		this.playerId = Id;
		if (this.playerId == NetworkID.ID)
		{
			this.SelectText();
			base.Invoke("SelectText", 0.2f);
			base.Invoke("SelectText", 0.5f);
		}
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x000E1478 File Offset: 0x000DF678
	private void OnPlayerConnect(NetworkPlayer NP)
	{
		if (Network.isServer)
		{
			base.networkView.RPC<string>(NP, new Action<string>(this.SendLabelTextRPC), this.input.value);
			base.networkView.RPC<int>(NP, new Action<int>(this.RPCSetFontSize), this.input.label.fontSize);
			base.networkView.RPC<float, float, float>(NP, new Action<float, float, float>(this.SetTextToolColor), this.input.label.color.r, this.input.label.color.g, this.input.label.color.b);
		}
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x000E1534 File Offset: 0x000DF734
	private void OnInputChanged()
	{
		NGUIText.CalculatePrintedSize(this.input.value);
		bool active = false;
		if (PlayerScript.Pointer != null)
		{
			active = (PlayerScript.PointerScript.CurrentPointerMode == PointerMode.Text);
		}
		this.activeBackground.gameObject.SetActive(active);
		if (!this.inputTextCache.Equals(this.input.value))
		{
			base.networkView.RPC<string>(RPCTarget.Others, new Action<string>(this.SendLabelTextRPC), this.input.value);
			return;
		}
		this.inputTextCache = string.Empty;
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x000E15C8 File Offset: 0x000DF7C8
	private void OnPointerChange(PointerMode pointerMode)
	{
		if (pointerMode == PointerMode.Text && this.interactable)
		{
			this.increaseFontSize.transform.parent.gameObject.SetActive(true);
			this.decreaseFontSize.transform.parent.gameObject.SetActive(true);
			this.colorButton.transform.parent.gameObject.SetActive(true);
			this.deleteText.transform.parent.gameObject.SetActive(true);
			this.activeBackground.gameObject.SetActive(true);
			base.GetComponent<BoxCollider>().enabled = true;
			base.GetComponent<UILabel>().supportEncoding = false;
			return;
		}
		this.increaseFontSize.transform.parent.gameObject.SetActive(false);
		this.decreaseFontSize.transform.parent.gameObject.SetActive(false);
		this.colorButton.transform.parent.gameObject.SetActive(false);
		this.deleteText.transform.parent.gameObject.SetActive(false);
		this.activeBackground.gameObject.SetActive(false);
		base.GetComponent<BoxCollider>().enabled = false;
		base.GetComponent<UILabel>().supportEncoding = true;
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x000E1714 File Offset: 0x000DF914
	public void SetColor(Color newColor)
	{
		this.input.label.color = newColor;
		UIButton component = this.input.GetComponent<UIButton>();
		component.defaultColor = newColor;
		component.hover = newColor;
		component.disabledColor = newColor;
		component.pressed = newColor;
		this.colorSwatch.color = newColor;
		this.input.activeTextColor = newColor;
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x000E1770 File Offset: 0x000DF970
	public void SetColorFromColorPicker(Color color)
	{
		base.networkView.RPC<float, float, float>(RPCTarget.All, new Action<float, float, float>(this.SetTextToolColor), color.r, color.g, color.b);
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x000E179C File Offset: 0x000DF99C
	private void OnTextSizeIncrease(GameObject go)
	{
		int value = this.input.label.fontSize + 4;
		this.input.label.fontSize = Mathf.Clamp(value, 24, 150);
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.RPCSetFontSize), this.input.label.fontSize);
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x000E1801 File Offset: 0x000DFA01
	public void SetFontSize(int FontSize)
	{
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCSetFontSize), Mathf.Clamp(FontSize, 24, 150));
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x000E1828 File Offset: 0x000DFA28
	[Remote("Permissions/Notes")]
	public void RPCSetFontSize(int fontSize)
	{
		this.input.label.fontSize = fontSize;
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x000E183C File Offset: 0x000DFA3C
	private void OnTextSizeDecrease(GameObject go)
	{
		int value = this.input.label.fontSize - 4;
		this.input.label.fontSize = Mathf.Clamp(value, 24, 150);
		base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.RPCSetFontSize), this.input.label.fontSize);
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x000E18A1 File Offset: 0x000DFAA1
	private void OnDeleteButtonClicked(GameObject go)
	{
		this.NetworkDestroy();
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x000E18A9 File Offset: 0x000DFAA9
	[Remote("Permissions/Notes")]
	private void NetworkDestroy()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.NetworkDestroy));
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
	}

	// Token: 0x04001353 RID: 4947
	public UIInput input;

	// Token: 0x04001354 RID: 4948
	public UISprite activeBackground;

	// Token: 0x04001355 RID: 4949
	public GameObject decreaseFontSize;

	// Token: 0x04001356 RID: 4950
	public GameObject increaseFontSize;

	// Token: 0x04001357 RID: 4951
	public GameObject deleteText;

	// Token: 0x04001358 RID: 4952
	public UIButton colorButton;

	// Token: 0x04001359 RID: 4953
	public UISprite colorSwatch;

	// Token: 0x0400135A RID: 4954
	private Vector3 creationPosition;

	// Token: 0x0400135B RID: 4955
	private Vector3 createRotation;

	// Token: 0x0400135C RID: 4956
	public int playerId = -1;

	// Token: 0x0400135D RID: 4957
	private string inputTextCache;

	// Token: 0x0400135E RID: 4958
	private const int minFontSize = 24;

	// Token: 0x0400135F RID: 4959
	private const int maxFontSize = 150;

	// Token: 0x04001360 RID: 4960
	private const int sizeChange = 4;

	// Token: 0x04001361 RID: 4961
	public bool interactable = true;
}
