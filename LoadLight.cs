using System;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class LoadLight : MonoBehaviour
{
	// Token: 0x0600111D RID: 4381 RVA: 0x00075E6E File Offset: 0x0007406E
	private void Start()
	{
		this.LightComp = base.GetComponent<Light>();
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x00075E7C File Offset: 0x0007407C
	private void FixedUpdate()
	{
		this.LightComp.color = this.ColorLogoRainbow();
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x00075E90 File Offset: 0x00074090
	private Color ColorLogoRainbow()
	{
		if (this.LoadColor.r >= 1f && this.LoadColor.g >= 1f && this.LoadColor.b >= 1f)
		{
			this.bRed = true;
		}
		if (this.bRed)
		{
			this.LoadColor.g = this.LoadColor.g - this.ColorChangeSpeed;
			this.LoadColor.b = this.LoadColor.b - this.ColorChangeSpeed;
			if (this.LoadColor.g <= 0f)
			{
				this.bRed = false;
				this.bYellow = true;
			}
		}
		else if (this.bYellow)
		{
			this.LoadColor.g = this.LoadColor.g + this.ColorChangeSpeed / 2f;
			if (this.LoadColor.g >= 1f)
			{
				this.bYellow = false;
				this.bGreen = true;
			}
		}
		else if (this.bGreen)
		{
			this.LoadColor.r = this.LoadColor.r - this.ColorChangeSpeed;
			if (this.LoadColor.r <= 0f)
			{
				this.bGreen = false;
				this.bBlue = true;
			}
		}
		else if (this.bBlue)
		{
			this.LoadColor.g = this.LoadColor.g - this.ColorChangeSpeed;
			this.LoadColor.b = this.LoadColor.b + this.ColorChangeSpeed;
			if (this.LoadColor.b >= 1f)
			{
				this.bBlue = false;
				this.bPurple = true;
			}
		}
		else if (this.bPurple)
		{
			this.LoadColor.r = this.LoadColor.r + this.ColorChangeSpeed / 2f;
			this.LoadColor.b = this.LoadColor.b - this.ColorChangeSpeed / 2f;
			if (this.LoadColor.b <= 0.5f)
			{
				this.bPurple = false;
				this.bPink = true;
			}
		}
		else if (this.bPink)
		{
			this.LoadColor.r = this.LoadColor.r + this.ColorChangeSpeed / 2f;
			this.LoadColor.b = this.LoadColor.b + this.ColorChangeSpeed / 2f;
			if (this.LoadColor.b >= 1f)
			{
				this.bPink = false;
				this.bWhite = true;
			}
		}
		else if (this.bWhite)
		{
			this.LoadColor.r = this.LoadColor.r + this.ColorChangeSpeed;
			this.LoadColor.g = this.LoadColor.g + this.ColorChangeSpeed;
			this.LoadColor.b = this.LoadColor.b + this.ColorChangeSpeed;
		}
		this.LoadColor.r = Mathf.Clamp(this.LoadColor.r, 0f, 1f);
		this.LoadColor.g = Mathf.Clamp(this.LoadColor.g, 0f, 1f);
		this.LoadColor.b = Mathf.Clamp(this.LoadColor.b, 0f, 1f);
		return this.LoadColor;
	}

	// Token: 0x04000AE6 RID: 2790
	private Color LoadColor = Colour.UnityWhite;

	// Token: 0x04000AE7 RID: 2791
	private bool bRed;

	// Token: 0x04000AE8 RID: 2792
	private bool bYellow;

	// Token: 0x04000AE9 RID: 2793
	private bool bGreen;

	// Token: 0x04000AEA RID: 2794
	private bool bBlue;

	// Token: 0x04000AEB RID: 2795
	private bool bPurple;

	// Token: 0x04000AEC RID: 2796
	private bool bPink;

	// Token: 0x04000AED RID: 2797
	private bool bWhite;

	// Token: 0x04000AEE RID: 2798
	private float ColorChangeSpeed = 0.05f;

	// Token: 0x04000AEF RID: 2799
	private Light LightComp;
}
