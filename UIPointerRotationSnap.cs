using System;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class UIPointerRotationSnap : MonoBehaviour
{
	// Token: 0x0600268B RID: 9867 RVA: 0x00112DB4 File Offset: 0x00110FB4
	private void Awake()
	{
		this.ThisUIButton = base.GetComponent<UIButton>();
		this.ThisLabelObject = NGUITools.GetChildLabel(base.gameObject).gameObject;
		this.ThisLabelScript = this.ThisLabelObject.GetComponent<UILabel>();
		EventManager.OnChangePlayerColor += this.OnPlayerChangedColor;
	}

	// Token: 0x0600268C RID: 9868 RVA: 0x00112E05 File Offset: 0x00111005
	private void OnDestroy()
	{
		EventManager.OnChangePlayerColor -= this.OnPlayerChangedColor;
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x00112E18 File Offset: 0x00111018
	private void OnPlayerChangedColor(Color newColor, int id)
	{
		if (id != NetworkID.ID)
		{
			return;
		}
		NGUIHelper.ButtonDisable(this.ThisUIButton, new Colour(newColor) == Colour.Grey, null, null);
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x00112E5C File Offset: 0x0011105C
	private void Update()
	{
		if (PlayerScript.Pointer && this.CachedRotatedionSnap != PlayerScript.PointerScript.RotationSnap)
		{
			this.ThisLabelScript.text = PlayerScript.PointerScript.RotationSnap.ToString() + "°";
			this.CachedRotatedionSnap = PlayerScript.PointerScript.RotationSnap;
		}
		this.bMouse1 = TTSInput.GetKey(KeyCode.Mouse1);
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x00112ED0 File Offset: 0x001110D0
	private void OnClick()
	{
		if (PlayerScript.Pointer)
		{
			int num;
			if (!this.bMouse1)
			{
				num = (PlayerScript.PointerScript.RotationSnap + 15) % 90;
				if (num == 0 || num == 75)
				{
					num = 90;
				}
			}
			else
			{
				num = (PlayerScript.PointerScript.RotationSnap - 15) % 90;
				if (num == 0)
				{
					num = 90;
				}
				if (num == 75)
				{
					num = 60;
				}
			}
			PlayerScript.PointerScript.RotationSnap = num;
		}
	}

	// Token: 0x04001922 RID: 6434
	private UIButton ThisUIButton;

	// Token: 0x04001923 RID: 6435
	private GameObject ThisLabelObject;

	// Token: 0x04001924 RID: 6436
	private UILabel ThisLabelScript;

	// Token: 0x04001925 RID: 6437
	private bool bMouse1;

	// Token: 0x04001926 RID: 6438
	private int CachedRotatedionSnap = -1;
}
