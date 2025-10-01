using System;
using NewNet;
using UnityEngine;

// Token: 0x0200028F RID: 655
public class UIColorSelection : MonoBehaviour
{
	// Token: 0x06002190 RID: 8592 RVA: 0x000F1930 File Offset: 0x000EFB30
	private void Start()
	{
		this.ColorUIButton = base.gameObject.GetComponent<UIButton>();
		this.ColorUISprite = base.gameObject.GetComponent<UISprite>();
		this.MainCamera = Camera.main;
		this.CameraUI = GameObject.Find("UICamera").GetComponent<Camera>();
		this.label = base.gameObject.name.Substring(0, base.gameObject.name.Length - 10);
		this.colour = Colour.ColourFromLabel(this.label);
		this.ColorUIButton.defaultColor = this.colour;
		this.ColorUIButton.hover = this.colour;
		this.ColorUIButton.disabledColor = this.colour;
		this.ColorUISprite.color = this.colour;
		if (this.label == "Grey" && NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			base.gameObject.SetActive(false);
		}
		if (this.label != "Grey")
		{
			this.LockObject = base.transform.GetChild(0).gameObject;
		}
		this.uiPulse = base.GetComponent<UIPulse>();
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000F1A74 File Offset: 0x000EFC74
	private void Update()
	{
		bool flag = PermissionsOptions.options.ChangeColor;
		if (Network.isServer || this.label == "Grey" || NetworkSingleton<PlayerManager>.Instance.IsPromoted(NetworkID.ID))
		{
			flag = true;
		}
		if (this.uiPulse)
		{
			this.uiPulse.enabled = flag;
		}
		Vector3 vector = Vector3.zero;
		if (this.label == "Grey")
		{
			if (!VRHMD.isVR)
			{
				Vector3 position = new Vector3(0f, 3f, 0f);
				if (Singleton<CameraController>.Instance.bTopDown && NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
				{
					position.z -= 2f;
				}
				vector = this.MainCamera.WorldToScreenPoint(position);
			}
			else
			{
				vector = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2 - 60), 0f);
			}
		}
		else if (this.label == "Black")
		{
			base.gameObject.SetActive(NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1));
			if (!VRHMD.isVR)
			{
				Vector3 position2 = new Vector3(0f, 10f, 0f);
				if (Singleton<CameraController>.Instance.bTopDown)
				{
					position2.z += 2f;
				}
				vector = this.MainCamera.WorldToScreenPoint(position2);
			}
			else
			{
				vector = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2 + 60), 0f);
			}
		}
		else
		{
			GameObject hand = HandZone.GetHand(this.label, 0);
			this.LockObject.SetActive(!flag);
			base.GetComponent<BoxCollider2D>().enabled = flag;
			if (hand && !NetworkSingleton<PlayerManager>.Instance.ColourInUse(this.colour))
			{
				this.ColorUIButton.enabled = true;
				this.ColorUISprite.enabled = true;
				if (!VRHMD.isVR)
				{
					vector = this.MainCamera.WorldToScreenPoint(hand.transform.position);
				}
				else
				{
					float num = 185f;
					float f = 0.62831855f * (float)Colour.IDFromColour(this.colour);
					float x = num * Mathf.Cos(f) + (float)(Screen.width / 2);
					float y = num * Mathf.Sin(f) + (float)(Screen.height / 2);
					vector = new Vector3(x, y, 0f);
				}
			}
			else
			{
				this.LockObject.SetActive(false);
				this.ColorUIButton.enabled = false;
				this.ColorUISprite.enabled = false;
			}
		}
		if (vector.z >= 0f)
		{
			vector.z = 0f;
		}
		if (vector != Vector3.zero)
		{
			base.transform.position = this.CameraUI.ScreenToWorldPoint(vector);
		}
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x000F1D35 File Offset: 0x000EFF35
	private void OnClick()
	{
		if (UIColorSelection.id != -1)
		{
			NetworkSingleton<NetworkUI>.Instance.bNeedToPickColour = false;
			NetworkSingleton<NetworkUI>.Instance.CheckColor(this.label, UIColorSelection.id);
			return;
		}
		NetworkSingleton<NetworkUI>.Instance.ClientRequestColor(this.label);
	}

	// Token: 0x040014D4 RID: 5332
	public static int id = -1;

	// Token: 0x040014D5 RID: 5333
	private string label = "";

	// Token: 0x040014D6 RID: 5334
	private Colour colour;

	// Token: 0x040014D7 RID: 5335
	private UIButton ColorUIButton;

	// Token: 0x040014D8 RID: 5336
	private UISprite ColorUISprite;

	// Token: 0x040014D9 RID: 5337
	private Camera CameraUI;

	// Token: 0x040014DA RID: 5338
	private Camera MainCamera;

	// Token: 0x040014DB RID: 5339
	private GameObject LockObject;

	// Token: 0x040014DC RID: 5340
	private UIPulse uiPulse;
}
