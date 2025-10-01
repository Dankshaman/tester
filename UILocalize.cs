using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	// Token: 0x17000157 RID: 343
	// (set) Token: 0x06000729 RID: 1833 RVA: 0x0003272C File Offset: 0x0003092C
	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				UIWidget component = base.GetComponent<UIWidget>();
				UILabel uilabel = component as UILabel;
				UISprite uisprite = component as UISprite;
				if (uilabel != null)
				{
					UIInput uiinput = NGUITools.FindInParents<UIInput>(uilabel.gameObject);
					if (uiinput != null && uiinput.label == uilabel)
					{
						uiinput.defaultText = value;
						return;
					}
					uilabel.text = value;
					return;
				}
				else if (uisprite != null)
				{
					UIButton uibutton = NGUITools.FindInParents<UIButton>(uisprite.gameObject);
					if (uibutton != null && uibutton.tweenTarget == uisprite.gameObject)
					{
						uibutton.normalSprite = value;
					}
					uisprite.spriteName = value;
					uisprite.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x000327D8 File Offset: 0x000309D8
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnLocalize();
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x000327E8 File Offset: 0x000309E8
	private void Start()
	{
		this.mStarted = true;
		this.OnLocalize();
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x000327F8 File Offset: 0x000309F8
	private void OnLocalize()
	{
		if (string.IsNullOrEmpty(this.key))
		{
			UILabel component = base.GetComponent<UILabel>();
			if (component != null)
			{
				this.key = component.text;
			}
		}
		if (!string.IsNullOrEmpty(this.key))
		{
			this.value = Localization.Get(this.key, true);
		}
	}

	// Token: 0x04000501 RID: 1281
	public string key;

	// Token: 0x04000502 RID: 1282
	private bool mStarted;
}
