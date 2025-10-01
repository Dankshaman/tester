using System;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class UIHoverIcon : MonoBehaviour
{
	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06002459 RID: 9305 RVA: 0x00100978 File Offset: 0x000FEB78
	// (set) Token: 0x0600245A RID: 9306 RVA: 0x00100980 File Offset: 0x000FEB80
	public static HoverIcons icon
	{
		get
		{
			return UIHoverIcon.icon_;
		}
		set
		{
			if (UIHoverIcon.icon_ != value)
			{
				for (int i = 0; i < UIHoverIcon.IconParent.transform.childCount; i++)
				{
					GameObject gameObject = UIHoverIcon.IconParent.transform.GetChild(i).gameObject;
					gameObject.SetActive(value.ToString().StartsWith(gameObject.name));
				}
				UIHoverIcon.icon_ = value;
				if (value != HoverIcons.None)
				{
					UIHoverText.UpdatePosition();
				}
			}
		}
	}

	// Token: 0x0600245B RID: 9307 RVA: 0x001009F1 File Offset: 0x000FEBF1
	private void Start()
	{
		UIHoverIcon.IconParent = base.gameObject;
	}

	// Token: 0x04001763 RID: 5987
	private static GameObject IconParent;

	// Token: 0x04001764 RID: 5988
	private static HoverIcons icon_;
}
